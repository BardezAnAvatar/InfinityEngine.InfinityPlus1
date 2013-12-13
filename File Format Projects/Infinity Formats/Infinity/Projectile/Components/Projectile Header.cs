using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Components
{
    /// <summary>Header of the Projectile class</summary>
    public class ProjectileHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x0100;
        #endregion


        #region Fields
        /// <summary>Type of the projectile</summary>
        public ProjectileType Type { get; set; }

        /// <summary>Speed at which the projectile travels</summary>
        public Int16 Speed { get; set; }

        /// <summary>Flags for any spark effects</summary>
        public SparklingFlags SparkFlags { get; set; }

        /// <summary>Sound to play for the travelling projectile</summary>
        public ResourceReference SoundTravelling { get; set; }

        /// <summary>Sound to play for the destination/explosion of the projectile</summary>
        public ResourceReference SoundDestination { get; set; }

        /// <summary>Animation to use for the travelling projectile</summary>
        public ResourceReference TravellingAnimation { get; set; }

        /// <summary>Color of any sparks</summary>
        public SparkColor SparksColor { get; set; }

        /// <summary>214 Bytes of unused padding at offset 0x002A</summary>
        public Byte[] Padding_0x002A { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.SoundTravelling = new ResourceReference();
            this.SoundDestination = new ResourceReference();
            this.TravellingAnimation = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 34);

            this.Type = (ProjectileType)ReusableIO.ReadUInt16FromArray(buffer, 0);
            this.Speed = ReusableIO.ReadInt16FromArray(buffer, 2);
            this.SparkFlags = (SparklingFlags)ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.SoundTravelling.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish);
            this.SoundDestination.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish);
            this.TravellingAnimation.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 24, CultureConstants.CultureCodeEnglish);
            this.SparksColor = (SparkColor)ReusableIO.ReadUInt16FromArray(buffer, 32);

            this.Padding_0x002A = ReusableIO.BinaryRead(input, 214);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output); //write signature and version
            
            ReusableIO.WriteUInt16ToStream((UInt16)this.Type, output);
            ReusableIO.WriteInt16ToStream(this.Speed, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.SparkFlags, output);
            ReusableIO.WriteStringToStream(this.SoundTravelling.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.SoundDestination.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.TravellingAnimation.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream((UInt16)this.SparksColor, output);
            output.Write(this.Padding_0x002A, 0, 214);
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Projectile header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Type (value)"));
            builder.Append((UInt16)this.Type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.Type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Travel Speed"));
            builder.Append(this.Speed);
            builder.Append(StringFormat.ToStringAlignment("Spark flags (value)"));
            builder.Append((UInt16)this.SparkFlags);
            builder.Append(StringFormat.ToStringAlignment("Spark flags (enumeration)"));
            builder.Append(this.GetSparkFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Travelling sound"));
            builder.Append(String.Format("'{0}'", this.SoundTravelling.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Destination sound"));
            builder.Append(String.Format("'{0}'", this.SoundDestination.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Travelling animation"));
            builder.Append(String.Format("'{0}'", this.TravellingAnimation.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Spark color (value)"));
            builder.Append((UInt16)this.SparksColor);
            builder.Append(StringFormat.ToStringAlignment("Spark color (description)"));
            builder.Append(this.SparksColor.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x2A"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x002A));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetSparkFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.Sparkles) == SparklingFlags.Sparkles, SparklingFlags.Sparkles.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.UseCoordinateZ) == SparklingFlags.UseCoordinateZ, SparklingFlags.UseCoordinateZ.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.LoopSoundTravelling) == SparklingFlags.LoopSoundTravelling, SparklingFlags.LoopSoundTravelling.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.LoopSoundDestination) == SparklingFlags.LoopSoundDestination, SparklingFlags.LoopSoundDestination.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.IgnoreCenter) == SparklingFlags.IgnoreCenter, SparklingFlags.IgnoreCenter.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SparkFlags & SparklingFlags.DrawBelowAnimateObjects) == SparklingFlags.DrawBelowAnimateObjects, SparklingFlags.DrawBelowAnimateObjects.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}