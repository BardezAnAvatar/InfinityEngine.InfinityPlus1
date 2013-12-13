using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TwoDimensionalArray._2DA1;
using Bardez.Projects.InfinityPlus1.Information;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>This factory is a builder of the various format objects</summary>
    public static class AssetFactory
    {
        /// <summary>Builds a format instance from the input stream</summary>
        /// <param name="input">Stream to instantiate a format from</param>
        /// <param name="extensionHint">Extension type of the asset, for hinting at the type</param>
        /// <param name="engineHint">Type of the game engine that would hint at the type to instantiate</param>
        /// <returns>An instance of the format</returns>
        public static Object BuildAsset(Stream input, String extensionHint, GameEngine engineHint)
        {
            Object asset = null;

            //Define how much data we assume to need to read
            Int32 readSize = 20;

            if (input == null)
                throw new ArgumentNullException("input", "The input Stream was unexpectedly null.");
            lock (input)
            {
                //check if can seek
                if (!input.CanSeek)
                    throw new InvalidOperationException("This method needs to be able to seek within the Stream being read from.");

                //determine the type of the asset being built

                //I think that 20 bytes should be sufficient, generally, to determine which type of asset to instantiate.
                //  the exception would be for something like text files only, I think
                Int64 availableSize = 0L;
                availableSize = input.Length - input.Position;

                if (availableSize > (readSize - 1))
                {
                }
                else    //guess based on the extension
                    asset = AssetFactory.BuildAssetFromExtension(input, extensionHint);
            }

            return asset;
        }

        /// <summary>Attempts to build an asset instance based on the file extension hint</summary>
        /// <param name="input">Stream to provide</param>
        /// <param name="extensionHint">File extension hinting at type</param>
        /// <returns>An instance of the asset format</returns>
        private static Object BuildAssetFromExtension(Stream input, String extensionHint)
        {
            Object asset = null;

            switch (extensionHint.ToLower())
            {
                case "2da":
                    TwoDimensionalArray1 twoda = new TwoDimensionalArray1();
                    twoda.Initialize();
                    twoda.Read(input, true);
                    asset = twoda;
                    break;

                case "are":

                default:
                    asset = null;
                    break;
            }

            return asset;
        }
    }
}