using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.ChitinKey
{
    /// <summary>This enum indicates the location of a given BIFF file</summary>
    [Flags]
    public enum ChitinKeyBifLocationEnum : ushort /* UInt16 */
    {
        Data = 1,
        Cache = 2,
        Disc1 = 4,
        Disc2 = 8,
        Disc3 = 16,
        Disc4 = 32,
        Disc5 = 64,
        Disc6 = 128
    }

    /// <summary>Indicates the location of BIFF Strings withing the KEY file</summary>
    public enum ChitinKeyBifStringsLocation
    {
        BeforeBifEntries,
        BetweenBifAndResource,
        TrailResource
    }
}