using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ChangeTeamCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "changeteam";
        public override string Syntax => $"{Command} teamNumber";

        public ChangeTeamCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            var player = _playerManager.GetPeerInfo(userId);
            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.SYNTAXERROR);
                ShowSyntax();
                return;
            }

            if (!int.TryParse(split[1], out var t))
            {
                return;
            }

            var team = t.ToTeamId();
            player.Champion.SetTeam(team);
        }
    }
}
