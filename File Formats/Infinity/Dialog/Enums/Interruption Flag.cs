using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Enums
{
    /// <summary>This enum represents the flags available to dialog headers</summary>
    /// <remarks>Any of these bits, apparently, sets the dialog to be interruptable</remarks>
    [Flags]
    public enum InterruptionFlagAction : uint /* UInt32 */
    {
        Enemy           = 1U,
        
        [Description("Escape Area")]
        EscapeArea      = 2U,

        /// <remarks>Not sure, but IESDP suggests this</remarks>
        [Description("Nothing, but similar to enemy")]
        Nothing         = 4U
    }
}