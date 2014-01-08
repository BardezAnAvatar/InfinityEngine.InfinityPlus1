using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the bits for weather effects</summary>
    [Flags]
    public enum Weather : ushort /* UInt16 */
    {
        None                = 0x00,
        Rain                = 0x01,
        Snow                = 0x02,
        Fog                 = 0x03, //Snow and Rain = Fog ?

        //BG2 code has rain and snow heavy/medium/light flags shared; NI generalizes it more
        WeatherLight        = 0x04,
        WeatherMedium       = 0x08,
        WeatherHeavy        = 0x0C, //Light and Heavy

        WindLight           = 0x10,
        WindMedium          = 0x20,
        WindHeavy           = 0x30, //Light and Heavy

        LightningRare       = 0x40,
        LightningRegular    = 0x80,
        LightningFrequent   = 0xC0, //Mask Light and Heavy lightning

        //Weather getting worse?
        StormIncreasing     = 0x0100,
    }
}