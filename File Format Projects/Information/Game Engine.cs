using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Information
{
    /// <summary>This enumeration describes the Infinity Engine variants known</summary>
    public enum GameEngine
    {
        /// <summary>Baldur's Gate</summary>
        [Description("Baldur's Gate")]
        BaldursGate,

        /// <summary>Planescape: Torment</summary>
        [Description("Planescape: Torment")]
        PlanescapeTorment,

        /// <summary>Icewind Dale</summary>
        [Description("Icewind Dale")]
        IcewindDale,

        /// <summary>Baldur's Gate 2</summary>
        [Description("Baldur's Gate 2")]
        BaldursGate2,

        /// <summary>Icewind Dale 2</summary>
        [Description("Icewind Dale 2")]
        IcewindDale2,

        /// <summary>Baldur's Gate: Enhanced Edition</summary>
        [Description("Baldur's Gate: Enhanced Edition")]
        BaldursGateEnhancedEdition,

        /// <summary>Baldur's Gate 2: Enhanced Edition</summary>
        [Description("Baldur's Gate 2: Enhanced Edition")]
        BaldursGate2EnhancedEdition,

        /// <summary>Neverwinter Nights</summary>
        [Description("Neverwinter Nights")]
        NeverwinterNights,
    }
}