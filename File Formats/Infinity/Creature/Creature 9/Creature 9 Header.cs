using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature9
{
    /// <summary>Creature header version 9</summary>
    public class Creature9Header : Creature2eHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 828;

        #region Members
        /// <summary>Large sword proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyLargeSword;

        /// <summary>Small swords proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencySmallSword;

        /// <summary>Spear proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencySpear;

        /// <summary>Missile proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyMissile;

        /// <summary>Missile proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyGreatSword;

        /// <summary>Blunt proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyDagger;

        /// <summary>Spiked proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyHalberd;

        /// <summary>Mace proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyMace;

        /// <summary>Flail proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyFlail;

        /// <summary>Hammer proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyHammer;

        /// <summary>Club proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyClub;

        /// <summary>Quarterstaff proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyQuarterstaff;

        /// <summary>Crossbow proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyCrossbow;

        //converges back to main 2E structs, until after the script blocks, inserting further IWD-fork variables

        //this looks like a union of UINT and 4 boolean variables, honestly

        /// <summary>Indicates whether or not the creature is visible</summary>
        protected Boolean visible;

        /// <summary>Sets the _DEAD variable on death</summary>
        /// <remarks>...BEEF</remarks>
        protected Boolean setVariableDEAD;

        /// <summary>"Sets" (increments?) the KILL_&lt;scriptname&gt;_CNT variable on death</summary>
        protected Boolean setVariableKILL_CNT;

        /// <summary>Unknown fourth byte that follows the preceeding three boolean flags above</summary>
        protected Byte unknown1;

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

        /// <summary>Unknown two bytes that follows the preceeding extra death variables</summary>
        protected UInt16 unknown2;

        /// <summary>Save X coordinate</summary>
        protected UInt16 savedCoordinateX;

        /// <summary>Save Y coordinate</summary>
        protected UInt16 savedCoordinateY;

        /// <summary>Save orientation</summary>
        /// <value>0-15?</value>
        protected UInt16 savedOrientation;

        /// <summary>Unknown 18 bytes that follow the preceeding coordinate data</summary>
        protected Byte[] unknown3;
        #endregion

        #region Properties
        /// <summary>Large sword proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyLargeSword
        {
            get { return this.proficiencyLargeSword; }
            set { this.proficiencyLargeSword = value; }
        }

        /// <summary>Small swords proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencySmallSword
        {
            get { return this.proficiencySmallSword; }
            set { this.proficiencySmallSword = value; }
        }

        /// <summary>Spear proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencySpear
        {
            get { return this.proficiencySpear; }
            set { this.proficiencySpear = value; }
        }

        /// <summary>Missile proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyMissile
        {
            get { return this.proficiencyMissile; }
            set { this.proficiencyMissile = value; }
        }

        /// <summary>Missile proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyGreatSword
        {
            get { return this.proficiencyGreatSword; }
            set { this.proficiencyGreatSword = value; }
        }

        /// <summary>Blunt proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyDagger
        {
            get { return this.proficiencyDagger; }
            set { this.proficiencyDagger = value; }
        }

        /// <summary>Spiked proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyHalberd
        {
            get { return this.proficiencyHalberd; }
            set { this.proficiencyHalberd = value; }
        }

        /// <summary>Mace proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyMace
        {
            get { return this.proficiencyMace; }
            set { this.proficiencyMace = value; }
        }

        /// <summary>Flail proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyFlail
        {
            get { return this.proficiencyFlail; }
            set { this.proficiencyFlail = value; }
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

        /// <summary>Quarterstaff proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyQuarterstaff
        {
            get { return this.proficiencyQuarterstaff; }
            set { this.proficiencyQuarterstaff = value; }
        }

        /// <summary>Crossbow proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyCrossbow
        {
            get { return this.proficiencyCrossbow; }
            set { this.proficiencyCrossbow = value; }
        }

        /// <summary>Indicates whether or not the creature is visible</summary>
        public Boolean Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        /// <summary>Sets the _DEAD variable on death</summary>
        /// <remarks>...BEEF</remarks>
        public Boolean SetVariableDEAD
        {
            get { return this.setVariableDEAD; }
            set { this.setVariableDEAD = value; }
        }

        /// <summary>"Sets" (increments?) the KILL_&lt;scriptname&gt;_CNT variable on death</summary>
        public Boolean SetVariableKILL_CNT
        {
            get { return this.setVariableKILL_CNT; }
            set { this.setVariableKILL_CNT = value; }
        }

        /// <summary>Unknown fourth byte that follows the preceeding three boolean flags above</summary>
        public Byte Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
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

        /// <summary>Secondary death variable</summary>
        /// <remarks>set to 1 on death</remarks>
        public ZString SecondaryDeathVariable { get; set; }

        /// <summary>Tertiary death variable</summary>
        /// <remarks>incremented by 1 on death</remarks>
        public ZString TertiaryDeathVariable { get; set; }

        /// <summary>Unknown two bytes that follows the preceeding extra death variables</summary>
        public UInt16 Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Save X coordinate</summary>
        public UInt16 SavedCoordinateX
        {
            get { return this.savedCoordinateX; }
            set { this.savedCoordinateX = value; }
        }

        /// <summary>Save Y coordinate</summary>
        public UInt16 SavedCoordinateY
        {
            get { return this.savedCoordinateY; }
            set { this.savedCoordinateY = value; }
        }

        /// <summary>Save orientation</summary>
        /// <value>0-15?</value>
        public UInt16 SavedOrientation
        {
            get { return this.savedOrientation; }
            set { this.savedOrientation = value; }
        }

        /// <summary>Unknown 18 bytes that follow the preceeding coordinate data</summary>
        public Byte[] Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Creature9Header()
        {
            this.Initialize();
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.unknown3 = new Byte[18];
            this.SecondaryDeathVariable = new ZString();
            this.TertiaryDeathVariable = new ZString();
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
            /* 59 */ this.soundSet.Add("RESPONSE_TO_INSULT2", new StringReference());
            /* 60 */ this.soundSet.Add("RESPONSE_TO_INSULT3", new StringReference());
            /* 61 */ this.soundSet.Add("DIALOG_HOSTILE", new StringReference());
            /* 62 */ this.soundSet.Add("DIALOG_DEFAULT", new StringReference());
            /* 63 */ this.soundSet.Add("Unknown63", new StringReference());
            /* 64 */ this.soundSet.Add("Unknown64", new StringReference());
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
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 820);
            this.ReadBodyLeadingValues(remainingBody);
            this.ReadBodyProficiencies(remainingBody);
            this.ReadSoundSet(remainingBody, 156);
            this.ReadBodyCommonAfterSoundset(remainingBody);
            this.ReadBodyIcewindDaleAdditions(remainingBody);
            this.ReadBodyClassifications(remainingBody);
            this.ReadBodyFooter(remainingBody, 732);
        }

        /// <summary>Reads the proficiencies section of the header</summary>
        /// <param name="remainingBody">Byte array to read from</param>
        protected override void ReadBodyProficiencies(Byte[] remainingBody)
        {
            this.proficiencyLargeSword = remainingBody[102];
            this.proficiencySmallSword = remainingBody[103];
            this.proficiencyBow = remainingBody[104];
            this.proficiencySpear = remainingBody[105];
            this.proficiencyAxe = remainingBody[106];
            this.proficiencyMissile = remainingBody[107];
            this.proficiencyGreatSword = remainingBody[108];
            this.proficiencyDagger = remainingBody[109];
            this.proficiencyHalberd = remainingBody[110];
            this.proficiencyMace = remainingBody[111];
            this.proficiencyFlail = remainingBody[112];
            this.proficiencyHammer = remainingBody[113];
            this.proficiencyClub = remainingBody[114];
            this.proficiencyQuarterstaff = remainingBody[115];
            this.proficiencyCrossbow = remainingBody[116];
            this.proficiencyUnused1 = remainingBody[117];
            this.proficiencyUnused1 = remainingBody[118];
            this.proficiencyUnused3 = remainingBody[119];
            this.proficiencyUnused4 = remainingBody[120];
            this.proficiencyUnused5 = remainingBody[121];
            this.proficiencyUnused6 = remainingBody[122];

            this.tracking = remainingBody[123];
            Array.Copy(remainingBody, 124, this.reservedNonweaponProficiencies, 0, 32);
        }

        /// <summary>Reads Icewind Dale structure additions</summary>
        /// <param name="headerBody">Byte array to read from. Expects to start reading at 616, reading 104 Bytes</param>
        protected void ReadBodyIcewindDaleAdditions(Byte[] headerBody)
        {
            //is this all one Int32, maybe bit-shifted to get the boolean values?
            this.visible = Convert.ToBoolean(headerBody[616]);
            this.setVariableDEAD = Convert.ToBoolean(headerBody[617]);
            this.setVariableKILL_CNT = Convert.ToBoolean(headerBody[618]);
            this.unknown1 = headerBody[619];
            this.internalVariable1 = ReusableIO.ReadInt16FromArray(headerBody, 620);
            this.internalVariable2 = ReusableIO.ReadInt16FromArray(headerBody, 622);
            this.internalVariable3 = ReusableIO.ReadInt16FromArray(headerBody, 624);
            this.internalVariable4 = ReusableIO.ReadInt16FromArray(headerBody, 626);
            this.internalVariable5 = ReusableIO.ReadInt16FromArray(headerBody, 628);
            this.SecondaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 630, Constants.CultureCodeEnglish, 32);
            this.TertiaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 662, Constants.CultureCodeEnglish, 32);
            this.unknown2 = ReusableIO.ReadUInt16FromArray(headerBody, 694);
            this.savedCoordinateX = ReusableIO.ReadUInt16FromArray(headerBody, 696);
            this.savedCoordinateY = ReusableIO.ReadUInt16FromArray(headerBody, 698);
            this.savedOrientation = ReusableIO.ReadUInt16FromArray(headerBody, 700);
            Array.Copy(headerBody, 702, this.unknown3, 0, 18);
        }

        /// <summary>Reads the classification entries from the creature file</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 616, size of at least 628 bytes.</param>
        protected override void ReadBodyClassifications(Byte[] headerBodyArray)
        {
            this.classificationHostility = headerBodyArray[720];
            this.classificationGeneral = headerBodyArray[721];
            this.classificationRace = headerBodyArray[722];
            this.classificationClass = headerBodyArray[723];
            this.classificationSpecific = headerBodyArray[724];
            this.classificationGender = headerBodyArray[725];
            this.classificationObject1 = headerBodyArray[726];
            this.classificationObject2 = headerBodyArray[727];
            this.classificationObject3 = headerBodyArray[728];
            this.classificationObject4 = headerBodyArray[729];
            this.classificationObject5 = headerBodyArray[730];
            this.classificationAlignment = headerBodyArray[731];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteBodyHeader(output);
            this.WriteProficiencies(output);
            this.WriteSoundSet(output);
            this.WriteBodyCommonAfterSoundset(output);
            this.WriteBodyIcewindDaleAdditions(output);
            this.WriteClassification(output);
            this.WriteBodyFooter(output);
        }

        /// <summary>This method writes the proficiency values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteProficiencies(Stream output)
        {
            output.WriteByte(this.proficiencyLargeSword);
            output.WriteByte(this.proficiencySmallSword);
            output.WriteByte(this.ProficiencyBow);
            output.WriteByte(this.proficiencySpear);
            output.WriteByte(this.proficiencyAxe);
            output.WriteByte(this.proficiencyMissile);
            output.WriteByte(this.proficiencyGreatSword);
            output.WriteByte(this.proficiencyDagger);
            output.WriteByte(this.proficiencyHalberd);
            output.WriteByte(this.proficiencyMace);
            output.WriteByte(this.proficiencyFlail);
            output.WriteByte(this.proficiencyHammer);
            output.WriteByte(this.proficiencyClub);
            output.WriteByte(this.proficiencyQuarterstaff);
            output.WriteByte(this.proficiencyCrossbow);
            output.WriteByte(this.proficiencyUnused1);
            output.WriteByte(this.proficiencyUnused2);
            output.WriteByte(this.proficiencyUnused3);
            output.WriteByte(this.proficiencyUnused4);
            output.WriteByte(this.proficiencyUnused5);
            output.WriteByte(this.proficiencyUnused6);
            output.WriteByte(this.tracking);
            output.Write(this.reservedNonweaponProficiencies, 0, 32);
        }
        
        /// <summary>Reads Icewind Dale structure additions</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBodyIcewindDaleAdditions(Stream output)
        {
            //is this all one Int32, maybe bit-shifted to get the boolean values?
            output.WriteByte(Convert.ToByte(this.visible));
            output.WriteByte(Convert.ToByte(this.setVariableDEAD));
            output.WriteByte(Convert.ToByte(this.setVariableKILL_CNT));
            output.WriteByte(this.unknown1);
            ReusableIO.WriteInt16ToStream(this.internalVariable1, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable2, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable3, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable4, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable5, output);
            ReusableIO.WriteStringToStream(this.SecondaryDeathVariable.Source, output, Constants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteStringToStream(this.TertiaryDeathVariable.Source, output, Constants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.unknown2, output);
            ReusableIO.WriteUInt16ToStream(this.savedCoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.savedCoordinateY, output);
            ReusableIO.WriteUInt16ToStream(this.savedOrientation, output);
            output.Write(this.unknown3, 0, 18);
        }
        #endregion

        #region ToString() methods
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
            this.ToStringIcewindDaleAdditions(builder);
            this.ToStringClassifications(builder);
            this.ToStringHeaderFooter(builder);

            return builder.ToString();
        }

        /// <summary>Returns the printable read-friendly version of the creature format</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringCreatureVersion(StringBuilder builder)
        {
            builder.AppendLine("Creature version 9.0 header:");
        }

        /// <summary>Generates a String representing the proficiencies area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected override void ToStringProcifiencies(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Large Sword)"));
            builder.Append(this.proficiencyLargeSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Small Sword)"));
            builder.Append(this.proficiencySmallSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Bow)"));
            builder.Append(this.proficiencyBow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Spear)"));
            builder.Append(this.proficiencySpear);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Axe)"));
            builder.Append(this.proficiencyAxe);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Missile)"));
            builder.Append(this.proficiencyMissile);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Great Sword)"));
            builder.Append(this.proficiencyGreatSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Dagger)"));
            builder.Append(this.proficiencyDagger);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Halberd)"));
            builder.Append(this.proficiencyHalberd);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Mace)"));
            builder.Append(this.proficiencyMace);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Flail)"));
            builder.Append(this.proficiencyFlail);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Hammer)"));
            builder.Append(this.proficiencyHammer);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Club)"));
            builder.Append(this.proficiencyClub);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Quarterstaff)"));
            builder.Append(this.proficiencyQuarterstaff);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Crossbow)"));
            builder.Append(this.proficiencyCrossbow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #1)"));
            builder.Append(this.proficiencyUnused1);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #2)"));
            builder.Append(this.proficiencyUnused2);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #3)"));
            builder.Append(this.proficiencyUnused3);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #4)"));
            builder.Append(this.proficiencyUnused4);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #5)"));
            builder.Append(this.proficiencyUnused5);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Unused #6)"));
            builder.Append(this.proficiencyUnused6);
            builder.Append(StringFormat.ToStringAlignment("Tracking"));
            builder.Append(this.tracking);
            builder.Append(StringFormat.ToStringAlignment("Reserved non-weapon proficiencies"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reservedNonweaponProficiencies));
        }

        /// <summary>Generates a String representing the added Icewind Dale values area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringIcewindDaleAdditions(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Visible"));
            builder.Append(this.visible);
            builder.Append(StringFormat.ToStringAlignment("Set _DEAD variable on death"));
            builder.Append(this.setVariableDEAD);
            builder.Append(StringFormat.ToStringAlignment("Set KILL_<scriptname>_CNT variable on death"));
            builder.Append(this.setVariableKILL_CNT);
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.unknown1);
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
            builder.Append(StringFormat.ToStringAlignment("Secondary Death variable"));
            builder.Append("'");
            builder.Append(this.SecondaryDeathVariable.Value);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Tertiary Death variable"));
            builder.Append("'");
            builder.Append(this.TertiaryDeathVariable.Value);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(this.unknown2);
            builder.Append(StringFormat.ToStringAlignment("Saved X Co-ordinate"));
            builder.Append(this.savedCoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Saved Y Co-ordinate"));
            builder.Append(this.savedCoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Saved Orientation"));
            builder.Append(this.savedOrientation);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown3));
        }
        #endregion
    }
}