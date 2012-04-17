using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an area song</summary>
    public class Song : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 144;
        #endregion


        #region Fields
        /// <summary>Song number during the day</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32 SongNumberDay { get; set; }

        /// <summary>Song number during the night</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32 SongNumberNight { get; set; }

        /// <summary>Song number after battle victory</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32 SongNumberVictory { get; set; }

        /// <summary>Song number during battle</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32 SongNumberBattle { get; set; }

        /// <summary>Song number after battle defeat</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32 SongNumberDefeat { get; set; }

        /// <summary>Array of 5 additional songs</summary>
        /// <remarks>Matches to song.ids</remarks>
        public Int32[] AdditionalSongs { get; set; }

        /// <summary>Main ambient # 1 during the day</summary>
        public ResourceReference MainAmbientDay1 { get; set; }

        /// <summary>Main ambient # 2 during the day</summary>
        public ResourceReference MainAmbientDay2 { get; set; }

        /// <summary>Ambient volume level during the day</summary>
        public Int32 AmbientVolumeDay { get; set; }

        /// <summary>Main ambient # 1 during the night</summary>
        public ResourceReference MainAmbientNight1 { get; set; }

        /// <summary>Main ambient # 2 during the night</summary>
        public ResourceReference MainAmbientNight2 { get; set; }

        /// <summary>Ambient volume level during the night</summary>
        public Int32 AmbientVolumeNight { get; set; }

        /// <summary>EAX Area Reverberation</summary>
        /// <remarks>Matches to REVERB.IDS</remarks>
        public Int32 AudioReverberation { get; set; }

        /// <summary>Trailing 60 bytesof padding at offset 0x54</summary>
        public Byte[] Padding_0x0054 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.AdditionalSongs = new Int32[5];
            this.MainAmbientDay1 = new ResourceReference();
            this.MainAmbientDay2 = new ResourceReference();
            this.MainAmbientNight1 = new ResourceReference();
            this.MainAmbientNight2 = new ResourceReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 84);

            this.SongNumberDay = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.SongNumberNight = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.SongNumberVictory = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.SongNumberBattle = ReusableIO.ReadInt32FromArray(buffer, 12);
            this.SongNumberDefeat = ReusableIO.ReadInt32FromArray(buffer, 16);

            for (Int32 index = 0; index < 5; ++index)
                this.AdditionalSongs[index] = ReusableIO.ReadInt32FromArray(buffer, 20 + (4 * index));

            this.MainAmbientDay1.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 40, CultureConstants.CultureCodeEnglish);
            this.MainAmbientDay2.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 48, CultureConstants.CultureCodeEnglish);
            this.AmbientVolumeDay = ReusableIO.ReadInt32FromArray(buffer, 56);
            this.MainAmbientNight1.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 60, CultureConstants.CultureCodeEnglish);
            this.MainAmbientNight2.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 68, CultureConstants.CultureCodeEnglish);
            this.AmbientVolumeNight = ReusableIO.ReadInt32FromArray(buffer, 76);
            this.AudioReverberation = ReusableIO.ReadInt32FromArray(buffer, 80);

            this.Padding_0x0054 = ReusableIO.BinaryRead(input, 60);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.SongNumberDay, output);
            ReusableIO.WriteInt32ToStream(this.SongNumberNight, output);
            ReusableIO.WriteInt32ToStream(this.SongNumberVictory, output);
            ReusableIO.WriteInt32ToStream(this.SongNumberBattle, output);
            ReusableIO.WriteInt32ToStream(this.SongNumberDefeat, output);

            for (Int32 index = 0; index < 5; ++index)
                ReusableIO.WriteInt32ToStream(this.AdditionalSongs[index], output);

            ReusableIO.WriteStringToStream(this.MainAmbientDay1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.MainAmbientDay2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.AmbientVolumeDay, output);
            ReusableIO.WriteStringToStream(this.MainAmbientNight1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.MainAmbientNight2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.AmbientVolumeNight, output);
            ReusableIO.WriteInt32ToStream(this.AudioReverberation, output);
            output.Write(this.Padding_0x0054, 0, 60);
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
            String header = this.GetVersionString();

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
            return StringFormat.ReturnAndIndent(String.Format("Song # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Song:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Song number during the day"));
            builder.Append(this.SongNumberDay);
            builder.Append(StringFormat.ToStringAlignment("Song number during the night"));
            builder.Append(this.SongNumberNight);
            builder.Append(StringFormat.ToStringAlignment("Song number after battle victory"));
            builder.Append(this.SongNumberVictory);
            builder.Append(StringFormat.ToStringAlignment("Song number during battle"));
            builder.Append(this.SongNumberBattle);
            builder.Append(StringFormat.ToStringAlignment("Song number after battle defeat"));
            builder.Append(this.SongNumberDefeat);

            for (Int32 index = 0; index < 5; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Additional song # {0}", index)));
                builder.Append(this.AdditionalSongs[index]);
            }

            builder.Append(StringFormat.ToStringAlignment("Main ambient # 1 during the day"));
            builder.Append(String.Format("'{0}'", this.MainAmbientDay1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Main ambient # 2 during the day"));
            builder.Append(String.Format("'{0}'", this.MainAmbientDay2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Ambient volume during the day"));
            builder.Append(this.AmbientVolumeDay);
            builder.Append(StringFormat.ToStringAlignment("Main ambient # 1 during the night"));
            builder.Append(String.Format("'{0}'", this.MainAmbientNight1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Main ambient # 2 during the night"));
            builder.Append(String.Format("'{0}'", this.MainAmbientNight2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Ambient volume during the night"));
            builder.Append(this.AmbientVolumeNight);
            builder.Append(StringFormat.ToStringAlignment("EAX audio reverberation"));
            builder.Append(this.AudioReverberation);
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x54"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0054));

            return builder.ToString();
        }
        #endregion
    }
}