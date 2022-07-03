using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class TalonShadowAssaultBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff ThisBuff;
        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;

            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonRake"), OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonCutthroat"), OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonBasicAttack"), OnSpellCast);

            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Talon_Base_R_Cas_Invis.troy", unit, 2.5f);
            AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "talon_ult_sound.troy", ownerSpell.CastInfo.Owner);

            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
            unit.AddStatModifier(StatsModifier);

            if (unit is IObjAIBase owner)
            {
                var r2Spell = owner.SetSpell("TalonShadowAssaultToggle", 3, true);
            }
        }

        public void OnSpellCast(ISpell spell)
        {
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            PlaySound("Play_vo_Talon_TalonShadowAssaultBuff_OnBuffDeactivate", unit);
            var spell = (unit as IObjAIBase).SetSpell("TalonShadowAssault", 3, true);
            spell.SetCooldown(spell.GetCooldown() - buff.TimeElapsed);
        }
    }
}