using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell
{
    /// <summary>Common base class for any spell ability</summary>
    public class SpellAbility : ItemSpellAbility
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 40;    //These data structures have a static size.
        #endregion


        #region Members
        /// <remarks>
        ///     This is the same byte as identifyRequired for items, unused by Spells.
        ///     Except for PS:T, which uses it to identify the level of hostility of the spell.
        ///     i.e.: Strength of One (SPWI219.SPL) is Non-Hostile (4)
        ///     
        ///     See <see cref="Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums.SpellHeader1Flags" /> for comparison
        ///     
        ///     BG2 code suggest this field is part of the previous field as a SHORT
        /// </remarks>
        /// <value>
        ///     Near Infinity is the only app to report on this.
        ///     Values known are:
        ///     Hostile (0)
        ///     Unknown (1)
        ///     Unknown (2)
        ///     Unknown (3)
        ///     Non-hostile (4)
        /// </value>
        public SpellAbilityFlags SpellFlags { get; set; }

        /// <summary>Level requirement for this spell ability</summary>
        public UInt16 LevelRequirement { get; set; }

        /// <summary>
        ///     This is for 2E. It is also the 'speed' item slot, followed by the attack bonus field.
        ///     IESDP suggests this as a 32-bit field, but the internal struckture would, then, suggest otherwise.
        ///     However, since the data field is irrelevant for spells, I will follow IESDP for the hell of it.
        /// </summary>
        public UInt16 CastingSpeed { get; set; }

        /// <summary>Times per day this spell can be cast?</summary>
        /// <remarks>BG2 source code comment: "// times/per/day 0xFFFF is unlimited"</remarks>
        public UInt16 TimesPerDay { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public SpellAbility()
        {
            this.Icon = null;
            this.FeatureCount = 0;
        }
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Icon = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public override void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public override void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 40);

            this.AbilityType = (ItemSpellAbilityType)remainingBody[0];
            this.SpellFlags = (SpellAbilityFlags)remainingBody[1];
            this.AbilityLocation = (ItemSpellAbilityLocation)ReusableIO.ReadUInt16FromArray(remainingBody, 2);
            this.Icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 4, CultureConstants.CultureCodeEnglish);
            this.Target = (ItemSpellAbilityTarget)remainingBody[12];
            this.TargetCount = remainingBody[13];
            this.Range = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.LevelRequirement = ReusableIO.ReadUInt16FromArray(remainingBody, 16);
            this.CastingSpeed = ReusableIO.ReadUInt16FromArray(remainingBody, 18);
            this.TimesPerDay = ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.DiceSides = ReusableIO.ReadUInt16FromArray(remainingBody, 22);
            this.DiceNumber = ReusableIO.ReadUInt16FromArray(remainingBody, 24);
            this.DamageBonus = ReusableIO.ReadInt16FromArray(remainingBody, 26);
            this.DamageType = (ItemSpellAbilityDamageType)ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.FeatureCount = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.FeatureOffset = ReusableIO.ReadUInt16FromArray(remainingBody, 32);
            this.Charges = ReusableIO.ReadUInt16FromArray(remainingBody, 34);
            this.DepletionBehavior = (ItemSpellAbilityDepletionBehavior)ReusableIO.ReadUInt16FromArray(remainingBody, 36);
            this.ProjectileAnimation = ReusableIO.ReadUInt16FromArray(remainingBody, 38);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            output.WriteByte((Byte)this.AbilityType);
            output.WriteByte((Byte)this.SpellFlags);
            ReusableIO.WriteUInt16ToStream((UInt16)this.AbilityLocation, output);
            ReusableIO.WriteStringToStream(this.Icon.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte((Byte)this.Target);
            output.WriteByte(this.TargetCount);
            ReusableIO.WriteUInt16ToStream(this.Range, output);
            ReusableIO.WriteUInt16ToStream(this.LevelRequirement, output);
            ReusableIO.WriteUInt16ToStream(this.CastingSpeed, output);
            ReusableIO.WriteUInt16ToStream(this.TimesPerDay, output);
            ReusableIO.WriteUInt16ToStream(this.DiceSides, output);
            ReusableIO.WriteUInt16ToStream(this.DiceNumber, output);
            ReusableIO.WriteInt16ToStream(this.DamageBonus, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.DamageType, output);
            ReusableIO.WriteUInt16ToStream(this.FeatureCount, output);
            ReusableIO.WriteUInt16ToStream(this.FeatureOffset, output);
            ReusableIO.WriteUInt16ToStream(this.Charges, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.DepletionBehavior, output);
            ReusableIO.WriteUInt16ToStream(this.ProjectileAnimation, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString(Boolean showType)
        {
            String header = "Spell Ability & Extended Header:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString(Int32 abilityIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Spell Ability # {0}:", abilityIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Ability type"));
            builder.Append((Byte)this.AbilityType);
            builder.Append(StringFormat.ToStringAlignment("Ability type (description)"));
            builder.Append(this.AbilityType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Use of ability hostility level"));
            builder.Append((Byte)this.SpellFlags);
            builder.Append(StringFormat.ToStringAlignment("Use of ability hostility level (enumerated)"));
            builder.Append(this.GetFlagString());
            builder.Append(StringFormat.ToStringAlignment("Ability UI location"));
            builder.Append((UInt16)this.AbilityLocation);
            builder.Append(StringFormat.ToStringAlignment("Ability UI location (description)"));
            builder.Append(this.AbilityLocation.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Icon"));
            builder.Append(String.Format("'{0}'", this.Icon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Ability Target"));
            builder.Append((Byte)this.Target);
            builder.Append(StringFormat.ToStringAlignment("Ability Target (description)"));
            builder.Append(this.Target.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Target Count"));
            builder.Append(this.TargetCount);
            builder.Append(StringFormat.ToStringAlignment("Ability Range (in search rectangles?)"));
            builder.Append(this.Range);
            builder.Append(StringFormat.ToStringAlignment("Level requirement"));
            builder.Append(this.LevelRequirement);
            builder.Append(StringFormat.ToStringAlignment("Casting speed"));
            builder.Append(this.CastingSpeed);
            builder.Append(StringFormat.ToStringAlignment("Ability Dice Sides"));
            builder.Append(this.DiceSides);
            builder.Append(StringFormat.ToStringAlignment("Ability Dice Number"));
            builder.Append(this.DiceNumber);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Bonus"));
            builder.Append(this.DamageBonus);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Type"));
            builder.Append((UInt16)this.DamageType);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Type (description)"));
            builder.Append(this.DamageType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Effects Count"));
            builder.Append(this.FeatureCount);
            builder.Append(StringFormat.ToStringAlignment("Ability Effects Offset"));
            builder.Append(this.FeatureOffset);
            builder.Append(StringFormat.ToStringAlignment("Ability Charges Count"));
            builder.Append(this.Charges);
            builder.Append(StringFormat.ToStringAlignment("Charge depletion behavior"));
            builder.Append((UInt16)this.DepletionBehavior);
            builder.Append(StringFormat.ToStringAlignment("Charge depletion behavior (description)"));
            builder.Append(this.DepletionBehavior.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Animation (see *.IDS)"));
            builder.Append(this.ProjectileAnimation);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFlagString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.SpellFlags & SpellAbilityFlags.Friendly) == SpellAbilityFlags.Friendly, SpellAbilityFlags.Friendly.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}