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
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 1582;
        #endregion


        #region Fields
        /// <summary>D20 creature flags</summary>
        public CreatureD20Flags Flags { get; set; }

        /// <summary>Saving throw modifier vs. Fortitude</summary>
        public Byte SavingThrowFortitude { get; set; }

        /// <summary>Saving throw modifier vs. Reflex</summary>
        public Byte SavingThrowReflex { get; set; }

        /// <summary>Saving throw modifier vs. Will</summary>
        public Byte SavingThrowWill { get; set; }

        /// <summary>Magic damage resistance</summary>
        public Byte ResistMagicDamage { get; set; }

        /// <summary>Unknown 4 bytes. Further resistances?</summary>
        public UInt32 Unknown1 { get; set; }

        /// <summary>Level at which you can turn undead</summary>
        public Byte TurnUndeadLevel { get; set; }

        /// <summary>33 unknown bytes</summary>
        public Byte[] Unknown2 { get; set; }

        /// <summary>Total levels taken</summary>
        public Byte LevelsTotal { get; set; }

        /// <summary>Number of levels of Barbarian taken</summary>
        public Byte LevelsOfBarbarian { get; set; }

        /// <summary>Number of levels of Bard taken</summary>
        public Byte LevelsOfBard { get; set; }

        /// <summary>Number of levels of Cleric taken</summary>
        public Byte LevelsOfCleric { get; set; }

        /// <summary>Number of levels of Druid taken</summary>
        public Byte LevelsOfDruid { get; set; }

        /// <summary>Number of levels of Fighter taken</summary>
        public Byte LevelsOfFighter { get; set; }

        /// <summary>Number of levels of Monk taken</summary>
        public Byte LevelsOfMonk { get; set; }

        /// <summary>Number of levels of Paladin taken</summary>
        public Byte LevelsOfPaladin { get; set; }

        /// <summary>Number of levels of Ranger taken</summary>
        public Byte LevelsOfRanger { get; set; }

        /// <summary>Number of levels of Rogue taken</summary>
        public Byte LevelsOfRogue { get; set; }

        /// <summary>Number of levels of Sorcerer taken</summary>
        public Byte LevelsOfSorcerer { get; set; }

        /// <summary>Number of levels of Barbarian taken</summary>
        public Byte LevelsOfWizard { get; set; }

        /// <summary>22 unknown bytes</summary>
        public Byte[] Unknown3;

        //64 soundset entries

        /// <summary>Team script</summary>
        public ResourceReference ScriptTeam { get; set; }

        /// <summary>Special script 1</summary>
        public ResourceReference ScriptSpecial1 { get; set; }

        /// <summary>Magical enchantment level of creature (to hit, immunity, etc.)</summary>
        /// <remarks>Might just be a Byte, but three following are all 0</remarks>
        public Int32 CreatureEnchantmentLevel { get; set; }

        //24 bytes of feats.
        //FEATS.IDS suggests this uses 75 bits
        //DLTCEP suggests 192 bits allocated (6 32-bit ints)
        // So, three 64-bit entries, two being flag fields, other reserved feats field
        //See Also FEATS.2DA

        /// <summary>First 64 bits of feat flags</summary>
        public D20Feats1 FeatsFlag1 { get; set; }

        /// <summary>Second 64 bits of feat flags</summary>
        public D20Feats2 FeatsFlag2 { get; set; }

        /// <summary>Third/reserved 64 bits of feat flags</summary>
        public UInt64 FeatsFlagReserved { get; set; }

        //Weapon proficiencies
        /// <summary>Bow weapon proficiency</summary>
        public Byte MartialProficiencyBow { get; set; }

        /// <summary>Crossbow weapon proficiency</summary>
        public Byte SimpleProficiencyCrossbow { get; set; }

        /// <summary>Missile weapon proficiency</summary>
        public Byte SimpleProficiencyMissile { get; set; }

        /// <summary>Axe weapon proficiency</summary>
        public Byte MartialProficiencyAxe { get; set; }

        /// <summary>Mace weapon proficiency</summary>
        public Byte SimpleProficiencyMace { get; set; }

        /// <summary>Flail weapon proficiency</summary>
        public Byte MartialProficiencyFlail { get; set; }

        /// <summary>Polearm weapon proficiency</summary>
        public Byte MartialProficiencyPolearm { get; set; }

        /// <summary>Hammer weapon proficiency</summary>
        public Byte MartialProficiencyHammer { get; set; }

        /// <summary>Quarterstaff weapon proficiency</summary>
        public Byte SimpleProficiencyQuarterstaff { get; set; }

        /// <summary>Great Sword weapon proficiency</summary>
        public Byte MartialProficiencyGreatSword { get; set; }

        /// <summary>Large Sword weapon proficiency</summary>
        public Byte MartialProficiencyLargeSword { get; set; }

        /// <summary>Small blade weapon proficiency</summary>
        public Byte SimpleProficiencySmallBlade { get; set; }

        //Feat ranks taken

        /// <summary>Number of times Toughness has been taken</summary>
        public Byte FeatToughness { get; set; }

        /// <summary>Number of times Armored Arcana has been taken</summary>
        public Byte FeatArmoredArcana { get; set; }

        /// <summary>Number of times Cleave has been taken</summary>
        public Byte FeatCleave { get; set; }

        /// <summary>Number of times Armor Proficiency has been taken</summary>
        public Byte FeatArmorProficiency { get; set; }

        /// <summary>Number of times Spell Focus: Enchantment has been taken</summary>
        public Byte FeatSpellFocusEnchantment { get; set; }

        /// <summary>Number of times Spell Focus: Evocation has been taken</summary>
        public Byte FeatSpellFocusEvocation { get; set; }

        /// <summary>Number of times Spell Focus: Necromancy has been taken</summary>
        public Byte FeatSpellFocusNecromancy { get; set; }

        /// <summary>Number of times Spell Focus: Transmutation has been taken</summary>
        public Byte FeatSpellFocusTransmutation { get; set; }

        /// <summary>Number of times Spell Penetration has been taken</summary>
        public Byte FeatSpellPenetration { get; set; }

        /// <summary>Number of times Extra Rage has been taken</summary>
        public Byte FeatExtraRage { get; set; }

        /// <summary>Number of times Extra Wild Shape has been taken</summary>
        public Byte FeatExtraWildShape { get; set; }

        /// <summary>Number of times Extra Smiting has been taken</summary>
        public Byte FeatExtraSmiting { get; set; }

        /// <summary>Number of times Extra Turning has been taken</summary>
        public Byte FeatExtraTurning { get; set; }

        /// <summary>Bastard Sword weapon proficiency</summary>
        public Byte ExoticProficiencyBastardSword { get; set; }

        /// <summary>38 bytes of unknown values. Most likely, ranks in specific feats.</summary>
        public Byte[] Unknown4 { get; set; }

        // Skills

        /// <summary>Skill ranks taken in Alchemy</summary>
        public Byte SkillAlchemy { get; set; }

        /// <summary>Skill ranks taken in Animal Empathy</summary>
        public Byte SkillAnimalEmpathy { get; set; }

        /// <summary>Skill ranks taken in Bluff</summary>
        public Byte SkillBluff { get; set; }

        /// <summary>Skill ranks taken in Concentration</summary>
        public Byte SkillConcentration { get; set; }

        /// <summary>Skill ranks taken in Diplomacy</summary>
        public Byte SkillDiplomacy { get; set; }

        /// <summary>Skill ranks taken in Disable Device</summary>
        public Byte SkillDisableDevice { get; set; }
        
        /// <summary>Skill ranks taken in Hide</summary>
        public Byte SkillHide { get; set; }
        
        /// <summary>Skill ranks taken in Intimidate</summary>
        public Byte SkillIntimidate { get; set; }

        /// <summary>Skill ranks taken in Knowledge (Arcana)</summary>
        public Byte SkillKnowledgeArcana { get; set; }
        
        /// <summary>Skill ranks taken in Move Silently</summary>
        public Byte SkillMoveSilently { get; set; }

        /// <summary>Skill ranks taken in Open Lock</summary>
        public Byte SkillOpenLock { get; set; }

        /// <summary>Skill ranks taken in Pick Pocket</summary>
        public Byte SkillPickPocket { get; set; }

        /// <summary>Skill ranks taken in Search</summary>
        public Byte SkillSearch { get; set; }

        /// <summary>Skill ranks taken in Spellcraft</summary>
        public Byte SkillSpellcraft { get; set; }

        /// <summary>Skill ranks taken in Use Magic Device</summary>
        public Byte SkillUseMagicDevice { get; set; }

        /// <summary>Skill ranks taken in Wilderness Lore</summary>
        public Byte SkillWildernessLore { get; set; }

        /// <summary>50 bytes reserved for further skills</summary>
        public Byte[] SkillReserved { get; set; }

        /// <summary>CR of the creature</summary>
        /// <value>See MONCRATE.2DA</value>
        public Byte ChallengeRating { get; set; }

        /// <summary>First favored enemy</summary>
        public Byte FavoredEnemy1 { get; set; }

        /// <summary>Second favored enemy</summary>
        public Byte FavoredEnemy2 { get; set; }

        /// <summary>Third favored enemy</summary>
        public Byte FavoredEnemy3 { get; set; }

        /// <summary>Fourth favored enemy</summary>
        public Byte FavoredEnemy4 { get; set; }

        /// <summary>Fifth favored enemy</summary>
        public Byte FavoredEnemy5 { get; set; }

        /// <summary>Sixth favored enemy</summary>
        public Byte FavoredEnemy6 { get; set; }

        /// <summary>Seventh favored enemy</summary>
        public Byte FavoredEnemy7 { get; set; }

        /// <summary>Eighth favored enemy</summary>
        public Byte FavoredEnemy8 { get; set; }

        /// <summary>Creature's sub-race</summary>
        /// <remarks>Match to SUBRACE.IDS</remarks>
        public Byte Subrace { get; set; }

        /// <summary>Unknown two bytes after subrace</summary>
        /// <value>Observed either 1 or 2</value>
        /// <remarks>
        ///     SEX, matching to GENDER.IDS?
        ///     seems unused, but roughly matches the gender field.
        ///     Could be the gender duplicate seen in other IE iterations.
        /// </remarks>
        public UInt16 Unknown5 { get; set; }

        //ability scores

        //it looks like morale, morale break, and morale recovery time ARE in here, from DLTCEP.
        //This is unknown at 0x26C in IESDP

        /// <summary>Class archetypes</summary>
        public KitD20 Archetype { get; set; }

        /// <summary>Special script 2</summary>
        public ResourceReference ScriptSpecial2 { get; set; }

        /// <summary>Combat script</summary>
        public ResourceReference ScriptCombat { get; set; }

        /// <summary>Special script 3</summary>
        public ResourceReference ScriptSpecial3 { get; set; }

        /// <summary>Movement script</summary>
        public ResourceReference ScriptMovement { get; set; }

        //IWD variables
        /// <summary>Indicates whether or not the creature is visible</summary>
        public Boolean Visible { get; set; }

        /// <summary>Sets the _DEAD variable on death</summary>
        /// <remarks>...BEEF</remarks>
        public Boolean SetVariableDEAD { get; set; }

        /// <summary>"Sets" (increments?) the KILL_&lt;scriptname&gt;_CNT variable on death</summary>
        public Boolean SetVariableKILL_CNT { get; set; }

        /// <summary>Unknown fourth byte that follows the preceeding three boolean flags above</summary>
        public Byte Unknown6 { get; set; }

        /// <summary>First internal variable array</summary>
        public Int16 InternalVariable1 { get; set; }

        /// <summary>Second internal variable array</summary>
        public Int16 InternalVariable2 { get; set; }

        /// <summary>Third internal variable array</summary>
        public Int16 InternalVariable3 { get; set; }

        /// <summary>Fourth internal variable array</summary>
        public Int16 InternalVariable4 { get; set; }

        /// <summary>Fifth internal variable array</summary>
        public Int16 InternalVariable5 { get; set; }

        /// <summary>Secondary death variable</summary>
        /// <remarks>set to 1 on death</remarks>
        public ZString SecondaryDeathVariable { get; set; }

        /// <summary>Tertiary death variable</summary>
        /// <remarks>incremented by 1 on death</remarks>
        public ZString TertiaryDeathVariable { get; set; }

        /// <summary>Unknown two bytes that follows the preceeding extra death variables</summary>
        public UInt16 Unknown7 { get; set; }

        /// <summary>Save X coordinate</summary>
        public UInt16 SavedCoordinateX { get; set; }

        /// <summary>Save Y coordinate</summary>
        public UInt16 SavedCoordinateY { get; set; }

        /// <summary>Save orientation</summary>
        /// <value>0-15?</value>
        public UInt16 SavedOrientation { get; set; }

        /// <summary>Unknown 15 bytes that follows the preceeding coordinate data</summary>
        public Byte[] Unknown8 { get; set; }

        /// <summary>Minimum transparency changed by fade_in/fade_out state</summary>
        public Byte FadeLevel { get; set; }

        /// <summary>used by the fade_in/fade_out state</summary>
        public Byte FadeSpeed { get; set; }

        /// <summary>Additional special flags for 3E/IWD2</summary>
        public SpecialFlags3E SpecialFlags { get; set; }

        /// <summary>Description is uncertain</summary>
        public Byte Visible_0x304 { get; set; }

        /// <summary>Still-unknown value</summary>
        public Byte Unknown_0x305 { get; set; }

        /// <summary>Still-unknown value</summary>
        public Byte Unknown_0x306 { get; set; }

        /// <summary>Skill point remaining for the character after a level-up or creation</summary>
        public Byte RemainingSkillPoints { get; set; }

        /// <summary>Remaining unknown 124 bytes</summary>
        public Byte[] Unknown_0x308 { get; set; }

        /// <summary>Duplicte of class, used for object matching</summary>
        public UInt16 AvClass { get; set; }

        /// <summary>Duplicte of class, used for object matching</summary>
        public UInt16 ClassMsk { get; set; }

        /// <summary>Two bytes unknown after class duplicates</summary>
        public UInt16 Unknown9 { get; set; }

        /// <summary>Collection of spells' offsets and counts</summary>
        public GenericOrderedDictionary<String, D20KnownSpellOffsetData> SpellOffsets { get; set; }
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

            this.Unknown2 = new Byte[33];
            this.Unknown3 = new Byte[22];
            this.Unknown4 = new Byte[38];
            this.Unknown8 = new Byte[15];
            this.Unknown_0x308 = new Byte[124];
            this.ScriptTeam = new ResourceReference();
            this.ScriptSpecial1 = new ResourceReference();
            this.ScriptSpecial2 = new ResourceReference();
            this.ScriptCombat = new ResourceReference();
            this.ScriptSpecial3 = new ResourceReference();
            this.ScriptMovement = new ResourceReference();
            this.SkillReserved = new Byte[50];
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
            this.SpellOffsets = new GenericOrderedDictionary<String, D20KnownSpellOffsetData>();

            this.SpellOffsets.Add("Bard1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Bard9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Cleric1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Cleric9", new D20KnownSpellOffsetData());
            
            this.SpellOffsets.Add("Druid1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Druid9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Paladin1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Paladin9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Ranger1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Ranger9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Sorcerer1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Sorcerer9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Wizard1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Wizard9", new D20KnownSpellOffsetData());

            this.SpellOffsets.Add("Domain1", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain2", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain3", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain4", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain5", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain6", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain7", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain8", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Domain9", new D20KnownSpellOffsetData());
            
            this.SpellOffsets.Add("Abilities", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Songs", new D20KnownSpellOffsetData());
            this.SpellOffsets.Add("Shapes", new D20KnownSpellOffsetData());
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
            this.Flags = (CreatureD20Flags)ReusableIO.ReadUInt32FromArray(remainingBody, 8);
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
            this.SavingThrowFortitude = remainingBody[74];
            this.SavingThrowReflex = remainingBody[75];
            this.SavingThrowWill = remainingBody[76];
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
            this.ResistMagicDamage = remainingBody[88];
            this.Unknown1 = ReusableIO.ReadUInt32FromArray(remainingBody, 89);
            this.fatigue = remainingBody[93];
            this.intoxication = remainingBody[94];
            this.luck = remainingBody[95];
            this.TurnUndeadLevel = remainingBody[96];
            Array.Copy(remainingBody, 97, this.Unknown2, 0, 33);
            this.LevelsTotal = remainingBody[130];
            this.LevelsOfBarbarian = remainingBody[131];
            this.LevelsOfBard = remainingBody[132];
            this.LevelsOfCleric = remainingBody[133];
            this.LevelsOfDruid = remainingBody[134];
            this.LevelsOfFighter = remainingBody[135];
            this.LevelsOfMonk = remainingBody[136];
            this.LevelsOfPaladin = remainingBody[137];
            this.LevelsOfRanger = remainingBody[138];
            this.LevelsOfRogue = remainingBody[139];
            this.LevelsOfSorcerer = remainingBody[140];
            this.LevelsOfWizard = remainingBody[141];
            Array.Copy(remainingBody, 142, this.Unknown3, 0, 22);
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
            this.ScriptTeam.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 420, CultureConstants.CultureCodeEnglish);
            this.ScriptSpecial1.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 428, CultureConstants.CultureCodeEnglish);
            this.CreatureEnchantmentLevel = ReusableIO.ReadInt32FromArray(remainingBody, 436);
            this.FeatsFlag1 = (D20Feats1)ReusableIO.ReadUInt64FromArray(remainingBody, 440);
            this.FeatsFlag2 = (D20Feats2)ReusableIO.ReadUInt64FromArray(remainingBody, 448);
            this.FeatsFlagReserved = ReusableIO.ReadUInt64FromArray(remainingBody, 456);
            this.MartialProficiencyBow = remainingBody[464];
            this.SimpleProficiencyCrossbow = remainingBody[465];
            this.SimpleProficiencyMissile = remainingBody[466];
            this.MartialProficiencyAxe = remainingBody[467];
            this.SimpleProficiencyMace = remainingBody[468];
            this.MartialProficiencyFlail = remainingBody[469];
            this.MartialProficiencyPolearm = remainingBody[470];
            this.MartialProficiencyHammer = remainingBody[471];
            this.SimpleProficiencyQuarterstaff = remainingBody[472];
            this.MartialProficiencyGreatSword = remainingBody[473];
            this.MartialProficiencyLargeSword = remainingBody[474];
            this.SimpleProficiencySmallBlade = remainingBody[475];
            this.FeatToughness = remainingBody[476];
            this.FeatArmoredArcana = remainingBody[477];
            this.FeatCleave = remainingBody[478];
            this.FeatArmorProficiency = remainingBody[479];
            this.FeatSpellFocusEnchantment = remainingBody[480];
            this.FeatSpellFocusEvocation = remainingBody[481];
            this.FeatSpellFocusNecromancy = remainingBody[482];
            this.FeatSpellFocusTransmutation = remainingBody[483];
            this.FeatSpellPenetration = remainingBody[484];
            this.FeatExtraRage = remainingBody[485];
            this.FeatExtraWildShape = remainingBody[486];
            this.FeatExtraSmiting = remainingBody[487];
            this.FeatExtraTurning = remainingBody[488];
            this.ExoticProficiencyBastardSword = remainingBody[489];
            Array.Copy(remainingBody, 490, this.Unknown4, 0, 38);
            this.SkillAlchemy = remainingBody[528];
            this.SkillAnimalEmpathy = remainingBody[529];
            this.SkillBluff = remainingBody[530];
            this.SkillConcentration = remainingBody[531];
            this.SkillDiplomacy = remainingBody[532];
            this.SkillDisableDevice = remainingBody[533];
            this.SkillHide = remainingBody[534];
            this.SkillIntimidate = remainingBody[535];
            this.SkillKnowledgeArcana = remainingBody[536];
            this.SkillMoveSilently = remainingBody[537];
            this.SkillOpenLock = remainingBody[538];
            this.SkillPickPocket = remainingBody[539];
            this.SkillSearch = remainingBody[540];
            this.SkillSpellcraft = remainingBody[541];
            this.SkillUseMagicDevice = remainingBody[542];
            this.SkillWildernessLore = remainingBody[543];
            Array.Copy(remainingBody, 544, this.SkillReserved, 0, 50);
        }

        /// <summary>Reads the Creature D20 elements after the proficiency entries</summary>
        /// <param name="headerBodyArray">Byte array to read from. Expected to be reading from index 594, until byte 660.</param>
        protected void ReadBodyAfterProficiencies(Byte[] remainingBody)
        {
            this.ChallengeRating = remainingBody[594];
            this.FavoredEnemy1 = remainingBody[595];
            this.FavoredEnemy2 = remainingBody[596];
            this.FavoredEnemy3 = remainingBody[597];
            this.FavoredEnemy4 = remainingBody[598];
            this.FavoredEnemy5 = remainingBody[599];
            this.FavoredEnemy6 = remainingBody[600];
            this.FavoredEnemy7 = remainingBody[601];
            this.FavoredEnemy8 = remainingBody[602];
            this.Subrace = remainingBody[603];
            this.Unknown5 = ReusableIO.ReadUInt16FromArray(remainingBody, 604);
            this.scoreStrength = remainingBody[606];
            this.scoreIntelligence = remainingBody[607];
            this.scoreWisdom = remainingBody[608];
            this.scoreDexterity = remainingBody[609];
            this.scoreConstitution = remainingBody[610];
            this.scoreCharisma = remainingBody[611];
            this.morale = remainingBody[612];
            this.moraleBreak = remainingBody[613];
            this.moraleRecoveryTime = ReusableIO.ReadUInt16FromArray(remainingBody, 614);
            this.Archetype = (KitD20)ReusableIO.ReadUInt32FromArray(remainingBody, 616);
            this.scriptOverride.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 620, CultureConstants.CultureCodeEnglish);
            this.ScriptSpecial2.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 628, CultureConstants.CultureCodeEnglish);
            this.ScriptCombat.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 636, CultureConstants.CultureCodeEnglish);
            this.ScriptSpecial3.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 644, CultureConstants.CultureCodeEnglish);
            this.ScriptMovement.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 652, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Reads Icewind Dale structure additions</summary>
        /// <param name="headerBody">Byte array to read from. Expects to start reading at 660, reading 104 Bytes</param>
        protected void ReadBodyIcewindDaleAdditions(Byte[] headerBody)
        {
            //is this all one Int32, maybe bit-shifted to get the boolean values?
            this.Visible = Convert.ToBoolean(headerBody[660]);
            this.SetVariableDEAD = Convert.ToBoolean(headerBody[661]);
            this.SetVariableKILL_CNT = Convert.ToBoolean(headerBody[662]);
            this.Unknown6 = headerBody[663];
            this.InternalVariable1 = ReusableIO.ReadInt16FromArray(headerBody, 664);
            this.InternalVariable2 = ReusableIO.ReadInt16FromArray(headerBody, 666);
            this.InternalVariable3 = ReusableIO.ReadInt16FromArray(headerBody, 668);
            this.InternalVariable4 = ReusableIO.ReadInt16FromArray(headerBody, 670);
            this.InternalVariable5 = ReusableIO.ReadInt16FromArray(headerBody, 672);
            this.SecondaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 674, CultureConstants.CultureCodeEnglish, 32);
            this.TertiaryDeathVariable.Source = ReusableIO.ReadStringFromByteArray(headerBody, 706, CultureConstants.CultureCodeEnglish, 32);
            this.Unknown7 = ReusableIO.ReadUInt16FromArray(headerBody, 738);
            this.SavedCoordinateX = ReusableIO.ReadUInt16FromArray(headerBody, 740);
            this.SavedCoordinateY = ReusableIO.ReadUInt16FromArray(headerBody, 742);
            this.SavedOrientation = ReusableIO.ReadUInt16FromArray(headerBody, 744);
            Array.Copy(headerBody, 746, this.Unknown8, 0, 15);
            this.FadeLevel = headerBody[761];
            this.FadeSpeed = headerBody[762];
            this.SpecialFlags = (SpecialFlags3E)headerBody[763];
            this.Visible_0x304 = headerBody[764];
            this.Unknown_0x305 = headerBody[765];
            this.Unknown_0x306 = headerBody[766];
            this.RemainingSkillPoints = headerBody[767];
            Array.Copy(headerBody, 768, this.Unknown_0x308, 0, 124);
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
            this.AvClass = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 940);
            this.ClassMsk = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 942);
            this.Unknown9 = ReusableIO.ReadUInt16FromArray(remainingHeaderArray, 944);
        }

        /// <summary>Reads the header's spell offsets</summary>
        /// <param name="header">Byte array to read from, starting at position 946 for X bytes</param>
        protected void ReadSpellOffsetsAndCounts(Byte[] header)
        {
            //Class offsets
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                this.SpellOffsets[index].Offset = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 946);

            //Class counts
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                this.SpellOffsets[index].Count = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1198);

            //Domain offsets
            for (Int32 index = 0; index < 9 /*9*7*/; ++index)
                this.SpellOffsets[index + 63].Offset = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1450);

            //Domain counts
            for (Int32 index = 0; index < 9 /*9*7*/; ++index)
                this.SpellOffsets[index + 63].Count = ReusableIO.ReadUInt32FromArray(header, (index * 4 /* sizeof(UInt32) */) + 1486);

            //Abilities
            this.SpellOffsets[72].Offset = ReusableIO.ReadUInt32FromArray(header, 1522);
            this.SpellOffsets[72].Count = ReusableIO.ReadUInt32FromArray(header, 1526);
            
            //Songs
            this.SpellOffsets[73].Offset = ReusableIO.ReadUInt32FromArray(header, 1530);
            this.SpellOffsets[73].Count = ReusableIO.ReadUInt32FromArray(header, 1534);

            //Shapes
            this.SpellOffsets[74].Offset = ReusableIO.ReadUInt32FromArray(header, 1538);
            this.SpellOffsets[74].Count = ReusableIO.ReadUInt32FromArray(header, 1542);
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
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
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
            output.WriteByte(this.SavingThrowFortitude);
            output.WriteByte(this.SavingThrowReflex);
            output.WriteByte(this.SavingThrowWill);
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
            output.WriteByte(this.ResistMagicDamage);
            ReusableIO.WriteUInt32ToStream(this.Unknown1, output);
            output.WriteByte(this.fatigue);
            output.WriteByte(this.intoxication);
            output.WriteByte(this.luck);
            output.WriteByte(this.TurnUndeadLevel);
            output.Write(this.Unknown2, 0, 33);
            output.WriteByte(this.LevelsTotal);
            output.WriteByte(this.LevelsOfBarbarian);
            output.WriteByte(this.LevelsOfBard);
            output.WriteByte(this.LevelsOfCleric);
            output.WriteByte(this.LevelsOfDruid);
            output.WriteByte(this.LevelsOfFighter);
            output.WriteByte(this.LevelsOfMonk);
            output.WriteByte(this.LevelsOfPaladin);
            output.WriteByte(this.LevelsOfRanger);
            output.WriteByte(this.LevelsOfRogue);
            output.WriteByte(this.LevelsOfSorcerer);
            output.WriteByte(this.LevelsOfWizard);
            output.Write(this.Unknown3, 0, 22);
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
            ReusableIO.WriteStringToStream(this.ScriptTeam.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptSpecial1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.CreatureEnchantmentLevel, output);
            ReusableIO.WriteUInt64ToStream((UInt64)this.FeatsFlag1, output);
            ReusableIO.WriteUInt64ToStream((UInt64)this.FeatsFlag2, output);
            ReusableIO.WriteUInt64ToStream(this.FeatsFlagReserved, output);
            output.WriteByte(this.MartialProficiencyBow);
            output.WriteByte(this.SimpleProficiencyCrossbow);
            output.WriteByte(this.SimpleProficiencyMissile);
            output.WriteByte(this.MartialProficiencyAxe);
            output.WriteByte(this.SimpleProficiencyMace);
            output.WriteByte(this.MartialProficiencyFlail);
            output.WriteByte(this.MartialProficiencyPolearm);
            output.WriteByte(this.MartialProficiencyHammer);
            output.WriteByte(this.SimpleProficiencyQuarterstaff);
            output.WriteByte(this.MartialProficiencyGreatSword);
            output.WriteByte(this.MartialProficiencyLargeSword);
            output.WriteByte(this.SimpleProficiencySmallBlade);
            output.WriteByte(this.FeatToughness);
            output.WriteByte(this.FeatArmoredArcana);
            output.WriteByte(this.FeatCleave);
            output.WriteByte(this.FeatArmorProficiency);
            output.WriteByte(this.FeatSpellFocusEnchantment);
            output.WriteByte(this.FeatSpellFocusEvocation);
            output.WriteByte(this.FeatSpellFocusNecromancy);
            output.WriteByte(this.FeatSpellFocusTransmutation);
            output.WriteByte(this.FeatSpellPenetration);
            output.WriteByte(this.FeatExtraRage);
            output.WriteByte(this.FeatExtraWildShape);
            output.WriteByte(this.FeatExtraSmiting);
            output.WriteByte(this.FeatExtraTurning);
            output.WriteByte(this.ExoticProficiencyBastardSword);
            output.Write(this.Unknown4, 0, 38);
            output.WriteByte(this.SkillAlchemy);
            output.WriteByte(this.SkillAnimalEmpathy);
            output.WriteByte(this.SkillBluff);
            output.WriteByte(this.SkillConcentration);
            output.WriteByte(this.SkillDiplomacy);
            output.WriteByte(this.SkillDisableDevice);
            output.WriteByte(this.SkillHide);
            output.WriteByte(this.SkillIntimidate);
            output.WriteByte(this.SkillKnowledgeArcana);
            output.WriteByte(this.SkillMoveSilently);
            output.WriteByte(this.SkillOpenLock);
            output.WriteByte(this.SkillPickPocket);
            output.WriteByte(this.SkillSearch);
            output.WriteByte(this.SkillSpellcraft);
            output.WriteByte(this.SkillUseMagicDevice);
            output.WriteByte(this.SkillWildernessLore);
            output.Write(this.SkillReserved, 0, 50);
        }

        /// <summary>Writes the Creature D20 elements after the proficiency entries</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteAfterProficiencies(Stream output)
        {
            output.WriteByte(this.ChallengeRating);
            output.WriteByte(this.FavoredEnemy1);
            output.WriteByte(this.FavoredEnemy2);
            output.WriteByte(this.FavoredEnemy3);
            output.WriteByte(this.FavoredEnemy4);
            output.WriteByte(this.FavoredEnemy5);
            output.WriteByte(this.FavoredEnemy6);
            output.WriteByte(this.FavoredEnemy7);
            output.WriteByte(this.FavoredEnemy8);
            output.WriteByte(this.Subrace);
            ReusableIO.WriteUInt16ToStream(this.Unknown5, output);
            output.WriteByte(this.scoreStrength);
            output.WriteByte(this.scoreIntelligence);
            output.WriteByte(this.scoreWisdom);
            output.WriteByte(this.scoreDexterity);
            output.WriteByte(this.scoreConstitution);
            output.WriteByte(this.scoreCharisma);
            output.WriteByte(this.morale);
            output.WriteByte(this.moraleBreak);
            ReusableIO.WriteUInt16ToStream(this.moraleRecoveryTime, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Archetype, output);
            ReusableIO.WriteStringToStream(this.scriptOverride.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptSpecial2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptCombat.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptSpecial3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptMovement.ResRef, output, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Writes the Icewind Dale structure additions to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteIcewindDaleAdditions(Stream output)
        {
            output.WriteByte(Convert.ToByte(this.Visible));
            output.WriteByte(Convert.ToByte(this.SetVariableDEAD));
            output.WriteByte(Convert.ToByte(this.SetVariableKILL_CNT));
            output.WriteByte(this.Unknown6);
            ReusableIO.WriteInt16ToStream(this.InternalVariable1, output);
            ReusableIO.WriteInt16ToStream(this.InternalVariable2, output);
            ReusableIO.WriteInt16ToStream(this.InternalVariable3, output);
            ReusableIO.WriteInt16ToStream(this.InternalVariable4, output);
            ReusableIO.WriteInt16ToStream(this.InternalVariable5, output);
            ReusableIO.WriteStringToStream(SecondaryDeathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteStringToStream(TertiaryDeathVariable.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Unknown7, output);
            ReusableIO.WriteUInt16ToStream(this.SavedCoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.SavedCoordinateY, output);
            ReusableIO.WriteUInt16ToStream(this.SavedOrientation, output);
            output.Write(this.Unknown8, 0, 15);
            output.WriteByte(this.FadeLevel);
            output.WriteByte(this.FadeSpeed);
            output.WriteByte((Byte)this.SpecialFlags);
            output.WriteByte(this.Visible_0x304);
            output.WriteByte(this.Unknown_0x305);
            output.WriteByte(this.Unknown_0x306);
            output.WriteByte(this.RemainingSkillPoints);
            output.Write(this.Unknown_0x308, 0, 124);
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
            ReusableIO.WriteUInt16ToStream(this.AvClass, output);
            ReusableIO.WriteUInt16ToStream(this.ClassMsk, output);
            ReusableIO.WriteUInt16ToStream(this.Unknown9, output);
        }

        /// <summary>Writes the header's spell offsets</summary>
        /// <param name="output">Output stream to write to</param>
        protected void WriteSpellOffsetsAndCounts(Stream output)
        {
            //Class offsets
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                ReusableIO.WriteUInt32ToStream(this.SpellOffsets[index].Offset, output);

            //Class counts
            for (Int32 index = 0; index < 63 /*9*7*/; ++index)
                ReusableIO.WriteUInt32ToStream(this.SpellOffsets[index].Count, output);

            //Domain offsets
            for (Int32 index = 63; index < 72 /* 9 more */; ++index)
                ReusableIO.WriteUInt32ToStream(this.SpellOffsets[index].Offset, output);

            //Domain counts
            for (Int32 index = 63; index < 72 /* 9 more */; ++index)
                ReusableIO.WriteUInt32ToStream(this.SpellOffsets[index].Count, output);

            //Abilities
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[72].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[72].Count, output);

            //Songs
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[73].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[73].Count, output);

            //Shapes
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[74].Offset, output);
            ReusableIO.WriteUInt32ToStream(this.SpellOffsets[74].Count, output);
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
            builder.Append((UInt32)this.Flags);
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
            builder.Append(this.SavingThrowFortitude);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Reflex)"));
            builder.Append(this.SavingThrowReflex);
            builder.Append(StringFormat.ToStringAlignment("Saving Throw Modifier (Will)"));
            builder.Append(this.SavingThrowWill);
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
            builder.Append(this.ResistMagicDamage);
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.Unknown1);
            builder.Append(StringFormat.ToStringAlignment("Fatigue"));
            builder.Append(this.fatigue);
            builder.Append(StringFormat.ToStringAlignment("Intoxication"));
            builder.Append(this.intoxication);
            builder.Append(StringFormat.ToStringAlignment("Luck"));
            builder.Append(this.luck);
            builder.Append(StringFormat.ToStringAlignment("Turn Undead Level"));
            builder.Append(this.TurnUndeadLevel);
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown2));
            builder.Append(StringFormat.ToStringAlignment("Total Levels"));
            builder.Append(this.LevelsTotal);
            builder.Append(StringFormat.ToStringAlignment("Barbarian Levels"));
            builder.Append(this.LevelsOfBarbarian);
            builder.Append(StringFormat.ToStringAlignment("Bard Levels"));
            builder.Append(this.LevelsOfBard);
            builder.Append(StringFormat.ToStringAlignment("Cleric Levels"));
            builder.Append(this.LevelsOfCleric);
            builder.Append(StringFormat.ToStringAlignment("Druid Levels"));
            builder.Append(this.LevelsOfDruid);
            builder.Append(StringFormat.ToStringAlignment("Fighter Levels"));
            builder.Append(this.LevelsOfFighter);
            builder.Append(StringFormat.ToStringAlignment("Monk Levels"));
            builder.Append(this.LevelsOfMonk);
            builder.Append(StringFormat.ToStringAlignment("Paladin Levels"));
            builder.Append(this.LevelsOfPaladin);
            builder.Append(StringFormat.ToStringAlignment("Ranger Levels"));
            builder.Append(this.LevelsOfRanger);
            builder.Append(StringFormat.ToStringAlignment("Rogue Levels"));
            builder.Append(this.LevelsOfRogue);
            builder.Append(StringFormat.ToStringAlignment("Sorcerer Levels"));
            builder.Append(this.LevelsOfSorcerer);
            builder.Append(StringFormat.ToStringAlignment("Wizard Levels"));
            builder.Append(this.LevelsOfWizard);
            builder.Append(StringFormat.ToStringAlignment("Unknown #3"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown3));
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
            builder.Append(String.Format("'{0}'", this.ScriptTeam.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Special script #1"));
            builder.Append(String.Format("'{0}'", this.ScriptSpecial1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Creature enchantment level (weapon to-hit)"));
            builder.Append(this.CreatureEnchantmentLevel);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #1"));
            builder.Append((UInt64)this.FeatsFlag1);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #1 (enumerated)"));
            builder.Append(this.GetFeatFlags1String());
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #2"));
            builder.Append((UInt64)this.FeatsFlag2);
            builder.Append(StringFormat.ToStringAlignment("Feat Flags #2 (enumerated)"));
            builder.Append(this.GetFeatFlags2String());
            builder.Append(StringFormat.ToStringAlignment("Reserved Creature Flags"));
            builder.Append(this.FeatsFlagReserved);

            //Weapon proficiencies
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Bow)"));
            builder.Append(this.MartialProficiencyBow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Crossbow)"));
            builder.Append(this.SimpleProficiencyCrossbow);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Missile)"));
            builder.Append(this.SimpleProficiencyMissile);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Axe)"));
            builder.Append(this.MartialProficiencyAxe);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Mace)"));
            builder.Append(this.SimpleProficiencyMace);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Flail)"));
            builder.Append(this.MartialProficiencyFlail);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Polearm)"));
            builder.Append(this.MartialProficiencyPolearm);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Hammer)"));
            builder.Append(this.MartialProficiencyHammer);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Quarterstaff)"));
            builder.Append(this.SimpleProficiencyQuarterstaff);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Great Sword)"));
            builder.Append(this.MartialProficiencyGreatSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Martial, Large Sword)"));
            builder.Append(this.MartialProficiencyLargeSword);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Simple, Small Blade)"));
            builder.Append(this.SimpleProficiencySmallBlade);
            builder.Append(StringFormat.ToStringAlignment("Feat (Toughness)"));
            builder.Append(this.FeatToughness);
            builder.Append(StringFormat.ToStringAlignment("Feat (Armored Arcana)"));
            builder.Append(this.FeatArmoredArcana);
            builder.Append(StringFormat.ToStringAlignment("Feat (Cleave)"));
            builder.Append(this.FeatCleave);
            builder.Append(StringFormat.ToStringAlignment("Feat (Armor Proficiency)"));
            builder.Append(this.FeatArmorProficiency);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Enchantment)"));
            builder.Append(this.FeatSpellFocusEnchantment);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Evocation)"));
            builder.Append(this.FeatSpellFocusEvocation);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Necromancy)"));
            builder.Append(this.FeatSpellFocusNecromancy);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Focus: Transmutation)"));
            builder.Append(this.FeatSpellFocusTransmutation);
            builder.Append(StringFormat.ToStringAlignment("Feat (Spell Penetration)"));
            builder.Append(this.FeatSpellPenetration);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Rage)"));
            builder.Append(this.FeatExtraRage);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Wild Shape)"));
            builder.Append(this.FeatExtraWildShape);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Smiting)"));
            builder.Append(this.FeatExtraSmiting);
            builder.Append(StringFormat.ToStringAlignment("Feat (Extra Turning)"));
            builder.Append(this.FeatExtraTurning);
            builder.Append(StringFormat.ToStringAlignment("Proficiency (Exotic, Bastard Sword)"));
            builder.Append(this.ExoticProficiencyBastardSword);
            builder.Append(StringFormat.ToStringAlignment("Remaining reserved Feat slots:"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown4));

            //Skills
            builder.Append(StringFormat.ToStringAlignment("Skill (Alchemy)"));
            builder.Append(this.SkillAlchemy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Animal Empathy)"));
            builder.Append(this.SkillAnimalEmpathy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Bluff)"));
            builder.Append(this.SkillBluff);
            builder.Append(StringFormat.ToStringAlignment("Skill (Concentration)"));
            builder.Append(this.SkillConcentration);
            builder.Append(StringFormat.ToStringAlignment("Skill (Diplomacy)"));
            builder.Append(this.SkillDiplomacy);
            builder.Append(StringFormat.ToStringAlignment("Skill (Disable Device)"));
            builder.Append(this.SkillDisableDevice);
            builder.Append(StringFormat.ToStringAlignment("Skill (Hide)"));
            builder.Append(this.SkillHide);
            builder.Append(StringFormat.ToStringAlignment("Skill (Intimidate)"));
            builder.Append(this.SkillIntimidate);
            builder.Append(StringFormat.ToStringAlignment("Skill (Knowledge, Arcana)"));
            builder.Append(this.SkillKnowledgeArcana);
            builder.Append(StringFormat.ToStringAlignment("Skill (Move Silently)"));
            builder.Append(this.SkillMoveSilently);
            builder.Append(StringFormat.ToStringAlignment("Skill (Open Lock)"));
            builder.Append(this.SkillOpenLock);
            builder.Append(StringFormat.ToStringAlignment("Skill (Pick Pocket)"));
            builder.Append(this.SkillPickPocket);
            builder.Append(StringFormat.ToStringAlignment("Skill (Search)"));
            builder.Append(this.SkillSearch);
            builder.Append(StringFormat.ToStringAlignment("Skill (Spellcraft)"));
            builder.Append(this.SkillSpellcraft);
            builder.Append(StringFormat.ToStringAlignment("Skill (Use Magic Device)"));
            builder.Append(this.SkillUseMagicDevice);
            builder.Append(StringFormat.ToStringAlignment("Skill (Wilderness Lore)"));
            builder.Append(this.SkillWildernessLore);
            builder.Append(StringFormat.ToStringAlignment("Remaining reserved Skill slots:"));
            builder.Append(StringFormat.ByteArrayToHexString(this.SkillReserved));
        }
        
        /// <summary>Generates a String representing the ability scores and simila after the proficiencis area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringAfterProcifiencies(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Challenge Rating"));
            builder.Append(this.ChallengeRating);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #1"));
            builder.Append(this.FavoredEnemy1);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #2"));
            builder.Append(this.FavoredEnemy2);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #3"));
            builder.Append(this.FavoredEnemy3);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #4"));
            builder.Append(this.FavoredEnemy4);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #5"));
            builder.Append(this.FavoredEnemy5);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #6"));
            builder.Append(this.FavoredEnemy6);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #7"));
            builder.Append(this.FavoredEnemy7);
            builder.Append(StringFormat.ToStringAlignment("Favored Enemy #8"));
            builder.Append(this.FavoredEnemy8);
            builder.Append(StringFormat.ToStringAlignment("Subrace"));
            builder.Append(this.Subrace);
            builder.Append(StringFormat.ToStringAlignment("Unknown #5"));
            builder.Append(this.Unknown5);
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
            builder.Append((UInt32)this.Archetype);
            builder.Append(StringFormat.ToStringAlignment("Archetypes (enumerated)"));
            builder.Append(this.GetArchetypeString());
            builder.Append(StringFormat.ToStringAlignment("Script (Override)"));
            builder.Append(String.Format("'{0}'", this.scriptOverride.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Special #2)"));
            builder.Append(String.Format("'{0}'", this.ScriptSpecial2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Combat)"));
            builder.Append(String.Format("'{0}'", this.ScriptCombat.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Special #3)"));
            builder.Append(String.Format("'{0}'", this.ScriptSpecial3.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Movement)"));
            builder.Append(String.Format("'{0}'", this.ScriptMovement.ZResRef));
        }

        /// <summary>Generates a String representing the added Icewind Dale values area of the creature data structure</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringIcewindDaleAdditions(StringBuilder builder)
        {
            builder.Append(StringFormat.ToStringAlignment("Visible"));
            builder.Append(this.Visible);
            builder.Append(StringFormat.ToStringAlignment("Set _DEAD variable on death"));
            builder.Append(this.SetVariableDEAD);
            builder.Append(StringFormat.ToStringAlignment("Set KILL_<scriptname>_CNT variable on death"));
            builder.Append(this.SetVariableKILL_CNT);
            builder.Append(StringFormat.ToStringAlignment("Unknown #6"));
            builder.Append(this.Unknown6);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #1"));
            builder.Append(this.InternalVariable1);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #2"));
            builder.Append(this.InternalVariable2);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #3"));
            builder.Append(this.InternalVariable3);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #4"));
            builder.Append(this.InternalVariable4);
            builder.Append(StringFormat.ToStringAlignment("Internal variable #5"));
            builder.Append(this.InternalVariable5);
            builder.Append(StringFormat.ToStringAlignment("Secondary Death variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.SecondaryDeathVariable.Value)));
            builder.Append(StringFormat.ToStringAlignment("Tertiary Death variable"));
            builder.Append(StringFormat.ToStringAlignment(String.Format("'{0}'", this.TertiaryDeathVariable.Value)));
            builder.Append(StringFormat.ToStringAlignment("Unknown #7"));
            builder.Append(this.Unknown7);
            builder.Append(StringFormat.ToStringAlignment("Saved X Co-ordinate"));
            builder.Append(this.SavedCoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Saved Y Co-ordinate"));
            builder.Append(this.SavedCoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Saved Orientation"));
            builder.Append(this.SavedOrientation);
            builder.Append(StringFormat.ToStringAlignment("Unknown #8 (Byte array)"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown8));
            builder.Append(StringFormat.ToStringAlignment("Fade Level"));
            builder.Append(this.FadeLevel);
            builder.Append(StringFormat.ToStringAlignment("Fade Speed"));
            builder.Append(this.FadeSpeed);
            builder.Append(StringFormat.ToStringAlignment("Special Flags (value)"));
            builder.Append((Byte)this.SpecialFlags);
            builder.Append(StringFormat.ToStringAlignment("Special Flags (enumerated)"));
            builder.Append(this.GetSpecialFlags3EString());
            builder.Append(StringFormat.ToStringAlignment("Visible (redundant?)"));
            builder.Append(this.Visible_0x304);
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x305"));
            builder.Append(this.Unknown_0x305);
            builder.Append(StringFormat.ToStringAlignment("Unknown at offset 0x306"));
            builder.Append(this.Unknown_0x306);
            builder.Append(StringFormat.ToStringAlignment("Remaining Skill Points"));
            builder.Append(this.RemainingSkillPoints);
            builder.Append(StringFormat.ToStringAlignment("Unknown bytes at offset 0x308"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown_0x308));
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
            builder.Append(this.AvClass);
            builder.Append(StringFormat.ToStringAlignment("Class Mask"));
            builder.Append(this.ClassMsk);
            builder.Append(StringFormat.ToStringAlignment("Unknown #9"));
            builder.Append(this.Unknown9);
        }

        /// <summary>Generates a String representing the header's spell offsets</summary>
        /// <param name="builder">StringBuilder to append to</param>
        protected void ToStringOffsetsAndCounts(StringBuilder builder)
        {
            //out of order, but hey
            foreach (String key in this.SpellOffsets.Keys)
            {
                builder.Append(StringFormat.ToStringAlignment(key + " offset"));
                builder.Append(this.SpellOffsets[key].Offset);
                builder.Append(StringFormat.ToStringAlignment(key + " count"));
                builder.Append(this.SpellOffsets[key].Count);
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

            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.KeepCastingWhenDamaged) == CreatureD20Flags.KeepCastingWhenDamaged, CreatureD20Flags.KeepCastingWhenDamaged.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.NoCorpse) == CreatureD20Flags.NoCorpse, CreatureD20Flags.NoCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.PermanentCorpse) == CreatureD20Flags.PermanentCorpse, CreatureD20Flags.PermanentCorpse.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassFighter) == CreatureD20Flags.OriginalClassFighter, CreatureD20Flags.OriginalClassFighter.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassMage) == CreatureD20Flags.OriginalClassMage, CreatureD20Flags.OriginalClassMage.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassCleric) == CreatureD20Flags.OriginalClassCleric, CreatureD20Flags.OriginalClassCleric.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassThief) == CreatureD20Flags.OriginalClassThief, CreatureD20Flags.OriginalClassThief.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassDruid) == CreatureD20Flags.OriginalClassDruid, CreatureD20Flags.OriginalClassDruid.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.OriginalClassRanger) == CreatureD20Flags.OriginalClassRanger, CreatureD20Flags.OriginalClassRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.FallenPaladin) == CreatureD20Flags.FallenPaladin, CreatureD20Flags.FallenPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.FallenRanger) == CreatureD20Flags.FallenRanger, CreatureD20Flags.FallenRanger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.Exportable) == CreatureD20Flags.Exportable, CreatureD20Flags.Exportable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.HideInjuryStatusInTooltip) == CreatureD20Flags.HideInjuryStatusInTooltip, CreatureD20Flags.HideInjuryStatusInTooltip.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.QuestCritical) == CreatureD20Flags.QuestCritical, CreatureD20Flags.QuestCritical.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.ActivatesCannotBeUsedByNpcs) == CreatureD20Flags.ActivatesCannotBeUsedByNpcs, CreatureD20Flags.ActivatesCannotBeUsedByNpcs.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.BeenInParty) == CreatureD20Flags.BeenInParty, CreatureD20Flags.BeenInParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.SeenParty) == CreatureD20Flags.SeenParty, CreatureD20Flags.SeenParty.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.Invulnerable) == CreatureD20Flags.Invulnerable, CreatureD20Flags.Invulnerable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.NonThreateningEnemy) == CreatureD20Flags.NonThreateningEnemy, CreatureD20Flags.NonThreateningEnemy.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.NoTalk) == CreatureD20Flags.NoTalk, CreatureD20Flags.NoTalk.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.IgnoreReturnToStart) == CreatureD20Flags.IgnoreReturnToStart, CreatureD20Flags.IgnoreReturnToStart.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.IgnoreInhibitAI) == CreatureD20Flags.IgnoreInhibitAI, CreatureD20Flags.IgnoreInhibitAI.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CreatureD20Flags.UnknownCorpseRelated) == CreatureD20Flags.UnknownCorpseRelated, CreatureD20Flags.UnknownCorpseRelated.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFeatFlags1String()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.AEGIS_OF_RIME) == D20Feats1.AEGIS_OF_RIME, D20Feats1.AEGIS_OF_RIME.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.AMBIDEXTERITY) == D20Feats1.AMBIDEXTERITY, D20Feats1.AMBIDEXTERITY.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.AQUA_MORTIS) == D20Feats1.AQUA_MORTIS, D20Feats1.AQUA_MORTIS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.ARMOR_PROF) == D20Feats1.ARMOR_PROF, D20Feats1.ARMOR_PROF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.ARMORED_ARCANA) == D20Feats1.ARMORED_ARCANA, D20Feats1.ARMORED_ARCANA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.ARTERIAL_STRIKE) == D20Feats1.ARTERIAL_STRIKE, D20Feats1.ARTERIAL_STRIKE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.BLIND_FIGHT) == D20Feats1.BLIND_FIGHT, D20Feats1.BLIND_FIGHT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.BULLHEADED) == D20Feats1.BULLHEADED, D20Feats1.BULLHEADED.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.CLEAVE) == D20Feats1.CLEAVE, D20Feats1.CLEAVE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.COMBAT_CASTING) == D20Feats1.COMBAT_CASTING, D20Feats1.COMBAT_CASTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.CONUNDRUM) == D20Feats1.CONUNDRUM, D20Feats1.CONUNDRUM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.CRIPPLING_STRIKE) == D20Feats1.CRIPPLING_STRIKE, D20Feats1.CRIPPLING_STRIKE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.DASH) == D20Feats1.DASH, D20Feats1.DASH.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.DEFLECT_ARROWS) == D20Feats1.DEFLECT_ARROWS, D20Feats1.DEFLECT_ARROWS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.DIRTY_FIGHTING) == D20Feats1.DIRTY_FIGHTING, D20Feats1.DIRTY_FIGHTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.DISCIPLINE) == D20Feats1.DISCIPLINE, D20Feats1.DISCIPLINE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.DODGE) == D20Feats1.DODGE, D20Feats1.DODGE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.ENVENOM_WEAPON) == D20Feats1.ENVENOM_WEAPON, D20Feats1.ENVENOM_WEAPON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXOTIC_BASTARD) == D20Feats1.EXOTIC_BASTARD, D20Feats1.EXOTIC_BASTARD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXPERTISE) == D20Feats1.EXPERTISE, D20Feats1.EXPERTISE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXTRA_RAGE) == D20Feats1.EXTRA_RAGE, D20Feats1.EXTRA_RAGE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXTRA_SHAPESHIFTING) == D20Feats1.EXTRA_SHAPESHIFTING, D20Feats1.EXTRA_SHAPESHIFTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXTRA_SMITING) == D20Feats1.EXTRA_SMITING, D20Feats1.EXTRA_SMITING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.EXTRA_TURNING) == D20Feats1.EXTRA_TURNING, D20Feats1.EXTRA_TURNING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.FIENDSLAYER) == D20Feats1.FIENDSLAYER, D20Feats1.FIENDSLAYER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.FORESTER) == D20Feats1.FORESTER, D20Feats1.FORESTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.GREAT_FORTITUDE) == D20Feats1.GREAT_FORTITUDE, D20Feats1.GREAT_FORTITUDE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.HAMSTRING) == D20Feats1.HAMSTRING, D20Feats1.HAMSTRING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.HERETICS_BANE) == D20Feats1.HERETICS_BANE, D20Feats1.HERETICS_BANE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.HEROIC_INSPIRATION) == D20Feats1.HEROIC_INSPIRATION, D20Feats1.HEROIC_INSPIRATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.IMPROVED_CRITICAL) == D20Feats1.IMPROVED_CRITICAL, D20Feats1.IMPROVED_CRITICAL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.IMPROVED_EVASION) == D20Feats1.IMPROVED_EVASION, D20Feats1.IMPROVED_EVASION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.IMPROVED_INITIATIVE) == D20Feats1.IMPROVED_INITIATIVE, D20Feats1.IMPROVED_INITIATIVE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.IMPROVED_TURNING) == D20Feats1.IMPROVED_TURNING, D20Feats1.IMPROVED_TURNING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.IRON_WILL) == D20Feats1.IRON_WILL, D20Feats1.IRON_WILL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.LIGHTNING_REFLEXES) == D20Feats1.LIGHTNING_REFLEXES, D20Feats1.LIGHTNING_REFLEXES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.LINGERING_SONG) == D20Feats1.LINGERING_SONG, D20Feats1.LINGERING_SONG.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.LUCK_OF_HEROES) == D20Feats1.LUCK_OF_HEROES, D20Feats1.LUCK_OF_HEROES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_AXE) == D20Feats1.MARTIAL_AXE, D20Feats1.MARTIAL_AXE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_BOW) == D20Feats1.MARTIAL_BOW, D20Feats1.MARTIAL_BOW.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_FLAIL) == D20Feats1.MARTIAL_FLAIL, D20Feats1.MARTIAL_FLAIL.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_GREATSWORD) == D20Feats1.MARTIAL_GREATSWORD, D20Feats1.MARTIAL_GREATSWORD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_HAMMER) == D20Feats1.MARTIAL_HAMMER, D20Feats1.MARTIAL_HAMMER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_LARGESWORD) == D20Feats1.MARTIAL_LARGESWORD, D20Feats1.MARTIAL_LARGESWORD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MARTIAL_POLEARM) == D20Feats1.MARTIAL_POLEARM, D20Feats1.MARTIAL_POLEARM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MAXIMIZED_ATTACKS) == D20Feats1.MAXIMIZED_ATTACKS, D20Feats1.MAXIMIZED_ATTACKS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.MERCANTILE_BACKGROUND) == D20Feats1.MERCANTILE_BACKGROUND, D20Feats1.MERCANTILE_BACKGROUND.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.POWER_ATTACK) == D20Feats1.POWER_ATTACK, D20Feats1.POWER_ATTACK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.PRECISE_SHOT) == D20Feats1.PRECISE_SHOT, D20Feats1.PRECISE_SHOT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.RAPID_SHOT) == D20Feats1.RAPID_SHOT, D20Feats1.RAPID_SHOT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.RESIST_POISON) == D20Feats1.RESIST_POISON, D20Feats1.RESIST_POISON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SCION_OF_STORMS) == D20Feats1.SCION_OF_STORMS, D20Feats1.SCION_OF_STORMS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SHIELD_PROF) == D20Feats1.SHIELD_PROF, D20Feats1.SHIELD_PROF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SIMPLE_CROSSBOW) == D20Feats1.SIMPLE_CROSSBOW, D20Feats1.SIMPLE_CROSSBOW.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SIMPLE_MACE) == D20Feats1.SIMPLE_MACE, D20Feats1.SIMPLE_MACE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SIMPLE_MISSILE) == D20Feats1.SIMPLE_MISSILE, D20Feats1.SIMPLE_MISSILE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SIMPLE_QUARTERSTAFF) == D20Feats1.SIMPLE_QUARTERSTAFF, D20Feats1.SIMPLE_QUARTERSTAFF.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SIMPLE_SMALLBLADE) == D20Feats1.SIMPLE_SMALLBLADE, D20Feats1.SIMPLE_SMALLBLADE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SLIPPERY_MIND) == D20Feats1.SLIPPERY_MIND, D20Feats1.SLIPPERY_MIND.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SNAKE_BLOOD) == D20Feats1.SNAKE_BLOOD, D20Feats1.SNAKE_BLOOD.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SPELL_FOCUS_ENCHANTMENT) == D20Feats1.SPELL_FOCUS_ENCHANTMENT, D20Feats1.SPELL_FOCUS_ENCHANTMENT.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SPELL_FOCUS_EVOCATION) == D20Feats1.SPELL_FOCUS_EVOCATION, D20Feats1.SPELL_FOCUS_EVOCATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SPELL_FOCUS_NECROMANCY) == D20Feats1.SPELL_FOCUS_NECROMANCY, D20Feats1.SPELL_FOCUS_NECROMANCY.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag1 & D20Feats1.SPELL_FOCUS_TRANSMUTE) == D20Feats1.SPELL_FOCUS_TRANSMUTE, D20Feats1.SPELL_FOCUS_TRANSMUTE.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFeatFlags2String()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.SPELL_PENETRATION) == D20Feats2.SPELL_PENETRATION, D20Feats2.SPELL_PENETRATION.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.SPIRIT_OF_FLAME) == D20Feats2.SPIRIT_OF_FLAME, D20Feats2.SPIRIT_OF_FLAME.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.STRONG_BACK) == D20Feats2.STRONG_BACK, D20Feats2.STRONG_BACK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.STUNNING_FIST) == D20Feats2.STUNNING_FIST, D20Feats2.STUNNING_FIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.SUBVOCAL_CASTING) == D20Feats2.SUBVOCAL_CASTING, D20Feats2.SUBVOCAL_CASTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.TOUGHNESS) == D20Feats2.TOUGHNESS, D20Feats2.TOUGHNESS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.TWO_WEAPON_FIGHTING) == D20Feats2.TWO_WEAPON_FIGHTING, D20Feats2.TWO_WEAPON_FIGHTING.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.WEAPON_FINESSE) == D20Feats2.WEAPON_FINESSE, D20Feats2.WEAPON_FINESSE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.WILDSHAPE_BOAR) == D20Feats2.WILDSHAPE_BOAR, D20Feats2.WILDSHAPE_BOAR.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.WILDSHAPE_PANTHER) == D20Feats2.WILDSHAPE_PANTHER, D20Feats2.WILDSHAPE_PANTHER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.FeatsFlag2 & D20Feats2.WILDSHAPE_SHAMBLER) == D20Feats2.WILDSHAPE_SHAMBLER, D20Feats2.WILDSHAPE_SHAMBLER.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which D20 arcetype flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetArchetypeString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.PALADIN_ILMATER) == KitD20.PALADIN_ILMATER, KitD20.PALADIN_ILMATER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.PALADIN_HELM) == KitD20.PALADIN_HELM, KitD20.PALADIN_HELM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.PALADIN_MYSTRA) == KitD20.PALADIN_MYSTRA, KitD20.PALADIN_MYSTRA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MONK_OLD_ORDER) == KitD20.MONK_OLD_ORDER, KitD20.MONK_OLD_ORDER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MONK_BROKEN_ONES) == KitD20.MONK_BROKEN_ONES, KitD20.MONK_BROKEN_ONES.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MONK_DARK_MOON) == KitD20.MONK_DARK_MOON, KitD20.MONK_DARK_MOON.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_ABJURER) == KitD20.MAGE_ABJURER, KitD20.MAGE_ABJURER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_CONJURER) == KitD20.MAGE_CONJURER, KitD20.MAGE_CONJURER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_DIVINER) == KitD20.MAGE_DIVINER, KitD20.MAGE_DIVINER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_ENCHANTER) == KitD20.MAGE_ENCHANTER, KitD20.MAGE_ENCHANTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_EVOKER) == KitD20.MAGE_EVOKER, KitD20.MAGE_EVOKER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_ILLUSIONIST) == KitD20.MAGE_ILLUSIONIST, KitD20.MAGE_ILLUSIONIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_NECROMANCER) == KitD20.MAGE_NECROMANCER, KitD20.MAGE_NECROMANCER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_TRANSMUTER) == KitD20.MAGE_TRANSMUTER, KitD20.MAGE_TRANSMUTER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.MAGE_GENERALIST) == KitD20.MAGE_GENERALIST, KitD20.MAGE_GENERALIST.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_ILMATER) == KitD20.CLERIC_ILMATER, KitD20.CLERIC_ILMATER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_LATHANDER) == KitD20.CLERIC_LATHANDER, KitD20.CLERIC_LATHANDER.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_SELUNE) == KitD20.CLERIC_SELUNE, KitD20.CLERIC_SELUNE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_HELM) == KitD20.CLERIC_HELM, KitD20.CLERIC_HELM.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_OGHMA) == KitD20.CLERIC_OGHMA, KitD20.CLERIC_OGHMA.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_TEMPUS) == KitD20.CLERIC_TEMPUS, KitD20.CLERIC_TEMPUS.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_BANE) == KitD20.CLERIC_BANE, KitD20.CLERIC_BANE.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_MASK) == KitD20.CLERIC_MASK, KitD20.CLERIC_MASK.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Archetype & KitD20.CLERIC_TALOS) == KitD20.CLERIC_TALOS, KitD20.CLERIC_TALOS.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which CreatureD20Flags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetSpecialFlags3EString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.SpecialFlags & SpecialFlags3E.ConcentrationSuccess) == SpecialFlags3E.ConcentrationSuccess, SpecialFlags3E.ConcentrationSuccess.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SpecialFlags & SpecialFlags3E.ImmunteToCriticalHits) == SpecialFlags3E.ImmunteToCriticalHits, SpecialFlags3E.ImmunteToCriticalHits.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SpecialFlags & SpecialFlags3E.CannotTakeLevelsInPaladin) == SpecialFlags3E.CannotTakeLevelsInPaladin, SpecialFlags3E.CannotTakeLevelsInPaladin.GetDescription());
            StringFormat.AppendSubItem(sb, (this.SpecialFlags & SpecialFlags3E.CannotTakeLevelsInMonk) == SpecialFlags3E.CannotTakeLevelsInMonk, SpecialFlags3E.CannotTakeLevelsInMonk.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}