using System;

namespace AtariBreakoutWPF
{
    [Flags]
    public enum Direction
    {
        Default = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        Horizontal = Up | Down,
        Vertical = Left | Right
    }
}