using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ManaCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "mana";
        public override string Syntax => $"{Command} maxMana";

        public ManaCommand(ChatCommandManager chatCommandManager, Game game)
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
            }
            else if (float.TryParse(split[1], out var mp))
            {
                player.Champion.Stats.ManaPoints.FlatBonus += mp;
                player.Champion.Stats.CurrentMana += mp;
            }
        }
    }
}
