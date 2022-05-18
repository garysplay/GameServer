using GameServerCore;
using System;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class HelpCommand : ChatCommandBase
    {
        private const string COMMAND_PREFIX = "<font color=\"#E175FF\"><b>";
        private const string COMMAND_SUFFIX = "</b></font>, ";
        private readonly int MESSAGE_MAX_SIZE = 512;

        public override string Command => "help";
        public override string Syntax => $"{Command}";

        private IPlayerManager _playerManager;
        public HelpCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var player = _playerManager.GetPeerInfo(userId);

            if (!Game.Config.ChatCheatsEnabled)
            {
                var msg = "[LS] Chat commands are disabled in this game.";
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, msg);
                return;
            }

            var commands = ChatCommandManager.GetCommandsStrings();
            var commandsString = "";
            var lastCommandString = "";
            var isNewMessage = false;

            ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, "List of available commands: ");

            foreach (var command in commands)
            {
                if(isNewMessage)
                {
                    commandsString = new String(lastCommandString);
                    isNewMessage = false;
                }

                lastCommandString = $"{COMMAND_PREFIX}" +
                $"{ChatCommandManager.CommandStarterCharacter}{command}" +
                $"{COMMAND_SUFFIX}";

                if(commandsString.Length + lastCommandString.Length >= MESSAGE_MAX_SIZE)
                {
                    ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.NORMAL, commandsString);
                    commandsString = "";
                    isNewMessage = true;
                }
                else
                {
                    commandsString = $"{commandsString}{lastCommandString}";
                }
            }

            if (commandsString.Length != 0)
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.NORMAL, commandsString);
            }

            ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.INFO, "There are " + commands.Count + " commands");
        }
    }
}
