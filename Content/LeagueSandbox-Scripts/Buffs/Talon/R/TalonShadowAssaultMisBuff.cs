using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;

namespace Buffs
{
    class TalonShadowAssaultMisBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion Blade;
        IParticle red;
		IParticle green;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Blade = unit as IMinion;
            IBuff ownerBuff = buff.SourceUnit.GetBuffWithName("TalonShadowAssaultBuff");
            string BLUE;
            string RED;

            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

			switch (Blade.Owner.SkinID)
            {
                case 3:
                    BLUE = "talon_ult_blade_hold_dragon.troy";
					RED = "talon_ult_blade_hold_team_ID_red_dragon.troy";
                    break;

                case 4:
                    BLUE = "talon_Skin04_ult_blade_hold.troy";
					RED = "talon_Skin04_ult_blade_hold_team_ID_red.troy";
                    break;
				case 5:
                    BLUE = "Talon_Skin05_R_Blade_Hold.troy";
					RED = "Talon_Skin05_R_Blade_Hold_enemy.troy";
                    break;
				case 20:
                    BLUE = "Talon_Skin20_R_Blade_Hold.troy";
					RED = "Talon_Skin05_R_Blade_Hold_enemy.troy";
                    break;
				case 29:
                    BLUE = "Talon_Skin29_R_Blade_Hold.troy";
					RED = "Talon_Skin29_R_Blade_Hold_enemy.troy";
                    break;
				case 38:
                    BLUE = "Talon_Skin38_R_Blade_Hold.troy";
					RED = "Talon_Skin29_R_Blade_Hold_enemy.troy";
                    break;

                default:
                    BLUE = "talon_ult_blade_hold.troy";
					RED = "talon_ult_blade_hold_team_ID_red.troy";
                    break;
            }
            
			if (Blade.Owner.Team == TeamId.TEAM_BLUE)
            {
				red = AddParticle(Blade.Owner, null, RED, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_PURPLE);
                green = AddParticle(Blade.Owner, null, BLUE, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_BLUE);
				//green2 = AddParticle(Blade.Owner, null, BLUE2, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_BLUE);
            }
            else
            {
                red = AddParticle(Blade.Owner, null, RED, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_BLUE);
                green = AddParticle(Blade.Owner, null, BLUE, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_PURPLE);
				//green2 = AddParticle(Blade.Owner, null, BLUE2, Blade.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_PURPLE);
            }

            if (ownerBuff != null)
            {
                ApiEventManager.OnBuffDeactivated.AddListener(this, ownerBuff, OnSpellCast, true);
            }
            else
            {
                buff.DeactivateBuff();
            }
            //AddParticleTarget(Blade.Owner, Blade, BLUE, Blade, buff.Duration,1,"C_BuffBone_Glb_Center_Loc");
            //p = AddParticleTarget(Blade.Owner, Blade.Owner, "", Blade, buff.Duration, flags: FXFlags.TargetDirection);
        }

		public void OnSpellCast(IBuff buff)
        {
            //Blade.Owner.RemoveBuffsWithName("TalonShadowAssaultBuff");	
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (Blade != null && !Blade.IsDead)
            {
                if (green != null && red != null)
                {
                    green.SetToRemove();
					red.SetToRemove();
                }
				SpellCast(Blade.Owner, 4, SpellSlotType.ExtraSlots, true, Blade.Owner, Blade.Position);
                Blade.TakeDamage(Blade, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}