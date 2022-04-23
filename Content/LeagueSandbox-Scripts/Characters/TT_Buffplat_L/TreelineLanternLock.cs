using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;

namespace Buffs
{
    internal class TreelineLanternLock : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public IStatsModifier StatsModifier { get; private set; }

        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            SetMinimapIcon(unit, "AltarLocked", true);
            AddParticleTarget(unit, null, "TT_Lock_Neutral_L", unit, buff.Duration, direction: new Vector3(-0.83027464f,0.0f, -0.5573545f) ,flags: (FXFlags)48);
            AttachFlexParticle(unit, 0, 0, 3);
            SetMinimapIcon(unit, "AltarInitial", true);
            unit.FaceDirection(new Vector3(-339.6001f, -12.0f, -505.0f));
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        float timer = 0.0f;
        public void OnUpdate(float diff)
        {
            timer -= diff;
            if(timer <= 0 && Unit != null)
            {
                OverrideAnimation(Unit, "LOCKLOOP1", "IDLE1");
                OverrideAnimation(Unit, "LockLoop1", "IDLE1");

                timer = 250.0f;
            }
        }
    }
}