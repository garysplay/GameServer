﻿using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Domain;
using System.Collections.Generic;

namespace Spells
{
    public class AscRelicCaptureChannel : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            IsDamagingSpell = true,
            ChannelDuration = 7.0f
        };

        ISpell Spell;
        IAttackableUnit Target;
        IObjAIBase Owner;
        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = owner;
        }

        public void OnDeactivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AscRelicCaptureChannel", 30.0f, 1, spell, owner, owner);

            PlayAnimation(owner, "channel_windup", flags: (AnimationFlags)2);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, true);
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
            var target = spell.CastInfo.Targets[0].Unit;
            if (target.HasBuff("AscRelicSuppression"))
            {
                var buffs = spell.CastInfo.Targets[0].Unit.GetBuffsWithName("AscRelicSuppression");
                var ownerBuff = buffs.Find(x => x.SourceUnit == spell.CastInfo.Owner);

                if (ownerBuff != null)
                {
                    ownerBuff.DeactivateBuff();
                }
            }

            AddParticleTarget(Owner, Owner, "OdinCaptureCancel", Owner, 1, 1, "spine", "spine");
            AddParticleTarget(Owner, Owner, "ezreal_essenceflux_tar", Owner, 1, 1, "ROOT");

            StopChannel();
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            if (spell.CastInfo.Targets[0].Unit != null)
            {
                var crystal = spell.CastInfo.Targets[0].Unit;
                //I Suspect that the condition that actually kills the crystal is it running out of mana, have to investigate further.
                crystal.Die(CreateDeathData(false, 0, crystal, crystal, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));

                if (spell.CastInfo.Owner is IChampion ch)
                {
                    ch.IncrementScore(3.0f, ScoreCategory.Objective, ScoreEvent.MajorRelicPickup, true, true);
                }
            }

            StopChannel();
        }

        public void StopChannel()
        {
            List<IBuff> buffs = new List<IBuff>
            {
                Target.GetBuffWithName("AscRelicSuppression"),
                Target.GetBuffWithName("OdinBombSuppressionOrder"),
                Target.GetBuffWithName("OdinBombSuppressionChaos"),
                Spell.CastInfo.Owner.GetBuffWithName("AscRelicCaptureChannel"),
                Spell.CastInfo.Owner.GetBuffWithName("OdinChannelVision")
            };

            foreach(var buff in buffs)
            {
                if (buff != null)
                {
                    buff.DeactivateBuff();
                }
            }

            Owner.StopAnimation("", true, true);
            ApiEventManager.OnTakeDamage.RemoveListener(this);
            Spell.SetCooldown(4.0f, true);
        }

        public void OnTakeDamage(IDamageData damageData)
        {
            Spell.StopChanneling(ChannelingStopCondition.Cancel, ChannelingStopSource.Attack);
        }


        public void OnUpdate(float diff)
        {
        }
    }
}

