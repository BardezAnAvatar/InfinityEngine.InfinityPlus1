using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Defines an interface for objects that are interactable and have a hovering cursor</summary>
    interface ICursor
    {
        #region PRoperties
        /// <summary>Index of the cursor to display for this object</summary>
        /// <remarks>Matches to CURSORS.BAM</remarks>
        Int32 CursorIndex { get; set; }
        #endregion
    }
}