using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Factories
{
    /// <summary>This class is a factory for building an asset manager</summary>
    public static class AssetManagerFactory
    {
        /// <summary>Builds an Asset Manager instance based off of the game directory passed into this method</summary>
        /// <param name="path">Path indicating the directory of the game installation location to be reviewed</param>
        /// <returns>The build asset manager</returns>
        public static IAssetManager BuildAssetManager(String path)
        {
            //first, detect if the path exists, as a directory
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(String.Format("The specified directory (\"{0}\") does not exist.", path));

            IAssetManager manager = null;

            //create some checks on the file system to determine which game we might be dealing with
            String eeDecryptTarget = Path.Combine(path, "decrypt.dll");
            String nwnTarget = Path.Combine(path, "nwn.exe");
            String ieKeymapTarget = Path.Combine(path, "keymap.ini");
            
            //check which install we are dealing with
            if (File.Exists(eeDecryptTarget))   //appears to be an enhanced edition install
            {
                //TODO: create an InfinityEnhancedAssetManager
            }
            else if (File.Exists(nwnTarget)) //Neverwinter Nights
            {
                //TODO: create an NeverwinterAssetManager
            }
            else if (File.Exists(ieKeymapTarget))
            {
                manager = new InfinityAssetManager(path);
            }

            return manager;
        }

        /// <summary>Builds an Asset Manager instance based off of the game directory passed into this method</summary>
        /// <param name="directory">Path indicating the directory of the game installation location to be reviewed</param>
        /// <returns>The build asset manager</returns>
        public static IAssetManager BuildAssetManager(DirectoryInfo directory)
        {
            return AssetManagerFactory.BuildAssetManager(directory.FullName);
        }
    }
}