using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Defines an interface of common properties for objects that can be trapped</summary>
    interface ITrappable
    {
        #region Properties
        /// <summary>Difficulty to detect a trap on this object</summary>
        Int16 DifficultyTrapDetection { get; set; }

        /// <summary>Difficulty to remove a trap on this object</summary>
        Int16 DifficultyTrapRemoval { get; set; }

        /// <summary>Object is trapped</summary>
        Boolean Trapped { get; set; }

        /// <summary>Indicated whether a trap has been detected</summary>
        Boolean TrapDetected { get; set; }

        /// <summary>Point from which this object's trap (if any) launches</summary>
        Point TrapLaunchLocation { get; set; }
        #endregion
    }
}