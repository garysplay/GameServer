﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameServerCore;
using LeagueSandbox.GameServer.Content;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    class PrintPackageListCommand : ChatCommandBase
    {
        public override string Command => "showpackages";
        public override string Syntax => $"{Command}";

        private ContentManager _contentManager;
        private IPlayerManager _playerManager;
        public PrintPackageListCommand(ChatCommandManager chatCommandManager, Game game) : base(chatCommandManager, game)
        {
            _contentManager = game.Config.ContentManager;
            _playerManager = game.PlayerManager;
        }

        
        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            List<Package> packageList = _contentManager.GetAllLoadedPackages();

            string printedString = "Loaded packages:\n";

            foreach (Package dataPackage in packageList)
            {
                printedString += $"- {dataPackage.PackageName}\n";
            }

            ChatCommandManager.SendDebugMsgFormatted(_playerManager.GetPeerInfo(userId), DebugMsgType.INFO, printedString);

        }
    }
}
