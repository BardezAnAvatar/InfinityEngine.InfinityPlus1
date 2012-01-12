using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    /// <summary>Creature header with elements common to 2E structures</summary>
    public abstract class Creature2eHeader : CreatureHeader
    {
        #region Members
        /// <summary>Unified 2E creature flags</summary>
        protected Creature2eFlags flags;

        /// <summary>Effective armor class</summary>
        /// <remarks>
        ///     Is this not superfluous? Most exaples appear to be equal to ArmorClassNatural
        ///     
        ///     ... It will be fun to do some Linq to objects comparison on this field.
        /// </remarks>
        protected Int16 armorClassEffective;

        /// <summary>Saving throw modifier vs. Death</summary>
        protected Byte savingThrowDeath;

        /// <summary>Saving throw modifier vs. Wands</summary>
        protected Byte savingThrowWands;

        /// <summary>Saving throw modifier vs. Polymorph</summary>
        protected Byte savingThrowPolymorph;

        /// <summary>Saving throw modifier vs. Breath attacks</summary>
        protected Byte savingThrowBreathAttacks;

        /// <summary>Saving throw modifier vs. Spells</summary>
        protected Byte savingThrowSpells;

        /// <summary>Detect illusions skill</summary>
        protected Byte detectIllusions;

        /// <summary>Set traps skill</summary>
        /// <remarks>Unused in BG1, IWD, PS:T, no?</remarks>
        protected Byte setTraps;

        /// <summary>Lore skill</summary>
        protected Byte lore;

        /// <summary>Lock picking skill</summary>
        protected Byte lockPicking;

        /// <summary>Stealth skill</summary>
        protected Byte stealth;

        /// <summary>Find/disarm traps skill</summary>
        protected Byte findDisarmTraps;

        /// <summary>Pick pockets skill</summary>
        protected Byte pickPockets;

        //after meeting up for fatigue with base class, we move on to proficiencies. Some of these differ in position,
        //  but can be shared declaratively. I will also include the common X unused proficiencies.

        /// <summary>Bow proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyBow;

        /// <summary>Axe proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyAxe;

        // stupid Baldur's Gate 1 not using proficiencies correctly...

        /// <summary>Unused proficiency #1</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused1;

        /// <summary>Unused proficiency #2</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused2;

        /// <summary>Unused proficiency #3</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused3;

        /// <summary>Unused proficiency #4</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused4;

        /// <summary>Unused proficiency #5</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused5;

        /// <summary>Unused proficiency #6</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        protected Byte proficiencyUnused6;

        /// <summary>Tracking skill</summary>
        protected Byte tracking;

        /// <summary>32-byte range of unused data, probably nonweapon proficiencies</summary>
        protected Byte[] reservedNonweaponProficiencies;

        /// <summary>Current level in first class</summary>
        protected Byte levelClass1;

        /// <summary>Current level in second class</summary>
        protected Byte levelClass2;

        /// <summary>Current level in third class</summary>
        protected Byte levelClass3;

        /// <summary>Gender of the creature</summary>
        /// <remarks>Checkable via the Sex stat in scripts</remarks>
        /// <value>Match to GENDER.IDS</value>
        protected Byte sex;

        /// <summary>Strength % bonus ability score</summary>
        protected Byte scoreStrengthBonus;
        
        /// <summary>Racial enemy</summary>
        /// <value>Match to RACE.IDS</value>
        protected Byte racialEnemy;

        /// <summary>Kit flag</summary>
        /// <remarks>
        ///     Apparently, PS:T breaks this up into 2 shorts, one the kit, the other deity. See Near Infinity.
        ///     
        ///     Since I am lazy, and this is in the middle of already-abstracted code, I think I might just do
        ///     a bit-shift for this value for PS:T -- no, that hurts my sensibility :(
        /// </remarks>
        protected Kit2e kit;

        /// <summary>Class script</summary>
        protected ResourceReference scriptClass;

        /// <summary>Race script</summary>
        protected ResourceReference scriptRace;

        /// <summary>General script</summary>
        protected ResourceReference scriptGeneral;

        /// <summary>Default script</summary>
        protected ResourceReference scriptDefault;

        // Diversion point among the 2E structures.

        /// <summary>Offset to known spells</summary>
        protected UInt32 offsetKnownSpells;

        /// <summary>Count of known spells</summary>
        protected UInt32 countKnownSpells;

        /// <summary>Offset to overlay memorization</summary>
        protected UInt32 offsetSpellMemorization;

        /// <summary>Count of overlay memorization entries</summary>
        protected UInt32 countSpellMemorizations;

        /// <summary>Offset to memorized spells</summary>
        protected UInt32 offsetMemorizedSpells;

        /// <summary>Count of memorized spells</summary>
        protected UInt32 countMemorizedSpells;
        #endregion

        #region Properties
        /// <summary>Unified 2E creature flags</summary>
        public Creature2eFlags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        /// <summary>Effective armor class</summary>
        /// <remarks>
        ///     Is this not superfluous? Most exaples appear to be equal to ArmorClassNatural
        ///     
        ///     ... It will be fun to do some Linq to objects comparison on this field.
        /// </remarks>
        public Int16 ArmorClassEffective
        {
            get { return this.armorClassEffective; }
            set { this.armorClassEffective = value; }
        }

        /// <summary>Saving throw modifier vs. Death</summary>
        public Byte SavingThrowDeath
        {
            get { return this.savingThrowDeath; }
            set { this.savingThrowDeath = value; }
        }

        /// <summary>Saving throw modifier vs. Wands</summary>
        public Byte SavingThrowWands
        {
            get { return this.savingThrowWands; }
            set { this.savingThrowWands = value; }
        }

        /// <summary>Saving throw modifier vs. Polymorph</summary>
        public Byte SavingThrowPolymorph
        {
            get { return this.savingThrowPolymorph; }
            set { this.savingThrowPolymorph = value; }
        }

        /// <summary>Saving throw modifier vs. Breath attacks</summary>
        public Byte SavingThrowBreathAttacks
        {
            get { return this.savingThrowBreathAttacks; }
            set { this.savingThrowBreathAttacks = value; }
        }

        /// <summary>Saving throw modifier vs. Spells</summary>
        public Byte SavingThrowSpells
        {
            get { return this.savingThrowSpells; }
            set { this.savingThrowSpells = value; }
        }

        /// <summary>Detect illusions skill</summary>
        public Byte DetectIllusions
        {
            get { return this.detectIllusions; }
            set { this.detectIllusions = value; }
        }

        /// <summary>Set traps skill</summary>
        /// <remarks>Unused in BG1, IWD, PS:T, no?</remarks>
        public Byte SetTraps
        {
            get { return this.setTraps; }
            set { this.setTraps = value; }
        }

        /// <summary>Lore skill</summary>
        public Byte Lore
        {
            get { return this.lore; }
            set { this.lore = value; }
        }

        /// <summary>Lock picking skill</summary>
        public Byte LockPicking
        {
            get { return this.lockPicking; }
            set { this.lockPicking = value; }
        }

        /// <summary>Stealth skill</summary>
        public Byte Stealth
        {
            get { return this.stealth; }
            set { this.stealth = value; }
        }

        /// <summary>Find/disarm traps skill</summary>
        public Byte FindDisarmTraps
        {
            get { return this.findDisarmTraps; }
            set { this.findDisarmTraps = value; }
        }

        /// <summary>Pick pockets skill</summary>
        public Byte PickPockets
        {
            get { return this.pickPockets; }
            set { this.pickPockets = value; }
        }

        /// <summary>Bow proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyBow
        {
            get { return this.proficiencyBow; }
            set { this.proficiencyBow = value; }
        }

        /// <summary>Axe proficiency</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyAxe
        {
            get { return this.proficiencyAxe; }
            set { this.proficiencyAxe = value; }
        }

        /// <summary>Unused proficiency #1</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused1
        {
            get { return this.proficiencyUnused1; }
            set { this.proficiencyUnused1 = value; }
        }

        /// <summary>Unused proficiency #2</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused2
        {
            get { return this.proficiencyUnused2; }
            set { this.proficiencyUnused2 = value; }
        }

        /// <summary>Unused proficiency #3</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused3
        {
            get { return this.proficiencyUnused3; }
            set { this.proficiencyUnused3 = value; }
        }

        /// <summary>Unused proficiency #4</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused4
        {
            get { return this.proficiencyUnused4; }
            set { this.proficiencyUnused4 = value; }
        }

        /// <summary>Unused proficiency #5</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused5
        {
            get { return this.proficiencyUnused5; }
            set { this.proficiencyUnused5 = value; }
        }

        /// <summary>Unused proficiency #6</summary>
        /// <remarks>Split into 3-bit groups for multiple class proficiencies</remarks>
        public Byte ProficiencyUnused6
        {
            get { return this.proficiencyUnused6; }
            set { this.proficiencyUnused6 = value; }
        }

        /// <summary>Tracking skill</summary>
        public Byte Tracking
        {
            get { return this.tracking; }
            set { this.tracking = value; }
        }

        /// <summary>32-byte range of unused data, probably nonweapon proficiencies</summary>
        public Byte[] ReservedNonweaponProficiencies
        {
            get { return this.reservedNonweaponProficiencies; }
            set { this.reservedNonweaponProficiencies = value; }
        }

        /// <summary>Current level in first class</summary>
        public Byte LevelClass1
        {
            get { return this.levelClass1; }
            set { this.levelClass1 = value; }
        }

        /// <summary>Current level in second class</summary>
        public Byte LevelClass2
        {
            get { return this.levelClass2; }
            set { this.levelClass2 = value; }
        }

        /// <summary>Current level in third class</summary>
        public Byte LevelClass3
        {
            get { return this.levelClass3; }
            set { this.levelClass3 = value; }
        }

        /// <summary>Gender of the creature</summary>
        /// <remarks>Checkable via the Sex stat in scripts</remarks>
        /// <value>Match to GENDER.IDS</value>
        public Byte Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }

        /// <summary>Strength % bonus ability score</summary>
        public Byte ScoreStrengthBonus
        {
            get { return this.scoreStrengthBonus; }
            set { this.scoreStrengthBonus = value; }
        }

        /// <summary>Racial enemy</summary>
        /// <value>Match to RACE.IDS</value>
        public Byte RacialEnemy
        {
            get { return this.racialEnemy; }
            set { this.racialEnemy = value; }
        }

        /// <summary>Kit flag</summary>
        public Kit2e Kit
        {
            get { return this.kit; }
            set { this.kit = value; }
        }

        /// <summary>Class script</summary>
        public ResourceReference ScriptClass
        {
            get { return this.scriptClass; }
            set { this.scriptClass = value; }
        }

        /// <summary>Race script</summary>
        public ResourceReference ScriptRace
        {
            get { return this.scriptRace; }
            set { this.scriptRace = value; }
        }

        /// <summary>General script</summary>
        public ResourceReference ScriptGeneral
        {
            get { return this.scriptGeneral; }
            set { this.scriptGeneral = value; }
        }

        /// <summary>Default script</summary>
        public ResourceReference ScriptDefault
        {
            get { return this.scriptDefault; }
            set { this.scriptDefault = value; }
        }

        /// <summary>Offset to known spells</summary>
        public UInt32 OffsetKnownSpells
        {
            get { return this.offsetKnownSpells; }
            set { this.offsetKnownSpells = value; }
        }

        /// <summary>Count of known spells</summary>
        public UInt32 CountKnownSpells
        {
            get { return this.countKnownSpells; }
            set { this.countKnownSpells = value; }
        }

        /// <summary>Offset to overlay memorization</summary>
        public UInt32 OffsetSpellMemorization
        {
            get { return this.offsetSpellMemorization; }
            set { this.offsetSpellMemorization = value; }
        }

        /// <summary>Count of overlay memorization entries</summary>
        public UInt32 CountSpellMemorizations
        {
            get { return this.countSpellMemorizations; }
            set { this.countSpellMemorizations = value; }
        }

        /// <summary>Offset to memorized spells</summary>
        public UInt32 OffsetMemorizedSpells
        {
            get { return this.offsetMemorizedSpells; }
            set { this.offsetMemorizedSpells = value; }
        }

        /// <summary>Count of memorized spells</summary>
        public UInt32 CountMemorizedSpells
        {
            get { return this.countMemorizedSpells; }
            set { this.countMemorizedSpells = value; }
        }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.reservedNonweaponProficiencies = new Byte[32];
            this.scriptClass = new ResourceReference();
            this.scriptRace = new ResourceReference();
            this.scriptGeneral = new ResourceReference();
            this.scriptDefault = new ResourceReference();
        }

        /// <summary>Initializes the soundset ordered dictionary</summary>
        protected override void InitializeSoundSet()
        {
            /* 00 */ this.soundSet.Add("MORALE_FAILURE_1", new StringReference());
            /* 01 */ this.soundSet.Add("MORALE_FAILURE_2", new StringReference());
            /* 02 */ this.soundSet.Add("BATTLE_CRY_1", new StringReference());
            /* 03 */ this.soundSet.Add("BATTLE_CRY_2", new StringReference());
            /* 04 */ this.soundSet.Add("BATTLE_CRY_3", new StringReference());
            /* 05 */ this.soundSet.Add("BATTLE_CRY_4", new StringReference());
            /* 06 */ this.soundSet.Add("BATTLE_CRY_5", new StringReference());
            /* 07 */ this.soundSet.Add("BECOMING_LEADER_1", new StringReference());
            /* 08 */ this.soundSet.Add("BECOMING_LEADER_2", new StringReference());
            /* 09 */ this.soundSet.Add("TIRED_1", new StringReference());
            /* 10 */ this.soundSet.Add("TIRED_2", new StringReference());
            /* 11 */ this.soundSet.Add("BORED_1", new StringReference());
            /* 12 */ this.soundSet.Add("BORED_2", new StringReference());
            /* 13 */ this.soundSet.Add("HURT_1", new StringReference());
            /* 14 */ this.soundSet.Add("HURT_2", new StringReference());
            /* 15 */ this.soundSet.Add("SELECTED_COMMON_1", new StringReference());
            /* 16 */ this.soundSet.Add("SELECTED_COMMON_2", new StringReference());
            /* 17 */ this.soundSet.Add("SELECTED_COMMON_3", new StringReference());
            /* 18 */ this.soundSet.Add("SELECTED_COMMON_4", new StringReference());
            /* 19 */ this.soundSet.Add("SELECTED_COMMON_5", new StringReference());
            /* 20 */ this.soundSet.Add("SELECTED_COMMON_6", new StringReference());
            /* 21 */ this.soundSet.Add("SELECTED_COMMON_7", new StringReference());
            /* 22 */ this.soundSet.Add("SELECTED_ACTION_1", new StringReference());
            /* 23 */ this.soundSet.Add("SELECTED_ACTION_2", new StringReference());
            /* 24 */ this.soundSet.Add("SELECTED_ACTION_3", new StringReference());
            /* 25 */ this.soundSet.Add("SELECTED_ACTION_4", new StringReference());
            /* 26 */ this.soundSet.Add("SELECTED_ACTION_5", new StringReference());
            /* 27 */ this.soundSet.Add("SELECTED_ACTION_6", new StringReference());
            /* 28 */ this.soundSet.Add("SELECTED_ACTION_7", new StringReference());
            /* 29 */ this.soundSet.Add("SELECTED_RARE_1", new StringReference());
            /* 30 */ this.soundSet.Add("SELECTED_RARE_2", new StringReference());
            /* 31 */ this.soundSet.Add("SELECTED_RARE_3", new StringReference());
            /* 32 */ this.soundSet.Add("SELECTED_RARE_4", new StringReference());
            /* 33 */ this.soundSet.Add("BEING_HIT_1", new StringReference());
            /* 34 */ this.soundSet.Add("BEING_HIT_2", new StringReference());
            /* 35 */ this.soundSet.Add("BEING_HIT_3", new StringReference());
            /* 36 */ this.soundSet.Add("DYING_1", new StringReference());
            /* 37 */ this.soundSet.Add("DYING_2", new StringReference());
            /* 38 */ this.soundSet.Add("REACTION_TO_PARTYMEMBER_DYING_1", new StringReference());
            /* 39 */ this.soundSet.Add("REACTION_TO_PARTYMEMBER_DYING_2", new StringReference());
            /* 40 */ this.soundSet.Add("CRITICAL_HIT_1", new StringReference());
            /* 41 */ this.soundSet.Add("CRITICAL_HIT_2", new StringReference());
            /* 42 */ this.soundSet.Add("FALLING", new StringReference());
            /* 43 */ this.soundSet.Add("Unknown43", new StringReference());
            /* 44 */ this.soundSet.Add("Unknown44", new StringReference());
            /* 45 */ this.soundSet.Add("Unknown45", new StringReference());
            /* 46 */ this.soundSet.Add("Unknown46", new StringReference());
            /* 47 */ this.soundSet.Add("Unknown47", new StringReference());
            /* 48 */ this.soundSet.Add("Unknown48", new StringReference());
            /* 49 */ this.soundSet.Add("Unknown49", new StringReference());
            /* 50 */ this.soundSet.Add("Unknown50", new StringReference());
            /* 51 */ this.soundSet.Add("Unknown51", new StringReference());
            /* 52 */ this.soundSet.Add("Unknown52", new StringReference());
            /* 53 */ this.soundSet.Add("Unknown53", new StringReference());
            /* 54 */ this.soundSet.Add("Unknown54", new StringReference());
            /* 55 */ this.soundSet.Add("Unknown55", new StringReference());
            /* 56 */ this.soundSet.Add("Unknown56", new StringReference());
            /* 57 */ this.soundSet.Add("Unknown57", new StringReference());
            /* 58 */ this.soundSet.Add("Unknown58", new StringReference());
            /* 59 */ this.soundSet.Add("Unknown59", new StringReference());
            /* 60 */ this.soundSet.Add("Unknown60", new StringReference());
            /* 61 */ this.soundSet.Add("Unknown61", new StringReference());
            /* 62 */ this.soundSet.Add("Unknown62", new StringReference());
            /* 63 */ this.soundSet.Add("Unknown63", new StringReference());
            /* 64 */ this.soundSet.Add("Unknown64", new StringReference());
        }
        #endregion

        #region IO method implemetations
        /// <summary>Reads the leading 102 bytes from the Creature 2E file</summary>
        /// <param name="remainingBody">Byte array to read from. Expected to be reading from index 0, size of at least 102 bytes.</param>
        protected void ReadBodyLeadingValues(Byte[] remainingBody)
        {
            this.nameLong.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameShort.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.flags = (Creature2eFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 8);
            this.experienceValue = ReusableIO.ReadUInt32FromArray(remainingBody, 12);
            this.experienceTotal = ReusableIO.ReadUInt32FromArray(remainingBody, 16);
            this.gold = ReusableIO.ReadUInt32FromArray(remainingBody, 20);
            this.statusFlags = ReusableIO.ReadUInt32FromArray(remainingBody, 24);
            this.hitPointsCurrent = ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.hitPointsMaximum = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.animationId = ReusableIO.ReadUInt32FromArray(remainingBody, 32);
            this.colorIndexMetal = remainingBody[36];
            this.colorIndexMinor = remainingBody[37];
            this.colorIndexMajor = remainingBody[38];
            this.colorIndexSkin = remainingBody[39];
            this.colorIndexLeather = remainingBody[40];
            this.colorIndexArmor = remainingBody[41];
            this.colorIndexHair = remainingBody[42];
            this.useEffectStructureVersion2 = Convert.ToBoolean(remainingBody[43]);
            this.portraitSmall.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 44, CultureConstants.CultureCodeEnglish);
            this.portraitLarge.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 52, CultureConstants.CultureCodeEnglish);
            this.reputation = remainingBody[60];
            this.hideInShadows = remainingBody[61];
            this.armorClassNatural = ReusableIO.ReadInt16FromArray(remainingBody, 62);
            this.armorClassEffective = ReusableIO.ReadInt16FromArray(remainingBody, 64);
            this.armorClassModifierCrushing = ReusableIO.ReadInt16FromArray(remainingBody, 66);
            this.armorClassModifierMissile = ReusableIO.ReadInt16FromArray(remainingBody, 68);
            this.armorClassModifierPiercing = ReusableIO.ReadInt16FromArray(remainingBody, 70);
            this.armorClassModifierSlashing = ReusableIO.ReadInt16FromArray(remainingBody, 72);
            this.attackBase = remainingBody[74];
            this.attacksPerRound = remainingBody[75];
            this.savingThrowDeath = remainingBody[76];
            this.savingThrowWands = remainingBody[77];
            this.savingThrowPolymorph = remainingBody[78];
            this.savingThrowBreathAttacks = remainingBody[79];
            this.savingThrowSpells = remainingBody[80];
            this.resistFire = remainingBody[81];
            this.resistCold = remainingBody[82];
            this.resistElectricity = remainingBody[83];
            this.resistAcid = remainingBody[84];
            this.resistMagic = remainingBody[85];
            this.resistFireMagic = remainingBody[86];
            this.resistColdMagic = remainingBody[87];
            this.resistPhysicalSlashing = remainingBody[88];
            this.resistPhysicalCrushing = remainingBody[89];
            this.resistPhysicalPiercing = remainingBody[90];
            this.resistPhysicalMissile = remainingBody[91];
            this.detectIllusions = remainingBody[92];
            this.setTraps = remainingBody[93];
            this.lore = remainingBody[94];
            this.lockPicking = remainingBody[95];
            this.stealth = remainingBody[96];
            this.findDisarmTraps = remainingBody[97];
            this.pickPockets = remainingBody[98];
            this.fatigue = remainingBody[99];
            this.intoxication = remainingBody[100];
            this.luck = remainingBody[101];
        }
                
        /// <summary>Reads the proficiencies section of the header</summary>
        /// <param name="remainingBody">Byte array to read from</param>
        protected abstract void ReadBodyProficiencies(Byte[] remainingBody);

        /// <summary>Reads all of the soundset variables from the byte array</summary>
        /// <param name="body">Byte Array to read from</param>
        /// <param name="bodyOffset">Offset in the byte array to start reading from</param>
        protected void ReadSoundSet(Byte[] body, Int32 bodyOffset)
        {
            for (Int32 index = 0; index < 100; ++index)
                this.soundSet[index].StringReferenceIndex = ReusableIO.ReadInt32FromArray(body, (index * 4) + bodyOffset);
        }

        /// <summary>Reads the common Creature 2E elements after the soundset entries</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 556, size of at least 616 bytes.</param>
        protected void ReadBodyCommonAfterSoundset(Byte[] remainingBody)
        {
            this.levelClass1 = remainingBody[556];
            this.levelClass2 = remainingBody[557];
            this.levelClass3 = remainingBody[558];
            this.sex = remainingBody[559];
            this.scoreStrength = remainingBody[560];
            this.scoreStrengthBonus = remainingBody[561];
            this.scoreIntelligence = remainingBody[562];
            this.scoreWisdom = remainingBody[563];
            this.scoreDexterity = remainingBody[564];
            this.scoreConstitution = remainingBody[565];
            this.scoreCharisma = remainingBody[566];
            this.morale = remainingBody[567];
            this.moraleBreak = remainingBody[568];
            this.racialEnemy = remainingBody[569];
            this.moraleRecoveryTime = ReusableIO.ReadUInt16FromArray(remainingBody, 570);
            this.ReadKitValues(remainingBody);
            this.scriptOverride.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 576, CultureConstants.CultureCodeEnglish);
            this.scriptClass.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 584, CultureConstants.CultureCodeEnglish);
            this.scriptRace.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 592, CultureConstants.CultureCodeEnglish);
            this.scriptGeneral.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 600, CultureConstants.CultureCodeEnglish);
            this.scriptDefault.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 608, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>This method will read the kit variable to the output Stream</summary>
        /// <param name="dataArray">Byte array to read from. Expected to be reading from index 572 and reading 4 bytes.</param>
        protected virtual void ReadKitValues(Byte[] dataArray)
        {
            this.kit = (Kit2e)ReusableIO.ReadUInt32FromArray(dataArray, 572);
        }
        
        /// <summary>Reads the classification entries from the creature file</summary>
        /// <param name="headerBodyArray">Byte array to read from.</param>
        protected abstract void ReadBodyClassifications(Byte[] headerBodyArray);

        /// <summary>Reads the header's footer, containing al the offsets, death variable, and enumerators</summary>
        /// <param name="remainingHeaderArray">Byte array to read from</param>
        /// <param name="offset">Starting index in remainingHeaderArray to read from</param>
        protected void ReadBodyFooter(Byte[] remainingHeaderArray, Int32 offset)
        {
            this.enumGlobal = ReusableIO.ReadInt16FromArray(remainingHeaderArray, offset);
            this.enumLocal = ReusableIO.ReadInt16FromArray(remainingHeaderArray, offset + 2);
            this.deathVariable.Source = ReusableIO.ReadStringFromByteArray(remainingHeaderArray, offset + 4, CultureConstants.CultureCodeEnglish, 32);
            this.offsetKnownSpells = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 36);
            this.countKnownSpells = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 40);
            this.offsetSpellMemorization = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 44);
            this.countSpellMemorizations = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 48);
            this.offsetMemorizedSpells = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 52);
            this.countMemorizedSpells = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 56);
            this.offsetItemSlots = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 60);
            this.offsetItems = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 64);
            this.countItems = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 68);
            this.offsetEffects = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 72);
            this.countEffects = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, offset + 76);
            this.dialog.ResRef = ReusableIO.ReadStringFromByteArray(remainingHeaderArray, offset + 80, CultureConstants.CultureCodeEnglish);
        }
        
        /// <summary>This method writes the common shared 2E header's header values.</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBodyHeader(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteInt32ToStream(this.nameLong.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.nameShort.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.flags, output);
            ReusableIO.WriteUInt32ToStream(this.experienceValue, output);
            ReusableIO.WriteUInt32ToStream(this.experienceTotal, output);
            ReusableIO.WriteUInt32ToStream(this.gold, output);
            ReusableIO.WriteUInt32ToStream(this.statusFlags, output);
            ReusableIO.WriteUInt16ToStream(this.hitPointsCurrent, output);
            ReusableIO.WriteUInt16ToStream(this.hitPointsMaximum, output);
            ReusableIO.WriteUInt32ToStream(this.animationId, output);
            output.WriteByte(this.colorIndexMetal);
            output.WriteByte(this.colorIndexMinor);
            output.WriteByte(this.colorIndexMajor);
            output.WriteByte(this.colorIndexSkin);
            output.WriteByte(this.colorIndexLeather);
            output.WriteByte(this.colorIndexArmor);
            output.WriteByte(this.colorIndexHair);
            output.WriteByte(Convert.ToByte(this.useEffectStructureVersion2));
            ReusableIO.WriteStringToStream(this.portraitSmall.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.portraitLarge.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.WriteByte(this.reputation);
            output.WriteByte(this.hideInShadows);
            ReusableIO.WriteInt16ToStream(this.armorClassNatural, output);
            ReusableIO.WriteInt16ToStream(this.armorClassEffective, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierCrushing, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierMissile, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierPiercing, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierSlashing, output);
            output.WriteByte(this.attackBase);
            output.WriteByte(this.attacksPerRound);
            output.WriteByte(this.savingThrowDeath);
            output.WriteByte(this.savingThrowWands);
            output.WriteByte(this.savingThrowPolymorph);
            output.WriteByte(this.savingThrowBreathAttacks);
            output.WriteByte(this.savingThrowSpells);
            output.WriteByte(this.resistFire);
            output.WriteByte(this.resistCold);
            output.WriteByte(this.resistElectricity);
            output.WriteByte(this.resistAcid);
            output.WriteByte(this.resistMagic);
            output.WriteByte(this.resistFireMagic);
            output.WriteByte(this.resistColdMagic);
            output.WriteByte(this.resistPhysicalSlashing);
            output.WriteByte(this.resistPhysicalCrushing);
            output.WriteByte(this.resistPhysicalPiercing);
            output.WriteByte(this.resistPhysicalMissile);
            output.WriteByte(this.detectIllusions);
            output.WriteByte(this.setTraps);
            output.WriteByte(this.lore);
            output.WriteByte(this.lockPicking);
            output.WriteByte(this.stealth);
            output.WriteByte(this.findDisarmTraps);
            output.WriteByte(this.pickPockets);
            output.WriteByte(this.fatigue);
            output.WriteByte(this.intoxication);
            output.WriteByte(this.luck);
        }

        /// <summary>This method writes the proficiency values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected abstract void WriteProficiencies(Stream output);

        /// <summary>Writes the sound set to the output stream</summary>
        /// <param name="output">">Output stream to write to</param>
        protected void WriteSoundSet(Stream output)
        {
            for (Int32 i = 0; i < 100; ++i) //i < this.soundSet.Count
                ReusableIO.WriteInt32ToStream(this.soundSet[i].StringReferenceIndex, output);
        }

        /// <summary>Writes common 2E creature structure values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBodyCommonAfterSoundset(Stream output)
        {
            output.WriteByte(this.levelClass1);
            output.WriteByte(this.levelClass2);
            output.WriteByte(this.levelClass3);
            output.WriteByte(this.sex);
            output.WriteByte(this.scoreStrength);
            output.WriteByte(this.scoreStrengthBonus);
            output.WriteByte(this.scoreIntelligence);
            output.WriteByte(this.scoreWisdom);
            output.WriteByte(this.scoreDexterity);
            output.WriteByte(this.scoreConstitution);
            output.WriteByte(this.scoreCharisma);
            output.WriteByte(this.morale);
            output.WriteByte(this.moraleBreak);
            output.WriteByte(this.racialEnemy);
            ReusableIO.WriteUInt16ToStream(this.moraleRecoveryTime, output);
            this.WriteKitValues(output);
            ReusableIO.WriteStringToStream(this.scriptOverride.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptClass.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptRace.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptGeneral.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptDefault.ResRef, output, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>This method will write out the kit variable to the output Stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected virtual void WriteKitValues(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.kit, output);
        }

        /// <summary>This method writes the classification values to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected virtual void WriteClassification(Stream output)
        {
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

        /// <summary>This method writes common elements of the header's footer to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBodyFooter(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.enumGlobal, output);
            ReusableIO.WriteInt16ToStream(this.enumLocal, output);
            ReusableIO.WriteStringToStream(this.deathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.offsetKnownSpells, output);
            ReusableIO.WriteUInt32ToStream(this.countKnownSpells, output);
            ReusableIO.WriteUInt32ToStream(this.offsetSpellMemorization, output);
            ReusableIO.WriteUInt32ToStream(this.countSpellMemorizations, output);
            ReusableIO.WriteUInt32ToStream(this.offsetMemorizedSpells, output);
            ReusableIO.WriteUInt32ToStream(this.countMemorizedSpells, output);
            ReusableIO.WriteUInt32ToStream(this.offsetItemSlots, output);
            ReusableIO.WriteUInt32ToStream(this.offsetItems, output);
            ReusableIO.WriteUInt32ToStream(this.countItems, output);
            ReusableIO.WriteUInt32ToStream(this.offsetEffects, output);
            ReusableIO.WriteUInt32ToStream(this.countEffects, output);
            ReusableIO.WriteStringToStream(this.dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
        }
        #endregion

        #region ToString() helpers
        /// <summary>Generates a human-readable multi-line string for console output that indicates which Creature2eFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetCreatureFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.ShowLongName) == Creature2eFlags.ShowLongName, Creature2eFlags.ShowLongName.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.NoCorpse) == Creature2eFlags.NoCorpse, Creature2eFlags.NoCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.PermanentCorpse) == Creature2eFlags.PermanentCorpse, Creature2eFlags.PermanentCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassFighter) == Creature2eFlags.OriginalClassFighter, Creature2eFlags.OriginalClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassMage) == Creature2eFlags.OriginalClassMage, Creature2eFlags.OriginalClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassCleric) == Creature2eFlags.OriginalClassCleric, Creature2eFlags.OriginalClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassThief) == Creature2eFlags.OriginalClassThief, Creature2eFlags.OriginalClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassDruid) == Creature2eFlags.OriginalClassDruid, Creature2eFlags.OriginalClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.OriginalClassRanger) == Creature2eFlags.OriginalClassRanger, Creature2eFlags.OriginalClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.FallenPaladin) == Creature2eFlags.FallenPaladin, Creature2eFlags.FallenPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.FallenRanger) == Creature2eFlags.FallenRanger, Creature2eFlags.FallenRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.Exportable) == Creature2eFlags.Exportable, Creature2eFlags.Exportable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.HideInjuryStatusInTooltip) == Creature2eFlags.HideInjuryStatusInTooltip, Creature2eFlags.HideInjuryStatusInTooltip.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.QuestCritical) == Creature2eFlags.QuestCritical, Creature2eFlags.QuestCritical.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.ActivatesCannotBeUsedByNpcs) == Creature2eFlags.ActivatesCannotBeUsedByNpcs, Creature2eFlags.ActivatesCannotBeUsedByNpcs.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.BeenInParty) == Creature2eFlags.BeenInParty, Creature2eFlags.BeenInParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RestoreItemInHand) == Creature2eFlags.RestoreItemInHand, Creature2eFlags.RestoreItemInHand.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.UnsetRestoreItemInHandFlag) == Creature2eFlags.UnsetRestoreItemInHandFlag, Creature2eFlags.UnsetRestoreItemInHandFlag.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkEnemyAlly) == Creature2eFlags.RandomWalkEnemyAlly, Creature2eFlags.RandomWalkEnemyAlly.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkGeneral) == Creature2eFlags.RandomWalkGeneral, Creature2eFlags.RandomWalkGeneral.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkRace) == Creature2eFlags.RandomWalkRace, Creature2eFlags.RandomWalkRace.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkClass) == Creature2eFlags.RandomWalkClass, Creature2eFlags.RandomWalkClass.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkSpecific) == Creature2eFlags.RandomWalkSpecific, Creature2eFlags.RandomWalkSpecific.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkGender) == Creature2eFlags.RandomWalkGender, Creature2eFlags.RandomWalkGender.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.RandomWalkAlignment) == Creature2eFlags.RandomWalkAlignment, Creature2eFlags.RandomWalkAlignment.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & Creature2eFlags.Uninterruptable) == Creature2eFlags.Uninterruptable, Creature2eFlags.Uninterruptable.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Returns the printable read-friendly version of the creature format</summary>
        protected abstract void ToStringCreatureVersion(StringBuilder builder);

        /// <summary>Generates a String representing the leading common 2E creature structure data</summary>
        protected void ToStringHeader(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Long Name StrRef"));
            builder.Append(this.nameLong.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Short Name StrRef"));
            builder.Append(this.nameShort.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Creature Flags"));
            builder.Append((UInt32)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Creature Flags (enumerated)"));
            builder.Append(this.GetCreatureFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Kill XP reward"));
            builder.Append(this.experienceValue);
            builder.Append(StringFormat.ToStringAlignment("Accumulated XP total"));
            builder.Append(this.experienceTotal);
            builder.Append(StringFormat.ToStringAlignment("Gold"));
            builder.Append(this.gold);
            builder.Append(StringFormat.ToStringAlignment("Status Flags"));
            builder.Append(this.statusFlags);
            builder.Append(StringFormat.ToStringAlignment("(Status flag enumeration unavailable due to being IDS values)"));
            builder.Append(StringFormat.ToStringAlignment("Hit points (current)"));
            builder.Append(this.hitPointsCurrent);
            builder.Append(StringFormat.ToStringAlignment("Hit points (total)"));
            builder.Append(this.hitPointsMaximum);
            builder.Append(StringFormat.ToStringAlignment("Animation ID"));
            builder.Append(this.animationId);
            builder.Append(StringFormat.ToStringAlignment("(Animation description unavailable due to being hard coded values)"));
            builder.Append(StringFormat.ToStringAlignment("Metal color index"));
            builder.Append(this.colorIndexMetal);
            builder.Append(StringFormat.ToStringAlignment("Minor color index"));
            builder.Append(this.colorIndexMinor);
            builder.Append(StringFormat.ToStringAlignment("Major color index"));
            builder.Append(this.colorIndexMajor);
            builder.Append(StringFormat.ToStringAlignment("Skin color index"));
            builder.Append(this.colorIndexSkin);
            builder.Append(StringFormat.ToStringAlignment("Leather color index"));
            builder.Append(this.colorIndexLeather);
            builder.Append(StringFormat.ToStringAlignment("Armor color index"));
            builder.Append(this.colorIndexArmor);
            builder.Append(StringFormat.ToStringAlignment("Hair color index"));
            builder.Append(this.colorIndexHair);
            builder.Append(StringFormat.ToStringAlignment("Use version 2 Effect structure"));
            builder.Append(this.useEffectStructureVersion2);
            builder.Append(StringFormat.ToStringAlignment("Small portrait"));
            builder.Append(String.Format("'{0}'", this.portraitSmall.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Large portrait"));
            builder.Append(String.Format("'{0}'", this.portraitLarge.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Reputation"));
            builder.Append(this.reputation);
            builder.Append(StringFormat.ToStringAlignment("Hide in shadows"));
            builder.Append(this.hideInShadows);
            builder.Append(StringFormat.ToStringAlignment("Armor Class (Natural)"));
            builder.Append(this.armorClassNatural);
            builder.Append(StringFormat.ToStringAlignment("Armor Class (Effective)"));
            builder.Append(this.armorClassEffective);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Crushing)"));
            builder.Append(this.armorClassModifierCrushing);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Missile)"));
            builder.Append(this.armorClassModifierMissile);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Piercing)"));
            builder.Append(this.armorClassModifierPiercing);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Slashing)"));
            builder.Append(this.armorClassModifierSlashing);
            builder.Append(StringFormat.ToStringAlignment("ThAC0"));
            builder.Append(this.attackBase);
            builder.Append(StringFormat.ToStringAlignment("Attacks per round (value)"));
            builder.Append(this.attacksPerRound);
            builder.Append(StringFormat.ToStringAlignment("Attacks per round (actual)"));
            builder.Append(this.attacksPerRound < 6 ? Convert.ToDecimal(this.attacksPerRound) : (this.attacksPerRound - 5) + 0.5M);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Death)"));
            builder.Append(this.savingThrowDeath);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Wands)"));
            builder.Append(this.savingThrowWands);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Polymorph)"));
            builder.Append(this.savingThrowPolymorph);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Breath attacks)"));
            builder.Append(this.savingThrowBreathAttacks);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Spells)"));
            builder.Append(this.savingThrowSpells);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Fire)"));
            builder.Append(this.resistFire);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Cold)"));
            builder.Append(this.resistCold);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Electricity)"));
            builder.Append(this.resistElectricity);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Acid)"));
            builder.Append(this.resistAcid);
            builder.Append(StringFormat.ToStringAlignment("Magic resistance"));
            builder.Append(this.resistMagic);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Magical Fire)"));
            builder.Append(this.resistFireMagic);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Magical Cold)"));
            builder.Append(this.resistColdMagic);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Slashing)"));
            builder.Append(this.resistPhysicalSlashing);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Crushing)"));
            builder.Append(this.resistPhysicalCrushing);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Piercing)"));
            builder.Append(this.resistPhysicalPiercing);
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Missile)"));
            builder.Append(this.resistPhysicalMissile);
            builder.Append(StringFormat.ToStringAlignment("Detect illusions"));
            builder.Append(this.detectIllusions);
            builder.Append(StringFormat.ToStringAlignment("Set traps"));
            builder.Append(this.setTraps);
            builder.Append(StringFormat.ToStringAlignment("Lore"));
            builder.Append(this.lore);
            builder.Append(StringFormat.ToStringAlignment("Lock picking"));
            builder.Append(this.lockPicking);
            builder.Append(StringFormat.ToStringAlignment("Stealth"));
            builder.Append(this.stealth);
            builder.Append(StringFormat.ToStringAlignment("Find/Disarm traps"));
            builder.Append(this.findDisarmTraps);
            builder.Append(StringFormat.ToStringAlignment("Pick pockets"));
            builder.Append(this.pickPockets);
            builder.Append(StringFormat.ToStringAlignment("Fatigue"));
            builder.Append(this.fatigue);
            builder.Append(StringFormat.ToStringAlignment("Intoxication"));
            builder.Append(this.intoxication);
            builder.Append(StringFormat.ToStringAlignment("Luck"));
            builder.Append(this.luck);
        }

        /// <summary>Generates a String representing the proficiencies area of the creature data structure</summary>
        protected abstract void ToStringProcifiencies(StringBuilder builder);

        /// <summary>Generates a String representing the sound set area of the creature data structure</summary>
        protected void ToStringSoundSet(StringBuilder builder)
        {
            foreach (String key in this.soundSet.Keys) //i < this.soundSet.Count
            {
                builder.Append(StringFormat.ToStringAlignment("Soundset " + "(" + key + ")"));
                builder.Append(this.soundSet[key].StringReferenceIndex);
            }
        }

        /// <summary>Generates a String representing the stats and scripts area of the creature data structure</summary>
        protected void ToStringStatsAndScripts(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Level (first class)"));
            builder.Append(this.levelClass1);
            builder.Append(StringFormat.ToStringAlignment("Level (second class)"));
            builder.Append(this.levelClass2);
            builder.Append(StringFormat.ToStringAlignment("Level (third class)"));
            builder.Append(this.levelClass3);
            builder.Append(StringFormat.ToStringAlignment("Sex"));
            builder.Append(this.sex);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Strength)"));
            builder.Append(this.scoreStrength);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Strength bonus)"));
            builder.Append(this.scoreStrengthBonus);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Intelligence)"));
            builder.Append(this.scoreIntelligence);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Wisdom)"));
            builder.Append(this.scoreWisdom);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Dexterity)"));
            builder.Append(this.scoreDexterity);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Constitution)"));
            builder.Append(this.scoreConstitution);
            builder.Append(StringFormat.ToStringAlignment("Ability score (Charisma)"));
            builder.Append(this.scoreCharisma);
            builder.Append(StringFormat.ToStringAlignment("Morale"));
            builder.Append(this.morale);
            builder.Append(StringFormat.ToStringAlignment("Morale break point"));
            builder.Append(this.moraleBreak);
            builder.Append(StringFormat.ToStringAlignment("Racial Enemy"));
            builder.Append(this.racialEnemy);
            builder.Append(StringFormat.ToStringAlignment("Morale recovery time"));
            builder.Append(this.moraleRecoveryTime);
            this.ToStringKitValues(builder);
            builder.Append(StringFormat.ToStringAlignment("Script (Override)"));
            builder.Append(String.Format("'{0}'", this.scriptOverride.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Class)"));
            builder.Append(String.Format("'{0}'", this.scriptClass.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Race)"));
            builder.Append(String.Format("'{0}'", this.scriptRace.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (General)"));
            builder.Append(String.Format("'{0}'", this.scriptGeneral.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Default)"));
            builder.Append(String.Format("'{0}'", this.scriptDefault.ZResRef));
        }

        /// <summary>This method appends the kit variable(s) to the output StringBuilder</summary>
        /// <param name="dataArray">StringBuilder to append to</param>
        protected virtual void ToStringKitValues(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Kit"));
            builder.Append((UInt32)this.kit);
            builder.Append(StringFormat.ToStringAlignment("Kit (description)"));
            builder.Append(this.kit.GetDescription());
        }

        /// <summary>Generates a String representing the added Planescape: Torment values area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected virtual void ToStringClassifications(StringBuilder builder)
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

        /// <summary>Generates a String representing the trailing footer area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringHeaderFooter(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Global enumerator"));
            builder.Append(this.enumGlobal);
            builder.Append(StringFormat.ToStringAlignment("Local enumerator"));
            builder.Append(this.enumLocal);
            builder.Append(StringFormat.ToStringAlignment("Death variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.deathVariable.Value), 2));
            builder.Append(StringFormat.ToStringAlignment("Known spells offset"));
            builder.Append(this.offsetKnownSpells);
            builder.Append(StringFormat.ToStringAlignment("Known spells count"));
            builder.Append(this.countKnownSpells);
            builder.Append(StringFormat.ToStringAlignment("Spell memorization offset"));
            builder.Append(this.offsetSpellMemorization);
            builder.Append(StringFormat.ToStringAlignment("Spell memorization count"));
            builder.Append(this.countSpellMemorizations);
            builder.Append(StringFormat.ToStringAlignment("Memorized spells offset:"));
            builder.Append(this.offsetMemorizedSpells);
            builder.Append(StringFormat.ToStringAlignment("Memorized spells count:"));
            builder.Append(this.countMemorizedSpells);
            builder.Append(StringFormat.ToStringAlignment("Item slots offset"));
            builder.Append(this.offsetItemSlots);
            builder.Append(StringFormat.ToStringAlignment("Items offset"));
            builder.Append(this.offsetItems);
            builder.Append(StringFormat.ToStringAlignment("Items count"));
            builder.Append(this.countItems);
            builder.Append(StringFormat.ToStringAlignment("Effects offset"));
            builder.Append(this.offsetEffects);
            builder.Append(StringFormat.ToStringAlignment("Effects count"));
            builder.Append(this.countEffects);
            builder.Append(StringFormat.ToStringAlignment("Dialog"));
            builder.Append(String.Format("'{0}'", this.dialog.ZResRef));
        }
        #endregion
    }
}