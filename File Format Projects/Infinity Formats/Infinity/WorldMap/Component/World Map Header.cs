using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>Represents the header of a World Map file</summary>
    public class WorldMapHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 16;
        #endregion


        #region Fields
        /// <summary>Count of map entries within the world map</summary>
        public UInt32 MapCount { get; set; }

        /// <summary>Offset within the file to the world map map entries</summary>
        public UInt32 MapOffset { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] buffer = ReusableIO.BinaryRead(input, WorldMapHeader.StructSize - 8);    //8 less due to version, signature

            this.MapCount = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.MapOffset = ReusableIO.ReadUInt32FromArray(buffer, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.MapCount, output);
            ReusableIO.WriteUInt32ToStream(this.MapOffset, output);
        }
        #endregion


        #region Hash code and comparison
        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.MapCount.GetHashCode();
            hash ^= this.MapOffset.GetHashCode();

            return hash;
        }
        #endregion


        #region ToString override(s)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("WMAP Version 1.0 Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Maps count"));
            builder.Append(this.MapCount);
            builder.Append(StringFormat.ToStringAlignment("Maps offset"));
            builder.Append(this.MapOffset);

            return builder.ToString();
        }
        #endregion
    }
}