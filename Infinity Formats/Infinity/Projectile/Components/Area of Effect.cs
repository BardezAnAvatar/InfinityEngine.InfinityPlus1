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
    /// <summary>Projectile class' area effect</summary>
    public class AreaEffect : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x0100;
        #endregion


        #region Fields
        /// <summary>Various flags for the area of effect</summary>
        public AreaOfEffectFlags EffectFlags { get; set; }

        /// <summary>Radius of the trigger</summary>
        /// <remarks>Divide by roughly 8.5 to get 'feet'</remarks>
        public Int16 TriggerRadius { get; set; }

        /// <summary>Radius of the effect</summary>
        /// <remarks>Divide by roughly 8.5 to get 'feet'</remarks>
        public Int16 EffectRadius { get; set; }

        /// <summary>Sound to play when triggering the effect</summary>
        public ResourceReference SoundTrigger { get; set; }

        /// <summary>Delay of the triggered explosion</summary>
        public Int16 ExplosionDelay { get; set; }

        /// <summary>Animation ID of fragments</summary>
        /// <remarks>Matches to animate.ids</remarks>
        public Int16 FragmentAnimation { get; set; }

        /// <summary>Secondatry projectile ID</summary>
        /// <remarks>Matches to projectl.ids - 1</remarks>
        public Int16 ProjectileSecondary { get; set; }

        /// <summary>Count of triggers (if neither caster level bit in <see cref="EffectFlags"/> is set</summary>
        public Byte TriggerCount { get; set; }

        /// <summary>Animation of any explosion</summary>
        /// <remarks>Matches to fireball.ids</remarks>
        public Byte ExplosionAnimation { get; set; }

        /// <summary>Color gradient palette to apply to the explosion</summary>
        /// <remarks>Matches to the BMP palettes</remarks>
        public Byte ExplosionColor { get; set; }

        /// <summary>Unknown byte at offset 0x19</summary>
        public Byte Unknown_0x0019 { get; set; }

        /// <summary>Explosion projectile ID</summary>
        /// <remarks>Matches to projectl.ids; played when a creature is hit by this effect</remarks>
        public Int16 ProjectileExplosionHit { get; set; }

        /// <summary>Animation to play when a creature is hit my this explosion</summary>
        public ResourceReference AnimationExplosionHit { get; set; }

        /// <summary>Width of the effect cone (in degrees?)</summary>
        /// <value>1 - 359?</value>
        public Int16 ConeAngle { get; set; }

        /// <summary>218 bytes of padding at offset 0x26</summary>
        public Byte[] Padding_0x0026 { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.SoundTrigger = new ResourceReference();
            this.AnimationExplosionHit = new ResourceReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 38);

            this.EffectFlags = (AreaOfEffectFlags)ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.TriggerRadius = ReusableIO.ReadInt16FromArray(buffer, 4);
            this.EffectRadius = ReusableIO.ReadInt16FromArray(buffer, 6);
            this.SoundTrigger.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish);
            this.ExplosionDelay = ReusableIO.ReadInt16FromArray(buffer, 16);
            this.FragmentAnimation = ReusableIO.ReadInt16FromArray(buffer, 18);
            this.ProjectileSecondary = ReusableIO.ReadInt16FromArray(buffer, 20);
            this.TriggerCount = buffer[22];
            this.ExplosionAnimation = buffer[23];
            this.ExplosionColor = buffer[24];
            this.Unknown_0x0019 = buffer[25];
            this.ProjectileExplosionHit = ReusableIO.ReadInt16FromArray(buffer, 26);
            this.AnimationExplosionHit.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 28, CultureConstants.CultureCodeEnglish);
            this.ConeAngle = ReusableIO.ReadInt16FromArray(buffer, 36);
            this.Padding_0x0026 = ReusableIO.BinaryRead(input, 218);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.EffectFlags, output);
            ReusableIO.WriteInt16ToStream(this.TriggerRadius, output);
            ReusableIO.WriteInt16ToStream(this.EffectRadius, output);
            ReusableIO.WriteStringToStream(this.SoundTrigger.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt16ToStream(this.ExplosionDelay, output);
            ReusableIO.WriteInt16ToStream(this.FragmentAnimation, output);
            ReusableIO.WriteInt16ToStream(this.ProjectileSecondary, output);
            output.WriteByte(this.TriggerCount);
            output.WriteByte(this.ExplosionAnimation);
            output.WriteByte(this.ExplosionColor);
            output.WriteByte(this.Unknown_0x0019);
            ReusableIO.WriteInt16ToStream(this.ProjectileExplosionHit, output);
            ReusableIO.WriteStringToStream(this.AnimationExplosionHit.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt16ToStream(this.ConeAngle, output);
            output.Write(this.Padding_0x0026, 0, 218);
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
            return "Projectile area effect:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Area effect flags (value)"));
            builder.Append((UInt16)this.EffectFlags);
            builder.Append(StringFormat.ToStringAlignment("Area effect flags (enumeration)"));
            builder.Append(this.GetAreaEffectFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Trigger radius"));
            builder.Append(this.TriggerRadius);
            builder.Append(StringFormat.ToStringAlignment("Effect radius"));
            builder.Append(this.EffectRadius);
            builder.Append(StringFormat.ToStringAlignment("Trigger sound"));
            builder.Append(String.Format("'{0}'", this.SoundTrigger.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Explosion delay"));
            builder.Append(this.ExplosionDelay);
            builder.Append(StringFormat.ToStringAlignment("Fragment animation"));
            builder.Append(this.FragmentAnimation);
            builder.Append(StringFormat.ToStringAlignment("Secondary projectile"));
            builder.Append(this.ProjectileSecondary);
            builder.Append(StringFormat.ToStringAlignment("Trigger count"));
            builder.Append(this.TriggerCount);
            builder.Append(StringFormat.ToStringAlignment("Explosion animation"));
            builder.Append(this.ExplosionAnimation);
            builder.Append(StringFormat.ToStringAlignment("Explosion color"));
            builder.Append(this.ExplosionColor);
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x19"));
            builder.Append(this.Unknown_0x0019);
            builder.Append(StringFormat.ToStringAlignment("Explosion hit projectile/animation"));
            builder.Append(this.ProjectileExplosionHit);
            builder.Append(StringFormat.ToStringAlignment("Explosion hit animation"));
            builder.Append(String.Format("'{0}'", this.AnimationExplosionHit.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Cone angle"));
            builder.Append(this.ConeAngle);
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x26"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0026));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetAreaEffectFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.VisibleUntilDetonated) == AreaOfEffectFlags.VisibleUntilDetonated, AreaOfEffectFlags.VisibleUntilDetonated.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.AffectsInanimates) == AreaOfEffectFlags.AffectsInanimates, AreaOfEffectFlags.AffectsInanimates.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.NeedsTrigger) == AreaOfEffectFlags.NeedsTrigger, AreaOfEffectFlags.NeedsTrigger.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.SynchronizeExplosion) == AreaOfEffectFlags.SynchronizeExplosion, AreaOfEffectFlags.SynchronizeExplosion.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.HasSecondaryProjectiles) == AreaOfEffectFlags.HasSecondaryProjectiles, AreaOfEffectFlags.HasSecondaryProjectiles.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.HasFragments) == AreaOfEffectFlags.HasFragments, AreaOfEffectFlags.HasFragments.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.AffectsEnemies) == AreaOfEffectFlags.AffectsEnemies, AreaOfEffectFlags.AffectsEnemies.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.AffectsAllies) == AreaOfEffectFlags.AffectsAllies, AreaOfEffectFlags.AffectsAllies.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.AffectsAll) == AreaOfEffectFlags.AffectsAll, AreaOfEffectFlags.AffectsAll.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.TriggerCountLevelMage) == AreaOfEffectFlags.TriggerCountLevelMage, AreaOfEffectFlags.TriggerCountLevelMage.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.TriggerCountLevelCleric) == AreaOfEffectFlags.TriggerCountLevelCleric, AreaOfEffectFlags.TriggerCountLevelCleric.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.HasVvcAnimation) == AreaOfEffectFlags.HasVvcAnimation, AreaOfEffectFlags.HasVvcAnimation.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.HasConeShape) == AreaOfEffectFlags.HasConeShape, AreaOfEffectFlags.HasConeShape.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.Intangible) == AreaOfEffectFlags.Intangible, AreaOfEffectFlags.Intangible.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.DelayedTrigger) == AreaOfEffectFlags.DelayedTrigger, AreaOfEffectFlags.DelayedTrigger.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.DelayedExplosion) == AreaOfEffectFlags.DelayedExplosion, AreaOfEffectFlags.DelayedExplosion.GetDescription());
            StringFormat.AppendSubItem(builder, (this.EffectFlags & AreaOfEffectFlags.SingleTarget) == AreaOfEffectFlags.SingleTarget, AreaOfEffectFlags.SingleTarget.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}