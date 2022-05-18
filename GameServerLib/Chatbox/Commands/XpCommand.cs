﻿using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class XpCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "xp";
        public override string Syntax => $"{Command} xp";

        public XpCommand(ChatCommandManager chatCommandManager, Game game)
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

            if (float.TryParse(split[1], out var xp))
            {
                player.Champion.AddExperience(xp);
            }
        }
    }
}
