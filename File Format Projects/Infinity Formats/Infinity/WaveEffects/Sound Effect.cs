using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WaveEffects
{
    /// <summary>Represents audio effects for sound playback</summary>
    public class SoundEffect : InfinityFormat
    {
        #region Construction
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x0108;
        #endregion


        #region Fields
        /// <summary>SR Curve radius</summary>
        public Int32 CurveRadius { get; set; }

        /// <summary>Various sound playback flags</summary>
        public WaveEffectFlag Flags { get; set; }

        public Int32 RandomFrequencyVariation { get; set; }

        /// <value>Is a percentage</value>
        public Int32 RandomVolumeVariation { get; set; }

        /// <summary>240 bytes of padding at offset 0x18</summary>
        public Byte[] Padding_0x0018 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 16);

            this.CurveRadius = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.Flags = (WaveEffectFlag)ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.RandomFrequencyVariation = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.RandomVolumeVariation = ReusableIO.ReadInt32FromArray(buffer, 12);

            this.Padding_0x0018 = ReusableIO.BinaryRead(input, 240);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteInt32ToStream(this.CurveRadius, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt32ToStream(this.RandomFrequencyVariation, output);
            ReusableIO.WriteInt32ToStream(this.RandomVolumeVariation, output);

            output.Write(this.Padding_0x0018, 0, 240);
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
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Wave effect # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Wave effect:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Curve radius"));
            builder.Append(this.CurveRadius);
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GenerateFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Random frequency variation"));
            builder.Append(this.RandomFrequencyVariation);
            builder.Append(StringFormat.ToStringAlignment("Random volume variation"));
            builder.Append(this.RandomVolumeVariation);
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x18"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0018));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable enumeration of flags set for Flags</summary>
        /// <returns>A human-readable enumeration of flags set for Flags</returns>
        protected String GenerateFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & WaveEffectFlag.PlaySoundLikeCutscene) == WaveEffectFlag.PlaySoundLikeCutscene, WaveEffectFlag.PlaySoundLikeCutscene.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & WaveEffectFlag.UseCurveRadius) == WaveEffectFlag.UseCurveRadius, WaveEffectFlag.UseCurveRadius.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & WaveEffectFlag.UseRandomFrequencyVariation) == WaveEffectFlag.UseRandomFrequencyVariation, WaveEffectFlag.UseRandomFrequencyVariation.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & WaveEffectFlag.UseRandomVolumeVariation) == WaveEffectFlag.UseRandomVolumeVariation, WaveEffectFlag.UseRandomVolumeVariation.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & WaveEffectFlag.DisableEAX) == WaveEffectFlag.DisableEAX, WaveEffectFlag.DisableEAX.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}