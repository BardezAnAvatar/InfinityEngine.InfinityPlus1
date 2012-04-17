using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents a saved variable</summary>
    public class Variable : IInfinityFormat
    {
        #region Constants
        /// <summary>The size on one variable on disk.</summary>
        public const Int32 StructSize = 84;
        #endregion


        #region Fields
        /// <summary>Represents the variable name</summary>
        /// <remarks>32 bytes in length</remarks>
        public ZString Name { get; set; }

        /// <summary>Unknown 8 bytes. Probably intended for a RESREF</summary>
        public UInt64 Unknown { get; set; }

        /// <summary>Represents the value of the variable</summary>
        public UInt32 Value { get; set; }

        /// <summary>40 bytes of padding after the value. Probably intended for metadata.</summary>
        public Byte[] Padding { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 44);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Unknown = ReusableIO.ReadUInt64FromArray(buffer, 32);
            this.Value = ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.Padding = ReusableIO.BinaryRead(input, 40);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt64ToStream(this.Unknown, output);
            ReusableIO.WriteUInt32ToStream(this.Value, output);
            output.Write(this.Padding, 0, 40);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.Unknown);
            builder.Append(StringFormat.ToStringAlignment("Value"));
            builder.Append(this.Value);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding));

            return builder.ToString();
        }
        #endregion
    }
}