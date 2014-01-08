using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an instance of an ambient sound</summary>
    public class Ambient : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 212;
        #endregion


        #region Fields
        /// <summary>Ambient's name</summary>
        public ZString Name { get; set; }

        /// <summary>Current position of the ambient</summary>
        public Point Position { get; set; }

        /// <summary>Radius of the ambient sound</summary>
        public Int16 Radius { get; set; }

        /// <summary>Height from which the sound occurs</summary>
        public Int16 Height { get; set; }

        /// <summary>Pitch variance</summary>
        public Int32 PitchVariance { get; set; }

        /// <summary>Volume variance</summary>
        public Int16 VolumeVariance { get; set; }

        /// <summary>Volume % at which the ambient is played</summary>
        public Int16 Volume { get; set; }

        /// <summary>Array of sounds to be played</summary>
        public ResourceReference[] Sounds { get; set; }

        /// <summary>Count of sounds</summary>
        public Int16 CountSounds { get; set; }

        /// <summary>Two bytes at offset 0x82 of structure memory alignment</summary>
        public Int16 Padding_0x0082 { get; set; }

        /// <summary>Base dela between ambients</summary>
        public Int32 DelayBase { get; set; }

        /// <summary>Deviation between delays</summary>
        /// <remarks>Time between start of consecutive sounds uses a uniform distribution (discrete?) over the range (DelayBase - DelayDeviation) to (DelayBase + DelayDeviation)</remarks>
        public Int32 DelayDeviation { get; set; }

        /// <summary>Schedule flags during which this ambient is valid</summary>
        public Schedule ApplicationSchedule { get; set; }

        /// <summary>Miscellaneous ambient flags</summary>
        public AmbientFlags Flags { get; set; }

        /// <summary>64 bytes of padding at offset 0x94</summary>
        public Byte[] Padding_0x0094 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Position = new Point();
            this.Sounds = new ResourceReference[10];
            for (Int32 index = 0; index < 10; ++index)
                this.Sounds[index] = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 148);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Position.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Position.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.Radius = ReusableIO.ReadInt16FromArray(buffer, 36);
            this.Height = ReusableIO.ReadInt16FromArray(buffer, 38);
            this.PitchVariance = ReusableIO.ReadInt32FromArray(buffer, 40);
            this.VolumeVariance = ReusableIO.ReadInt16FromArray(buffer, 44);
            this.Volume = ReusableIO.ReadInt16FromArray(buffer, 46);

            for (Int32 index = 0; index < 10; ++index)
                this.Sounds[index].ResRef = ReusableIO.ReadStringFromByteArray(buffer, 48 + (8 * index), CultureConstants.CultureCodeEnglish);

            this.CountSounds = ReusableIO.ReadInt16FromArray(buffer, 128);
            this.Padding_0x0082 = ReusableIO.ReadInt16FromArray(buffer, 130);
            this.DelayBase = ReusableIO.ReadInt32FromArray(buffer, 132);
            this.DelayDeviation = ReusableIO.ReadInt32FromArray(buffer, 136);
            this.ApplicationSchedule = (Schedule)ReusableIO.ReadUInt32FromArray(buffer, 140);
            this.Flags = (AmbientFlags)ReusableIO.ReadUInt32FromArray(buffer, 144);

            this.Padding_0x0094 = ReusableIO.BinaryRead(input, 64);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Position.X, output);
            ReusableIO.WriteUInt16ToStream(this.Position.Y, output);
            ReusableIO.WriteInt16ToStream(this.Radius, output);
            ReusableIO.WriteInt16ToStream(this.Height, output);
            ReusableIO.WriteInt32ToStream(this.PitchVariance, output);
            ReusableIO.WriteInt16ToStream(this.VolumeVariance, output);
            ReusableIO.WriteInt16ToStream(this.Volume, output);

            for (Int32 index = 0; index < 10; ++index)
                ReusableIO.WriteStringToStream(this.Sounds[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            ReusableIO.WriteInt16ToStream(this.CountSounds, output);
            ReusableIO.WriteInt16ToStream(this.Padding_0x0082, output);
            ReusableIO.WriteInt32ToStream(this.DelayBase, output);
            ReusableIO.WriteInt32ToStream(this.DelayDeviation, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ApplicationSchedule, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            output.Write(this.Padding_0x0094, 0, 64);
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
            return StringFormat.ReturnAndIndent(String.Format("Ambient # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Ambient:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Current position"));
            builder.Append(this.Position.ToString());
            builder.Append(StringFormat.ToStringAlignment("Radius"));
            builder.Append(this.Radius);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Pitch variance"));
            builder.Append(this.PitchVariance);
            builder.Append(StringFormat.ToStringAlignment("Volume variance"));
            builder.Append(this.VolumeVariance);
            builder.Append(StringFormat.ToStringAlignment("Volume %"));
            builder.Append(this.Volume);

            for (Int32 index = 0; index < 10; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Ambient sound # {0}", index)));
                builder.Append(String.Format("'{0}'", this.Sounds[index].ZResRef));
            }

            builder.Append(StringFormat.ToStringAlignment("Count of sounds"));
            builder.Append(this.CountSounds);
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x82"));
            builder.Append(this.Padding_0x0082);
            builder.Append(StringFormat.ToStringAlignment("Base delay between sounds"));
            builder.Append(this.DelayBase);
            builder.Append(StringFormat.ToStringAlignment("Deviation of delay between sounds"));
            builder.Append(this.DelayDeviation);
            builder.Append(StringFormat.ToStringAlignment("Appearance Schedule (value)"));
            builder.Append((UInt32)this.ApplicationSchedule);
            builder.Append(StringFormat.ToStringAlignment("Appearance Schedule (enumeration)"));
            builder.Append(this.GetScheduleFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0094));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & AmbientFlags.Enabled) == AmbientFlags.Enabled, AmbientFlags.Enabled.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AmbientFlags.Looping) == AmbientFlags.Looping, AmbientFlags.Looping.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AmbientFlags.GlobalSound) == AmbientFlags.GlobalSound, AmbientFlags.GlobalSound.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AmbientFlags.RandomSelection) == AmbientFlags.RandomSelection, AmbientFlags.RandomSelection.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AmbientFlags.DisableIfLowMemory) == AmbientFlags.DisableIfLowMemory, AmbientFlags.DisableIfLowMemory.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Gets a human-readable enumeration String of set Schedule enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Schedule enumeration values</returns>
        protected String GetScheduleFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_00_30_to_01_30) == Schedule.Time_00_30_to_01_30, Schedule.Time_00_30_to_01_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_01_30_to_02_30) == Schedule.Time_01_30_to_02_30, Schedule.Time_01_30_to_02_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_02_30_to_03_30) == Schedule.Time_02_30_to_03_30, Schedule.Time_02_30_to_03_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_03_30_to_04_30) == Schedule.Time_03_30_to_04_30, Schedule.Time_03_30_to_04_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_04_30_to_05_30) == Schedule.Time_04_30_to_05_30, Schedule.Time_04_30_to_05_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_05_30_to_06_30) == Schedule.Time_05_30_to_06_30, Schedule.Time_05_30_to_06_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_06_30_to_07_30) == Schedule.Time_06_30_to_07_30, Schedule.Time_06_30_to_07_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_07_30_to_08_30) == Schedule.Time_07_30_to_08_30, Schedule.Time_07_30_to_08_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_08_30_to_09_30) == Schedule.Time_08_30_to_09_30, Schedule.Time_08_30_to_09_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_09_30_to_10_30) == Schedule.Time_09_30_to_10_30, Schedule.Time_09_30_to_10_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_10_30_to_11_30) == Schedule.Time_10_30_to_11_30, Schedule.Time_10_30_to_11_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_11_30_to_12_30) == Schedule.Time_11_30_to_12_30, Schedule.Time_11_30_to_12_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_12_30_to_13_30) == Schedule.Time_12_30_to_13_30, Schedule.Time_12_30_to_13_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_13_30_to_14_30) == Schedule.Time_13_30_to_14_30, Schedule.Time_13_30_to_14_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_14_30_to_15_30) == Schedule.Time_14_30_to_15_30, Schedule.Time_14_30_to_15_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_15_30_to_16_30) == Schedule.Time_15_30_to_16_30, Schedule.Time_15_30_to_16_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_16_30_to_17_30) == Schedule.Time_16_30_to_17_30, Schedule.Time_16_30_to_17_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_17_30_to_18_30) == Schedule.Time_17_30_to_18_30, Schedule.Time_17_30_to_18_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_18_30_to_19_30) == Schedule.Time_18_30_to_19_30, Schedule.Time_18_30_to_19_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_19_30_to_20_30) == Schedule.Time_19_30_to_20_30, Schedule.Time_19_30_to_20_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_20_30_to_21_30) == Schedule.Time_20_30_to_21_30, Schedule.Time_20_30_to_21_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_21_30_to_22_30) == Schedule.Time_21_30_to_22_30, Schedule.Time_21_30_to_22_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_22_30_to_23_30) == Schedule.Time_22_30_to_23_30, Schedule.Time_22_30_to_23_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_23_30_to_00_30) == Schedule.Time_23_30_to_00_30, Schedule.Time_23_30_to_00_30.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}