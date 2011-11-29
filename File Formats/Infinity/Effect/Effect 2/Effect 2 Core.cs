using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect2
{
    public class Effect2Core : EffectBase
    {
        /// <summary>Size of the effect structure</summary>
        public const UInt32 StructureSize = 272U;    //yes, hard-coded. These data structures have a static size.
        
        #region Members
        /// <summary>Effect Type/Opcode</summary>
        protected UInt32 opcode;

        /// <summary>Target of the effect</summary>
        protected EffectTarget target;

        /// <summary>Level of the overlay? Level of the caster?</summary>
        protected UInt32 power;

        /// <summary>Timing of the effect</summary>
        protected EffectTimingMode2 timingMode;

        /// <summary>Unknown, flagged as Timing? in IESDP</summary>
        /// <remarks>,Full 4 byte join of previous field in Near Infinity</remarks>
        protected UInt16 timingUnknown;

        /// <summary>First probability of the effect</summary>
        protected UInt16 probability1;

        /// <summary>Second probability of the effect</summary>
        protected UInt16 probability2;

        /// <summary>Set local variable only if non-existant</summary>
        /// <remarks>
        ///     This apears to be the same location as unknown1 in effect version 1.
        ///     Gem RB apparently thinkgs this is just a variable flag, not an if unset condition
        ///     (I am inclined to agree).
        /// </remarks>
        protected UInt32 setLocalIfNotSet;

        /// <summary>School to which the (magical) effect might belong to</summary>
        protected EffectMageSchool mageSchool;

        /// <summary>Save for half damage</summary>
        protected UInt16 saveForHalf;

        /// <summary>Minimum level effected</summary>
        protected UInt32 levelAffectedMin;
        
        /// <summary>Maximum level effected</summary>
        protected UInt32 levelAffectedMax;

        //4 bytes for EffectResistance
        /// <summary>4 bytes that reprsent a one-byte value</summary>
        /// <remarks>I will have a property expose EffectResistance</remarks>
        protected UInt32 resistanceField;
        
        /// <summary>Third parameter to the effect</summary>
        protected Int32 parameter3;

        /// <summary>ourth parameter to the effect</summary>
        protected Int32 parameter4;

        /// <summary>Second resource referenced by the effect</summary>
        protected ResourceReference resource2;

        /// <summary>Third resource referenced by the effect</summary>
        /// <remarks>Refernces VVC animation helper</remarks>
        protected ResourceReference resource3;

        //8 Bytes of unknown
        /// <summary>
        ///     The data for referencing looks like it is junk ZString data, so this is
        ///     actually a resource reference?
        /// </summary>
        protected ResourceReference resourceUnknown;

        /// <summary>X-cooridnate position (offset) of caster effect</summary>
        protected Int32 positionCasterX;

        /// <summary>Y-cooridnate position (offset) of caster effect</summary>
        protected Int32 positionCasterY;

        /// <summary>X-cooridnate position (offset) of target effect</summary>
        protected Int32 positionTargetX;

        /// <summary>Y-cooridnate position (offset) of target effect</summary>
        protected Int32 positionTargetY;

        /// <summary>Resource type of referencing resource</summary>
        protected EffectResourceType parentResourceType;

        /// <summary>Parent resource intended to be referencing the effect</summary>
        protected ResourceReference parentResource;

        /// <summary>Parent resource flags</summary>
        /// <remarks>IESP description doesn't make much sense</remarks>
        protected EffectResourceFlags parentResourceFlags;

        /// <summary>Projectile</summary>
        protected UInt32 projectile;

        /// <summary>parent resource slot</summary>
        protected Int32 parentResourceSlot;

        /// <summary>Caster level of any effect applied</summary>
        protected UInt32 casterLevel;

        /// <summary>Second unknown. 4 bytes.</summary>
        protected UInt32 unknown2;

        /// <summary>Secondary efect type</summary>
        protected EffectMagicType magicType;

        /// <summary>trailing 60 unknown bytes</summary>
        protected Byte[] unknown3;
        #endregion

        #region Properties
        /// <summary>Effect Type/Opcode</summary>
        public UInt32 Opcode
        {
            get { return this.opcode; }
            set { this.opcode = value; }
        }

        /// <summary>Target of the effect</summary>
        public EffectTarget Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        /// <summary>Level of the overlay? Level of the caster?</summary>
        public UInt32 Power
        {
            get { return this.power; }
            set { this.power = value; }
        }

        /// <summary>Timing of the effect</summary>
        public EffectTimingMode2 TimingMode
        {
            get { return this.timingMode; }
            set { this.timingMode = value; }
        }

        /// <summary>Unknown, flagged as Timing? in IESDP</summary>
        /// <remarks>,Full 4 byte join of previous field in Near Infinity</remarks>
        public UInt16 TimingUnknown
        {
            get { return this.timingUnknown; }
            set { this.timingUnknown = value; }
        }

        /// <summary>First probability of the effect</summary>
        public UInt16 Probability1
        {
            get { return this.probability1; }
            set { this.probability1 = value; }
        }

        /// <summary>Second probability of the effect</summary>
        public UInt16 Probability2
        {
            get { return this.probability2; }
            set { this.probability2 = value; }
        }

        /// <summary>Set local variable only if non-existant</summary>
        /// <remarks>
        ///     This apears to be the same location as unknown1 in effect version 1.
        ///     Gem RB apparently thinkgs this is just a variable flag, not an if unset condition
        ///     (I am inclined to agree).
        /// </remarks>
        public UInt32 SetLocalIfNotSet
        {
            get { return this.setLocalIfNotSet; }
            set { this.setLocalIfNotSet = value; }
        }

        /// <summary>School to which the (magical) effect might belong to</summary>
        public EffectMageSchool MageSchool
        {
            get { return this.mageSchool; }
            set { this.mageSchool = value; }
        }

        /// <summary>Save for half damage</summary>
        public UInt16 SaveForHalf
        {
            get { return this.saveForHalf; }
            set { this.saveForHalf = value; }
        }

        /// <summary>Minimum level effected</summary>
        public UInt32 LevelAffectedMin
        {
            get { return this.levelAffectedMin; }
            set { this.levelAffectedMin = value; }
        }

        /// <summary>Maximum level effected</summary>
        public UInt32 LevelAffectedMax
        {
            get { return this.levelAffectedMax; }
            set { this.levelAffectedMax = value; }
        }

        /// <summary>4 bytes that reprsent a one-byte value</summary>
        /// <remarks>I will have a property expose EffectResistance</remarks>
        public UInt32 ResistanceField
        {
            get { return this.resistanceField; }
            set { this.resistanceField = value; }
        }

        /// <summary>Resistance against the effect</summary>
        /// <remarks>This is not an actual member, just a property exposing one alternatively</remarks>
        public EffectResistance Resistance
        {
            get { return (EffectResistance)Convert.ToByte(this.resistanceField); }
            set { this.resistanceField = (Byte)value; }
        }

        /// <summary>Third parameter to the effect</summary>
        public Int32 Parameter3
        {
            get { return this.parameter3; }
            set { this.parameter3 = value; }
        }

        /// <summary>ourth parameter to the effect</summary>
        public Int32 Parameter4
        {
            get { return this.parameter4; }
            set { this.parameter4 = value; }
        }

        /// <summary>Second resource referenced by the effect</summary>
        public ResourceReference Resource2
        {
            get { return this.resource2; }
            set { this.resource2 = value; }
        }

        /// <summary>Third resource referenced by the effect</summary>
        /// <remarks>Refernces VVC animation helper</remarks>
        public ResourceReference Resource3
        {
            get { return this.resource3; }
            set { this.resource3 = value; }
        }

        /// <summary>
        ///     The data for referencing looks like it is junk ZString data, so this is
        ///     actually a resource reference?
        /// </summary>
        public ResourceReference ResourceUnknown
        {
            get { return this.resourceUnknown; }
            set { this.resourceUnknown = value; }
        }

        /// <summary>X-cooridnate position (offset) of caster effect</summary>
        public Int32 PositionCasterX
        {
            get { return this.positionCasterX; }
            set { this.positionCasterX = value; }
        }

        /// <summary>Y-cooridnate position (offset) of caster effect</summary>
        public Int32 PositionCasterY
        {
            get { return this.positionCasterY; }
            set { this.positionCasterY = value; }
        }

        /// <summary>X-cooridnate position (offset) of target effect</summary>
        public Int32 PositionTargetX
        {
            get { return this.positionTargetX; }
            set { this.positionTargetX = value; }
        }

        /// <summary>Y-cooridnate position (offset) of target effect</summary>
        public Int32 PositionTargetY
        {
            get { return this.positionTargetY; }
            set { this.positionTargetY = value; }
        }

        /// <summary>Resource type of referencing resource</summary>
        public EffectResourceType ParentResourceType
        {
            get { return this.parentResourceType; }
            set { this.parentResourceType = value; }
        }

        /// <summary>Parent resource intended to be referencing the effect</summary>
        public ResourceReference ParentResource
        {
            get { return this.parentResource; }
            set { this.parentResource = value; }
        }

        /// <summary>Parent resource flags</summary>
        /// <remarks>IESP description doesn't make much sense</remarks>
        public EffectResourceFlags ParentResourceFlags
        {
            get { return this.parentResourceFlags; }
            set { this.parentResourceFlags = value; }
        }

        /// <summary>Projectile</summary>
        public UInt32 Projectile
        {
            get { return this.projectile; }
            set { this.projectile = value; }
        }

        /// <summary>parent resource slot</summary>
        public Int32 ParentResourceSlot
        {
            get { return this.parentResourceSlot; }
            set { this.parentResourceSlot = value; }
        }

        /// <summary>Name of ay local variable being set. 32 bytes.</summary>
        public ZString LocalVariableName { get; set; }

        /// <summary>Caster level of any effect applied</summary>
        public UInt32 CasterLevel
        {
            get { return this.casterLevel; }
            set { this.casterLevel = value; }
        }

        /// <summary>Second unknown. 4 bytes.</summary>
        public UInt32 Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Secondary efect type</summary>
        public EffectMagicType MagicType
        {
            get { return this.magicType; }
            set { this.magicType = value; }
        }

        /// <summary>trailing 60 unknown bytes</summary>
        public Byte[] Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }
        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public Effect2Core()
        {
            this.resource1 = null;
            this.unknown3 = null;
            this.resource2 = null;
            this.resource3 = null;
            this.resourceUnknown = null;
            this.LocalVariableName = null;
        }
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.resource1 = new ResourceReference();
            this.unknown3 = new Byte[60];
            this.resource2 = new ResourceReference();
            this.resource3 = new ResourceReference();
            this.resourceUnknown = new ResourceReference();
            this.parentResource = new ResourceReference();
            this.LocalVariableName = new ZString();
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

            //read effect
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 256);

            this.opcode = ReusableIO.ReadUInt32FromArray(remainingBody, 0);
            this.target = (EffectTarget)Convert.ToSByte(ReusableIO.ReadInt32FromArray(remainingBody, 4));
            this.power = ReusableIO.ReadUInt32FromArray(remainingBody, 8);
            this.parameter1 = ReusableIO.ReadInt32FromArray(remainingBody, 12);
            this.parameter2 = ReusableIO.ReadInt32FromArray(remainingBody, 16);
            this.timingMode = (EffectTimingMode2)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.timingUnknown = ReusableIO.ReadUInt16FromArray(remainingBody, 22);
            this.duration = ReusableIO.ReadUInt32FromArray(remainingBody, 24);
            this.probability1 = ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.probability2 = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.resource1.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 32, Constants.CultureCodeEnglish);
            this.diceCount = ReusableIO.ReadUInt32FromArray(remainingBody, 40);
            this.diceSides = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
            this.savingThrowType = (EffectSavingThrow)ReusableIO.ReadUInt32FromArray(remainingBody, 48);
            this.savingThrowModifier = ReusableIO.ReadInt32FromArray(remainingBody, 52);
            this.setLocalIfNotSet = ReusableIO.ReadUInt32FromArray(remainingBody, 56);
            this.mageSchool = (EffectMageSchool)ReusableIO.ReadUInt16FromArray(remainingBody, 60);
            this.saveForHalf = ReusableIO.ReadUInt16FromArray(remainingBody, 62);
            this.unknown1 = ReusableIO.ReadUInt32FromArray(remainingBody, 64);
            this.levelAffectedMin = ReusableIO.ReadUInt32FromArray(remainingBody, 68);
            this.levelAffectedMax = ReusableIO.ReadUInt32FromArray(remainingBody, 72);
            this.Resistance = (EffectResistance)Convert.ToByte(ReusableIO.ReadUInt16FromArray(remainingBody, 76));
            this.parameter3 = ReusableIO.ReadInt32FromArray(remainingBody, 80);
            this.parameter4 = ReusableIO.ReadInt32FromArray(remainingBody, 84);
            this.resource2.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 88, Constants.CultureCodeEnglish);
            this.resource3.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 96, Constants.CultureCodeEnglish);
            this.resourceUnknown.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 104, Constants.CultureCodeEnglish);
            this.positionCasterX = ReusableIO.ReadInt32FromArray(remainingBody, 112);
            this.positionCasterY = ReusableIO.ReadInt32FromArray(remainingBody, 116);
            this.positionTargetX = ReusableIO.ReadInt32FromArray(remainingBody, 120);
            this.positionTargetY = ReusableIO.ReadInt32FromArray(remainingBody, 124);
            this.parentResourceType = (EffectResourceType)ReusableIO.ReadUInt32FromArray(remainingBody, 128);
            this.parentResource.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 132, Constants.CultureCodeEnglish);
            this.parentResourceFlags = (EffectResourceFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 140);
            this.projectile = ReusableIO.ReadUInt32FromArray(remainingBody, 144);
            this.parentResourceSlot = ReusableIO.ReadInt32FromArray(remainingBody, 148);
            this.LocalVariableName.Source = ReusableIO.ReadStringFromByteArray(remainingBody, 152, Constants.CultureCodeEnglish, 32);
            this.casterLevel = ReusableIO.ReadUInt32FromArray(remainingBody, 184);
            this.unknown2 = ReusableIO.ReadUInt32FromArray(remainingBody, 188);
            this.magicType = (EffectMagicType)ReusableIO.ReadUInt32FromArray(remainingBody, 192);
            Array.Copy(remainingBody, 196, this.unknown3, 0, 60);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.opcode, output);
            ReusableIO.WriteInt32ToStream((SByte)this.target, output);
            ReusableIO.WriteUInt32ToStream(this.power, output);
            ReusableIO.WriteInt32ToStream(this.parameter1, output);
            ReusableIO.WriteInt32ToStream(this.parameter2, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.timingMode, output);
            ReusableIO.WriteUInt16ToStream(this.timingUnknown, output);
            ReusableIO.WriteUInt32ToStream(this.duration, output);
            ReusableIO.WriteUInt16ToStream(this.probability1, output);
            ReusableIO.WriteUInt16ToStream(this.probability2, output);
            ReusableIO.WriteStringToStream(this.resource1.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.diceCount, output);
            ReusableIO.WriteUInt32ToStream(this.diceSides, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.savingThrowType, output);
            ReusableIO.WriteInt32ToStream(this.savingThrowModifier, output);
            ReusableIO.WriteUInt32ToStream(this.setLocalIfNotSet, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.mageSchool, output);
            ReusableIO.WriteUInt16ToStream(this.saveForHalf, output);
            ReusableIO.WriteUInt32ToStream(this.unknown1, output);
            ReusableIO.WriteUInt32ToStream(this.levelAffectedMin, output);
            ReusableIO.WriteUInt32ToStream(this.levelAffectedMax, output);
            ReusableIO.WriteUInt32ToStream(this.resistanceField, output);
            ReusableIO.WriteInt32ToStream(this.parameter3, output);
            ReusableIO.WriteInt32ToStream(this.parameter4, output);
            ReusableIO.WriteStringToStream(this.resource2.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.resource3.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.resourceUnknown.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.positionCasterX, output);
            ReusableIO.WriteInt32ToStream(this.positionCasterY, output);
            ReusableIO.WriteInt32ToStream(this.positionTargetX, output);
            ReusableIO.WriteInt32ToStream(this.positionTargetY, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.parentResourceType, output);
            ReusableIO.WriteStringToStream(this.parentResource.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.parentResourceFlags, output);
            ReusableIO.WriteUInt32ToStream(this.projectile, output);
            ReusableIO.WriteInt32ToStream(this.parentResourceSlot, output);
            ReusableIO.WriteStringToStream(this.LocalVariableName.Source, output, Constants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.casterLevel, output);
            ReusableIO.WriteUInt32ToStream(this.unknown2, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.magicType, output);
            output.Write(this.unknown3, 0, 60);
        }
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
        public String ToString(Boolean showType)
        {
            String header = "Effect Version 2:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 abilityIndex)
        {
            return String.Format("Effect # {0}:", abilityIndex) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Effect opcode"));
            builder.Append(this.opcode);
            builder.Append(StringFormat.ToStringAlignment("Effect target"));
            builder.Append((SByte)this.target);
            builder.Append(StringFormat.ToStringAlignment("Effect target (description)"));
            builder.Append(this.target.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Power"));
            builder.Append(this.power);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 1"));
            builder.Append(this.parameter1);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 2"));
            builder.Append(this.parameter2);
            builder.Append(StringFormat.ToStringAlignment("Timing mode"));
            builder.Append((UInt16)this.timingMode);
            builder.Append(StringFormat.ToStringAlignment("Timing mode (description)"));
            builder.Append(this.timingMode.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Timing unknown"));
            builder.Append((UInt16)this.timingUnknown);
            builder.Append(StringFormat.ToStringAlignment("Effect duration"));
            builder.Append(this.duration);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 1"));
            builder.Append(this.probability1);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 2"));
            builder.Append(this.probability2);
            builder.Append(StringFormat.ToStringAlignment("Effect resource #1"));
            builder.Append(String.Format("'{0}'", this.resource1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Dice count"));
            builder.Append(this.diceCount);
            builder.Append(StringFormat.ToStringAlignment("Dice sides"));
            builder.Append(this.diceSides);
            builder.Append(StringFormat.ToStringAlignment("Saving throw"));
            builder.Append((UInt32)this.savingThrowType);
            builder.Append(StringFormat.ToStringAlignment("Saving throw (enumerated)"));
            builder.Append(this.GetEffectSavingThrowString());
            builder.Append(StringFormat.ToStringAlignment("Saving throw modifier"));
            builder.Append(this.savingThrowModifier);
            builder.Append(StringFormat.ToStringAlignment("Set local variable (if unset?)"));
            builder.Append(this.setLocalIfNotSet);
            builder.Append(StringFormat.ToStringAlignment("Mage School"));
            builder.Append((UInt32)this.mageSchool);
            builder.Append(StringFormat.ToStringAlignment("Mage School (description)"));
            builder.Append(this.mageSchool.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.unknown1);
            builder.Append(StringFormat.ToStringAlignment("Lowest affected level"));
            builder.Append(this.levelAffectedMin);
            builder.Append(StringFormat.ToStringAlignment("Highest affected level"));
            builder.Append(this.levelAffectedMax);
            builder.Append(StringFormat.ToStringAlignment("Resistance"));
            builder.Append(this.resistanceField);
            builder.Append(StringFormat.ToStringAlignment("Resistance (enumerated)"));
            builder.Append(this.GetEffectResistanceString());
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 3"));
            builder.Append(this.parameter3);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 4"));
            builder.Append(this.parameter4);
            builder.Append(StringFormat.ToStringAlignment("Effect resource #2"));
            builder.Append(String.Format("'{0}'", this.resource2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Effect resource #3 (VVC)"));
            builder.Append(String.Format("'{0}'", this.resource3.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Effect resource #4 (unknown)"));
            builder.Append(String.Format("'{0}'", this.resourceUnknown.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Caster coordinate X"));
            builder.Append(this.positionCasterX);
            builder.Append(StringFormat.ToStringAlignment("Caster coordinate Y"));
            builder.Append(this.positionCasterY);
            builder.Append(StringFormat.ToStringAlignment("Target coordinate X"));
            builder.Append(this.positionTargetX);
            builder.Append(StringFormat.ToStringAlignment("Target coordinate Y"));
            builder.Append(this.positionTargetY);
            builder.Append(StringFormat.ToStringAlignment("Parent resource type"));
            builder.Append((UInt32)this.parentResourceType);
            builder.Append(StringFormat.ToStringAlignment("Parent resource type (enumerated)"));
            builder.Append(this.parentResourceType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Parent resource"));
            builder.Append(String.Format("'{0}'", this.parentResource.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Parent resource flag"));
            builder.Append((UInt32)this.parentResourceFlags);
            builder.Append(StringFormat.ToStringAlignment("Parent resource flag (enumerated)"));
            builder.Append(this.parentResourceFlags.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Projectile"));
            builder.Append(this.projectile);
            builder.Append(StringFormat.ToStringAlignment("Parent resource slot"));
            builder.Append(this.parentResourceSlot);
            builder.Append(StringFormat.ToStringAlignment("Local variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.LocalVariableName.Value), 2));
            builder.Append(StringFormat.ToStringAlignment("Caster level"));
            builder.Append(this.casterLevel);
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(this.unknown2);
            builder.Append(StringFormat.ToStringAlignment("Magic type"));
            builder.Append((UInt32)this.magicType);
            builder.Append(StringFormat.ToStringAlignment("Magic type (description)"));
            builder.Append(this.magicType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Trailing unknown 60 Bytes"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown3));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemSpellAbilityEffectResistance flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetEffectResistanceString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Resistance & EffectResistance.Dispellable) == EffectResistance.Dispellable, EffectResistance.Dispellable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Resistance & EffectResistance.IgnoreResistance) == EffectResistance.IgnoreResistance, EffectResistance.IgnoreResistance.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemUability1 flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetEffectSavingThrowString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.savingThrowType & EffectSavingThrow.SpellsOrUnused) == EffectSavingThrow.SpellsOrUnused, EffectSavingThrow.SpellsOrUnused.GetDescription());
            StringFormat.AppendSubItem(sb, (this.savingThrowType & EffectSavingThrow.BreathOrUnused) == EffectSavingThrow.BreathOrUnused, EffectSavingThrow.BreathOrUnused.GetDescription());
            StringFormat.AppendSubItem(sb, (this.savingThrowType & EffectSavingThrow.DeathOrFortitude) == EffectSavingThrow.DeathOrFortitude, EffectSavingThrow.DeathOrFortitude.GetDescription());
            StringFormat.AppendSubItem(sb, (this.savingThrowType & EffectSavingThrow.WandsOrReflex) == EffectSavingThrow.WandsOrReflex, EffectSavingThrow.WandsOrReflex.GetDescription());
            StringFormat.AppendSubItem(sb, (this.savingThrowType & EffectSavingThrow.PolymorphOrWill) == EffectSavingThrow.PolymorphOrWill, EffectSavingThrow.PolymorphOrWill.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}