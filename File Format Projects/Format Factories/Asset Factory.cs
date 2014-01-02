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
                //validation
                if (!input.CanRead)
                    throw new InvalidOperationException("The input stream cannot be read from.");
                else if (!input.CanSeek)
                    throw new InvalidOperationException("The input stream cannot seek, which is required.");

                //determine the type of the asset being built

                //I think that 20 bytes should be sufficient, generally, to determine which type of asset to instantiate.
                //  the exception would be for something like text files only, I think
                Int64 availableSize = 0L;
                availableSize = input.Length - input.Position;

                if (availableSize > (readSize - 1))
                {
                }
                else    //guess based on the extension
                    asset = AssetFactory.BuildAssetFromExtension(input, extensionHint, engineHint);
            }

            return asset;
        }

        /// <summary>Attempts to build an asset instance based on the file extension hint</summary>
        /// <param name="input">Stream to provide</param>
        /// <param name="extensionHint">File extension hinting at type</param>
        /// <param name="engineHint">Type of the game engine that would hint at the type to instantiate</param>
        /// <returns>An instance of the asset format</returns>
        private static Object BuildAssetFromExtension(Stream input, String extensionHint, GameEngine engineHint)
        {
            Object asset = null;

            switch (extensionHint.ToLower())
            {
                case "2da":
                    asset = TwoDimensionalArrayFactory.Build2DA(input);
                    break;

                case "acm":
                    asset = AcmFactory.BuildAcm(input);
                    break;

                case "are":
                    asset = AreaFactory.BuildArea(input, engineHint);
                    break;

                default:
                    asset = null;
                    break;
            }

            return asset;
        }
    }
}