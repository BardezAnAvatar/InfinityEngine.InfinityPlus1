using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component
{
    /// <summary>Represents a single entry in the BAM file for a frame, like a disjoint header</summary>
    public class FrameEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 12;
        #endregion


        #region Fields
        /// <summary>Width of the frame</summary>
        public UInt16 Width { get; set; }

        /// <summary>Height of the frame</summary>
        public UInt16 Height { get; set; }

        /// <summary>Represents the rendering center X co-ordinate</summary>
        public Int16 CenterX { get; set; }

        /// <summary>Represents the rendering center Y co-ordinate</summary>
        public Int16 CenterY { get; set; }

        /// <summary>Bitfield of both the offset to the frame data as well as RLE flag</summary>
        /// <remarks>
        ///     The easiest way, I think, to deal with this is to use Int32, and use the sign as the RLE flag.
        ///     It's probably how Bioware/Black Isle did it anyway
        /// </remarks>
        public Int32 DataField { get; set; }

        /// <summary>Represents the frame data of this entry</summary>
        /// <remarks>Not stored on disk as part of the structure, but a pointer is.</remarks>
        public FrameData Data { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the offset to the frame binary data</summary>
        public UInt32 OffsetData
        {
            get { return Convert.ToUInt32(this.DataField & 0x7FFFFFFF); }
            set
            {
                if (value > (UInt32.MaxValue >> 1))
                    throw new ArgumentOutOfRangeException(String.Format("passed in value ({0}) is too large for storage in this 31-bit-field.", value));

                this.DataField = Convert.ToInt32(this.RunLengthEncoded ? Convert.ToInt64(value) : Convert.ToInt64(0 - value));
            }
        }

        /// <summary>Exposes the bit indicating whether the frame data is RLE encoded or not</summary>
        public Boolean RunLengthEncoded
        {
            get { return this.DataField > -1; }
            set { this.DataField = Convert.ToInt32(value ? this.OffsetData : 0 - this.OffsetData); }
        }

        /// <summary>Exposes the rendering point of origin along the X axis</summary>
        public Int32 RenderOriginX
        {
            get { return 0 - this.CenterX; }
        }

        /// <summary>Exposes the rendering point of origin along the Y axis</summary>
        public Int32 RenderOriginY
        {
            get { return 0 - this.CenterY; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize() { }
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

            Byte[] data = ReusableIO.BinaryRead(input, FrameEntry.StructSize);

            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
            this.CenterX = ReusableIO.ReadInt16FromArray(data, 4);
            this.CenterY = ReusableIO.ReadInt16FromArray(data, 6);
            this.DataField = ReusableIO.ReadInt32FromArray(data, 8);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
            ReusableIO.WriteInt16ToStream(this.CenterX, output);
            ReusableIO.WriteInt16ToStream(this.CenterY, output);
            ReusableIO.WriteInt32ToStream(this.DataField, output);
        }
        #endregion


        #region ToString() methods
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
        /// <param name="index">Index of the overlay</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 index)
        {
            String header = this.GetVersionString(index);
            header += this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "BAM Frame Entry:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Frame Entry # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Center X Coordinate"));
            builder.Append(this.CenterX);
            builder.Append(StringFormat.ToStringAlignment("Center Y Coordinate"));
            builder.Append(this.CenterY);
            builder.Append(StringFormat.ToStringAlignment("Data Offset and RLE bitfield"));
            builder.Append(this.DataField);
            builder.Append(StringFormat.ToStringAlignment("Data Offset"));
            builder.Append(this.OffsetData);
            builder.Append(StringFormat.ToStringAlignment("Run Length Encoded"));
            builder.Append(this.RunLengthEncoded);

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
                if (obj != null && obj is FrameEntry)
                {
                    FrameEntry compare = obj as FrameEntry;

                    Boolean structureEquality = (this.Width == compare.Width);
                    structureEquality &= (this.Height == compare.Height);
                    structureEquality &= (this.CenterX == compare.CenterX);
                    structureEquality &= (this.CenterY == compare.CenterY);
                    structureEquality &= (this.RunLengthEncoded == compare.RunLengthEncoded);
                    //offset does not qualify for data value equality

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
            Int32 hash = this.Width.GetHashCode();
            hash ^= this.Height.GetHashCode();
            hash ^= this.CenterX.GetHashCode();
            hash ^= this.CenterY.GetHashCode();
            hash ^= this.RunLengthEncoded.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}