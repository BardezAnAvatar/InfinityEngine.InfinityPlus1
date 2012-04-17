using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a coordinate stored by StorePartyLocations() script command or a Pocket Plane effect</summary>
    public class StoredCoordinate : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 12;
        #endregion


        #region Fields
        /// <summary>Reference to the area in which locations were stored</summary>
        public ResourceReference Area { get; set; }

        /// <summary>X coordinate stored</summary>
        public UInt16 CoordinateX { get; set; }

        /// <summary>Y coordinate stored</summary>
        public UInt16 CoordinateY { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Area = new ResourceReference(ResourceType.Area);
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, StoredCoordinate.StructSize);

            this.Area.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.CoordinateX = ReusableIO.ReadUInt16FromArray(buffer, 8);
            this.CoordinateY = ReusableIO.ReadUInt16FromArray(buffer, 10);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Area.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.CoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.CoordinateY, output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the header</summary>
        /// <returns>A human-readable String representation of the header</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());
            builder.Append(StringFormat.ToStringAlignment("Area code"));
            builder.Append(String.Format("'{0}'", this.Area.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("X coordinate"));
            builder.Append(this.CoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Y coordinate"));
            builder.Append(this.CoordinateY);

            return builder.ToString();
        }
        #endregion
    }
}