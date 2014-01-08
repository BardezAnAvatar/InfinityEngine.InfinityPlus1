using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums
{
    /// <summary>Enumeration of valid PVR channel data types</summary>
    public enum ChannelType : uint /* UInt32 */
    {
        /// <summary>Unsigned Byte Normalised</summary>
        [Description("Unsigned Byte Normalised")]
        UnsignedByteNormalised      = 0,

        /// <summary>Signed Byte Normalised</summary>
        [Description("Signed Byte Normalised")]
        SignedByteNormalised        = 1,

        /// <summary>Unsigned Byte</summary>
        [Description("Unsigned Byte")]
        UnsignedByte                = 2,

        /// <summary>Signed Byte</summary>
        [Description("Signed Byte")]
        SignedByte                  = 3,

        /// <summary>Unsigned Short Normalised</summary>
        [Description("Unsigned Short Normalised")]
        UnsignedShortNormalised     = 4,

        /// <summary>Signed Short Normalised</summary>
        [Description("Signed Short Normalised")]
        SignedShortNormalised       = 5,

        /// <summary>Unsigned Short</summary>
        [Description("Unsigned Short")]
        UnsignedShort               = 6,

        /// <summary>Signed Short</summary>
        [Description("Signed Short")]
        SignedShort                 = 7,

        /// <summary>Unsigned Integer Normalised</summary>
        [Description("Unsigned Integer Normalised")]
        UnsignedIntegerNormalised   = 8,

        /// <summary>Signed Integer Normalised</summary>
        [Description("Signed Integer Normalised")]
        SignedIntegerNormalised     = 9,

        /// <summary>Unsigned Integer</summary>
        [Description("Unsigned Integer")]
        UnsignedInteger             = 10,

        /// <summary>Signed Integer</summary>
        [Description("Signed Integer")]
        SignedInteger               = 11,

        Float                       = 12,
    }
}