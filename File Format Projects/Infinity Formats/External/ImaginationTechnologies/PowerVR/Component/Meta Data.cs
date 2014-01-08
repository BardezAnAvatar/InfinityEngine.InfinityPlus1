using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Component
{
    /// <summary>Meta data for this image</summary>
    public class MetaData
    {
        #region Fields
        /// <summary>Four bytes containing a fource character code indicating type of meta data</summary>
        public Byte[] FourCC { get; set; }

        /// <summary>Key indicting how meta data should be handled</summary>
        public UInt32 Key { get; set; }

        /// <summary>Count, in bytes, of meta data</summary>
        public UInt32 Size { get; set; }

        /// <summary>Actual meta data</summary>
        public Byte[] Data { get; set; }
        #endregion


        #region IO
        /// <summary>Read method, reads the header from the input stream</summary>
        /// <param name="input">Stream to read the header from</param>
        public void Read(Stream input)
        {
            this.FourCC = ReusableIO.BinaryRead(input, 4);

            Byte[] header = ReusableIO.BinaryRead(input, 8);

            this.Key = ReusableIO.ReadUInt32FromArray(header, 0);
            this.Size = ReusableIO.ReadUInt32FromArray(header, 4);

            this.Data = ReusableIO.BinaryRead(input, this.Size);
        }

        /// <summary>Write method, writes the header to the output stream</summary>
        /// <param name="output">Stream to write the header to</param>
        public void Write(Stream output)
        {
            output.Write(this.FourCC, 0, 4);
            ReusableIO.WriteUInt32ToStream(this.Key, output);
            ReusableIO.WriteUInt32ToStream(this.Size, output);
            output.Write(this.Data, 0, Convert.ToInt32(this.Size));
        }
        #endregion
    }
}