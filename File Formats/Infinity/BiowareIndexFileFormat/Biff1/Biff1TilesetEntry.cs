﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1
{
    /// <summary>This class describes a tileset entry within the BIFF archive</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    /// 
    ///     Offset 	Size    (data type)     Description
    ///     0x0000 	4       (dword)         Resource locator
    ///                                         NB: On disk, only bits 14-19 are matched upon. They are matched against the
    ///                                         tileset index in the "resource1 locator" field from the KEY file
    ///                                         resource1 entries which claim to exist in this BIFF.
    ///     0x0004 	4       (dword)         Offset (from start of file) to resource1 data
    ///     0x0008 	4       (dword)         Count of tiles in this resource1
    ///     0x000c 	4       (dword)         Size of each tile in this resource1
    ///     0x0010 	2       (word) 	        Type of this resource1 (always 0x3eb - TIS)
    ///     0x0012 	2       (word) 	        Unknown
    ///     
    /// </remarks>
    public class Biff1TilesetEntry : Biff1FileEntry
    {
        /// <summary>This member defines the count of tiles within the resource1 data.</summary>
        protected UInt32 countTile;

        /// <summary>This member defines the length of each tile in the resource1 data.</summary>
        protected UInt32 sizeTile;

        /// <summary>This property exposes the count of tiles within the resource1 data.</summary>
        public UInt32 CountTile
        {
            get { return this.countTile; }
            set { this.countTile = value; }
        }

        /// <summary>This property exposes the length of each tile in the resource1 data.</summary>
        public override UInt32 SizeResource
        {
            get { return this.sizeTile * this.countTile; }
        }

        /// <summary>This public method reads the 16-byte resource1 entry into the header record</summary>
        /// <param name="input">Stream object from which to read from</param>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 20);   //data buffer
            Byte[] temp = new Byte[4];

            //Resource locator
            this.resourceLocator.Locator = ReusableIO.ReadInt32FromArray(buffer, 0x0);

            //Resource offset
            this.offsetResource = ReusableIO.ReadUInt32FromArray(buffer, 0x4);

            //Tile count
            this.countTile = ReusableIO.ReadUInt32FromArray(buffer, 0x8);

            //Tile size
            this.sizeTile = ReusableIO.ReadUInt32FromArray(buffer, 0xC);

            //Resource type
            this.typeResource = (ResourceType)ReusableIO.ReadInt16FromArray(buffer, 0x10);

            //unknown / padding
            this.unknownPadding = ReusableIO.ReadUInt16FromArray(buffer, 0x12);
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

            //Tile Count
            writeBytes = BitConverter.GetBytes(this.countTile);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Tile Size
            writeBytes = BitConverter.GetBytes(this.sizeTile);
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
            builder.Append("Biff1TilesetEntry:");
            builder.Append("\n\tResource Locator:  ");
            builder.Append(this.resourceLocator.Locator);
            builder.Append("\n\t\tBIF Index:           ");
            builder.Append(this.resourceLocator.BifIndex);
            builder.Append("\n\t\tBIF Tileset Index:   ");
            builder.Append(this.resourceLocator.TilesetIndex);
            builder.Append("\n\t\tBIF Resource Index:  ");
            builder.Append(this.resourceLocator.ResourceIndex);
            builder.Append("\n\tOffset:            ");
            builder.Append(this.offsetResource);
            builder.Append("\n\tTile Count:        ");
            builder.Append(this.countTile);
            builder.Append("\n\tTile Size:         ");
            builder.Append(this.sizeTile);
            builder.Append("\n\tSize:              ");
            builder.Append(this.SizeResource);
            builder.Append("\n\tType:              ");
            builder.Append(this.typeResource.ToString("G"));
            builder.Append("\n\tType (hex):        ");
            builder.Append(String.Format("0x{0:X4}",(Int16)this.typeResource));
            builder.Append("\n\n");

            return builder.ToString();
        }
    }
}