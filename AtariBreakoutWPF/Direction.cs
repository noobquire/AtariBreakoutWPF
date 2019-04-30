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
        Up,
        Down,
        Left,
        Right,
        Horizontal = Up | Down,
        Vertical = Left | Right,
        Default
    }
}
