using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

namespace Spells
{
    public class SummonerFlash : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = false,
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
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
            var current = owner.Position;
            var dist = Vector2.Distance(current, start);

            FaceDirection(start, owner, true);

            if (dist > spell.SpellData.CastRangeDisplayOverride)
            {
                start = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);
            }

            StopChanneling(owner, ChannelingStopCondition.Cancel, ChannelingStopSource.Move);

            AddParticle(owner, null, "global_ss_flash", owner.Position);
            AddParticleTarget(owner, owner, "global_ss_flash_02", owner);

            TeleportTo(owner, start.X, start.Y);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
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

