using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component
{
    /// <summary>Represents a single entry in the BAM file for a frame, like a disjoint header</summary>
    public class FrameEntry_v2 : IInfinityFormat
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

        /// <summary>First index of the frame's data block</summary>
        public Int16 BlockIndexOffset { get; set; }

        /// <summary>Count of data blocks used by this frame</summary>
        public Int16 BlockCount { get; set; }
        #endregion


        #region Properties
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

            Byte[] data = ReusableIO.BinaryRead(input, FrameEntry_v2.StructSize);

            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
            this.CenterX = ReusableIO.ReadInt16FromArray(data, 4);
            this.CenterY = ReusableIO.ReadInt16FromArray(data, 6);
            this.BlockIndexOffset = ReusableIO.ReadInt16FromArray(data, 8);
            this.BlockCount = ReusableIO.ReadInt16FromArray(data, 10);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
            ReusableIO.WriteInt16ToStream(this.CenterX, output);
            ReusableIO.WriteInt16ToStream(this.CenterY, output);
            ReusableIO.WriteInt16ToStream(this.BlockIndexOffset, output);
            ReusableIO.WriteInt16ToStream(this.BlockCount, output);
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
            return "BAM v2 Frame Entry:";
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
            builder.Append(StringFormat.ToStringAlignment("Data Block first Index"));
            builder.Append(this.BlockIndexOffset);
            builder.Append(StringFormat.ToStringAlignment("Data Block count"));
            builder.Append(this.BlockCount);

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
                if (obj != null && obj is FrameEntry_v2)
                {
                    FrameEntry_v2 compare = obj as FrameEntry_v2;

                    Boolean structureEquality = (this.Width == compare.Width);
                    structureEquality &= (this.Height == compare.Height);
                    structureEquality &= (this.CenterX == compare.CenterX);
                    structureEquality &= (this.CenterY == compare.CenterY);
                    structureEquality &= (this.BlockIndexOffset == compare.BlockIndexOffset);
                    structureEquality &= (this.BlockCount == compare.BlockCount);

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
            hash ^= this.BlockIndexOffset.GetHashCode();
            hash ^= this.BlockCount.GetHashCode();

            return hash;
        }
        #endregion
    }
}