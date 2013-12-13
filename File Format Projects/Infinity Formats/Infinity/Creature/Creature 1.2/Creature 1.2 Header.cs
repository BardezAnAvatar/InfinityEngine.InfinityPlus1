using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature1_2
{
    /// <summary>Creature header version 1.2</summary>
    public class Creature1_2Header : Creature2eHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 888;

        #region Members
        /// <summary>Fist proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyFist;

        /// <summary>Edged weapons proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyEdged;

        /// <summary>Hammer proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyHammer;

        /// <summary>Club proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyClub;

        /// <summary>Unused proficiency #7</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused7;

        /// <summary>Unused proficiency #8</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused8;

        /// <summary>Unused proficiency #9</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused9;

        /// <summary>Unused proficiency #10</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused10;

        /// <summary>Unused proficiency #11</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused11;

        /// <summary>Unused proficiency #12</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused12;

        /// <summary>Unused proficiency #13</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused13;

        /// <summary>Unused proficiency #14</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused14;

        /// <summary>Unused proficiency #15</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused15;

        /// <summary>Deity</summary>
        /// <remarks>This seems to be the leading 2 bytes of the "Kit" field, expressed in IESDP</remarks>
        /// <value>So far, only 1 ("Experience" in NI) and 0</value>
        protected UInt16 deity;

        /// <summary>24 unknown bytes.</summary>
        /// <remarks>Part of 36 unknown in IESDP</remarks>
        protected Byte[] unknown1;

        /// <summary>Apparently four bytes indicating a zombie disguise.</summary>
        /// <value>0 for no, -1 for yes</value>
        /// <remarks>This is taken from Near Infinity, and is ultimately just a guess.</remarks>
        protected Int32 zombieDisguise;

        /// <summary>8 unknown bytes.</summary>
        /// <remarks>Part of 36 unknown in IESDP</remarks>
        protected Byte[] unknown2;

        /// <summary>Offset to the overlay section</summary>
        protected UInt32 offsetOverlays;

        /// <summary>Size of the overlay section</summary>
        /// <remarks>Not count, actual binary size</remarks>
        protected UInt32 sizeOverlays;

        /// <summary>Experience totalled in second class</summary>
        protected UInt32 experienceSecondClass;

        /// <summary>Experience totalled in third class</summary>
        protected UInt32 experienceThirdClass;

        /// <summary>First internal variable array</summary>
        protected Int16 internalVariable1;

        /// <summary>Second internal variable array</summary>
        protected Int16 internalVariable2;

        /// <summary>Third internal variable array</summary>
        protected Int16 internalVariable3;

        /// <summary>Fourth internal variable array</summary>
        protected Int16 internalVariable4;

        /// <summary>Fifth internal variable array</summary>
        protected Int16 internalVariable5;

        /// <summary>Sixth internal variable array</summary>
        protected Int16 internalVariable6;

        /// <summary>Seventh internal variable array</summary>
        protected Int16 internalVariable7;

        /// <summary>Eighth internal variable array</summary>
        protected Int16 internalVariable8;

        /// <summary>Ninth internal variable array</summary>
        protected Int16 internalVariable9;

        /// <summary>Tenth internal variable array</summary>
        protected Int16 internalVariable10;

        /// <summary>Amount by which to increment the PC's good variable</summary>
        protected Byte variableIncrementGood;

        /// <summary>Amount by which to increment the PC's law variable</summary>
        protected Byte variableIncrementLaw;

        /// <summary>Amount by which to increment the PC's Lady variable</summary>
        protected Byte variableIncrementLady;

        /// <summary>Amount by which to increment the PC's murder variable</summary>
        protected Byte variableIncrementMurder;

        /// <summary>Range to activate dialog</summary>
        protected Byte dialogActivationRange;

        /// <summary>Selecion circle Size</summary>
        protected UInt16 selectionCircleSize;

        /// <summary>Count of colors</summary>
        protected Byte countColors;

        /// <summary>Additional Flags seen as necessary for</summary>
        protected AttributeFlags attributeFlags;

        /// <summary>Color 1</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color1;

        /// <summary>Color 2</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color2;

        /// <summary>Color 3</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color3;

        /// <summary>Color 4</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color4;

        /// <summary>Color 5</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color5;

        /// <summary>Color 6</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color6;

        /// <summary>Color 7</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        protected UInt16 color7;

        //next comes 3 unknown bytes. Frequently, they are 0x20 00 20
        /// <summary>Unknown 3 bytes after color entries</summary>
        protected Byte[] unknown3;

        /// <summary>color placement index 1</summary>
        protected Byte colorPlacement1;

        /// <summary>color placement index 2</summary>
        protected Byte colorPlacement2;

        /// <summary>color placement index 3</summary>
        protected Byte colorPlacement3;

        /// <summary>color placement index 4</summary>
        protected Byte colorPlacement4;

        /// <summary>color placement index 5</summary>
        protected Byte colorPlacement5;

        /// <summary>color placement index 6</summary>
        protected Byte colorPlacement6;

        /// <summary>color placement index 7</summary>
        protected Byte colorPlacement7;
        
        //next comes 21 unknown bytes. Frequently, they end with 0x20 00 04 00 00
        /// <summary>Unknown 16 bytes after color placement entries</summary>
        /// <value>appears to be zeroed out</value>
        protected Byte[] unknown4;

        /// <summary>Trailing after 16 unknown bytes a 2-byte value</summary>
        /// <value>almost exclusively 0x20</value>
        protected UInt16 unknown5;

        /// <summary>Trailing after 16 unknown bytes another 2-byte value</summary>
        /// <value>Most frequently 0x04, but also 2 and 8... flag/enumerator field?</value>
        protected UInt16 unknown6;

        /// <summary>Trailing byte ofthe unknown 21 bytes after color placement entries</summary>
        protected Byte unknown7;

        /// <summary>Species value</summary>
        /// <value>Matches to RACE.IDS</value>
        protected Byte species;

        /// <summary>Team value</summary>
        /// <value>Matches to TEAM.IDS</value>
        protected Byte team;

        /// <summary>Faction value</summary>
        /// <value>Matches to FACTION.IDS</value>
        protected Byte faction;
        #endregion

        #region Properties
        /// <summary>Fist proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyFist
        {
            get { return this.proficiencyFist; }
            set { this.proficiencyFist = value; }
        }

        /// <summary>Edged weapons proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyEdged
        {
            get { return this.proficiencyEdged; }
            set { this.proficiencyEdged = value; }
        }

        /// <summary>Hammer proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyHammer
        {
            get { return this.proficiencyHammer; }
            set { this.proficiencyHammer = value; }
        }

        /// <summary>Club proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyClub
        {
            get { return this.proficiencyClub; }
            set { this.proficiencyClub = value; }
        }

        /// <summary>Unused proficiency #7</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused7
        {
            get { return this.proficiencyUnused7; }
            set { this.proficiencyUnused7 = value; }
        }

        /// <summary>Unused proficiency #8</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused8
        {
            get { return this.proficiencyUnused8; }
            set { this.proficiencyUnused8 = value; }
        }

        /// <summary>Unused proficiency #9</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused9
        {
            get { return this.proficiencyUnused9; }
            set { this.proficiencyUnused9 = value; }
        }

        /// <summary>Unused proficiency #10</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused10
        {
            get { return this.proficiencyUnused10; }
            set { this.proficiencyUnused10 = value; }
        }

        /// <summary>Unused proficiency #11</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused11
        {
            get { return this.proficiencyUnused11; }
            set { this.proficiencyUnused11 = value; }
        }

        /// <summary>Unused proficiency #12</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused12
        {
            get { return this.proficiencyUnused12; }
            set { this.proficiencyUnused12 = value; }
        }

        /// <summary>Unused proficiency #13</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused13
        {
            get { return this.proficiencyUnused13; }
            set { this.proficiencyUnused13 = value; }
        }

        /// <summary>Unused proficiency #14</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused14
        {
            get { return this.proficiencyUnused14; }
            set { this.proficiencyUnused14 = value; }
        }

        /// <summary>Unused proficiency #15</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused15
        {
            get { return this.proficiencyUnused15; }
            set { this.proficiencyUnused15 = value; }
        }

        /// <summary>Deity</summary>
        /// <remarks>This seems to be the leading 2 bytes of the "Kit" field, expressed in IESDP</remarks>
        /// <value>So far, only 1 ("Experience" in NI) and 0</value>
        public UInt16 Deity
        {
            get { return this.deity; }
            set { this.deity = value; }
        }

        /// <summary>24 unknown bytes.</summary>
        /// <remarks>Part of 36 unknown in IESDP</remarks>
        public Byte[] Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }

        /// <summary>Apparently four bytes indicating a zombie disguise.</summary>
        /// <value>0 for no, -1 for yes</value>
        /// <remarks>This is taken from Near Infinity, and is ultimately just a guess.</remarks>
        public Int32 ZombieDisguise
        {
            get { return this.zombieDisguise; }
            set { this.zombieDisguise = value; }
        }

        /// <summary>8 unknown bytes.</summary>
        /// <remarks>Part of 36 unknown in IESDP</remarks>
        public Byte[] Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Offset to the overlay section</summary>
        public UInt32 OffsetOverlays
        {
            get { return this.offsetOverlays; }
            set { this.offsetOverlays = value; }
        }

        /// <summary>Size of the overlay section</summary>
        /// <remarks>Not count, actual binary size</remarks>
        public UInt32 SizeOverlays
        {
            get { return this.sizeOverlays; }
            set { this.sizeOverlays = value; }
        }

        /// <summary>Experience totalled in second class</summary>
        public UInt32 ExperienceSecondClass
        {
            get { return this.experienceSecondClass; }
            set { this.experienceSecondClass = value; }
        }

        /// <summary>Experience totalled in third class</summary>
        public UInt32 ExperienceThirdClass
        {
            get { return this.experienceThirdClass; }
            set { this.experienceThirdClass = value; }
        }

        /// <summary>First internal variable array</summary>
        public Int16 InternalVariable1
        {
            get { return this.internalVariable1; }
            set { this.internalVariable1 = value; }
        }

        /// <summary>Second internal variable array</summary>
        public Int16 InternalVariable2
        {
            get { return this.internalVariable2; }
            set { this.internalVariable2 = value; }
        }

        /// <summary>Third internal variable array</summary>
        public Int16 InternalVariable3
        {
            get { return this.internalVariable3; }
            set { this.internalVariable3 = value; }
        }

        /// <summary>Fourth internal variable array</summary>
        public Int16 InternalVariable4
        {
            get { return this.internalVariable4; }
            set { this.internalVariable4 = value; }
        }

        /// <summary>Fifth internal variable array</summary>
        public Int16 InternalVariable5
        {
            get { return this.internalVariable5; }
            set { this.internalVariable5 = value; }
        }

        /// <summary>Sixth internal variable array</summary>
        public Int16 InternalVariable6
        {
            get { return this.internalVariable6; }
            set { this.internalVariable6 = value; }
        }

        /// <summary>Seventh internal variable array</summary>
        public Int16 InternalVariable7
        {
            get { return this.internalVariable7; }
            set { this.internalVariable7 = value; }
        }

        /// <summary>Eighth internal variable array</summary>
        public Int16 InternalVariable8
        {
            get { return this.internalVariable8; }
            set { this.internalVariable8 = value; }
        }

        /// <summary>Ninth internal variable array</summary>
        public Int16 InternalVariable9
        {
            get { return this.internalVariable9; }
            set { this.internalVariable9 = value; }
        }

        /// <summary>Tenth internal variable array</summary>
        public Int16 InternalVariable10
        {
            get { return this.internalVariable10; }
            set { this.internalVariable10 = value; }
        }

        /// <summary>Amount by which to increment the PC's good variable</summary>
        public Byte VariableIncrementGood
        {
            get { return this.variableIncrementGood; }
            set { this.variableIncrementGood = value; }
        }

        /// <summary>Amount by which to increment the PC's law variable</summary>
        public Byte VariableIncrementLaw
        {
            get { return this.variableIncrementLaw; }
            set { this.variableIncrementLaw = value; }
        }

        /// <summary>Amount by which to increment the PC's Lady variable</summary>
        public Byte VariableIncrementLady
        {
            get { return this.variableIncrementLady; }
            set { this.variableIncrementLady = value; }
        }

        /// <summary>Amount by which to increment the PC's murder variable</summary>
        public Byte VariableIncrementMurder
        {
            get { return this.variableIncrementMurder; }
            set { this.variableIncrementMurder = value; }
        }

        /// <summary>Name/group to add into the Monstrous Compendium</summary>
        public ZString MonstrousCompendiumEntry { get; set; }

        /// <summary>Range to activate dialog</summary>
        public Byte DialogActivationRange
        {
            get { return this.dialogActivationRange; }
            set { this.dialogActivationRange = value; }
        }

        /// <summary>Selecion circle Size</summary>
        public UInt16 SelectionCircleSize
        {
            get { return this.selectionCircleSize; }
            set { this.selectionCircleSize = value; }
        }

        /// <summary>Count of colors</summary>
        public Byte CountColors
        {
            get { return this.countColors; }
            set { this.countColors = value; }
        }

        /// <summary>Additional Flags seen as necessary for</summary>
        public AttributeFlags AttributeFlags
        {
            get { return this.attributeFlags; }
            set { this.attributeFlags = value; }
        }

        /// <summary>Color 1</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color1
        {
            get { return this.color1; }
            set { this.color1 = value; }
        }

        /// <summary>Color 2</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color2
        {
            get { return this.color2; }
            set { this.color2 = value; }
        }

        /// <summary>Color 3</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color3
        {
            get { return this.color3; }
            set { this.color3 = value; }
        }

        /// <summary>Color 4</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color4
        {
            get { return this.color4; }
            set { this.color4 = value; }
        }

        /// <summary>Color 5</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color5
        {
            get { return this.color5; }
            set { this.color5 = value; }
        }

        /// <summary>Color 6</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color6
        {
            get { return this.color6; }
            set { this.color6 = value; }
        }

        /// <summary>Color 7</summary>
        /// <value>Matches CLOWNCLR.IDS</value>
        public UInt16 Color7
        {
            get { return this.color7; }
            set { this.color7 = value; }
        }

        /// <summary>Unknown 3 bytes after color entries</summary>
        public Byte[] Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }

        /// <summary>color placement index 1</summary>
        public Byte ColorPlacement1
        {
            get { return this.colorPlacement1; }
            set { this.colorPlacement1 = value; }
        }

        /// <summary>color placement index 2</summary>
        public Byte ColorPlacement2
        {
            get { return this.colorPlacement2; }
            set { this.colorPlacement2 = value; }
        }

        /// <summary>color placement index 3</summary>
        public Byte ColorPlacement3
        {
            get { return this.colorPlacement3; }
            set { this.colorPlacement3 = value; }
        }

        /// <summary>color placement index 4</summary>
        public Byte ColorPlacement4
        {
            get { return this.colorPlacement4; }
            set { this.colorPlacement4 = value; }
        }

        /// <summary>color placement index 5</summary>
        public Byte ColorPlacement5
        {
            get { return this.colorPlacement5; }
            set { this.colorPlacement5 = value; }
        }

        /// <summary>color placement index 6</summary>
        public Byte ColorPlacement6
        {
            get { return this.colorPlacement6; }
            set { this.colorPlacement6 = value; }
        }

        /// <summary>color placement index 7</summary>
        public Byte ColorPlacement7
        {
            get { return this.colorPlacement7; }
            set { this.colorPlacement7 = value; }
        }

        /// <summary>Unknown 16 bytes after color placement entries</summary>
        /// <value>appears to be zeroed out</value>
        public Byte[] Unknown4
        {
            get { return this.unknown4; }
            set { this.unknown4 = value; }
        }

        /// <summary>Trailing after 16 unknown bytes a 2-byte value</summary>
        /// <value>almost exclusively 0x20</value>
        public UInt16 Unknown5
        {
            get { return this.unknown5; }
            set { this.unknown5 = value; }
        }

        /// <summary>Trailing after 16 unknown bytes another 2-byte value</summary>
        /// <value>Most frequently 0x04, but also 2 and 8... flag/enumerator field?</value>
        public UInt16 Unknown6
        {
            get { return this.unknown6; }
            set { this.unknown6 = value; }
        }

        /// <summary>Trailing byte ofthe unknown 21 bytes after color placement entries</summary>
        public Byte Unknown7
        {
            get { return this.unknown7; }
            set { this.unknown7 = value; }
        }

        /// <summary>Species value</summary>
        /// <value>Matches to RACE.IDS</value>
        public Byte Species
        {
            get { return this.species; }
            set { this.species = value; }
        }

        /// <summary>Team value</summary>
        /// <value>Matches to TEAM.IDS</value>
        public Byte Team
        {
            get { return this.team; }
            set { this.team = value; }
        }

        /// <summary>Faction value</summary>
        /// <value>Matches to FACTION.IDS</value>
        public Byte Faction
        {
            get { return this.faction; }
            set { this.faction = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Creature1_2Header()
        {
            this.Initialize();
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.unknown1 = new Byte[24];
            this.unknown2 = new Byte[8];
            this.unknown3 = new Byte[3];
            this.unknown4 = new Byte[16];
            this.MonstrousCompendiumEntry = new ZString();
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
            
            /* 35 */ this.soundSet.Add("SELECT_RARE1", new StringReference());
            /* 36 */ this.soundSet.Add("SELECT_RARE2", new StringReference());
            /* 37 */ this.soundSet.Add("SELECT_RARE3", new StringReference());
            /* 38 */ this.soundSet.Add("SELECT_RARE4", new StringReference());

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
            /* 59 */ this.soundSet.Add("DIALOG_HOSTILE", new StringReference());
            /* 60 */ this.soundSet.Add("DIALOG_DEFAULT", new StringReference());
            /* 61 */ this.soundSet.Add("NORDOM_PORTAL", new StringReference());
            /* 62 */ this.soundSet.Add("Unknown62", new StringReference());
            /* 63 */ this.soundSet.Add("Unknown63", new StringReference());
            /* 64 */ this.soundSet.Add("Unknown64", new StringReference());
            /* 65 */ this.soundSet.Add("Unknown65", new StringReference());
            /* 66 */ this.soundSet.Add("Unknown66", new StringReference());
            /* 67 */ this.soundSet.Add("Unknown67", new StringReference());
            /* 68 */ this.soundSet.Add("Unknown68", new StringReference());
            /* 69 */ this.soundSet.Add("Unknown69", new StringReference());
            /* 70 */ this.soundSet.Add("LOCATION1", new StringReference());
            /* 71 */ this.soundSet.Add("LOCATION2", new StringReference());
            /* 72 */ this.soundSet.Add("LOCATION3", new StringReference());
            /* 73 */ this.soundSet.Add("LOCATION4", new StringReference());
            /* 74 */ this.soundSet.Add("LOCATION5", new StringReference());
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
            this.Initialize();

            Byte[] headerBody = ReusableIO.BinaryRead(input, 880);

            this.ReadBodyLeadingValues(headerBody);
            this.ReadBodyProficiencies(headerBody);
            this.ReadSoundSet(headerBody, 156);
            this.ReadBodyCommonAfterSoundset(headerBody);
            this.ReadBodyPlanescapeAdditions(headerBody);
            this.ReadBodyClassifications(headerBody);
            this.ReadBodyFooter(headerBody, 792);
        }
        
        /// <summary>Reads the proficiencies section of the header</summary>
        /// <param name="remainingBody">Byte array to read from</param>
        protected override void ReadBodyProficiencies(Byte[] remainingBody)
        {
            this.ProficiencyFist = remainingBody[102];
            this.ProficiencyEdged = remainingBody[103];
            this.ProficiencyHammer = remainingBody[104];
            this.ProficiencyAxe = remainingBody[105];
            this.ProficiencyClub = remainingBody[106];
            this.ProficiencyBow = remainingBody[107];
            this.ProficiencyUnused1 = remainingBody[108];
            this.ProficiencyUnused2 = remainingBody[109];
            this.ProficiencyUnused3 = remainingBody[110];
            this.ProficiencyUnused4 = remainingBody[111];
            this.ProficiencyUnused5 = remainingBody[112];
            this.ProficiencyUnused6 = remainingBody[113];
            this.ProficiencyUnused7 = remainingBody[114];
            this.ProficiencyUnused8 = remainingBody[115];
            this.ProficiencyUnused9 = remainingBody[116];
            this.ProficiencyUnused10 = remainingBody[117];
            this.ProficiencyUnused11 = remainingBody[118];
            this.ProficiencyUnused12 = remainingBody[119];
            this.ProficiencyUnused13 = remainingBody[120];
            this.ProficiencyUnused14 = remainingBody[121];
            this.ProficiencyUnused15 = remainingBody[122];
            this.Tracking = remainingBody[123];
            Array.Copy(remainingBody, 124, this.ReservedNonweaponProficiencies, 0, 32);
        }

        /// <summary>This method will read the kit variable to the output Stream</summary>
        /// <param name="dataArray">Byte array to read from. Expected to be reading from index 572 and reading 4 bytes.</param>
        protected override void ReadKitValues(Byte[] dataArray)
        {
            this.deity = ReusableIO.ReadUInt16FromArray(dataArray, 572);
            this.Kit = (Kit2e)(ReusableIO.ReadUInt16FromArray(dataArray, 574) << 16);
        }

        /// <summary>Reads Planescape: Torment structure additions</summary>
        /// <param name="headerBody">Byte array to read from</param>
        protected void ReadBodyPlanescapeAdditions(Byte[] headerBody)
        {
            Array.Copy(headerBody, 616, this.unknown1, 0, 24);
            this.zombieDisguise = ReusableIO.ReadInt32FromArray(headerBody, 640);
            Array.Copy(headerBody, 644, this.unknown2, 0, 8);
            this.offsetOverlays = ReusableIO.ReadUInt32FromArray(headerBody, 652);
            this.sizeOverlays = ReusableIO.ReadUInt32FromArray(headerBody, 656);
            this.experienceSecondClass = ReusableIO.ReadUInt32FromArray(headerBody, 660);
            this.experienceThirdClass = ReusableIO.ReadUInt32FromArray(headerBody, 664);
            this.internalVariable1 = ReusableIO.ReadInt16FromArray(headerBody, 668);
            this.internalVariable2 = ReusableIO.ReadInt16FromArray(headerBody, 670);
            this.internalVariable3 = ReusableIO.ReadInt16FromArray(headerBody, 672);
            this.internalVariable4 = ReusableIO.ReadInt16FromArray(headerBody, 674);
            this.internalVariable5 = ReusableIO.ReadInt16FromArray(headerBody, 676);
            this.internalVariable6 = ReusableIO.ReadInt16FromArray(headerBody, 678);
            this.internalVariable7 = ReusableIO.ReadInt16FromArray(headerBody, 680);
            this.internalVariable8 = ReusableIO.ReadInt16FromArray(headerBody, 682);
            this.internalVariable9 = ReusableIO.ReadInt16FromArray(headerBody, 684);
            this.internalVariable10 = ReusableIO.ReadInt16FromArray(headerBody, 686);
            this.variableIncrementGood = headerBody[688];
            this.variableIncrementLaw = headerBody[689];
            this.variableIncrementLady = headerBody[690];
            this.variableIncrementMurder = headerBody[691];
            this.MonstrousCompendiumEntry.Source = ReusableIO.ReadStringFromByteArray(headerBody, 692, CultureConstants.CultureCodeEnglish, 32);
            this.dialogActivationRange = headerBody[724];
            this.selectionCircleSize = ReusableIO.ReadUInt16FromArray(headerBody, 725);
            this.countColors = headerBody[727];
            this.attributeFlags = (Enums.AttributeFlags)ReusableIO.ReadUInt32FromArray(headerBody, 728);
            this.color1 = ReusableIO.ReadUInt16FromArray(headerBody, 732);
            this.color2 = ReusableIO.ReadUInt16FromArray(headerBody, 734);
            this.color3 = ReusableIO.ReadUInt16FromArray(headerBody, 736);
            this.color4 = ReusableIO.ReadUInt16FromArray(headerBody, 738);
            this.color5 = ReusableIO.ReadUInt16FromArray(headerBody, 740);
            this.color6 = ReusableIO.ReadUInt16FromArray(headerBody, 742);
            this.color7 = ReusableIO.ReadUInt16FromArray(headerBody, 744);
            Array.Copy(headerBody, 746, this.unknown3, 0, 3);
            this.colorPlacement1 = headerBody[749];
            this.colorPlacement2 = headerBody[750];
            this.colorPlacement3 = headerBody[751];
            this.colorPlacement4 = headerBody[752];
            this.colorPlacement5 = headerBody[753];
            this.colorPlacement6 = headerBody[754];
            this.colorPlacement7 = headerBody[755];
            Array.Copy(headerBody, 756, this.unknown4, 0, 16);
            this.unknown5 = ReusableIO.ReadUInt16FromArray(headerBody, 772);
            this.unknown6 = ReusableIO.ReadUInt16FromArray(headerBody, 774);
            this.unknown7 = headerBody[776];
        }

        /// <summary>Reads the classification entries from the creature file</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 616, size of at least 628 bytes.</param>
        protected override void ReadBodyClassifications(Byte[] headerBodyArray)
        {
            this.species = headerBodyArray[777];
            this.team = headerBodyArray[778];
            this.faction = headerBodyArray[779];
            this.classificationHostility = headerBodyArray[780];
            this.classificationGeneral = headerBodyArray[781];
            this.classificationRace = headerBodyArray[782];
            this.classificationClass = headerBodyArray[783];
            this.classificationSpecific = headerBodyArray[784];
            this.classificationGender = headerBodyArray[785];
            this.classificationObject1 = headerBodyArray[786];
            this.classificationObject2 = headerBodyArray[787];
            this.classificationObject3 = headerBodyArray[788];
            this.classificationObject4 = headerBodyArray[789];
            this.classificationObject5 = headerBodyArray[790];
            this.classificationAlignment = headerBodyArray[791];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteBodyHeader(output);
            this.WriteProficiencies(output);
            this.WriteSoundSet(output);
            this.WriteBodyCommonAfterSoundset(output);
            this.WriteBodyPlanescapeAdditions(output);
            this.WriteClassification(output);
            this.WriteBodyFooter(output);
        }

        /// <summary>This method writes the proficiency values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteProficiencies(Stream output)
        {
            output.WriteByte(this.ProficiencyFist);
            output.WriteByte(this.ProficiencyEdged);
            output.WriteByte(this.ProficiencyHammer);
            output.WriteByte(this.ProficiencyAxe);
            output.WriteByte(this.ProficiencyClub);
            output.WriteByte(this.ProficiencyBow);
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
            output.WriteByte(this.ProficiencyUnused14);
            output.WriteByte(this.ProficiencyUnused15);
            output.WriteByte(this.Tracking);
            output.Write(this.ReservedNonweaponProficiencies, 0, 32);
        }

        /// <summary>This method will write out the kit variable to the output Stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteKitValues(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.deity, output);
            UInt16 _kit = (UInt16)((UInt32)this.Kit >> 16);
            ReusableIO.WriteUInt16ToStream(_kit, output);
        }

        /// <summary>Reads Planescape: Torment structure additions</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBodyPlanescapeAdditions(Stream output)
        {
            output.Write(this.unknown1, 0, 24);
            ReusableIO.WriteInt32ToStream(this.zombieDisguise, output);
            output.Write(this.unknown2, 0, 8);
            ReusableIO.WriteUInt32ToStream(this.offsetOverlays, output);
            ReusableIO.WriteUInt32ToStream(this.sizeOverlays, output);
            ReusableIO.WriteUInt32ToStream(this.experienceSecondClass, output);
            ReusableIO.WriteUInt32ToStream(this.experienceThirdClass, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable1, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable2, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable3, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable4, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable5, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable6, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable7, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable8, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable9, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable10, output);
            output.WriteByte(this.variableIncrementGood);
            output.WriteByte(this.variableIncrementLaw);
            output.WriteByte(this.variableIncrementLady);
            output.WriteByte(this.variableIncrementMurder);
            ReusableIO.WriteStringToStream(this.MonstrousCompendiumEntry.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            output.WriteByte(this.dialogActivationRange);
            ReusableIO.WriteUInt16ToStream(this.selectionCircleSize, output);
            output.WriteByte(this.countColors);
            ReusableIO.WriteUInt32ToStream((UInt32)this.attributeFlags, output);
            ReusableIO.WriteUInt16ToStream(this.color1, output);
            ReusableIO.WriteUInt16ToStream(this.color2, output);
            ReusableIO.WriteUInt16ToStream(this.color3, output);
            ReusableIO.WriteUInt16ToStream(this.color4, output);
            ReusableIO.WriteUInt16ToStream(this.color5, output);
            ReusableIO.WriteUInt16ToStream(this.color6, output);
            ReusableIO.WriteUInt16ToStream(this.color7, output);
            output.Write(this.unknown3, 0, 3);
            output.WriteByte(this.colorPlacement1);
            output.WriteByte(this.colorPlacement2);
            output.WriteByte(this.colorPlacement3);
            output.WriteByte(this.colorPlacement4);
            output.WriteByte(this.colorPlacement5);
            output.WriteByte(this.colorPlacement6);
            output.WriteByte(this.colorPlacement7);
            output.Write(this.unknown4, 0, 16);
            ReusableIO.WriteUInt16ToStream(this.unknown5, output);
            ReusableIO.WriteUInt16ToStream(this.unknown6, output);
            output.WriteByte(this.unknown7);
        }

        /// <summary>This method writes the classification values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteClassification(Stream output)
        {
            output.WriteByte(this.species);
            output.WriteByte(this.team);
            output.WriteByte(this.faction);
            output.WriteByte(this.classificationHostility);
            output.WriteByte(this.classificationGeneral);
            output.WriteByte(this.classificationRace);
            output.WriteByte(this.classificationClass);
            output.WriteByte(this.classificationSpecific);
            output.WriteByte(this.classificationGender);
            output.WriteByte(this.classificationObject1);
            output.WriteByte(this.classificationObject2);
            output.WriteByte(this.classificationObject3);
            output.WriteByte(this.classificationObject4);
            output.WriteByte(this.classificationObject5);
            output.WriteByte(this.classificationAlignment);
        }
        #endregion

        #region ToString() helpers
        /// <summary>Generates a human-readable multi-line string for console output that indicates which AttributeFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetCreatureAttributeFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.Transparent) == AttributeFlags.Transparent, AttributeFlags.Transparent.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementDeathVariableCounter) == AttributeFlags.IncrementDeathVariableCounter, AttributeFlags.IncrementDeathVariableCounter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementCharacterTypeDeathCounter) == AttributeFlags.IncrementCharacterTypeDeathCounter, AttributeFlags.IncrementCharacterTypeDeathCounter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.DeathCounterStartsWithKill) == AttributeFlags.DeathCounterStartsWithKill, AttributeFlags.DeathCounterStartsWithKill.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementFactionDeathCounter) == AttributeFlags.IncrementFactionDeathCounter, AttributeFlags.IncrementFactionDeathCounter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementTeamDeathCounter) == AttributeFlags.IncrementTeamDeathCounter, AttributeFlags.IncrementTeamDeathCounter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.Invulnerable) == AttributeFlags.Invulnerable, AttributeFlags.Invulnerable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementVariableGood) == AttributeFlags.IncrementVariableGood, AttributeFlags.IncrementVariableGood.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementVariableLaw) == AttributeFlags.IncrementVariableLaw, AttributeFlags.IncrementVariableLaw.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementVariableLady) == AttributeFlags.IncrementVariableLady, AttributeFlags.IncrementVariableLady.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.IncrementVariableMurder) == AttributeFlags.IncrementVariableMurder, AttributeFlags.IncrementVariableMurder.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.DoNotForceFacingTargetDuringDialog) == AttributeFlags.DoNotForceFacingTargetDuringDialog, AttributeFlags.DoNotForceFacingTargetDuringDialog.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.HelpCallTurnsNearbyCreaturesHostileToParty) == AttributeFlags.HelpCallTurnsNearbyCreaturesHostileToParty, AttributeFlags.HelpCallTurnsNearbyCreaturesHostileToParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.attributeFlags & AttributeFlags.DoNotIncrementGlobals) == AttributeFlags.DoNotIncrementGlobals, AttributeFlags.DoNotIncrementGlobals.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

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
            this.ToStringPlanescapeAdditions(builder);
            this.ToStringClassifications(builder);
            this.ToStringHeaderFooter(builder);

            return builder.ToString();
        }

        /// <summary>Returns the printable read-friendly version of the creature format</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringCreatureVersion(StringBuilder builder)
        {
            builder.AppendLine("Creature version 1.2 header:");
        }

        /// <summary>Generates a String representing the proficiencies area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringProcifiencies(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Fist)"));
            builder.Append(this.ProficiencyFist);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Edged)"));
            builder.Append(this.ProficiencyEdged);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Hammer)"));
            builder.Append(this.ProficiencyHammer);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Axe)"));
            builder.Append(this.ProficiencyAxe);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Club)"));
            builder.Append(this.ProficiencyClub);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Bow)"));
            builder.Append(this.ProficiencyBow);
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
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #14)"));
            builder.Append(this.ProficiencyUnused14);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #15)"));
            builder.Append(this.ProficiencyUnused15);
            builder.Append(StringFormat.ToStringAlignment("Tracking"));
            builder.Append(this.Tracking);
            builder.Append(StringFormat.ToStringAlignment("Reserved non-weapon proficiencies"));
            builder.Append(StringFormat.ByteArrayToHexString(this.ReservedNonweaponProficiencies));
        }

        /// <summary>This method appends the kit variable(s) to the output StringBuilder</summary>
        /// <param name="dataArray">StringBuilder to append to</param>
        protected override void ToStringKitValues(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Deity"));
            builder.Append(this.deity);
            builder.Append(StringFormat.ToStringAlignment("Kit"));
            builder.Append((UInt32)this.Kit);
            builder.Append(StringFormat.ToStringAlignment("Kit (description)"));
            builder.Append(this.Kit.GetDescription());
        }

        /// <summary>Generates a String representing the classifications area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringClassifications(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Classification (Hostility) [EA.IDS]"));
            builder.Append(this.classificationHostility);
            builder.Append(StringFormat.ToStringAlignment("Classification (General)   [GENERAL.IDS]"));
            builder.Append(this.classificationGeneral);
            builder.Append(StringFormat.ToStringAlignment("Classification (Race)      [RACE.IDS]"));
            builder.Append(this.classificationRace);
            builder.Append(StringFormat.ToStringAlignment("Classification (Class)     [CLASS.IDS]"));
            builder.Append(this.classificationClass);
            builder.Append(StringFormat.ToStringAlignment("Classification (Gender)    [GENDER.IDS]"));
            builder.Append(this.classificationGender);
            builder.Append(StringFormat.ToStringAlignment("Classification (Object 1)  [OBJECT.IDS]"));
            builder.Append(this.classificationObject1);
            builder.Append(StringFormat.ToStringAlignment("Classification (Object 2)  [OBJECT.IDS]"));
            builder.Append(this.classificationObject2);
            builder.Append(StringFormat.ToStringAlignment("Classification (Object 3)  [OBJECT.IDS]"));
            builder.Append(this.classificationObject3);
            builder.Append(StringFormat.ToStringAlignment("Classification (Object 4)  [OBJECT.IDS]"));
            builder.Append(this.classificationObject4);
            builder.Append(StringFormat.ToStringAlignment("Classification (Object 5)  [OBJECT.IDS]"));
            builder.Append(this.classificationObject5);
            builder.Append(StringFormat.ToStringAlignment("Classification (Alignment) [ALIGNMEN.IDS]"));
            builder.Append(this.classificationAlignment);
        }

        /// <summary>Generates a String representing the Planescape:torment data additions area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringPlanescapeAdditions(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Unknown #1 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown1));
            builder.Append(StringFormat.ToStringAlignment("Zombie disguise"));
            builder.Append(this.zombieDisguise);
            builder.Append(StringFormat.ToStringAlignment("Zombie disguise (description)"));
            builder.Append(this.zombieDisguise == -1 ? "True" : this.zombieDisguise == 0 ? "False" : "Unknown");
            builder.Append(StringFormat.ToStringAlignment("Unknown #2 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown2));
            builder.Append(StringFormat.ToStringAlignment("Overlays offset"));
            builder.Append(this.offsetOverlays);
            builder.Append(StringFormat.ToStringAlignment("Overlays size"));
            builder.Append(this.sizeOverlays);
            builder.Append(StringFormat.ToStringAlignment("Second class XP total"));
            builder.Append(this.experienceSecondClass);
            builder.Append(StringFormat.ToStringAlignment("Third class XP total"));
            builder.Append(this.experienceThirdClass);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #1"));
            builder.Append(this.internalVariable1);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #2"));
            builder.Append(this.internalVariable2);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #3"));
            builder.Append(this.internalVariable3);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #4"));
            builder.Append(this.internalVariable4);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #5"));
            builder.Append(this.internalVariable5);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #6"));
            builder.Append(this.internalVariable6);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #7"));
            builder.Append(this.internalVariable7);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #8"));
            builder.Append(this.internalVariable8);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #9"));
            builder.Append(this.internalVariable9);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #10"));
            builder.Append(this.internalVariable10);
            builder.Append(StringFormat.ToStringAlignment("GOOD variable increment value"));
            builder.Append(this.variableIncrementGood);
            builder.Append(StringFormat.ToStringAlignment("LAW variable increment value"));
            builder.Append(this.variableIncrementLaw);
            builder.Append(StringFormat.ToStringAlignment("LADY variable increment value"));
            builder.Append(this.variableIncrementLady);
            builder.Append(StringFormat.ToStringAlignment("MURDER variable increment value"));
            builder.Append(this.variableIncrementMurder);
            builder.Append(StringFormat.ToStringAlignment("Monstrous compendium entry"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.MonstrousCompendiumEntry.Value)));
            builder.Append(StringFormat.ToStringAlignment("Dialog activation range"));
            builder.Append(this.dialogActivationRange);
            builder.Append(StringFormat.ToStringAlignment("Selection circle size"));
            builder.Append(this.selectionCircleSize);
            builder.Append(StringFormat.ToStringAlignment("Colors count"));
            builder.Append(this.countColors);
            builder.Append(StringFormat.ToStringAlignment("Attribute Flags"));
            builder.Append((UInt32)this.attributeFlags);
            builder.Append(StringFormat.ToStringAlignment("Attribute Flags (enumerated)"));
            builder.Append(this.GetCreatureAttributeFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Color #1"));
            builder.Append(this.color1);
            builder.Append(StringFormat.ToStringAlignment("Color #2"));
            builder.Append(this.color2);
            builder.Append(StringFormat.ToStringAlignment("Color #3"));
            builder.Append(this.color3);
            builder.Append(StringFormat.ToStringAlignment("Color #4"));
            builder.Append(this.color4);
            builder.Append(StringFormat.ToStringAlignment("Color #5"));
            builder.Append(this.color5);
            builder.Append(StringFormat.ToStringAlignment("Color #6"));
            builder.Append(this.color6);
            builder.Append(StringFormat.ToStringAlignment("Color #7"));
            builder.Append(this.color7);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown3));
            builder.Append(StringFormat.ToStringAlignment("Color placement #1"));
            builder.Append(this.colorPlacement1);
            builder.Append(StringFormat.ToStringAlignment("Color placement #2"));
            builder.Append(this.colorPlacement2);
            builder.Append(StringFormat.ToStringAlignment("Color placement #3"));
            builder.Append(this.colorPlacement3);
            builder.Append(StringFormat.ToStringAlignment("Color placement #4"));
            builder.Append(this.colorPlacement4);
            builder.Append(StringFormat.ToStringAlignment("Color placement #5"));
            builder.Append(this.colorPlacement5);
            builder.Append(StringFormat.ToStringAlignment("Color placement #6"));
            builder.Append(this.colorPlacement6);
            builder.Append(StringFormat.ToStringAlignment("Color placement #7"));
            builder.Append(this.colorPlacement7);
            builder.Append(StringFormat.ToStringAlignment("Unknown #4 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown4));
            builder.Append(StringFormat.ToStringAlignment("Unknown #5"));
            builder.Append(this.unknown5);
            builder.Append(StringFormat.ToStringAlignment("Unknown #6"));
            builder.Append(this.unknown6);
            builder.Append(StringFormat.ToStringAlignment("Unknown #7"));
            builder.Append(this.unknown7);
        }
        #endregion
    }
}