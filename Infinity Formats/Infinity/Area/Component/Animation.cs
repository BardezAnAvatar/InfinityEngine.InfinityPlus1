using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents a (BAM) animation within an ARE file</summary>
    public class Animation : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 76;
        #endregion


        #region Fields
        /// <summary>Animation's name</summary>
        public ZString Name { get; set; }

        /// <summary>Position of the animation</summary>
        public Point Position { get; set; }

        /// <summary>Schedule flags during which this animation is valid</summary>
        public Schedule ApplicationSchedule { get; set; }

        /// <summary>BAM resource of the animation</summary>
        public ResourceReference BamResource { get; set; }

        /// <summary>Animation sequence number to play from the BAM</summary>
        public Int16 AnimationNumber { get; set; }

        /// <summary>Frame number of the BAM</summary>
        public Int16 FrameNumber { get; set; }

        /// <summary>Miscellaneous animation flags</summary>
        public AnimationFlags Flags { get; set; }

        /// <summary>Height of the animation</summary>
        /// <remarks>Affects wall covering</remarks>
        public Int16 Height { get; set; }

        /// <summary>Transparency of the animation</summary>
        /// <value>-1 is invisible</value>
        public Int16 Transparency { get; set; }

        /// <summary>Start frame of the animation</summary>
        /// <value>0 is random, cleared by Synchronization</value>
        public Int16 StartFrame { get; set; }

        /// <summary>Probability that the animation will loop</summary>
        /// <value>0 = 100</value>
        public Byte LoopChance { get; set; }

        /// <summary>Indicates whether or not to skip cycles to synchronize the animation</summary>
        public Byte SkipCycles { get; set; }

        /// <summary>BMP reference to a palette for the animation</summary>
        public ResourceReference Palette { get; set; }

        /// <summary>Unknown 4 bytes at offset 0x48</summary>
        public Int32 Unknown_0x0048 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Position = new Point();
            this.BamResource = new ResourceReference(ResourceType.Bam);
            this.Palette = new ResourceReference(ResourceType.ImageBitmap);
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

            Byte[] buffer = ReusableIO.BinaryRead(input, Animation.StructSize);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Position.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Position.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.ApplicationSchedule = (Schedule)ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.BamResource.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 40, CultureConstants.CultureCodeEnglish);
            this.AnimationNumber = ReusableIO.ReadInt16FromArray(buffer, 48);
            this.FrameNumber = ReusableIO.ReadInt16FromArray(buffer, 50);
            this.Flags = (AnimationFlags)ReusableIO.ReadUInt32FromArray(buffer, 52);
            this.Height = ReusableIO.ReadInt16FromArray(buffer, 56);
            this.Transparency = ReusableIO.ReadInt16FromArray(buffer, 58);
            this.StartFrame = ReusableIO.ReadInt16FromArray(buffer, 60);
            this.LoopChance = buffer[62];
            this.SkipCycles = buffer[63];
            this.Palette.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 64, CultureConstants.CultureCodeEnglish);
            this.Unknown_0x0048 = ReusableIO.ReadInt32FromArray(buffer, 72);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Position.X, output);
            ReusableIO.WriteUInt16ToStream(this.Position.Y, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ApplicationSchedule, output);
            ReusableIO.WriteStringToStream(this.BamResource.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt16ToStream(this.AnimationNumber, output);
            ReusableIO.WriteInt16ToStream(this.FrameNumber, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt16ToStream(this.Height, output);
            ReusableIO.WriteInt16ToStream(this.Transparency, output);
            ReusableIO.WriteInt16ToStream(this.StartFrame, output);
            output.WriteByte(this.LoopChance);
            output.WriteByte(this.SkipCycles);
            ReusableIO.WriteStringToStream(this.Palette.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0048, output);
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
            return StringFormat.ReturnAndIndent(String.Format("Animation # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Animation:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Current position"));
            builder.Append(this.Position.ToString());
            builder.Append(StringFormat.ToStringAlignment("Animation BAM Resource"));
            builder.Append(String.Format("'{0}'", this.BamResource.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Animation number"));
            builder.Append(this.AnimationNumber);
            builder.Append(StringFormat.ToStringAlignment("Frame number"));
            builder.Append(this.FrameNumber);
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Transparency"));
            builder.Append(this.Transparency);
            builder.Append(StringFormat.ToStringAlignment("Start frame number"));
            builder.Append(this.StartFrame);
            builder.Append(StringFormat.ToStringAlignment("Loop chance %(?)"));
            builder.Append(this.LoopChance);
            builder.Append(StringFormat.ToStringAlignment("Skip frames to sync to rendering"));
            builder.Append(this.SkipCycles);
            builder.Append(StringFormat.ToStringAlignment("Palette Resource"));
            builder.Append(String.Format("'{0}'", this.Palette.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x48"));
            builder.Append(this.Unknown_0x0048);

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.Enabled) == AnimationFlags.Enabled, AnimationFlags.Enabled.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.BlackIsTransparent) == AnimationFlags.BlackIsTransparent, AnimationFlags.BlackIsTransparent.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.NotIlluminated) == AnimationFlags.NotIlluminated, AnimationFlags.NotIlluminated.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.PlayOnce) == AnimationFlags.PlayOnce, AnimationFlags.PlayOnce.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.SyncDraw) == AnimationFlags.SyncDraw, AnimationFlags.SyncDraw.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.NotHiddenByWalls) == AnimationFlags.NotHiddenByWalls, AnimationFlags.NotHiddenByWalls.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.InvisibleInFogOfWar) == AnimationFlags.InvisibleInFogOfWar, AnimationFlags.InvisibleInFogOfWar.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.DrawBeforeActors) == AnimationFlags.DrawBeforeActors, AnimationFlags.DrawBeforeActors.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.PlayAllFrames) == AnimationFlags.PlayAllFrames, AnimationFlags.PlayAllFrames.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.UseBitmapPalette) == AnimationFlags.UseBitmapPalette, AnimationFlags.UseBitmapPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.Mirrored) == AnimationFlags.Mirrored, AnimationFlags.Mirrored.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AnimationFlags.ShowInCombat) == AnimationFlags.ShowInCombat, AnimationFlags.ShowInCombat.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Gets a human-readable enumeration String of set Schedule enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Schedule enumeration values</returns>
        protected String GetScheduleFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_00_30_to_01_30) == Schedule.Time_00_30_to_01_30, Schedule.Time_00_30_to_01_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_01_30_to_02_30) == Schedule.Time_01_30_to_02_30, Schedule.Time_01_30_to_02_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_02_30_to_03_30) == Schedule.Time_02_30_to_03_30, Schedule.Time_02_30_to_03_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_03_30_to_04_30) == Schedule.Time_03_30_to_04_30, Schedule.Time_03_30_to_04_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_04_30_to_05_30) == Schedule.Time_04_30_to_05_30, Schedule.Time_04_30_to_05_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_05_30_to_06_30) == Schedule.Time_05_30_to_06_30, Schedule.Time_05_30_to_06_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_06_30_to_07_30) == Schedule.Time_06_30_to_07_30, Schedule.Time_06_30_to_07_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_07_30_to_08_30) == Schedule.Time_07_30_to_08_30, Schedule.Time_07_30_to_08_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_08_30_to_09_30) == Schedule.Time_08_30_to_09_30, Schedule.Time_08_30_to_09_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_09_30_to_10_30) == Schedule.Time_09_30_to_10_30, Schedule.Time_09_30_to_10_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_10_30_to_11_30) == Schedule.Time_10_30_to_11_30, Schedule.Time_10_30_to_11_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_11_30_to_12_30) == Schedule.Time_11_30_to_12_30, Schedule.Time_11_30_to_12_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_12_30_to_13_30) == Schedule.Time_12_30_to_13_30, Schedule.Time_12_30_to_13_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_13_30_to_14_30) == Schedule.Time_13_30_to_14_30, Schedule.Time_13_30_to_14_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_14_30_to_15_30) == Schedule.Time_14_30_to_15_30, Schedule.Time_14_30_to_15_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_15_30_to_16_30) == Schedule.Time_15_30_to_16_30, Schedule.Time_15_30_to_16_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_16_30_to_17_30) == Schedule.Time_16_30_to_17_30, Schedule.Time_16_30_to_17_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_17_30_to_18_30) == Schedule.Time_17_30_to_18_30, Schedule.Time_17_30_to_18_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_18_30_to_19_30) == Schedule.Time_18_30_to_19_30, Schedule.Time_18_30_to_19_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_19_30_to_20_30) == Schedule.Time_19_30_to_20_30, Schedule.Time_19_30_to_20_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_20_30_to_21_30) == Schedule.Time_20_30_to_21_30, Schedule.Time_20_30_to_21_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_21_30_to_22_30) == Schedule.Time_21_30_to_22_30, Schedule.Time_21_30_to_22_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_22_30_to_23_30) == Schedule.Time_22_30_to_23_30, Schedule.Time_22_30_to_23_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.ApplicationSchedule & Schedule.Time_23_30_to_00_30) == Schedule.Time_23_30_to_00_30, Schedule.Time_23_30_to_00_30.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}