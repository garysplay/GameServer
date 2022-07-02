﻿using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using System;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class GalioResoluteSmite : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public float finaldamage;
        public Vector2 castcoords;
        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            var units = GetUnitsInRange(missile.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = owner.Stats.AbilityPower.Total * 0.65f;
						var QLevel = owner.GetSpell(0).CastInfo.SpellLevel;
						var damage = 85 + (40 * (QLevel - 1)) + AP;	
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                    }
                }     
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {      
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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
