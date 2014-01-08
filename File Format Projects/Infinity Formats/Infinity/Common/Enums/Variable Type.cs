using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    /// <summary>Enumeration of variable types</summary>
    public enum VariableType : ushort /* UInt16 */
    {
        Integer             = 0,
        FloatingPoint       = 1,
        ScriptName          = 2,
        ResourceReference   = 3,
        StringReference     = 4,
        DWORD               = 5,    //how is this different from integer?
    }
}