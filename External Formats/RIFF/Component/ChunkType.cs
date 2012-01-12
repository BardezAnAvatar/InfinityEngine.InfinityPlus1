using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Enumerator descring the RIF chunk type. Enumerator is for known RIFF chunk types</summary>
    /// <remarks>I wrote with hex because the UInt32 represenation is backward from the actual value; if recompiling or a differnt endianness, invert the values</remarks>
    public enum ChunkType : uint /* UInt32 */
    {
        RIFF = 0x46464952U,
        LIST = 0x5453494CU,
        JUNK = 0x4B4E554AU,
        INFO = 0x4F464E49U,
                         
        WAVE = 0x45564157U,
        fmt  = 0x20746D66U,
        data = 0x61746164U,
                         
        AVI  = 0x20495641U,
        hdrl = 0x6C726468U,
        avih = 0x68697661U,
        strl = 0x6C727473U,
        strh = 0x68727473U,
        strf = 0x66727473U,
        strd = 0x64727473U,
        movi = 0x69766F6DU,
        rec  = 0x20636572U,
        idx1 = 0x31786469U
    }
}