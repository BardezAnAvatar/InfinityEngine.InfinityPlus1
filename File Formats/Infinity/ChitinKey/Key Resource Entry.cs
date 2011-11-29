﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
    /// <summary>This class is a resource1 netry within a chitin.key file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset      Size (data type)  	Description
    ///     0x0000 	    8 (resref) 	        Resource name
    ///     0x0008 	    2 (word) 	        Resource type
    ///     0x000a 	    4 (dword) 	        Resource locator. The IE resource1 manager uses 32-bit values
    ///                                         as a 'resource1 index', which codifies the source of the
    ///                                         resource1 as well as which source it refers to. The layout
    ///                                         of this value is below.
    ///                                     * bits 31-20: source index (the ordinal value giving the index of the
    ///                                         corresponding BIF entry)
    ///                                     * bits 19-14: tileset index
    ///                                     * bits 13- 0: non-tileset file index (any 12 bit value, so long as it matches
    ///                                         the value used in the BIF file)
    /// </remarks>
    public class ChitinKeyResourceEntry : IInfinityFormat, IDeepCloneable
    {
        #region Protected members
        /// <summary>This member contains the Int32 value dictating the type of the resource1.</summary>
        protected ResourceType resourceType;

        /// <summary>
        ///     This member contains is a bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        protected ResourceLocator1 resourceLocator; 
        #endregion

        #region Properties
        /// <summary>This property represents the name of the resource1.</summary>
        public ZString ResourceName { get; set; }

        /// <summary>This property represents the Int16 value dictating the type of the resource1.</summary>
        public ResourceType ResourceType
        {
            get { return this.resourceType; }
            set { this.resourceType = value; }
        }

        /// <summary>
        ///     This property represents the bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        public ResourceLocator1 ResourceLocator
        {
            get { return this.resourceLocator; }
            set { this.resourceLocator = value; }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.ResourceName = new ZString();
        }
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
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
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 14);   //header buffer

            //Resource Name
            this.ResourceName.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, Constants.CultureCodeEnglish);

            //Resource Type
            this.resourceType = (ResourceType)ReusableIO.ReadInt16FromArray(buffer, 0x8);

            //Resource Locator
            this.resourceLocator.Locator = ReusableIO.ReadInt32FromArray(buffer, 0xA);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            //Resource Name
            ReusableIO.WriteStringToStream(this.ResourceName.Source, output, Constants.CultureCodeEnglish);

            //Resource Type
            ReusableIO.WriteInt16ToStream((Int16)this.resourceType, output);

            //Resource Locator
            ReusableIO.WriteInt32ToStream(this.resourceLocator.Locator, output);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("ChitinKeyResourceEntry:", 0));
            builder.Append(StringFormat.ToStringAlignment("Resource Name"));
            builder.Append(String.Format("'{0}'", this.ResourceName.Value));
            builder.Append(StringFormat.ToStringAlignment("Resource Type"));
            builder.Append(this.resourceType.ToString("G"));
            builder.Append(StringFormat.ToStringAlignment("Resource Type Value"));
            builder.Append(((Int16)this.resourceType).ToString("X"));
            builder.Append(StringFormat.ToStringAlignment("Resource Locator"));
            builder.Append(this.resourceLocator.Locator);
            builder.Append(StringFormat.ToStringAlignment("BIFF Index", 2));
            builder.Append(this.resourceLocator.BifIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Tileset Index", 2));
            builder.Append(this.resourceLocator.TilesetIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Resource Index", 2));
            builder.Append(this.resourceLocator.ResourceIndex);

            return builder.ToString();
        }

        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        /// <returns>A deeply copied separate instance clone of the insance called from.</returns>
        public IDeepCloneable Clone()
        {
            return this.MemberwiseClone() as ChitinKeyResourceEntry;
        }
        #endregion
    }
}