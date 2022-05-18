using GameServerCore;
using GameServerCore.Packets.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Linq;
using System.Numerics;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class MobsCommand : ChatCommandBase
    {
        private readonly Game _game;
        private readonly IPlayerManager _playerManager;

        public override string Command => "mobs";
        public override string Syntax => $"{Command} teamNumber";

        public MobsCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _game = game;
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

            if (!int.TryParse(split[1], out var team))
            {
                return;
            }

            var units = Game.ObjectManager.GetObjects()
                .Where(xx => xx.Value.Team == team.ToTeamId())
                .Where(xx => xx.Value is Minion || xx.Value is Monster);

            foreach (var unit in units)
            {
                 _game.PacketNotifier.NotifyS2C_MapPing(unit.Value.Position, Pings.PING_DANGER, client: player);
            }
        }
    }
}
