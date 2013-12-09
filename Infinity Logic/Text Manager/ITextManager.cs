using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.TextManager
{
    /// <summary>
    ///     This interface is the interface from which a game might create new strings (TOT, TOH)
    ///     or update the existing repository (TLK)
    /// </summary>
    public interface ITextManager
    {
        /// <summary>Gets the string structure for the referenced index</summary>
        /// <param name="language">The language specified to use</param>
        /// <param name="strref">String index to retrieve</param>
        /// <param name="gender">Gender of the string being requested</param>
        /// <returns>The requested String index</returns>
        TextLocationKeyStringReference GetString(LanguageCode language, Int32 strref, LanguageGender gender = LanguageGender.Masculine);

        /// <summary>Updates the specified string structure</summary>
        /// <param name="language">The language specified to use</param>
        /// <param name="index">Index of the string to update</param>
        /// <param name="strref">The data to update the string with</param>
        /// <param name="gender">Gender of the string being updated</param>
        void UpdateString(LanguageCode language, Int32 index, TextLocationKeyStringReference strref, LanguageGender gender = LanguageGender.Masculine);

        /// <summary>Adds the string to the TLK file</summary>
        /// <param name="language">The language specified to use</param>
        /// <param name="strref">The data to add the string with</param>
        /// <param name="gender">Gender of the string being added</param>
        /// <returns>The new index of the added string</returns>
        Int32 AddString(LanguageCode language, TextLocationKeyStringReference strref, LanguageGender gender = LanguageGender.Masculine);

        /// <summary>Saves any changes to the masculine Dialog.tlk file</summary>
        /// <param name="language">Language code of the tlk file to save</param>
        /// <param name="gender">Gender of the string collection being saved</param>
        void SaveTlk(LanguageCode language, LanguageGender gender = LanguageGender.Masculine);
    }
}