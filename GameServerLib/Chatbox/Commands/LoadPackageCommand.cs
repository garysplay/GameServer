using System;
using LeagueSandbox.GameServer.Content;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    class LoadPackageCommand : ChatCommandBase
    {
        public override string Command => "loadpackage";
        public override string Syntax => $"{Command} packagename";

        private ContentManager _contentManager;
        private Game _game;
        public LoadPackageCommand(ChatCommandManager chatCommandManager, Game game) : base(chatCommandManager, game)
        {
            _contentManager = game.Config.ContentManager;
            _game = game;
        }

        
        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.Split(' ');
            var player = _game.PlayerManager.GetPeerInfo(userId);

            if (split.Length >= 2)
            {
                string packageName = split[1];

                _contentManager.LoadPackage(packageName);
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, $"Loading content from package: {packageName}");
            }
            else
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
        }
    }
}
