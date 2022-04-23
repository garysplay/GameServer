using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    internal class CharScriptTT_DummyPusher : ICharScript
    {
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            AddBuff("ResistantSkinDragon", 25000.0f, 1, null, owner, owner, false);
            AddBuff("TreelineLanternLock", 180.0f, 1, null, owner, owner, false);
            SetMinimapIcon(owner, "AltarLocked", true);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
