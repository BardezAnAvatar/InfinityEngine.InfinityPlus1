using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable;
using Bardez.Projects.InfinityPlus1.Information;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets
{
    /// <summary>Interface for an asset manager, which lists and extracts asset streams</summary>
    public interface IAssetManager
    {
        #region Asset retrieval
        /// <summary>Gets a binary stream containing the asset requested</summary>
        /// <param name="filename">Asset name and file extension</param>
        /// <returns>A binary stream containing the asset</returns>
        /// <remarks>This would be used for items, spells, etc.</remarks>
        Stream GetAsset(String filename);

        /// <summary>Gets a binary stream containing the asset requested</summary>
        /// <param name="resource">The resource reference being requested (filename without extension; extension inferred from resource type)</param>
        /// <returns>A binary stream containing the asset</returns>
        /// <remarks>This would be used for items, spells, etc.</remarks>
        Stream GetAsset(ResourceReference resource);

        /// <summary>Gets a binary stream containing the specific instance of the asset requested</summary>
        /// <param name="location">Location structure that defines the location of an asset's instance</param>
        /// <returns>A binary stream containing the instnace of the requested asset</returns>
        Stream GetAssetInstance(AssetLocator location);
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
        AssetNode GetAssetTree_GroupedOverridden();

        /// <summary>Gets the asset tree showing all instances</summary>
        /// <returns>The asset tree showing all instances of assets found, grouped by their extension</returns>
        /// <remarks>
        ///     Matches design view 2
        /// 
        ///     This shall be a root node with a TLK, DLG, ITM, STO, BIF, BMP, etc., with every instance of a files
        ///     matching that extension inside (i.e.: [2DA.Bif, 0xFF123400] and [override] shown
        /// </remarks>
        AssetNode GetAssetTree_GroupedAllInstances();

        /// <summary>Gets the asset tree showing all instances in their locations</summary>
        /// <returns>The asset tree showing all instances of assets found, grouped by their location</returns>
        /// <remarks>
        ///     Matches design view 3
        /// 
        ///     This shall be a root node with a location root (TLK, chitin.key, override, xp1.key, portraits)
        ///     with each instance of an asset listed within its location, duplicates shown
        ///     
        ///     Additional note: This view should also expose orphaned assets within a BIFF archive. If a BIFF
        ///     asset is not found in ANY key file, then it will be displayed under its BIF file as an orphaned
        ///     asset.
        /// </remarks>
        AssetNode GetAssetTree_LocationInstances();

        /// <summary>Gets the asset tree showing overridden instances, in their locations, grouped by type</summary>
        /// <returns>The asset tree showing overridden assets found, grouped by location and asset type</returns>
        /// <remarks>Matches design view 4</remarks>
        AssetNode GetAssetTree_LocationGroupedOverridden();
        #endregion


        #region Specific asset exposure
        /// <summary>Gets the collection of languages for which a dialog.tlk file has been found</summary>
        /// <returns>The collection of languages for which support has been found</returns>
        IList<LanguageCode> ListLanguagesSupported();

        /// <summary>Gets the asset for a text location key (dialog.tlk) for the specified language</summary>
        /// <param name="langauge">Language specified</param>
        /// <param name="gender">Gender being specified</param>
        /// <returns>A reference to the specified TLK asset</returns>
        AssetReference GetLanguageTalk(LanguageCode langauge, LanguageGender gender = LanguageGender.Masculine);

        /// <summary>Lists the saves found under the specified save directory</summary>
        /// <param name="saveType">Save directory specified (save, mpsave, etc.)</param>
        /// <returns>The collection of folders assocaited with this save type</returns>
        IList<SaveFolder> ListSaves(String saveType);

        /// <summary>Adds a save to the asset collections</summary>
        /// <param name="saveType">Type of save (mpsave, save, bpsave, mpbpsave, etc.) to add</param>
        /// <param name="save">Save folder to add</param>
        void AddSave(String saveType, SaveFolder save);

        /// <summary>Removes a save from the asset collections</summary>
        /// <param name="saveType">Type of save (mpsave, save, bpsave, mpbpsave, etc.) to add</param>
        /// <param name="save">Name of the save folder to remove</param>
        void RemoveSave(String saveType, String saveName);

        /// <summary>Lists the exported characters located</summary>
        /// <returns>The collection of exported characters located</returns>
        IList<AssetReference> ListCharacters();

        /// <summary>Adds a character exported to the asset collection</summary>
        /// <param name="character">Character asset to add</param>
        /// <param name="biography">Optional character biography asset to add</param>
        void AddCharacter(AssetReference character, AssetReference biography = null);

        /// <summary>Removes a character exported from the asset collection</summary>
        /// <param name="character">Character asset to remove</param>
        /// <param name="biography">Optional character biography asset to remove</param>
        void RemoveCharacter(AssetReference character, AssetReference biography = null);

        //TODO: other specific asset exposures: portraits (each view does large + small), soundsets (match regex (.*)[.]{1}\.wav) which are grouped
        #endregion


        #region Key & BIF archive operations
        /// <summary>Adds the contents of a directory to the specified key file</summary>
        /// <param name="keyFile">Name of the key file to add assets in a BIF to</param>
        /// <param name="location">Locations (HD0, CD1, etc.) indicating where the BIFF should be written</param>
        /// <param name="directoryPath">Path of the source directory to create a BIF archive from</param>
        /// <param name="targetDirectory">Target directory in which to place the created BIF file</param>
        /// <param name="overrides">Flag indicating whether the these assets will override current assets being managed</param>
        void CreateBiffFromDirectory(String keyFile, KeyTableBifLocationEnum location, String directoryPath, String targetDirectory, Boolean overrides = false);

        /// <summary>Removes an archive from the specified key file</summary>
        /// <param name="keyFile">Key file to remove an archive from</param>
        /// <param name="location">Locations (HD0, CD1, etc.) indicating where the BIFF should be removed</param>
        /// <param name="biffIndex">Index of the archive to remove</param>
        void RemoveArchive(String keyFile, KeyTableBifLocationEnum location, Int32 biffIndex);

        /// <summary>Saves the existing key file and any changes made to it</summary>
        /// <param name="keyFile">Name of the key file to save</param>
        void SaveKey(String keyFile);

        /// <summary>Adds a file to the specified archive</summary>
        /// <param name="keyFile">Name of the key file in which to record the addition</param>
        /// <param name="biffIndex">Index of the key's archive to open and add to</param>
        /// <param name="filePath">Path of the source file to add to the archive and key files</param>
        void AddAssetToArchive(String keyFile, Int32 biffIndex, String filePath);

        /// <summary>Removes an asset from the specified key file. A flag is available to remove from the containing archive.</summary>
        /// <param name="keyFile">Name of the key file to remove the asset from</param>
        /// <param name="location">Targeted asset to remove from a key file</param>
        /// <param name="removeFromArchive">Flag indicating whether to remove the file from the housing archive</param>
        void RemoveAssetFromKey(String keyFile, ResourceLocator1 locator, Boolean removeFromArchive = false);
        #endregion


        #region Informational Displays
        /// <summary>Gets the game engine instance that this asset manager currently exposes</summary>
        /// <returns>The game engine instance that this asset manager currently exposes</returns>
        GameEngine GetGameEngine();

        /// <summary>Gets the game installation instance that this asset manager currently exposes</summary>
        /// <returns>The game installation instance that this asset manager currently exposes</returns>
        GameInstall GetGameInstall();

        /// <summary>Gets the install application directory</summary>
        /// <returns>The install application directory</returns>
        String GetApplicationDirectory();

        /// <summary>Gets the related User directory</summary>
        /// <returns>The related User directory</returns>
        String GetUserDirectory();
        #endregion
    }
}