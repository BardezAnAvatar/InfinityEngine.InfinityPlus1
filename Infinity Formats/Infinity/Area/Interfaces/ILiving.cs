using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Defines common properties to actors, spawns, etc.</summary>
    interface ILiving
    {
        #region Properties
        /// <summary>Time, in seconds, that the creature survives</summary>
        Int32 Lifespan { get; set; }

        /// <summary>Appears to restrict movement from current location</summary>
        Int16 MovementRestrictionDistance { get; set; }

        /// <summary>Appears to restrict movement from current location, and MoveToObject</summary>
        Int16 MovementRestrictionDistanceToObject { get; set; }
        #endregion
    }
}