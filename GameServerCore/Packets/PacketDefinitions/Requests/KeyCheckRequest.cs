namespace GameServerCore.Packets.PacketDefinitions.Requests
{
    public class KeyCheckRequest : ICoreRequest
    {
        public byte[] PartialKey { get; } = new byte[3];
        public int ClientID { get; }
        public long PlayerID { get; }
        public ulong EncryptedPlayerId { get; }
        public ulong CheckSum { get; }

        public KeyCheckRequest(long playerNo, int clientId, ulong checkSum, ulong encryptedPlayerId)
        {
            PlayerID = playerNo;
            ClientID = clientId;
            EncryptedPlayerId = encryptedPlayerId;
            CheckSum = checkSum;
        }
    }
}
