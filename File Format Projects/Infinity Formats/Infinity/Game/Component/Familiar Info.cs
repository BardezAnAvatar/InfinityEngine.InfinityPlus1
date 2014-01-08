using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the Familiar info structure, which indicates which familiar you get based on alignment</summary>
    /// <remarks>It appears that each familiar goes up to level 9, so 9 values for each one.</remarks>
    public class FamiliarInfo : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        /// <remarks>IESPD reports an incorrect size.</remarks>
        public const Int32 StructSize = 400;

        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 LawfulGood = 0;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 LawfulNeutral = 1;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 LawfulEvil = 2;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 NeutralGood = 3;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 TrueNeutral = 4;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 NeutralEvil = 5;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 ChaoticGood = 6;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 ChaoticNeutral = 7;
        /// <summary>Index into Familiar ResRefs for this alignment</summary>
        public const Int32 ChaoticEvil = 8;
        #endregion


        #region Fields
        /// <summary>Array of familiars, one for each alignment</summary>
        public ResourceReference[] Familiars { get; set; }

        /// <summary>Offset to familiar ResRefs table</summary>
        public Int32 FamiliarResRefsOffset { get; set; }

        /// <summary>A two-dimensional array of familiar counts</summary>
        /// <remarks>It is unclear what this means</remarks>
        public UInt32[,] FamiliarCounts { get; set; }

        /// <summary>Vague list of ResRefs related to familiars</summary>
        public List<ResourceReference> FamiliarExtra { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the total count of familiar levels. Recomputed each reference.</summary>
        public UInt32 TotalCount
        {
            get
            {
                UInt32 sum = 0;

                for (Int32 alignment = 0; alignment < 9; ++alignment)
                    for (Int32 level = 0; level < 9; ++level)
                        sum += this.FamiliarCounts[alignment, level];

                return sum;
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Familiars = new ResourceReference[9];
            for (Int32 index = 0; index < 9; ++index)
                this.Familiars[index] = new ResourceReference(ResourceType.Creature);

            this.FamiliarCounts = new UInt32[9, 9];
            this.FamiliarExtra = new List<ResourceReference>();
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, FamiliarInfo.StructSize);

            for (Int32 index = 0; index < 9; ++index)
                this.Familiars[index].ResRef = ReusableIO.ReadStringFromByteArray(buffer, (index * 8), CultureConstants.CultureCodeEnglish);

            this.FamiliarResRefsOffset = ReusableIO.ReadInt32FromArray(buffer, 72);

            for (Int32 alignment = 0; alignment < 9; ++alignment)
                for (Int32 level = 0; level < 9; ++level)
                    this.FamiliarCounts[alignment, level] = ReusableIO.ReadUInt32FromArray(buffer, 76 + (alignment * 36) + (level * 4));


            //familiar extras
            if (this.FamiliarResRefsOffset > 0)
            {
                ReusableIO.SeekIfAble(input, this.FamiliarResRefsOffset);
                UInt32 count = this.TotalCount;
                buffer = ReusableIO.BinaryRead(input, (8 * count));
                for (Int64 index = 0; index < count; ++index)
                {
                    ResourceReference resref = new ResourceReference(ResourceType.Creature);
                    resref.ResRef = ReusableIO.ReadStringFromByteArray(buffer, Convert.ToInt32(8 * index), CultureConstants.CultureCodeEnglish);
                    this.FamiliarExtra.Add(resref);
                }
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            for (Int32 index = 0; index < 9; ++index)
                ReusableIO.WriteStringToStream(this.Familiars[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            ReusableIO.WriteInt32ToStream(this.FamiliarResRefsOffset, output);

            for (Int32 alignment = 0; alignment < 9; ++alignment)
                for (Int32 level = 0; level < 9; ++level)
                    ReusableIO.WriteUInt32ToStream(this.FamiliarCounts[alignment, level], output);

            //familiar extras
            if (this.FamiliarResRefsOffset > 0)
            {
                ReusableIO.SeekIfAble(output, this.FamiliarResRefsOffset);
                if (output.Length < this.FamiliarResRefsOffset)
                    output.SetLength(this.FamiliarResRefsOffset);
                foreach (ResourceReference resref in this.FamiliarExtra)
                    ReusableIO.WriteStringToStream(resref.ResRef, output, CultureConstants.CultureCodeEnglish);
            }
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            //Familiar alignment RESREFs
            builder.Append(StringFormat.ToStringAlignment("Lawful Good Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[0].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Lawful Neutral Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[1].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Lawful Evil Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[2].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Neutral Good Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[3].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("True Neutral Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[4].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Neutral Evil Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[5].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Chaotic Good Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[6].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Chaotic Neutral Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[7].ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Chaotic Evil Familiar"));
            builder.Append(String.Format("'{0}'", this.Familiars[8].ZResRef));

            //unknown
            builder.Append(StringFormat.ToStringAlignment("Offset to resource list of Familiars"));
            builder.Append(this.FamiliarResRefsOffset);

            //caster level familiar counts?
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Lawful Good Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[0, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Lawful Neutral Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[1, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Lawful Evil Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[2, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Neutral Good Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[3, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("True Neutral Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[4, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Neutral Evil Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[5, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Chaotic Good Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[6, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Chaotic Neutral Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[7, level]);
            }
            for (Int32 level = 0; level < 9; ++level)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Chaotic Evil Level {0} familiar count", level)));
                builder.Append(this.FamiliarCounts[8, level]);
            }
            
            return builder.ToString();
        }
        #endregion
    }
}