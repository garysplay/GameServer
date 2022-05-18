using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class SpawnStateCommand : ChatCommandBase
    {
        public override string Command => "spawnstate";
        public override string Syntax => $"{Command} 0 (disable) / 1 (enable)";
        private IPlayerManager _playerManager;

        public SpawnStateCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');

            if (split.Length < 2 || !byte.TryParse(split[1], out var input) || input > 1)
            {
                ChatCommandManager.SendDebugMsgFormatted(_playerManager.GetPeerInfo(userId), DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                Game.Map.MapScript.MapScriptMetadata.MinionSpawnEnabled = input != 0;
            }
        }
    }
}
