using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable
{
    /// <summary>This enum indicates the location of a given BIFF file</summary>
    [Flags]
    public enum KeyTableBifLocationEnum : ushort /* UInt16 */
    {
        MaskAll = UInt16.MaxValue,
        None = 0,       //ease of use flag that indicates that the locations are all unset
        HardDrive = 1,  //this looks like Data at first look, but recall PS:T where BIFs are in the root directory
        Cache = 2,
        Disc1 = 4,
        Disc2 = 8,
        Disc3 = 16,
        Disc4 = 32,
        Disc5 = 64,
        Disc6 = 128,
    }

    /// <summary>Provides extension methods for the AssetLocation enum</summary>
    public static class KeyTableBifLocationEnumExtender
    {
        /// <summary>Gets the short name for the asset location</summary>
        /// <param name="asset">The asset to get the short name for</param>
        /// <returns>The short name of the asset</returns>
        public static String GetShortName(this KeyTableBifLocationEnum location)
        {
            String shortName = null;

            switch (location)
            {
                case KeyTableBifLocationEnum.HardDrive:
                    shortName = "HD0";
                    break;

                case KeyTableBifLocationEnum.Cache:
                    shortName = "cache";
                    break;

                case KeyTableBifLocationEnum.Disc1:
                    shortName = "CD1";
                    break;

                case KeyTableBifLocationEnum.Disc2:
                    shortName = "CD2";
                    break;

                case KeyTableBifLocationEnum.Disc3:
                    shortName = "CD3";
                    break;

                case KeyTableBifLocationEnum.Disc4:
                    shortName = "CD4";
                    break;

                case KeyTableBifLocationEnum.Disc5:
                    shortName = "CD5";
                    break;

                case KeyTableBifLocationEnum.Disc6:
                    shortName = "CD6";
                    break;
            }

            return shortName;
        }
    }

    /// <summary>Indicates the location of BIFF Strings within the KEY file</summary>
    public enum ChitinKeyBifStringsLocation
    {
        BeforeBifEntries,
        BetweenBifAndResource,
        TrailResource,
    }
}