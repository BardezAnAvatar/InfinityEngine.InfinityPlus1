using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature1
{
    /// <summary>Creature header version 1</summary>
    public class Creature1Header : Creature2eHeader
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 724;
        #endregion


        #region Fields
        /// <summary>Large sword proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyLargeSword { get; set; }

        /// <summary>Small swords proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencySmallSword { get; set; }

        /// <summary>Spear proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencySpear { get; set; }

        /// <summary>Blunt proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyBlunt { get; set; }

        /// <summary>Spiked proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencySpiked { get; set; }

        /// <summary>Missile proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyMissile { get; set; }

        /// <summary>Unused proficiency #7</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused7 { get; set; }

        /// <summary>Unused proficiency #8</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused8 { get; set; }

        /// <summary>Unused proficiency #9</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused9 { get; set; }

        /// <summary>Unused proficiency #10</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused10 { get; set; }

        /// <summary>Unused proficiency #11</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused11 { get; set; }

        /// <summary>Unused proficiency #12</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused12 { get; set; }

        /// <summary>Unused proficiency #13</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte ProficiencyUnused13 { get; set; }
        #endregion
        

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Creature1Header()
        {
            this.Initialize();
        }

        /// <summary>Initializes the soundset ordered dictionary</summary>
        protected override void InitializeSoundSet()
        {
            /* 00 */ this.soundSet.Add("INITIAL_MEETING", new StringReference());
            /* 01 */ this.soundSet.Add("MORALE", new StringReference());
            /* 02 */ this.soundSet.Add("HAPPY", new StringReference());
            /* 03 */ this.soundSet.Add("UNHAPPY_ANNOYED", new StringReference());
            /* 04 */ this.soundSet.Add("UNHAPPY_SERIOUS", new StringReference());
            /* 05 */ this.soundSet.Add("UNHAPPY_BREAKING_POINT", new StringReference());
            /* 06 */ this.soundSet.Add("LEADER", new StringReference());
            /* 07 */ this.soundSet.Add("TIRED", new StringReference());
            /* 08 */ this.soundSet.Add("BORED", new StringReference());
            /* 09 */ this.soundSet.Add("BATTLE_CRY1", new StringReference());
            /* 10 */ this.soundSet.Add("BATTLE_CRY2", new StringReference());
            /* 11 */ this.soundSet.Add("BATTLE_CRY3", new StringReference());
            /* 12 */ this.soundSet.Add("BATTLE_CRY4", new StringReference());
            /* 13 */ this.soundSet.Add("BATTLE_CRY5", new StringReference());
            /* 14 */ this.soundSet.Add("ATTACK1", new StringReference());
            /* 15 */ this.soundSet.Add("ATTACK2", new StringReference());
            /* 16 */ this.soundSet.Add("ATTACK3", new StringReference());
            /* 17 */ this.soundSet.Add("ATTACK4", new StringReference());
            /* 18 */ this.soundSet.Add("DAMAGE", new StringReference());
            /* 19 */ this.soundSet.Add("DYING", new StringReference());
            /* 20 */ this.soundSet.Add("HURT", new StringReference());
            /* 21 */ this.soundSet.Add("AREA_FOREST", new StringReference());
            /* 22 */ this.soundSet.Add("AREA_CITY", new StringReference());
            /* 23 */ this.soundSet.Add("AREA_DUNGEON", new StringReference());
            /* 24 */ this.soundSet.Add("AREA_DAY", new StringReference());
            /* 25 */ this.soundSet.Add("AREA_NIGHT", new StringReference());
            /* 26 */ this.soundSet.Add("SELECT_COMMON1", new StringReference());
            /* 27 */ this.soundSet.Add("SELECT_COMMON2", new StringReference());
            /* 28 */ this.soundSet.Add("SELECT_COMMON3", new StringReference());
            /* 29 */ this.soundSet.Add("SELECT_COMMON4", new StringReference());
            /* 30 */ this.soundSet.Add("SELECT_COMMON5", new StringReference());
            /* 31 */ this.soundSet.Add("SELECT_COMMON6", new StringReference());
            /* 32 */ this.soundSet.Add("SELECT_ACTION1", new StringReference());
            /* 33 */ this.soundSet.Add("SELECT_ACTION2", new StringReference());
            /* 34 */ this.soundSet.Add("SELECT_ACTION3", new StringReference());

            // Deviation between BG1 & BG2 -- Rare clicks start here in BG1, whereas in BG2 they are actions
            /* 35 */ this.soundSet.Add("SELECT_ACTION4/RARE1", new StringReference());
            /* 36 */ this.soundSet.Add("SELECT_ACTION5/RARE2", new StringReference());
            /* 37 */ this.soundSet.Add("SELECT_ACTION6/RARE3", new StringReference());
            /* 38 */ this.soundSet.Add("SELECT_ACTION7/RARE4", new StringReference());

            /* 39 */ this.soundSet.Add("INTERACTION1", new StringReference());
            /* 40 */ this.soundSet.Add("INTERACTION2", new StringReference());
            /* 41 */ this.soundSet.Add("INTERACTION3", new StringReference());
            /* 42 */ this.soundSet.Add("INTERACTION4", new StringReference());
            /* 43 */ this.soundSet.Add("INTERACTION5", new StringReference());
            /* 44 */ this.soundSet.Add("INSULT1", new StringReference());
            /* 45 */ this.soundSet.Add("INSULT2", new StringReference());
            /* 46 */ this.soundSet.Add("INSULT3", new StringReference());
            /* 47 */ this.soundSet.Add("COMPLIMENT1", new StringReference());
            /* 48 */ this.soundSet.Add("COMPLIMENT2", new StringReference());
            /* 49 */ this.soundSet.Add("COMPLIMENT3", new StringReference());
            /* 50 */ this.soundSet.Add("SPECIAL1", new StringReference());
            /* 51 */ this.soundSet.Add("SPECIAL2", new StringReference());
            /* 52 */ this.soundSet.Add("SPECIAL3", new StringReference());
            /* 53 */ this.soundSet.Add("REACT_TO_DIE_GENERAL", new StringReference());
            /* 54 */ this.soundSet.Add("REACT_TO_DIE_SPECIFIC", new StringReference());
            /* 55 */ this.soundSet.Add("RESPONSE_TO_COMPLIMENT1", new StringReference());
            /* 56 */ this.soundSet.Add("RESPONSE_TO_COMPLIMENT2", new StringReference());
            /* 57 */ this.soundSet.Add("RESPONSE_TO_COMPLIMENT3", new StringReference());
            /* 58 */ this.soundSet.Add("RESPONSE_TO_INSULT1", new StringReference());
            /* 59 */ this.soundSet.Add("RESPONSE_TO_INSULT2", new StringReference());
            /* 60 */ this.soundSet.Add("RESPONSE_TO_INSULT3", new StringReference());
            /* 61 */ this.soundSet.Add("DIALOG_HOSTILE", new StringReference());
            /* 62 */ this.soundSet.Add("DIALOG_DEFAULT", new StringReference());
            /* 63 */ this.soundSet.Add("SELECT_RARE1", new StringReference());
            /* 64 */ this.soundSet.Add("SELECT_RARE2", new StringReference());
            /* 65 */ this.soundSet.Add("CRITICAL_HIT", new StringReference());
            /* 66 */ this.soundSet.Add("CRITICAL_MISS", new StringReference());
            /* 67 */ this.soundSet.Add("TARGET_IMMUNE", new StringReference());
            /* 68 */ this.soundSet.Add("INVENTORY_FULL", new StringReference());
            /* 69 */ this.soundSet.Add("PICKED_POCKET", new StringReference());
            /* 70 */ this.soundSet.Add("HIDDEN_IN_SHADOWS", new StringReference());
            /* 71 */ this.soundSet.Add("SPELL_DISRUPTED", new StringReference());
            /* 72 */ this.soundSet.Add("SET_A_TRAP", new StringReference());
            /* 73 */ this.soundSet.Add("EXISTANCE4", new StringReference());
            /* 74 */ this.soundSet.Add("BIOGRAPHY", new StringReference());
            /* 75 */ this.soundSet.Add("Unknown75", new StringReference());
            /* 76 */ this.soundSet.Add("Unknown76", new StringReference());
            /* 77 */ this.soundSet.Add("Unknown77", new StringReference());
            /* 78 */ this.soundSet.Add("Unknown78", new StringReference());
            /* 79 */ this.soundSet.Add("Unknown79", new StringReference());
            /* 80 */ this.soundSet.Add("Unknown80", new StringReference());
            /* 81 */ this.soundSet.Add("Unknown81", new StringReference());
            /* 82 */ this.soundSet.Add("Unknown82", new StringReference());
            /* 83 */ this.soundSet.Add("Unknown83", new StringReference());
            /* 84 */ this.soundSet.Add("Unknown84", new StringReference());
            /* 85 */ this.soundSet.Add("Unknown85", new StringReference());
            /* 86 */ this.soundSet.Add("Unknown86", new StringReference());
            /* 87 */ this.soundSet.Add("Unknown87", new StringReference());
            /* 88 */ this.soundSet.Add("Unknown88", new StringReference());
            /* 89 */ this.soundSet.Add("Unknown89", new StringReference());
            /* 90 */ this.soundSet.Add("Unknown90", new StringReference());
            /* 91 */ this.soundSet.Add("Unknown91", new StringReference());
            /* 92 */ this.soundSet.Add("Unknown92", new StringReference());
            /* 93 */ this.soundSet.Add("Unknown93", new StringReference());
            /* 94 */ this.soundSet.Add("Unknown94", new StringReference());
            /* 95 */ this.soundSet.Add("Unknown95", new StringReference());
            /* 96 */ this.soundSet.Add("Unknown96", new StringReference());
            /* 97 */ this.soundSet.Add("Unknown97", new StringReference());
            /* 98 */ this.soundSet.Add("Unknown98", new StringReference());
            /* 99 */ this.soundSet.Add("Unknown99", new StringReference());
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 716);   //724-8(sig+ver)
            this.ReadBodyLeadingValues(remainingBody);
            this.ReadBodyProficiencies(remainingBody);
            this.ReadSoundSet(remainingBody, 156);
            this.ReadBodyCommonAfterSoundset(remainingBody);
            this.ReadBodyClassifications(remainingBody);
            this.ReadBodyFooter(remainingBody, 628);
        }

        /// <summary>Reads the classification entries from the creature file</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 616, size of at least 628 bytes.</param>
        protected override void ReadBodyClassifications(Byte[] headerBodyArray)
        {
            this.classificationHostility = headerBodyArray[616];
            this.classificationGeneral = headerBodyArray[617];
            this.classificationRace = headerBodyArray[618];
            this.classificationClass = headerBodyArray[619];
            this.classificationSpecific = headerBodyArray[620];
            this.classificationGender = headerBodyArray[621];
            this.classificationObject1 = headerBodyArray[622];
            this.classificationObject2 = headerBodyArray[623];
            this.classificationObject3 = headerBodyArray[624];
            this.classificationObject4 = headerBodyArray[625];
            this.classificationObject5 = headerBodyArray[626];
            this.classificationAlignment = headerBodyArray[627];
        }

        /// <summary>Reads the proficiencies section of the header</summary>
        /// <param name="remainingBody">Byte array to read from</param>
        protected override void ReadBodyProficiencies(Byte[] remainingBody)
        {
            this.ProficiencyLargeSword = remainingBody[102];
            this.ProficiencySmallSword = remainingBody[103];
            this.ProficiencyBow = remainingBody[104];
            this.ProficiencySpear = remainingBody[105];
            this.ProficiencyBlunt = remainingBody[106];
            this.ProficiencySpiked = remainingBody[107];
            this.ProficiencyAxe = remainingBody[108];
            this.ProficiencyMissile = remainingBody[109];
            this.ProficiencyUnused1 = remainingBody[110];
            this.ProficiencyUnused2 = remainingBody[111];
            this.ProficiencyUnused3 = remainingBody[112];
            this.ProficiencyUnused4 = remainingBody[113];
            this.ProficiencyUnused5 = remainingBody[114];
            this.ProficiencyUnused6 = remainingBody[115];
            this.ProficiencyUnused7 = remainingBody[116];
            this.ProficiencyUnused8 = remainingBody[117];
            this.ProficiencyUnused9 = remainingBody[118];
            this.ProficiencyUnused10 = remainingBody[119];
            this.ProficiencyUnused11 = remainingBody[120];
            this.ProficiencyUnused12 = remainingBody[121];
            this.ProficiencyUnused13 = remainingBody[122];
            this.Tracking = remainingBody[123];
            Array.Copy(remainingBody, 124, this.ReservedNonweaponProficiencies, 0, 32);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteBodyHeader(output);
            this.WriteProficiencies(output);
            this.WriteSoundSet(output);
            this.WriteBodyCommonAfterSoundset(output);
            this.WriteClassification(output);
            this.WriteBodyFooter(output);
        }

        /// <summary>This method writes the proficiency values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteProficiencies(Stream output)
        {
            output.WriteByte(this.ProficiencyLargeSword);
            output.WriteByte(this.ProficiencySmallSword);
            output.WriteByte(this.ProficiencyBow);
            output.WriteByte(this.ProficiencySpear);
            output.WriteByte(this.ProficiencyBlunt);
            output.WriteByte(this.ProficiencySpiked);
            output.WriteByte(this.ProficiencyAxe);
            output.WriteByte(this.ProficiencyMissile);
            output.WriteByte(this.ProficiencyUnused1);
            output.WriteByte(this.ProficiencyUnused2);
            output.WriteByte(this.ProficiencyUnused3);
            output.WriteByte(this.ProficiencyUnused4);
            output.WriteByte(this.ProficiencyUnused5);
            output.WriteByte(this.ProficiencyUnused6);
            output.WriteByte(this.ProficiencyUnused7);
            output.WriteByte(this.ProficiencyUnused8);
            output.WriteByte(this.ProficiencyUnused9);
            output.WriteByte(this.ProficiencyUnused10);
            output.WriteByte(this.ProficiencyUnused11);
            output.WriteByte(this.ProficiencyUnused12);
            output.WriteByte(this.ProficiencyUnused13);
            output.WriteByte(this.Tracking);
            output.Write(this.ReservedNonweaponProficiencies, 0, 32);
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.ToStringCreatureVersion(builder);
            this.ToStringHeader(builder);
            this.ToStringProcifiencies(builder);
            this.ToStringSoundSet(builder);
            this.ToStringStatsAndScripts(builder);
            this.ToStringClassifications(builder);
            this.ToStringHeaderFooter(builder);

            return builder.ToString();
        }

        /// <summary>Returns the printable read-friendly version of the creature format</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringCreatureVersion(StringBuilder builder)
        {
            builder.AppendLine("Creature version 1.0 header:");
        }

        /// <summary>Generates a String representing the proficiencies area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringProcifiencies(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Large Sword)"));
            builder.Append(this.ProficiencyLargeSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Small Sword)"));
            builder.Append(this.ProficiencySmallSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Bow)"));
            builder.Append(this.ProficiencyBow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Spear)"));
            builder.Append(this.ProficiencySpear);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Blunt)"));
            builder.Append(this.ProficiencyBlunt);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Spiked)"));
            builder.Append(this.ProficiencySpiked);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Axe)"));
            builder.Append(this.ProficiencyAxe);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Missile)"));
            builder.Append(this.ProficiencyMissile);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #1)"));
            builder.Append(this.ProficiencyUnused1);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #2)"));
            builder.Append(this.ProficiencyUnused2);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #3)"));
            builder.Append(this.ProficiencyUnused3);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #4)"));
            builder.Append(this.ProficiencyUnused4);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #5)"));
            builder.Append(this.ProficiencyUnused5);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #6)"));
            builder.Append(this.ProficiencyUnused6);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #7)"));
            builder.Append(this.ProficiencyUnused7);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #8)"));
            builder.Append(this.ProficiencyUnused8);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #9)"));
            builder.Append(this.ProficiencyUnused9);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #10)"));
            builder.Append(this.ProficiencyUnused10);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #11)"));
            builder.Append(this.ProficiencyUnused11);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #12)"));
            builder.Append(this.ProficiencyUnused12);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #13)"));
            builder.Append(this.ProficiencyUnused13);
            builder.Append(StringFormat.ToStringAlignment("Tracking"));
            builder.Append(this.Tracking);
            builder.Append(StringFormat.ToStringAlignment("Reserved non-weapon proficiencies"));
            builder.Append(StringFormat.ByteArrayToHexString(this.ReservedNonweaponProficiencies));
        }
        #endregion
    }
}