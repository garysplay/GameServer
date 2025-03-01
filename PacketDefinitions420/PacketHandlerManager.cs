﻿using LENet;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Packets.Handlers;
using GameServerCore.Packets.PacketDefinitions;
using LeaguePackets;
using LeaguePackets.Game.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Channel = GameServerCore.Packets.Enums.Channel;
using GameServerCore.NetInfo;

namespace PacketDefinitions420
{
    /// <summary>
    /// Class containing all functions related to sending and receiving packets.
    /// TODO: refactor this class (may be able to replace it with LeaguePackets' implementation), get rid of IGame, use generic API requests+responses also for disconnect and unpause
    /// </summary>
    public class PacketHandlerManager : IPacketHandlerManager
    {
        private delegate ICoreRequest RequestConvertor(byte[] data);
        private readonly Dictionary<Tuple<GamePacketID, Channel>, RequestConvertor> _gameConvertorTable;
        private readonly Dictionary<LoadScreenPacketID, RequestConvertor> _loadScreenConvertorTable;
        // should be one-to-one, no two users for the same Peer
        private readonly Peer[] _peers;
        private readonly IPlayerManager _playerManager;
        private readonly BlowFish[] _blowfishes;
        private readonly Host _server;
        private readonly IGame _game;

        private readonly NetworkHandler<ICoreRequest> _netReq;
        private readonly NetworkHandler<ICoreRequest> _netResp;

        public PacketHandlerManager(BlowFish[] blowfishes, Host server, IGame game, NetworkHandler<ICoreRequest> netReq, NetworkHandler<ICoreRequest> netResp)
        {
            _blowfishes = blowfishes;
            _server = server;
            _game = game;
            _peers = new Peer[blowfishes.Length];
            _playerManager = _game.PlayerManager;
            _netReq = netReq;
            _netResp = netResp;
            _gameConvertorTable = new Dictionary<Tuple<GamePacketID, Channel>, RequestConvertor>();
            _loadScreenConvertorTable = new Dictionary<LoadScreenPacketID, RequestConvertor>();
            InitializePacketConvertors();
        }

        internal void InitializePacketConvertors()
        {
            foreach(var m in typeof(PacketReader).GetMethods())
            {
                foreach (Attribute attr in m.GetCustomAttributes(true))
                {
                    if (attr is PacketType)
                    {
                        if (((PacketType)attr).ChannelId == Channel.CHL_LOADING_SCREEN || ((PacketType)attr).ChannelId == Channel.CHL_COMMUNICATION)
                        {
                            var method = (RequestConvertor)Delegate.CreateDelegate(typeof(RequestConvertor), m);
                            _loadScreenConvertorTable.Add(((PacketType)attr).LoadScreenPacketId, method);
                        }
                        else
                        {
                            var key = new Tuple<GamePacketID, Channel>(((PacketType)attr).GamePacketId, ((PacketType)attr).ChannelId);
                            var method = (RequestConvertor)Delegate.CreateDelegate(typeof(RequestConvertor), m);
                            _gameConvertorTable.Add(key, method);
                        }
                    }
                }
            }
        }

        private RequestConvertor GetConvertor(LoadScreenPacketID packetId)
        {
            var packetsHandledWhilePaused = new List<LoadScreenPacketID>
            {
                LoadScreenPacketID.RequestJoinTeam,
                LoadScreenPacketID.Chat
            };

            if (_game.IsPaused && !packetsHandledWhilePaused.Contains(packetId))
            {
                return null;
            }

            if (_loadScreenConvertorTable.ContainsKey(packetId))
            {
                return _loadScreenConvertorTable[packetId];
            }

            return null;

        }

        private RequestConvertor GetConvertor(GamePacketID packetId, Channel channelId)
        {
            var packetsHandledWhilePaused = new List<GamePacketID>
            {
                GamePacketID.Dummy,
                GamePacketID.SynchSimTimeC2S,
                GamePacketID.ResumePacket,
                GamePacketID.C2S_QueryStatusReq,
                GamePacketID.C2S_ClientReady,
                GamePacketID.C2S_Exit,
                GamePacketID.World_SendGameNumber,
                GamePacketID.SendSelectedObjID,
                GamePacketID.C2S_CharSelected,

                // The next two are required to reconnect 
                GamePacketID.SynchVersionC2S,
                GamePacketID.C2S_Ping_Load_Info,

                // The next 5 are not really needed when reconnecting,
                // but they don't do much harm either
                GamePacketID.C2S_UpdateGameOptions,
                GamePacketID.OnReplication_Acc,
                GamePacketID.C2S_StatsUpdateReq,
                GamePacketID.World_SendCamera_Server,
                GamePacketID.C2S_OnTipEvent
            };
            if (_game.IsPaused && !packetsHandledWhilePaused.Contains(packetId))
            {
                return null;
            }
            var key = new Tuple<GamePacketID, Channel>(packetId, channelId);
            if (_gameConvertorTable.ContainsKey(key))
            {
                return _gameConvertorTable[key];
            }

            return null;
        }

        private void PrintPacket(byte[] buffer, string str)
        {
            // FIXME: currently lock disabled, not needed?
            Console.Write(str);
            foreach (var b in buffer)
            {
                Console.Write(b.ToString("X2") + " ");
            }

            Console.WriteLine("");
            Console.WriteLine("--------");
        }

        public bool SendPacket(int userId, byte[] source, Channel channelNo, PacketFlags flag = PacketFlags.RELIABLE)
        {
            // Sometimes we try to send packets to a user that doesn't exist (like in broadcast when not all players are connected).
            if (0 <= userId && userId < _peers.Length && _peers[userId] != null)
            {
                byte[] temp;
                if (source.Length >= 8)
                {
                    // _peers.Length == _blowfishes.Length
                    temp = _blowfishes[userId].Encrypt(source);
                }
                else
                {
                    temp = source;
                }
                return _peers[userId].Send((byte)channelNo, new LENet.Packet(temp, flag)) == 0;
            }
            return false;
        }

        public bool BroadcastPacket(byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.RELIABLE)
        {
            if (data.Length >= 8)
            {
                // send packet to all peers and save failed ones
                int failedPeers = 0;
                for(int i = 0; i < _peers.Length; i++)
                {
                    if(_peers[i] != null && _peers[i].Send((byte)channelNo, new LENet.Packet(_blowfishes[i].Encrypt(data), flag)) < 0)
                    {
                        failedPeers++;
                    }
                }

                if (failedPeers > 0)
                {
                    Debug.WriteLine($"Broadcasting packet failed for {failedPeers} peers.");
                    return false;
                }
                return true;
            }
            else
            {
                var packet = new LENet.Packet(data, flag);
                _server.Broadcast((byte)channelNo, packet);
                return true;
            }
        }

        // TODO: find a way with no need of player manager
        public bool BroadcastPacketTeam(TeamId team, byte[] data, Channel channelNo,
            PacketFlags flag = PacketFlags.RELIABLE)
        {
            foreach (var ci in _playerManager.GetPlayers(false))
            {
                if (ci.Team == team)
                {
                    SendPacket(ci.ClientId, data, channelNo, flag);
                }
            }

            return true;
        }

        public bool BroadcastPacketVision(IGameObject o, byte[] data, Channel channelNo,
            PacketFlags flag = PacketFlags.RELIABLE)
        {
            foreach (int pid in o.VisibleForPlayers)
            {
                SendPacket(pid, data, channelNo, flag);
            }
            return true;
        }

        public bool HandlePacket(Peer peer, byte[] data, Channel channelId)
        {
            var reader = new BinaryReader(new MemoryStream(data));
            RequestConvertor convertor;

            if (channelId == Channel.CHL_COMMUNICATION || channelId == Channel.CHL_LOADING_SCREEN)
            {
                var loadScreenPacketId = (LoadScreenPacketID)reader.ReadByte();
                //Console.WriteLine($"-> {loadScreenPacketId}");
                convertor = GetConvertor(loadScreenPacketId);
            }
            else
            {
                var gamePacketId = (GamePacketID)reader.ReadByte();
                //Console.WriteLine($"-> {gamePacketId}");
                convertor = GetConvertor(gamePacketId, channelId);
            }

            reader.Close();

            if (convertor != null)
            {
                int clientId = ((int)peer.UserData) - 1;
                dynamic req = convertor(data);
                _netReq.OnMessage(clientId, req);
                return true;
            }

            #if DEBUG
            PrintPacket(data, "Error: ");
            #endif

            return false;
        }

        public bool HandleDisconnect(Peer peer)
        {
            if(peer == null)
            {
                return true;
            }

            int clientId = ((int)peer.UserData) - 1;
            if(clientId < 0)
            {
                // Didn't receive an ID by initiating a handshake.
                return true;
            }
            return HandleDisconnect(clientId);
        }

        public bool HandleDisconnect(int clientId)
        {
            var peerInfo = _game.PlayerManager.GetPeerInfo(clientId);
            if (peerInfo.IsDisconnected)
            {
                Debug.WriteLine($"Prevented double disconnect of {peerInfo.PlayerId}");
                return true;
            }

            Debug.WriteLine($"Player {peerInfo.PlayerId} disconnected!");
            
            var annoucement = new OnLeave { OtherNetID = peerInfo.Champion.NetId };
            _game.PacketNotifier.NotifyS2C_OnEventWorld(annoucement, peerInfo.Champion);
            peerInfo.IsDisconnected = true;
            peerInfo.IsStartedClient = false;
            _peers[clientId] = null;

            return _game.CheckIfAllPlayersLeft() || peerInfo.Champion.OnDisconnect();
        }

        public bool HandlePacket(Peer peer, Packet packet, Channel channelId)
        {
            var data = packet.Data;

            // if channel id is HANDSHAKE we should initialize blowfish key and return
            if(channelId == Channel.CHL_HANDSHAKE)
            {
                return HandleHandshake(peer, data);
            }

            // every packet that is not blowfish go here
            if (data.Length >= 8)
            {
                int clientId = ((int)peer.UserData) - 1;
                data = _blowfishes[clientId].Decrypt(data);
            }

            return HandlePacket(peer, data, channelId);
        }

        private bool HandleHandshake(Peer peer, byte[] data)
        {
            var request = PacketReader.ReadKeyCheckRequest(data);

            var peerInfo = _playerManager.GetClientInfoByPlayerId(request.PlayerID);
            if (peerInfo == null)
            {
                Debug.WriteLine($"Player ID {request.PlayerID} is invalid.");
                return false;
            }

            if(_peers[peerInfo.ClientId] != null && !peerInfo.IsDisconnected)
            {
                Debug.WriteLine($"Player {request.PlayerID} is already connected. Request from {peer.Address.IPEndPoint.Address.ToString()}.");
                return false;
            }

            long playerID = _blowfishes[peerInfo.ClientId].Decrypt(request.CheckSum);
            if(request.PlayerID != playerID)
            {
                Debug.WriteLine($"Blowfish key is wrong!");
                return false;
            }

            peerInfo.IsStartedClient = true;

            Debug.WriteLine("Connected client No " + peerInfo.ClientId);      

            peer.UserData = (int)peerInfo.ClientId + 1;
            _peers[peerInfo.ClientId] = peer;

            bool result = true;
            // inform players about their player numbers
            foreach (var player in _playerManager.GetPlayers(false))
            {
                var response = new KeyCheckPacket
                {
                    ClientID = player.ClientId,
                    PlayerID = player.PlayerId,
                    VersionNumber = request.VersionNo,
                    Action = 0,
                    CheckSum = request.CheckSum
                };
                result = result && SendPacket(peerInfo.ClientId, response.GetBytes(), Channel.CHL_HANDSHAKE);
            }

            // only if all packets were sent successfully return true
            return result;
        }
    }
}
