using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1
{
    /// <summary>A version 1 effect</summary>
    public class Effect1 : EffectBase
    {
        #region Constants
        /// <summary>Size of the effect structure</summary>
        public const Int32 StructSize = 48;    //These data structures have a static size.
        #endregion


        #region Members
        /// <summary>Effect Type/Opcode</summary>
        public UInt16 Opcode { get; set; }

        /// <summary>Target of the effect</summary>
        public EffectTarget Target { get; set; }

        /// <summary>Level of the overlay? Level of the caster?</summary>
        public Byte Power { get; set; }

        /// <summary>Timing of the effect</summary>
        public EffectTimingMode1 TimingMode { get; set; }

        /// <summary>Resistance against the effect</summary>
        public EffectResistance Resistance { get; set; }

        /// <summary>First probability of the effect</summary>
        public Byte Probability1 { get; set; }

        /// <summary>Second probability of the effect</summary>
        public Byte Probability2 { get; set; }
        #endregion
        

        #region Construction
        /// <summary>Default constructor</summary>
        public Effect1()
        {
            this.Resource1 = null;
        }
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Resource1 = new ResourceReference();
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
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 48);

            this.Opcode = ReusableIO.ReadUInt16FromArray(remainingBody, 0);
            this.Target = (EffectTarget)remainingBody[2];
            this.Power = remainingBody[3];
            this.Parameter1 = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.Parameter2 = ReusableIO.ReadInt32FromArray(remainingBody, 8);
            this.TimingMode = (EffectTimingMode1)remainingBody[12];
            this.Resistance = (EffectResistance)remainingBody[13];
            this.Duration = ReusableIO.ReadUInt32FromArray(remainingBody, 14);
            this.Probability1 = remainingBody[18];
            this.Probability2 = remainingBody[19];
            this.Resource1.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 20, CultureConstants.CultureCodeEnglish);
            this.DiceCount = ReusableIO.ReadUInt32FromArray(remainingBody, 28);
            this.DiceSides = ReusableIO.ReadUInt32FromArray(remainingBody, 32);
            this.SavingThrowType = (EffectSavingThrow)ReusableIO.ReadUInt32FromArray(remainingBody, 36);
            this.SavingThrowModifier = ReusableIO.ReadInt32FromArray(remainingBody, 40);
            this.Special = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Opcode, output);
            output.WriteByte((Byte)this.Target);
            output.WriteByte(this.Power);
            ReusableIO.WriteInt32ToStream(this.Parameter1, output);
            ReusableIO.WriteInt32ToStream(this.Parameter2, output);
            output.WriteByte((Byte)this.TimingMode);
            output.WriteByte((Byte)this.Resistance);
            ReusableIO.WriteUInt32ToStream(this.Duration, output);
            output.WriteByte(this.Probability1);
            output.WriteByte(this.Probability2);
            ReusableIO.WriteStringToStream(this.Resource1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.DiceCount, output);
            ReusableIO.WriteUInt32ToStream(this.DiceSides, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.SavingThrowType, output);
            ReusableIO.WriteInt32ToStream(this.SavingThrowModifier, output);
            ReusableIO.WriteUInt32ToStream(this.Special, output);
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
            builder.Append(this.Opcode);
            builder.Append(StringFormat.ToStringAlignment("Effect target"));
            builder.Append((Byte)this.Target);
            builder.Append(StringFormat.ToStringAlignment("Effect target (description)"));
            builder.Append(this.Target.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Power"));
            builder.Append(this.Power);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 1"));
            builder.Append(this.Parameter1);
            builder.Append(StringFormat.ToStringAlignment("Effect Parameter 2"));
            builder.Append(this.Parameter2);
            builder.Append(StringFormat.ToStringAlignment("Timing mode"));
            builder.Append((Byte)this.TimingMode);
            builder.Append(StringFormat.ToStringAlignment("Timing mode (description)"));
            builder.Append(this.TimingMode.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Resistance"));
            builder.Append((Byte)this.Resistance);
            builder.Append(StringFormat.ToStringAlignment("Resistance (enumerated)"));
            builder.Append(this.GetEffectResistanceString());
            builder.Append(StringFormat.ToStringAlignment("Effect duration"));
            builder.Append(this.Duration);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 1"));
            builder.Append(this.Probability1);
            builder.Append(StringFormat.ToStringAlignment("Effect probability 2"));
            builder.Append(this.Probability2);
            builder.Append(StringFormat.ToStringAlignment("Effect resource"));
            builder.Append(String.Format("'{0}'", this.Resource1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Dice count"));
            builder.Append(this.DiceCount);
            builder.Append(StringFormat.ToStringAlignment("Dice sides"));
            builder.Append(this.DiceSides);
            builder.Append(StringFormat.ToStringAlignment("Saving throw"));
            builder.Append((UInt32)this.SavingThrowType);
            builder.Append(StringFormat.ToStringAlignment("Saving throw (enumerated)"));
            builder.Append(this.GetEffectSavingThrowString());
            builder.Append(StringFormat.ToStringAlignment("Saving throw modifier"));
            builder.Append(this.SavingThrowModifier);
            builder.Append(StringFormat.ToStringAlignment("Special value (unknown purpose)"));
            builder.Append(this.Special);

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

            StringFormat.AppendSubItem(sb, (this.SavingThrowType & EffectSavingThrow.SpellsOrUnused) == EffectSavingThrow.SpellsOrUnused, EffectSavingThrow.SpellsOrUnused.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SavingThrowType & EffectSavingThrow.BreathOrUnused) == EffectSavingThrow.BreathOrUnused, EffectSavingThrow.BreathOrUnused.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SavingThrowType & EffectSavingThrow.DeathOrFortitude) == EffectSavingThrow.DeathOrFortitude, EffectSavingThrow.DeathOrFortitude.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SavingThrowType & EffectSavingThrow.WandsOrReflex) == EffectSavingThrow.WandsOrReflex, EffectSavingThrow.WandsOrReflex.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SavingThrowType & EffectSavingThrow.PolymorphOrWill) == EffectSavingThrow.PolymorphOrWill, EffectSavingThrow.PolymorphOrWill.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}