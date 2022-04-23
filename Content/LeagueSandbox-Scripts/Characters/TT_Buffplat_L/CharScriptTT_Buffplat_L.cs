using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    internal class CharScriptTT_Buffplat_L : ICharScript
    {
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            //I believe the issues regarding all other units when buff plates are spawned are related to the plate's Replication.
            //Further investigation is necessary
            AddBuff("TreelineLanternPostLockNeutral", 25000.0f, 1, null, owner, owner, false);
            AddBuff("TreelineLanternLock", 180.0f, 1, null, owner, owner, false);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
