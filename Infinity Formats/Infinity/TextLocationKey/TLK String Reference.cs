using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TextLocationKey
{
    /// <summary>This public struct is the entry for a string reference within the TLK file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset  	Size (data type)  	Description
    ///     0x0000 	    2 (word) 	        Flags
    ///                                         * 00 - No message data
    ///                                         * 01 - Text exists
    ///                                         * 02 - Sound exists
    ///                                         * 03 - Standard message. Ambient message. Used for sound without text (BG1) or message displayed over characters head (BG2), Message with tags (for instance &lt;CHARNAME&gt;) for all games except BG2
    ///                                         * 07 - Message with tags (for instance &lt;CHARNAME&gt; ) in BG2 only
    ///     0x0002 	    8 (resref) 	        Resource name of associated sound
    ///     0x000a 	    4 (dword) 	        Volume variance
    ///     0x000e 	    4 (dword) 	        Pitch variance
    ///     0x0012 	    4 (dword) 	        Offset of this string relative to the strings section
    ///     0x0016 	    4 (dword) 	        Length of this string
    /// </remarks>
    public class TextLocationKeyStringReference : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 26;

        #region Fields
        /// <summary>This is the actual string referenced</summary>
        private String stringReferenced;
        #endregion

        #region Properties
        /// <summary>This property gets and sets a collection of reference state flags for the string entry</summary>
        public InfinityEngineStringReferenceFlags ReferenceFlags { get; set ;}

        /// <summary>This property gets and sets the associated sound's resource1 reference</summary>
        public ResourceReference SoundResourceReference { get; set; }

        /// <summary>This property gets and sets the associated sound's volume variance. There is little explanation of this.</summary>
        public Int32 VolumeVariance { get; set; }

        /// <summary>This property gets and sets the associated sound's pitch variance. There is little explanation of this.</summary>
        public Int32 PitchVariance { get; set; }

        /// <summary>This property gets and sets the offset to the string</summary>
        public Int32 StringOffset { get; set; }

        /// <summary>This property gets and sets the length of the string being referenced</summary>
        public Int32 StringLength { get; set; }

        /// <summary>This property gets and sets the string referenced. Setting this will update the stringLength value</summary>
        public String StringReferenced
        {
            get { return stringReferenced; }
            set
            {
                this.stringReferenced = value;
                this.StringLength = new ASCIIEncoding().GetByteCount(value);
            }
        }

        /// <summary>Localized culture code string, meant for decoding specific ASCII strings into .NET</summary>
        /// <remarks>Not specifically part of the data structure.</remarks>
        public String CultureCode { get; set; }
        #endregion

        #region Consruction
        /// <summary>Localization constructor</summary>
        /// <param name="cultureCode">String representing the culture code to use when reading strings</param>
        public TextLocationKeyStringReference(String cultureCode)
        {
            this.CultureCode = cultureCode;
            this.SoundResourceReference = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.SoundResourceReference = new ResourceReference();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <param name="input">Stream to read from</param>
        public void Read(Stream input)
        {
            this.Initialize();
            this.ReadStringReferenceEntry(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);

            if (fullRead)
                this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.ReadStringReferenced(input, this.StringOffset);
        }

        /// <summary>This public method writes the String reference entry to an output stream</summary>
        /// <param name="output">Stream object w=into which to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt16ToStream((Int16)this.ReferenceFlags, output);
            ReusableIO.WriteStringToStream(this.SoundResourceReference.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.VolumeVariance, output);
            ReusableIO.WriteInt32ToStream(this.PitchVariance, output);
            ReusableIO.WriteInt32ToStream(this.StringOffset, output);
            ReusableIO.WriteInt32ToStream(this.StringLength, output);
        }
        #endregion

        #region Public Methods
        /// <summary>This method reads a fullString reference</summary>
        /// <param name="source">Binary Stream to read from</param>
        /// <param name="stringDataOffset">Offset in the stream where the String data begins</param>
        public void ReadStringReferenceFull(Stream source, Int32 stringDataOffset)
        {
            this.ReadStringReferenceEntry(source);
            this.ReadStringReferenced(source, stringDataOffset);
        }

        /// <summary>This method reads the String Reference from the TLK file.</summary>
        /// <param name="source">The binary Stream to read from</param>
        /// <remarks>Stream must be in the proper location to read, as there is no native seek/offset in the file structure</remarks>
        public void ReadStringReferenceEntry(Stream source)
        {
            //read entry
            Byte[] buffer = ReusableIO.BinaryRead(source, 26);

            this.ReferenceFlags = (InfinityEngineStringReferenceFlags)ReusableIO.ReadInt16FromArray(buffer, 0);
            this.SoundResourceReference.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 2, CultureConstants.CultureCodeEnglish);
            this.VolumeVariance = ReusableIO.ReadInt32FromArray(buffer, 0xA);
            this.PitchVariance = ReusableIO.ReadInt32FromArray(buffer, 0xE);
            this.StringOffset = ReusableIO.ReadInt32FromArray(buffer, 0x12);
            this.StringLength = ReusableIO.ReadInt32FromArray(buffer, 0x16);
        }

        /// <summary>This private method reads the referenced string from the file</summary>
        /// <param name="Input">Stream to read from</param>
        /// <param name="stringDataOffset">Offset to the start of the string data</param>
        public void ReadStringReferenced(Stream Input, Int32 stringDataOffset)
        {
            //seek if necessary
            ReusableIO.SeekIfAble(Input, stringDataOffset + this.StringOffset);

            Byte[] buffer = ReusableIO.BinaryRead(Input, this.StringLength);
            this.stringReferenced = ReusableIO.ReadStringFromByteArray(buffer, 0, this.CultureCode, this.StringLength);
        }

        /// <summary>This public member overrides the default ToString() method</summary>
        /// <returns>A string containing the values and descriptions of all values in this struct</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("TextLocationKeyStringReference:", 0));
            builder.Append(StringFormat.ToStringAlignment("String Flags"));
            builder.Append((Int16)this.ReferenceFlags);
            builder.Append(this.GetFlags());

            builder.Append(StringFormat.ToStringAlignment("Sound ResRef"));
            builder.Append(String.Format("'{0}'", this.SoundResourceReference.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Volume Variance"));
            builder.Append(this.VolumeVariance);
            builder.Append(StringFormat.ToStringAlignment("Pitch Variance"));
            builder.Append(this.PitchVariance);
            builder.Append(StringFormat.ToStringAlignment("String Offset"));
            builder.Append(this.StringOffset);
            builder.Append(StringFormat.ToStringAlignment("String Length"));
            builder.Append(this.StringLength);
            builder.Append(StringFormat.ToStringAlignment("String Referenced"));
            builder.Append(String.Format("'{0}'", this.stringReferenced));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which InfinityEngineStringReferenceFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFlags()
        {
            StringBuilder sb = new StringBuilder();

            //first one is set by the typical ending return statement
            //StringFormat.AppendSubItem(sb, (this.ReferenceFlags & InfinityEngineStringReferenceFlags.None) == InfinityEngineStringReferenceFlags.None, InfinityEngineStringReferenceFlags.None.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ReferenceFlags & InfinityEngineStringReferenceFlags.Text) == InfinityEngineStringReferenceFlags.Text, InfinityEngineStringReferenceFlags.Text.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ReferenceFlags & InfinityEngineStringReferenceFlags.Sound) == InfinityEngineStringReferenceFlags.Sound, InfinityEngineStringReferenceFlags.Sound.GetDescription());
            StringFormat.AppendSubItem(sb, (this.ReferenceFlags & InfinityEngineStringReferenceFlags.Tags) == InfinityEngineStringReferenceFlags.Tags, InfinityEngineStringReferenceFlags.Tags.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}