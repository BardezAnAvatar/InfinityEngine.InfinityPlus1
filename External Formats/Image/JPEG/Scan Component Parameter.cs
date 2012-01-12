using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a scan component specificaion</summary>
    public class ScanComponentParameter
    {
        #region Fields
        /// <summary>Represents the selector of the component. Matches an identifier in the JpegFrame Icomponents collection.</summary>
        public Byte Identifier { get; set; }

        /// <summary>Indicates which table index to use for decoding the DC coefficient</summary>
        public Byte IndexDC { get; set; }

        /// <summary>Indicates which table index to use for decoding the AC coefficients</summary>
        public Byte IndexAC { get; set; }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.Identifier = (Byte)input.ReadByte();

            //read table indecies
            Byte huffman, arethmetic;
            JpegParser.ReadParameters4(input, out huffman, out arethmetic);
            this.IndexDC = huffman;
            this.IndexAC = arethmetic;
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            output.WriteByte(this.Identifier);

            //write table indecies
            Byte temp = (Byte)(this.IndexDC << 4);
            temp &= (Byte)(this.IndexAC & 0x0F);
            output.WriteByte(temp);
        }
        #endregion
    }
}