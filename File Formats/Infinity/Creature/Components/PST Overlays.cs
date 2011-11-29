using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    /// <summary>
    ///     This class details visual effects applied to the creature by opcode 201
    ///     (the specific effect applied is controled by paramter 2).
    /// </summary>
    /// <remarks>
    ///     This can't be 40 bytes, not 38; it has to be 36, as when I cast Balance in all Things AND
    ///     Guardian Mantle, both appeared one after another, taking up 36 bytes. There could be a
    ///     couple of trailing bytes, but maybe/maybe not. Anyway, for entries, this is now 36.
    /// </remarks>
    public class PstOverlay : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 36;

        #region Members
        /// <summary>Referenced BAM to overlay over creature</summary>
        protected ResourceReference overlayBam;

        /// <summary>First unknown value</summary>
        /// <value>I've seen 0x40 here</value>
        protected UInt32 unknown1;

        /// <summary>Timing value</summary>
        protected UInt16 timing;

        /// <summary>Type value</summary>
        protected UInt16 type;

        /// <summary>Duration value</summary>
        protected UInt32 duration;

        /// <summary>Second unknown value</summary>
        /// <value>Seen 0 so far</value>
        protected UInt32 unknown2;

        /// <summary>Third unknown value</summary>
        /// <value>Seen 0 so far</value>
        protected UInt32 unknown3;

        /// <summary>Fourth unknown value</summary>
        /// <value>Seen 0xFF FF FF 00 so far</value>
        protected UInt32 unknown4;

        /// <summary>Fifth unknown value</summary>
        /// <value>Seen 0 so far</value>
        protected UInt32 unknown5;

        ///// <summary>Sixth unknown value</summary>
        ///// <value>Seen 0 so far</value>
        //protected UInt16 unknown6;

        ///// <summary>Seventh unknown value</summary>
        ///// <value>Seen 0xFF FF so far</value>
        ///// <remarks>IESDP says 40 bytes, but then describes only 38. is this valid?</remarks>
        //protected UInt16 unknown7;
        #endregion

        #region Properties
        /// <summary>Referenced BAM to overlay over creature</summary>
        public ResourceReference OverlayBam
        {
            get { return this.overlayBam; }
            set { this.overlayBam = value; }
        }

        /// <summary>First unknown value</summary>
        /// <value>I've seen 0x40 here</value>
        public UInt32 Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }

        /// <summary>Timing value</summary>
        public UInt16 Timing
        {
            get { return this.timing; }
            set { this.timing = value; }
        }

        /// <summary>Type value</summary>
        public UInt16 Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>Duration value</summary>
        public UInt32 Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }

        /// <summary>Second unknown value</summary>
        /// <value>Seen 0 so far</value>
        public UInt32 Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Third unknown value</summary>
        /// <value>Seen 0 so far</value>
        public UInt32 Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }

        /// <summary>Fourth unknown value</summary>
        /// <value>Seen 0xFF FF FF 00 so far</value>
        public UInt32 Unknown4
        {
            get { return this.unknown4; }
            set { this.unknown4 = value; }
        }

        /// <summary>Fifth unknown value</summary>
        /// <value>Seen 0 so far</value>
        public UInt32 Unknown5
        {
            get { return this.unknown5; }
            set { this.unknown5 = value; }
        }

        ///// <summary>Sixth unknown value</summary>
        ///// <value>Seen 0 so far</value>
        //public UInt16 Unknown6
        //{
        //    get { return this.unknown6; }
        //    set { this.unknown6 = value; }
        //}

        ///// <summary>Seventh unknown value</summary>
        ///// <value>Seen 0xFF FF so far</value>
        ///// <remarks>IESDP says 40 bytes, but then describes only 38. is this valid?</remarks>
        //public UInt16 Unknown7
        //{
        //    get { return this.unknown7; }
        //    set { this.unknown7 = value; }
        //}
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public PstOverlay()
        {
            this.overlayBam = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.overlayBam = new ResourceReference();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //binary read
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 40);

            this.overlayBam.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 0, Constants.CultureCodeEnglish);
            this.unknown1 = ReusableIO.ReadUInt32FromArray(remainingBody, 8);
            this.timing = ReusableIO.ReadUInt16FromArray(remainingBody, 12);
            this.type = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.duration = ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.unknown2 = ReusableIO.ReadUInt32FromArray(remainingBody, 20);
            this.unknown3 = ReusableIO.ReadUInt32FromArray(remainingBody, 24);
            this.unknown4 = ReusableIO.ReadUInt32FromArray(remainingBody, 28);
            this.unknown5 = ReusableIO.ReadUInt32FromArray(remainingBody, 32);
            //this.unknown6 = ReusableIO.ReadUInt16FromArray(remainingBody, 36);
            //this.unknown7 = ReusableIO.ReadUInt16FromArray(remainingBody, 38);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.overlayBam.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.unknown1, output);
            ReusableIO.WriteUInt16ToStream(this.timing, output);
            ReusableIO.WriteUInt16ToStream(this.type, output);
            ReusableIO.WriteUInt32ToStream(this.duration, output);
            ReusableIO.WriteUInt32ToStream(this.unknown2, output);
            ReusableIO.WriteUInt32ToStream(this.unknown3, output);
            ReusableIO.WriteUInt32ToStream(this.unknown4, output);
            ReusableIO.WriteUInt32ToStream(this.unknown5, output);
            //ReusableIO.WriteUInt16ToStream(this.unknown6, output);
            //ReusableIO.WriteUInt16ToStream(this.unknown7, output);
        }
        #endregion

        #region ToString() Helpers
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
            String header = "PST Creature Overlay:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Overlay Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Overlay resource"));
            builder.Append(String.Format("'{0}'", this.overlayBam.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.unknown1);
            builder.Append(StringFormat.ToStringAlignment("Timing"));
            builder.Append(this.timing);
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append(this.type);
            builder.Append(StringFormat.ToStringAlignment("Duration"));
            builder.Append(this.duration);
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(this.unknown2);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3"));
            builder.Append(this.unknown3);
            builder.Append(StringFormat.ToStringAlignment("Unknown #4"));
            builder.Append(this.unknown4);
            builder.Append(StringFormat.ToStringAlignment("Unknown #5"));
            builder.Append(this.unknown5);

            return builder.ToString();
        }
        #endregion
    }
}