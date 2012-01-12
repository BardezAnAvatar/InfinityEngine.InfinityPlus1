using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature2_2
{
    /// <summary>Creature header version 2.2</summary>
    public class Creature2_2Header : CreatureHeader
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 1582;

        #region Members
        /// <summary>D20 creature flags</summary>
        protected CreatureD20Flags flags;

        /// <summary>Saving throw modifier vs. Fortitude</summary>
        protected Byte savingThrowFortitude;

        /// <summary>Saving throw modifier vs. Reflex</summary>
        protected Byte savingThrowReflex;

        /// <summary>Saving throw modifier vs. Will</summary>
        protected Byte savingThrowWill;

        /// <summary>Physical missile damage resistance</summary>
        protected Byte resistMagicDamage;

        /// <summary>Unknown 4 bytes. Further resistances?</summary>
        protected UInt32 unknown1;

        /// <summary>Level at which you can turn undead</summary>
        protected Byte turnUndeadLevel;

        /// <summary>33 unknown bytes</summary>
        protected Byte[] unknown2;

        /// <summary>Total levels taken</summary>
        protected Byte levelsTotal;

        /// <summary>Number of levels of Barbarian taken</summary>
        protected Byte levelsOfBarbarian;

        /// <summary>Number of levels of Bard taken</summary>
        protected Byte levelsOfBard;

        /// <summary>Number of levels of Cleric taken</summary>
        protected Byte levelsOfCleric;

        /// <summary>Number of levels of Druid taken</summary>
        protected Byte levelsOfDruid;

        /// <summary>Number of levels of Fighter taken</summary>
        protected Byte levelsOfFighter;

        /// <summary>Number of levels of Monk taken</summary>
        protected Byte levelsOfMonk;

        /// <summary>Number of levels of Paladin taken</summary>
        protected Byte levelsOfPaladin;

        /// <summary>Number of levels of Ranger taken</summary>
        protected Byte levelsOfRanger;

        /// <summary>Number of levels of Rogue taken</summary>
        protected Byte levelsOfRogue;

        /// <summary>Number of levels of Sorcerer taken</summary>
        protected Byte levelsOfSorcerer;

        /// <summary>Number of levels of Barbarian taken</summary>
        protected Byte levelsOfWizard;

        /// <summary>22 unknown bytes</summary>
        protected Byte[] unknown3;

        //64 soundset entries

        /// <summary>Team script</summary>
        protected ResourceReference scriptTeam;

        /// <summary>Special script 1</summary>
        protected ResourceReference scriptSpecial1;

        /// <summary>Magical enchantment level of creature (to hit, immunity, etc.)</summary>
        /// <remarks>Might just be a Byte, but three following are all 0</remarks>
        protected Int32 creatureEnchantmentLevel;

        //24 bytes of feats.
        //FEATS.IDS suggests this uses 75 bits
        //DLTCEP suggests 192 bits allocated (6 32-bit ints)
        // So, three 64-bit entries, two being flag fields, other reserved feats field
        //See Also FEATS.2DA

        /// <summary>First 64 bits of feat flags</summary>
        protected D20Feats1 featsFlag1;

        /// <summary>Second 64 bits of feat flags</summary>
        protected D20Feats2 featsFlag2;

        /// <summary>Third/reserved 64 bits of feat flags</summary>
        protected UInt64 featsFlagReserved;
        

        //Weapon proficiencies
        /// <summary>Bow weapon proficiency</summary>
        protected Byte martialProficiencyBow;

        /// <summary>Crossbow weapon proficiency</summary>
        protected Byte simpleProficiencyCrossbow;

        /// <summary>Missile weapon proficiency</summary>
        protected Byte simpleProficiencyMissile;

        /// <summary>Axe weapon proficiency</summary>
        protected Byte martialProficiencyAxe;

        /// <summary>Mace weapon proficiency</summary>
        protected Byte simpleProficiencyMace;

        /// <summary>Flail weapon proficiency</summary>
        protected Byte martialProficiencyFlail;

        /// <summary>Polearm weapon proficiency</summary>
        protected Byte martialProficiencyPolearm;

        /// <summary>Hammer weapon proficiency</summary>
        protected Byte martialProficiencyHammer;

        /// <summary>Quarterstaff weapon proficiency</summary>
        protected Byte simpleProficiencyQuarterstaff;

        /// <summary>Great Sword weapon proficiency</summary>
        protected Byte martialProficiencyGreatSword;

        /// <summary>Large Sword weapon proficiency</summary>
        protected Byte martialProficiencyLargeSword;

        /// <summary>Small blade weapon proficiency</summary>
        protected Byte simpleProficiencySmallBlade;

        //Feat ranks taken

        /// <summary>Number of times Toughness has been taken</summary>
        protected Byte featToughness;

        /// <summary>Number of times Armored Arcana has been taken</summary>
        protected Byte featArmoredArcana;

        /// <summary>Number of times Cleave has been taken</summary>
        protected Byte featCleave;

        /// <summary>Number of times Armor Proficiency has been taken</summary>
        protected Byte featArmorProficiency;

        /// <summary>Number of times Spell Focus: Enchantment has been taken</summary>
        protected Byte featSpellFocusEnchantment;

        /// <summary>Number of times Spell Focus: Evocation has been taken</summary>
        protected Byte featSpellFocusEvocation;

        /// <summary>Number of times Spell Focus: Necromancy has been taken</summary>
        protected Byte featSpellFocusNecromancy;

        /// <summary>Number of times Spell Focus: Transmutation has been taken</summary>
        protected Byte featSpellFocusTransmutation;

        /// <summary>Number of times Spell Penetration has been taken</summary>
        protected Byte featSpellPenetration;

        /// <summary>Number of times Extra Rage has been taken</summary>
        protected Byte featExtraRage;

        /// <summary>Number of times Extra Wild Shape has been taken</summary>
        protected Byte featExtraWildShape;

        /// <summary>Number of times Extra Smiting has been taken</summary>
        protected Byte featExtraSmiting;

        /// <summary>Number of times Extra Turning has been taken</summary>
        protected Byte featExtraTurning;

        /// <summary>Bastard Sword weapon proficiency</summary>
        protected Byte exoticProficiencyBastardSword;

        /// <summary>38 bytes of unknown values. Most likely, ranks in specific feats.</summary>
        protected Byte[] unknown4;

        // Skills

        /// <summary>Skill ranks taken in Alchemy</summary>
        protected Byte skillAlchemy;

        /// <summary>Skill ranks taken in Animal Empathy</summary>
        protected Byte skillAnimalEmpathy;

        /// <summary>Skill ranks taken in Bluff</summary>
        protected Byte skillBluff;

        /// <summary>Skill ranks taken in Concentration</summary>
        protected Byte skillConcentration;

        /// <summary>Skill ranks taken in Diplomacy</summary>
        protected Byte skillDiplomacy;

        /// <summary>Skill ranks taken in Disable Device</summary>
        protected Byte skillDisableDevice;
        
        /// <summary>Skill ranks taken in Hide</summary>
        protected Byte skillHide;
        
        /// <summary>Skill ranks taken in Intimidate</summary>
        protected Byte skillIntimidate;

        /// <summary>Skill ranks taken in Knowledge (Arcana)</summary>
        protected Byte skillKnowledgeArcana;
        
        /// <summary>Skill ranks taken in Move Silently</summary>
        protected Byte skillMoveSilently;

        /// <summary>Skill ranks taken in Open Lock</summary>
        protected Byte skillOpenLock;

        /// <summary>Skill ranks taken in Pick Pocket</summary>
        protected Byte skillPickPocket;

        /// <summary>Skill ranks taken in Search</summary>
        protected Byte skillSearch;

        /// <summary>Skill ranks taken in Spellcraft</summary>
        protected Byte skillSpellcraft;

        /// <summary>Skill ranks taken in Use Magic Device</summary>
        protected Byte skillUseMagicDevice;

        /// <summary>Skill ranks taken in Wilderness Lore</summary>
        protected Byte skillWildernessLore;

        /// <summary>50 bytes reserved for further skills</summary>
        protected Byte[] skillReserved;

        /// <summary>CR of the creature</summary>
        /// <value>See MONCRATE.2DA</value>
        protected Byte challengeRating;

        /// <summary>First favored enemy</summary>
        protected Byte favoredEnemy1;

        /// <summary>Second favored enemy</summary>
        protected Byte favoredEnemy2;

        /// <summary>Third favored enemy</summary>
        protected Byte favoredEnemy3;

        /// <summary>Fourth favored enemy</summary>
        protected Byte favoredEnemy4;

        /// <summary>Fifth favored enemy</summary>
        protected Byte favoredEnemy5;

        /// <summary>Sixth favored enemy</summary>
        protected Byte favoredEnemy6;

        /// <summary>Seventh favored enemy</summary>
        protected Byte favoredEnemy7;

        /// <summary>Eighth favored enemy</summary>
        protected Byte favoredEnemy8;

        /// <summary>Creature's sub-race</summary>
        /// <remarks>Match to SUBRACE.IDS</remarks>
        protected Byte subrace;

        /// <summary>Unknown two bytes after subrace</summary>
        /// <value>Observed either 1 or 2</value>
        /// <remarks>
        ///     SEX, matching to GENDER.IDS?
        ///     seems unused, but roughly matches the gender field.
        ///     Could be the gender duplicate seen in other IE iterations.
        /// </remarks>
        protected UInt16 unknown5;

        //ability scores

        //it looks like morale, morale break, and morale recovery time ARE in here, from DLTCEP.
        //This is unknown at 0x26C in IESDP

        /// <summary>Class archetypes</summary>
        protected KitD20 archetype;

        /// <summary>Special script 2</summary>
        protected ResourceReference scriptSpecial2;

        /// <summary>Combat script</summary>
        protected ResourceReference scriptCombat;

        /// <summary>Special script 3</summary>
        protected ResourceReference scriptSpecial3;

        /// <summary>Movement script</summary>
        protected ResourceReference scriptMovement;

        //IWD variables
        /// <summary>Indicates whether or not the creature is visible</summary>
        protected Boolean visible;

        /// <summary>Sets the _DEAD variable on death</summary>
        /// <remarks>...BEEF</remarks>
        protected Boolean setVariableDEAD;

        /// <summary>"Sets" (increments?) the KILL_&lt;scriptname&gt;_CNT variable on death</summary>
        protected Boolean setVariableKILL_CNT;

        /// <summary>Unknown fourth byte that follows the preceeding three boolean flags above</summary>
        protected Byte unknown6;

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
        protected UInt16 unknown7;

        /// <summary>Save X coordinate</summary>
        protected UInt16 savedCoordinateX;

        /// <summary>Save Y coordinate</summary>
        protected UInt16 savedCoordinateY;

        /// <summary>Save orientation</summary>
        /// <value>0-15?</value>
        protected UInt16 savedOrientation;

        /// <summary>Unknown 146 bytes that follows the preceeding coordinate data</summary>
        protected Byte[] unknown8;

        /// <summary>Duplicte of class, used for object matching</summary>
        protected UInt16 avClass;

        /// <summary>Duplicte of class, used for object matching</summary>
        protected UInt16 classMsk;

        /// <summary>Two bytes unknown after class duplicates</summary>
        protected UInt16 unknown9;

        /// <summary>Collection of spells' offsets and counts</summary>
        protected GenericOrderedDictionary<String, D20KnownSpellOffsetData> spellOffsets;
        #endregion

        #region Properties
        /// <summary>D20 creature flags</summary>
        public CreatureD20Flags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        /// <summary>Saving throw modifier vs. Fortitude</summary>
        public Byte SavingThrowFortitude
        {
            get { return this.savingThrowFortitude; }
            set { this.savingThrowFortitude = value; }
        }

        /// <summary>Saving throw modifier vs. Reflex</summary>
        public Byte SavingThrowReflex
        {
            get { return this.savingThrowReflex; }
            set { this.savingThrowReflex = value; }
        }

        /// <summary>Saving throw modifier vs. Will</summary>
        public Byte SavingThrowWill
        {
            get { return this.savingThrowWill; }
            set { this.savingThrowWill = value; }
        }

        /// <summary>Physical missile damage resistance</summary>
        public Byte ResistMagicDamage
        {
            get { return this.resistMagicDamage; }
            set { this.resistMagicDamage = value; }
        }

        /// <summary>Unknown 4 bytes. Further resistances?</summary>
        public UInt32 Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }

        /// <summary>Level at which you can turn undead</summary>
        public Byte TurnUndeadLevel
        {
            get { return this.turnUndeadLevel; }
            set { this.turnUndeadLevel = value; }
        }

        /// <summary>33 unknown bytes</summary>
        public Byte[] Unknown2
        {
            get { return this.unknown2; }
            set { this.unknown2 = value; }
        }

        /// <summary>Total levels taken</summary>
        public Byte LevelsTotal
        {
            get { return this.levelsTotal; }
            set { this.levelsTotal = value; }
        }

        /// <summary>Number of levels of Barbarian taken</summary>
        public Byte LevelsOfBarbarian
        {
            get { return this.levelsOfBarbarian; }
            set { this.levelsOfBarbarian = value; }
        }

        /// <summary>Number of levels of Bard taken</summary>
        public Byte LevelsOfBard
        {
            get { return this.levelsOfBard; }
            set { this.levelsOfBard = value; }
        }

        /// <summary>Number of levels of Cleric taken</summary>
        public Byte LevelsOfCleric
        {
            get { return this.levelsOfCleric; }
            set { this.levelsOfCleric = value; }
        }

        /// <summary>Number of levels of Druid taken</summary>
        public Byte LevelsOfDruid
        {
            get { return this.levelsOfDruid; }
            set { this.levelsOfDruid = value; }
        }

        /// <summary>Number of levels of Fighter taken</summary>
        public Byte LevelsOfFighter
        {
            get { return this.levelsOfFighter; }
            set { this.levelsOfFighter = value; }
        }

        /// <summary>Number of levels of Monk taken</summary>
        public Byte LevelsOfMonk
        {
            get { return this.levelsOfMonk; }
            set { this.levelsOfMonk = value; }
        }

        /// <summary>Number of levels of Paladin taken</summary>
        public Byte LevelsOfPaladin
        {
            get { return this.levelsOfPaladin; }
            set { this.levelsOfPaladin = value; }
        }

        /// <summary>Number of levels of Ranger taken</summary>
        public Byte LevelsOfRanger
        {
            get { return this.levelsOfRanger; }
            set { this.levelsOfRanger = value; }
        }

        /// <summary>Number of levels of Rogue taken</summary>
        public Byte LevelsOfRogue
        {
            get { return this.levelsOfRogue; }
            set { this.levelsOfRogue = value; }
        }

        /// <summary>Number of levels of Sorcerer taken</summary>
        public Byte LevelsOfSorcerer
        {
            get { return this.levelsOfSorcerer; }
            set { this.levelsOfSorcerer = value; }
        }

        /// <summary>Number of levels of Barbarian taken</summary>
        public Byte LevelsOfWizard
        {
            get { return this.levelsOfWizard; }
            set { this.levelsOfWizard = value; }
        }

        /// <summary>22 unknown bytes</summary>
        public Byte[] Unknown3
        {
            get { return this.unknown3; }
            set { this.unknown3 = value; }
        }

        /// <summary>Team script</summary>
        public ResourceReference ScriptTeam
        {
            get { return this.scriptTeam; }
            set { this.scriptTeam = value; }
        }

        /// <summary>Special script 1</summary>
        public ResourceReference ScriptSpecial1
        {
            get { return this.scriptSpecial1; }
            set { this.scriptSpecial1 = value; }
        }

        /// <summary>Magical enchantment level of creature (to hit, immunity, etc.)</summary>
        /// <remarks>Might just be a Byte, but three following are all 0</remarks>
        public Int32 CreatureEnchantmentLevel
        {
            get { return this.creatureEnchantmentLevel; }
            set { this.creatureEnchantmentLevel = value; }
        }

        /// <summary>First 64 bits of feat flags</summary>
        public D20Feats1 FeatsFlag1
        {
            get { return this.featsFlag1; }
            set { this.featsFlag1 = value; }
        }

        /// <summary>Second 64 bits of feat flags</summary>
        public D20Feats2 FeatsFlag2
        {
            get { return this.featsFlag2; }
            set { this.featsFlag2 = value; }
        }

        /// <summary>Third/reserved 64 bits of feat flags</summary>
        public UInt64 FeatsFlagReserved
        {
            get { return this.featsFlagReserved; }
            set { this.featsFlagReserved = value; }
        }

        /// <summary>Bow weapon proficiency</summary>
        public Byte MartialProficiencyBow
        {
            get { return this.martialProficiencyBow; }
            set { this.martialProficiencyBow = value; }
        }

        /// <summary>Crossbow weapon proficiency</summary>
        public Byte SimpleProficiencyCrossbow
        {
            get { return this.simpleProficiencyCrossbow; }
            set { this.simpleProficiencyCrossbow = value; }
        }

        /// <summary>Missile weapon proficiency</summary>
        public Byte SimpleProficiencyMissile
        {
            get { return this.simpleProficiencyMissile; }
            set { this.simpleProficiencyMissile = value; }
        }

        /// <summary>Axe weapon proficiency</summary>
        public Byte MartialProficiencyAxe
        {
            get { return this.martialProficiencyAxe; }
            set { this.martialProficiencyAxe = value; }
        }

        /// <summary>Mace weapon proficiency</summary>
        public Byte SimpleProficiencyMace
        {
            get { return this.simpleProficiencyMace; }
            set { this.simpleProficiencyMace = value; }
        }

        /// <summary>Flail weapon proficiency</summary>
        public Byte MartialProficiencyFlail
        {
            get { return this.martialProficiencyFlail; }
            set { this.martialProficiencyFlail = value; }
        }

        /// <summary>Polearm weapon proficiency</summary>
        public Byte MartialProficiencyPolearm
        {
            get { return this.martialProficiencyPolearm; }
            set { this.martialProficiencyPolearm = value; }
        }

        /// <summary>Hammer weapon proficiency</summary>
        public Byte MartialProficiencyHammer
        {
            get { return this.martialProficiencyHammer; }
            set { this.martialProficiencyHammer = value; }
        }

        /// <summary>Quarterstaff weapon proficiency</summary>
        public Byte SimpleProficiencyQuarterstaff
        {
            get { return this.simpleProficiencyQuarterstaff; }
            set { this.simpleProficiencyQuarterstaff = value; }
        }

        /// <summary>Great Sword weapon proficiency</summary>
        public Byte MartialProficiencyGreatSword
        {
            get { return this.martialProficiencyGreatSword; }
            set { this.martialProficiencyGreatSword = value; }
        }

        /// <summary>Large Sword weapon proficiency</summary>
        public Byte MartialProficiencyLargeSword
        {
            get { return this.martialProficiencyLargeSword; }
            set { this.martialProficiencyLargeSword = value; }
        }

        /// <summary>Small blade weapon proficiency</summary>
        public Byte SimpleProficiencySmallBlade
        {
            get { return this.simpleProficiencySmallBlade; }
            set { this.simpleProficiencySmallBlade = value; }
        }

        /// <summary>Number of times Toughness has been taken</summary>
        public Byte FeatToughness
        {
            get { return this.featToughness; }
            set { this.featToughness = value; }
        }

        /// <summary>Number of times Armored Arcana has been taken</summary>
        public Byte FeatArmoredArcana
        {
            get { return this.featArmoredArcana; }
            set { this.featArmoredArcana = value; }
        }

        /// <summary>Number of times Cleave has been taken</summary>
        public Byte FeatCleave
        {
            get { return this.featCleave; }
            set { this.featCleave = value; }
        }

        /// <summary>Number of times Armor Proficiency has been taken</summary>
        public Byte FeatArmorProficiency
        {
            get { return this.featArmorProficiency; }
            set { this.featArmorProficiency = value; }
        }

        /// <summary>Number of times Spell Focus: Enchantment has been taken</summary>
        public Byte FeatSpellFocusEnchantment
        {
            get { return this.featSpellFocusEnchantment; }
            set { this.featSpellFocusEnchantment = value; }
        }

        /// <summary>Number of times Spell Focus: Evocation has been taken</summary>
        public Byte FeatSpellFocusEvocation
        {
            get { return this.featSpellFocusEvocation; }
            set { this.featSpellFocusEvocation = value; }
        }

        /// <summary>Number of times Spell Focus: Necromancy has been taken</summary>
        public Byte FeatSpellFocusNecromancy
        {
            get { return this.featSpellFocusNecromancy; }
            set { this.featSpellFocusNecromancy = value; }
        }

        /// <summary>Number of times Spell Focus: Transmutation has been taken</summary>
        public Byte FeatSpellFocusTransmutation
        {
            get { return this.featSpellFocusTransmutation; }
            set { this.featSpellFocusTransmutation = value; }
        }

        /// <summary>Number of times Spell Penetration has been taken</summary>
        public Byte FeatSpellPenetration
        {
            get { return this.featSpellPenetration; }
            set { this.featSpellPenetration = value; }
        }

        /// <summary>Number of times Extra Rage has been taken</summary>
        public Byte FeatExtraRage
        {
            get { return this.featExtraRage; }
            set { this.featExtraRage = value; }
        }

        /// <summary>Number of times Extra Wild Shape has been taken</summary>
        public Byte FeatExtraWildShape
        {
            get { return this.featExtraWildShape; }
            set { this.featExtraWildShape = value; }
        }

        /// <summary>Number of times Extra Smiting has been taken</summary>
        public Byte FeatExtraSmiting
        {
            get { return this.featExtraSmiting; }
            set { this.featExtraSmiting = value; }
        }

        /// <summary>Number of times Extra Turning has been taken</summary>
        public Byte FeatExtraTurning
        {
            get { return this.featExtraTurning; }
            set { this.featExtraTurning = value; }
        }

        /// <summary>Bastard Sword weapon proficiency</summary>
        public Byte ExoticProficiencyBastardSword
        {
            get { return this.exoticProficiencyBastardSword; }
            set { this.exoticProficiencyBastardSword = value; }
        }

        /// <summary>38 bytes of unknown values. Most likely, ranks in specific feats.</summary>
        public Byte[] Unknown4
        {
            get { return this.unknown4; }
            set { this.unknown4 = value; }
        }
        
        /// <summary>Skill ranks taken in Alchemy</summary>
        public Byte SkillAlchemy
        {
            get { return this.skillAlchemy; }
            set { this.skillAlchemy = value; }
        }

        /// <summary>Skill ranks taken in Animal Empathy</summary>
        public Byte SkillAnimalEmpathy
        {
            get { return this.skillAnimalEmpathy; }
            set { this.skillAnimalEmpathy = value; }
        }

        /// <summary>Skill ranks taken in Bluff</summary>
        public Byte SkillBluff
        {
            get { return this.skillBluff; }
            set { this.skillBluff = value; }
        }

        /// <summary>Skill ranks taken in Concentration</summary>
        public Byte SkillConcentration
        {
            get { return this.skillConcentration; }
            set { this.skillConcentration = value; }
        }

        /// <summary>Skill ranks taken in Diplomacy</summary>
        public Byte SkillDiplomacy
        {
            get { return this.skillDiplomacy; }
            set { this.skillDiplomacy = value; }
        }

        /// <summary>Skill ranks taken in Disable Device</summary>
        public Byte SkillDisableDevice
        {
            get { return this.skillDisableDevice; }
            set { this.skillDisableDevice = value; }
        }
        
        /// <summary>Skill ranks taken in Hide</summary>
        public Byte SkillHide
        {
            get { return this.skillHide; }
            set { this.skillHide = value; }
        }
        
        /// <summary>Skill ranks taken in Intimidate</summary>
        public Byte SkillIntimidate
        {
            get { return this.skillIntimidate; }
            set { this.skillIntimidate = value; }
        }

        /// <summary>Skill ranks taken in Knowledge (Arcana)</summary>
        public Byte SkillKnowledgeArcana
        {
            get { return this.skillKnowledgeArcana; }
            set { this.skillKnowledgeArcana = value; }
        }
        
        /// <summary>Skill ranks taken in Move Silently</summary>
        public Byte SkillMoveSilently
        {
            get { return this.skillMoveSilently; }
            set { this.skillMoveSilently = value; }
        }

        /// <summary>Skill ranks taken in Open Lock</summary>
        public Byte SkillOpenLock
        {
            get { return this.skillOpenLock; }
            set { this.skillOpenLock = value; }
        }

        /// <summary>Skill ranks taken in Pick Pocket</summary>
        public Byte SkillPickPocket
        {
            get { return this.skillPickPocket; }
            set { this.skillPickPocket = value; }
        }

        /// <summary>Skill ranks taken in Search</summary>
        public Byte SkillSearch
        {
            get { return this.skillSearch; }
            set { this.skillSearch = value; }
        }

        /// <summary>Skill ranks taken in Spellcraft</summary>
        public Byte SkillSpellcraft
        {
            get { return this.skillSpellcraft; }
            set { this.skillSpellcraft = value; }
        }

        /// <summary>Skill ranks taken in Use Magic Device</summary>
        public Byte SkillUseMagicDevice
        {
            get { return this.skillUseMagicDevice; }
            set { this.skillUseMagicDevice = value; }
        }

        /// <summary>Skill ranks taken in Wilderness Lore</summary>
        public Byte SkillWildernessLore
        {
            get { return this.skillWildernessLore; }
            set { this.skillWildernessLore = value; }
        }

        /// <summary>50 bytes reserved for further skills</summary>
        public Byte[] SkillReserved
        {
            get { return this.skillReserved; }
            set { this.skillReserved = value; }
        }

        /// <summary>CR of the creature</summary>
        /// <value>See MONCRATE.2DA</value>
        public Byte ChallengeRating
        {
            get { return this.challengeRating; }
            set { this.challengeRating = value; }
        }

        /// <summary>First favored enemy</summary>
        public Byte FavoredEnemy1
        {
            get { return this.favoredEnemy1; }
            set { this.favoredEnemy1 = value; }
        }

        /// <summary>Second favored enemy</summary>
        public Byte FavoredEnemy2
        {
            get { return this.favoredEnemy2; }
            set { this.favoredEnemy2 = value; }
        }

        /// <summary>Third favored enemy</summary>
        public Byte FavoredEnemy3
        {
            get { return this.favoredEnemy3; }
            set { this.favoredEnemy3 = value; }
        }

        /// <summary>Fourth favored enemy</summary>
        public Byte FavoredEnemy4
        {
            get { return this.favoredEnemy4; }
            set { this.favoredEnemy4 = value; }
        }

        /// <summary>Fifth favored enemy</summary>
        public Byte FavoredEnemy5
        {
            get { return this.favoredEnemy5; }
            set { this.favoredEnemy5 = value; }
        }

        /// <summary>Sixth favored enemy</summary>
        public Byte FavoredEnemy6
        {
            get { return this.favoredEnemy6; }
            set { this.favoredEnemy6 = value; }
        }

        /// <summary>Seventh favored enemy</summary>
        public Byte FavoredEnemy7
        {
            get { return this.favoredEnemy7; }
            set { this.favoredEnemy7 = value; }
        }

        /// <summary>Eighth favored enemy</summary>
        public Byte FavoredEnemy8
        {
            get { return this.favoredEnemy8; }
            set { this.favoredEnemy8 = value; }
        }

        /// <summary>Creature's sub-race</summary>
        /// <remarks>Match to SUBRACE.IDS</remarks>
        public Byte Subrace
        {
            get { return this.subrace; }
            set { this.subrace = value; }
        }

        /// <summary>Unknown two bytes after subrace</summary>
        /// <value>Observed either 1 or 2</value>
        /// <remarks>
        ///     SEX, matching to GENDER.IDS?
        ///     seems unused, but roughly matches the gender field
        /// </remarks>
        public UInt16 Unknown5
        {
            get { return this.unknown5; }
            set { this.unknown5 = value; }
        }

        /// <summary>Class archetypes</summary>
        public KitD20 Archetype
        {
            get { return this.archetype; }
            set { this.archetype = value; }
        }

        /// <summary>Special script 2</summary>
        public ResourceReference ScriptSpecial2
        {
            get { return this.scriptSpecial2; }
            set { this.scriptSpecial2 = value; }
        }

        /// <summary>Combat script</summary>
        public ResourceReference ScriptCombat
        {
            get { return this.scriptCombat; }
            set { this.scriptCombat = value; }
        }

        /// <summary>Special script 3</summary>
        public ResourceReference ScriptSpecial3
        {
            get { return this.scriptSpecial3; }
            set { this.scriptSpecial3 = value; }
        }

        /// <summary>Movement script</summary>
        public ResourceReference ScriptMovement
        {
            get { return this.scriptMovement; }
            set { this.scriptMovement = value; }
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
        public Byte Unknown6
        {
            get { return this.unknown6; }
            set { this.unknown6 = value; }
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
        public UInt16 Unknown7
        {
            get { return this.unknown7; }
            set { this.unknown7 = value; }
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

        /// <summary>Unknown 146 bytes that follows the preceeding coordinate data</summary>
        public Byte[] Unknown8
        {
            get { return this.unknown8; }
            set { this.unknown8 = value; }
        }

        /// <summary>Duplicte of class, used for object matching</summary>
        public UInt16 AvClass
        {
            get { return this.avClass; }
            set { this.avClass = value; }
        }

        /// <summary>Duplicte of class, used for object matching</summary>
        public UInt16 ClassMsk
        {
            get { return this.classMsk; }
            set { this.classMsk = value; }
        }

        /// <summary>Two bytes unknown after class duplicates</summary>
        public UInt16 Unknown9
        {
            get { return this.unknown9; }
            set { this.unknown9 = value; }
        }

        /// <summary>Collection of spells' offsets and counts</summary>
        public GenericOrderedDictionary<String, D20KnownSpellOffsetData> SpellOffsets
        {
            get { return this.spellOffsets; }
            set { this.spellOffsets = value; }
        }
        #endregion
        
        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Creature2_2Header()
        {
            this.Initialize();
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.InitializeOffsets();

            this.unknown2 = new Byte[33];
            this.unknown3 = new Byte[22];
            this.unknown4 = new Byte[38];
            this.unknown8 = new Byte[146];
            this.scriptTeam = new ResourceReference();
            this.scriptSpecial1 = new ResourceReference();
            this.scriptSpecial2 = new ResourceReference();
            this.scriptCombat = new ResourceReference();
            this.scriptSpecial3 = new ResourceReference();
            this.scriptMovement = new ResourceReference();
            this.skillReserved = new Byte[50];
            this.SecondaryDeathVariable = new ZString();
            this.TertiaryDeathVariable = new ZString();
        }
        
        /// <summary>Initializes the soundset ordered dictionary</summary>
        /// <remarks>
        ///     Keys matched from SOUNDOFF.IDS
        ///     Also look to CSOUND.2DA
        /// </remarks>
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
            /* 43 */ this.soundSet.Add("Unknown 43", new StringReference());
            /* 44 */ this.soundSet.Add("Unknown 44", new StringReference());
            /* 45 */ this.soundSet.Add("Unknown 45", new StringReference());
            /* 46 */ this.soundSet.Add("Unknown 46", new StringReference());
            /* 47 */ this.soundSet.Add("Unknown 47", new StringReference());
            /* 48 */ this.soundSet.Add("Unknown 48", new StringReference());
            /* 49 */ this.soundSet.Add("Unknown 49", new StringReference());
            /* 50 */ this.soundSet.Add("Unknown 50", new StringReference());
            /* 51 */ this.soundSet.Add("Unknown 51", new StringReference());
            /* 52 */ this.soundSet.Add("Unknown 52", new StringReference());
            /* 53 */ this.soundSet.Add("Unknown 53", new StringReference());
            /* 54 */ this.soundSet.Add("Unknown 54", new StringReference());
            /* 55 */ this.soundSet.Add("Unknown 55", new StringReference());
            /* 56 */ this.soundSet.Add("Unknown 56", new StringReference());
            /* 57 */ this.soundSet.Add("Unknown 57", new StringReference());
            /* 58 */ this.soundSet.Add("Unknown 58", new StringReference());
            /* 59 */ this.soundSet.Add("Unknown 59", new StringReference());
            /* 60 */ this.soundSet.Add("Unknown 60", new StringReference());
            /* 61 */ this.soundSet.Add("Unknown 61", new StringReference());
            /* 62 */ this.soundSet.Add("Unknown 62", new StringReference());
            /* 63 */ this.soundSet.Add("Unknown 63", new StringReference());
        }
        
        /// <summary>Initializes the offset ordered dictionary</summary>
        protected void InitializeOffsets()
        {
            this.spellOffsets = new GenericOrderedDictionary<String, D20KnownSpellOffsetData>();

            this.spellOffsets.Add("Bard1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Bard9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Cleric1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Cleric9", new D20KnownSpellOffsetData());
            
            this.spellOffsets.Add("Druid1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Druid9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Paladin1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Paladin9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Ranger1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Ranger9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Sorcerer1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Sorcerer9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Wizard1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Wizard9", new D20KnownSpellOffsetData());

            this.spellOffsets.Add("Domain1", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain2", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain3", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain4", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain5", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain6", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain7", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain8", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Domain9", new D20KnownSpellOffsetData());
            
            this.spellOffsets.Add("Abilities", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Songs", new D20KnownSpellOffsetData());
            this.spellOffsets.Add("Shapes", new D20KnownSpellOffsetData());
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
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 1574);
            this.ReadBodyLeadingValues(remainingBody);
            this.ReadSoundSet(remainingBody, 164);
            this.ReadBodyFeatsSkillsAndProficiencies(remainingBody);
            this.ReadBodyAfterProficiencies(remainingBody);
            this.ReadBodyIcewindDaleAdditions(remainingBody);
            this.ReadBodyClassifications(remainingBody);
            this.ReadBodyBeforeOffsets(remainingBody);
            this.ReadSpellOffsetsAndCounts(remainingBody);
            this.ReadBodyFooter(remainingBody);
        }

        /// <summary>Reads the leading 102 bytes from the Creature D20 file</summary>
        /// <param name="remainingBody">Byte array to read from. Expected to be reading from index 0, size of at least 164 bytes.</param>
        protected void ReadBodyLeadingValues(Byte[] remainingBody)
        {
            this.nameLong.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 0);
            this.nameShort.StringReferenceIndex = ReusableIO.ReadInt32FromArray(remainingBody, 4);
            this.flags = (CreatureD20Flags)ReusableIO.ReadUInt32FromArray(remainingBody, 8);
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
            this.armorClassModifierCrushing = ReusableIO.ReadInt16FromArray(remainingBody, 64);
            this.armorClassModifierMissile = ReusableIO.ReadInt16FromArray(remainingBody, 66);
            this.armorClassModifierPiercing = ReusableIO.ReadInt16FromArray(remainingBody, 68);
            this.armorClassModifierSlashing = ReusableIO.ReadInt16FromArray(remainingBody, 70);
            this.attackBase = remainingBody[72];
            this.attacksPerRound = remainingBody[73];
            this.savingThrowFortitude = remainingBody[74];
            this.savingThrowReflex = remainingBody[75];
            this.savingThrowWill = remainingBody[76];
            this.resistFire = remainingBody[77];
            this.resistCold = remainingBody[78];
            this.resistElectricity = remainingBody[79];
            this.resistAcid = remainingBody[80];
            this.resistMagic = remainingBody[81];
            this.resistFireMagic = remainingBody[82];
            this.resistColdMagic = remainingBody[83];
            this.resistPhysicalSlashing = remainingBody[84];
            this.resistPhysicalCrushing = remainingBody[85];
            this.resistPhysicalPiercing = remainingBody[86];
            this.resistPhysicalMissile = remainingBody[87];
            this.resistMagicDamage = remainingBody[88];
            this.unknown1 = ReusableIO.ReadUInt32FromArray(remainingBody, 89);
            this.fatigue = remainingBody[93];
            this.intoxication = remainingBody[94];
            this.luck = remainingBody[95];
            this.turnUndeadLevel = remainingBody[96];
            Array.Copy(remainingBody, 97, this.unknown2, 0, 33);
            this.levelsTotal = remainingBody[130];
            this.levelsOfBarbarian = remainingBody[131];
            this.levelsOfBard = remainingBody[132];
            this.levelsOfCleric = remainingBody[133];
            this.levelsOfDruid = remainingBody[134];
            this.levelsOfFighter = remainingBody[135];
            this.levelsOfMonk = remainingBody[136];
            this.levelsOfPaladin = remainingBody[137];
            this.levelsOfRanger = remainingBody[138];
            this.levelsOfRogue = remainingBody[139];
            this.levelsOfSorcerer = remainingBody[140];
            this.levelsOfWizard = remainingBody[141];
            Array.Copy(remainingBody, 142, this.unknown3, 0, 22);
        }

        /// <summary>Reads all of the soundset variables from the byte array</summary>
        /// <param name="body">Byte Array to read from</param>
        /// <param name="bodyOffset">Offset in the byte array to start reading from</param>
        protected void ReadSoundSet(Byte[] body, Int32 bodyOffset)
        {
            for (Int32 index = 0; index < 64; ++index)
                this.soundSet[index].StringReferenceIndex = ReusableIO.ReadInt32FromArray(body, (index * 4) + bodyOffset);
        }

        /// <summary>Reads the feats, skills and proficiencies section of the header</summary>
        /// <param name="remainingBody">Byte array to read from</param>
        protected void ReadBodyFeatsSkillsAndProficiencies(Byte[] remainingBody)
        {
            this.scriptTeam.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 420, CultureConstants.CultureCodeEnglish);
            this.scriptSpecial1.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 428, CultureConstants.CultureCodeEnglish);
            this.creatureEnchantmentLevel = ReusableIO.ReadInt32FromArray(remainingBody, 436);
            this.featsFlag1 = (D20Feats1)ReusableIO.ReadUInt64FromArray(remainingBody, 440);
            this.featsFlag2 = (D20Feats2)ReusableIO.ReadUInt64FromArray(remainingBody, 448);
            this.featsFlagReserved = ReusableIO.ReadUInt64FromArray(remainingBody, 456);
            this.martialProficiencyBow = remainingBody[464];
            this.simpleProficiencyCrossbow = remainingBody[465];
            this.simpleProficiencyMissile = remainingBody[466];
            this.martialProficiencyAxe = remainingBody[467];
            this.simpleProficiencyMace = remainingBody[468];
            this.martialProficiencyFlail = remainingBody[469];
            this.martialProficiencyPolearm = remainingBody[470];
            this.martialProficiencyHammer = remainingBody[471];
            this.simpleProficiencyQuarterstaff = remainingBody[472];
            this.martialProficiencyGreatSword = remainingBody[473];
            this.martialProficiencyLargeSword = remainingBody[474];
            this.simpleProficiencySmallBlade = remainingBody[475];
            this.featToughness = remainingBody[476];
            this.featArmoredArcana = remainingBody[477];
            this.featCleave = remainingBody[478];
            this.featArmorProficiency = remainingBody[479];
            this.featSpellFocusEnchantment = remainingBody[480];
            this.featSpellFocusEvocation = remainingBody[481];
            this.featSpellFocusNecromancy = remainingBody[482];
            this.featSpellFocusTransmutation = remainingBody[483];
            this.featSpellPenetration = remainingBody[484];
            this.featExtraRage = remainingBody[485];
            this.featExtraWildShape = remainingBody[486];
            this.featExtraSmiting = remainingBody[487];
            this.featExtraTurning = remainingBody[488];
            this.exoticProficiencyBastardSword = remainingBody[489];
            Array.Copy(remainingBody, 490, this.unknown4, 0, 38);
            this.skillAlchemy = remainingBody[528];
            this.skillAnimalEmpathy = remainingBody[529];
            this.skillBluff = remainingBody[530];
            this.skillConcentration = remainingBody[531];
            this.skillDiplomacy = remainingBody[532];
            this.skillDisableDevice = remainingBody[533];
            this.skillHide = remainingBody[534];
            this.skillIntimidate = remainingBody[535];
            this.skillKnowledgeArcana = remainingBody[536];
            this.skillMoveSilently = remainingBody[537];
            this.skillOpenLock = remainingBody[538];
            this.skillPickPocket = remainingBody[539];
            this.skillSearch = remainingBody[540];
            this.skillSpellcraft = remainingBody[541];
            this.skillUseMagicDevice = remainingBody[542];
            this.skillWildernessLore = remainingBody[543];
            Array.Copy(remainingBody, 544, this.skillReserved, 0, 50);
        }

        /// <summary>Reads the Creature D20 elements after the proficiency entries</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 594, until byte 660.</param>
        protected void ReadBodyAfterProficiencies(Byte[] remainingBody)
        {
            this.challengeRating = remainingBody[594];
            this.favoredEnemy1 = remainingBody[595];
            this.favoredEnemy2 = remainingBody[596];
            this.favoredEnemy3 = remainingBody[597];
            this.favoredEnemy4 = remainingBody[598];
            this.favoredEnemy5 = remainingBody[599];
            this.favoredEnemy6 = remainingBody[600];
            this.favoredEnemy7 = remainingBody[601];
            this.favoredEnemy8 = remainingBody[602];
            this.subrace = remainingBody[603];
            this.unknown5 = ReusableIO.ReadUInt16FromArray(remainingBody, 604);
            this.scoreStrength = remainingBody[606];
            this.scoreIntelligence = remainingBody[607];
            this.scoreWisdom = remainingBody[608];
            this.scoreDexterity = remainingBody[609];
            this.scoreConstitution = remainingBody[610];
            this.scoreCharisma = remainingBody[611];
            this.morale = remainingBody[612];
            this.moraleBreak = remainingBody[613];
            this.moraleRecoveryTime = ReusableIO.ReadUInt16FromArray(remainingBody, 614);
            this.archetype = (KitD20)ReusableIO.ReadUInt32FromArray(remainingBody, 616);
            this.scriptOverride.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 620, CultureConstants.CultureCodeEnglish);
            this.scriptSpecial2.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 628, CultureConstants.CultureCodeEnglish);
            this.scriptCombat.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 636, CultureConstants.CultureCodeEnglish);
            this.scriptSpecial3.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 644, CultureConstants.CultureCodeEnglish);
            this.scriptMovement.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 652, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Reads Icewind Dale structure additions</summary>
        /// <param name="headerBody">Byte array to read from. Expects to start reading at 660, reading 104 Bytes</param>
        protected void ReadBodyIcewindDaleAdditions(Byte[] headerBody)
        {
            //is this all one Int32, maybe bit-shifted to get the boolean values?
            this.visible = Convert.ToBoolean(headerBody[660]);
            this.setVariableDEAD = Convert.ToBoolean(headerBody[661]);
            this.setVariableKILL_CNT = Convert.ToBoolean(headerBody[662]);
            this.unknown6 = headerBody[663];
            this.internalVariable1 = ReusableIO.ReadInt16FromArray(headerBody, 664);
            this.internalVariable2 = ReusableIO.ReadInt16FromArray(headerBody, 666);
            this.internalVariable3 = ReusableIO.ReadInt16FromArray(headerBody, 668);
            this.internalVariable4 = ReusableIO.ReadInt16FromArray(headerBody, 670);
            this.internalVariable5 = ReusableIO.ReadInt16FromArray(headerBody, 672);
            this.SecondaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 674, CultureConstants.CultureCodeEnglish, 32);
            this.TertiaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 706, CultureConstants.CultureCodeEnglish, 32);
            this.unknown7 = ReusableIO.ReadUInt16FromArray(headerBody, 738);
            this.savedCoordinateX = ReusableIO.ReadUInt16FromArray(headerBody, 740);
            this.savedCoordinateY = ReusableIO.ReadUInt16FromArray(headerBody, 742);
            this.savedOrientation = ReusableIO.ReadUInt16FromArray(headerBody, 744);
            Array.Copy(headerBody, 746, this.unknown8, 0, 146);
        }

        /// <summary>Reads the classification entries from the creature file</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 892, reading 12 bytes.</param>
        protected void ReadBodyClassifications(Byte[] headerBodyArray)
        {
            this.classificationHostility = headerBodyArray[892];
            this.classificationGeneral = headerBodyArray[893];
            this.classificationRace = headerBodyArray[894];
            this.classificationClass = headerBodyArray[895];
            this.classificationSpecific = headerBodyArray[896];
            this.classificationGender = headerBodyArray[897];
            this.classificationObject1 = headerBodyArray[898];
            this.classificationObject2 = headerBodyArray[899];
            this.classificationObject3 = headerBodyArray[900];
            this.classificationObject4 = headerBodyArray[901];
            this.classificationObject5 = headerBodyArray[902];
            this.classificationAlignment = headerBodyArray[903];
        }

        /// <summary>Reads the header's footer, containing all the offsets, death variable, and enumerators</summary>
        /// <param name="remainingHeaderArray">Byte array to read from</param>
        protected void ReadBodyBeforeOffsets(Byte[] remainingHeaderArray)
        {
            this.enumGlobal = ReusableIO.ReadInt16FromArray(remainingHeaderArray, 904);
            this.enumLocal = ReusableIO.ReadInt16FromArray(remainingHeaderArray, 906);
            this.deathVariable.Source = ReusableIO.ReadStringFromByteArray(remainingHeaderArray, 908, CultureConstants.CultureCodeEnglish, 32);
            this.avClass = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 940);
            this.classMsk = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 942);
            this.unknown9 = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 944);
        }

        /// <summary>Reads the header's spell offsets</summary>
        /// <param name="header">Byte array to read from, starting at position 946 for X bytes</param>
        protected void ReadSpellOffsetsAndCounts(Byte[] header)
        {
            //Class offsets
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                this.spellOffsets[index].Offset = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 946);

            //Class counts
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                this.spellOffsets[index].Count = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1198);

            //Domain offsets
            for (Int32 index = 0; index < 9 /*9*7*/; ++index)
                this.spellOffsets[index + 63].Offset = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1450);

            //Domain counts
            for (Int32 index = 0; index < 9 /*9*7*/; ++index)
                this.spellOffsets[index + 63].Count = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1486);

            //Abilities
            this.spellOffsets[72].Offset = ReusableIO.ReadUInt32FromArray(header, 1522);
            this.spellOffsets[72].Count = ReusableIO.ReadUInt32FromArray(header, 1526);
            
            //Songs
            this.spellOffsets[73].Offset = ReusableIO.ReadUInt32FromArray(header, 1530);
            this.spellOffsets[73].Count = ReusableIO.ReadUInt32FromArray(header, 1534);

            //Shapes
            this.spellOffsets[74].Offset = ReusableIO.ReadUInt32FromArray(header, 1538);
            this.spellOffsets[74].Count = ReusableIO.ReadUInt32FromArray(header, 1542);
        }

        /// <summary>Reads the header's footer, containing item & effect offsets & counts and dialog</summary>
        /// <param name="remainingHeaderArray">Byte array to read from</param>
        /// <param name="offset">Starting index in remainingHeaderArray to read from</param>
        protected void ReadBodyFooter(Byte[] remainingHeaderArray)
        {
            this.offsetItemSlots = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, 1546);
            this.offsetItems = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, 1550);
            this.countItems = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, 1554);
            this.offsetEffects = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, 1558);
            this.countEffects = ReusableIO.ReadUInt32FromArray(remainingHeaderArray, 1562);
            this.dialog.ResRef = ReusableIO.ReadStringFromByteArray(remainingHeaderArray, 1566, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteLeadingValues(output);
            this.WriteSoundSet(output);
            this.WriteFeatsSkillsAndProficiencies(output);
            this.WriteAfterProficiencies(output);
            this.WriteIcewindDaleAdditions(output);
            this.WriteClassifications(output);
            this.WriteBeforeOffsets(output);
            this.WriteSpellOffsetsAndCounts(output);
            this.WriteFooter(output);
        }

        /// <summary>This method writes the leading 102 bytes from the Creature D20 file.</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteLeadingValues(Stream output)
        {
            //Write the signature & version
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);

            //Write header
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
            ReusableIO.WriteInt16ToStream(this.armorClassModifierCrushing, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierMissile, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierPiercing, output);
            ReusableIO.WriteInt16ToStream(this.armorClassModifierSlashing, output);
            output.WriteByte(this.attackBase);
            output.WriteByte(this.attacksPerRound);
            output.WriteByte(this.savingThrowFortitude);
            output.WriteByte(this.savingThrowReflex);
            output.WriteByte(this.savingThrowWill);
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
            output.WriteByte(this.resistMagicDamage);
            ReusableIO.WriteUInt32ToStream(this.unknown1, output);
            output.WriteByte(this.fatigue);
            output.WriteByte(this.intoxication);
            output.WriteByte(this.luck);
            output.WriteByte(this.turnUndeadLevel);
            output.Write(this.unknown2, 0, 33);
            output.WriteByte(this.levelsTotal);
            output.WriteByte(this.levelsOfBarbarian);
            output.WriteByte(this.levelsOfBard);
            output.WriteByte(this.levelsOfCleric);
            output.WriteByte(this.levelsOfDruid);
            output.WriteByte(this.levelsOfFighter);
            output.WriteByte(this.levelsOfMonk);
            output.WriteByte(this.levelsOfPaladin);
            output.WriteByte(this.levelsOfRanger);
            output.WriteByte(this.levelsOfRogue);
            output.WriteByte(this.levelsOfSorcerer);
            output.WriteByte(this.levelsOfWizard);
            output.Write(this.unknown3, 0, 22);
        }

        /// <summary>Writes all of the soundset variables to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteSoundSet(Stream output)
        {
            for (Int32 index = 0; index < 64; ++index)
                ReusableIO.WriteInt32ToStream(this.soundSet[index].StringReferenceIndex, output);
        }

        /// <summary>Writes the feats, skills and proficiencies section of the header to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteFeatsSkillsAndProficiencies(Stream output)
        {
            ReusableIO.WriteStringToStream(this.scriptTeam.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptSpecial1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.creatureEnchantmentLevel, output);
            ReusableIO.WriteUInt64ToStream((UInt64)this.featsFlag1, output);
            ReusableIO.WriteUInt64ToStream((UInt64)this.featsFlag2, output);
            ReusableIO.WriteUInt64ToStream(this.featsFlagReserved, output);
            output.WriteByte(this.martialProficiencyBow);
            output.WriteByte(this.simpleProficiencyCrossbow);
            output.WriteByte(this.simpleProficiencyMissile);
            output.WriteByte(this.martialProficiencyAxe);
            output.WriteByte(this.simpleProficiencyMace);
            output.WriteByte(this.martialProficiencyFlail);
            output.WriteByte(this.martialProficiencyPolearm);
            output.WriteByte(this.martialProficiencyHammer);
            output.WriteByte(this.simpleProficiencyQuarterstaff);
            output.WriteByte(this.martialProficiencyGreatSword);
            output.WriteByte(this.martialProficiencyLargeSword);
            output.WriteByte(this.simpleProficiencySmallBlade);
            output.WriteByte(this.featToughness);
            output.WriteByte(this.featArmoredArcana);
            output.WriteByte(this.featCleave);
            output.WriteByte(this.featArmorProficiency);
            output.WriteByte(this.featSpellFocusEnchantment);
            output.WriteByte(this.featSpellFocusEvocation);
            output.WriteByte(this.featSpellFocusNecromancy);
            output.WriteByte(this.featSpellFocusTransmutation);
            output.WriteByte(this.featSpellPenetration);
            output.WriteByte(this.featExtraRage);
            output.WriteByte(this.featExtraWildShape);
            output.WriteByte(this.featExtraSmiting);
            output.WriteByte(this.featExtraTurning);
            output.WriteByte(this.exoticProficiencyBastardSword);
            output.Write(this.unknown4, 0, 38);
            output.WriteByte(this.skillAlchemy);
            output.WriteByte(this.skillAnimalEmpathy);
            output.WriteByte(this.skillBluff);
            output.WriteByte(this.skillConcentration);
            output.WriteByte(this.skillDiplomacy);
            output.WriteByte(this.skillDisableDevice);
            output.WriteByte(this.skillHide);
            output.WriteByte(this.skillIntimidate);
            output.WriteByte(this.skillKnowledgeArcana);
            output.WriteByte(this.skillMoveSilently);
            output.WriteByte(this.skillOpenLock);
            output.WriteByte(this.skillPickPocket);
            output.WriteByte(this.skillSearch);
            output.WriteByte(this.skillSpellcraft);
            output.WriteByte(this.skillUseMagicDevice);
            output.WriteByte(this.skillWildernessLore);
            output.Write(this.skillReserved, 0, 50);
        }

        /// <summary>Writes the Creature D20 elements after the proficiency entries</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteAfterProficiencies(Stream output)
        {
            output.WriteByte(this.challengeRating);
            output.WriteByte(this.favoredEnemy1);
            output.WriteByte(this.favoredEnemy2);
            output.WriteByte(this.favoredEnemy3);
            output.WriteByte(this.favoredEnemy4);
            output.WriteByte(this.favoredEnemy5);
            output.WriteByte(this.favoredEnemy6);
            output.WriteByte(this.favoredEnemy7);
            output.WriteByte(this.favoredEnemy8);
            output.WriteByte(this.subrace);
            ReusableIO.WriteUInt16ToStream(this.unknown5, output);
            output.WriteByte(this.scoreStrength);
            output.WriteByte(this.scoreIntelligence);
            output.WriteByte(this.scoreWisdom);
            output.WriteByte(this.scoreDexterity);
            output.WriteByte(this.scoreConstitution);
            output.WriteByte(this.scoreCharisma);
            output.WriteByte(this.morale);
            output.WriteByte(this.moraleBreak);
            ReusableIO.WriteUInt16ToStream(this.moraleRecoveryTime, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.archetype, output);
            ReusableIO.WriteStringToStream(this.scriptOverride.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptSpecial2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptCombat.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptSpecial3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.scriptMovement.ResRef, output, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Writes the Icewind Dale structure additions to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteIcewindDaleAdditions(Stream output)
        {
            output.WriteByte(Convert.ToByte(this.visible));
            output.WriteByte(Convert.ToByte(this.setVariableDEAD));
            output.WriteByte(Convert.ToByte(this.setVariableKILL_CNT));
            output.WriteByte(this.unknown6);
            ReusableIO.WriteInt16ToStream(this.internalVariable1, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable2, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable3, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable4, output);
            ReusableIO.WriteInt16ToStream(this.internalVariable5, output);
            ReusableIO.WriteStringToStream(SecondaryDeathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteStringToStream(TertiaryDeathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.unknown7, output);
            ReusableIO.WriteUInt16ToStream(this.savedCoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.savedCoordinateY, output);
            ReusableIO.WriteUInt16ToStream(this.savedOrientation, output);
            output.Write(this.unknown8, 0, 146);
        }

        /// <summary>Writes the classification entries from the creature file to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteClassifications(Stream output)
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

        /// <summary>Writes the header's footer, containing all the offsets, death variable, and enumerators</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteBeforeOffsets(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.enumGlobal, output);
            ReusableIO.WriteInt16ToStream(this.enumLocal, output);
            ReusableIO.WriteStringToStream(this.deathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.avClass, output);
            ReusableIO.WriteUInt16ToStream(this.classMsk, output);
            ReusableIO.WriteUInt16ToStream(this.unknown9, output);
        }

        /// <summary>Writes the header's spell offsets</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteSpellOffsetsAndCounts(Stream output)
        {
            //Class offsets
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                ReusableIO.WriteUInt32ToStream(this.spellOffsets[index].Offset, output);

            //Class counts
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                ReusableIO.WriteUInt32ToStream(this.spellOffsets[index].Count, output);

            //Domain offsets
            for (Int32 index = 63; index < 72 /* 9 more */; ++index)
                ReusableIO.WriteUInt32ToStream(this.spellOffsets[index].Offset, output);

            //Domain counts
            for (Int32 index = 63; index < 72 /* 9 more */; ++index)
                ReusableIO.WriteUInt32ToStream(this.spellOffsets[index].Count, output);

            //Abilities
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[72].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[72].Count, output);

            //Songs
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[73].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[73].Count, output);

            //Shapes
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[74].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.spellOffsets[74].Count, output);
        }

        /// <summary>Writes the header's footer, containing item & effect offsets & counts and dialog</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteFooter(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.offsetItemSlots, output);
            ReusableIO.WriteUInt32ToStream(this.offsetItems, output);
            ReusableIO.WriteUInt32ToStream(this.countItems, output);
            ReusableIO.WriteUInt32ToStream(this.offsetEffects, output);
            ReusableIO.WriteUInt32ToStream(this.countEffects, output);
            ReusableIO.WriteStringToStream(this.dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
        }
        #endregion

        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.ToStringCreatureVersion(builder);
            this.ToStringLeadingValues(builder);
            this.ToStringSoundSet(builder);
            this.ToStringProficienciesFeatsAndSkills(builder);
            this.ToStringAfterProcifiencies(builder);
            this.ToStringIcewindDaleAdditions(builder);
            this.ToStringClassifications(builder);
            this.ToStringBeforeOffsets(builder);
            this.ToStringOffsetsAndCounts(builder);
            this.ToStringHeaderFooter(builder);

            return builder.ToString();
        }

        /// <summary>Returns the printable read-friendly version of the creature format</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringCreatureVersion(StringBuilder builder)
        {
            builder.AppendLine("Creature version 2.2 header:");
        }

        /// <summary>Generates a String representing the leading D20 creature structure data</summary>
        protected void ToStringLeadingValues(StringBuilder builder)
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
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Crushing)"));
            builder.Append(this.armorClassModifierCrushing);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Missile)"));
            builder.Append(this.armorClassModifierMissile);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Piercing)"));
            builder.Append(this.armorClassModifierPiercing);
            builder.Append(StringFormat.ToStringAlignment("Armor Class Modifier (Slashing)"));
            builder.Append(this.armorClassModifierSlashing);
            builder.Append(StringFormat.ToStringAlignment("Base Attack Bonus"));
            builder.Append(this.attackBase);
            builder.Append(StringFormat.ToStringAlignment("Attacks per round"));
            builder.Append(this.attacksPerRound);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Fortitude)"));
            builder.Append(this.savingThrowFortitude);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Reflex)"));
            builder.Append(this.savingThrowReflex);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Will)"));
            builder.Append(this.savingThrowWill);
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
            builder.Append(StringFormat.ToStringAlignment("Damage resistance (Magic)"));
            builder.Append(this.resistMagicDamage);
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.unknown1);
            builder.Append(StringFormat.ToStringAlignment("Fatigue"));
            builder.Append(this.fatigue);
            builder.Append(StringFormat.ToStringAlignment("Intoxication"));
            builder.Append(this.intoxication);
            builder.Append(StringFormat.ToStringAlignment("Luck"));
            builder.Append(this.luck);
            builder.Append(StringFormat.ToStringAlignment("Turn Undead Level"));
            builder.Append(this.turnUndeadLevel);
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown2));
            builder.Append(StringFormat.ToStringAlignment("Total Levels"));
            builder.Append(this.levelsTotal);
            builder.Append(StringFormat.ToStringAlignment("Barbarian Levels"));
            builder.Append(this.levelsOfBarbarian);
            builder.Append(StringFormat.ToStringAlignment("Bard Levels"));
            builder.Append(this.levelsOfBard);
            builder.Append(StringFormat.ToStringAlignment("Cleric Levels"));
            builder.Append(this.levelsOfCleric);
            builder.Append(StringFormat.ToStringAlignment("Druid Levels"));
            builder.Append(this.levelsOfDruid);
            builder.Append(StringFormat.ToStringAlignment("Fighter Levels"));
            builder.Append(this.levelsOfFighter);
            builder.Append(StringFormat.ToStringAlignment("Monk Levels"));
            builder.Append(this.levelsOfMonk);
            builder.Append(StringFormat.ToStringAlignment("Paladin Levels"));
            builder.Append(this.levelsOfPaladin);
            builder.Append(StringFormat.ToStringAlignment("Ranger Levels"));
            builder.Append(this.levelsOfRanger);
            builder.Append(StringFormat.ToStringAlignment("Rogue Levels"));
            builder.Append(this.levelsOfRogue);
            builder.Append(StringFormat.ToStringAlignment("Sorcerer Levels"));
            builder.Append(this.levelsOfSorcerer);
            builder.Append(StringFormat.ToStringAlignment("Wizard Levels"));
            builder.Append(this.levelsOfWizard);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown3));
        }

        /// <summary>Generates a String representing the sound set area of the creature data structure</summary>
        protected void ToStringSoundSet(StringBuilder builder)
        {
            foreach (String key in this.soundSet.Keys) //i < this.soundSet.Count
            {
                builder.Append(StringFormat.ToStringAlignment("Soundset " + "(" + key + ")"));
                builder.Append(this.soundSet[key].StringReferenceIndex);
            }
        }

        /// <summary>Generates a String representing the proficiencies area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringProficienciesFeatsAndSkills(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Team script"));
            builder.Append(String.Format("'{0}'", this.scriptTeam.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Special script #1"));
            builder.Append(String.Format("'{0}'", this.scriptSpecial1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Creature enchantment level (weapon to-hit)"));
            builder.Append(this.creatureEnchantmentLevel);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #1"));
            builder.Append((UInt64)this.featsFlag1);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #1 (enumerated)"));
            builder.Append(this.GetFeatFlags1String());
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #2"));
            builder.Append((UInt64)this.featsFlag2);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #2 (enumerated)"));
            builder.Append(this.GetFeatFlags2String());
            builder.Append(StringFormat.ToStringAlignment("Reserved Creature Flags"));
            builder.Append(this.featsFlagReserved);

            //Weapon proficiencies
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Bow)"));
            builder.Append(this.martialProficiencyBow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Crossbow)"));
            builder.Append(this.simpleProficiencyCrossbow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Missile)"));
            builder.Append(this.simpleProficiencyMissile);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Axe)"));
            builder.Append(this.martialProficiencyAxe);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Mace)"));
            builder.Append(this.simpleProficiencyMace);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Flail)"));
            builder.Append(this.martialProficiencyFlail);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Polearm)"));
            builder.Append(this.martialProficiencyPolearm);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Hammer)"));
            builder.Append(this.martialProficiencyHammer);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Quarterstaff)"));
            builder.Append(this.simpleProficiencyQuarterstaff);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Great Sword)"));
            builder.Append(this.martialProficiencyGreatSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Large Sword)"));
            builder.Append(this.martialProficiencyLargeSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Small Blade)"));
            builder.Append(this.simpleProficiencySmallBlade);
            builder.Append(StringFormat.ToStringAlignment("Feat (Toughness)"));
            builder.Append(this.featToughness);
            builder.Append(StringFormat.ToStringAlignment("Feat (Armored Arcana)"));
            builder.Append(this.featArmoredArcana);
            builder.Append(StringFormat.ToStringAlignment("Feat (Cleave)"));
            builder.Append(this.featCleave);
            builder.Append(StringFormat.ToStringAlignment("Feat (Armor Proficiency)"));
            builder.Append(this.featArmorProficiency);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Enchantment)"));
            builder.Append(this.featSpellFocusEnchantment);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Evocation)"));
            builder.Append(this.featSpellFocusEvocation);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Necromancy)"));
            builder.Append(this.featSpellFocusNecromancy);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Transmutation)"));
            builder.Append(this.featSpellFocusTransmutation);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Penetration)"));
            builder.Append(this.featSpellPenetration);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Rage)"));
            builder.Append(this.featExtraRage);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Wild Shape)"));
            builder.Append(this.featExtraWildShape);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Smiting)"));
            builder.Append(this.featExtraSmiting);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Turning)"));
            builder.Append(this.featExtraTurning);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Exotic, Bastard Sword)"));
            builder.Append(this.exoticProficiencyBastardSword);
            builder.Append(StringFormat.ToStringAlignment("Remaining reserved Feat slots:"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown4));

            //Skills
            builder.Append(StringFormat.ToStringAlignment("Skill (Alchemy)"));
            builder.Append(this.skillAlchemy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Animal Empathy)"));
            builder.Append(this.skillAnimalEmpathy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Bluff)"));
            builder.Append(this.skillBluff);
            builder.Append(StringFormat.ToStringAlignment("Skill (Concentration)"));
            builder.Append(this.skillConcentration);
            builder.Append(StringFormat.ToStringAlignment("Skill (Diplomacy)"));
            builder.Append(this.skillDiplomacy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Disable Device)"));
            builder.Append(this.skillDisableDevice);
            builder.Append(StringFormat.ToStringAlignment("Skill (Hide)"));
            builder.Append(this.skillHide);
            builder.Append(StringFormat.ToStringAlignment("Skill (Intimidate)"));
            builder.Append(this.skillIntimidate);
            builder.Append(StringFormat.ToStringAlignment("Skill (Knowledge, Arcana)"));
            builder.Append(this.skillKnowledgeArcana);
            builder.Append(StringFormat.ToStringAlignment("Skill (Move Silently)"));
            builder.Append(this.skillMoveSilently);
            builder.Append(StringFormat.ToStringAlignment("Skill (Open Lock)"));
            builder.Append(this.skillOpenLock);
            builder.Append(StringFormat.ToStringAlignment("Skill (Pick Pocket)"));
            builder.Append(this.skillPickPocket);
            builder.Append(StringFormat.ToStringAlignment("Skill (Search)"));
            builder.Append(this.skillSearch);
            builder.Append(StringFormat.ToStringAlignment("Skill (Spellcraft)"));
            builder.Append(this.skillSpellcraft);
            builder.Append(StringFormat.ToStringAlignment("Skill (Use Magic Device)"));
            builder.Append(this.skillUseMagicDevice);
            builder.Append(StringFormat.ToStringAlignment("Skill (Wilderness Lore)"));
            builder.Append(this.skillWildernessLore);
            builder.Append(StringFormat.ToStringAlignment("Remaining reserved Skill slots:"));
            builder.Append(StringFormat.ByteArrayToHexString(this.skillReserved));
        }
        
        /// <summary>Generates a String representing the ability scores and simila after the proficiencis area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringAfterProcifiencies(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Challenge Rating"));
            builder.Append(this.challengeRating);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #1"));
            builder.Append(this.favoredEnemy1);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #2"));
            builder.Append(this.favoredEnemy2);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #3"));
            builder.Append(this.favoredEnemy3);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #4"));
            builder.Append(this.favoredEnemy4);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #5"));
            builder.Append(this.favoredEnemy5);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #6"));
            builder.Append(this.favoredEnemy6);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #7"));
            builder.Append(this.favoredEnemy7);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #8"));
            builder.Append(this.favoredEnemy8);
            builder.Append(StringFormat.ToStringAlignment("Subrace"));
            builder.Append(this.subrace);
            builder.Append(StringFormat.ToStringAlignment("Unknown #5"));
            builder.Append(this.unknown5);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Strength)"));
            builder.Append(this.scoreStrength);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Intelligence)"));
            builder.Append(this.scoreIntelligence);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Wisdom)"));
            builder.Append(this.scoreWisdom);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Dexterity)"));
            builder.Append(this.scoreDexterity);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Constitution)"));
            builder.Append(this.scoreConstitution);
            builder.Append(StringFormat.ToStringAlignment("Ability Score (Charisma)"));
            builder.Append(this.scoreCharisma);
            builder.Append(StringFormat.ToStringAlignment("Morale"));
            builder.Append(this.morale);
            builder.Append(StringFormat.ToStringAlignment("Morale Break"));
            builder.Append(this.moraleBreak);
            builder.Append(StringFormat.ToStringAlignment("Morale Recovery Time"));
            builder.Append(this.moraleRecoveryTime);
            builder.Append(StringFormat.ToStringAlignment("Archetype"));
            builder.Append((UInt32)this.archetype);
            builder.Append(StringFormat.ToStringAlignment("Archetypes (enumerated)"));
            builder.Append(this.GetArchetypeString());
            builder.Append(StringFormat.ToStringAlignment("Script (Override)"));
            builder.Append(String.Format("'{0}'", this.scriptOverride.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Special #2)"));
            builder.Append(String.Format("'{0}'", this.scriptSpecial2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Combat)"));
            builder.Append(String.Format("'{0}'", this.scriptCombat.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Special #3)"));
            builder.Append(String.Format("'{0}'", this.scriptSpecial3.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Movement)"));
            builder.Append(String.Format("'{0}'", this.scriptMovement.ZResRef));
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
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.SecondaryDeathVariable.Value)));
            builder.Append(StringFormat.ToStringAlignment("Tertiary Death variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.TertiaryDeathVariable.Value)));
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown2));
            builder.Append(StringFormat.ToStringAlignment("Saved X Co-ordinate"));
            builder.Append(this.savedCoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Saved Y Co-ordinate"));
            builder.Append(this.savedCoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Saved Orientation"));
            builder.Append(this.savedOrientation);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown3));
        }

        /// <summary>Generates a String representing the added Planescape: Torment values area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringClassifications(StringBuilder builder)
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
        protected void ToStringBeforeOffsets(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Global enumerator"));
            builder.Append(this.enumGlobal);
            builder.Append(StringFormat.ToStringAlignment("Local enumerator"));
            builder.Append(this.enumLocal);
            builder.Append(StringFormat.ToStringAlignment("Death variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.deathVariable.Value)));
            builder.Append(StringFormat.ToStringAlignment("AV Class"));
            builder.Append(this.avClass);
            builder.Append(StringFormat.ToStringAlignment("Class Mask"));
            builder.Append(this.classMsk);
            builder.Append(StringFormat.ToStringAlignment("Unknown #9"));
            builder.Append(this.unknown9);
        }

        /// <summary>Generates a String representing the header's spell offsets</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringOffsetsAndCounts(StringBuilder builder)
        {
            //out of order, but hey
            foreach (String key in this.spellOffsets.Keys)
            {
                builder.Append(StringFormat.ToStringAlignment(key + " offset"));
                builder.Append(this.spellOffsets[key].Offset);
                builder.Append(StringFormat.ToStringAlignment(key + " count"));
                builder.Append(this.spellOffsets[key].Count);
            }
        }

        /// <summary>Generates a String representing the trailing footer area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringHeaderFooter(StringBuilder builder)
        {
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

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetCreatureFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.KeepCastingWhenDamaged) == CreatureD20Flags.KeepCastingWhenDamaged, CreatureD20Flags.KeepCastingWhenDamaged.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.NoCorpse) == CreatureD20Flags.NoCorpse, CreatureD20Flags.NoCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.PermanentCorpse) == CreatureD20Flags.PermanentCorpse, CreatureD20Flags.PermanentCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassFighter) == CreatureD20Flags.OriginalClassFighter, CreatureD20Flags.OriginalClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassMage) == CreatureD20Flags.OriginalClassMage, CreatureD20Flags.OriginalClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassCleric) == CreatureD20Flags.OriginalClassCleric, CreatureD20Flags.OriginalClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassThief) == CreatureD20Flags.OriginalClassThief, CreatureD20Flags.OriginalClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassDruid) == CreatureD20Flags.OriginalClassDruid, CreatureD20Flags.OriginalClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.OriginalClassRanger) == CreatureD20Flags.OriginalClassRanger, CreatureD20Flags.OriginalClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.FallenPaladin) == CreatureD20Flags.FallenPaladin, CreatureD20Flags.FallenPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.FallenRanger) == CreatureD20Flags.FallenRanger, CreatureD20Flags.FallenRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.Exportable) == CreatureD20Flags.Exportable, CreatureD20Flags.Exportable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.HideInjuryStatusInTooltip) == CreatureD20Flags.HideInjuryStatusInTooltip, CreatureD20Flags.HideInjuryStatusInTooltip.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.QuestCritical) == CreatureD20Flags.QuestCritical, CreatureD20Flags.QuestCritical.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.ActivatesCannotBeUsedByNpcs) == CreatureD20Flags.ActivatesCannotBeUsedByNpcs, CreatureD20Flags.ActivatesCannotBeUsedByNpcs.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.BeenInParty) == CreatureD20Flags.BeenInParty, CreatureD20Flags.BeenInParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.SeenParty) == CreatureD20Flags.SeenParty, CreatureD20Flags.SeenParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.Invulnerable) == CreatureD20Flags.Invulnerable, CreatureD20Flags.Invulnerable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.NonThreateningEnemy) == CreatureD20Flags.NonThreateningEnemy, CreatureD20Flags.NonThreateningEnemy.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.NoTalk) == CreatureD20Flags.NoTalk, CreatureD20Flags.NoTalk.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.IgnoreReturnToStart) == CreatureD20Flags.IgnoreReturnToStart, CreatureD20Flags.IgnoreReturnToStart.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.IgnoreInhibitAI) == CreatureD20Flags.IgnoreInhibitAI, CreatureD20Flags.IgnoreInhibitAI.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & CreatureD20Flags.UnknownCorpseRelated) == CreatureD20Flags.UnknownCorpseRelated, CreatureD20Flags.UnknownCorpseRelated.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFeatFlags1String()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.AEGIS_OF_RIME) == D20Feats1.AEGIS_OF_RIME, D20Feats1.AEGIS_OF_RIME.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.AMBIDEXTERITY) == D20Feats1.AMBIDEXTERITY, D20Feats1.AMBIDEXTERITY.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.AQUA_MORTIS) == D20Feats1.AQUA_MORTIS, D20Feats1.AQUA_MORTIS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.ARMOR_PROF) == D20Feats1.ARMOR_PROF, D20Feats1.ARMOR_PROF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.ARMORED_ARCANA) == D20Feats1.ARMORED_ARCANA, D20Feats1.ARMORED_ARCANA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.ARTERIAL_STRIKE) == D20Feats1.ARTERIAL_STRIKE, D20Feats1.ARTERIAL_STRIKE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.BLIND_FIGHT) == D20Feats1.BLIND_FIGHT, D20Feats1.BLIND_FIGHT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.BULLHEADED) == D20Feats1.BULLHEADED, D20Feats1.BULLHEADED.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.CLEAVE) == D20Feats1.CLEAVE, D20Feats1.CLEAVE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.COMBAT_CASTING) == D20Feats1.COMBAT_CASTING, D20Feats1.COMBAT_CASTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.CONUNDRUM) == D20Feats1.CONUNDRUM, D20Feats1.CONUNDRUM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.CRIPPLING_STRIKE) == D20Feats1.CRIPPLING_STRIKE, D20Feats1.CRIPPLING_STRIKE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.DASH) == D20Feats1.DASH, D20Feats1.DASH.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.DEFLECT_ARROWS) == D20Feats1.DEFLECT_ARROWS, D20Feats1.DEFLECT_ARROWS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.DIRTY_FIGHTING) == D20Feats1.DIRTY_FIGHTING, D20Feats1.DIRTY_FIGHTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.DISCIPLINE) == D20Feats1.DISCIPLINE, D20Feats1.DISCIPLINE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.DODGE) == D20Feats1.DODGE, D20Feats1.DODGE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.ENVENOM_WEAPON) == D20Feats1.ENVENOM_WEAPON, D20Feats1.ENVENOM_WEAPON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXOTIC_BASTARD) == D20Feats1.EXOTIC_BASTARD, D20Feats1.EXOTIC_BASTARD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXPERTISE) == D20Feats1.EXPERTISE, D20Feats1.EXPERTISE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXTRA_RAGE) == D20Feats1.EXTRA_RAGE, D20Feats1.EXTRA_RAGE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXTRA_SHAPESHIFTING) == D20Feats1.EXTRA_SHAPESHIFTING, D20Feats1.EXTRA_SHAPESHIFTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXTRA_SMITING) == D20Feats1.EXTRA_SMITING, D20Feats1.EXTRA_SMITING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.EXTRA_TURNING) == D20Feats1.EXTRA_TURNING, D20Feats1.EXTRA_TURNING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.FIENDSLAYER) == D20Feats1.FIENDSLAYER, D20Feats1.FIENDSLAYER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.FORESTER) == D20Feats1.FORESTER, D20Feats1.FORESTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.GREAT_FORTITUDE) == D20Feats1.GREAT_FORTITUDE, D20Feats1.GREAT_FORTITUDE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.HAMSTRING) == D20Feats1.HAMSTRING, D20Feats1.HAMSTRING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.HERETICS_BANE) == D20Feats1.HERETICS_BANE, D20Feats1.HERETICS_BANE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.HEROIC_INSPIRATION) == D20Feats1.HEROIC_INSPIRATION, D20Feats1.HEROIC_INSPIRATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.IMPROVED_CRITICAL) == D20Feats1.IMPROVED_CRITICAL, D20Feats1.IMPROVED_CRITICAL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.IMPROVED_EVASION) == D20Feats1.IMPROVED_EVASION, D20Feats1.IMPROVED_EVASION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.IMPROVED_INITIATIVE) == D20Feats1.IMPROVED_INITIATIVE, D20Feats1.IMPROVED_INITIATIVE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.IMPROVED_TURNING) == D20Feats1.IMPROVED_TURNING, D20Feats1.IMPROVED_TURNING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.IRON_WILL) == D20Feats1.IRON_WILL, D20Feats1.IRON_WILL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.LIGHTNING_REFLEXES) == D20Feats1.LIGHTNING_REFLEXES, D20Feats1.LIGHTNING_REFLEXES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.LINGERING_SONG) == D20Feats1.LINGERING_SONG, D20Feats1.LINGERING_SONG.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.LUCK_OF_HEROES) == D20Feats1.LUCK_OF_HEROES, D20Feats1.LUCK_OF_HEROES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_AXE) == D20Feats1.MARTIAL_AXE, D20Feats1.MARTIAL_AXE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_BOW) == D20Feats1.MARTIAL_BOW, D20Feats1.MARTIAL_BOW.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_FLAIL) == D20Feats1.MARTIAL_FLAIL, D20Feats1.MARTIAL_FLAIL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_GREATSWORD) == D20Feats1.MARTIAL_GREATSWORD, D20Feats1.MARTIAL_GREATSWORD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_HAMMER) == D20Feats1.MARTIAL_HAMMER, D20Feats1.MARTIAL_HAMMER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_LARGESWORD) == D20Feats1.MARTIAL_LARGESWORD, D20Feats1.MARTIAL_LARGESWORD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MARTIAL_POLEARM) == D20Feats1.MARTIAL_POLEARM, D20Feats1.MARTIAL_POLEARM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MAXIMIZED_ATTACKS) == D20Feats1.MAXIMIZED_ATTACKS, D20Feats1.MAXIMIZED_ATTACKS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.MERCANTILE_BACKGROUND) == D20Feats1.MERCANTILE_BACKGROUND, D20Feats1.MERCANTILE_BACKGROUND.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.POWER_ATTACK) == D20Feats1.POWER_ATTACK, D20Feats1.POWER_ATTACK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.PRECISE_SHOT) == D20Feats1.PRECISE_SHOT, D20Feats1.PRECISE_SHOT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.RAPID_SHOT) == D20Feats1.RAPID_SHOT, D20Feats1.RAPID_SHOT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.RESIST_POISON) == D20Feats1.RESIST_POISON, D20Feats1.RESIST_POISON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SCION_OF_STORMS) == D20Feats1.SCION_OF_STORMS, D20Feats1.SCION_OF_STORMS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SHIELD_PROF) == D20Feats1.SHIELD_PROF, D20Feats1.SHIELD_PROF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SIMPLE_CROSSBOW) == D20Feats1.SIMPLE_CROSSBOW, D20Feats1.SIMPLE_CROSSBOW.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SIMPLE_MACE) == D20Feats1.SIMPLE_MACE, D20Feats1.SIMPLE_MACE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SIMPLE_MISSILE) == D20Feats1.SIMPLE_MISSILE, D20Feats1.SIMPLE_MISSILE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SIMPLE_QUARTERSTAFF) == D20Feats1.SIMPLE_QUARTERSTAFF, D20Feats1.SIMPLE_QUARTERSTAFF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SIMPLE_SMALLBLADE) == D20Feats1.SIMPLE_SMALLBLADE, D20Feats1.SIMPLE_SMALLBLADE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SLIPPERY_MIND) == D20Feats1.SLIPPERY_MIND, D20Feats1.SLIPPERY_MIND.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SNAKE_BLOOD) == D20Feats1.SNAKE_BLOOD, D20Feats1.SNAKE_BLOOD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SPELL_FOCUS_ENCHANTMENT) == D20Feats1.SPELL_FOCUS_ENCHANTMENT, D20Feats1.SPELL_FOCUS_ENCHANTMENT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SPELL_FOCUS_EVOCATION) == D20Feats1.SPELL_FOCUS_EVOCATION, D20Feats1.SPELL_FOCUS_EVOCATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SPELL_FOCUS_NECROMANCY) == D20Feats1.SPELL_FOCUS_NECROMANCY, D20Feats1.SPELL_FOCUS_NECROMANCY.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag1 & D20Feats1.SPELL_FOCUS_TRANSMUTE) == D20Feats1.SPELL_FOCUS_TRANSMUTE, D20Feats1.SPELL_FOCUS_TRANSMUTE.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFeatFlags2String()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.SPELL_PENETRATION) == D20Feats2.SPELL_PENETRATION, D20Feats2.SPELL_PENETRATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.SPIRIT_OF_FLAME) == D20Feats2.SPIRIT_OF_FLAME, D20Feats2.SPIRIT_OF_FLAME.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.STRONG_BACK) == D20Feats2.STRONG_BACK, D20Feats2.STRONG_BACK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.STUNNING_FIST) == D20Feats2.STUNNING_FIST, D20Feats2.STUNNING_FIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.SUBVOCAL_CASTING) == D20Feats2.SUBVOCAL_CASTING, D20Feats2.SUBVOCAL_CASTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.TOUGHNESS) == D20Feats2.TOUGHNESS, D20Feats2.TOUGHNESS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.TWO_WEAPON_FIGHTING) == D20Feats2.TWO_WEAPON_FIGHTING, D20Feats2.TWO_WEAPON_FIGHTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.WEAPON_FINESSE) == D20Feats2.WEAPON_FINESSE, D20Feats2.WEAPON_FINESSE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.WILDSHAPE_BOAR) == D20Feats2.WILDSHAPE_BOAR, D20Feats2.WILDSHAPE_BOAR.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.WILDSHAPE_PANTHER) == D20Feats2.WILDSHAPE_PANTHER, D20Feats2.WILDSHAPE_PANTHER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.featsFlag2 & D20Feats2.WILDSHAPE_SHAMBLER) == D20Feats2.WILDSHAPE_SHAMBLER, D20Feats2.WILDSHAPE_SHAMBLER.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which D20 arcetype flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetArchetypeString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.PALADIN_ILMATER) == KitD20.PALADIN_ILMATER, KitD20.PALADIN_ILMATER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.PALADIN_HELM) == KitD20.PALADIN_HELM, KitD20.PALADIN_HELM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.PALADIN_MYSTRA) == KitD20.PALADIN_MYSTRA, KitD20.PALADIN_MYSTRA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MONK_OLD_ORDER) == KitD20.MONK_OLD_ORDER, KitD20.MONK_OLD_ORDER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MONK_BROKEN_ONES) == KitD20.MONK_BROKEN_ONES, KitD20.MONK_BROKEN_ONES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MONK_DARK_MOON) == KitD20.MONK_DARK_MOON, KitD20.MONK_DARK_MOON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_ABJURER) == KitD20.MAGE_ABJURER, KitD20.MAGE_ABJURER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_CONJURER) == KitD20.MAGE_CONJURER, KitD20.MAGE_CONJURER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_DIVINER) == KitD20.MAGE_DIVINER, KitD20.MAGE_DIVINER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_ENCHANTER) == KitD20.MAGE_ENCHANTER, KitD20.MAGE_ENCHANTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_EVOKER) == KitD20.MAGE_EVOKER, KitD20.MAGE_EVOKER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_ILLUSIONIST) == KitD20.MAGE_ILLUSIONIST, KitD20.MAGE_ILLUSIONIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_NECROMANCER) == KitD20.MAGE_NECROMANCER, KitD20.MAGE_NECROMANCER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_TRANSMUTER) == KitD20.MAGE_TRANSMUTER, KitD20.MAGE_TRANSMUTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.MAGE_GENERALIST) == KitD20.MAGE_GENERALIST, KitD20.MAGE_GENERALIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_ILMATER) == KitD20.CLERIC_ILMATER, KitD20.CLERIC_ILMATER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_LATHANDER) == KitD20.CLERIC_LATHANDER, KitD20.CLERIC_LATHANDER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_SELUNE) == KitD20.CLERIC_SELUNE, KitD20.CLERIC_SELUNE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_HELM) == KitD20.CLERIC_HELM, KitD20.CLERIC_HELM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_OGHMA) == KitD20.CLERIC_OGHMA, KitD20.CLERIC_OGHMA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_TEMPUS) == KitD20.CLERIC_TEMPUS, KitD20.CLERIC_TEMPUS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_BANE) == KitD20.CLERIC_BANE, KitD20.CLERIC_BANE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_MASK) == KitD20.CLERIC_MASK, KitD20.CLERIC_MASK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.archetype & KitD20.CLERIC_TALOS) == KitD20.CLERIC_TALOS, KitD20.CLERIC_TALOS.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}