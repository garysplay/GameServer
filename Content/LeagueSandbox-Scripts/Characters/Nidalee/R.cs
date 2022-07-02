using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class AspectOfTheCougar : ISpellScript
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
            var owner = spell.CastInfo.Owner;
			if (owner.Model == "Nidalee")
            {
                owner.ChangeModel("Nidalee_Cougar");
				//owner.SetAutoAttackSpell("Nidalee_CougarBasicAttack2", false);
				(owner as IObjAIBase).SetSpell("Takedown", 0, true);
				(owner as IObjAIBase).SetSpell("Pounce", 1, true);
				(owner as IObjAIBase).SetSpell("Swipe", 2, true);
				//(owner as IObjAIBase).SetSpell("AspectOfTheCougar", 3, true);
            }
			else
            {		
                owner.ChangeModel("Nidalee");
				//owner.SetAutoAttackSpell("NidaleeBasicAttack2", false);
				(owner as IObjAIBase).SetSpell("JavelinToss", 0, true);
				(owner as IObjAIBase).SetSpell("Bushwhack", 1, true);
				(owner as IObjAIBase).SetSpell("PrimalSurge", 2, true);
				//(owner as IObjAIBase).SetSpell("AspectOfTheCougar", 3, true);
            }
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