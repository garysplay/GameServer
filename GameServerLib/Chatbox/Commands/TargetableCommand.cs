using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class TargettableCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;
        public override string Command => "targetable";
        public override string Syntax => $"{Command} false (untargetable) / true (targetable)";

        public TargettableCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            var player = _playerManager.GetPeerInfo(userId);

            if (split.Length != 2 || !bool.TryParse(split[1], out var userInput))
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                player.Champion.SetStatus(GameServerCore.Enums.StatusFlags.Targetable, userInput);
            }
        }
    }
}