using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>This enumerator describes the possible locations of an asset</summary>
    [Flags]
    public enum AssetLocation : int /* Int32 */
    {
        /// <summary>The directory in which the executable, key file, *.ini reside</summary>
        [Description("Executable directory")]
        ApplicationDirectory = 1,

        /// <summary>The INI hard drive location</summary>
        HD0 = 2,

        /// <summary>The INI first CD location</summary>
        CD1 = 4,

        /// <summary>The INI second CD location</summary>
        CD2 = 8,

        /// <summary>The INI third CD location</summary>
        CD3 = 16,

        /// <summary>The INI fourth CD location</summary>
        CD4 = 32,

        /// <summary>The INI fifth CD location</summary>
        CD5 = 64,

        /// <summary>The INI sixth CD location</summary>
        CD6 = 128,

        /// <summary>The user directory location</summary>
        [Description("User directory")]
        UserDirectory = 256,

        /// <summary>Value that indicates that there is no specific location used to look up an asset</summary>
        [Description("No Directory Specified")]
        LookUp = 0
    }

    /// <summary>Provides extension methods for the AssetLocation enum</summary>
    public static class AssetLocationExtender
    {
        /// <summary>Gets the short name for the asset location</summary>
        /// <param name="location">The asset to get the short name for</param>
        /// <returns>The short name of the asset</returns>
        public static String GetShortName(this AssetLocation location)
        {
            String shortName = null;

            switch (location)
            {
                case AssetLocation.ApplicationDirectory:
                    shortName = "App";
                    break;

                case AssetLocation.UserDirectory:
                    shortName = "User";
                    break;

                case AssetLocation.HD0:
                case AssetLocation.CD1:
                case AssetLocation.CD2:
                case AssetLocation.CD3:
                case AssetLocation.CD4:
                case AssetLocation.CD5:
                case AssetLocation.CD6:
                    shortName = location.ToString();
                    break;
            }

            return shortName;
        }
    }
}