using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum
{
    /// <summary>Set of flags indicating which streams the audio samples will belong do</summary>
    [Flags]
    public enum AudioStreamChannels : ushort /* UInt16 */
    {
        /// <summary>Channel # 0</summary>
        [Description("Channel # 0")]
        Channel00 = 0x0001,
        /// <summary>Channel # 1</summary>
        [Description("Channel # 1")]
        Channel01 = 0x0002,
        /// <summary>Channel # 2</summary>
        [Description("Channel # 2")]
        Channel02 = 0x0004,
        /// <summary>Channel # 3</summary>
        [Description("Channel # 3")]
        Channel03 = 0x0008,
        /// <summary>Channel # 4</summary>
        [Description("Channel # 4")]
        Channel04 = 0x0010,
        /// <summary>Channel # 5</summary>
        [Description("Channel # 5")]
        Channel05 = 0x0020,
        /// <summary>Channel # 6</summary>
        [Description("Channel # 6")]
        Channel06 = 0x0040,
        /// <summary>Channel # 7</summary>
        [Description("Channel # 7")]
        Channel07 = 0x0080,
        /// <summary>Channel # 8</summary>
        [Description("Channel # 8")]
        Channel08 = 0x0100,
        /// <summary>Channel # 9</summary>
        [Description("Channel # 9")]
        Channel09 = 0x0200,
        /// <summary>Channel # 10</summary>
        [Description("Channel # 10")]
        Channel10 = 0x0400,
        /// <summary>Channel # 11</summary>
        [Description("Channel # 11")]
        Channel11 = 0x0800,
        /// <summary>Channel # 12</summary>
        [Description("Channel # 12")]
        Channel12 = 0x1000,
        /// <summary>Channel # 13</summary>
        [Description("Channel # 13")]
        Channel13 = 0x2000,
        /// <summary>Channel # 14</summary>
        [Description("Channel # 14")]
        Channel14 = 0x4000,
        /// <summary>Channel # 15</summary>
        [Description("Channel # 15")]
        Channel15 = 0x8000,
    }
}