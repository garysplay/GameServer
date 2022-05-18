using GameServerCore;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Content;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ChCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "ch";
        public override string Syntax => $"{Command} championName";

        public ChCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.Split(' ');
            var player = _playerManager.GetPeerInfo(userId);
            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(player, DebugMsgType.SYNTAXERROR);
                ShowSyntax();
                return;
            }
            var currentChampion = _playerManager.GetPeerInfo(userId).Champion;

            var c = new Champion(
                Game,
                split[1],
                (uint)player.PlayerId,
                0, // Doesnt matter at this point
                currentChampion.RuneList,
                currentChampion.TalentInventory,
                player,
                currentChampion.NetId,
                player.Champion.Team
            );
            c.SetPosition(
                player.Champion.Position
            );

            c.ChangeModel(split[1]); // trigger the "modelUpdate" proc
            c.SetTeam(player.Champion.Team);
            Game.ObjectManager.RemoveObject(player.Champion);
            Game.ObjectManager.AddObject(c);
            player.Champion = c;
        }
    }
}
