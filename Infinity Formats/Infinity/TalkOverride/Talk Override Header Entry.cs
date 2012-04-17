using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Represents an entry for a given String Reference in the TOH talk override header</summary>
    public class TalkOverrideHeaderEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this data structure on disk</summary>
        public const Int32 StructSize = 0x1C;
        #endregion


        #region Fields
        /// <summary>Index to the string number to override or add</summary>
        public Int32 StringReferenceIndex { get; set; }

        /// <summary>Presently unknown DWORD at offset 4</summary>
        /// <value>
        ///     I have observed 1 and 0, where 1 seems to indicate that yes, there is associated text and 0 seems to indicate that there is none.
        ///     Regardless of the value, there still appears to be an associated table entry.
        /// </value>
        protected Int32 AssociatedText { get; set; }

        /// <summary>Presently unknown DWORD at offset 8</summary>
        /// <value>
        ///     I think this is a flags field.
        ///     Mostly, I see 16, 32, 11250944. I have seen 48, 33, 145, 96, 1441817, 64, 1966263, 
        /// </value>
        /// <remarks>
        ///     These flags confuse me. For example, in a set of automap notes, I see STRREFs 3000016-3000018,
        ///     and values of { 16, 11250944, 11250944 }, respectively.
        /// </remarks>
        public Int32 Unknown_0x0008 { get; set; }

        /// <summary>Presently unknown DWORD at offset 12</summary>
        /// <value>
        ///     I usually see 0.
        ///     I have seen 32, 0, 33
        /// </value>
        public Int32 Unknown_0x000C { get; set; }

        ///// <summary>Presently unknown WORD at offset 8</summary>
        //public Int16 Unknown_0x0008 { get; set; }

        ///// <summary>Presently unknown WORD at offset 10</summary>
        //public Int32 Unknown_0x000A { get; set; }

        ///// <summary>Presently unknown WORD at offset 14</summary>
        //public Int16 Unknown_0x000E { get; set; }


        /*
         * For offsets 0x0008 and 0x000C I usually see 0 and 0 both.
         * Often I see 16 and 33, respectively.
         * Other pairs I've seen:
         * 33 and 32,
         * 1441817 and 1279655936
         * 48 and 33
         * 64 and 33
         * 11250944 and 0
         * 1966263 and -65536
         * 145 and 32
         * 96 and 33
         */


        /*
         * Looking at some Data, I am slowly becoming convinced that offset 0x0008 is a WORD, and 0x000A is a DWORD and 0x0000E is a WORD.
         * Changing accordingly.
         */ 

        /// <summary>Presumed to be a RESREF to associated Sound clip</summary>
        public ResourceReference WavReference { get; set; }

        /// <summary>Offset into the associated TLK Override Table where text resides</summary>
        public Int32 TableOffset { get; set; }
        #endregion


        #region Properties
        public Boolean AssociatedTextPresent
        {
            get { return Convert.ToBoolean(this.AssociatedText); }
            set { this.AssociatedText = value ? 1 : 0; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.WavReference = new ResourceReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, TalkOverrideHeaderEntry.StructSize);
            this.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.AssociatedText = ReusableIO.ReadInt32FromArray(buffer, 4);
            
            //this.Unknown_0x0008 = ReusableIO.ReadInt16FromArray(buffer, 8);
            //this.Unknown_0x000A = ReusableIO.ReadInt32FromArray(buffer, 10);
            //this.Unknown_0x000E = ReusableIO.ReadInt16FromArray(buffer, 14);

            this.Unknown_0x0008 = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.Unknown_0x000C = ReusableIO.ReadInt32FromArray(buffer, 12);

            this.WavReference.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish);
            this.TableOffset = ReusableIO.ReadInt32FromArray(buffer, 24);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.AssociatedText, output);

            //ReusableIO.WriteInt16ToStream(this.Unknown_0x0008, output);
            //ReusableIO.WriteInt32ToStream(this.Unknown_0x000A, output);
            //ReusableIO.WriteInt16ToStream(this.Unknown_0x000E, output);

            ReusableIO.WriteInt32ToStream(this.Unknown_0x0008, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x000C, output);

            ReusableIO.WriteStringToStream(this.WavReference.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.TableOffset, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Talk Override Header Entry:");
            builder.Append(StringFormat.ToStringAlignment("String Reference Index"));
            builder.Append(this.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Associated text is present in TOT"));
            builder.Append(this.AssociatedTextPresent);

            //builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x0008"));
            //builder.Append(this.Unknown_0x0008);
            //builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x000A"));
            //builder.Append(this.Unknown_0x000A);
            //builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x000E"));
            //builder.Append(this.Unknown_0x000E);

            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x0008"));
            builder.Append(this.Unknown_0x0008);
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x000C"));
            builder.Append(this.Unknown_0x000C);

            builder.Append(StringFormat.ToStringAlignment("WavReference"));
            builder.Append(String.Format("'{0}'", this.WavReference.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Offset into the TOT where string is stored"));
            builder.AppendLine(this.TableOffset.ToString());

            return builder.ToString();
        }
        #endregion
    }
}