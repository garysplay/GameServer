using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using static GameServerCore.Domain.GameObjects.IGameObject;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Spells
{
    public class TalonShadowAssault : ISpellScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            PlayAnimation(owner, "Spell4");
            AddBuff("TalonShadowAssaultBuff", 2.5f, 1, spell, owner, owner, false);
            for (int bladeCount = 0; bladeCount <= 7; bladeCount++)
            {
                var start = GetPointFromUnit(owner, 25f, bladeCount * 45f);
                var end = GetPointFromUnit(owner, 615f, bladeCount * 45f);
                SpellCast(owner, 3, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                SpellCast(owner, 5, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
            }
            AddParticle(owner, null, "Talon_Base_R_Cas.troy", owner.Position, 10f);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            spell.SetCooldown(0.25f, true);
        }
    }

    public class TalonShadowAssaultMisOne : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.75f;
            var damage = 120 + 50f * (owner.GetSpell(3).CastInfo.SpellLevel - 1) + ADratio;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
            var damageamp = 0.03f * ELevel;
            if (target.HasBuff("TalonDamageAmp"))
            {
                damage = damage + damage * damageamp;
            }
            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
                if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
                {
                    UnitsHit.Add(target);
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(owner, target, "talon_ult_tar.troy", target, 1f);
                }
            }
        }
    }
    public class TalonShadowAssaultMisOneHalf : ISpellScript
    {
        ISpell spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            //In League, they don't summon a minion.
            IMinion Blade = AddMinion(owner, "TestCube", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
            AddBuff("TalonShadowAssaultMisBuff", 25000.0f, 1, spell, Blade, owner, false);
        }
    }
    public class TalonShadowAssaultMisTwo : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
                missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.75f;
            var damage = 120 + 50f * (owner.GetSpell(3).CastInfo.SpellLevel - 1) + ADratio;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
            var damageamp = 0.03f * ELevel;
            if (target.HasBuff("TalonDamageAmp"))
            {
                damage = damage + damage * damageamp;
            }

            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
                if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
                {
                    UnitsHit.Add(target);
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(owner, target, "talon_ult_tar.troy", target, 1f);
                }
            }
        }
    }
    public class TalonShadowAssaultToggle : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            IBuff buff = owner.GetBuffWithName("TalonShadowAssaultBuff");
            if (buff != null)
            {
                buff.DeactivateBuff();
            }
        }
    }
}

