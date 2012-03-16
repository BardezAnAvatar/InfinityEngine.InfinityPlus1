using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component
{
    /// <summary>Represents the very basic MVE header</summary>
    public class MveHeader : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 26;

        /// <summary>Length of the expected signature</summary>
        public const Int32 SignatureLength = 20;
        #endregion


        #region Fields
        /// <summary>Signature of the MVE file</summary>
        /// <remarks>Expected to be "Interplay MVE File[0x1A][NUL]"</remarks>
        public ZString Signature { get; set; }

        /// <summary>First trailing unknown UInt16</summary>
        public UInt16 Unknown1 { get; set; }

        /// <summary>Second trailing unknown UInt16</summary>
        public UInt16 Unknown2 { get; set; }

        /// <summary>Third trailing unknown UInt16</summary>
        public UInt16 Unknown3 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Signature = new ZString();
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

            Byte[] data = ReusableIO.BinaryRead(input, MveHeader.StructSize);

            this.Signature.Source = ReusableIO.ReadStringFromByteArray(data, 0, CultureConstants.CultureCodeEnglish, MveHeader.SignatureLength);   //"Interplay MVE File[0x1A][NUL]"
            this.Unknown1 = ReusableIO.ReadUInt16FromArray(data, 20);
            this.Unknown2 = ReusableIO.ReadUInt16FromArray(data, 22);
            this.Unknown3 = ReusableIO.ReadUInt16FromArray(data, 24);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Signature.Source, output, CultureConstants.CultureCodeEnglish, true, MveHeader.SignatureLength);   //should be 20
            ReusableIO.WriteUInt16ToStream(this.Unknown1, output);
            ReusableIO.WriteUInt16ToStream(this.Unknown2, output);
            ReusableIO.WriteUInt16ToStream(this.Unknown3, output);
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "MVE Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.Signature.Value);
            builder.Append(StringFormat.ToStringAlignment("Unknown # 1"));
            builder.Append(this.Unknown1);
            builder.Append(StringFormat.ToStringAlignment("Unknown # 2"));
            builder.Append(this.Unknown2);
            builder.Append(StringFormat.ToStringAlignment("Unknown # 3"));
            builder.Append(this.Unknown3);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is MveHeader)
                {
                    MveHeader compare = obj as MveHeader;

                    Boolean structureEquality;
                    structureEquality = (this.Signature.Value == compare.Signature.Value);
                    structureEquality &= (this.Unknown1 == compare.Unknown1);
                    structureEquality &= (this.Unknown2 == compare.Unknown2);
                    structureEquality &= (this.Unknown3 == compare.Unknown3);

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.Signature.Value.GetHashCode();
            hash ^= this.Unknown1;
            hash ^= this.Unknown2;
            hash ^= this.Unknown3;

            return hash;
        }
        #endregion
    }
}