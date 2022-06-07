using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System;
using System.Collections.Generic;

namespace LeagueSandbox.GameServer.GameObjects.Stats
{
    public class ReplicationHero : Replication
    {
        /*readonly List<short> SpellSlotsToReplicate = new List<short>
        {
            0, 0, 0, 0, 0, 1, 2, 3, 42, 43, 44, 45, 46, 47, 48, 49, 52, 53
        };*/
        public ReplicationHero(Champion owner) : base(owner)
        {
        }
        public override void Update()
        {
            //EXP
            UpdateFloat(Stats.Experience, 0, 0);

            //Gold
            UpdateFloat(Stats.Gold, 0, 1);

            //mCanCastBitsLo
            //Unlocks Q and W ?
            //UpdateUint((uint)Stats.ActionState, 0, 2);

            //nCanCastBitsHi
            //Unlocks SummonerSpells?
            UpdateUint((uint)Stats.ActionState, 0, 3);

            UpdateFloat(384, 0, 17);
            /*for (int i = 4; i <= 17; i++)
            {
                var test = SpellSlotsToReplicate[i];
                if ((Owner as Champion).Spells[test] != null)
                {
                    UpdateFloat((Owner as Champion).Spells[SpellSlotsToReplicate[i]].CastInfo.ManaCost, 0, i);
                }
            }*/

            //Base AP
            UpdateFloat(Stats.AbilityPower.BaseValue, 1, 7);

            //Base AD(?)
            UpdateFloat(Stats.AttackDamage.BaseValue, 1, 6);

            //TODO: Implement dodge
            //Dodge
            //UpdateFloat(Stats.Dodge.Total, 1, 8);

            //Crit Chance
            UpdateFloat(Stats.CriticalChance.Total, 1, 9);

            //Base Armor
            UpdateFloat(Stats.Armor.BaseValue, 1, 10);

            //Something replated to MR
            UpdateFloat(Stats.MagicResist.BaseValue, 1, 11);

            //Hp regen per 5 sec
            UpdateFloat(Stats.HealthRegeneration.Total, 1, 12);

            //Mana regen per 5 sec
            UpdateFloat(Stats.ManaRegeneration.Total, 1, 13);

            //Range
            UpdateFloat(Stats.Range.Total, 1, 14);

            //Bonus Ad(?)
            UpdateFloat(Stats.AttackDamage.FlatBonus, 1, 15);

            //Bonus AP
            UpdateFloat(Stats.AbilityPower.FlatBonus, 1, 17);

            //Bonus AttackSpeed(?)
            UpdateFloat(1, 1, 20);

            //CDR
            UpdateFloat(Stats.CooldownReduction.Total, 1, 22);

            //Flat Armor Pen
            UpdateFloat(Stats.ArmorPenetration.Total, 1, 23);

            //% Armor Pen
            //UpdateFloat(Stats.SpellVamp.Total, 1, 24);

            //Flat Magic Pen
            UpdateFloat(Stats.MagicPenetration.Total, 1, 25);

            //% Magic Pen
            //UpdateFloat(Stats.SpellVamp.Total, 1, 26);

            //Life Steal
            UpdateFloat(Stats.LifeSteal.Total, 1, 27);

            //Spell Vamp
            UpdateFloat(Stats.SpellVamp.Total, 1, 28);

            //Current Health
            UpdateFloat(Stats.CurrentHealth, 3, 0);

            //Current Mana
            UpdateFloat(Stats.CurrentMana, 3, 1);

            //Max/Base Health
            UpdateFloat(Stats.HealthPoints.Total, 3, 2);

            //Max/Base Mana
            UpdateFloat(Stats.ManaPoints.Total, 3, 3);

            //Something related to vision/fog of war
            UpdateFloat(0f, 3, 5);

            //Bonus MovSpeed
            UpdateFloat(Stats.MoveSpeed.Total, 3, 6);

            //Size
            UpdateFloat(Stats.Size.Total, 3, 7);

            //Level
            UpdateInt(Stats.Level, 3, 8);

            UpdateInt((Owner as Champion).MinionCounter, 3, 9);
        }
    }
}
