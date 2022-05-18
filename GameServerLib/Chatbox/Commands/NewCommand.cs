namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class NewCommand : ChatCommandBase
    {
        public override string Command => "newcommand";
        public override string Syntax => $"{Command}";
        private Game _game;
        public NewCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _game = game;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var msg = $"The new command added by {ChatCommandManager.CommandStarterCharacter}help has been executed";
            ChatCommandManager.SendDebugMsgFormatted(_game.PlayerManager.GetPeerInfo(userId), DebugMsgType.INFO, msg);
            ChatCommandManager.RemoveCommand(Command);
        }
    }
}
