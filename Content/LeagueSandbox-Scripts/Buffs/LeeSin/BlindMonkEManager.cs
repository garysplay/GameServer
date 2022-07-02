using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;

namespace Buffs
{
    internal class BlindMonkEManager : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = unit as IObjAiBase;

            if (owner != null)
            {
                var eTwoSpell = owner.SetSpell("BlindMonkETwo", 2, true, true);
                // TODO: Instead of doing this, simply make an API function for SetSpellSlotCooldown
                eTwoSpell.SetCooldown(0, true);
                var qOneBuff = buff.SourceUnit.GetBuffWithName("BlindMonkEOne");
                if (qOneBuff == null)
                {
                    RemoveBuff(buff);
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = unit as IObjAiBase;

            if (owner != null)
            {
                float[] cdByLevel = { 10f, 10f, 10f, 10f, 10f };
                var newCooldown = (MathF.Max(buff.TimeElapsed - 3.0f, 0f) + cdByLevel[ownerSpell.CastInfo.SpellLevel - 1]) * owner.Stats.CooldownReduction.Total;

                var eTwoSpell = owner.SetSpell("BlindMonkEOne", 2, true);
                // TODO: Instead of doing this, simply make an API function for SetSpellSlotCooldown
                eTwoSpell.SetCooldown(newCooldown, true);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}