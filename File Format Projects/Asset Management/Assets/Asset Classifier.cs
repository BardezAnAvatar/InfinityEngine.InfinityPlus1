using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>Looks at an asset's extension and classifies it to ge generalized type</summary>
    public static class AssetClassifier
    {
        /// <summary>Classifies a file extension as a generalized asset type</summary>
        /// <param name="extension">Extension to classify</param>
        /// <returns>The generalized classified asset type</returns>
        public static GeneralizedAssetType Classify(String extension)
        {
            GeneralizedAssetType type = GeneralizedAssetType.Unknown;

            switch (extension.ToLower())
            {
                case "key":
                    type = GeneralizedAssetType.Key;
                    break;
                
                case "tlk":
                case "toh":
                case "tot":
                    type = GeneralizedAssetType.Book;
                    break;

                case "tga":
                case "bam":
                case "bmp":
                case "png":
                case "gif":
                case "jpg":
                case "jpeg":
                case "jfif":
                case "mos":
                case "tis":
                case "plt":
                    type = GeneralizedAssetType.Picture;
                    break;

                case "mpeg":
                case "mpg":
                case "wbm":
                case "mve":
                case "mp4":
                    type = GeneralizedAssetType.Movie;
                    break;

                case "wav":
                case "ogg":
                case "mp3":
                case "bmu":
                case "acm":
                    type = GeneralizedAssetType.Speaker;
                    break;

                case "wfx":
                case "wed":
                case "chu":
                case "itm":
                case "spl":
                case "are":
                case "gam":
                case "eff":
                case "vvc":
                case "var":
                case "vef":
                case "pro":
                case "src":
                    type = GeneralizedAssetType.Structure;
                    break;

                case "bs":
                case "bcs":
                case "baf":
                    type = GeneralizedAssetType.Script;
                    break;

                case "ids":
                case "2da":
                    type = GeneralizedAssetType.Spreadsheet;
                    break;

                case "cre":
                case "chr":
                    type = GeneralizedAssetType.Person;
                    break;

                case "dlg":
                    type = GeneralizedAssetType.Speech;
                    break;

                case "sto":
                    type = GeneralizedAssetType.Store;
                    break;

                case "wmp":
                    type = GeneralizedAssetType.Map;
                    break;

                case "bio":
                case "res":
                case "txt":
                    type = GeneralizedAssetType.Text;
                    break;

                case "bah":
                    type = GeneralizedAssetType.Unknown;
                    break;

                case "ini":
                    type = GeneralizedAssetType.Configuration;
                    break;

                case "mus":
                    type = GeneralizedAssetType.Playlist;
                    break;

                case "sav":
                case "bif":
                case "cbf":
                    type = GeneralizedAssetType.Archive;
                    break;
            }

            return type;
        }
    }
}