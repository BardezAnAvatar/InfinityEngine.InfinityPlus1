using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.Configuration.Ini;
using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Factories;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Version;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1;
using Bardez.Projects.InfinityPlus1.Information;
using Bardez.Projects.InfinityPlus1.Logic.Infinity.Factories;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    #region Detail on how Infinity treats resources
    //  Infinity locates resources in two primary ways. The first is the application directory, in which the *.exe
    //      executable resides and is ran from. The second is through the data locations specified in the
    //      baldur/icewind/torment *.ini file. These locations are HD0 and CD1 through CD6.
    //
    //  After performing a good deal of original research, I have come to find that the following resources reside as follows:
    //  Application Directory:
    //      executable, chitin.key, *.ini, override folders, music folder (beta location)
    //  HD0:
    //      music folder (alpha location), dialog.tlk
    //  key-specified:
    //      BIFF archives
    #endregion

    /// <summary>Represents the basic asset manager for Infinity Engine installs</summary>
    public class InfinityAssetManager : IAssetManager
    {
        #region Subclasses
        /// <summary>This class contains the various paths known in the IE</summary>
        protected class InfinityPaths
        {
            #region Fields
            /// <summary>The root directory information for the application to scan</summary>
            public String Application;

            /// <summary>The directory of HD0 assets</summary>
            public String HD0;

            /// <summary>The directory of CD1 assets</summary>
            public String CD1;

            /// <summary>The directory of CD2 assets</summary>
            public String CD2;

            /// <summary>The directory of CD3 assets</summary>
            public String CD3;

            /// <summary>The directory of CD4 assets</summary>
            public String CD4;

            /// <summary>The directory of CD5 assets</summary>
            public String CD5;

            /// <summary>The directory of CD6 assets</summary>
            public String CD6;
            #endregion
        }
        #endregion


        #region Constants
        /****************************************************
        *   This almost seems unnecessary, but I am having  *
        *   trouble tracking "NotOverridable" and whatnot,  *
        *   so just make a bunch of these constant.         *
        *   Also, I messed up "portraits", so constant.     *
        ****************************************************/

        /// <summary>Override folder string</summary>
        protected static readonly String StringOverride = "override";

        /// <summary>Scripts folder string</summary>
        protected static readonly String StringScripts = "scripts";

        /// <summary>Sounds folder string</summary>
        protected static readonly String StringSounds = "sounds";

        /// <summary>Portraits folder string</summary>
        protected static readonly String StringPortraits = "portraits";

        /// <summary>Characters folder string</summary>
        protected static readonly String StringCharacters = "characters";

        /// <summary>Music folder string</summary>
        protected static readonly String StringMusic = "music";

        /// <summary>Chitin.key file name string</summary>
        protected static readonly String StringKeyFile = "chitin.key";

        /// <summary>Chitin.key file name string</summary>
        protected static readonly String StringKeyAssetsCollection = "Key file";

        /// <summary>Non-overridable assets collection string</summary>
        protected static readonly String StringNotOverridableAsset = "NotOverridable";
        #endregion


        #region Fields
        /// <summary>Container for the known/found paths that indicate a location in the IE</summary>
        protected InfinityPaths paths;

        /// <summary>Represents the individual chitin.key file within an infinity engine game</summary>
        protected KeyTable chitinKey;

        /// <summary>This is the dictionary used to assign and retrieve all resources in their overridden hierarchy</summary>
        protected Dictionary<String, AssetReference> overriddenAssets;

        /// <summary>Dictionary containing a list of ALL resources found.</summary>
        protected Dictionary<String, List<AssetReference>> assetsByContainer;

        /// <summary>Dictionary of save directory collections</summary>
        protected Dictionary<String, List<SaveFolder>> saves;

        /// <summary>
        ///     Flag indicating whether the contained resources has been altered in any way, requiring
        ///     the overridden resources to be recalculated (such as an add operation to a Biff requiring reloading).
        /// </summary>
        protected Boolean dirty;

        /// <summary>Represents the language found when reading the Dialog.tlk file</summary>
        protected LanguageCode foundLanguage;

        /// <summary>The location at which the music folder was found</summary>
        protected AssetLocation musicLocation;
        #endregion


        #region Properties
        /// <summary>This is the dictionary used to assign and retrieve all resources in their overridden hierarchy</summary>
        protected Dictionary<String, AssetReference> OverriddenAssets
        {
            get
            {
                if (this.dirty)
                    this.BuildOverrideAssetTree();

                return this.overriddenAssets;
            }
        }
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="appDirectory">Target directory of the game installation</param>
        public InfinityAssetManager(String appDirectory)
        {
            this.paths = new InfinityPaths();
            this.paths.Application = appDirectory;

            this.overriddenAssets = null;
            this.dirty = false;
            this.foundLanguage = LanguageCode.Undefined;
            this.musicLocation = AssetLocation.LookUp;  //unknown

            this.LocateAssets();
        }
        #endregion


        #region Load methods
        /// <summary>Reads the resources from the chitin.key file and other asset folders</summary>
        protected void LocateAssets()
        {
            //initialize (clear) 
            this.assetsByContainer = new Dictionary<String, List<AssetReference>>();

            //find and open a game-specific 
            this.ReadConfigurationPaths();

            //locate and read the chitin.key file
            this.ReadChitinKey();
            this.LocateChitinKeyAssets();

            /********************************
            *   read the override folders   *
            ********************************/
            //the override hierarchy starts with override on the bottom, and characters on the top (overrides all others)
            this.LocateOverrideAssets(InfinityAssetManager.StringOverride);
            this.LocateOverrideAssets(InfinityAssetManager.StringScripts);
            this.LocateOverrideAssets(InfinityAssetManager.StringSounds);
            this.LocateOverrideAssets(InfinityAssetManager.StringPortraits);
            this.LocateOverrideAssets(InfinityAssetManager.StringCharacters);

            //locate the music folder in priority order, then read its assets
            this.LocateMusic();

            //locate the save folders
            this.LocateSaves();

            //locate the files that cannot be overridden
            this.LocateNonOverridableAssets();

            //since assets were added, the override tree needs to be updated
            this.dirty = true;
        }

        /// <summary>
        ///     Looks in the Application directory for specific *.ini files and loads the first found in the list,
        ///     reading it for application paths.
        /// </summary>
        protected void ReadConfigurationPaths()
        {
            String fileName = null;
            String target = null;

            if (File.Exists(target = Path.Combine(this.paths.Application, "baldur.ini")))
                fileName = target;
            else if (File.Exists(target = Path.Combine(this.paths.Application, "icewind.ini")))
                fileName = target;
            else if (File.Exists(target = Path.Combine(this.paths.Application, "torment.ini")))
                fileName = target;
            else if (File.Exists(target = Path.Combine(this.paths.Application, "icewind2.ini")))
                fileName = target;

            if (fileName == null)
                throw new InvalidOperationException(String.Format("Could not find any infinity-specific *.ini file in the target directory (\"{0}\").", this.paths.Application));

            //read the HD & CD directories from the ini file and then add it to the assets.
            this.ReadIniDiscPaths(fileName);
        }

        /// <summary>Opens the *.ini file specified and extracts its HD & CD paths from the settings</summary>
        /// <param name="iniFileName">Path to the file to extract paths from</param>
        protected void ReadIniDiscPaths(String iniFilePath)
        {
            if (!File.Exists(iniFilePath))
                throw new FileNotFoundException(String.Format("Could not file the specified configuration file (\"{0}\").", iniFilePath), iniFilePath);

            using (FileStream file = ReusableIO.OpenFile(iniFilePath))
            {
                IniConfiguration config = new IniConfiguration(file);

                /****************************************************************************************
                *   I think I have to assume that HD0 and Alias sections are present.                   *
                *   I will have to run some install tests with DVD packages to see what they generate   *
                *   -as well as GOG installs and demo installs- to see how their config files turn out. *
                *                                                                                       *
                *   Aside from those, the original disc versions have CDs 1 & 2 at minimum.             *
                *   TotSC has CD 6 at maximum.                                                          *
                ****************************************************************************************/

                if (!config.Sections.Keys.Contains("Alias"))
                    throw new ApplicationException(String.Format("The specified configuration file (\"{0}\") does not contain the \"Alias\" configuration section.", iniFilePath));

                if (!config.Sections["Alias"].Keys.Contains("HD0:"))
                    throw new ApplicationException(String.Format("The specified configuration file (\"{0}\") does not contain the \"HD0\" configuration item in the \"Alias\" section.", iniFilePath));

                this.paths.HD0 = config.Sections["Alias"]["HD0:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD1:"))
                    this.paths.CD1 = config.Sections["Alias"]["CD1:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD2:"))
                    this.paths.CD2 = config.Sections["Alias"]["CD2:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD3:"))
                    this.paths.CD3 = config.Sections["Alias"]["CD3:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD4:"))
                    this.paths.CD4 = config.Sections["Alias"]["CD4:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD5:"))
                    this.paths.CD5 = config.Sections["Alias"]["CD5:"].Value;

                if (config.Sections["Alias"].Keys.Contains("CD6:"))
                    this.paths.CD6 = config.Sections["Alias"]["CD6:"].Value;
            }
        }

        /// <summary>Locates and reads the chitin.key file</summary>
        protected void ReadChitinKey()
        {
            String chitinKeyPath = this.GetFilePathForLocation(this.paths.Application, InfinityAssetManager.StringKeyFile);

            if (!File.Exists(chitinKeyPath))
                throw new FileNotFoundException("Could not find chitin.key in the application directory", chitinKeyPath);

            //read the chitin file
            using (FileStream file = ReusableIO.OpenFile(chitinKeyPath))
            {
                KeyTable key = new KeyTable(true);
                key.Read(file, true);
                this.chitinKey = key;
            }
        }

        /// <summary>Locates the resources within the chitin.key file that has been read</summary>
        protected void LocateChitinKeyAssets()
        {
            //map the resources
            this.assetsByContainer[InfinityAssetManager.StringKeyAssetsCollection] = new List<AssetReference>();
            foreach (KeyTableResourceEntry resource in this.chitinKey.EntriesResource)
            {
                KeyTableBifEntry biff = this.chitinKey.EntriesBif[Convert.ToInt32(resource.ResourceLocator.BiffIndex)];
                AssetReference asset = new AssetReference(InfinityAssetManager.StringKeyFile, resource, biff);
                this.assetsByContainer[InfinityAssetManager.StringKeyAssetsCollection].Add(asset);
            }
        }

        /// <summary>Locates override items found in the specified override folder into the resource stack</summary>
        /// <param name="folderName">Name of the override folder to add</param>
        /// <remarks>
        ///     Does 'vanilla' IE restrict the assets found in certain paths (i.e.: "sounds" to that directory only?) Enhanced edition does not, but does IE?
        ///     Gibberlings 3 suggests not: http://forums.gibberlings3.net/index.php?showtopic=13204&view=findpost&p=113570
        ///     
        ///     My original research indicates that the override folders only work in the application directory.
        /// </remarks>
        protected void LocateOverrideAssets(String folderName)
        {
            String overridePath = Path.Combine(this.paths.Application, folderName);

            if (Directory.Exists(overridePath))
            {
                DirectoryInfo overrideDirectory = new DirectoryInfo(overridePath);
                FileInfo[] files = overrideDirectory.GetFiles();

                //override
                this.assetsByContainer[folderName] = new List<AssetReference>();
                foreach (FileInfo file in files)
                {
                    AssetReference asset = new AssetReference(AssetLocation.ApplicationDirectory, Path.Combine(overrideDirectory.Name, file.Name));
                    this.assetsByContainer[folderName].Add(asset);
                }
            }
        }

        /// <summary>Locates the music files within the music folder in its overriden location order</summary>
        protected void LocateMusic()
        {
            //try HD0 first, per research on IE
            String directory = null;
            AssetLocation location = AssetLocation.LookUp;

            //check for existance
            if (this.DirectoryExistsInLocation(this.paths.HD0, InfinityAssetManager.StringMusic))
            {
                directory = this.paths.HD0;
                location = AssetLocation.HD0;
            }
            else if (this.DirectoryExistsInLocation(this.paths.Application, InfinityAssetManager.StringMusic))
            {
                directory = this.paths.Application;
                location = AssetLocation.ApplicationDirectory;
            }

            //if no music was found, IE will continue and play no music, ever (at least from opening screens), so not a failure condition
            //  if (directory == null)
            //  throw new DirectoryNotFoundException("Could not find the \"music\" directory in either the Application or the HD0 locations.");

            if (directory != null)  //music was found
            {
                this.musicLocation = location;  //only set if it was found

                //get the actual name of the directory in the file system;
                DirectoryInfo musicDirectory = new DirectoryInfo(Path.Combine(directory, InfinityAssetManager.StringMusic));

                //read the *.mus files
                this.assetsByContainer[InfinityAssetManager.StringMusic] = new List<AssetReference>();
                FileInfo[] files = musicDirectory.GetFiles();
                foreach (FileInfo file in files)
                {
                    AssetReference asset = new AssetReference(location, Path.Combine(musicDirectory.Name, file.Name));
                    this.assetsByContainer[InfinityAssetManager.StringMusic].Add(asset);
                }

                /****************************************************************************************************
                *   music is treated in this odd way, where only direct references to the music are made.           *
                *   "music\Theme\Themea.acm" is only ever referenced, and "music\BC1\Themea.acm" is never loaded.   *
                *****************************************************************************************************/
                //read the *.acm directories & files
                DirectoryInfo[] directories = musicDirectory.GetDirectories();
                foreach (DirectoryInfo dir in directories)
                {
                    String musicSubFolderName = Path.Combine(musicDirectory.Name, dir.Name);

                    //get the contents of each directory
                    files = dir.GetFiles(String.Format("{0}*.acm", dir.Name));
                    foreach (FileInfo file in files)
                    {
                        AssetReference asset = new AssetReference(location, Path.Combine(musicSubFolderName, file.Name));
                        this.assetsByContainer[InfinityAssetManager.StringMusic].Add(asset);
                    }
                }
            }
        }

        /// <summary>Locates the save files and folders</summary>
        protected void LocateSaves()
        {
            //initialize
            this.saves = new Dictionary<String, List<SaveFolder>>();

            //read the save folders
            this.LocateSaveDirectory("save");
            this.LocateSaveDirectory("mpsave");
        }

        /// <summary>Locates the save files within the specified subdirectory</summary>
        /// <remarks>This method will wipe any previously located or registered saves and repopulate them.</remarks>
        protected void LocateSaveDirectory(String saveDirectory)
        {
            String saveDir = Path.Combine(this.paths.Application, saveDirectory);

            if (Directory.Exists(saveDir))
            {
                this.saves[saveDirectory] = new List<SaveFolder>();

                DirectoryInfo saveRoot = new DirectoryInfo(saveDir);
                DirectoryInfo[] saves = saveRoot.GetDirectories();

                foreach (DirectoryInfo save in saves)
                {
                    SaveFolder folder = new SaveFolder(save.FullName);

                    FileInfo[] files = save.GetFiles();
                    foreach (FileInfo file in files)
                        folder.Resources.Add(new AssetReference(AssetLocation.ApplicationDirectory, Path.Combine(saveRoot.Name, save.Name, file.Name)));

                    this.saves[saveDirectory].Add(folder);
                }
            }
        }

        /// <summary>Locates assets that cannot be overridden, such as chitin.key, dialog.tlk and keymap.ini</summary>
        protected void LocateNonOverridableAssets()
        {
            this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset] = new List<AssetReference>();

            this.LocateConfigurationFiles();
            this.LocateVariableFile();
            this.LocateKeyFile();
            this.LocateDialogTlkAssets();
        }

        /// <summary>Locates specific *.ini files in the application directory</summary>
        protected void LocateConfigurationFiles()
        {
            DirectoryInfo appDir = new DirectoryInfo(this.paths.Application);

            FileInfo[] files = appDir.GetFiles("*.ini");
            foreach (FileInfo ini in files)
            {
                String iniName = ini.Name.ToLower();

                //track the valid *.ini files
                switch (iniName)
                {
                    case "baldur.ini":
                    case "torment.ini":
                    case "icewind.ini":
                    case "icewind2.ini":

                    case "keymap.ini":
                    case "autonote.ini":
                    case "beast.ini":
                    case "layout.ini":
                    case "quests.ini":
                    case "party.ini":
                        //add the ini files to the assets
                        AssetReference asset = new AssetReference(AssetLocation.ApplicationDirectory, ini.Name);
                        this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Add(asset);
                        break;
                }
            }
        }

        /// <summary>Locates var.var in the application directory</summary>
        /// <remarks>Original research revealed that moving VAR.VAR to the HD0 location crashes the game</remarks>
        protected void LocateVariableFile()
        {
            String varPath = Path.Combine(this.paths.Application, "VAR.VAR");

            if (File.Exists(varPath))
            {
                FileInfo varFile = new FileInfo(varPath);

                AssetReference asset = new AssetReference(AssetLocation.ApplicationDirectory, varFile.Name);
                this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Add(asset);
            }
        }

        /// <summary>Locates and adds the chitin.key file</summary>
        protected void LocateKeyFile()
        {
            String chitinPath = Path.Combine(this.paths.Application, InfinityAssetManager.StringKeyFile);

            if (!File.Exists(chitinPath))
                throw new FileNotFoundException("Could not find chitin.key in the application directory", chitinPath);

            FileInfo chitinKeyFile = new FileInfo(chitinPath);

            //add chitin.key to the resources
            AssetReference keyFile = new AssetReference(AssetLocation.ApplicationDirectory, chitinKeyFile.Name);
            this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Add(keyFile);
        }

        /// <summary>Locates and adds the dialog.tlk file and dialogf.tlk</summary>
        protected void LocateDialogTlkAssets()
        {
            //look for dialog.tlk, required
            this.LocateDialogTlkAssets_Single("dialog.tlk");

            //look for dialogf.tlk, optional
            this.LocateDialogTlkAssets_Single("dialogf.tlk", false, false);
        }

        /// <summary>Loads a specifically targeted TLK file into the assets</summary>
        /// <param name="tlkTarget">Dialog.tlk, Dialogf.tlk</param>
        /// <param name="required">Flag indicating whether the target being nonexistant should raise an error</param>
        /// <param name="peek">Opens the file and reads its language ID for retention</param>
        /// <remarks>Original research indicates that dialog.tlk resides in HD0, oddly</remarks>
        protected void LocateDialogTlkAssets_Single(String tlkTarget, Boolean required = true, Boolean peek = true)
        {
            //look for .tlk file
            String tlkPath = Path.Combine(this.paths.HD0, tlkTarget);

            if (File.Exists(tlkPath))
            {
                FileInfo dialogFile = new FileInfo(tlkPath);

                //add chitin.key to the resources
                AssetReference tlkFile = new AssetReference(AssetLocation.HD0, dialogFile.Name);
                this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Add(tlkFile);

                //store its language ID
                if (peek)
                    this.ReadLangaugeIdFromTlkAsset(dialogFile);
            }
            else if (required)
                throw new FileNotFoundException(String.Format("Could not find {0} in the application directory", tlkTarget), tlkPath);
        }

        /// <summary>Opens the specified tlk file and reads its language code from its header</summary>
        /// <param name="dialogTlk">FileInfo loaded pointing to a TLK file</param>
        protected void ReadLangaugeIdFromTlkAsset(FileInfo dialogTlk)
        {
            //open the Dialog.tlk file and peek to see which language was installed
            using (Stream input = dialogTlk.OpenRead())
            {
                TalkTable dialog = new TalkTable(false);
                dialog.Read(input, false); //just get the header

                //check the language known
                if (dialog.Header != null)
                {
                    //the languages act in a strange way. Everything but Italian has a normal 0, 1, 2, or 3.
                    //  Italian has the "4" bit set in everything EXCEPT dialogF.tlk for Icewind Dale, which indicates English.
                    //  So (grasping at straws), if I just look at dialog.tlk and do it in reverse order I can maybe
                    //  detect the language.

                    Int16 language = (Int16)(dialog.Header.Language);   //I have to cast this, I think.
                    if ((language & (Int16)(InfinityEngineLanguage.Italian)) == (Int16)(InfinityEngineLanguage.Italian))    //Italian, I guess, is 4
                        this.foundLanguage = LanguageCode.it_IT;
                    else if ((language & (Int16)(InfinityEngineLanguage.Spanish)) == (Int16)(InfinityEngineLanguage.Spanish))
                        this.foundLanguage = LanguageCode.es_ES;
                    else if ((language & (Int16)(InfinityEngineLanguage.German)) == (Int16)(InfinityEngineLanguage.German))
                        this.foundLanguage = LanguageCode.de_DE;
                    else if ((language & (Int16)(InfinityEngineLanguage.French)) == (Int16)(InfinityEngineLanguage.French))
                        this.foundLanguage = LanguageCode.fr_FR;
                    else if ((language & (Int16)(InfinityEngineLanguage.English)) == (Int16)(InfinityEngineLanguage.English))
                        this.foundLanguage = LanguageCode.en_US;
                }
            }
        }
        #endregion


        #region Asset retrieval
        /// <summary>Gets a binary stream containing the asset requested</summary>
        /// <param name="filename">Asset name and file extension</param>
        /// <returns>A binary stream containing the asset</returns>
        /// <remarks>This would be used for items, spells, etc.</remarks>
        public Stream GetAsset(String filename)
        {
            AssetReference asset = this.OverriddenAssets[filename];

            return this.GetAssetInstance(asset.Locator);
        }

        /// <summary>Gets a binary stream containing the specific instance of the asset requested</summary>
        /// <param name="location">Location structure that defines the location of an asset's instance</param>
        /// <returns>A binary stream containing the instance of the requested asset</returns>
        public Stream GetAssetInstance(AssetLocator location)
        {
            Stream output = null;

            if (location != null)
            {
                if (location.ResourceLocator != null)
                    output = this.GetAssetFromBiffArchive(location);
                else
                    output = this.GetAssetFromFile(location);
            }

            return output;
        }

        /// <summary>Gets the asset from a BIFF archive</summary>
        /// <param name="location">Location from which to locate the asset</param>
        /// <returns>A binary stream containing the asset</returns>
        protected Stream GetAssetFromBiffArchive(AssetLocator location)
        {
            Stream output = null;

            if (location != null)
            {
                if (location.ResourceLocator != null)
                {
                    KeyTableBifEntry bifEntry = this.chitinKey.EntriesBif[Convert.ToInt32(location.ResourceLocator.BiffIndex)];

                    String bifName = bifEntry.BifFileName.Value;
                    String bifPath = this.LocateBiffArchive(bifEntry);

                    if (!File.Exists(bifPath))
                        throw new FileNotFoundException(String.Format("Could not find the requested archive \"{0}\".", bifName), bifName);

                    MemoryStream copy = new MemoryStream();

                    using (FileStream file = ReusableIO.OpenFile(bifPath))
                    {
                        IBiff archive = BiffFactory.BuildBiff(file);

                        if (location.ResourceType == ResourceType.Tileset)
                            output = archive.ExtractTileset(location.ResourceLocator);
                        else
                            output = archive.ExtractResource(location.ResourceLocator);
                    }
                }
                else
                    throw new ApplicationException("The specified resource location is not within a BIFF archive.");
            }

            return output;
        }

        /// <summary>Gets an asset from a filesystem location</summary>
        /// <param name="location">Location from which to locate the asset</param>
        /// <returns>A binary stream containing the asset</returns>
        protected Stream GetAssetFromFile(AssetLocator location)
        {
            Stream output = null;

            if (location != null && location.ResourceLocator == null && location.RelativePath != null && location.Location != AssetLocation.LookUp)
                output = ReusableIO.OpenFile(this.GetFilePathForLocation(location.Location, location.RelativePath));

            return output;
        }

        /// <summary>Returns the full file system path to the BIFF archive specified</summary>
        /// <param name="biffName">The biff entry to get a location for</param>
        /// <returns>The full file system path to the BIFF archive specified</returns>
        protected String LocateBiffArchive(KeyTableBifEntry biffEntry)
        {
            String biffPath = null;

            //get the first found path that can be used
            if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.HardDrive) == KeyTableBifLocationEnum.HardDrive)
                biffPath = Path.Combine(this.paths.HD0, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc1) == KeyTableBifLocationEnum.Disc1)
                biffPath = Path.Combine(this.paths.CD1, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc2) == KeyTableBifLocationEnum.Disc2)
                biffPath = Path.Combine(this.paths.CD2, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc3) == KeyTableBifLocationEnum.Disc3)
                biffPath = Path.Combine(this.paths.CD3, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc4) == KeyTableBifLocationEnum.Disc4)
                biffPath = Path.Combine(this.paths.CD4, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc5) == KeyTableBifLocationEnum.Disc5)
                biffPath = Path.Combine(this.paths.CD5, biffEntry.BifFileName.Value);
            else if ((biffEntry.BifLocationFlags & KeyTableBifLocationEnum.Disc6) == KeyTableBifLocationEnum.Disc6)
                biffPath = Path.Combine(this.paths.CD6, biffEntry.BifFileName.Value);

            return biffPath;
        }
        #endregion


        #region Asset Tree exposure
        /// <summary>
        ///     Gets the asset tree completely overridden with single instances of each file
        ///     inside a grouping of its own asset type
        /// </summary>
        /// <returns>The asset tree of completely overridden files, grouped by their extension</returns>
        /// <remarks>
        ///     Matches design view 1
        ///
        ///     This shall be a root node with a TLK, DLG, ITM, STO, BIF, BMP, etc., with all files matching
        ///     that extension inside
        /// </remarks>
        public AssetNode GetAssetTree_GroupedOverridden()
        {
            AssetNode tree = new AssetNode("Root");

            //since all I care about is the extension type, I will group them by that using linq
            var extensions = this.OverriddenAssets.Select(assets => assets.Value).GroupBy(asset => asset.AssetType);

            //populate the asset types
            this.PopulateAssetTree_GroupByExtension(tree, extensions);

            //create a BIFF node and attach biffs to it, then attach it between the grouped extensions, in place.
            AssetNode bifNode = new AssetNode("BIF");
            this.PopulateAssetTree_CreateBiffNodesNoLocation(bifNode);
            this.PopulateAssetTree_InsertNodeBetweenChildren(tree, bifNode);

            //add saves
            this.PopulateAssetTree_AddSaveFolders(tree);

            return tree;
        }

        /// <summary>Gets the asset tree showing all instances</summary>
        /// <returns>The asset tree showing all instances of assets found, grouped by their extension</returns>
        /// <remarks>
        ///     Matches design view 2
        ///
        ///     This shall be a root node with a TLK, DLG, ITM, STO, BIF, BMP, etc., with every instance of a files
        ///     matching that extension inside (i.e.: [2DA.Bif, 0xFF123400] and [override] shown
        /// </remarks>
        public AssetNode GetAssetTree_GroupedAllInstances()
        {
            AssetNode tree = new AssetNode("Root");

            //since all I care about is the extension type, I will group them by that using linq
            var extensions = this.assetsByContainer.SelectMany(assetCollection => assetCollection.Value).GroupBy(asset => asset.AssetType);

            //populate the assets grouped by aasset file extension
            this.PopulateAssetTree_GroupByExtension(tree, extensions);

            //create a BIFF node and attach biffs to it, then attach it between the grouped extensions, in place.
            AssetNode bifNode = new AssetNode("BIF");
            this.PopulateAssetTree_CreateBiffNodesNoLocation(bifNode);
            this.PopulateAssetTree_InsertNodeBetweenChildren(tree, bifNode);

            //add saves
            this.PopulateAssetTree_AddSaveFolders(tree);

            return tree;
        }

        /// <summary>Gets the asset tree showing all instances in their locations</summary>
        /// <returns>The asset tree showing all instances of assets found, grouped by their location</returns>
        /// <remarks>
        ///     Matches design view 3
        ///
        ///     This shall be a root node with a location root (chitin.key, override, xp1.key, portraits)
        ///     with each instance of an asset listed within its location, duplicates shown
        /// </remarks>
        public AssetNode GetAssetTree_LocationInstances()
        {
            AssetNode tree = new AssetNode("Root");

            //create the App, HD0, etc. nodes
            this.PopulateAssetTree_CreateLocationNodes(tree);

            //check for an application directory node within the children
            AssetNode app = tree.Children.Where(node => node.Name == "App").FirstOrDefault();
            if (app == null)
                throw new InvalidOperationException("Could not locate App node in the Root's children.");

            AssetNode hd0 = tree.Children.Where(node => node.Name == "HD0").FirstOrDefault();
            if (hd0 == null)
                throw new InvalidOperationException("Could not locate HD0 node in the Root's children.");

            //first attach anything from chitin.key
            this.PopulateAssetTree_PopulateKeyAssetsNoGroup(app);

            //attach the various override directories
            this.PopulateAssetTree_PopulateOverrideAssetsInLocation(app);

            //Populate BIFF archives
            this.PopulateAssetTree_PopulateBiffsInLocations(tree, false);

            //attach the music directory in its appropriate location
            this.PopulateAssetTree_PopulateMusicAssetsInLocation(tree, false);

            //add the saves
            this.PopulateAssetTree_AddSaveFolders(app);

            //then, attach assets found in non-chitin root locations
            this.PopulateAssetTree_PopulateApplicationContainerAssets(app);
            this.PopulateAssetTree_PopulateHD0ContainerAssets(hd0);

            return tree;
        }

        /// <summary>Gets the asset tree showing overridden instances, in their locations, grouped by type</summary>
        /// <returns>The asset tree showing overridden assets found, grouped by location and asset type</returns>
        /// <remarks>Matches design view 4</remarks>
        public AssetNode GetAssetTree_LocationGroupedOverridden()
        {
            AssetNode tree = new AssetNode("Root");

            //create the App, HD0, etc. nodes
            this.PopulateAssetTree_CreateLocationNodes(tree);

            //check for an application directory node within the children
            AssetNode app = tree.Children.Where(node => node.Name == "App").FirstOrDefault();
            if (app == null)
                throw new InvalidOperationException("Could not locate App node in the Root's children.");

            AssetNode hd0 = tree.Children.Where(node => node.Name == "HD0").FirstOrDefault();
            if (hd0 == null)
                throw new InvalidOperationException("Could not locate HD0 node in the Root's children.");

            //since all I care about is the extension type, I will group them by that using linq
            var extensions = this.OverriddenAssets.Where(pair=>pair.Value.Locator.KeyFileName == InfinityAssetManager.StringKeyFile).Select(assets => assets.Value).GroupBy(asset => asset.AssetType);

            //populate the key file node:
            AssetNode keyFile = new AssetNode(this.OverriddenAssets[InfinityAssetManager.StringKeyFile]);
            app.Children.Add(keyFile);
            this.PopulateAssetTree_GroupByExtension(keyFile, extensions);

            //now I have to break it up by override locations
            this.PopulateAssetTree_PopulateOverriddenAssetsInOverrideLocationsByType(app);

            //attach the music directory in its appropriate location
            this.PopulateAssetTree_PopulateMusicAssetsInLocation(tree, true);

            //Populate BIFF archives
            this.PopulateAssetTree_PopulateBiffsInLocations(tree, true);

            //then, attach assets found in non-chitin locations (TLK, INI, VAR, SRC)


            //BUG: saves are showing up underneath the chitin.key

            return tree;
        }

        /*
        /// <summary>Gets the asset tree showing all asset instances, in their locations, grouped by type</summary>
        /// <returns>The asset tree showing overridden assets found, grouped by location and asset type</returns>
        /// <remarks>Matches design view 5</remarks>
        public AssetNode GetAssetTree_LocationGroupedAllInstances();
        */

        /// <summary>Populates a tree and adds its resources based on the matched assets</summary>
        /// <param name="parent">The parent node at which to attach the assets grouped by extension</param>
        /// <param name="extensions">Assets found (via Linq)</param>
        protected void PopulateAssetTree_GroupByExtension(AssetNode parent, IEnumerable<IGrouping<String, AssetReference>> extensions)
        {
            //take all the non-empty extensions first
            var knownExtensions = extensions.Where(grouping => this.IsKnownExtension(grouping.Key));
            this.PopulateAssetTree_AssetsByExtension(parent, knownExtensions);

            //take the empty extensions next
            var unknownExtensions = extensions.Where(grouping => !this.IsKnownExtension(grouping.Key)).OrderBy(grouping => grouping.Key);
            this.PopulateAssetTree_AssetsByExtension(parent, unknownExtensions);
        }

        /// <summary>Takes the ordered enumeration of file extension groups and populates them to the asset tree</summary>
        /// <param name="parent">The parent node of the asset tree</param>
        /// <param name="extensions">Ordered collection of file extensions to add to the root of the tree</param>
        protected void PopulateAssetTree_AssetsByExtension(AssetNode parent, IEnumerable<IGrouping<String, AssetReference>> extensions)
        {
            foreach (var extensionGroup in extensions.OrderBy(pair => pair.Key))
            {
                AssetNode extension = new AssetNode(extensionGroup.Key.ToUpper());

                foreach (AssetReference asset in extensionGroup.OrderBy(assets => assets.AssetName))
                {
                    AssetNode assetInstance = new AssetNode(asset);
                    extension.Children.Add(assetInstance);
                }

                parent.Children.Add(extension);
            }
        }

        /// <summary>Adds the save directories to the asset tree, if they exist</summary>
        /// <param name="tree">The root of the asset tree</param>
        protected void PopulateAssetTree_AddSaveFolders(AssetNode tree)
        {
            if (this.saves.Count > 0)   //create a save folder at all?
            {
                AssetNode savesNode = new AssetNode("Saves");   //Saves folder

                foreach (String key in this.saves.Keys)         //mp save, save
                {
                    AssetNode saveGroup = new AssetNode(key);

                    foreach (SaveFolder save in this.saves[key])    //0000000-whitty name
                    {
                        AssetNode saveInstance = new AssetNode(save.SaveFolderName);

                        foreach (AssetReference asset in save.Resources)     //00000000-whitty name\baldur.sav
                        {
                            AssetNode assetInstance = new AssetNode(asset);
                            saveInstance.Children.Add(assetInstance);
                        }

                        saveGroup.Children.Add(saveInstance);
                    }

                    savesNode.Children.Add(saveGroup);
                }

                tree.Children.Add(savesNode);
            }
        }

        /// <summary>Populates all asset instances in the chitin.key node</summary>
        /// <param name="parent">root node to attach a chitin.key node to</param>
        protected void PopulateAssetTree_PopulateKeyAssetsNoGroup(AssetNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "Parent node was unexpectedly null.");

            //create a chitin.key node
            AssetReference keyFile = this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Where(asset => asset.AssetName.ToLower() == InfinityAssetManager.StringKeyFile).FirstOrDefault();
            AssetNode chitinKey = new AssetNode(keyFile);

            List<AssetReference> chitinAssets = this.assetsByContainer[InfinityAssetManager.StringKeyAssetsCollection];

            //load up HD0, which should always be present
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "HD0", KeyTableBifLocationEnum.HardDrive, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD1", KeyTableBifLocationEnum.Disc1, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD2", KeyTableBifLocationEnum.Disc2, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD3", KeyTableBifLocationEnum.Disc3, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD4", KeyTableBifLocationEnum.Disc4, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD5", KeyTableBifLocationEnum.Disc5, chitinAssets);
            this.PopulateAssetTree_PopulateKeyAssets_SpecificLocation(chitinKey, "CD6", KeyTableBifLocationEnum.Disc6, chitinAssets);

            parent.Children.Add(chitinKey);
        }

        /// <summary>Gets the assets found in a specific location and adds the location and its assets them to the node tree</summary>
        /// <param name="keyNode">Asset node representing chitin.key</param>
        /// <param name="locationName">Name of the key location to add (HD0, CD1, CD2, etc.)</param>
        /// <param name="location">Specific location flag to match on</param>
        /// <param name="assets">Assets to query from</param>
        protected void PopulateAssetTree_PopulateKeyAssets_SpecificLocation(AssetNode keyNode, String locationName, KeyTableBifLocationEnum location, IEnumerable<AssetReference> assets)
        {
            IEnumerable<AssetReference> locationAssets = assets.Where(asset => asset.Locator != null && asset.Locator.ResourceLocator != null && (this.chitinKey.EntriesBif[Convert.ToInt32(asset.Locator.ResourceLocator.BiffIndex)].BifLocationFlags & location) == location);
            if (locationAssets.Count() > 0)
                this.PopulateAssetTree_PopulateKeyAssetsInLocationNoGroup(keyNode, locationName, locationAssets);
        }

        /// <summary>Populates the container (HD0, CD1, etc.) with its BIFFs and their child assets</summary>
        /// <param name="keyNode">The key file container to add the location node to</param>
        /// <param name="location">The name of the location to add (CD1, HD0, CD6, etc.)</param>
        /// <param name="assets">The collection of assets that can be queried to populate BIFFs with</param>
        protected void PopulateAssetTree_PopulateKeyAssetsInLocationNoGroup(AssetNode keyNode, String location, IEnumerable<AssetReference> assets)
        {
            AssetNode container = new AssetNode(location);

            //isolate individual BIF entries referenced
            var indices = assets.Where(asset => asset.Locator != null && asset.Locator.ResourceLocator != null).Select(asset => asset.Locator.ResourceLocator.BiffIndex).Distinct();
            Dictionary<Int32, KeyTableBifEntry> biffEntries = new Dictionary<Int32, KeyTableBifEntry>();
            foreach (Int32 index in indices)
                biffEntries[index] = this.chitinKey.EntriesBif[index];

            //now with the BIF entries, create hierarchies of the BIF data folders
            IEnumerable<String> directories = biffEntries.Select(entry => Path.GetDirectoryName(entry.Value.BifFileName.Value)).Distinct();

            //traverse the directories set up and create them nodes.
            this.PopulateAssetTree_CreateSubdirectoryNodes(container, directories);

            //create a nod efor each BIF entry (even if redundant, secondary, whatever)
            this.PopulateAssetTree_PopulateBifsAndAssets(container, biffEntries, assets);

            keyNode.Children.Add(container);
        }

        /// <summary>Populates the asset tree with assets found in the override directories</summary>
        /// <param name="parent">Parent node at which to add the items</param>
        protected void PopulateAssetTree_PopulateOverrideAssetsInLocation(AssetNode parent)
        {
            //get the collection of sub folders that have been created. This, then, is entirely optional.
            IEnumerable<String> containers = this.assetsByContainer.Keys.Where(key => key != InfinityAssetManager.StringNotOverridableAsset && key != InfinityAssetManager.StringKeyAssetsCollection && key != InfinityAssetManager.StringMusic);

            //create subdirectories
            this.PopulateAssetTree_CreateSubdirectoryNodes(parent, containers);

            //iterate through the containers and populate the corresponding node
            foreach (String location in containers)
            {
                AssetNode container = this.TraverseToNode(parent, location);

                foreach (AssetReference asset in this.assetsByContainer[location])
                {
                    AssetNode assetInstance = new AssetNode(asset);
                    container.Children.Add(assetInstance);
                }
            }
        }

        /// <summary>Populates the BIFFs in their locations and creates necessary hierarchy for them</summary>
        /// <param name="parent">Parent node at which to attach these assets</param>
        /// <param name="group">Flag indicating whether the BIF assets need to be grouped</param>
        protected void PopulateAssetTree_PopulateBiffsInLocations(AssetNode parent, Boolean group)
        {
            AssetNode hd0, cd1, cd2, cd3, cd4, cd5, cd6;
            hd0 = parent.Children.Where(node => node.Name == "HD0").FirstOrDefault();
            cd1 = parent.Children.Where(node => node.Name == "CD1").FirstOrDefault();
            cd2 = parent.Children.Where(node => node.Name == "CD2").FirstOrDefault();
            cd3 = parent.Children.Where(node => node.Name == "CD3").FirstOrDefault();
            cd4 = parent.Children.Where(node => node.Name == "CD4").FirstOrDefault();
            cd5 = parent.Children.Where(node => node.Name == "CD5").FirstOrDefault();
            cd6 = parent.Children.Where(node => node.Name == "CD6").FirstOrDefault();

            if (hd0 == null)
                throw new InvalidOperationException("Could not find an HD0 node in the parent node's children.");

            this.PopulateAssetTree_PopulateBiffsInLocation(hd0, AssetLocation.HD0, group);

            if (cd1 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd1, AssetLocation.CD1, group);
            if (cd2 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd2, AssetLocation.CD2, group);
            if (cd3 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd3, AssetLocation.CD3, group);
            if (cd4 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd4, AssetLocation.CD4, group);
            if (cd5 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd5, AssetLocation.CD5, group);
            if (cd6 != null)
                this.PopulateAssetTree_PopulateBiffsInLocation(cd6, AssetLocation.CD6, group);
        }

        /// <summary>Creates the directories for BIFF paths and their nodes within the asset tree</summary>
        /// <param name="parent">Parent at which to add the paths (HD0, CD1, etc.)</param>
        /// <param name="location">Location used to identify matching BIFFs</param>
        /// <param name="group">Flag indicating whether the assets need to be grouped</param>
        protected void PopulateAssetTree_PopulateBiffsInLocation(AssetNode parent, AssetLocation location, Boolean group)
        {
            //find the directories needed
            IEnumerable<String> directories = this.chitinKey.EntriesBif.Select(biff => Path.GetDirectoryName(biff.BifFileName.Value)).Distinct();
            
            //create subdirectories
            this.PopulateAssetTree_CreateSubdirectoryNodes(parent, directories);

            this.PopulateAssetTree_CreateBiffNodesInLocation(parent, location, group);
        }

        /// <summary>Populates the music directory (if located) and its assets in the location at which they were found</summary>
        /// <param name="root">The root node, which should contain a node for the location at which music was located, if applicable</param>
        /// <param name="group">Flag indicating whether or not the assets should be grouped</param>
        protected void PopulateAssetTree_PopulateMusicAssetsInLocation(AssetNode root, Boolean group)
        {
            //check to see if there is any work to be done
            if (this.musicLocation != AssetLocation.LookUp)
            {
                //validate
                if (!this.assetsByContainer.ContainsKey(InfinityAssetManager.StringMusic))
                    throw new InvalidOperationException("The music location was set, but the assets collection did not appear to contain any music assets.");

                //get the node for the music directory's location
                AssetNode locationNode = this.TraverseToNode(root, this.musicLocation.GetShortName());

                //get the paths found for the assets
                IEnumerable<String> directories = this.assetsByContainer[InfinityAssetManager.StringMusic].Select(asset => Path.GetDirectoryName(asset.Locator.RelativePath)).Distinct();

                //create subdirectories
                this.PopulateAssetTree_CreateSubdirectoryNodes(locationNode, directories);

                //iterate through the assets and populate
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringMusic])
                {
                    AssetNode container = this.TraverseToNode(locationNode, Path.GetDirectoryName(asset.Locator.RelativePath));

                    if (group)  //check for a grouping folder
                    {
                        AssetNode typeContainer = container.Children.Where(node => node.Name == asset.AssetType.ToUpper()).FirstOrDefault();
                        if (typeContainer == null)
                        {
                            typeContainer = new AssetNode(asset.AssetType.ToUpper());
                            container.Children.Add(typeContainer);
                        }

                        container = typeContainer;
                    }

                    AssetNode assetInstance = new AssetNode(asset);
                    container.Children.Add(assetInstance);
                }
            }
        }

        /// <summary>Populates the asset tree with assets found in the override directories that are overridden</summary>
        /// <param name="parent">Parent node at which to add the items</param>
        protected void PopulateAssetTree_PopulateOverriddenAssetsInOverrideLocationsByType(AssetNode parent)
        {
            //get the collection of sub folders that have been created. This, then, is entirely optional.
            IEnumerable<String> containers = this.OverriddenAssets.Values.Where(asset => this.IsOverrideAsset(asset.Locator)).Select(asset => Path.GetDirectoryName(asset.Locator.RelativePath)).Distinct();

            //create subdirectories
            this.PopulateAssetTree_CreateSubdirectoryNodes(parent, containers);

            //iterate through the containers and populate the corresponding node
            foreach (String location in containers)
            {
                AssetNode container = this.TraverseToNode(parent, location);

                IEnumerable<IGrouping<String, AssetReference>> groupedAssets = this.OverriddenAssets.Values.Where(asset => Path.GetDirectoryName(asset.Locator.RelativePath) == location).GroupBy(asset => asset.AssetType);

                this.PopulateAssetTree_GroupByExtension(container, groupedAssets);
            }
        }

        /// <summary>Loads the root containter assets into the asset tree</summary>
        /// <param name="parent">Parent node at which to add the "root" assets</param>
        protected void PopulateAssetTree_PopulateApplicationContainerAssets(AssetNode parent)
        {
            //load files that are not the key file. the order should be something like dialogs, ini, var, src.
            IEnumerable<AssetReference> rootAssets = this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Where(asset => asset.AssetType.ToLower() != "key" && asset.AssetType.ToLower() != "tlk").OrderBy(asset => asset.AssetType.ToLower() == "ini");

            foreach (AssetReference asset in rootAssets)
            {
                AssetNode node = new AssetNode(asset);
                parent.Children.Add(node);
            }
        }

        /// <summary>Loads the root containter assets into the asset tree</summary>
        /// <param name="parent">Parent node at which to add the "root" assets</param>
        protected void PopulateAssetTree_PopulateHD0ContainerAssets(AssetNode parent)
        {
            //load files that are not the key file. the order should be something like dialogs, ini, var, src.
            IEnumerable<AssetReference> rootAssets = this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Where(asset => asset.AssetType.ToLower() == "tlk").OrderBy(asset => asset.AssetName);

            foreach (AssetReference asset in rootAssets)
            {
                AssetNode node = new AssetNode(asset);
                parent.Children.Add(node);
            }
        }

        /// <summary>Recursively populates the parent node with subdirectory nodes in the specified directores</summary>
        /// <param name="parent">Parent node at which to attach these subdirectories</param>
        /// <param name="directories">Directories to add</param>
        /// <remarks>This is unanticipated corner code for someone screwing around with their key file. I really only expect "data" and "movies"</remarks>
        protected void PopulateAssetTree_CreateSubdirectoryNodes(AssetNode parent, IEnumerable<String> directories)
        {
            foreach (String dir in directories)
            {
                AssetNode relativeParent = parent;

                IList<String> subDirs = this.GetPathItems(dir);

                //iterate through the tree to create the directories
                foreach (String subDir in subDirs)
                {
                    AssetNode match = relativeParent.Children.Where(node => node.Name.ToLower() == subDir.ToLower()).FirstOrDefault();

                    if (match == null)
                    {
                        AssetNode node = new AssetNode(subDir);
                        relativeParent.Children.Add(node);
                        relativeParent = node;
                    }
                    else
                        relativeParent = match;
                }
            }
        }

        /// <summary>Populates the chitin.key location asset node (HD0, CD1, etc.) with the Biffs and their assets</summary>
        /// <param name="parent">Parent location (HD0, CD1, etc.) node within chitin.key to populate with BIFFs</param>
        /// <param name="biffEntries">Collection of BIFF entries and their entry indices to populate</param>
        /// <param name="assets">Assets to populate</param>
        protected void PopulateAssetTree_PopulateBifsAndAssets(AssetNode parent, Dictionary<Int32, KeyTableBifEntry> biffEntries, IEnumerable<AssetReference> assets)
        {
            //iterate through BIFFs
            foreach (KeyValuePair<Int32, KeyTableBifEntry> item in biffEntries)
            {
                AssetNode relativeParent = parent;

                IList<String> path = this.GetPathItems(item.Value.BifFileName.Value);

                //iterate through all but the last item
                Int32 index = 0;
                for (index = 0; index < (path.Count - 1); ++index)
                {
                    relativeParent = relativeParent.Children.Where(node => node.Name.ToLower() == path[index].ToLower()).FirstOrDefault();
                    if (relativeParent == null)
                        throw new ApplicationException(String.Format("Could not find the directory item (\"{0}\") under parent node (\"{1}\") within the asset node tree.", path[index], relativeParent.Name));
                }

                //Build an asset reference for the BIF itself
                //AssetLocation location = this.GetLocationsForBiff(item.Value.BifLocationFlags);
                //AssetReference biffAsset = new AssetReference(location, item.Value.BifFileName.Value);

                //add the last item, the BIF itself
                //AssetNode biffNode = new AssetNode(String.Format("{0} [Index {1}]", path[index], item.Key), biffAsset);
                AssetNode biffNode = new AssetNode(String.Format("{0} [Index {1}]", path[index], item.Key));
                relativeParent.Children.Add(biffNode);

                relativeParent = biffNode;

                //finally, populate the Biff node with the children.
                var children = assets.Where(asset => asset.Locator != null && asset.Locator.ResourceLocator != null && asset.Locator.ResourceLocator.BiffIndex == item.Key).OrderBy(asset => asset.AssetName).ThenBy(asset => asset.Locator.ResourceLocator.Locator);
                foreach (AssetReference asset in children)
                {
                    AssetNode assetInstance = new AssetNode(asset);
                    relativeParent.Children.Add(assetInstance);
                }
            }
        }

        /// <summary>Traverses the tree to find a destination point and returns it</summary>
        /// <param name="current">Starting point in the tree</param>
        /// <param name="path">Relative tree path to the destination node</param>
        /// <returns>The targeted node</returns>
        protected AssetNode TraverseToNode(AssetNode current, String path)
        {
            if (current == null)
                throw new ArgumentNullException("current", "The current node was unexpectedly null.");

            AssetNode target = current;

            IList<String> splitPath = this.GetPathItems(path);

            foreach (String subDir in splitPath)
            {
                AssetNode match = target.Children.Where(node => node.Name.ToLower() == subDir.ToLower()).FirstOrDefault();
                if (match == null)
                    throw new ApplicationException(String.Format("Could not find the target node within the tree; child node (\"{0}\") not found under parent (\"{1}\").", subDir, target.Name));

                target = match;
            }

            return target;
        }

        /// <summary>splits a file path into its hierarchy elements</summary>
        /// <param name="path">Path to split</param>
        /// <returns>The split path</returns>
        protected IList<String> GetPathItems(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path to split was unexpectedly null.");

            List<String> splitPath = new List<String>();

            //Allow for weird, unexpected stuff such as a key file with C:\data or \\server
            if (Path.IsPathRooted(path))
            {
                String root = Path.GetPathRoot(path);
                splitPath.Add(root);
                path = path.Substring(root.Length);
            }

            //break out the remaining subDirs
            splitPath.AddRange(path.Split(new Char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));

            return splitPath;
        }

        /// <summary>Creates the Application, HD0, etc. directories in the tree under the parent</summary>
        /// <param name="parent">Parent AssetNode whose children collection will have the location nodes added</param>
        protected void PopulateAssetTree_CreateLocationNodes(AssetNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("The parent node was unexpectedly null.");
            else if (parent.Children == null)
                throw new InvalidOperationException("The parent node's Children collection was unexpectedly null.");
            else if (this.paths == null)
                throw new InvalidOperationException("The manager's paths were unexpectedly null.");

            if (this.paths.Application != null)
                parent.Children.Add(new AssetNode("App"));

            if (this.paths.HD0 != null)
                parent.Children.Add(new AssetNode("HD0"));

            if (this.paths.CD1 != null)
                parent.Children.Add(new AssetNode("CD1"));

            if (this.paths.CD2 != null)
                parent.Children.Add(new AssetNode("CD2"));

            if (this.paths.CD3 != null)
                parent.Children.Add(new AssetNode("CD3"));

            if (this.paths.CD4 != null)
                parent.Children.Add(new AssetNode("CD4"));

            if (this.paths.CD5 != null)
                parent.Children.Add(new AssetNode("CD5"));

            if (this.paths.CD6 != null)
                parent.Children.Add(new AssetNode("CD6"));
        }

        /// <summary>Creates the BIFF nodes and asset references underneath the parent with no concern for location</summary>
        /// <param name="parent">Parent node at which to populate the BIFF nodes</param>
        protected void PopulateAssetTree_CreateBiffNodesNoLocation(AssetNode parent)
        {
            //collect the biff entries for each of these BIFF assets
            Dictionary<Int32, KeyTableBifEntry> biffEntries = new Dictionary<Int32, KeyTableBifEntry>();
            for (Int32 index = 0; index < this.chitinKey.EntriesBif.Count; ++index)
                biffEntries[index] = this.chitinKey.EntriesBif[index];

            //iterate through BIFFs
            foreach (KeyValuePair<Int32, KeyTableBifEntry> item in biffEntries)
            {
                //Build an asset reference for the BIF itself
                AssetLocation location = this.GetLocationsForBiff(item.Value.BifLocationFlags);
                AssetReference biffAsset = new AssetReference(location, item.Value.BifFileName.Value);

                //add the last item, the BIF itself
                AssetNode biffNode = new AssetNode(biffAsset, String.Format("{0} [Index {1}]", item.Value.BifFileName.Value, item.Key));
                parent.Children.Add(biffNode);
            }
        }

        /// <summary>Creates the BIFF nodes and asset references underneath the parent with no concern for location</summary>
        /// <param name="parent">Parent node at which to populate the BIFF nodes</param>
        /// <param name="location">Location of the assets being added, for filtering the correct results</param>
        /// <param name="group">Flag indicating whether the added BIFFs need to be grouped underneath the parent</param>
        protected void PopulateAssetTree_CreateBiffNodesInLocation(AssetNode parent, AssetLocation location, Boolean group)
        {
            //collect the biff entries for each of these BIFF assets
            Dictionary<Int32, KeyTableBifEntry> biffEntries = new Dictionary<Int32, KeyTableBifEntry>();
            for (Int32 index = 0; index < this.chitinKey.EntriesBif.Count; ++index)
                biffEntries[index] = this.chitinKey.EntriesBif[index];

            //Determine a container for the BIFFs; a BIF group if group, otherwise the parent
            AssetNode biffContainerNode = null;
            if (group)
                biffContainerNode = new AssetNode("BIF");
            else
                biffContainerNode = parent;

            //iterate through BIFFs
            foreach (KeyValuePair<Int32, KeyTableBifEntry> item in biffEntries)
            {
                //Build an asset reference for the BIF itself
                AssetLocation biffLocation = this.GetLocationsForBiff(item.Value.BifLocationFlags);
                AssetNode relativeParent = biffContainerNode;

                if ((biffLocation & location) == location)
                {
                    AssetReference biffAsset = new AssetReference(location, item.Value.BifFileName.Value);

                    relativeParent = this.TraverseToNode(relativeParent, Path.GetDirectoryName(item.Value.BifFileName.Value));

                    //add the last item, the BIF itself
                    AssetNode biffNode = new AssetNode(biffAsset, String.Format("{0} [Index {1}]", item.Value.BifFileName.Value, item.Key));
                    relativeParent.Children.Add(biffNode);
                }
            }

            if (group)  //the container needs to be added
                parent.Children.Add(biffContainerNode);
        }

        /// <summary>Creates the BIFF nodes and asset references underneath the parent with no concern for location</summary>
        /// <param name="parent">Parent node at which to populate the BIFF nodes</param>
        protected void PopulateAssetTree_CreateBiffNodesInLocationWithGroup(AssetNode parent, AssetLocation location)
        {
            //collect the biff entries for each of these BIFF assets
            Dictionary<Int32, KeyTableBifEntry> biffEntries = new Dictionary<Int32, KeyTableBifEntry>();
            for (Int32 index = 0; index < this.chitinKey.EntriesBif.Count; ++index)
                biffEntries[index] = this.chitinKey.EntriesBif[index];

            AssetNode biffContainerNode = new AssetNode("BIF");

            //iterate through BIFFs
            foreach (KeyValuePair<Int32, KeyTableBifEntry> item in biffEntries)
            {
                //Build an asset reference for the BIF itself
                AssetLocation biffLocation = this.GetLocationsForBiff(item.Value.BifLocationFlags);

                if ((biffLocation & location) == location)
                {
                    AssetReference biffAsset = new AssetReference(location, item.Value.BifFileName.Value);

                    //add the last item, the BIF itself
                    AssetNode biffNode = new AssetNode(biffAsset, String.Format("{0} [Index {1}]", item.Value.BifFileName.Value, item.Key));
                    biffContainerNode.Children.Add(biffNode);
                }
            }

            parent.Children.Add(biffContainerNode);
        }

        /// <summary>Creates & inserts the BIFF nodes between</summary>
        /// <param name="parent">Parent in which to place the biff node</param>
        /// <param name="insertNode">Node to be inserted</param>
        /// <remarks>This method assumes that the children are already sorted alphabetically</remarks>
        protected void PopulateAssetTree_InsertNodeBetweenChildren(AssetNode parent, AssetNode insertNode)
        {
            Int32 insertIndex = 0;
            for (insertIndex = 0; insertIndex < insertNode.Children.Count; ++insertIndex)
                if (String.Compare(parent.Children[insertIndex].Name, insertNode.Name) > 0)
                    break;
            parent.Children.Insert(insertIndex, insertNode);
        }
        #endregion


        #region Specific asset exposure
        /// <summary>Gets the collection of languages for which a dialog.tlk file has been found</summary>
        /// <returns>The collection of languages for which support has been found</returns>
        /// <remarks>
        ///     The languages that I have seen in (vanilla) Infinity Engine games are:
        ///     English (0), French (1), Spanish (2), German (3) and Italian. Italian has a bizarre language ID;
        ///     I can only assume that this needs to be some sort of mask
        /// </remarks>
        public IList<LanguageCode> ListLanguagesSupported()
        {
            return new LanguageCode[] { this.foundLanguage };
        }

        /// <summary>Gets the asset for a text location key (dialog.tlk) for the specified language</summary>
        /// <param name="langauge">Language specified</param>
        /// <param name="gender">Gender being specified</param>
        /// <returns>A reference to the specified TLK asset</returns>
        public AssetReference GetLanguageTalk(LanguageCode langauge, LanguageGender gender = LanguageGender.Masculine)
        {
            if (langauge != this.foundLanguage)
                throw new InvalidOperationException(String.Format("The specified langauge ({0}) was not the language found while loading assets ({1}).", langauge.GetDescription(), this.foundLanguage.GetDescription()));

            String target = "dialog.tlk";
            if (gender == LanguageGender.Feminine)
                target = "dialogf.tlk";

            AssetReference asset = this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].Where(a => a.AssetName.ToLower() == target).FirstOrDefault();

            return asset;
        }

        /// <summary>Lists the saves found under the specified save directory</summary>
        /// <param name="saveType">Save directory specified (save, mpsave, etc.)</param>
        /// <returns>The collection of folders assocaited with this save type</returns>
        public IList<SaveFolder> ListSaves(String saveType)
        {
            IList<SaveFolder> saveCollection = null;

            if (this.saves.ContainsKey(saveType))
                saveCollection = this.saves[saveType];

            return saveCollection;
        }

        /// <summary>Adds a save to the asset collection</summary>
        /// <param name="saveType">Type of save (mpsave, save, bpsave, mpbpsave, etc.) to add</param>
        /// <param name="save">Save folder to add</param>
        public void AddSave(String saveType, SaveFolder save)
        {
            //if no save collection previously existed, create it (new multiplayer or single player save, for example)
            if (!this.saves.ContainsKey(saveType))
                this.saves[saveType] = new List<SaveFolder>();

            //add the folder
            this.saves[saveType].Add(save);

            //do not set the assets to dirty, since saves are not put in the override tree.
        }

        /// <summary>Removes a save from the asset collections</summary>
        /// <param name="saveType">Type of save (mpsave, save, bpsave, mpbpsave, etc.) to add</param>
        /// <param name="save">Name of the save folder to remove</param>
        public void RemoveSave(String saveType, String saveName)
        {
            if (!this.saves.ContainsKey(saveType))
                throw new InvalidOperationException(String.Format("The specified type (\"{0}\") does not currently exist."));

            //replace the save folder if it already exists
            IEnumerable<SaveFolder> existingFolders = this.saves[saveType].Where(f => f.SaveFolderName == saveName);

            if (existingFolders.Count() < 1)
                throw new InvalidOperationException(String.Format("The save name (\"{0}\") was not found to be removed.", saveName));

            foreach (SaveFolder folder in existingFolders)
                this.saves[saveType].Remove(folder);

            //do not set the assets to dirty, since saves are not put in the override tree.
        }

        /// <summary>Lists the exported characters located</summary>
        /// <returns>The collection of exported characters located</returns>
        public IList<AssetReference> ListCharacters()
        {
            return this.OverriddenAssets.Where(pair => pair.Value.AssetType.ToLower() == "chr").Select(pair => pair.Value).ToList();
        }

        /// <summary>Adds a character exported to the asset collection</summary>
        /// <param name="character">Character asset to add</param>
        /// <param name="biography">Optional character biography asset to add</param>
        public void AddCharacter(AssetReference character, AssetReference biography = null)
        {
            this.AddCharacterAsset(character);

            if (biography != null)
                this.AddCharacterAsset(biography);
        }

        /// <summary>Adds the specified asset to the assets collection</summary>
        /// <param name="asset">Asset to add</param>
        protected void AddCharacterAsset(AssetReference asset)
        {
            //error checking
            if (asset == null)
                throw new ArgumentNullException("asset", "The asset being added was unexpectedly null.");
            else if (asset.Locator == null)
                throw new ArgumentNullException("asset.Locator", "The asset being added did not have a valid locator.");
            else if (asset.Locator.RelativePath == null)
                throw new InvalidOperationException("The asset being added did not have a relative filesystem path. This method is used to add a file system asset.");

            String directory = Path.GetDirectoryName(asset.Locator.RelativePath).ToLower();
            if (directory != InfinityAssetManager.StringCharacters && directory != InfinityAssetManager.StringOverride && directory != InfinityAssetManager.StringPortraits && directory != InfinityAssetManager.StringScripts && directory != InfinityAssetManager.StringSounds)
                throw new InvalidOperationException(String.Format("The realtive path specified (\"{0}\") does not have a path matching any known override directory.", asset.Locator.RelativePath));

            //if exporting your first character, or what have you, create the asset collection specified
            if (!this.assetsByContainer.ContainsKey(directory))
                this.assetsByContainer[directory] = new List<AssetReference>();

            this.assetsByContainer[directory].Add(asset);

            //the override hierarchy has now changed
            this.dirty = true;
        }

        /// <summary>Removes a character exported from the asset collection</summary>
        /// <param name="character">Character asset to remove</param>
        /// <param name="biography">Optional character biography asset to remove</param>
        public void RemoveCharacter(AssetReference character, AssetReference biography = null)
        {
            this.RemoveCharacterAsset(character);

            if (biography != null)
                this.RemoveCharacterAsset(biography);
        }

        /// <summary>Removes the specified asset from the assets collection</summary>
        /// <param name="asset">Asset to remove</param>
        protected void RemoveCharacterAsset(AssetReference asset)
        {
            //error checking
            if (asset == null)
                throw new ArgumentNullException("asset", "The asset being added was unexpectedly null.");
            else if (asset.Locator == null)
                throw new ArgumentNullException("asset.Locator", "The asset being added did not have a valid locator.");
            else if (asset.Locator.RelativePath == null)
                throw new InvalidOperationException("The asset being added did not have a relative filesystem path. This method is used to add a file system asset.");

            String directory = Path.GetDirectoryName(asset.Locator.RelativePath).ToLower();
            if (directory != InfinityAssetManager.StringCharacters && directory != InfinityAssetManager.StringOverride && directory != InfinityAssetManager.StringPortraits && directory != InfinityAssetManager.StringScripts && directory != InfinityAssetManager.StringSounds)
                throw new InvalidOperationException(String.Format("The asset's realtive path specified (\"{0}\") does not have a path matching any known override directory.", asset.Locator.RelativePath));

            IEnumerable<AssetReference> assetsFound = this.assetsByContainer[directory].Where(a => a.Locator.RelativePath == asset.Locator.RelativePath);

            if (assetsFound.Count() < 1)
                throw new InvalidOperationException(String.Format("The realtive path specified (\"{0}\") does not have a matching asset in any known override directory."));

            foreach (AssetReference reference in assetsFound)
                this.assetsByContainer[directory].Remove(reference);

            //the override hierarchy has now changed
            this.dirty = true;
        }
        #endregion


        #region Key & BIF archive operations
        /// <summary>Adds the contents of a directory to the specified key file</summary>
        /// <param name="keyFile">Name of the key file to add assets in a BIF to</param>
        /// <param name="location">Locations (HD0, CD1, etc.) indicating where the BIFF should be written</param>
        /// <param name="directoryPath">Path of the source directory to create a BIF archive from</param>
        /// <param name="targetDirectory">Target directory in which to place the created BIF file (data, movies, etc.)</param>
        /// <param name="overrides">Flag indicating whether the these assets will override current assets being managed</param>
        public void CreateBiffFromDirectory(String keyFile, KeyTableBifLocationEnum location, String directoryPath, String targetDirectory, Boolean overrides = false)
        {
            if (keyFile == null)
                throw new ArgumentNullException("keyFile", "The specified key file was unexpectedly null.");
            else if (directoryPath == null)
                throw new ArgumentNullException("directoryPath", "The specified directory Path was unexpectedly null.");
            else if (directoryPath == null)
                throw new ArgumentNullException("targetDirectory", "target directory was unexpectedly null.");
            else if (keyFile.ToLower() != InfinityAssetManager.StringKeyFile)
                throw new InvalidOperationException("Chitin.key is the only valid key file for the infinity engine.");
            else if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(String.Format("Could not find the specified directory (\"{0}\").", directoryPath));
            else if (!Directory.Exists(targetDirectory))
                throw new DirectoryNotFoundException(String.Format("Could not find the specified directory (\"{0}\").", targetDirectory));
            else if (location == KeyTableBifLocationEnum.None)
                throw new InvalidOperationException("The operation requested to add a BIF in no locations, thus there is no point in attempting it.");

            String[] files = Directory.GetFiles(directoryPath);

            if (files.Length == 0)
                throw new InvalidOperationException("No files found to add to archive.");

            //create a new archive in memory
            Biff1Archive archive = new Biff1Archive(true);
            archive.Initialize();

            //get the BIFF index
            Int32 biffIndex = this.chitinKey.EntriesBif.Count;  //count = next entry
            String biffName = String.Concat(Path.GetDirectoryName(directoryPath), ".bif");
            String describedLocation = Path.Combine(this.GetContainingLocationsForKey(location), targetDirectory, biffName);

            //create a collection to add to the combined resource pool after the artchive has been written
            List<KeyTableResourceEntry> resourcesToAdd = new List<KeyTableResourceEntry>();

            //add each file in the directory
            foreach (String file in files)
            {
                //.NET cannot easily get the 8.3 filename, so just push the onus back onto the user.
                if (file.Length > 8)
                    throw new InvalidOperationException(String.Format("The file name (\"{0}\") is too long. It must be 8 characters.", file));

                //get the filename without its extension
                String fileNoExtension = file.Replace(Path.GetExtension(file), String.Empty);

                using (FileStream stream = ReusableIO.OpenFile(Path.Combine(directoryPath, file)))
                {
                    Byte[] data = ReusableIO.BinaryRead(stream, stream.Length);
                    if (file.ToLower().EndsWith(".tis"))
                    {
                        ReusableIO.SeekIfAble(stream, 0);

                        //create a new TIS from this to get the header info
                        TileSet1 tileset = new TileSet1();
                        tileset.Initialize();
                        tileset.ReadHeader(stream);

                        //build the locator
                        ResourceLocator1 locator = new ResourceLocator1();
                        locator.BiffIndex = Convert.ToUInt32(biffIndex);
                        locator.TilesetIndex = Convert.ToUInt32(archive.EntriesTileset.Count);

                        //build the tileset entry
                        Biff1TilesetEntry tilesetEntry = new Biff1TilesetEntry();
                        tilesetEntry.Initialize();
                        tilesetEntry.ResourceLocator = locator;
                        tilesetEntry.SizeResource = Convert.ToUInt32(data.LongLength);
                        tilesetEntry.TypeResource = ResourceType.Tileset;
                        tilesetEntry.CountTile = Convert.ToUInt32(tileset.FrameCount);
                        tilesetEntry.SizeResource = Biff1TilesetEntry.TileSize;
                        archive.EntriesTileset.Add(tilesetEntry);

                        //add the tileset data, and increment its counter
                        archive.DataTileset.Add(data);
                        archive.Header.CountTileset = archive.Header.CountTileset + 1;

                        //build the chitin.key entry
                        KeyTableResourceEntry resource = new KeyTableResourceEntry();
                        resource.Initialize();
                        resource.ResourceName = ZString.FromString(fileNoExtension);
                        resource.ResourceLocator = locator;
                        resource.ResourceType = ResourceType.Tileset;

                        //add the resource to chitin.key
                        this.chitinKey.EntriesResource.Add(resource);

                        //add this resource to the combined resource pool
                        resourcesToAdd.Add(resource);
                    }
                    else
                    {
                        //build the locator
                        ResourceLocator1 locator = new ResourceLocator1();
                        locator.BiffIndex = Convert.ToUInt32(biffIndex);
                        locator.ResourceIndex = Convert.ToUInt32(archive.EntriesResource.Count);

                        Biff1ResourceEntry entry = new Biff1ResourceEntry();
                        entry.Initialize();
                        entry.ResourceLocator = locator;
                        entry.SizeResource = Convert.ToUInt32(data.LongLength);
                        entry.TypeResource = ResourceTypeExtender.FromExtension(Path.GetExtension(file));

                        //add the tileset data, and increment its counter
                        archive.DataResource.Add(data);
                        archive.Header.CountResource = archive.Header.CountResource + 1;

                        //build the chitin.key entry
                        KeyTableResourceEntry resource = new KeyTableResourceEntry();
                        resource.Initialize();
                        resource.ResourceName = ZString.FromString(fileNoExtension);
                        resource.ResourceLocator = locator;
                        resource.ResourceType = ResourceType.Tileset;

                        //add the resource to chitin.key
                        this.chitinKey.EntriesResource.Add(resource);

                        //add this resource to the combined resource pool
                        resourcesToAdd.Add(resource);
                    }
                }
            }

            //write the BIFF in each location specified
            Int64 size = 0; //the size should be the same in all instances.
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.HardDrive))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.HD0, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc1))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD1, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc2))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD2, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc3))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD3, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc4))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD4, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc5))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD5, biffName);
            if (location == (KeyTableBifLocationEnum.HardDrive & KeyTableBifLocationEnum.Disc6))
                size = this.WriteBiffToLocation(archive, targetDirectory, this.paths.CD6, biffName);

            //create a bif entry
            DirectoryInfo biffedDirectory = new DirectoryInfo(directoryPath);

            KeyTableBifEntry biffEntry = new KeyTableBifEntry();
            biffEntry.Initialize();
            biffEntry.BifLocationFlags = location;
            biffEntry.BifFileName = ZString.FromString(String.Format("{0}\\{1}.bif", targetDirectory, biffedDirectory.Name));
            biffEntry.LengthBifFile = Convert.ToUInt32(size);

            foreach (KeyTableResourceEntry resource in resourcesToAdd)
            {
                AssetReference asset = new AssetReference(InfinityAssetManager.StringKeyAssetsCollection, resource, biffEntry);
                this.assetsByContainer[InfinityAssetManager.StringKeyAssetsCollection].Add(asset);
            }

            //since an asset was added, the override tree needs to be updated
            this.dirty = true;
        }

        /// <summary>Writes the BIFF archive to the specified location</summary>
        /// <param name="archive">Biff archive to write</param>
        /// <param name="subDirectory">Subdirectory in the specified location to write (data, movies, etc.)</param>
        /// <param name="destination">The path of the IE key location target (CD1, CD2, HD0, etc.)</param>
        /// <param name="archiveName">Name of the archive (excluding its extension) to create</param>
        /// <returns>The size of the written BIFF</returns>
        protected Int64 WriteBiffToLocation(Biff1Archive archive, String subDirectory, String destination, String archiveName)
        {
            String destPath = Path.Combine(destination, subDirectory);

            //create the directory
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            //write the Biff
            Int64 size = 0;
            using (FileStream output = ReusableIO.OpenFile(Path.Combine(destPath, archiveName), FileMode.Create, FileAccess.Write))
            {
                archive.Write(output);
                output.Flush();
                size = output.Length;
            }

            return size;
        }

        /// <summary>Removes an archive from the specified key file</summary>
        /// <param name="keyFile">Key file to remove an archive from</param>
        /// <param name="location">Locations (HD0, CD1, etc.) indicating where the BIFF should be removed</param>
        /// <param name="biffIndex">Index of the archive to remove</param>
        public void RemoveArchive(String keyFile, KeyTableBifLocationEnum location, Int32 biffIndex)
        {
            if (keyFile == null)
                throw new ArgumentNullException("keyFile", "The specified key file was unexpectedly null.");
            else if (keyFile.ToLower() != InfinityAssetManager.StringKeyFile)
                throw new InvalidOperationException("Chitin.key is the only valid key file for the infinity engine.");
            else if (location == KeyTableBifLocationEnum.None)
                throw new InvalidOperationException("The locations to remove are empty and therefore no operation to be performed");
            else if (biffIndex < 0)
                throw new ArgumentOutOfRangeException("biffIndex", "The BIFF index is zero-based and must be greater than or equal to 0.");
            else if (biffIndex >= this.chitinKey.EntriesBif.Count)
                throw new ArgumentOutOfRangeException("biffIndex", "The BIFF index exceeds the current BIFF entry count and is out of its range.");

            Boolean deleted = false;

            //unset the locations specified
            KeyTableBifLocationEnum current = this.chitinKey.EntriesBif[biffIndex].BifLocationFlags;
            current = current & (KeyTableBifLocationEnum.MaskAll ^ location);
            this.chitinKey.EntriesBif[biffIndex].BifLocationFlags = current;

            //if the BIF no longer exists in any location, delete it
            if (current == KeyTableBifLocationEnum.None)
            {
                this.chitinKey.EntriesBif.RemoveAt(biffIndex);
                deleted = true;
            }

            if (deleted)
            {
                //remove references to this index
                for (Int32 entry = 0; entry < this.chitinKey.EntriesResource.Count; ++entry)
                {
                    if (this.chitinKey.EntriesResource[entry].ResourceLocator.BiffIndex == biffIndex)
                    {
                        this.chitinKey.EntriesResource.RemoveAt(entry);
                        entry--;

                        //remove the related contained resource
                    }
                }

                //update existing resource indexes to account for the lowered index
                foreach (KeyTableResourceEntry entry in this.chitinKey.EntriesResource)
                {
                    if (entry.ResourceLocator.BiffIndex > biffIndex)
                        entry.ResourceLocator.BiffIndex--;
                }

                //since assets were removed, the override tree needs to be updated
                this.dirty = true;
            }
        }

        /// <summary>Saves the existing key file and any changes made to it</summary>
        /// <param name="keyFile">Name of the key file to save</param>
        public void SaveKey(String keyFile)
        {
            if (keyFile == null)
                throw new ArgumentNullException("keyFile", "The specified key file was unexpectedly null.");
            else if (keyFile.ToLower() != InfinityAssetManager.StringKeyFile)
                throw new InvalidOperationException("Chitin.key is the only valid key file for the infinity engine.");

            using (FileStream output = ReusableIO.OpenFile(Path.Combine(this.paths.Application, InfinityAssetManager.StringKeyFile), FileMode.Create, FileAccess.Write))
                this.chitinKey.Write(output);
        }

        /// <summary>Adds a file to the specified archive</summary>
        /// <param name="keyFile">Name of the key file in which to record the addition</param>
        /// <param name="biffIndex">Index of the key's archive to open and add to</param>
        /// <param name="filePath">Path of the source file to add to the archive and key files</param>
        public void AddAssetToArchive(String keyFile, Int32 biffIndex, String filePath)
        {
            if (keyFile == null)
                throw new ArgumentNullException("keyFile", "The specified key file was unexpectedly null.");
            else if (keyFile.ToLower() != InfinityAssetManager.StringKeyFile)
                throw new InvalidOperationException("Chitin.key is the only valid key file for the infinity engine.");
            else if (biffIndex < 0)
                throw new InvalidOperationException("The BIFF index is zero-based and must be greater than or equal to 0.");
            else if (biffIndex >= this.chitinKey.EntriesBif.Count)
                throw new ArgumentOutOfRangeException("biffIndex", "The BIFF index exceeds the current BIFF entry count and is out of its range.");
            else if (filePath == null)
                throw new ArgumentNullException("filePath", "The source file path was unexpectedly null.");
            else if (!File.Exists(filePath))
                throw new FileNotFoundException(String.Format("Could not locate the specified file (\"{0}\").", filePath), filePath);

            //TODO: I need to detect what kind of BIF asset is referenced. 
            //  This implies the need for the IBiff interface to expand a bit, exposing the headers and the resources, etc.
            throw new NotImplementedException("I need to expand the IBiff interface first, but my edit stack is already off the expected course.");
        }

        /// <summary>Removes an asset from the specified key file. A flag is available to remove from the containing archive.</summary>
        /// <param name="keyFile">Name of the key file to remove the asset from</param>
        /// <param name="location">Targeted asset to remove from a key file</param>
        /// <param name="removeFromArchive">Flag indicating whether to remove the file from the housing archive</param>
        public void RemoveAssetFromKey(String keyFile, ResourceLocator1 locator, Boolean removeFromArchive = false)
        {
            if (keyFile == null)
                throw new ArgumentNullException("keyFile", "The specified key file was unexpectedly null.");
            else if (keyFile.ToLower() != InfinityAssetManager.StringKeyFile)
                throw new InvalidOperationException("Chitin.key is the only valid key file for the infinity engine.");
            else if (locator == null)
                throw new ArgumentNullException("locator", "The locator provided was unexpectedly null.");

            //remove from chitin
            IEnumerable<KeyTableResourceEntry> resources = this.chitinKey.EntriesResource.Where(resource => resource.Equals(locator));
            foreach (KeyTableResourceEntry resource in resources)
                this.chitinKey.EntriesResource.Remove(resource);
                
            if (removeFromArchive)
            {
                //TODO: I need to detect what kind of BIF asset is referenced. 
                //  This implies the need for the IBiff interface to expand a bit, exposing the headers and the resources, etc.
                throw new NotImplementedException("I need to expand the IBiff interface first, but my edit stack is already off the expected course.");
            }
        }
        #endregion


        #region Informational Displays
        /// <summary>Gets the type of engine that this asset manager exposes</summary>
        /// <returns>The type of engine that this asset manager exposes</returns>
        public GameEngine GetGameEngine()
        {
            //The easy thing to do, here, would be to just go off of the ini file.
            //  However, BG & BG2 violate this approach, sharing baldur.ini
            //Q: how does NI examine the game engine? A: it does exactly what I was about to do,
            //  look for *.exe files. But, I don't want that approach. EXEs are PC-only, for example.
            //  What about assets? BG:Tutu, BGT; won't work, I should know.
            //  how about BIFFs? I cut those out of the resources collection.
            //  What asset is in BG2 that would never be in BG1? Look in the SoA demo.
            //My best answer is to use a BG2 asset that BG would never use, not the other way 'round.
            //BG does not support wfx, how about one of those...

            GameEngine engine;

            if (this.OverriddenAssets.ContainsKey("torment.ini"))                       //PST
                engine = GameEngine.PlanescapeTorment;
            else if (this.OverriddenAssets.ContainsKey("icewind2.ini"))                 //IWD2
                engine = GameEngine.IcewindDale2;
            else if (this.OverriddenAssets.ContainsKey("icewind.ini"))                  //IWD
                engine = GameEngine.IcewindDale;
            else if (this.OverriddenAssets.ContainsKey("eff_m21.wfx"))                  //BG2
                engine = GameEngine.BaldursGate2;
            else //if (this.OverriddenAssets.ContainsKey("baldur.ini"))                 //BG
                engine = GameEngine.BaldursGate;

            return engine;
        }

        /// <summary>Gets the game installation instance that this asset manager currently exposes</summary>
        /// <returns>The game installation instance that this asset manager currently exposes</returns>
        public GameInstall GetGameInstall()
        {
            GameInstall game = GameInstall.Unknown;

            //This, I think, I can reliably use assets on

            //PS:T
            if (this.OverriddenAssets.ContainsKey("torment.ini"))
                game = GameInstall.PlanescapeTorment;

            //IWD2
            else if (this.OverriddenAssets.ContainsKey("icewind2.ini"))
                game = GameInstall.IcewindDale2;

            //IWD:TotL; zARE.bif?
            else if (this.OverriddenAssets.ContainsKey("icewind.ini") && this.OverriddenAssets.ContainsKey("ar9700.are"))
                game = GameInstall.TrialsOfTheLuremaster;
            //IWD:HoW; Icasaracht in ar9604
            else if (this.OverriddenAssets.ContainsKey("icewind.ini") && this.OverriddenAssets.ContainsKey("ar9604.are"))
                game = GameInstall.HeartOfWinter;
            //IWD
            else if (this.OverriddenAssets.ContainsKey("icewind.ini"))
                game = GameInstall.IcewindDale;

            //BG2: BGT; I hate my old naming convention from being a stupid teenager
            else if (this.OverriddenAssets.ContainsKey("eff_m21.wfx") && this.OverriddenAssets.ContainsKey("arwo00.are"))
                game = GameInstall.BaldursGateTrilogy;
            //BG2: ToB; 25Store.bif file
            else if (this.OverriddenAssets.ContainsKey("eff_m21.wfx") && this.OverriddenAssets.ContainsKey("25spell.sto"))
                game = GameInstall.ThroneOfBhaal;
            //BG2 proper; flythrough movies
            else if (this.OverriddenAssets.ContainsKey("eff_m21.wfx") && this.OverriddenAssets.ContainsKey("flythr01.mve"))
                game = GameInstall.ShadowsOfAmn;
            //BG2 demo
            else if (this.OverriddenAssets.ContainsKey("eff_m21.wfx"))
                game = GameInstall.BaldursGate2Demo;

            //Baldur's Gate: Tales of the Sword Coast
            else if (this.OverriddenAssets.ContainsKey("baldur.ini") && this.OverriddenAssets.ContainsKey("wreck.mve"))
                game = GameInstall.BaldursGate;
            //Baldur's Gate
            else if (this.OverriddenAssets.ContainsKey("baldur.ini") && this.OverriddenAssets.ContainsKey("bhaal.mve"))
                game = GameInstall.BaldursGate;
            //Baldur's Gate Abridged, Chapters 1 & 2
            else if (this.OverriddenAssets.ContainsKey("baldur.ini"))
                game = GameInstall.BaldursGate;


            return game;
        }

        /// <summary>Gets the install application directory</summary>
        /// <returns>The install application directory</returns>
        public String GetApplicationDirectory()
        {
            return this.paths.Application;
        }

        /// <summary>Gets the related User directory</summary>
        /// <returns>The related User directory</returns>
        public String GetUserDirectory()
        {
            return null;
        }
        #endregion


        #region Override asset calculation
        /// <summary>Iterates through the assets located and creates an override collection of these assets</summary>
        protected void BuildOverrideAssetTree()
        {
            //initialize the collection
            this.overriddenAssets = new Dictionary<String, AssetReference>();
            
            //grab all chitin.key resources. order them by biff index, then resource locator
            foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringKeyAssetsCollection].OrderBy(a => a.Locator.ResourceLocator.BiffIndex).ThenBy(a => a.Locator.ResourceLocator.Locator))
                this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            //next, override everything in the order of the override directories. Order them by their resource name.
            if (this.assetsByContainer.Keys.Contains(InfinityAssetManager.StringOverride))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringOverride].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            if (this.assetsByContainer.Keys.Contains(InfinityAssetManager.StringScripts))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringScripts].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            if (this.assetsByContainer.Keys.Contains(InfinityAssetManager.StringSounds))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringSounds].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            if (this.assetsByContainer.Keys.Contains(InfinityAssetManager.StringPortraits))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringPortraits].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            if (this.assetsByContainer.Keys.Contains(InfinityAssetManager.StringCharacters))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringCharacters].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            //music is treated in this odd way, where only direct references to the music are made.
            //  "music\Theme\Themea.acm" is only ever referenced, and "music\BC1\Themea.acm" is never loaded.
            if (this.assetsByContainer.ContainsKey(InfinityAssetManager.StringMusic))
                foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringMusic].OrderBy(a => a.AssetName))
                    this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            //BIFFs cannot be overridden, so do not even try.

            foreach (AssetReference asset in this.assetsByContainer[InfinityAssetManager.StringNotOverridableAsset].OrderBy(a => a.AssetName))
                this.overriddenAssets[asset.AssetName.ToLower()] = asset;

            this.dirty = false;
        }
        #endregion


        #region Generic helper methods
        /// <summary>Indicates whether an extension is known</summary>
        /// <param name="extension">File extension to identify</param>
        /// <returns>Flag indicating whether the extension is recognized or not</returns>
        protected Boolean IsKnownExtension(String extension)
        {
            Boolean knownExtension = false;

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

            return knownExtension;
        }

        /// <summary>Gets the containing locations for a BIF archive</summary>
        /// <param name="locations">The locations of a BIF archive</param>
        /// <returns>The descriptive locations of a BIF archive</returns>
        protected String GetContainingLocationsForKey(KeyTableBifLocationEnum locations)
        {
            List<String> keyLocations = new List<String>();

            if ((locations & KeyTableBifLocationEnum.HardDrive) == KeyTableBifLocationEnum.HardDrive)
                keyLocations.Add(KeyTableBifLocationEnum.HardDrive.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc1) == KeyTableBifLocationEnum.Disc1)
                keyLocations.Add(KeyTableBifLocationEnum.Disc1.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc2) == KeyTableBifLocationEnum.Disc2)
                keyLocations.Add(KeyTableBifLocationEnum.Disc2.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc3) == KeyTableBifLocationEnum.Disc3)
                keyLocations.Add(KeyTableBifLocationEnum.Disc3.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc4) == KeyTableBifLocationEnum.Disc4)
                keyLocations.Add(KeyTableBifLocationEnum.Disc4.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc5) == KeyTableBifLocationEnum.Disc5)
                keyLocations.Add(KeyTableBifLocationEnum.Disc5.GetShortName());
            else if ((locations & KeyTableBifLocationEnum.Disc6) == KeyTableBifLocationEnum.Disc6)
                keyLocations.Add(KeyTableBifLocationEnum.Disc6.GetShortName());

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

            return null;
        }

        /// <summary>This method checks to see if a file exists in the location specified.</summary>
        /// <param name="path">The path string from an *.ini file; it can, perhaps, have multiple entries</param>
        /// <param name="relativeRequest">Path of the file to locate</param>
        /// <returns>True if found; false otherwise</returns>
        protected Boolean FileExistsInLocation(String path, String relativeRequest)
        {
            Boolean found = false;

            String[] paths = path.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String specifiedLocation in paths)
            {
                if (File.Exists(Path.Combine(specifiedLocation, relativeRequest)))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>This method checks to see if a directory exists in the location specified.</summary>
        /// <param name="path">The path string from an *.ini file; it can, perhaps, have multiple entries</param>
        /// <param name="relativeRequest">Path of the file to locate</param>
        /// <returns>True if found; false otherwise</returns>
        protected Boolean DirectoryExistsInLocation(String path, String relativeRequest)
        {
            Boolean found = false;

            String[] paths = path.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String specifiedLocation in paths)
            {
                if (Directory.Exists(Path.Combine(specifiedLocation, relativeRequest)))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>Gets the path to the requested file</summary>
        /// <param name="location">Location directory provided by *.ini file</param>
        /// <param name="relativeRequest">Path of the file being requested</param>
        /// <returns>The file system path to the requested file</returns>
        protected String GetFilePathForLocation(String location, String relativeRequest)
        {
            String path = null;

            //the ini file (for BG2, at least) supports two locations for HD0, CD1, etc. If one is bogus, then it will
            //  work by searching the second location. The order of this precedence is that the first path is tried first,
            //  and the next path on failure. If a previous item has been found, a second will not be looked for.
            //  Dialog.tlk and Dialogf.tlk do not have any impact on this behavior. The paths are not tried in reverse.
            String[] paths = location.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String specifiedLocation in paths)
            {
                String combinedPath = Path.Combine(specifiedLocation, relativeRequest);
                if (File.Exists(combinedPath))
                {
                    FileInfo onDisk = new FileInfo(combinedPath);
                    path = onDisk.FullName;
                    break;
                }
            }

            return path;
        }

        /// <summary>Gets the path to the requested file</summary>
        /// <param name="location">Location directory provided by *.ini file</param>
        /// <param name="relativeRequest">Path of the file being requested</param>
        /// <returns>The file system path to the requested file</returns>
        protected String GetFilePathForLocation(AssetLocation location, String relativeRequest)
        {
            String path = null;

            if (location == AssetLocation.HD0)
                path = this.paths.HD0;
            else if (location == AssetLocation.CD1)
                path = this.paths.CD1;
            else if (location == AssetLocation.CD2)
                path = this.paths.CD2;
            else if (location == AssetLocation.CD3)
                path = this.paths.CD3;
            else if (location == AssetLocation.CD4)
                path = this.paths.CD4;
            else if (location == AssetLocation.CD5)
                path = this.paths.CD5;
            else if (location == AssetLocation.CD6)
                path = this.paths.CD6;
            else
                throw new InvalidOperationException(String.Format("Cannot convert the specified location (\"{0}\") to a directory.", location.GetDescription()));

            return this.GetFilePathForLocation(path, relativeRequest);
        }

        /// <summary>Gets the path to the requested directory</summary>
        /// <param name="location">Location directory provided by *.ini file</param>
        /// <param name="relativeRequest">Path of the directory being requested</param>
        /// <returns>The file system path to the requested file</returns>
        protected String GetDirectoryPathForLocation(String location, String relativeRequest)
        {
            String path = null;

            String[] paths = location.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String specifiedLocation in paths)
            {
                String combinedPath = Path.Combine(specifiedLocation, relativeRequest);
                if (Directory.Exists(combinedPath))
                {
                    DirectoryInfo onDisk = new DirectoryInfo(combinedPath);
                    path = onDisk.FullName;
                    break;
                }
            }

            return path;
        }

        /// <summary>Creates a collection of AssetLocation items indicating the locations for a BIFF archive</summary>
        /// <param name="locations">BIFF locations specified within a key entry</param>
        /// <returns>The collection of AssetLocation items indicating the locations for a BIFF archive</returns>
        protected AssetLocation GetLocationsForBiff(KeyTableBifLocationEnum locations)
        {
            AssetLocation assetLocations = AssetLocation.LookUp;    //none

            if ((locations & KeyTableBifLocationEnum.HardDrive) == KeyTableBifLocationEnum.HardDrive)
                assetLocations |= AssetLocation.HD0;

            if ((locations & KeyTableBifLocationEnum.Disc1) == KeyTableBifLocationEnum.Disc1)
                assetLocations |= AssetLocation.CD1;

            if ((locations & KeyTableBifLocationEnum.Disc2) == KeyTableBifLocationEnum.Disc2)
                assetLocations |= AssetLocation.CD2;

            if ((locations & KeyTableBifLocationEnum.Disc3) == KeyTableBifLocationEnum.Disc3)
                assetLocations |= AssetLocation.CD3;

            if ((locations & KeyTableBifLocationEnum.Disc4) == KeyTableBifLocationEnum.Disc4)
                assetLocations |= AssetLocation.CD4;

            if ((locations & KeyTableBifLocationEnum.Disc5) == KeyTableBifLocationEnum.Disc5)
                assetLocations |= AssetLocation.CD5;

            if ((locations & KeyTableBifLocationEnum.Disc6) == KeyTableBifLocationEnum.Disc6)
                assetLocations |= AssetLocation.CD6;

            return assetLocations;
        }

        /// <summary>Determines whether the specified asset is an override folder asset</summary>
        /// <param name="locator">AssetLocator to evaluate</param>
        /// <returns>True if the relative path indicates an override foler asset</returns>
        protected Boolean IsOverrideAsset(AssetLocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException("locator", "The asset locator provided was unexpectedly null.");

            Boolean overrideAsset = false;

            if (locator.Location == AssetLocation.ApplicationDirectory && locator.RelativePath != null)
            {
                overrideAsset = (
                        locator.RelativePath.StartsWith(InfinityAssetManager.StringOverride) ||
                        locator.RelativePath.StartsWith(InfinityAssetManager.StringCharacters) ||
                        locator.RelativePath.StartsWith(InfinityAssetManager.StringPortraits) ||
                        locator.RelativePath.StartsWith(InfinityAssetManager.StringScripts) ||
                        locator.RelativePath.StartsWith(InfinityAssetManager.StringSounds));
            }

            return overrideAsset;
        }
        #endregion
    }
}