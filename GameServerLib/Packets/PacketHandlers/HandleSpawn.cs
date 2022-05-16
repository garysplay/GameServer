﻿using GameServerCore.Packets.PacketDefinitions.Requests;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Packets.Handlers;
using LeagueSandbox.GameServer.Inventory;
using LeagueSandbox.GameServer.Logging;
using log4net;

namespace LeagueSandbox.GameServer.Packets.PacketHandlers
{
    public class HandleSpawn : PacketHandlerBase<SpawnRequest>
    {
        private readonly ILog _logger;
        private readonly Game _game;
        private readonly ItemManager _itemManager;
        private readonly IPlayerManager _playerManager;
        private readonly NetworkIdManager _networkIdManager;

        public HandleSpawn(Game game)
        {
            _logger = LoggerProvider.GetLogger();
            _game = game;
            _itemManager = game.ItemManager;
            _playerManager = game.PlayerManager;
            _networkIdManager = game.NetworkIdManager;
        }

        public override bool HandlePacket(int userId, SpawnRequest req)
        {
            _logger.Debug("Spawning map");
            _game.PacketNotifier.NotifyS2C_StartSpawn(userId);

            var userInfo = _playerManager.GetPeerInfo(userId);
            var om = _game.ObjectManager as ObjectManager;
            if (_game.IsRunning)
            {
                om.OnReconnect(userId, userInfo.Team);
            }
            else
            {
                om.SpawnObjects(userInfo);
            }

            _game.PacketNotifier.NotifySpawnEnd(userId);
            //_game.PacketNotifier.NotifyEnterVisibilityClient(userInfo.Champion, userId, true, true);
            return true;
        }
    }
}