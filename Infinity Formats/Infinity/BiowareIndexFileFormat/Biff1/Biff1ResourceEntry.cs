using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BiowareIndexFileFormat.Biff1
{
    /// <summary>This class describes a resource1 entry within the BIFF archive</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    /// 
    ///     Offset 	Size    (data type)     Description
    ///     0x0000 	4       (dword)     	Resource locator
    ///                                     NB: On disk, only bits 0-13 are matched. They are matched against the file
    ///                                         index in the "resource1 locator" field from the KEY file resource1 entries
    ///                                         which claim to exist in this BIFF.
    ///     0x0004 	4       (dword)     	Offset (from start of file) to resource1 data
    ///     0x0008 	4       (dword)     	Size of this resource1
    ///     0x000c 	2       (word) 	        Type of this resource1
    ///     0x000e 	2       (word) 	        Unknown - presumed to be binary padding/wrapping to 4-byte size
    /// </remarks>
    public class Biff1ResourceEntry : Biff1FileEntry
    {
        /// <summary>This member defines the length of the resource1 data entry.</summary>
        protected UInt32 sizeResource;

        /// <summary>This property exposes the length of the resource1 data entry.</summary>
        new public UInt32 SizeResource
        {
            get { return this.sizeResource; }
            set { this.sizeResource = value; }
        }

        /// <summary>This public method reads the 16-byte resource1 entry into the header record</summary>
        /// <param name="input">Stream object from which to read from</param>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 16);   //data buffer
            Byte[] temp = new Byte[4];

            //Resource locator
            this.resourceLocator.Locator = ReusableIO.ReadInt32FromArray(buffer, 0x0);

            //Resource offset
            this.offsetResource = ReusableIO.ReadUInt32FromArray(buffer, 0x4);

            //Resource size
            this.sizeResource = ReusableIO.ReadUInt32FromArray(buffer, 0x8);

            //Resource type
            this.typeResource = (ResourceType)ReusableIO.ReadInt16FromArray(buffer, 0xC);

            //unknown / padding
            this.unknownPadding = ReusableIO.ReadUInt16FromArray(buffer, 0xE);
        }

        /// <summary>This public method writes the resource1 entry to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            Byte[] writeBytes;

            //Resource locator
            writeBytes = BitConverter.GetBytes(this.resourceLocator.Locator);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Resource offset
            writeBytes = BitConverter.GetBytes(this.offsetResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Resource Size
            writeBytes = BitConverter.GetBytes(this.sizeResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Resource Type
            writeBytes = BitConverter.GetBytes((Int16)this.typeResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //binary padding
            writeBytes = BitConverter.GetBytes(this.unknownPadding);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Biff1ResourceEntry", 0));
            builder.Append(StringFormat.ToStringAlignment("Resource Locator"));
            builder.Append(this.resourceLocator.Locator);
            builder.Append(StringFormat.ToStringAlignment("BIFF Index"));
            builder.Append(this.resourceLocator.BifIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Tileset Index"));
            builder.Append(this.resourceLocator.TilesetIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Resource Index"));
            builder.Append(this.resourceLocator.ResourceIndex);
            builder.Append(StringFormat.ToStringAlignment("Offset"));
            builder.Append(this.offsetResource);
            builder.Append(StringFormat.ToStringAlignment("Size"));
            builder.Append(this.sizeResource);
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append(this.typeResource.ToString("G"));
            builder.Append(StringFormat.ToStringAlignment("Type (hex)"));
            builder.AppendLine(String.Format("0x{0:X4}",(Int16)this.typeResource));

            return builder.ToString();
        }
    }
}