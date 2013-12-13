using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component
{
    /// <summary>Represents the frame data of a BAM frame</summary>
    public class FrameData : IInfinityFormat
    {
        #region Fields
        /// <summary>Indicates whether the data is Run length encoded</summary>
        public Boolean RunLengthEncoded { get; set; }

        /// <summary>Width of the frame</summary>
        public UInt16 Width { get; set; }

        /// <summary>Height of the frame</summary>
        public UInt16 Height { get; set; }

        /// <summary>Exposes the </summary>
        public Byte[] Data { get; set; }

        /// <summary>The palette index which is transparent</summary>
        public Int32 TransparentIndex { get; set; }

        /// <summary>Index to the palette that the Header indcates as the RLE index</summary>
        /// <remarks>Usually the transparency index, but I have seen instances where this is not the case.</remarks>
        public Int32 RleIndex { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the count of pixels in the pixel data</summary>
        protected Int32 PixelCount
        {
            get { return this.Width * this.Height; }
        }
        #endregion


        #region Construction
        /// <summary>Parameter constructor</summary>
        /// <param name="frame">Frame entry data to indicate how to read the frame data</param>
        public FrameData(FrameEntry frame, Int32 transparentPaletteIndex, Int32 rleIndex)
        {
            this.RunLengthEncoded = frame.RunLengthEncoded;
            this.Width = frame.Width;
            this.Height = frame.Height;
            this.TransparentIndex = transparentPaletteIndex;
            this.RleIndex = rleIndex;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Data = new Byte[this.PixelCount];
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

            if (this.RunLengthEncoded)
                this.ReadRLE(input);
            else
                this.ReadRaw(input);
        }

        /// <summary>Reads the data from the input Stream, with sepcial case for run length encoding</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadRLE(Stream input)
        {
            for (Int32 index = 0; index < this.PixelCount; ++index)
            {
                Byte tmp = ReusableIO.BinaryReadByte(input);
                this.Data[index] = tmp;

                if (tmp == this.RleIndex)
                {
                    Int32 run = ReusableIO.BinaryReadByte(input);

                    for (Int32 j = 0; j < run; ++j)
                    {
                        ++index;
                        this.Data[index] = tmp;
                    }
                }
            }
        }

        /// <summary>Reads the data from the input Stream, with no run length encoding</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadRaw(Stream input)
        {
            for (Int32 index = 0; index < this.PixelCount; ++index)
                this.Data[index] = ReusableIO.BinaryReadByte(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            if (this.RunLengthEncoded)
                this.WriteRLE(output);
            else
                this.WriteRaw(output);
        }

        /// <summary>Writes the data to the output Stream, with sepcial case for run length encoding</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteRLE(Stream output)
        {
            for (Int32 index = 0; index < this.PixelCount; ++index)
            {
                Byte tmp = this.Data[index];
                output.WriteByte(tmp);

                if (tmp == this.RleIndex)
                {
                    Byte count = Byte.MinValue;

                    for (; (index + 1) < this.PixelCount; ++index)
                    {
                        if (this.Data[index + 1] == this.TransparentIndex)
                            ++count;
                        else
                            break;
                    }

                    output.WriteByte(count);    //write the run length
                }
            }
        }

        /// <summary>Writes the data to the output Stream, with no run length encoding</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteRaw(Stream output)
        {
            for (Int32 index = 0; index < this.PixelCount; ++index)
                output.WriteByte(this.Data[index]);
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
            return "BAM Frame Data:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Frame # {0,5} Data:", index);
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

            return builder.ToString();
        }
        #endregion
    }
}