using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an initialize Video Stream opcode</summary>
    public class InitializeVideoStream : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 6;
        #endregion


        #region Fields
        /// <summary>Represents the width of the video frames</summary>
        public UInt16 Width { get; set; }

        /// <summary>Represents the height of the video frames</summary>
        public UInt16 Height { get; set; }

        /// <summary>Represents the flags of the audio buffers</summary>
        public UInt16 Flags { get; set; }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, InitializeVideoStream.OpcodeSize);

            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
            this.Flags = ReusableIO.ReadUInt16FromArray(data, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
            ReusableIO.WriteUInt16ToStream(this.Flags, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This gets a human-readable representation of the flags.</summary>
        /// <returns>A string, formatted largely for console, that describes the flags' values.</returns>
        protected virtual String GetFlagStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append(this.Flags);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is InitializeVideoStream)
                {
                    InitializeVideoStream compare = obj as InitializeVideoStream;

                    Boolean structureEquality;
                    structureEquality = base.Equals(compare);
                    structureEquality &= (this.Width == compare.Width);
                    structureEquality &= (this.Height == compare.Height);
                    structureEquality &= (this.Flags == compare.Flags);

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.Width.GetHashCode();
            hash ^= this.Height.GetHashCode();
            hash ^= this.Flags.GetHashCode();

            return hash;
        }
        #endregion
    }
}