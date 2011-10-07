using System;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
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

    public enum ChitinKeyBifStringsLocation
    {
        BeforeBifEntries,
        BetweenBifAndResource,
        TrailResource
    }
}