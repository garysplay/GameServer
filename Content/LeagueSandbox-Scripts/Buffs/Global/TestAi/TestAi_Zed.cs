using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class TestAi_Zed : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IChampion Taunter)
            {
			FaceDirection(ownerSpell.CastInfo.Owner.Position, Taunter);
			//Taunter.SetTargetUnit(ownerSpell.CastInfo.Owner, true);
            //Taunter.UpdateMoveOrder(OrderType.AttackTo, true);
			SpellCast(Taunter, 3, SpellSlotType.SpellSlots, false, ownerSpell.CastInfo.Owner, Vector2.Zero);
			//SpellCast(Taunter, 0, SpellSlotType.SpellSlots, false, Taunter, Vector2.Zero);
			CreateTimer((float) 1.25 , () =>
            {
				if (Taunter.Spells[1].SpellName == "ZedShadowDash")
              {
		        SpellCast(Taunter, 1, SpellSlotType.SpellSlots, ownerSpell.CastInfo.Owner.Position, Vector2.Zero, false, Taunter.Position);
			  }
			//SpellCast(Taunter, 1, SpellSlotType.SpellSlots, ownerSpell.CastInfo.Owner.Position, Vector2.Zero, false, Taunter.Position);
		    //SpellCast(Taunter, 1, SpellSlotType.SpellSlots, false, Taunter, Vector2.Zero);
			});
			CreateTimer((float) 1.75 , () =>
            {
		    SpellCast(Taunter, 0, SpellSlotType.SpellSlots, ownerSpell.CastInfo.Owner.Position, Vector2.Zero, false, Taunter.Position);
			});
			CreateTimer((float) 2 , () =>
            {
		    SpellCast(Taunter, 2, SpellSlotType.SpellSlots, false, Taunter, Vector2.Zero);
			});
			CreateTimer((float) 3.5 , () =>
            {
			  if (Taunter.Spells[1].SpellName == "ZedW2")
              {
		        SpellCast(Taunter, 1, SpellSlotType.SpellSlots, false, Taunter, Vector2.Zero);
			  }
			});
			CreateTimer((float) 5.5 , () =>
            {
			  if (Taunter.Spells[3].SpellName == "ZedR2")
              {
		        SpellCast(Taunter, 3, SpellSlotType.SpellSlots, false, Taunter, Vector2.Zero);
			  }
			});
			}
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LOC_Taunt", unit, buff.Duration, bone: "head");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IChampion Taunter)
            {
			Taunter.SetTargetUnit(null, true);
			}
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}