using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl
{
    public abstract class ItemSpellAbility : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 0;    //here for signature purposes

        #region Members
        protected ItemSpellAbilityType abilityType;
        protected ItemSpellAbilityLocation abilityLocation;
        protected ResourceReference icon;
        protected ItemSpellAbilityTarget target;
        protected Byte targetCount;

        /// <summary>
        ///     This is largely unreferenced. It Would Seem, that, according to:
        ///     http://www.sorcerers.net/Games/BG2/SpellsReference/GeneralSpellInfo.htm
        ///     
        ///     That range is in-game as yards, and converted to search squares
        ///     inside of the engine.
        /// </summary>
        protected UInt16 range;
        protected UInt16 diceSides;
        protected UInt16 diceNumber;

        /// <summary>This is the damage bonus, such as from enchanted weapons.</summary>
        protected Int16 damageBonus;
        protected ItemSpellAbilityDamageType damageType;
        protected UInt16 featureCount;
        protected UInt16 featureOffset;
        protected UInt16 charges;
        protected ItemSpellAbilityDepletionBehavior depletionBehavior;

        /// <summary>Please see Projectl.ids, missile.ids</summary>
        protected UInt16 projectileAnimation;
        #endregion

        #region Properties
        public ItemSpellAbilityType AbilityType
        {
            get { return this.abilityType; }
            set { this.abilityType = value; }
        }

        public ItemSpellAbilityLocation AbilityLocation
        {
            get { return this.abilityLocation; }
            set { this.abilityLocation = value; }
        }

        public ResourceReference Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }

        public ItemSpellAbilityTarget Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        public Byte TargetCount
        {
            get { return this.targetCount; }
            set { this.targetCount = value; }
        }

        public UInt16 Range
        {
            get { return this.range; }
            set { this.range = value; }
        }

        public UInt16 DiceSides
        {
            get { return this.diceSides; }
            set { this.diceSides = value; }
        }

        public UInt16 DiceNumber
        {
            get { return this.diceNumber; }
            set { this.diceNumber = value; }
        }

        public Int16 DamageBonus
        {
            get { return this.damageBonus; }
            set { this.damageBonus = value; }
        }

        public ItemSpellAbilityDamageType DamageType
        {
            get { return this.damageType; }
            set { this.damageType = value; }
        }

        public UInt16 FeatureCount
        {
            get { return this.featureCount; }
            set { this.featureCount = value; }
        }

        public UInt16 FeatureOffset
        {
            get { return this.featureOffset; }
            set { this.featureOffset = value; }
        }

        public UInt16 Charges
        {
            get { return this.charges; }
            set { this.charges = value; }
        }

        public ItemSpellAbilityDepletionBehavior DepletionBehavior
        {
            get { return this.depletionBehavior; }
            set { this.depletionBehavior = value; }
        }
        
        public UInt16 ProjectileAnimation
        {
            get { return this.projectileAnimation; }
            set { this.projectileAnimation = value; }
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();

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