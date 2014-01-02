using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl
{
    /// <summary>Common elements of item and spell abilities</summary>
    public abstract class ItemSpellAbility : IInfinityFormat
    {
        #region Fields
        /// <summary>Type of ability</summary>
        public ItemSpellAbilityType AbilityType { get; set; }

        /// <summary>Location within the game UI at which this ability will appear</summary>
        public ItemSpellAbilityLocation AbilityLocation { get; set; }

        /// <summary>UI icon used to represent this ability</summary>
        public ResourceReference Icon { get; set; }

        /// <summary>Valid target for this ability</summary>
        public ItemSpellAbilityTarget Target { get; set; }

        /// <summary>Count of targets this ability can affect</summary>
        public Byte TargetCount { get; set; }

        /// <summary>
        ///     This is largely unreferenced. It Would Seem, that, according to:
        ///     http://www.sorcerers.net/Games/BG2/SpellsReference/GeneralSpellInfo.htm
        ///     
        ///     That range is in-game as yards, and converted to search squares
        ///     inside of the engine.
        /// </summary>
        public UInt16 Range { get; set; }

        /// <summary>The maximum number allowed for a dice roll associated with this ability</summary>
        public UInt16 DiceSides { get; set; }

        /// <summary>The number of dice rolled associated with this ability</summary>
        public UInt16 DiceNumber { get; set; }

        /// <summary>This is the damage bonus, such as from enchanted weapons.</summary>
        public Int16 DamageBonus { get; set; }

        /// <summary>Type of damage associated with this ability</summary>
        public ItemSpellAbilityDamageType DamageType { get; set; }

        /// <summary>Count of features to this ability</summary>
        public UInt16 FeatureCount { get; set; }

        /// <summary>Offset within the source file to the features</summary>
        public UInt16 FeatureOffset { get; set; }

        /// <summary>Charges for this ability</summary>
        public UInt16 Charges { get; set; }

        /// <summary>Behavior associated with charge depletion</summary>
        public ItemSpellAbilityDepletionBehavior DepletionBehavior { get; set; }

        /// <summary>Please see Projectl.ids, missile.ids</summary>
        public UInt16 ProjectileAnimation { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();
        #endregion


        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void Read(Stream input);

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public abstract void Read(Stream input, Boolean fullRead);

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void ReadBody(Stream input);

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public abstract void Write(Stream output);
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public abstract String ToString(Boolean showType);

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="abilityIndex">Index of the entry to print, enumerated</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public abstract String ToString(Int32 abilityIndex);

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected abstract String GetStringRepresentation();
        #endregion
    }
}