using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class AkaliShadowDance : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var current = owner.Position;
            var to = Vector2.Normalize(target.Position - current);
            var range = to * 800;

            var trueCoords = current + range;

            //TODO: Dash to the correct location (in front of the enemy IChampion) instead of far behind or inside them
            ForceMovement(owner, target, "Spell4", 2200, 0, 0, 0, 20000);
            //ForceMovement(spell.CastInfo.Owner, "Spell4", trueCoords, 2200, 0, 0, 0);
            AddParticleTarget(owner, target, "akali_shadowDance_tar", target);
        }

        public void ApplyEffects(IObjAIBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
        {
            var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 200 + spell.CastInfo.SpellLevel * 150 + bonusAd + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
