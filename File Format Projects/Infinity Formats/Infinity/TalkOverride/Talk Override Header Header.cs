using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Represents the header of the TOH file</summary>
    public class TalkOverrideHeaderHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x14;
        #endregion


        #region Fields
        /// <summary>Represents the first unknown at offset 0x0008</summary>
        public Int32 Unknown_0x0008 { get; set; }

        /// <summary>Count of StrRefs in this header</summary>
        public Int32 StrrefCount { get; set; }

        /// <summary>Represents the first unknown at offset 0x0010</summary>
        public Int32 Unknown_0x0010 { get; set; }
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

            Byte[] buffer = ReusableIO.BinaryRead(input, TalkOverrideHeaderHeader.StructSize - 8);

            this.Unknown_0x0008 = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.StrrefCount = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.Unknown_0x0010 = ReusableIO.ReadInt32FromArray(buffer, 8);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0008, output);
            ReusableIO.WriteInt32ToStream(this.StrrefCount, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0010, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("TOH Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Signature)));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Version)));
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x0008"));
            builder.Append(this.Unknown_0x0008);
            builder.Append(StringFormat.ToStringAlignment("StrRef Count"));
            builder.Append(this.StrrefCount);
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x0010"));
            builder.AppendLine(this.Unknown_0x0010.ToString());

            return builder.ToString();
        }
        #endregion
    }
}