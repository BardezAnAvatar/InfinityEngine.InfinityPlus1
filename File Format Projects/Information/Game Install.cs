using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Information
{
    /// <summary>This enumeration describes the game install variants known</summary>
    public enum GameInstall
    {
        /// <summary>Unknown</summary>
        Unknown,

        /// <summary>Baldur's Gate</summary>
        [Description("Baldur's Gate")]
        BaldursGate,

        /// <summary>Baldur's Gate Abridged, Chapters 1 & 2</summary>
        [Description("Baldur's Gate Abridged, Chapters 1 & 2")]
        BaldursGateDemo,

        /// <summary>Baldur's Gate: Tales of the Sword Coast</summary>
        [Description("Baldur's Gate: Tales of the Sword Coast")]
        TotSC,

        /// <summary>Planescape: Torment</summary>
        [Description("Planescape: Torment")]
        PlanescapeTorment,

        /// <summary>Icewind Dale</summary>
        [Description("Icewind Dale")]
        IcewindDale,

        /// <summary>Icewind Dale: Heart of Winter</summary>
        [Description("Icewind Dale: Heart of Winter")]
        HeartOfWinter,

        /// <summary>Icewind Dale: Trials of the Luremaster</summary>
        [Description("Icewind Dale: Trials of the Luremaster")]
        TrialsOfTheLuremaster,

        /// <summary>Baldur's Gate 2</summary>
        [Description("Baldur's Gate 2")]
        ShadowsOfAmn,

        /// <summary>Baldur's Gate 2 Demo</summary>
        [Description("Baldur's Gate 2 Demo")]
        BaldursGate2Demo,

        /// <summary>Baldur's Gate 2: Throne of Bhaal</summary>
        [Description("Baldur's Gate 2: Throne of Bhaal")]
        ThroneOfBhaal,

        /// <summary>Baldur's Gate Trilogy</summary>
        [Description("Baldur's Gate Trilogy")]
        BaldursGateTrilogy,

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

        /// <summary>Neverwinter Nights: Shadows Of Undrentide</summary>
        [Description("Neverwinter Nights: Shadows Of Undrentide")]
        ShadowsOfUndrentide,

        /// <summary>Neverwinter Nights: Hordes of the Underdark</summary>
        [Description("Neverwinter Nights: Hordes of the Underdark")]
        HordesOfTheUnderdark,

        /// <summary>Neverwinter Nights: Kingmaker</summary>
        [Description("Neverwinter Nights: Kingmaker")]
        Kingmaker,
    }
}