using System.Numerics;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

namespace Spells
{
    public class LucianW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
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
            var current = spell.CastInfo.Owner.Position;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var to = Vector2.Normalize(spellPos - current);
            var range = to * 900;
            var trueCoords = current + range;

            //spell.AddProjectile("LucianWMissile", current, current, trueCoords);
        }

        public void ApplyEffects(IObjAIBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
        {
            float damage = 20 + (spell.CastInfo.SpellLevel * 40) + owner.Stats.AbilityPower.Total * 0.9f;
            target.TakeDamage(owner, damage, GameServerCore.Enums.DamageType.DAMAGE_TYPE_MAGICAL, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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
