using GameServerCore.Enums;

namespace GameServerCore.Packets.PacketDefinitions.Requests
{
    public class ChatMessageRequest : ICoreRequest
    {
        public int ClientID { get; }
        public string Message { get; }
        public ChatType ChatType { get; }

        public ChatMessageRequest(string message, ChatType type, int clientId)
        {
            Message = message;
            ChatType = type;
            ClientID = clientId;
        }
    }
}
