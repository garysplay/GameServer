using System;

namespace GameServerCore.Enums
{
    [Flags]
    public enum ActionState : uint
    {
        CAN_ATTACK = 1,
        CAN_CAST = 2,
        CAN_MOVE = 4,
        ALLSTATES = 7
    }
}