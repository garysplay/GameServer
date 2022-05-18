using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Spell;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class HotReloadCommand : ChatCommandBase
    {
        public override string Command => "hotreload";
        public override string Syntax => $"{Command} 0 (disable) / 1 (enable)";
        Game _game;
        public HotReloadCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _game = game;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            var player = _game.PlayerManager.GetPeerInfo(userId);
            if (split.Length < 2 || !byte.TryParse(split[1], out byte input) || input > 1)
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                if (input == 1)
                {
                    Game.EnableHotReload(true, player);
                    ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, "Scripts hot reload enabled.");
                }
                else
                {
                    Game.EnableHotReload(false, player);
                    ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, "Scripts hot reload disabled.");
                }

            }
        }
    }
}
