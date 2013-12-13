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
    /// <summary>Represnets a common ARE file header, used in BG, BG2 and IWD</summary>
    public class CommonAreaHeader : AreaHeaderBase
    {
        #region Fields
        /// <summary>Various area flags</summary>
        public AreaFlags Flags { get; set; }

        /// <summary>Type of area (location in the planes)</summary>
        public AreaTypeFlags AreaType { get; set; }

        /// <summary>Padding at offset 0xE4</summary>
        /// <remarks>56 bytes in length</remarks>
        public Byte[] Padding_0x00E4 { get; set; }
        #endregion


        #region IO method implemetations
        #region Read
        /// <summary>Reads the area flags value from the Byte buffer</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaFlags(Byte[] buffer)
        {
            this.Flags = (AreaFlags)ReusableIO.ReadUInt32FromArray(buffer, 12);
        }

        /// <summary>Reads the area type flags value from the Byte buffer array</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaTypeFlags(Byte[] buffer)
        {
            this.AreaType = (AreaTypeFlags)ReusableIO.ReadUInt32FromArray(buffer, 64);
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

            builder.Append(this.GenerateAreaDescriptionString());
            builder.Append(this.GenerateLeadingHeaderString());
            builder.Append(this.GenerateOffsetGroup1String());
            builder.Append(this.GenerateOffsetGroup2String());
            builder.AppendLine(this.GenerateTrailingPaddingString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the type of the area</summary>
        /// <returns>A human-readable String representing the type of the area</returns>
        protected override String GenerateAreaDescriptionString()
        {
            return "Common Area Header:";
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

            StringFormat.AppendSubItem(builder, (this.Flags & AreaFlags.SaveAllowed) == AreaFlags.SaveAllowed, AreaFlags.SaveAllowed.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AreaFlags.TutorialArea) == AreaFlags.TutorialArea, AreaFlags.TutorialArea.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AreaFlags.DeadMagicArea) == AreaFlags.DeadMagicArea, AreaFlags.DeadMagicArea.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AreaFlags.DreamArea) == AreaFlags.DreamArea, AreaFlags.DreamArea.GetDescription());

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