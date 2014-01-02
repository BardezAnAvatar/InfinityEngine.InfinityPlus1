using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item
{
    /// <summary>The ability structure for an item</summary>
    public class ItemAbility : ItemSpellAbility
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 56;    //yes, hard-coded. These data structures have a static size.
        #endregion


        #region Members
        /// <summary>Flag indicating whether identification is required to use this item ability</summary>
        public Boolean IdentifyRequired { get; set; }

        /// <summary>This is the type of projectile required to use this item.</summary>
        public ItemAbilityProjectileType ProjectileType { get; set; }

        /// <summary>2E speed factor associated with the item</summary>
        public UInt16 SpeedFactor { get; set; }

        /// <summary>THAC0 or BaB attack bonus</summary>
        public UInt16 AttackBonus { get; set; }

        /// <summary>IESDP and DLTECP suggest this is a flag field, with bit 1 meaning breakable... SW1H01.ITM seems to back this up.</summary>
        /// <remarks>This is one 4-byte field in IESDP, which actually makes sense (but so does separating them, so not refactoring yet...)</remarks>
        public ItemAbilityFlags Flags { get; set; }

        /// <summary>Type of attack associated with this ability</summary>
        public ItemAbilityAttackType AttackType { get; set; }

        /// <summary>Probability of an overhand attack</summary>
        public UInt16 AttackPercentOverhand { get; set; }

        /// <summary>Probability of a backhand attack</summary>
        public UInt16 AttackPercentBackhand { get; set; }

        /// <summary>Probability of a thrust attack</summary>
        public UInt16 AttackPercentThrust { get; set; }

        /// <summary>Flag indicating whether this ability represents an arrow</summary>
        public UInt16 IsArrow { get; set; }

        /// <summary>Flag indicating whether this ability represents a bolt</summary>
        public UInt16 IsBolt { get; set; }

        /// <summary>Flag indicating whether this ability represents another missile type</summary>
        public UInt16 IsMissile { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ItemAbility()
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
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 56);

            this.AbilityType = (ItemSpellAbilityType)remainingBody[0];
            this.IdentifyRequired = Convert.ToBoolean(remainingBody[1]);
            this.AbilityLocation = (ItemSpellAbilityLocation)ReusableIO.ReadUInt16FromArray(remainingBody, 2);
            this.Icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 4, CultureConstants.CultureCodeEnglish);
            this.Target = (ItemSpellAbilityTarget)remainingBody[12];
            this.TargetCount = remainingBody[13];
            this.Range = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.ProjectileType = (ItemAbilityProjectileType)ReusableIO.ReadUInt16FromArray(remainingBody, 16);
            this.SpeedFactor = ReusableIO.ReadUInt16FromArray(remainingBody, 18);
            this.AttackBonus = ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.DiceSides = ReusableIO.ReadUInt16FromArray(remainingBody, 22);
            this.DiceNumber = ReusableIO.ReadUInt16FromArray(remainingBody, 24);
            this.DamageBonus = ReusableIO.ReadInt16FromArray(remainingBody, 26);
            this.DamageType = (ItemSpellAbilityDamageType)ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.FeatureCount = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.FeatureOffset = ReusableIO.ReadUInt16FromArray(remainingBody, 32);
            this.Charges = ReusableIO.ReadUInt16FromArray(remainingBody, 34);
            this.DepletionBehavior = (ItemSpellAbilityDepletionBehavior)ReusableIO.ReadUInt16FromArray(remainingBody, 36);
            this.Flags = (ItemAbilityFlags)ReusableIO.ReadUInt16FromArray(remainingBody, 38);
            this.AttackType = (ItemAbilityAttackType)ReusableIO.ReadUInt16FromArray(remainingBody, 40);
            this.ProjectileAnimation = ReusableIO.ReadUInt16FromArray(remainingBody, 42);
            this.AttackPercentOverhand = ReusableIO.ReadUInt16FromArray(remainingBody, 44);
            this.AttackPercentBackhand = ReusableIO.ReadUInt16FromArray(remainingBody, 46);
            this.AttackPercentThrust = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.IsArrow = ReusableIO.ReadUInt16FromArray(remainingBody, 50);
            this.IsBolt = ReusableIO.ReadUInt16FromArray(remainingBody, 52);
            this.IsMissile = ReusableIO.ReadUInt16FromArray(remainingBody, 54);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            output.WriteByte((Byte)this.AbilityType);
            output.WriteByte(Convert.ToByte(this.IdentifyRequired));
            ReusableIO.WriteUInt16ToStream((UInt16)this.AbilityLocation, output);
            ReusableIO.WriteStringToStream(this.Icon.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte((Byte)this.Target);
            output.WriteByte(this.TargetCount);
            ReusableIO.WriteUInt16ToStream(this.Range, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.ProjectileType, output);
            ReusableIO.WriteUInt16ToStream(this.SpeedFactor, output);
            ReusableIO.WriteUInt16ToStream(this.AttackBonus, output);
            ReusableIO.WriteUInt16ToStream(this.DiceSides, output);
            ReusableIO.WriteUInt16ToStream(this.DiceNumber, output);
            ReusableIO.WriteInt16ToStream(this.DamageBonus, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.DamageType, output);
            ReusableIO.WriteUInt16ToStream(this.FeatureCount, output);
            ReusableIO.WriteUInt16ToStream(this.FeatureOffset, output);
            ReusableIO.WriteUInt16ToStream(this.Charges, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.DepletionBehavior, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Flags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.AttackType, output);
            ReusableIO.WriteUInt16ToStream(this.ProjectileAnimation, output);
            ReusableIO.WriteUInt16ToStream(this.AttackPercentOverhand, output);
            ReusableIO.WriteUInt16ToStream(this.AttackPercentBackhand, output);
            ReusableIO.WriteUInt16ToStream(this.AttackPercentThrust, output);
            ReusableIO.WriteUInt16ToStream(this.IsArrow, output);
            ReusableIO.WriteUInt16ToStream(this.IsBolt, output);
            ReusableIO.WriteUInt16ToStream(this.IsMissile, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString(Boolean showType)
        {
            String header = "ITEM Ability & Extended Header:";

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
            return StringFormat.ReturnAndIndent(String.Format("ITEM Ability # {0}:", abilityIndex), 0) + this.GetStringRepresentation();
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
            builder.Append(StringFormat.ToStringAlignment("Use of ability requires item to be identified"));
            builder.Append(this.IdentifyRequired);
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
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Type"));
            builder.Append((UInt16)this.ProjectileType);
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Type (description)"));
            builder.Append(this.ProjectileType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Speed Factor"));
            builder.Append(this.SpeedFactor);
            builder.Append(StringFormat.ToStringAlignment("Ability Attack Bonus (Thac0 2E, Attack 3E)"));
            builder.Append(this.AttackBonus);
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
            builder.Append(StringFormat.ToStringAlignment("Ability flags"));
            builder.Append((UInt16)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Ability flags (enumerated)"));
            builder.Append(this.GetAbilityFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Ability attack type"));
            builder.Append((UInt16)this.AttackType);
            builder.Append(StringFormat.ToStringAlignment("Ability attack type (enumerated)"));
            builder.Append(this.GetAbilityItemAttackTypeString());
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Animation (see *.IDS)"));
            builder.Append(this.ProjectileAnimation);
            builder.Append(StringFormat.ToStringAlignment("Attack % Overhand"));
            builder.Append(this.AttackPercentOverhand);
            builder.Append(StringFormat.ToStringAlignment("Attack % Backhand"));
            builder.Append(this.AttackPercentBackhand);
            builder.Append(StringFormat.ToStringAlignment("Attack % Thrust"));
            builder.Append(this.AttackPercentThrust);
            builder.Append(StringFormat.ToStringAlignment("Is arrow"));
            builder.Append(this.IsArrow);
            builder.Append(StringFormat.ToStringAlignment("Is bolt"));
            builder.Append(this.IsBolt);
            builder.Append(StringFormat.ToStringAlignment("Is missile"));
            builder.Append(this.IsMissile);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetAbilityItemAttackTypeString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.AttackType & ItemAbilityAttackType.BypassArmor) == ItemAbilityAttackType.BypassArmor, ItemAbilityAttackType.BypassArmor.GetDescription());
            StringFormat.AppendSubItem(sb, (this.AttackType & ItemAbilityAttackType.Keen) == ItemAbilityAttackType.Keen, ItemAbilityAttackType.Keen.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetAbilityFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Flags & ItemAbilityFlags.AddStrength) == ItemAbilityFlags.AddStrength, ItemAbilityFlags.AddStrength.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & ItemAbilityFlags.Breakable) == ItemAbilityFlags.Breakable, ItemAbilityFlags.Breakable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & ItemAbilityFlags.Hostile) == ItemAbilityFlags.Hostile, ItemAbilityFlags.Hostile.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & ItemAbilityFlags.AfterRest) == ItemAbilityFlags.AfterRest, ItemAbilityFlags.AfterRest.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}