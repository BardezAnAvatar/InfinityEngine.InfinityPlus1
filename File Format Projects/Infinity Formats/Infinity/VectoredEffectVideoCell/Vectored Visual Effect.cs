using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell
{
    /// <summary>Represents vectored effects for displaying an animation</summary>
    /// <remarks>I am dumb and do not understand much about this format</remarks>
    public class VectoredVisualEffect : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x01EC;
        #endregion


        #region Fields
        /// <summary>Resource played by this visual effect</summary>
        public ResourceReference Animation { get; set; }

        /// <summary>Resource played by this visual effect for the shadow</summary>
        public ResourceReference ShadowAnimation { get; set; }

        /// <summary>Various display flags</summary>
        public DisplayFlags Display { get; set; }

        /// <summary>Various coloring flags</summary>
        public ColorFlags Color { get; set; }
        
        /// <summary>Unknown 4 bytes at offset 0x1C</summary>
        public Int32 Unknown_0x001C { get; set; }

        /// <summary>Various playback flags</summary>
        public SequenceFlags Sequence { get; set; }
        
        /// <summary>Unknown 4 bytes at offset 0x24</summary>
        public Int32 Unknown_0x0024 { get; set; }

        /// <summary>X position</summary>
        /// <value>0 is smallest</value>
        public Int32 CoordinateX { get; set; }

        /// <summary>Y position</summary>
        /// <value>0xFF is smallest; 0 is highest</value>
        public Int32 CoordinateY { get; set; }

        /// <summary>Flag indicating whether to use orientation</summary>
        public Int32 UseOrientation { get; set; }

        /// <summary>Frame rate of the animation</summary>
        /// <value>1 = slow</value>
        public Int32 FrameRate { get; set; }

        /// <summary>Count of orientations</summary>
        /// <value>0; 1; 8; 16 are valid</value>
        public Int32 CountOrientations { get; set; }

        /// <summary>Base orientation</summary>
        public Int32 BaseOrientation { get; set; }

        /// <summary>Various position flags</summary>
        public PositionFlags Position { get; set; }

        /// <summary>Referenced bitmap to use as a palette</summary>
        public ResourceReference Palette { get; set; }
        
        /// <summary>Z position</summary>
        public Int32 CoordinateZ { get; set; }

        /// <summary>Center X position</summary>
        /// <remarks>Used for lighting</remarks>
        public Int32 CenterX { get; set; }

        /// <summary>Center Y position</summary>
        public Int32 CenterY { get; set; }

        /// <summary>Lighting effect brightness</summary>
        public Int32 Brightness { get; set; }

        /// <summary>Duration in frames</summary>
        public Int32 CountFrames { get; set; }

        /// <summary>Internal name of this VVC</summary>
        public ZString Name { get; set; }

        /// <summary>First animation cycle</summary>
        public Int32 IndexAnimationIntroduction { get; set; }

        /// <summary>Looping animation cycle</summary>
        public Int32 IndexAnimationLoop { get; set; }

        /// <summary>Current animation cycle</summary>
        public Int32 IndexCurrentAnimation { get; set; }

        /// <summary>?</summary>
        public Int32 ContinuousSequencesBoolean { get; set; }

        /// <summary>Introduction sound</summary>
        public ResourceReference SoundIntroduction { get; set; }

        /// <summary>Looping sound</summary>
        public ResourceReference SoundLoop { get; set; }

        /// <summary>Alpha blending animation</summary>
        public ResourceReference Alpha { get; set; }

        /// <summary>Ending animation cycle</summary>
        public Int32 IndexAnimationClose { get; set; }

        /// <summary>Looping sound</summary>
        public ResourceReference SoundClose { get; set; }

        /// <summary>336 bytes of padding at offset 0x9C</summary>
        public Byte[] Padding_0x009C { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Animation = new ResourceReference(ResourceType.Bam);
            this.ShadowAnimation = new ResourceReference(ResourceType.Bam);
            this.Palette = new ResourceReference(ResourceType.ImageBitmap);
            this.Name = new ZString();
            this.SoundIntroduction = new ResourceReference();
            this.SoundLoop = new ResourceReference();
            this.Alpha = new ResourceReference();
            this.SoundClose = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 148);

            this.Animation.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.ShadowAnimation.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish);
            this.Display = (DisplayFlags)ReusableIO.ReadUInt16FromArray(buffer, 16);
            this.Color = (ColorFlags)ReusableIO.ReadUInt16FromArray(buffer, 18);
            this.Unknown_0x001C = ReusableIO.ReadInt32FromArray(buffer, 20);
            this.Sequence = (SequenceFlags)ReusableIO.ReadUInt32FromArray(buffer, 24);
            this.Unknown_0x0024 = ReusableIO.ReadInt32FromArray(buffer, 28);
            this.CoordinateX = ReusableIO.ReadInt32FromArray(buffer, 32);
            this.CoordinateY = ReusableIO.ReadInt32FromArray(buffer, 36);
            this.UseOrientation = ReusableIO.ReadInt32FromArray(buffer, 40);
            this.FrameRate = ReusableIO.ReadInt32FromArray(buffer, 44);
            this.CountOrientations = ReusableIO.ReadInt32FromArray(buffer, 48);
            this.BaseOrientation = ReusableIO.ReadInt32FromArray(buffer, 52);
            this.Position = (PositionFlags)ReusableIO.ReadUInt32FromArray(buffer, 56);
            this.Palette.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 60, CultureConstants.CultureCodeEnglish);
            this.CoordinateZ = ReusableIO.ReadInt32FromArray(buffer, 68);
            this.CenterX = ReusableIO.ReadInt32FromArray(buffer, 72);
            this.CenterY = ReusableIO.ReadInt32FromArray(buffer, 76);
            this.Brightness = ReusableIO.ReadInt32FromArray(buffer, 80);
            this.CountFrames = ReusableIO.ReadInt32FromArray(buffer, 84);
            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 88, CultureConstants.CultureCodeEnglish);
            this.IndexAnimationIntroduction = ReusableIO.ReadInt32FromArray(buffer, 96);
            this.IndexAnimationLoop = ReusableIO.ReadInt32FromArray(buffer, 100);
            this.IndexCurrentAnimation = ReusableIO.ReadInt32FromArray(buffer, 104);
            this.ContinuousSequencesBoolean = ReusableIO.ReadInt32FromArray(buffer, 108);
            this.SoundIntroduction.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 112, CultureConstants.CultureCodeEnglish);
            this.SoundLoop.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 120, CultureConstants.CultureCodeEnglish);
            this.Alpha.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 128, CultureConstants.CultureCodeEnglish);
            this.IndexAnimationClose = ReusableIO.ReadInt32FromArray(buffer, 136);
            this.SoundClose.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 140, CultureConstants.CultureCodeEnglish);

            this.Padding_0x009C = ReusableIO.BinaryRead(input, 336);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteStringToStream(this.Animation.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ShadowAnimation.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Display, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Color, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x001C, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Sequence, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0024, output);
            ReusableIO.WriteInt32ToStream(this.CoordinateX, output);
            ReusableIO.WriteInt32ToStream(this.CoordinateY, output);
            ReusableIO.WriteInt32ToStream(this.UseOrientation, output);
            ReusableIO.WriteInt32ToStream(this.FrameRate, output);
            ReusableIO.WriteInt32ToStream(this.CountOrientations, output);
            ReusableIO.WriteInt32ToStream(this.BaseOrientation, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Position, output);
            ReusableIO.WriteStringToStream(this.Palette.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.CoordinateZ, output);
            ReusableIO.WriteInt32ToStream(this.CenterX, output);
            ReusableIO.WriteInt32ToStream(this.CenterY, output);
            ReusableIO.WriteInt32ToStream(this.Brightness, output);
            ReusableIO.WriteInt32ToStream(this.CountFrames, output);
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.IndexAnimationIntroduction, output);
            ReusableIO.WriteInt32ToStream(this.IndexAnimationLoop, output);
            ReusableIO.WriteInt32ToStream(this.IndexCurrentAnimation, output);
            ReusableIO.WriteInt32ToStream(this.ContinuousSequencesBoolean, output);
            ReusableIO.WriteStringToStream(this.SoundIntroduction.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.SoundLoop.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.Alpha.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.IndexAnimationClose, output);
            ReusableIO.WriteStringToStream(this.SoundClose.ResRef, output, CultureConstants.CultureCodeEnglish);

            output.Write(this.Padding_0x009C, 0, 336);
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
            return StringFormat.ReturnAndIndent(String.Format("Vectored Effect Video Cell # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Vectored Effect Video Cell:";
        }
        
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Signature)));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Version)));
            builder.Append(StringFormat.ToStringAlignment("Animation"));
            builder.Append(String.Format("'{0}'", this.Animation.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Shadow Animation"));
            builder.Append(String.Format("'{0}'", this.ShadowAnimation.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Display flags (value)"));
            builder.Append((UInt32)this.Display);
            builder.Append(StringFormat.ToStringAlignment("Display flags (enumeration)"));
            builder.Append(this.GeneratedDisplayFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Color flags (value)"));
            builder.Append((UInt32)this.Color);
            builder.Append(StringFormat.ToStringAlignment("Color flags (enumeration)"));
            builder.Append(this.GenerateColorFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Unknown data @ offset 0x1C"));
            builder.Append(this.Unknown_0x001C);
            builder.Append(StringFormat.ToStringAlignment("Sequence flags (value)"));
            builder.Append((UInt32)this.Sequence);
            builder.Append(StringFormat.ToStringAlignment("Sequence flags (enumeration)"));
            builder.Append(this.GenerateSequenceFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Unknown data @ offset 0x24"));
            builder.Append(this.Unknown_0x0024);
            builder.Append(StringFormat.ToStringAlignment("X-coordinate"));
            builder.Append(this.CoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Y-coordinate"));
            builder.Append(this.CoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Use Orientation (value)"));
            builder.Append(this.UseOrientation);
            builder.Append(StringFormat.ToStringAlignment("Use Orientation (Boolean)"));
            builder.Append(Convert.ToBoolean(this.UseOrientation));
            builder.Append(StringFormat.ToStringAlignment("Frame rate"));
            builder.Append(this.FrameRate);
            builder.Append(StringFormat.ToStringAlignment("Orientation count"));
            builder.Append(this.CountOrientations);
            builder.Append(StringFormat.ToStringAlignment("Orientation base"));
            builder.Append(this.BaseOrientation);
            builder.Append(StringFormat.ToStringAlignment("Position flags (value)"));
            builder.Append((UInt32)this.Position);
            builder.Append(StringFormat.ToStringAlignment("Position flags (enumeration)"));
            builder.Append(this.GeneratePositionFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Palette"));
            builder.Append(String.Format("'{0}'", this.Palette.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Z-coordinate"));
            builder.Append(this.CoordinateZ);
            builder.Append(StringFormat.ToStringAlignment("Center X-coordinate"));
            builder.Append(this.CenterX);
            builder.Append(StringFormat.ToStringAlignment("Center Y-coordinate"));
            builder.Append(this.CenterY);
            builder.Append(StringFormat.ToStringAlignment("Brightness"));
            builder.Append(this.Brightness);
            builder.Append(StringFormat.ToStringAlignment("Frame count"));
            builder.Append(this.CountFrames);
            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("BAM introduction animation index"));
            builder.Append(this.IndexAnimationIntroduction);
            builder.Append(StringFormat.ToStringAlignment("BAM looping animation index"));
            builder.Append(this.IndexAnimationLoop);
            builder.Append(StringFormat.ToStringAlignment("BAM current animation index"));
            builder.Append(this.IndexCurrentAnimation);
            builder.Append(StringFormat.ToStringAlignment("Continuous sequences (value)"));
            builder.Append(this.ContinuousSequencesBoolean);
            builder.Append(StringFormat.ToStringAlignment("Continuous sequences (Boolean)"));
            builder.Append(Convert.ToBoolean(this.ContinuousSequencesBoolean));
            builder.Append(StringFormat.ToStringAlignment("Introduction sound"));
            builder.Append(String.Format("'{0}'", this.SoundIntroduction.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Looping sound"));
            builder.Append(String.Format("'{0}'", this.SoundLoop.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Alpha blending animation"));
            builder.Append(String.Format("'{0}'", this.Alpha.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("BAM closing animation index"));
            builder.Append(this.IndexAnimationClose);
            builder.Append(StringFormat.ToStringAlignment("Closing sound"));
            builder.Append(String.Format("'{0}'", this.SoundClose.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x9C"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x009C));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable enumeration of flags set for Display</summary>
        /// <returns>A human-readable enumeration of flags set for Display</returns>
        protected String GeneratedDisplayFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Transparent) == DisplayFlags.Transparent, DisplayFlags.Transparent.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Translucent) == DisplayFlags.Translucent, DisplayFlags.Translucent.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.TranslucentShadow) == DisplayFlags.TranslucentShadow, DisplayFlags.TranslucentShadow.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Blended) == DisplayFlags.Blended, DisplayFlags.Blended.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.MirrorAxisX) == DisplayFlags.MirrorAxisX, DisplayFlags.MirrorAxisX.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.MirrorAxisY) == DisplayFlags.MirrorAxisY, DisplayFlags.MirrorAxisY.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Clipped) == DisplayFlags.Clipped, DisplayFlags.Clipped.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.CopyFromBack) == DisplayFlags.CopyFromBack, DisplayFlags.CopyFromBack.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.ClearFill) == DisplayFlags.ClearFill, DisplayFlags.ClearFill.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Blend3D) == DisplayFlags.Blend3D, DisplayFlags.Blend3D.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.RenderAboveWall) == DisplayFlags.RenderAboveWall, DisplayFlags.RenderAboveWall.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.IgnoreDreamTimeStopPalette) == DisplayFlags.IgnoreDreamTimeStopPalette, DisplayFlags.IgnoreDreamTimeStopPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.IgnoreDreamPalette) == DisplayFlags.IgnoreDreamPalette, DisplayFlags.IgnoreDreamPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Display & DisplayFlags.Blend2D) == DisplayFlags.Blend2D, DisplayFlags.Blend2D.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable enumeration of flags set for Color</summary>
        /// <returns>A human-readable enumeration of flags set for Color</returns>
        protected String GenerateColorFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.LightSource) == ColorFlags.LightSource, ColorFlags.LightSource.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.InternalBrightness) == ColorFlags.InternalBrightness, ColorFlags.InternalBrightness.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.TimeStopped) == ColorFlags.TimeStopped, ColorFlags.TimeStopped.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.InternalGamma) == ColorFlags.InternalGamma, ColorFlags.InternalGamma.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.NonReservedPalette) == ColorFlags.NonReservedPalette, ColorFlags.NonReservedPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.FullPalette) == ColorFlags.FullPalette, ColorFlags.FullPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Color & ColorFlags.Sepia) == ColorFlags.Sepia, ColorFlags.Sepia.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable enumeration of flags set for Sequence</summary>
        /// <returns>A human-readable enumeration of flags set for Sequence</returns>
        protected String GenerateSequenceFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.Looping) == SequenceFlags.Looping, SequenceFlags.Looping.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.SpecialLighting) == SequenceFlags.SpecialLighting, SequenceFlags.SpecialLighting.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.Height) == SequenceFlags.Height, SequenceFlags.Height.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.DrawAnimation) == SequenceFlags.DrawAnimation, SequenceFlags.DrawAnimation.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.CustomPalette) == SequenceFlags.CustomPalette, SequenceFlags.CustomPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.Purgeable) == SequenceFlags.Purgeable, SequenceFlags.Purgeable.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.NotCoveredByWallgroups) == SequenceFlags.NotCoveredByWallgroups, SequenceFlags.NotCoveredByWallgroups.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.BrightenLevelMid) == SequenceFlags.BrightenLevelMid, SequenceFlags.BrightenLevelMid.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Sequence & SequenceFlags.BrightenLevelHigh) == SequenceFlags.BrightenLevelHigh, SequenceFlags.BrightenLevelHigh.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable enumeration of flags set for Position</summary>
        /// <returns>A human-readable enumeration of flags set for Position</returns>
        protected String GeneratePositionFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Position & PositionFlags.Orbit) == PositionFlags.Orbit, PositionFlags.Orbit.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Position & PositionFlags.Relative) == PositionFlags.Relative, PositionFlags.Relative.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Position & PositionFlags.IgnoreOrientation) == PositionFlags.IgnoreOrientation, PositionFlags.IgnoreOrientation.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}