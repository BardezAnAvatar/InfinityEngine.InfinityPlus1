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
    /// <summary>BAM animation of the Projectile class</summary>
    public class ProjectileAnimation : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x0100;
        #endregion


        #region Fields
        /// <summary>Various flags for the travelling projectile</summary>
        public TravelFlags TravellingFlags { get; set; }

        /// <summary>BAM resource for the projectile animation</summary>
        public ResourceReference Projectile { get; set; }

        /// <summary>BAM resource for the projectile shadow</summary>
        public ResourceReference Shadow { get; set; }

        /// <summary>Index of the animation for the projectile</summary>
        public Byte AnimationIndexProjectile { get; set; }

        /// <summary>Index of the animation for the shadow</summary>
        public Byte AnimationIndexShadow { get; set; }

        /// <summary>Intensity of the lighting (z-height, essentially?)</summary>
        public Int16 LightingZ { get; set; }

        /// <summary>Width of the lighting (width, essentially?)</summary>
        public Int16 LightingX { get; set; }

        /// <summary>Height of the lighting (y-height, essentially?)</summary>
        public Int16 LightingY { get; set; }

        /// <summary>BMP resource for the projectile's palette</summary>
        public ResourceReference Palette { get; set; }

        /// <summary>Color gradient palette indeces for the projectile</summary>
        public Byte[] GradientPaletteIndecesProjectile { get; set; }

        /// <summary>Length of smoke puff</summary>
        /// <remarks>... I think this is the 'smoke' puff for things like summoning monsters, etc.</remarks>
        public Byte SmokeSpeed { get; set; }

        /// <summary>Color gradient palette indeces for smoke</summary>
        public Byte[] GradientPaletteIndecesSmoke { get; set; }

        /// <summary>Not sure; GemRB suggests cycles or orientations. IESDP suggests mirroring, etc.</summary>
        public Byte Aim { get; set; }

        /// <summary>Animation of the smoke poof</summary>
        /// <remarks>Matches to Animate.IDS</remarks>
        public Int16 SmokeAnimation { get; set; }

        /// <summary>Rresource for the projectile's Trailing animation # 1</summary>
        public ResourceReference AnimationTrailing1 { get; set; }

        /// <summary>Rresource for the projectile's Trailing animation # 2</summary>
        public ResourceReference AnimationTrailing2 { get; set; }

        /// <summary>Rresource for the projectile's Trailing animation # 3</summary>
        public ResourceReference AnimationTrailing3 { get; set; }

        /// <summary>Animation index of trailing animation # 1</summary>
        public Int16 AnimationTrailingAnimationIndex1 { get; set; }

        /// <summary>Animation index of trailing animation # 2</summary>
        public Int16 AnimationTrailingAnimationIndex2 { get; set; }

        /// <summary>Animation index of trailing animation # 3</summary>
        public Int16 AnimationTrailingAnimationIndex3 { get; set; }

        /// <summary>172 Bytes of padding at offset 0x54</summary>
        public Byte[] Padding_0x0054 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Projectile = new ResourceReference();
            this.Shadow = new ResourceReference();
            this.Palette = new ResourceReference();
            this.GradientPaletteIndecesProjectile = new Byte[7];
            this.GradientPaletteIndecesSmoke = new Byte[7];
            this.AnimationTrailing1 = new ResourceReference();
            this.AnimationTrailing2 = new ResourceReference();
            this.AnimationTrailing3 = new ResourceReference();
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
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 84);

            this.TravellingFlags = (TravelFlags)ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.Projectile.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish);
            this.Shadow.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 12, CultureConstants.CultureCodeEnglish);
            this.AnimationIndexProjectile = buffer[20];
            this.AnimationIndexShadow = buffer[21];
            this.LightingZ = ReusableIO.ReadInt16FromArray(buffer, 22);
            this.LightingX = ReusableIO.ReadInt16FromArray(buffer, 24);
            this.LightingY = ReusableIO.ReadInt16FromArray(buffer, 26);
            this.Palette.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 28, CultureConstants.CultureCodeEnglish);
            Array.Copy(buffer, 36, this.GradientPaletteIndecesProjectile, 0, 7);
            this.SmokeSpeed = buffer[43];
            Array.Copy(buffer, 44, this.GradientPaletteIndecesSmoke, 0, 7);
            this.Aim = buffer[51];
            this.SmokeAnimation = ReusableIO.ReadInt16FromArray(buffer, 52);
            this.AnimationTrailing1.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 54, CultureConstants.CultureCodeEnglish);
            this.AnimationTrailing2.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 62, CultureConstants.CultureCodeEnglish);
            this.AnimationTrailing3.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 70, CultureConstants.CultureCodeEnglish);
            this.AnimationTrailingAnimationIndex1 = ReusableIO.ReadInt16FromArray(buffer, 78);
            this.AnimationTrailingAnimationIndex2 = ReusableIO.ReadInt16FromArray(buffer, 80);
            this.AnimationTrailingAnimationIndex3 = ReusableIO.ReadInt16FromArray(buffer, 82);

            this.Padding_0x0054 = ReusableIO.BinaryRead(input, 172);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.TravellingFlags, output);
            ReusableIO.WriteStringToStream(this.Projectile.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.Shadow.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte(this.AnimationIndexProjectile);
            output.WriteByte(this.AnimationIndexShadow);
            ReusableIO.WriteInt16ToStream(this.LightingZ, output);
            ReusableIO.WriteInt16ToStream(this.LightingX, output);
            ReusableIO.WriteInt16ToStream(this.LightingY, output);
            ReusableIO.WriteStringToStream(this.Palette.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.Write(this.GradientPaletteIndecesProjectile, 0, 7);
            output.WriteByte(this.SmokeSpeed);
            output.Write(this.GradientPaletteIndecesSmoke, 0, 7);
            output.WriteByte(this.Aim);
            ReusableIO.WriteInt16ToStream(this.SmokeAnimation, output);
            ReusableIO.WriteStringToStream(this.AnimationTrailing1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.AnimationTrailing2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.AnimationTrailing3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt16ToStream(this.AnimationTrailingAnimationIndex1, output);
            ReusableIO.WriteInt16ToStream(this.AnimationTrailingAnimationIndex2, output);
            ReusableIO.WriteInt16ToStream(this.AnimationTrailingAnimationIndex3, output);
            output.Write(this.Padding_0x0054, 0, 172);
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
            return "Projectile animation:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Travel flags (value)"));
            builder.Append((UInt16)this.TravellingFlags);
            builder.Append(StringFormat.ToStringAlignment("Travel flags (enumeration)"));
            builder.Append(this.GetTravelFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Projectile animation"));
            builder.Append(String.Format("'{0}'", this.Projectile.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Shadow animation"));
            builder.Append(String.Format("'{0}'", this.Shadow.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Projectile animation index"));
            builder.Append(this.AnimationIndexProjectile);
            builder.Append(StringFormat.ToStringAlignment("Shadow animation index"));
            builder.Append(this.AnimationIndexShadow);
            builder.Append(StringFormat.ToStringAlignment("Lighting Z coordinate"));
            builder.Append(this.LightingZ);
            builder.Append(StringFormat.ToStringAlignment("Lighting X coordinate"));
            builder.Append(this.LightingX);
            builder.Append(StringFormat.ToStringAlignment("Lighting Y coordinate"));
            builder.Append(this.LightingY);
            builder.Append(StringFormat.ToStringAlignment("Palette"));
            builder.Append(String.Format("'{0}'", this.Palette.ZResRef));

            for (Int32 index = 0; index < 7; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Projectile palette gradient # {0}", index)));
                builder.Append(this.GradientPaletteIndecesProjectile[index]);
            }

            builder.Append(StringFormat.ToStringAlignment("Smoke puff speed"));
            builder.Append(this.SmokeSpeed);

            for (Int32 index = 0; index < 7; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Smoke palette gradient # {0}", index)));
                builder.Append(this.GradientPaletteIndecesSmoke[index]);
            }

            builder.Append(StringFormat.ToStringAlignment("Aim (?)"));
            builder.Append(this.Aim);
            builder.Append(StringFormat.ToStringAlignment("Smoke animation # (animate.ids)"));
            builder.Append(this.SmokeAnimation);
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 1"));
            builder.Append(String.Format("'{0}'", this.AnimationTrailing1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 2"));
            builder.Append(String.Format("'{0}'", this.AnimationTrailing2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 3"));
            builder.Append(String.Format("'{0}'", this.AnimationTrailing3.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 1 animation index"));
            builder.Append(this.AnimationTrailingAnimationIndex1);
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 2 animation index"));
            builder.Append(this.AnimationTrailingAnimationIndex2);
            builder.Append(StringFormat.ToStringAlignment("Trailing animation # 3 animation index"));
            builder.Append(this.AnimationTrailingAnimationIndex3);
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x54"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0054));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetTravelFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.UseSpecifiedPalette) == TravelFlags.UseSpecifiedPalette, TravelFlags.UseSpecifiedPalette.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.Smokes) == TravelFlags.Smokes, TravelFlags.Smokes.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.UseTint) == TravelFlags.UseTint, TravelFlags.UseTint.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.UseHeightMap) == TravelFlags.UseHeightMap, TravelFlags.UseHeightMap.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.HasShadow) == TravelFlags.HasShadow, TravelFlags.HasShadow.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.HasLightShadow) == TravelFlags.HasLightShadow, TravelFlags.HasLightShadow.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.Blend) == TravelFlags.Blend, TravelFlags.Blend.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.BrightenLevelLow) == TravelFlags.BrightenLevelLow, TravelFlags.BrightenLevelLow.GetDescription());
            StringFormat.AppendSubItem(builder, (this.TravellingFlags & TravelFlags.BrightenLevelHigh) == TravelFlags.BrightenLevelHigh, TravelFlags.BrightenLevelHigh.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}