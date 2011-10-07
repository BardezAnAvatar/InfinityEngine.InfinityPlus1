using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey
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
    public struct TextLocationKeyStringReference
    {
        #region Private Members
        /// <summary>This Int16 member is a flag field of options for the string entry</summary>
        private Int16 strRefFlags;

        /// <summary>This is a reference to the appropriate</summary>
        private ResourceReference soundResRef;

        /// <summary>This is the related sound's volume variance</summary>
        private Int32 volumeVariance;

        /// <summary>This is the related sound's pitch variance</summary>
        private Int32 pitchVariance;

        /// <summary>This is the offset to the string being refrenced</summary>
        private Int32 stringOffset;

        /// <summary>This is the length of the string being referenced</summary>
        private Int32 stringLength;

        /// <summary>This is the actual string referenced</summary>
        private String stringReferenced;
        #endregion

        #region Public Properties
        /// <summary>This public property gets and sets a collection of reference state flags for the string entry</summary>
        public InfinityEngineStringReferenceFlags ReferenceFlags
        {
            get { return (InfinityEngineStringReferenceFlags)strRefFlags; }
            set { strRefFlags = (Int16)value; }
        }

        /// <summary>This public property gets and sets the associated sound's resource1 reference</summary>
        public ResourceReference SoundResourceReference
        {
            get { return soundResRef; }
            set { soundResRef = value; }
        }

        /// <summary>This public property gets and sets the associated sound's volume variance. There is little explination of this.</summary>
        public Int32 VolumeVariance
        {
            get { return volumeVariance; }
            set { volumeVariance = value; }
        }

        /// <summary>This public property gets and sets the associated sound's pitch variance. There is little explination of this.</summary>
        public Int32 PitchVariance
        {
            get { return pitchVariance; }
            set { pitchVariance = value; }
        }

        /// <summary>This public property gets and sets the offset to the string</summary>
        public Int32 StringOffset
        {
            get { return stringOffset; }
            set { stringOffset = value; }
        }

        /// <summary>This public property gets and sets the length of the string being referenced</summary>
        public Int32 StringLength
        {
            get { return stringLength; }
            set { stringLength = value; }
        }

        /// <summary>This public property gets and sets the string referenced. Setting this will update the stringLength value</summary>
        public String StringReferenced
        {
            get { return stringReferenced; }
            set
            {
                stringReferenced = value;
                Encoding encoding = new ASCIIEncoding();
                stringLength = encoding.GetByteCount(value);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>This method reads a fullString reference</summary>
        /// <param name="Source">Binary Stream to read from</param>
        /// <param name="StringDataOffset">Offset in the stream where the String data begins</param>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenceFull(Stream Source, Int32 StringDataOffset, String CultureReference)
        {
            ReadStringReferenceEntry(Source, CultureReference);
            ReadStringReferenced(Source, StringDataOffset, CultureReference);
        }

        /// <summary>This method reads the String Reference from the TLK file.</summary>
        /// <param name="Source">The binary Stream to read from</param>
        /// <remarks>Stream must be in the proper location to read, as there is no native seek/offset in the file structure</remarks>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenceEntry(Stream Source, String CultureReference)
        {
            //read entry
            Byte[] buffer = ReusableIO.BinaryRead(Source, 26);

            //Flags
            strRefFlags = ReusableIO.ReadInt16FromArray(buffer, 0);

            //sound resref
            soundResRef = new ResourceReference();
            soundResRef.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 2, CultureReference);

            //volume variance
            volumeVariance = ReusableIO.ReadInt32FromArray(buffer, 0xA);

            //pitch variance
            pitchVariance = ReusableIO.ReadInt32FromArray(buffer, 0xE);

            //string offset
            stringOffset = ReusableIO.ReadInt32FromArray(buffer, 0x12);

            //string offset
            stringLength = ReusableIO.ReadInt32FromArray(buffer, 0x16);
        }

        /// <summary>This private method reads the referenced string from the file</summary>
        /// <param name="Input">Stream to read from</param>
        /// <param name="StringDataOffset">Offset to the start of the string data</param>
        /// <param name="CultureReference">String representing the source culture</param>
        public void ReadStringReferenced(Stream Input, Int32 StringDataOffset, String CultureReference)
        {
            //seek if necessary
            Int64 absoluteOffset = StringDataOffset + stringOffset;
            ReusableIO.SeekIfAble(Input, absoluteOffset, SeekOrigin.Begin);

            Byte[] buffer = ReusableIO.BinaryRead(Input, stringLength);
            StringReferenced = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureReference, stringLength);
        }

        /// <summary>This public method writes the String reference entry to an output stream</summary>
        /// <param name="Output">Stream object w=into which to write to</param>
        public void Write(Stream Output)
        {
            Byte[] writeBytes;

            //Flags
            writeBytes = BitConverter.GetBytes(this.strRefFlags);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //Sound resref
            writeBytes = ReusableIO.WriteStringToByteArray(this.soundResRef.ResRef, 8);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //Volume variance
            writeBytes = BitConverter.GetBytes(this.volumeVariance);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //Pitch variance
            writeBytes = BitConverter.GetBytes(this.pitchVariance);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //String data offset
            writeBytes = BitConverter.GetBytes(this.stringOffset);
            Output.Write(writeBytes, 0, writeBytes.Length);

            //String data Length
            writeBytes = BitConverter.GetBytes(this.stringLength);
            Output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public member overrides the default ToString() method</summary>
        /// <returns>A string containing the values and descriptions of all values in this struct</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TextLocationKeyStringReference:");
            builder.Append("\n     String Flags: ");
            builder.Append(strRefFlags);
            //print the flags
            if (ReferenceFlags == InfinityEngineStringReferenceFlags.None)
                builder.Append("\n                      * None");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Text) == InfinityEngineStringReferenceFlags.Text)
                builder.Append("\n                      * Text");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Sound) == InfinityEngineStringReferenceFlags.Sound)
                builder.Append("\n                      * Sound");
            if ((ReferenceFlags & InfinityEngineStringReferenceFlags.Tags) == InfinityEngineStringReferenceFlags.Tags)
                builder.Append("\n                      * Tags");

            builder.Append("\n     Sound ResRef: '");
            builder.Append(soundResRef.ResRef);
            builder.Append("'\n  Volume Variance: ");
            builder.Append(volumeVariance);
            builder.Append("\n   Pitch Variance: ");
            builder.Append(pitchVariance);
            builder.Append("\n    String Offset: ");
            builder.Append(stringOffset);
            builder.Append("\n    String Length: ");
            builder.Append(stringLength);
            builder.Append("\nString Referenced: ");
            builder.Append(stringReferenced);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}