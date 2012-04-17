using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags indicating the schedule during which actors appear in the area</summary>
    [Flags]
    public enum Schedule : uint /* UInt32 */
    {
        /// <summary>Between 00:30 and 01:30</summary>
        [Description("Between 00:30 and 01:30")]
        Time_00_30_to_01_30 = 0x00000001U,

        /// <summary>Between 01:30 and 02:30</summary>
        [Description("Between 01:30 and 02:30")]
        Time_01_30_to_02_30 = 0x00000002U,

        /// <summary>Between 02:30 and 03:30</summary>
        [Description("Between 02:30 and 03:30")]
        Time_02_30_to_03_30 = 0x00000004U,

        /// <summary>Between 03:30 and 04:30</summary>
        [Description("Between 03:30 and 04:30")]
        Time_03_30_to_04_30 = 0x00000008U,

        /// <summary>Between 04:30 and 05:30</summary>
        [Description("Between 04:30 and 05:30")]
        Time_04_30_to_05_30 = 0x00000010U,

        /// <summary>Between 05:30 and 06:30</summary>
        [Description("Between 05:30 and 06:30")]
        Time_05_30_to_06_30 = 0x00000020U,

        /// <summary>Between 06:30 and 07:30</summary>
        [Description("Between 06:30 and 07:30")]
        Time_06_30_to_07_30 = 0x00000040U,

        /// <summary>Between 07:30 and 08:30</summary>
        [Description("Between 07:30 and 08:30")]
        Time_07_30_to_08_30 = 0x00000080U,

        /// <summary>Between 08:30 and 09:30</summary>
        [Description("Between 08:30 and 09:30")]
        Time_08_30_to_09_30 = 0x00000100U,

        /// <summary>Between 09:30 and 10:30</summary>
        [Description("Between 09:30 and 10:30")]
        Time_09_30_to_10_30 = 0x00000200U,

        /// <summary>Between 10:30 and 11:30</summary>
        [Description("Between 10:30 and 11:30")]
        Time_10_30_to_11_30 = 0x00000400U,

        /// <summary>Between 11:30 and 12:30</summary>
        [Description("Between 11:30 and 12:30")]
        Time_11_30_to_12_30 = 0x00000800U,

        /// <summary>Between 12:30 and 13:30</summary>
        [Description("Between 12:30 and 13:30")]
        Time_12_30_to_13_30 = 0x00001000U,

        /// <summary>Between 13:30 and 14:30</summary>
        [Description("Between 13:30 and 14:30")]
        Time_13_30_to_14_30 = 0x00002000U,

        /// <summary>Between 14:30 and 15:30</summary>
        [Description("Between 14:30 and 15:30")]
        Time_14_30_to_15_30 = 0x00004000U,

        /// <summary>Between 15:30 and 16:30</summary>
        [Description("Between 15:30 and 16:30")]
        Time_15_30_to_16_30 = 0x00008000U,

        /// <summary>Between 16:30 and 17:30</summary>
        [Description("Between 16:30 and 17:30")]
        Time_16_30_to_17_30 = 0x00010000U,

        /// <summary>Between 17:30 and 18:30</summary>
        [Description("Between 17:30 and 18:30")]
        Time_17_30_to_18_30 = 0x00020000U,

        /// <summary>Between 18:30 and 19:30</summary>
        [Description("Between 18:30 and 19:30")]
        Time_18_30_to_19_30 = 0x00040000U,

        /// <summary>Between 19:30 and 20:30</summary>
        [Description("Between 19:30 and 20:30")]
        Time_19_30_to_20_30 = 0x00080000U,

        /// <summary>Between 20:30 and 21:30</summary>
        [Description("Between 20:30 and 21:30")]
        Time_20_30_to_21_30 = 0x00100000U,

        /// <summary>Between 21:30 and 22:30</summary>
        [Description("Between 21:30 and 22:30")]
        Time_21_30_to_22_30 = 0x00200000U,

        /// <summary>Between 22:30 and 23:30</summary>
        [Description("Between 22:30 and 23:30")]
        Time_22_30_to_23_30 = 0x00400000U,

        /// <summary>Between 23:30 and 00:30</summary>
        [Description("Between 23:30 and 00:30")]
        Time_23_30_to_00_30 = 0x00800000U,
    }
}