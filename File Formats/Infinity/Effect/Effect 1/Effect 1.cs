using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1
{
    public class Effect1 : EffectBase
    {
        /// <summary>Size of the effect structure</summary>
        public const Int32 StructSize = 48;    //yes, hard-coded. These data structures have a static size.
        
        #region Members
        /// <summary>Effect Type/Opcode</summary>
        protected UInt16 opcode;

        /// <summary>Target of the effect</summary>
        protected EffectTarget target;

        /// <summary>Level of the overlay? Level of the caster?</summary>
        protected Byte power;

        /// <summary>Timing of the effect</summary>
        protected EffectTimingMode1 timingMode;

        /// <summary>Resistance against the effect</summary>
        protected EffectResistance resistance;

        /// <summary>First probability of the effect</summary>
        protected Byte probability1;

        /// <summary>Second probability of the effect</summary>
        protected Byte probability2;
        #endregion

        #region Properties
        public UInt16 Opcode
        {
            get { return opcode; }
            set { opcode = value; }
        }

        public EffectTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        public Byte Power
        {
            get { return power; }
            set { power = value; }
        }

        public EffectTimingMode1 TimingMode
        {
            get { return timingMode; }
            set { timingMode = value; }
        }

        public EffectResistance Resistance
        {
            get { return resistance; }
            set { resistance = value; }
        }

        public Byte Probability1
        {
            get { return probability1; }
            set { probability1 = value; }
        }

        public Byte Probability2
        {
            get { return probability2; }
            set { probability2 = value; }
        }
        #endregion
        
        #region Constructor(s)
        public Effect1()
        {
            this.resource1 = null;
        }
        #endregion
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.resource1 = new ResourceReference();
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

            //read effect
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 48);

            this.opcode = ReusableIO.ReadUInt16FromArray(remainingBody, 0);
            this.target = (EffectTarget)remainingBody[2];
            this.power = remainingBody[3];
            this.parameter1 = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.parameter2 = ReusableIO.ReadInt32FromArray(remainingBody, 8);
            this.timingMode = (EffectTimingMode1)remainingBody[12];
            this.resistance = (EffectResistance)remainingBody[13];
            this.duration = ReusableIO.ReadUInt32FromArray(remainingBody, 14);
            this.probability1 = remainingBody[18];
            this.probability2 = remainingBody[19];
            this.resource1.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 20, Constants.CultureCodeEnglish);
            this.diceCount = ReusableIO.ReadUInt32FromArray(remainingBody, 28);
            this.diceSides = ReusableIO.ReadUInt32FromArray(remainingBody, 32);
            this.savingThrowType = (EffectSavingThrow)ReusableIO.ReadUInt32FromArray(remainingBody, 36);
            this.savingThrowModifier = ReusableIO.ReadInt32FromArray(remainingBody, 40);
            this.unknown1 = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.opcode, output);
            output.WriteByte((Byte)this.target);
            output.WriteByte(this.power);
            ReusableIO.WriteInt32ToStream(this.parameter1, output);
            ReusableIO.WriteInt32ToStream(this.parameter2, output);
            output.WriteByte((Byte)this.timingMode);
            output.WriteByte((Byte)this.resistance);
            ReusableIO.WriteUInt32ToStream(this.duration, output);
            output.WriteByte(this.probability1);
            output.WriteByte(this.probability2);
            ReusableIO.WriteStringToStream(this.resource1.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.diceCount, output);
            ReusableIO.WriteUInt32ToStream(this.diceSides, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.savingThrowType, output);
            ReusableIO.WriteInt32ToStream(this.savingThrowModifier, output);
            ReusableIO.WriteUInt32ToStream(this.unknown1, output);
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
            String header = "Effect Version 1:";

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
            return StringFormat.ReturnAndIndent(String.Format("Effect # {0}:", abilityIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Effect opcode"));
            builder.Append(this.opcode);
            builder.Append(StringFormat.ToStringAlignment("Effect target"));
            builder.Append((Byte)this.target);
            builder.Append(StringFormat.ToStringAlignment("Effect target (description)"));
            builder.Append(this.target.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Power"));
            builder.Append(this.power);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 1"));
            builder.Append(this.parameter1);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 2"));
            builder.Append(this.parameter2);
            builder.Append(StringFormat.ToStringAlignment("Timing mode"));
            builder.Append((Byte)this.timingMode);
            builder.Append(StringFormat.ToStringAlignment("Timing mode (description)"));
            builder.Append(this.timingMode.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Resistance"));
            builder.Append((Byte)this.resistance);
            builder.Append(StringFormat.ToStringAlignment("Resistance (enumerated)"));
            builder.Append(this.GetEffectResistanceString());
            builder.Append(StringFormat.ToStringAlignment("Effect duration"));
            builder.Append(this.duration);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 1"));
            builder.Append(this.probability1);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 2"));
            builder.Append(this.probability2);
            builder.Append(StringFormat.ToStringAlignment("Effect resource"));
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
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.unknown1);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemSpellAbilityEffectResistance flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetEffectResistanceString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.resistance & EffectResistance.Dispellable) == EffectResistance.Dispellable, EffectResistance.Dispellable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.resistance & EffectResistance.IgnoreResistance) == EffectResistance.IgnoreResistance, EffectResistance.IgnoreResistance.GetDescription());

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