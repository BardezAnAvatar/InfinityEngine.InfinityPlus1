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

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell1
{
    public class Spell1Header : SpellHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 114;    //here for signature purposes

        #region Members & Properties
        protected SpellHeader1Flags flags;

        public SpellHeader1Flags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }
        #endregion

        #region Cobstructors(s)
        /// <summary>Default constructor</summary>
        public Spell1Header()
        {
            this.castingCompletionSound = null;
            this.descriptionIdentified = null;
            this.descriptionUnidentified = null;
            this.icon = null;
            this.nameIdentified = null;
            this.nameUnidentified = null;
            this.reserved1 = null;
            this.reserved2 = null;
            this.reserved3 = null;
            this.signature = null;
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.castingCompletionSound = new ResourceReference();
            this.descriptionIdentified = new StringReference();
            this.descriptionUnidentified = new StringReference();
            this.icon = new ResourceReference();
            this.nameIdentified = new StringReference();
            this.nameUnidentified = new StringReference();
            this.reserved1 = new Byte[12];
            this.reserved2 = new Byte[14];
            this.reserved3 = new Byte[12];
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 106);

            this.nameUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.castingCompletionSound.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 8, CultureConstants.CultureCodeEnglish);
            this.flags = (SpellHeader1Flags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.type = (SpellType)ReusableIO.ReadUInt16FromArray(remainingBody, 20);
            this.prohibitionFlags = (SpellProhibitionFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 22);
            this.castingGraphics = (SpellCastingGraphics)ReusableIO.ReadUInt16FromArray(remainingBody, 26);
            this.school = (SpellSchool)ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.nature = (SpellNature)ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            Array.Copy(remainingBody, 32, this.reserved1, 0, 12);
            this.level = ReusableIO.ReadUInt32FromArray(remainingBody, 44);
            this.stackSize = ReusableIO.ReadUInt16FromArray(remainingBody, 48);
            this.icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 50, CultureConstants.CultureCodeEnglish);
            Array.Copy(remainingBody, 58, this.reserved2, 0, 14);
            this.descriptionUnidentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 72);
            this.descriptionIdentified.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 76);
            Array.Copy(remainingBody, 80, this.reserved3, 0, 12);
            this.offsetAbilities = ReusableIO.ReadUInt32FromArray(remainingBody, 92);
            this.countAbilities = ReusableIO.ReadUInt16FromArray(remainingBody, 96);
            this.offsetAbilityEffects = ReusableIO.ReadUInt32FromArray(remainingBody, 98);
            this.indexEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 102);
            this.countEquippedEffects = ReusableIO.ReadUInt16FromArray(remainingBody, 104);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.nameUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.nameIdentified.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.castingCompletionSound.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.flags, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.type, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.prohibitionFlags, output);
            ReusableIO.WriteInt16ToStream((Int16)this.castingGraphics, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.school, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.nature, output);
            output.Write(this.reserved1, 0, 12);
            ReusableIO.WriteUInt32ToStream(this.level, output);
            ReusableIO.WriteUInt16ToStream(this.stackSize, output);
            ReusableIO.WriteStringToStream(this.icon.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.Write(this.reserved2, 0, 14);
            ReusableIO.WriteInt32ToStream(this.descriptionUnidentified.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.descriptionIdentified.StringReferenceIndex, output);
            output.Write(this.reserved3, 0, 12);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilities, output);
            ReusableIO.WriteUInt16ToStream(this.countAbilities, output);
            ReusableIO.WriteUInt32ToStream(this.offsetAbilityEffects, output);
            ReusableIO.WriteUInt16ToStream(this.indexEquippedEffects, output);
            ReusableIO.WriteUInt16ToStream(this.countEquippedEffects, output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("SPELL Version 1.0 Header:", 0));
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Unidentified Name StrRef"));
            builder.Append(this.nameUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Name StrRef"));
            builder.Append(this.nameIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Casting completed sound"));
            builder.Append(String.Format("'{0}'", this.castingCompletionSound.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append((UInt32)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumerated)"));
            builder.Append(this.GetFlagString());
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append((UInt16)this.type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Prohibitions"));
            builder.Append((UInt32)this.prohibitionFlags);
            builder.Append(StringFormat.ToStringAlignment("Prohibitions (enumerated)"));
            builder.Append(this.GetProhibitionsString());
            builder.Append(StringFormat.ToStringAlignment("Casting graphics"));
            builder.Append((Int16)this.castingGraphics);
            builder.Append(StringFormat.ToStringAlignment("Casting graphics (description)"));
            builder.Append(this.castingGraphics.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Spell school"));
            builder.Append((UInt16)this.school);
            builder.Append(StringFormat.ToStringAlignment("Spell school (description)"));
            builder.Append(this.school.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Spell nature"));
            builder.Append((UInt16)this.nature);
            builder.Append(StringFormat.ToStringAlignment("Spell nature (description)"));
            builder.Append(this.nature.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Reserved data set #1"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved1));
            builder.Append(StringFormat.ToStringAlignment("Spell level"));
            builder.Append(this.level);
            builder.Append(StringFormat.ToStringAlignment("Stack size"));
            builder.Append(this.stackSize);
            builder.Append(StringFormat.ToStringAlignment("Icon"));
            builder.Append(String.Format("'{0}'", this.icon.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Reserved data set #2"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved2));
            builder.Append(StringFormat.ToStringAlignment("Unidentified Description StrRef"));
            builder.Append(this.descriptionUnidentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Identified Description StrRef"));
            builder.Append(this.descriptionIdentified.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Reserved data set #3"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved3));
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities"));
            builder.Append(this.offsetAbilities);
            builder.Append(StringFormat.ToStringAlignment("Count of item Abilities"));
            builder.Append(this.countAbilities);
            builder.Append(StringFormat.ToStringAlignment("Offset to item Abilities Effects"));
            builder.Append(this.offsetAbilityEffects);
            builder.Append(StringFormat.ToStringAlignment("Index to item Equipped Effects"));
            builder.Append(this.indexEquippedEffects);
            builder.Append(StringFormat.ToStringAlignment("Count of item Equipped Effects"));
            builder.Append(this.countEquippedEffects);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which ItemFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFlagString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.OffensiveSpell) == SpellHeader1Flags.OffensiveSpell, SpellHeader1Flags.OffensiveSpell.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.NoLineOfSight) == SpellHeader1Flags.NoLineOfSight, SpellHeader1Flags.NoLineOfSight.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.OutdoorsOnly) == SpellHeader1Flags.OutdoorsOnly, SpellHeader1Flags.OutdoorsOnly.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.NonMagical) == SpellHeader1Flags.NonMagical, SpellHeader1Flags.NonMagical.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.TriggerContingency) == SpellHeader1Flags.TriggerContingency, SpellHeader1Flags.TriggerContingency.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & SpellHeader1Flags.OutOfCombatOnly) == SpellHeader1Flags.OutOfCombatOnly, SpellHeader1Flags.OutOfCombatOnly.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}