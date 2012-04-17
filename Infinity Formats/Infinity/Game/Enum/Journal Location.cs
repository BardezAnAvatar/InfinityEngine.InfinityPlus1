using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the location where the journal StrRef resides</summary>
    public enum JournalLocation : byte /* Byte */
    {
        /// <summary>StrRef is in Dlialog(f).TLK</summary>
        Talk        = 0xFF,

        /// <summary>StrRef is in *.TOH/*.TOT</summary>
        Supplement  = 0x1F,
    }
}