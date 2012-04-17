using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the flags on a BG2-style journal entry</summary>
    public enum JournalEntry : byte /* Byte */
    {
        [Description("User-written entry")]
        UserEntry       = 0,

        [Description("Uncompleted Quest")]
        Quest           = 1,
        
        [Description("Completed Quest")]
        CompletedQuest  = 2,
        
        [Description("Informational journal entry")]
        Journal         = 4,
    }
}