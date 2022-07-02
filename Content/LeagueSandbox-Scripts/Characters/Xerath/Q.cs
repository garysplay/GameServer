using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class XerathArcanopulseChargeUp: ISpellScript
    {
        public static IAttackableUnit Target = null;
		ISpell Spell;
		IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			ChannelDuration = 4f,
            TriggersSpellCasts = true,
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Spell = spell;
			Owner = owner;
			Target = target;		
            owner.CancelAutoAttack(false, true);
            owner.SetTargetUnit(null, true);			
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			//spell.SetCooldown(0.5f, true);		      
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        { 
        }

        public void OnSpellChannel(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            //owner.SetSpell("XerathLocusPulse", 3, true);	
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
			var owner = spell.CastInfo.Owner;
			var targetPos = GetPointFromUnit(owner, 1050.0f);
            //FaceDirection(targetPos, owner);
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
			//owner.SetSpell("XerathLocusOfPower2", 3, true);
        }

		public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }

    }
}