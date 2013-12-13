using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable
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
    public class ChitinKeyBifEntry : IInfinityFormat, IDeepCloneable
    {
        #region Protected Members
        /// <summary>This four-byte value indicates the (expected) length of the BIF file</summary>
        protected UInt32 lengthBifFile;

        /// <summary>This four-byte value indicates the offset to the filename of the BIF file</summary>
        protected UInt32 offsetBifFileName;

        /// <summary>This two-byte value indicates the length of the filename of the BIF file</summary>
        protected UInt16 lengthBifFileName;

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
        public ZString BifFileName { get; set; }

        /// <summary>This two-byte value indicates the location(s) of the BIF file within the engine.</summary>
        public ChitinKeyBifLocationEnum BifLocationFlags
        {
            get { return this.bifLocationFlags; }
            set { this.bifLocationFlags = value; }
        }

        public Boolean LocationInstallDirectory
        {
            get { return (ChitinKeyBifLocationEnum.HardDrive & this.bifLocationFlags) == ChitinKeyBifLocationEnum.HardDrive; }
            set { this.bifLocationFlags |= ChitinKeyBifLocationEnum.HardDrive; }
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


        #region Construction
        /// <summary>Default constructor</summary>
        public ChitinKeyBifEntry() { }

        /// <summary>Definition constructor</summary>
        /// <param name="length">The (expected) length of the BIF file</param>
        /// <param name="offsetName">Offset to the filename of the BIF file</param>
        /// <param name="name">Name of this BIFF archive</param>
        /// <param name="locations">Flags indicating the location(s) of the BIF file within the engine</param>
        public ChitinKeyBifEntry(UInt32 length, UInt32 offsetName, String name, ChitinKeyBifLocationEnum locations)
        {
            this.lengthBifFile = length;
            this.offsetBifFileName = offsetName;
            this.BifFileName = ZString.FromString(name);
            this.lengthBifFileName = Convert.ToUInt16(this.BifFileName.Source.Length);
            this.bifLocationFlags = locations;
        }

        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.BifFileName = new ZString();
        }
        #endregion


        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

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
        public virtual void ReadBifName(Stream input, String cultureReference)
        {
            //seek to offset
            ReusableIO.SeekIfAble(input, this.offsetBifFileName, SeekOrigin.Begin);

            Byte[] buffer = ReusableIO.BinaryRead(input, this.lengthBifFileName);
            this.BifFileName.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, cultureReference, this.lengthBifFileName);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public virtual void Write(Stream output)
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
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("ChitinKeyBifEntry:", 0));
            builder.Append(StringFormat.ToStringAlignment("BIF length"));
            builder.Append(this.lengthBifFile);
            builder.Append(StringFormat.ToStringAlignment("BIF Name Offset"));
            builder.Append(this.offsetBifFileName);
            builder.Append(StringFormat.ToStringAlignment("BIF Name Length"));
            builder.Append(this.lengthBifFileName);
            builder.Append(StringFormat.ToStringAlignment("BIF Name"));
            builder.Append(String.Format("'{0}'", this.BifFileName.Value));
            builder.Append(StringFormat.ToStringAlignment("BIF Location Flags"));
            builder.Append(this.bifLocationFlags.ToString("G"));
            builder.Append(StringFormat.ToStringAlignment("BIF Location Flags Value"));
            builder.Append((Int16)this.bifLocationFlags);
            builder.Append(StringFormat.ToStringAlignment("Data Directory", 2));
            builder.Append(this.LocationInstallDirectory);
            builder.Append(StringFormat.ToStringAlignment("Cache Directory", 2));
            builder.Append(this.LocationCache);
            builder.Append(StringFormat.ToStringAlignment("Disc 1", 2));
            builder.Append(this.LocationDisc1);
            builder.Append(StringFormat.ToStringAlignment("Disc 2", 2));
            builder.Append(this.LocationDisc2);
            builder.Append(StringFormat.ToStringAlignment("Disc 3", 2));
            builder.Append(this.LocationDisc3);
            builder.Append(StringFormat.ToStringAlignment("Disc 4", 2));
            builder.Append(this.LocationDisc4);
            builder.Append(StringFormat.ToStringAlignment("Disc 5", 2));
            builder.Append(this.LocationDisc5);
            builder.Append(StringFormat.ToStringAlignment("Disc 6", 2));
            builder.Append(this.LocationDisc6);

            return builder.ToString();
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <param name="detail">Boolean indicating whether or not to print full detail</param>
        /// <returns>A String containing the member data line by line</returns>
        public String ToString(Boolean detail)
        {
            String data;

            if (detail)
                data = this.ToString();
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(StringFormat.ReturnAndIndent("ChitinKeyBifEntry:", 0));
                builder.Append(StringFormat.ToStringAlignment("BIFF length"));
                builder.Append(this.lengthBifFile);
                builder.Append(StringFormat.ToStringAlignment("BIFF Name Offset"));
                builder.Append(this.offsetBifFileName);
                builder.Append(StringFormat.ToStringAlignment("BIFF Name Length"));
                builder.Append(this.lengthBifFileName);
                builder.Append(StringFormat.ToStringAlignment("BIFF Name"));
                builder.Append(String.Format("'{0}'", this.BifFileName.Value));
                builder.Append(StringFormat.ToStringAlignment("BIFF Location Flags"));
                builder.Append(this.bifLocationFlags.ToString("G"));
                builder.Append(StringFormat.ToStringAlignment("BIFF Location Flags Value"));
                builder.Append((Int16)this.bifLocationFlags);

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