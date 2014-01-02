using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell2
{
    /// <summary>Icewind Dale 2 Spell header</summary>
    public class Spell2Header : SpellHeader
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 130;
        #endregion


        #region Fields
        /// <summary>Flags associated with this spell</summary>
        public SpellHeader2Flags Flags { get; set; }

        /// <summary>The overlay duration will be set to this * level + durationModifierRoundsFlat rounds.</summary>
        public Byte DurationModifierRoundsPerLevel { get; set; }

        /// <summary>The overlay duration will be set to durationModifierRoundsPerLevel * level + this rounds.</summary>
        public Byte DurationModifierRoundsFlat { get; set; }

        /// <summary>14 bytes at the trail of the header</summary>
        public Byte[] Reserved { get; set; }
        #endregion


        #region Cobstructors(s)
        /// <summary>Default constructor</summary>
        public Spell2Header()
        {
            this.CastingCompletionSound = null;
            this.DescriptionIdentified = null;
            this.DescriptionUnidentified = null;
            this.Icon = null;
            this.NameIdentified = null;
            this.NameUnidentified = null;
            this.Reserved = null;
            this.signature = null;
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.CastingCompletionSound = new ResourceReference();
            this.DescriptionIdentified = new StringReference();
            this.DescriptionUnidentified = new StringReference();
            this.Icon = new ResourceReference();
            this.NameIdentified = new StringReference();
            this.NameUnidentified = new StringReference();
            this.Reserved = new Byte[14];
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 122);

            this.NameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.NameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.CastingCompletionSound.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, CultureConstants.CultureCodeEnglish);
            this.Flags = (SpellHeader2Flags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.Type = (SpellType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.ProhibitionFlags = (SpellProhibitionFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
            this.CastingGraphics = (SpellCastingGraphics)ReusableIO.ReadUInt16FromArray(remainingBody, 26);

            this.MinimumLevel = remainingBody[28];
            this.School = (SpellSchool)remainingBody[29];
            this.MinimumStrength = remainingBody[30];
            this.Nature = (SpellNature)remainingBody[31];
            this.MinimumStrengthBonus = remainingBody[32];
            this.KitProhibitions1 = remainingBody[33];
            this.MinimumIntelligence = remainingBody[34];
            this.KitProhibitions2 = remainingBody[35];
            this.MinimumDexterity = remainingBody[36];
            this.KitProhibitions3 = remainingBody[37];
            this.MinimumWisdom = remainingBody[38];
            this.KitProhibitions4 = remainingBody[39];
            this.MinimumConstitution = ReusableIO.ReadUInt16FromArray(remainingBody, 40);
            this.MinimumCharisma = ReusableIO.ReadUInt16FromArray(remainingBody, 42);
            this.Level = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
            this.StackSize = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.Icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 50, CultureConstants.CultureCodeEnglish);
            this.IdentifyThreshold = ReusableIO.ReadUInt16FromArray(remainingBody, 58);
            this.GroundIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 60, CultureConstants.CultureCodeEnglish);
            this.Weight = ReusableIO.ReadUInt32FromArray(remainingBody, 68);
            this.DescriptionUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 72);
            this.DescriptionIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 76);
            this.DescriptionIcon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 80, CultureConstants.CultureCodeEnglish);
            this.Enchantment = ReusableIO.ReadUInt32FromArray(remainingBody, 88);
            this.OffsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.CountAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.OffsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.IndexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.CountEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
            this.DurationModifierRoundsPerLevel = remainingBody[106];
            this.DurationModifierRoundsFlat = remainingBody[107];
            Array.Copy(remainingBody, 108, this.Reserved, 0, 14);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.NameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.NameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.CastingCompletionSound.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Type, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.ProhibitionFlags, output);
            ReusableIO.WriteInt16ToStream((Int16)this.CastingGraphics, output);
            output.WriteByte(this.MinimumLevel);
            output.WriteByte((Byte)this.School);
            output.WriteByte(this.MinimumStrength);
            output.WriteByte((Byte)this.Nature);
            output.WriteByte(this.MinimumStrengthBonus);
            output.WriteByte(this.KitProhibitions1);
            output.WriteByte(this.MinimumIntelligence);
            output.WriteByte(this.KitProhibitions2);
            output.WriteByte(this.MinimumDexterity);
            output.WriteByte(this.KitProhibitions3);
            output.WriteByte(this.MinimumWisdom);
            output.WriteByte(this.KitProhibitions4);
            ReusableIO.WriteUInt16ToStream(this.MinimumConstitution, output);
            ReusableIO.WriteUInt16ToStream(this.MinimumCharisma, output);
            ReusableIO.WriteUInt32ToStream(this.Level, output);
            ReusableIO.WriteUInt16ToStream(this.StackSize, output);
            ReusableIO.WriteStringToStream(this.Icon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.IdentifyThreshold, output);
            ReusableIO.WriteStringToStream(this.GroundIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.Weight, output);
            ReusableIO.WriteInt32ToStream(this.DescriptionUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.DescriptionIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.DescriptionIcon.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.Enchantment, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.CountAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.IndexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.CountEquippedEffects, output);
            output.WriteByte(this.DurationModifierRoundsPerLevel);
            output.WriteByte(this.DurationModifierRoundsFlat);
            output.Write(this.Reserved, 0, 14);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("SPELL Version 2.0 Header:", 0));
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Unidentified Name StrRef"));
            builder.Append(this.NameUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Name StrRef"));
            builder.Append(this.NameIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Casting completed sound"));
            builder.Append(String.Format("'{0}'", this.CastingCompletionSound.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumerated)"));
            builder.Append(this.GetFlagString());
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append((UInt16)this.Type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.Type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Prohibitions"));
            builder.Append((UInt32)this.ProhibitionFlags);
            builder.Append(StringFormat.ToStringAlignment("Prohibitions (enumerated)"));
            builder.Append(this.GetProhibitionsString());
            builder.Append(StringFormat.ToStringAlignment("Casting graphics"));
            builder.Append((Int16)this.CastingGraphics);
            builder.Append(StringFormat.ToStringAlignment("Casting graphics (description)"));
            builder.Append(this.CastingGraphics.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Spell school"));
            builder.Append((UInt16)this.School);
            builder.Append(StringFormat.ToStringAlignment("Spell school (description)"));
            builder.Append(this.School.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Spell nature"));
            builder.Append((UInt16)this.Nature);
            builder.Append(StringFormat.ToStringAlignment("Spell nature (description)"));
            builder.Append(this.Nature.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength to use [unused field]"));
            builder.Append(this.MinimumStrength);
            builder.Append(StringFormat.ToStringAlignment("Minimum Strength Bonus to use [unused field]"));
            builder.Append(this.MinimumStrengthBonus);
            builder.Append(StringFormat.ToStringAlignment("Minimum Dexterity to use [unused field]"));
            builder.Append(this.MinimumDexterity);
            builder.Append(StringFormat.ToStringAlignment("Minimum Constitution to use [unused field]"));
            builder.Append(this.MinimumConstitution);
            builder.Append(StringFormat.ToStringAlignment("Minimum Intelligence to use [unused field]"));
            builder.Append(this.MinimumIntelligence);
            builder.Append(StringFormat.ToStringAlignment("Minimum Wisdom to use [unused field]"));
            builder.Append(this.MinimumWisdom);
            builder.Append(StringFormat.ToStringAlignment("Minimum Charisma to use [unused field]"));
            builder.Append(this.MinimumCharisma);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 1 [unused field]"));
            builder.Append(this.KitProhibitions1);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 2 [unused field]"));
            builder.Append(this.KitProhibitions2);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 3 [unused field]"));
            builder.Append(this.KitProhibitions3);
            builder.Append(StringFormat.ToStringAlignment("Kit prohibition flags 4 [unused field]"));
            builder.Append(this.KitProhibitions4);
            builder.Append(StringFormat.ToStringAlignment("Spell level"));
            builder.Append(this.Level);
            builder.Append(StringFormat.ToStringAlignment("Stack size"));
            builder.Append(this.StackSize);
            builder.Append(StringFormat.ToStringAlignment("Icon"));
            builder.Append(String.Format("'{0}'", this.Icon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Identify Threshold"));
            builder.Append(this.IdentifyThreshold);
            builder.Append(StringFormat.ToStringAlignment("Ground image"));
            builder.Append(String.Format("'{0}'", this.GroundIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Weight (in pounds)"));
            builder.Append(this.Weight);
            builder.Append(StringFormat.ToStringAlignment("Unidentified Description StrRef"));
            builder.Append(this.DescriptionUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Description StrRef"));
            builder.Append(this.DescriptionIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Description Image"));
            builder.Append(String.Format("'{0}'", this.DescriptionIcon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Magical Enchantment"));
            builder.Append(this.Enchantment);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities"));
            builder.Append(this.OffsetAbilities);
            builder.Append(StringFormat.ToStringAlignment("Count of item Abilities"));
            builder.Append(this.CountAbilities);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities Effects"));
            builder.Append(this.OffsetAbilityEffects);
            builder.Append(StringFormat.ToStringAlignment("Index to item Equipped Effects"));
            builder.Append(this.IndexEquippedEffects);
            builder.Append(StringFormat.ToStringAlignment("Count of item Equipped Effects"));
            builder.Append(this.CountEquippedEffects);
            builder.Append(StringFormat.ToStringAlignment("Simplified duration rounds per level"));
            builder.Append(this.DurationModifierRoundsPerLevel);
            builder.Append(StringFormat.ToStringAlignment("Simplified duration rounds flat bonus"));
            builder.Append(this.DurationModifierRoundsFlat);
            builder.Append(StringFormat.ToStringAlignment("Reserved trailing data set"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Reserved));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFlagString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.OffensiveSpell) == SpellHeader2Flags.OffensiveSpell, SpellHeader2Flags.OffensiveSpell.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.NoLineOfSight) == SpellHeader2Flags.NoLineOfSight, SpellHeader2Flags.NoLineOfSight.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.OutdoorsOnly) == SpellHeader2Flags.OutdoorsOnly, SpellHeader2Flags.OutdoorsOnly.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.SimplifiedDuration) == SpellHeader2Flags.SimplifiedDuration, SpellHeader2Flags.SimplifiedDuration.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.TriggerContingency) == SpellHeader2Flags.TriggerContingency, SpellHeader2Flags.TriggerContingency.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & SpellHeader2Flags.OutOfCombatOnly) == SpellHeader2Flags.OutOfCombatOnly, SpellHeader2Flags.OutOfCombatOnly.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}