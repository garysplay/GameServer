﻿using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;
using System.Numerics;

namespace LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI
{
    public class Minion : ObjAIBase, IMinion
    {
        /// <summary>
        /// Unit which spawned this minion.
        /// </summary>
        public IObjAIBase Owner { get; }
        /// <summary>
        /// Whether or not this minion should ignore collisions.
        /// </summary>
        public bool IgnoresCollision { get; protected set; }
        /// <summary>
        /// Whether or not this minion is considered a ward.
        /// </summary>
        public bool IsWard { get; protected set; }
        /// <summary>
        /// Whether or not this minion is a LaneMinion.
        /// </summary>
        public bool IsLaneMinion { get; protected set; }
        /// <summary>
        /// Whether or not this minion is targetable at all.
        /// </summary>
        public bool IsTargetable { get; protected set; }
        /// <summary>
        /// Internal name of the minion.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Only unit which is allowed to see this minion.
        /// </summary>
        public IObjAIBase VisibilityOwner { get; }

        //TODO: Implement these variables
        public int DamageBonus { get; protected set; }
        public int HealthBonus { get; protected set; }
        public int InitialLevel { get; protected set; }

        public Minion(
            Game game,
            IObjAIBase owner,
            Vector2 position,
            string model,
            string name,
            uint netId = 0,
            TeamId team = TeamId.TEAM_NEUTRAL,
            int skinId = 0,
            bool ignoreCollision = false,
            bool targetable = true,
            bool isWard = false,
            IObjAIBase visibilityOwner = null,
            IStats stats = null,
            string AIScript = "",
            int damageBonus = 0,
            int healthBonus = 0,
            int initialLevel = 1
        ) : base(game, model, 40, position, 1100, skinId, netId, team, stats, AIScript)
        {
            Name = name;
            Owner = owner;

            IsLaneMinion = false;
            IsWard = isWard;
            IgnoresCollision = ignoreCollision;
            if (IgnoresCollision)
            {
                SetStatus(StatusFlags.Ghosted, true);
            }

            IsTargetable = targetable;
            if (!IsTargetable)
            {
                SetStatus(StatusFlags.Targetable, false);
            }

            VisibilityOwner = visibilityOwner;
            DamageBonus = damageBonus;
            HealthBonus = healthBonus;
            InitialLevel = initialLevel;
            MoveOrder = OrderType.Stop;

            Replication = new ReplicationMinion(this);
        }
    }
}