using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Vertical = Left | Right,
    }
}
