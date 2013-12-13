using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents the common opcode data: size, code and version</summary>
    public class Opcode : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of an opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 4;
        #endregion


        #region Fields
        /// <summary>Length of the opcode data</summary>
        public UInt16 Length { get; set; }

        /// <summary>Version of the opcode</summary>
        public Byte Version { get; set; }

        /// <summary>Tthe OpcodeTypes enum for this opcode</summary>
        public OpcodeTypes OpCode { get; set; }

        /// <summary>Parameters and data for the opcode</summary>
        public OpcodeData Data { get; set; }

        /// <summary>Start offset of the Opcode</summary>
        public Int64 OpcodeStartOffset { get; set; }

        /// <summary>Start offset of the Opcode data & parameters</summary>
        public Int64 OpcodeDataOffset { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the version-specific opcode representation</summary>
        public virtual OpcodeVersions Operation
        {
            get { return this.GetVersion(); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
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
        /// <remarks>The reader must read an opcode prior to reading the body, and the opcode is the only common datum to all opcode subchunks</remarks>
        public virtual void ReadBody(Stream input)
        {
            this.OpcodeStartOffset = input.Position;

            Byte[] opcode = ReusableIO.BinaryRead(input, Opcode.OpcodeSize);

            this.Length = ReusableIO.ReadUInt16FromArray(opcode, 0);
            this.OpCode = (OpcodeTypes)opcode[2];
            this.Version = opcode[3];


            this.OpcodeDataOffset = input.Position;
            this.ReadContent(input);
        }

        /// <summary>Reads a single opcode from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>An opcode type</returns>
        public static OpcodeTypes ReadOpcode(Stream input)
        {
            return (OpcodeTypes)ReusableIO.BinaryReadByte(input);
        }

        /// <summary>Reads the opcode length from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>The opcode length</returns>
        public static UInt32 ReadLength(Stream input)
        {
            Byte[] datum = ReusableIO.BinaryRead(input, 4);
            return ReusableIO.ReadUInt32FromArray(datum, 0);
        }
        
        /// <summary>Reads opcode content and data from the input stream, otherwise it skips the length to the next opcode.</summary>
        /// <param name="input">Stream to read from</param>
        protected virtual void ReadContent(Stream input)
        {
            OpcodeData data = null;

            switch (this.Operation)
            {
                case OpcodeVersions.CreateTimer:
                    data = new CreateTimer();
                    break;
                case OpcodeVersions.InitializeAudioBuffers0:
                    data = new InitializeAudioBuffers0();
                    break;
                case OpcodeVersions.InitializeAudioBuffers1:
                    data = new InitializeAudioBuffers1();
                    break;
                case OpcodeVersions.InitializeVideoBuffers0:
                    data = new InitializeVideoBuffers0();
                    break;
                case OpcodeVersions.InitializeVideoBuffers1:
                    data = new InitializeVideoBuffers1();
                    break;
                case OpcodeVersions.InitializeVideoBuffers2:
                    data = new InitializeVideoBuffers2();
                    break;
                case OpcodeVersions.RenderVideoBuffer0:
                    data = new RenderBuffer0();
                    break;
                case OpcodeVersions.RenderVideoBuffer1:
                    data = new RenderBuffer1();
                    break;
                case OpcodeVersions.AudioSamples:
                    data = new AudioStreamData(this.Length);
                    break;
                case OpcodeVersions.AudioSilence:
                    data = new AudioStream();
                    break;
                case OpcodeVersions.InitializeVideoStream:
                    data = new InitializeVideoStream();
                    break;
                case OpcodeVersions.SetPalette:
                    data = new SetPalette();
                    break;
                case OpcodeVersions.SetDecodingMap:
                    data = new SetDecodingMap(this.Length); //needs to know how much data to read
                    break;
                case OpcodeVersions.VideoData:
                    data = new VideoData(this.Length); //needs to know how much data to read
                    break;

                //no real data
                case OpcodeVersions.EndOfStream:
                case OpcodeVersions.EndOfChunk:
                case OpcodeVersions.ToggleAudio:

                default:    // unknown/unused, skip
                    ReusableIO.SeekIfAble(input, this.Length, SeekOrigin.Current);
                    //allow for common read to be removed from the cases. Goto can be appropriate from time to time.
                    goto Assignment;
            }
            
            data.Read(input);   //common read

        Assignment:
            this.Data = data;
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Length, output);
            output.WriteByte((Byte)this.OpCode);
            output.WriteByte(this.Version);
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
        public virtual String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Int32 index)
        {
            String header = this.GetVersionString(index);
            header += this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "Opcode:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected virtual String GetVersionString(Int32 index)
        {
            return String.Format("Opcode # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Length"));
            builder.Append(this.Length);
            builder.Append(StringFormat.ToStringAlignment("Opcode"));
            builder.Append(this.OpCode.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Opcode value"));
            builder.Append((UInt16)this.OpCode);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.Version);

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
                if (obj != null && obj is Opcode)
                {
                    Opcode compare = obj as Opcode;
                    equal = (this.OpCode == compare.OpCode);
                    equal &= (this.Length == compare.Length);
                    equal &= (this.Version == compare.Version);
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.Length.GetHashCode();
            hash ^= (Byte)this.OpCode;
            hash ^= this.Version;

            return hash;
        }
        #endregion

        /// <summary>Gets the opcode version enumeration value</summary>
        /// <returns>An appropriate OpcodeVersions enum value</returns>
        protected virtual OpcodeVersions GetVersion()
        {
            OpcodeVersions opcode;

            switch (this.OpCode)
            {
                case OpcodeTypes.CreateTimer:
                    opcode = OpcodeVersions.CreateTimer;
                    break;
                case OpcodeTypes.InitializeAudioBuffers:
                    switch (this.Version)
                    {
                        case 0:
                            opcode = OpcodeVersions.InitializeAudioBuffers0;
                            break;
                        case 1:
                            opcode = OpcodeVersions.InitializeAudioBuffers1;
                            break;
                        default:
                            throw new ApplicationException(String.Format("Unexpected value of {0}", this.Version));
                    }
                    break;
                case OpcodeTypes.InitializeVideoBuffers:
                    switch (this.Version)
                    {
                        case 0:
                            opcode = OpcodeVersions.InitializeVideoBuffers0;
                            break;
                        case 1:
                            opcode = OpcodeVersions.InitializeVideoBuffers1;
                            break;
                        case 2:
                            opcode = OpcodeVersions.InitializeVideoBuffers2;
                            break;
                        default:
                            throw new ApplicationException(String.Format("Unexpected value of {0}", this.Version));
                    }
                    break;
                case OpcodeTypes.RenderVideoBuffer:
                    switch (this.Version)
                    {
                        case 0:
                            opcode = OpcodeVersions.RenderVideoBuffer0;
                            break;
                        case 1:
                            opcode = OpcodeVersions.RenderVideoBuffer1;
                            break;
                        default:
                            throw new ApplicationException(String.Format("Unexpected value of {0}", this.Version));
                    }
                    break;
                case OpcodeTypes.AudioSamples:
                    opcode = OpcodeVersions.AudioSamples;
                    break;
                case OpcodeTypes.AudioSilence:
                    opcode = OpcodeVersions.AudioSilence;
                    break;
                case OpcodeTypes.InitializeVideoStream:
                    opcode = OpcodeVersions.InitializeVideoStream;
                    break;
                case OpcodeTypes.SetPalette:
                    opcode = OpcodeVersions.SetPalette;
                    break;
                case OpcodeTypes.SetDecodingMap:
                    opcode = OpcodeVersions.SetDecodingMap;
                    break;
                case OpcodeTypes.VideoData:
                    opcode = OpcodeVersions.VideoData;
                    break;
                case OpcodeTypes.EndOfStream:
                    opcode = OpcodeVersions.EndOfStream;
                    break;
                case OpcodeTypes.EndOfChunk:
                    opcode = OpcodeVersions.EndOfChunk;
                    break;
                case OpcodeTypes.ToggleAudio:
                    opcode = OpcodeVersions.ToggleAudio;
                    break;
                default:    // unknown/unused, skip
                    opcode = OpcodeVersions.Invalid;
                    break;
            }

            return opcode;
        }
    }
}