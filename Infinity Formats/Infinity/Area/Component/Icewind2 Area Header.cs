using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represnets an IWD2 ARE file header</summary>
    public class Icewind2AreaHeader : AreaHeaderBase
    {
        #region Fields
        /// <summary>Various area flags</summary>
        public Icewind2AreaFlags Flags { get; set; }

        /// <summary>Type of area (location in the planes)</summary>
        public AreaTypeFlags AreaType { get; set; }

        /// <summary>Area is difficulty level 2</summary>
        public Byte Difficulty2 { get; set; }

        /// <summary>Area is difficulty level 3</summary>
        public Byte Difficulty3 { get; set; }

        /// <summary>Unknown at offset 0x56</summary>
        public Int16 Unknown_0x0056 { get; set; }

        /// <summary>Unknown 12 Bytes at offset 0x58</summary>
        public Byte[] Unknown_0x0058 { get; set; }

        /// <summary>Padding at offset 0xE4</summary>
        /// <remarks>56 bytes in length</remarks>
        public Byte[] Padding_0x00E4 { get; set; }
        #endregion


        #region IO method implemetations
        #region Read
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            //read first data groups
            this.ReadLeadingData(input);

            //read the difficulty 16-byte structure
            this.ReadIcewind2Difficulty(input);

            //offset group
            this.ReadOffsetGroup1(input);

            //offset group #3
            this.ReadTrailingData(input);

            //read trailing padding bytes
            this.ReadTrailingPadding(input);
        }

        /// <summary>Reads the area flags value from the Byte buffer</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaFlags(Byte[] buffer)
        {
            this.Flags = (Icewind2AreaFlags)ReusableIO.ReadUInt32FromArray(buffer, 12);
        }

        /// <summary>Reads the area type flags value from the Byte buffer array</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaTypeFlags(Byte[] buffer)
        {
            this.AreaType = (AreaTypeFlags)ReusableIO.ReadUInt16FromArray(buffer, 64);
        }

        /// <summary>Reads the Icewind Dale 2 difficulty struct from the input stream if available</summary>
        /// <param name="input">Input Stream to read from</param>
        protected void ReadIcewind2Difficulty(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 4);

            this.Difficulty2 = buffer[0];
            this.Difficulty3 = buffer[1];
            this.Unknown_0x0056 = ReusableIO.ReadInt16FromArray(buffer, 2);
            this.Unknown_0x0058 = ReusableIO.BinaryRead(input, 12);
        }

        /// <summary>Reads the trailing padding bytes for the header from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        protected override void ReadTrailingPadding(Stream input)
        {
            this.Padding_0x00E4 = ReusableIO.BinaryRead(input, 56);
        }
        #endregion


        #region Write
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteLeadingData(output);
            this.WriteIcewind2Difficulty(output);
            this.WriteOffsetGroup1(output);
            this.WriteTrailingData(output);
            this.WriteTrailingPadding(output);
        }

        /// <summary>Writes the area flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteAreaFlags(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
        }

        /// <summary>Writes the area type flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteAreaTypeFlags(Stream output)
        {
            ReusableIO.WriteUInt16ToStream((UInt16)this.AreaType, output);
        }

        /// <summary>Writes the Icewind Dale 2 difficulty struct to the output stream if available</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteIcewind2Difficulty(Stream output)
        {
            output.WriteByte(this.Difficulty2);
            output.WriteByte(this.Difficulty3);
            ReusableIO.WriteInt16ToStream(this.Unknown_0x0056, output);
            output.Write(this.Unknown_0x0058, 0, 12);
        }

        /// <summary>Writes the trailing padding bytes for the header to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteTrailingPadding(Stream output)
        {
            output.Write(this.Padding_0x00E4, 0, 56);
        }
        #endregion
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(this.GenerateAreaDescriptionString());
            builder.AppendLine(this.GenerateLeadingHeaderString());
            builder.AppendLine(this.GenerateIcewind2DifficultyString());
            builder.AppendLine(this.GenerateOffsetGroup1String());
            builder.AppendLine(this.GenerateOffsetGroup2String());
            builder.AppendLine(this.GenerateTrailingPaddingString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the type of the area</summary>
        /// <returns>A human-readable String representing the type of the area</returns>
        protected override String GenerateAreaDescriptionString()
        {
            return "Icewind 2 Area Header:";
        }

        /// <summary>Generates a human-readable String representing the area flags and values</summary>
        /// <returns>A human-readable String representing the area flags and values</returns>
        protected override String GenerateAreaFlagsString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Area flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Area flags (enumerated)"));
            builder.Append(this.GenerateAreaFlagsEnumerationString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the area type flags and values</summary>
        /// <returns>A human-readable String representing the area type flags and values</returns>
        protected override String GenerateAreaTypeFlagsString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Area Type flags (value)"));
            builder.Append((UInt32)this.AreaType);
            builder.Append(StringFormat.ToStringAlignment("Area Type (enumerated)"));
            builder.Append(this.GenerateAreaTypeFlagsEnumerationString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the IWD2 difficulty structure</summary>
        /// <returns>A human-readable String representing the IWD2 difficulty structure</returns>
        protected String GenerateIcewind2DifficultyString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Difficulty level 2"));
            builder.Append(this.Difficulty2);
            builder.Append(StringFormat.ToStringAlignment("Difficulty level 3"));
            builder.Append(this.Difficulty3);
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x0056"));
            builder.Append(this.Unknown_0x0056);
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x0058"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown_0x0058));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the trailing padding of the header</summary>
        /// <returns>A human-readable String representing the trailing padding of the header</returns>
        protected override String GenerateTrailingPaddingString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Trailing padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x00E4));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable enumeration of flags set for RestFlags</summary>
        /// <returns>A human-readable enumeration of flags set for RestFlags</returns>
        protected String GenerateAreaFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & Icewind2AreaFlags.SaveDisabled) == Icewind2AreaFlags.SaveDisabled, Icewind2AreaFlags.SaveDisabled.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & Icewind2AreaFlags.RestDisabled) == Icewind2AreaFlags.RestDisabled, Icewind2AreaFlags.RestDisabled.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & Icewind2AreaFlags.LockBattleMusic) == Icewind2AreaFlags.LockBattleMusic, Icewind2AreaFlags.LockBattleMusic.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable enumeration of flags set for AreaType</summary>
        /// <returns>A human-readable enumeration of flags set for RestFlags</returns>
        protected String GenerateAreaTypeFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.Outdoor) == AreaTypeFlags.Outdoor, AreaTypeFlags.Outdoor.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.HasSolarLight) == AreaTypeFlags.HasSolarLight, AreaTypeFlags.HasSolarLight.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.HasWeather) == AreaTypeFlags.HasWeather, AreaTypeFlags.HasWeather.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.IsCity) == AreaTypeFlags.IsCity, AreaTypeFlags.IsCity.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.IsForest) == AreaTypeFlags.IsForest, AreaTypeFlags.IsForest.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.IsDungeon) == AreaTypeFlags.IsDungeon, AreaTypeFlags.IsDungeon.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.HasLongNight) == AreaTypeFlags.HasLongNight, AreaTypeFlags.HasLongNight.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & AreaTypeFlags.IndoorRestAllowed) == AreaTypeFlags.IndoorRestAllowed, AreaTypeFlags.IndoorRestAllowed.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}