﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
    /// <summary>This class is an entry in the BIF entries table in a chitin.key file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset      Size (data type)  	Description
    ///     0x0000 	    4 (char array) 	    Length of BIF file
    ///     0x0004 	    4 (char array) 	    Offset from start of file to ASCIIZ BIF filename
    ///     0x0008 	    2 (word) 	        Length, including terminating NUL, of ASCIIZ BIF filename
    ///     0x000a 	    2 (word) 	        The 16 bits of this field are used individually to mark the
    ///                                         location of the relevant file.
    ///                                     (MSB) xxxx xxxx ABCD EFGH (LSB)
    ///                                     * Bits marked A to F determine on which CD the file is stored
    ///                                         (A = CD6, F = CD1)
    ///                                     * Bit G determines if the file is in the \cache directory
    ///                                     * Bit H determines if the file is in the \data directory
    /// </remarks>
    public class ChitinKeyBifEntry : IDeepCloneable
    {
        #region Protected Members
        /// <summary>This four-byte value indicates the (expected) length of the BIF file</summary>
        protected UInt32 lengthBifFile;

        /// <summary>This four-byte value indicates the offset to the filename of the BIF file</summary>
        protected UInt32 offsetBifFileName;

        /// <summary>This two-byte value indicates the length of the filename of the BIF file</summary>
        protected UInt16 lengthBifFileName;

        /// <summary>This String is the filename of the BIF file. It is read from elsewhere in the file, and not contained in the structure when written.</summary>
        protected String bifFileName;

        /// <summary>This two-byte value indicates the location(s) of the BIF file within the engine.</summary>
        protected ChitinKeyBifLocationEnum bifLocationFlags; 
        #endregion

        #region Properties
        /// <summary>This four-byte value indicates the (expected) length of the BIF file</summary>
        public UInt32 LengthBifFile
        {
            get { return this.lengthBifFile; }
            set { this.lengthBifFile = value; }
        }

        /// <summary>This four-byte value indicates the offset to the filename of the BIF file</summary>
        public UInt32 OffsetBifFileName
        {
            get { return this.offsetBifFileName; }
            set { this.offsetBifFileName = value; }
        }

        /// <summary>This two-byte value indicates the length of the filename of the BIF file</summary>
        public UInt16 LengthBifFileName
        {
            get { return this.lengthBifFileName; }
            set { this.lengthBifFileName = value; }
        }

        /// <summary>This String is the filename of the BIF file. It is read from elsewhere in the file, and not contained in the structure when written.</summary>
        public String BifFileName
        {
            get { return this.bifFileName; }
            set { this.bifFileName = value; }
        }

        /// <summary>This two-byte value indicates the location(s) of the BIF file within the engine.</summary>
        public ChitinKeyBifLocationEnum BifLocationFlags
        {
            get { return this.bifLocationFlags; }
            set { this.bifLocationFlags = value; }
        }

        public Boolean LocationData
        {
            get { return (ChitinKeyBifLocationEnum.Data & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Data; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Data; }
        }

        public Boolean LocationCache
        {
            get { return (ChitinKeyBifLocationEnum.Cache & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Cache; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Cache; }
        }

        public Boolean LocationDisc1
        {
            get { return (ChitinKeyBifLocationEnum.Disc1 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc1; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc1; }
        }

        public Boolean LocationDisc2
        {
            get { return (ChitinKeyBifLocationEnum.Disc2 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc2; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc2; }
        }

        public Boolean LocationDisc3
        {
            get { return (ChitinKeyBifLocationEnum.Disc3 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc3; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc3; }
        }

        public Boolean LocationDisc4
        {
            get { return (ChitinKeyBifLocationEnum.Disc4 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc4; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc4; }
        }

        public Boolean LocationDisc5
        {
            get { return (ChitinKeyBifLocationEnum.Disc5 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc5; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc5; }
        }

        public Boolean LocationDisc6
        {
            get { return (ChitinKeyBifLocationEnum.Disc6 & this.bifLocationFlags) == ChitinKeyBifLocationEnum.Disc6; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.Disc6; }
        }
        #endregion
        
        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 12);   //header buffer

            //BIF length
            this.lengthBifFile = ReusableIO.ReadUInt32FromArray(buffer, 0x0);

            //BIF name offset
            this.offsetBifFileName = ReusableIO.ReadUInt32FromArray(buffer, 0x4);

            //BIF name length
            this.lengthBifFileName = ReusableIO.ReadUInt16FromArray(buffer, 0x8);

            //BIF location flags
            this.bifLocationFlags = (ChitinKeyBifLocationEnum)ReusableIO.ReadUInt16FromArray(buffer, 0xA);
        }

        /// <summary>This private method reads the referenced string from the file</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="cultureReference">String representing the source culture</param>
        public void ReadBifName(Stream input, String cultureReference)
        {
            //seek to offset
            ReusableIO.SeekIfAble(input, this.offsetBifFileName, SeekOrigin.Begin);

            Byte[] buffer = ReusableIO.BinaryRead(input, this.lengthBifFileName);
            this.bifFileName = ReusableIO.ReadStringFromByteArray(buffer, 0, cultureReference, this.lengthBifFileName);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            //BIF length
            ReusableIO.WriteUInt32ToStream(this.lengthBifFile, output);

            //BIF name offset
            ReusableIO.WriteUInt32ToStream(this.offsetBifFileName, output);

            //BIF name length
            ReusableIO.WriteUInt16ToStream(this.lengthBifFileName, output);

            //BIF location flags
            ReusableIO.WriteUInt16ToStream((UInt16)this.bifLocationFlags, output);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ChitinKeyBifEntry:");
            builder.Append("\n\tBIF length:                ");
            builder.Append(this.lengthBifFile);
            builder.Append("\n\tBIF Name Offset:           ");
            builder.Append(this.offsetBifFileName);
            builder.Append("\n\tBIF Name Length:           ");
            builder.Append(this.lengthBifFileName);
            builder.Append("\n\tBIF Name:                  '");
            builder.Append(this.bifFileName);
            builder.Append("'\n\tBIF Location Flags:        ");
            builder.Append(this.bifLocationFlags.ToString("G"));
            builder.Append("\n\tBIF Location Flags Value:  ");
            builder.Append((Int16)this.bifLocationFlags);
            builder.Append("\n\t\tData Directory:    ");
            builder.Append(this.LocationData);
            builder.Append("\n\t\tCache Directory:   ");
            builder.Append(this.LocationCache);
            builder.Append("\n\t\tDisc 1:            ");
            builder.Append(this.LocationDisc1);
            builder.Append("\n\t\tDisc 2:            ");
            builder.Append(this.LocationDisc2);
            builder.Append("\n\t\tDisc 3:            ");
            builder.Append(this.LocationDisc3);
            builder.Append("\n\t\tDisc 4:            ");
            builder.Append(this.LocationDisc4);
            builder.Append("\n\t\tDisc 5:            ");
            builder.Append(this.LocationDisc5);
            builder.Append("\n\t\tDisc 6:            ");
            builder.Append(this.LocationDisc6);
            builder.Append("\n\n");

            return builder.ToString();
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <param name="detail">Boolean indicating whether or not to print full detail</param>
        /// <returns>A String containing the member data line by line</returns>
        public string ToString(Boolean detail)
        {
            String data;

            if (detail)
                data = this.ToString();
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ChitinKeyBifEntry:");
                builder.Append("\n\tBIF length:                ");
                builder.Append(this.lengthBifFile);
                builder.Append("\n\tBIF Name Offset:           ");
                builder.Append(this.offsetBifFileName);
                builder.Append("\n\tBIF Name Length:           ");
                builder.Append(this.lengthBifFileName);
                builder.Append("\n\tBIF Name:                  '");
                builder.Append(this.bifFileName);
                builder.Append("'\n\tBIF Location Flags:        ");
                builder.Append(this.bifLocationFlags.ToString("G"));
                builder.Append("\n\tBIF Location Flags Value:  ");
                builder.Append((Int16)this.bifLocationFlags);
                builder.Append("\n");

                data = builder.ToString();
            }

            return data;
        }

        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        /// <returns>A deeply copied separate instance clone of the insance called from.</returns>
        public IDeepCloneable Clone()
        {
            return this.MemberwiseClone() as ChitinKeyBifEntry;
        }
        #endregion
    }
}