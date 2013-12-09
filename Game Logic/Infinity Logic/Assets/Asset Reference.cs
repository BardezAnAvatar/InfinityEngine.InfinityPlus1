using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>This class contains information used to reference an asset and display it</summary>
    public class AssetReference
    {
        #region Fields
        /// <summary>This is the name of the asset, as it should be displayed (filename and its extension)</summary>
        /// <remarks>
        ///     If the file extension is not known, it should display the resource type code in hexidecimal.
        ///     If there is not ex
        /// </remarks>
        /// <value>
        ///     Sample suggestions:
        ///         Extension:      "spwi240.spl"
        ///         Unknown:        "spwi240 [0x03EE]"
        ///         No extension:   "spwi240"
        /// </value>
        protected readonly String assetName;

        /// <summary>Flag indicating whether the type is known or not. Pre-computed to save processing cycles.</summary>
        protected readonly Boolean knownAssetType;

        /// <summary>This is the type of the asset, as it should be grouped.</summary>
        /// <value>
        ///     Sample suggestions:
        ///         "SPL", "ITM", "STO", "BAM", "SQL"
        ///         "[0x03EE]" (unknown)
        ///         null (none)
        /// </value>
        protected readonly String assetType;

        /// <summary>
        ///     Description of the asset location. This will be the full path to the resource, but
        ///     not necessarily the path within the file system.
        /// </summary>
        /// <value>
        ///     Sample values:
        ///         "[HD0]\music\bd1.mus"
        ///         "[App]\override\cdmb2a9.BAM"
        ///         "[User]\bpsave\000000000-Auto-Save\BALDUR.BMP"
        ///         "[App]\chitin.key\[CD1, CD2]\data\AREA2000.BIF\ar2000.tis"
        ///         "[CD4]\data\AREA0700.BIF" (or would it be "[App]\chitin.key\[CD4]\data\AREA0700.BIF")?
        /// </value>
        protected readonly String assetPathDescription;

        /// <summary>The type of resource for the described asset</summary>
        protected readonly ResourceType resourceType;

        /// <summary>Information on how to locate this specific asset, if so desired</summary>
        protected readonly AssetLocator locator;
        #endregion


        #region Properties
        /// <summary>The name of the resource</summary>
        public String AssetName { get { return this.assetName; } }

        /// <summary>The type/extension of the asset</summary>
        public String AssetType
        {
            get
            {
                String extension = this.assetType;

                if (extension == null)
                {
                    if (this.resourceType != ResourceType.Invalid)
                        extension = String.Format("0x{0}", ((Int16)this.resourceType).ToString("X"));
                    else
                        extension = "Invalid";
                }

                return extension;
            }
        }

        /// <summary>Flag indicating whether the type is known or not. Pre-computed to save processing cycles.</summary>
        public Boolean AssetTypeKnown { get { return this.knownAssetType; } }

        /// <summary>
        ///     Description of the asset location. This will be the full path to the resource, but
        ///     not necessarily the path within the file system.
        /// </summary>
        public String AssetPathDescription { get { return this.assetPathDescription; } }

        /// <summary>Information on how to locate this specific asset, if so desired</summary>
        public AssetLocator Locator { get { return this.locator; } }
        #endregion


        #region Construction
        /// <summary>Key file source definition constructor</summary>
        /// <param name="keyName">Name of the key file that this resource is referencing/derived from</param>
        /// <param name="resource">The resource entry within a key file that this asset is referencing</param>
        /// <param name="containingArchive">The archive entry for which this asset is referencing</param>
        public AssetReference(String keyName, ChitinKeyResourceEntry resource, ChitinKeyBifEntry containingArchive)
        {
            this.assetType = resource.ResourceType.ToFileExtension();

            if (this.assetType == null)
                this.assetName = resource.ResourceName.Value;
            else
                this.assetName = String.Format("{0}.{1}", resource.ResourceName.Value, this.assetType);
            
            this.resourceType = resource.ResourceType;

            // The example: [App]\chitin.key\[HD0, CD1]\Data\Misc.bif\MISC45.itm
            this.assetPathDescription = String.Format(@"[App]\{0}\{1}\{2}\{3}", keyName, this.GetLocationString(containingArchive.BifLocationFlags), containingArchive.BifFileName.Value, this.assetName);

            this.knownAssetType = this.IsKnownExtension(this.assetType);

            this.locator = new AssetLocator(keyName, resource);
        }

        /// <summary>File system source definition constructor</summary>
        /// <param name="location">Location on disk at which this asset exists</param>
        /// <param name="relativePath">Relative path from the location at which the asset resides</param>
        public AssetReference(AssetLocation location, String relativePath)
        {
            this.assetType = Path.GetExtension(relativePath).ToLower();

            if (this.assetType.StartsWith("."))
                this.assetType = this.assetType.Substring(1);
            else if (this.assetName == String.Empty)    //i.e.: "readme" GPL file found in override
                this.assetType = null;

            this.assetName = Path.GetFileName(relativePath);

            //lookup resource type
            this.resourceType = ResourceTypeExtender.FromExtension(this.assetType);

            // The example: [App]\chitin.key\[HD0, CD1]\Data\Misc.bif\MISC45.itm
            this.assetPathDescription = String.Format(@"{0}\{1}", this.GetLocationString(location), relativePath);
            
            this.knownAssetType = this.IsKnownExtension(this.assetType);

            this.locator = new AssetLocator(location, relativePath, this.resourceType);
        }
        #endregion


        #region Helper Methods
        /// <summary>Gets a string describing the specified locations for a displayable file path</summary>
        /// <param name="locations">The locations of a BIF archive</param>
        /// <returns>The descriptive locations of a BIF archive</returns>
        protected String GetLocationString(ChitinKeyBifLocationEnum locations)
        {
            List<String> keyLocations = new List<String>();

            if ((locations & ChitinKeyBifLocationEnum.HardDrive) == ChitinKeyBifLocationEnum.HardDrive)
                keyLocations.Add("HD0");
            if ((locations & ChitinKeyBifLocationEnum.Disc1) == ChitinKeyBifLocationEnum.Disc1)
                keyLocations.Add("CD1");
            if ((locations & ChitinKeyBifLocationEnum.Disc2) == ChitinKeyBifLocationEnum.Disc2)
                keyLocations.Add("CD2");
            if ((locations & ChitinKeyBifLocationEnum.Disc3) == ChitinKeyBifLocationEnum.Disc3)
                keyLocations.Add("CD3");
            if ((locations & ChitinKeyBifLocationEnum.Disc4) == ChitinKeyBifLocationEnum.Disc4)
                keyLocations.Add("CD4");
            if ((locations & ChitinKeyBifLocationEnum.Disc5) == ChitinKeyBifLocationEnum.Disc5)
                keyLocations.Add("CD5");
            if ((locations & ChitinKeyBifLocationEnum.Disc6) == ChitinKeyBifLocationEnum.Disc6)
                keyLocations.Add("CD6");

            StringBuilder builder = new StringBuilder();
            builder.Append("[");

            if (keyLocations.Count == 0)
                builder.Append("No location");
            else
            {
                builder.Append(keyLocations[0]);

                for (Int32 index = 1; index < keyLocations.Count; ++index)
                {
                    builder.Append("; ");
                    builder.Append(keyLocations[index]);
                }
            }

            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>Gets a string describing the specified locations for a displayable file path</summary>
        /// <param name="locations">The active locations specified</param>
        /// <returns>The descriptive string representation of the specified locations</returns>
        protected String GetLocationString(AssetLocation locations)
        {
            List<String> keyLocations = new List<String>();

            if ((locations & AssetLocation.ApplicationDirectory) == AssetLocation.ApplicationDirectory)
                keyLocations.Add("App");
            if ((locations & AssetLocation.HD0) == AssetLocation.HD0)
                keyLocations.Add("HD0");
            if ((locations & AssetLocation.UserDirectory) == AssetLocation.UserDirectory)
                keyLocations.Add("User");
            if ((locations & AssetLocation.CD1) == AssetLocation.CD1)
                keyLocations.Add("CD1");
            if ((locations & AssetLocation.CD2) == AssetLocation.CD2)
                keyLocations.Add("CD2");
            if ((locations & AssetLocation.CD3) == AssetLocation.CD3)
                keyLocations.Add("CD3");
            if ((locations & AssetLocation.CD4) == AssetLocation.CD4)
                keyLocations.Add("CD4");
            if ((locations & AssetLocation.CD5) == AssetLocation.CD5)
                keyLocations.Add("CD5");
            if ((locations & AssetLocation.CD6) == AssetLocation.CD6)
                keyLocations.Add("CD6");

            StringBuilder builder = new StringBuilder();
            builder.Append("[");

            if (keyLocations.Count == 0)
                builder.Append("No location");
            else
            {
                builder.Append(keyLocations[0]);

                for (Int32 index = 1; index < keyLocations.Count; ++index)
                {
                    builder.Append("; ");
                    builder.Append(keyLocations[index]);
                }
            }

            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>Indicates whether an extension is known</summary>
        /// <param name="extension">File extension to identify</param>
        /// <returns>Flag indicating whether the extension is recognized or not</returns>
        protected Boolean IsKnownExtension(String extension)
        {
            Boolean knownExtension = false;

            if (extension != null)
            {
                switch (extension.ToUpper())
                {
                    case "ARE":
                    case "BAM":
                    case "BIO":
                    case "CHR":
                    case "CHU":
                    case "CRE":
                    case "DLG":
                    case "EFF":
                    case "GAM":
                    case "IDS":
                    case "BMP":
                    case "INI":
                    case "ITM":
                    case "MOS":
                    case "MVE":
                    case "PLT":
                    case "PRO":
                    case "SAV":
                    case "BS":
                    case "BCS":
                    case "WAV":
                    case "SPL":
                    case "SRC":
                    case "STO":
                    case "TIS":
                    case "2DA":
                    case "VVC":
                    case "WED":
                    case "WFX":
                    case "WMP":
                    case "CBF":
                    case "BIF":
                    case "TLK":
                    case "KEY":
                    case "ACM":
                    case "MUS":
                    case "RES":
                    case "TOH":
                    case "TOT":
                    case "VAR":
                        knownExtension = true;
                        break;
                }
            }

            return knownExtension;
        }
        #endregion
    }
}