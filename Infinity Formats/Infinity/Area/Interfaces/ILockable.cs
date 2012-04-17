using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Defines an interface to objects that can be locked</summary>
    interface ILockable
    {
        #region Properties
        /// <summary>Item resref which is the key to this object</summary>
        ResourceReference KeyItem { get; set; }

        /// <summary>Difficulty of lock on object</summary>
        Int32 DifficultyLock { get; set; }

        /// <summary>Strref to lockpick string</summary>
        StringReference LockpickText { get; set; }

        /// <summary>Exposes a flag indicating whether or not the object is locked</summary>
        Boolean Locked { get; set; }
        #endregion
    }
}