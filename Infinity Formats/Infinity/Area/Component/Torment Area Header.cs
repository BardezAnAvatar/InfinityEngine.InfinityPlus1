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
    /// <summary>Represnets a PS:T ARE file header</summary>
    public class TormentAreaHeader : AreaHeaderBase
    {
        #region Fields

        /// <summary>Rest availability flags</summary>
        public TormentAreaRestFlags RestFlags { get; set; }

        /// <summary>Type of area (location in the planes)</summary>
        public TormentAreaTypeFlags AreaType { get; set; }

        /// <summary>Unknown at offset 0xC4</summary>
        public Int32 Unknown_0x00C4 { get; set; }

        /// <summary>Padding at offset 0xE8</summary>
        /// <remarks>52 bytes in length</remarks>
        public Byte[] Padding_0x00E8 { get; set; }
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

            //offset group
            this.ReadOffsetGroup1(input);

            //torment offset; read or don't
            this.ReadTormentUnknownOffset(input);

            //offset group #3
            this.ReadTrailingData(input);

            //read trailing padding bytes
            this.ReadTrailingPadding(input);
        }

        /// <summary>Reads the area flags value from the Byte buffer</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaFlags(Byte[] buffer)
        {
            this.RestFlags = (TormentAreaRestFlags)ReusableIO.ReadUInt32FromArray(buffer, 12);
        }

        /// <summary>Reads the area type flags value from the Byte buffer array</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected override void ReadAreaTypeFlags(Byte[] buffer)
        {
            this.AreaType = (TormentAreaTypeFlags)ReusableIO.ReadUInt16FromArray(buffer, 64);
        }

        /// <summary>Reads the PS:T unknown at offset 0x00C4 if available, or doesn't</summary>
        /// <param name="input">Input Stream to read from</param>
        protected void ReadTormentUnknownOffset(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 4);
            this.Unknown_0x00C4 = ReusableIO.ReadInt32FromArray(buffer, 0);
        }

        /// <summary>Reads the trailing padding bytes for the header from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        protected override void ReadTrailingPadding(Stream input)
        {
            this.Padding_0x00E8 = ReusableIO.BinaryRead(input, 52);
        }
        #endregion


        #region Write
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteLeadingData(output);
            this.WriteOffsetGroup1(output);
            this.WriteTormentUnknownOffset(output);
            this.WriteTrailingData(output);
            this.WriteTrailingPadding(output);
        }

        /// <summary>Writes the area flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteAreaFlags(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.RestFlags, output);
        }

        /// <summary>Writes the area type flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteAreaTypeFlags(Stream output)
        {
            ReusableIO.WriteUInt16ToStream((UInt16)this.AreaType, output);
        }

        /// <summary>Writes the PS:T unknown at offset 0x00C4 if available, or doesn't</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTormentUnknownOffset(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.Unknown_0x00C4, output);
        }

        /// <summary>Writes the trailing padding bytes for the header to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteTrailingPadding(Stream output)
        {
            output.Write(this.Padding_0x00E8, 0, 52);
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
            builder.AppendLine(this.GenerateOffsetGroup1String());
            builder.AppendLine(this.GenerateTormentUnknownOffsetString());
            builder.AppendLine(this.GenerateOffsetGroup2String());
            builder.AppendLine(this.GenerateTrailingPaddingString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the type of the area</summary>
        /// <returns>A human-readable String representing the type of the area</returns>
        protected override String GenerateAreaDescriptionString()
        {
            return "Torment Area Header:";
        }

        /// <summary>Generates a human-readable String representing the area flags and values</summary>
        /// <returns>A human-readable String representing the area flags and values</returns>
        protected override String GenerateAreaFlagsString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Rest flags (value)"));
            builder.Append((UInt32)this.RestFlags);
            builder.Append(StringFormat.ToStringAlignment("Rest flags (enumerated)"));
            builder.Append(this.GenerateAreaRestFlagsEnumerationString());

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
        
        /// <summary>Generates a human-readable String representing the PS:T unknown offset</summary>
        /// <returns>A human-readable String representing the PS:T unknown offset</returns>
        protected String GenerateTormentUnknownOffsetString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x00C4"));
            builder.Append(this.Unknown_0x00C4);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the trailing padding of the header</summary>
        /// <returns>A human-readable String representing the trailing padding of the header</returns>
        protected override String GenerateTrailingPaddingString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Trailing padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x00E8));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable enumeration of flags set for RestFlags</summary>
        /// <returns>A human-readable enumeration of flags set for RestFlags</returns>
        protected String GenerateAreaRestFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.RestAllowed) == TormentAreaRestFlags.RestAllowed, TormentAreaRestFlags.RestAllowed.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.RestAllowedDupe) == TormentAreaRestFlags.RestAllowedDupe, TormentAreaRestFlags.RestAllowedDupe.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.CannotRest) == TormentAreaRestFlags.CannotRest, TormentAreaRestFlags.CannotRest.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.CannotRestDupe) == TormentAreaRestFlags.CannotRestDupe, TormentAreaRestFlags.CannotRestDupe.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.CannotRestRightNow) == TormentAreaRestFlags.CannotRestRightNow, TormentAreaRestFlags.CannotRestRightNow.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.CannotRestRightNowDupe) == TormentAreaRestFlags.CannotRestRightNowDupe, TormentAreaRestFlags.CannotRestRightNowDupe.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.RestNeedPermission) == TormentAreaRestFlags.RestNeedPermission, TormentAreaRestFlags.RestNeedPermission.GetDescription());
            StringFormat.AppendSubItem(builder, (this.RestFlags & TormentAreaRestFlags.RestNeedPermissionDupe) == TormentAreaRestFlags.RestNeedPermissionDupe, TormentAreaRestFlags.RestNeedPermissionDupe.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable enumeration of flags set for AreaType</summary>
        /// <returns>A human-readable enumeration of flags set for RestFlags</returns>
        protected String GenerateAreaTypeFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.Hive) == TormentAreaTypeFlags.Hive, TormentAreaTypeFlags.Hive.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.HiveNight) == TormentAreaTypeFlags.HiveNight, TormentAreaTypeFlags.HiveNight.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.ClerksWard) == TormentAreaTypeFlags.ClerksWard, TormentAreaTypeFlags.ClerksWard.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.LowerWard) == TormentAreaTypeFlags.LowerWard, TormentAreaTypeFlags.LowerWard.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.RavelsMaze) == TormentAreaTypeFlags.RavelsMaze, TormentAreaTypeFlags.RavelsMaze.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.Baator) == TormentAreaTypeFlags.Baator, TormentAreaTypeFlags.Baator.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.ModronCube) == TormentAreaTypeFlags.ModronCube, TormentAreaTypeFlags.ModronCube.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.FortressOfRegrets) == TormentAreaTypeFlags.FortressOfRegrets, TormentAreaTypeFlags.FortressOfRegrets.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.Curst) == TormentAreaTypeFlags.Curst, TormentAreaTypeFlags.Curst.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.Carceri) == TormentAreaTypeFlags.Carceri, TormentAreaTypeFlags.Carceri.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AreaType & TormentAreaTypeFlags.Outdoors) == TormentAreaTypeFlags.Outdoors, TormentAreaTypeFlags.Outdoors.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}