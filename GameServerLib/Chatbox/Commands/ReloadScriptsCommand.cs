using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ReloadScriptsCommand : ChatCommandBase
    {
        public override string Command => "reloadscripts";
        public override string Syntax => $"{Command}";
        private IPlayerManager _playerManager;
        public ReloadScriptsCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            ChatCommandManager.SendDebugMsgFormatted(_playerManager.GetPeerInfo(userId), DebugMsgType.INFO,
                Game.LoadScripts() ? "Scripts reloaded." : "Scripts failed to reload.");
        }
    }
}
