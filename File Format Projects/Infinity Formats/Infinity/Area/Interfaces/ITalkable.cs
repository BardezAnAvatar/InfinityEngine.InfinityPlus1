using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Defines a common interface for objects that are non-actors that are talkable (doors, regions, etc.)</summary>
    interface ITalkable
    {
        #region Properties
        /// <summary>Strref of (displayed) name of the object during dialogs</summary>
        StringReference DisplayName { get; set; }

        /// <summary>Associated dialog file</summary>
        ResourceReference Dialog { get; set; }
        #endregion
    }
}