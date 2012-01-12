using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item
{
    public class ItemAbility : ItemSpellAbility
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 56;    //yes, hard-coded. These data structures have a static size.

        #region Members
        protected Boolean identifyRequired;

        /// <summary>This is the type of projectile required to use this item.</summary>
        protected ItemAbilityProjectileType projectileType;
        protected UInt16 speedFactor;
        protected UInt16 attackBonus;

        /// <summary>IESDP and DLTECP suggest this is a flag field, with bit 1 meaning breakable... SW1H01.ITM seems to back this up.</summary>
        /// <remarks>This is one 4-byte field in IESDP, which actually makes sense (but so does separating them, so not refactoring yet...)</remarks>
        protected ItemAbilityFlags flags;
        protected ItemAbilityAttackType attackType;
        protected UInt16 attackPercentOverhand;
        protected UInt16 attackPercentBackhand;
        protected UInt16 attackPercentThrust;
        protected UInt16 isArrow;
        protected UInt16 isBolt;
        protected UInt16 isMissile;
        #endregion

        #region Properties
        public Boolean IdentifyRequired
        {
            get { return this.identifyRequired; }
            set { this.identifyRequired = value; }
        }

        public ItemAbilityProjectileType ProjectileType
        {
            get { return projectileType; }
            set { projectileType = value; }
        }

        public UInt16 SpeedFactor
        {
            get { return speedFactor; }
            set { speedFactor = value; }
        }

        public UInt16 AttackBonus
        {
            get { return attackBonus; }
            set { attackBonus = value; }
        }

        public ItemAbilityFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public ItemAbilityAttackType AttackType
        {
            get { return attackType; }
            set { attackType = value; }
        }

        public UInt16 AttackPercentOverhand
        {
            get { return attackPercentOverhand; }
            set { attackPercentOverhand = value; }
        }

        public UInt16 AttackPercentBackhand
        {
            get { return attackPercentBackhand; }
            set { attackPercentBackhand = value; }
        }

        public UInt16 AttackPercentThrust
        {
            get { return attackPercentThrust; }
            set { attackPercentThrust = value; }
        }

        public UInt16 IsArrow
        {
            get { return isArrow; }
            set { isArrow = value; }
        }

        public UInt16 IsBolt
        {
            get { return isBolt; }
            set { isBolt = value; }
        }

        public UInt16 IsMissile
        {
            get { return isMissile; }
            set { isMissile = value; }
        }
        #endregion

        #region Constructor(s)
        public ItemAbility()
        {
            this.icon = null;
            this.featureCount = 0;
        }
        #endregion
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.icon = new ResourceReference();
        }

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

            this.abilityType = (ItemSpellAbilityType)remainingBody[0];
            this.identifyRequired = Convert.ToBoolean(remainingBody[1]);
            this.abilityLocation = (ItemSpellAbilityLocation)ReusableIO.ReadUInt16FromArray(remainingBody, 2);
            this.icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 4, CultureConstants.CultureCodeEnglish);
            this.target = (ItemSpellAbilityTarget)remainingBody[12];
            this.targetCount = remainingBody[13];
            this.range = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.projectileType = (ItemAbilityProjectileType)ReusableIO.ReadUInt16FromArray(remainingBody, 16);
            this.speedFactor = ReusableIO.ReadUInt16FromArray(remainingBody, 18);
            this.attackBonus = ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.diceSides = ReusableIO.ReadUInt16FromArray(remainingBody, 22);
            this.diceNumber = ReusableIO.ReadUInt16FromArray(remainingBody, 24);
            this.damageBonus = ReusableIO.ReadInt16FromArray(remainingBody, 26);
            this.damageType = (ItemSpellAbilityDamageType)ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.featureCount = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.featureOffset = ReusableIO.ReadUInt16FromArray(remainingBody, 32);
            this.charges = ReusableIO.ReadUInt16FromArray(remainingBody, 34);
            this.depletionBehavior = (ItemSpellAbilityDepletionBehavior)ReusableIO.ReadUInt16FromArray(remainingBody, 36);
            this.flags = (ItemAbilityFlags)ReusableIO.ReadUInt16FromArray(remainingBody, 38);
            this.attackType = (ItemAbilityAttackType)ReusableIO.ReadUInt16FromArray(remainingBody, 40);
            this.projectileAnimation = ReusableIO.ReadUInt16FromArray(remainingBody, 42);
            this.attackPercentOverhand = ReusableIO.ReadUInt16FromArray(remainingBody, 44);
            this.attackPercentBackhand = ReusableIO.ReadUInt16FromArray(remainingBody, 46);
            this.attackPercentThrust = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.isArrow = ReusableIO.ReadUInt16FromArray(remainingBody, 50);
            this.isBolt = ReusableIO.ReadUInt16FromArray(remainingBody, 52);
            this.isMissile = ReusableIO.ReadUInt16FromArray(remainingBody, 54);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            output.WriteByte((Byte)this.abilityType);
            output.WriteByte(Convert.ToByte(this.identifyRequired));
            ReusableIO.WriteUInt16ToStream((UInt16)this.abilityLocation, output);
            ReusableIO.WriteStringToStream(this.icon.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte((Byte)this.target);
            output.WriteByte(this.targetCount);
            ReusableIO.WriteUInt16ToStream(this.range, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.projectileType, output);
            ReusableIO.WriteUInt16ToStream(this.speedFactor, output);
            ReusableIO.WriteUInt16ToStream(this.attackBonus, output);
            ReusableIO.WriteUInt16ToStream(this.diceSides, output);
            ReusableIO.WriteUInt16ToStream(this.diceNumber, output);
            ReusableIO.WriteInt16ToStream(this.damageBonus, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.damageType, output);
            ReusableIO.WriteUInt16ToStream(this.featureCount, output);
            ReusableIO.WriteUInt16ToStream(this.featureOffset, output);
            ReusableIO.WriteUInt16ToStream(this.charges, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.depletionBehavior, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.flags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.attackType, output);
            ReusableIO.WriteUInt16ToStream(this.projectileAnimation, output);
            ReusableIO.WriteUInt16ToStream(this.attackPercentOverhand, output);
            ReusableIO.WriteUInt16ToStream(this.attackPercentBackhand, output);
            ReusableIO.WriteUInt16ToStream(this.attackPercentThrust, output);
            ReusableIO.WriteUInt16ToStream(this.isArrow, output);
            ReusableIO.WriteUInt16ToStream(this.isBolt, output);
            ReusableIO.WriteUInt16ToStream(this.isMissile, output);
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
            builder.Append((Byte)this.abilityType);
            builder.Append(StringFormat.ToStringAlignment("Ability type (description)"));
            builder.Append(this.abilityType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Use of ability requires item to be identified"));
            builder.Append(this.identifyRequired);
            builder.Append(StringFormat.ToStringAlignment("Ability UI location"));
            builder.Append((UInt16)this.abilityLocation);
            builder.Append(StringFormat.ToStringAlignment("Ability UI location (description)"));
            builder.Append(this.abilityLocation.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Icon"));
            builder.Append(String.Format("'{0}'", this.icon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Ability Target"));
            builder.Append((Byte)this.target);
            builder.Append(StringFormat.ToStringAlignment("Ability Target (description)"));
            builder.Append(this.target.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Target Count"));
            builder.Append(this.targetCount);
            builder.Append(StringFormat.ToStringAlignment("Ability Range (in search rectangles?)"));
            builder.Append(this.range);
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Type"));
            builder.Append((UInt16)this.projectileType);
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Type (description)"));
            builder.Append(this.projectileType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Speed Factor"));
            builder.Append(this.speedFactor);
            builder.Append(StringFormat.ToStringAlignment("Ability Attack Bonus (Thac0 2E, Attack 3E)"));
            builder.Append(this.attackBonus);
            builder.Append(StringFormat.ToStringAlignment("Ability Dice Sides"));
            builder.Append(this.diceSides);
            builder.Append(StringFormat.ToStringAlignment("Ability Dice Number"));
            builder.Append(this.diceNumber);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Bonus"));
            builder.Append(this.damageBonus);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Type"));
            builder.Append((UInt16)this.damageType);
            builder.Append(StringFormat.ToStringAlignment("Ability Damage Type (description)"));
            builder.Append(this.damageType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability Effects Count"));
            builder.Append(this.featureCount);
            builder.Append(StringFormat.ToStringAlignment("Ability Effects Offset"));
            builder.Append(this.featureOffset);
            builder.Append(StringFormat.ToStringAlignment("Ability Charges Count"));
            builder.Append(this.charges);
            builder.Append(StringFormat.ToStringAlignment("Charge depletion behavior"));
            builder.Append((UInt16)this.depletionBehavior);
            builder.Append(StringFormat.ToStringAlignment("Charge depletion behavior (description)"));
            builder.Append(this.depletionBehavior.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Ability flags"));
            builder.Append((UInt16)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Ability flags (enumerated)"));
            builder.Append(this.GetAbilityFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Ability attack type"));
            builder.Append((UInt16)this.attackType);
            builder.Append(StringFormat.ToStringAlignment("Ability attack type (enumerated)"));
            builder.Append(this.GetAbilityItemAttackTypeString());
            builder.Append(StringFormat.ToStringAlignment("Ability Projectile Animation (see *.IDS)"));
            builder.Append(this.projectileAnimation);
            builder.Append(StringFormat.ToStringAlignment("Attack % Overhand"));
            builder.Append(this.attackPercentOverhand);
            builder.Append(StringFormat.ToStringAlignment("Attack % Backhand"));
            builder.Append(this.attackPercentBackhand);
            builder.Append(StringFormat.ToStringAlignment("Attack % Thrust"));
            builder.Append(this.attackPercentThrust);
            builder.Append(StringFormat.ToStringAlignment("Is arrow"));
            builder.Append(this.isArrow);
            builder.Append(StringFormat.ToStringAlignment("Is bolt"));
            builder.Append(this.isBolt);
            builder.Append(StringFormat.ToStringAlignment("Is missile"));
            builder.Append(this.isMissile);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetAbilityItemAttackTypeString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.attackType & ItemAbilityAttackType.BypassArmor) == ItemAbilityAttackType.BypassArmor, ItemAbilityAttackType.BypassArmor.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attackType & ItemAbilityAttackType.Keen) == ItemAbilityAttackType.Keen, ItemAbilityAttackType.Keen.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetAbilityFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & ItemAbilityFlags.AddStrength) == ItemAbilityFlags.AddStrength, ItemAbilityFlags.AddStrength.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemAbilityFlags.Breakable) == ItemAbilityFlags.Breakable, ItemAbilityFlags.Breakable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemAbilityFlags.Hostile) == ItemAbilityFlags.Hostile, ItemAbilityFlags.Hostile.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemAbilityFlags.AfterRest) == ItemAbilityFlags.AfterRest, ItemAbilityFlags.AfterRest.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}