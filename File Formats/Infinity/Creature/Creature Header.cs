using System;
using System.Collections.Specialized;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature
{
    public abstract class CreatureHeader : InfinityFormat
    {
        #region Members
        /// <summary>The full name of the creature</summary>
        protected StringReference nameLong;

        /// <summary>Shot name of the creature (often displayed as the tooltip)</summary>
        protected StringReference nameShort;

        //Common 32-bit flag field, however, while many of the flags are shared, I prefer to use enumerators.
        // Using one enumerator with a field for multiple values, an example of of DamageOrShowLongName is impractical
        // if there is more than just one of these.

        /// <summary>Experience value for killing this creature</summary>
        protected UInt32 experienceValue;

        /// <summary>Total XP that this creature has/creature power level for summoning</summary>
        protected UInt32 experienceTotal;

        /// <summary>The amount of gold that this creature carries (added when joining party? I thought this was an item ususally)</summary>
        protected UInt32 gold;

        /// <summary>Matches values in STATE.IDS</summary>
        protected UInt32 statusFlags;

        /// <summary>The current number of hit points (this can be above maximum, I believe, if effects such as from items add additional HP)</summary>
        protected UInt16 hitPointsCurrent;

        /// <summary>The character's total hit points from hit dice rolls, permanent adjustments, etc.</summary>
        protected UInt16 hitPointsMaximum;

        /// <summary>
        ///     This is the creature animation for the creature.
        ///     From IESDP: "There is some structure to the ordering of these entries, however, it is mostly hard-coded into the .exe, and uneditable."
        /// </summary>
        /// <remarks>
        ///     Fun. I'll have to hard code the file import for IDs. Yay.
        ///     
        ///     IESDP says it is 16-but, and there is a two-byte unknown that follows this, and until I hear otherwise, it is to me a 32-bit field.
        /// </remarks>
        protected UInt32 animationId;

        /// <summary>Metal color index</summary>
        protected Byte colorIndexMetal;

        /// <summary>Minor color index</summary>
        protected Byte colorIndexMinor;

        /// <summary>Major color index</summary>
        protected Byte colorIndexMajor;

        /// <summary>Skin color index</summary>
        protected Byte colorIndexSkin;

        /// <summary>Leather color index</summary>
        protected Byte colorIndexLeather;

        /// <summary>Armor color index</summary>
        protected Byte colorIndexArmor;

        /// <summary>Hair color index</summary>
        protected Byte colorIndexHair;

        /// <summary>Boolean flag indicating whether to use the expanded EFF structure (version 2) or the default version 1</summary>
        protected Boolean useEffectStructureVersion2;

        /// <summary>Resource reference to the small portrait</summary>
        protected ResourceReference portraitSmall;

        /// <summary>Resource reference to the larger portrait</summary>
        protected ResourceReference portraitLarge;

        /// <summary>reputation value (minimum of 0)</summary>
        protected Byte reputation;

        /// <summary>
        ///     This byte is the value of the hide in shadows skill, that maxes out around 255.
        ///     The 2E player's handbook specifies that this value maxes out at 95. lolwut for BG2.
        ///     
        ///     This value is also unused in IWD2 (it was an unknown until someone realized it was copied code, dur), and is assumed
        ///     to be wrapped into an Int16 field for reputation in PST; I think I will instead go
        ///     with the unused, copied code line of thought for PS:T. -- also, NI reports it as separate.
        /// </summary>
        protected Byte hideInShadows;

        /// <summary>The natural armor class for the creature</summary>
        protected Int16 armorClassNatural;

        //This is where the different headers start to diverge, with IWD2 dropping an AC field, but, this structure isn't byte-aligned
        //  and I write my own read/write routines, so, natch, inheritance for the win.

        /// <summary>Crushing AC modifier.</summary>
        /// <remarks>IESDP suggests this is signed, but I have my doubts. Plus, non-negative values are stored the same as signed ints.</remarks>
        protected Int16 armorClassModifierCrushing;

        /// <summary>Missile AC modifier.</summary>
        /// <remarks>IESDP suggests this is signed, but I have my doubts. Plus, non-negative values are stored the same as signed ints.</remarks>
        protected Int16 armorClassModifierMissile;

        /// <summary>Piercing AC modifier.</summary>
        /// <remarks>IESDP suggests this is signed, but I have my doubts. Plus, non-negative values are stored the same as signed ints.</remarks>
        protected Int16 armorClassModifierPiercing;

        /// <summary>Slashing AC modifier.</summary>
        /// <remarks>IESDP suggests this is signed, but I have my doubts. Plus, non-negative values are stored the same as signed ints.</remarks>
        protected Int16 armorClassModifierSlashing;

        /// <summary>THAC0 in 2E, BAB for D20</summary>
        protected Byte attackBase;

        /// <summary>Number of attacks per round</summary>
        /// <value>
        ///     For 2E, this is really annoying. It appears to go 0->1->2->3->4->5->1.5->2.5->3.5->4.5
        ///     So, 0=0 per round, 1=1, 2=2 3=3, 4=4, 5=5, 6 = 1.5, 7 = 2.5, 8 = 3.5 and 9 (Sarevok) = 4.5
        ///     
        ///     Couldn't they have just used a float? or a decimal notation?
        ///     
        ///     For D20, thankfully, it is an integer (mathematical, not memory size)
        /// </value>
        protected Byte attacksPerRound;

        //Next up are the saves. 5 2E saves, 3 D20 saves. Skipping.

        /// <summary>Fire damage resistance</summary>
        protected Byte resistFire;

        /// <summary>Cold damage resistance</summary>
        protected Byte resistCold;

        /// <summary>Electricity damage resistance</summary>
        protected Byte resistElectricity;

        /// <summary>Acid damage resistance</summary>
        protected Byte resistAcid;

        /// <summary>Magic resistance. Not magic damage resistance.</summary>
        protected Byte resistMagic;

        /// <summary>Magic fire damage resistance.</summary>
        /// <remarks>The difference between fire and magic fire is lame.</remarks>
        protected Byte resistFireMagic;

        /// <summary>Magic cold damage resistance.</summary>
        /// <remarks>The difference between cold and magic cold is lame.</remarks>
        protected Byte resistColdMagic;

        /// <summary>Physical slashing damage resistance</summary>
        protected Byte resistPhysicalSlashing;

        /// <summary>Physical crushing damage resistance</summary>
        protected Byte resistPhysicalCrushing;

        /// <summary>Physical piercing damage resistance</summary>
        protected Byte resistPhysicalPiercing;

        /// <summary>Physical missile damage resistance</summary>
        protected Byte resistPhysicalMissile;

        //D20 structure splits here, adds additional resistance. 2E structures have additional fields:
        //  Detect Ilusions, set traps, lore, lockpicking, stealth, find/disarm traps, pick pockets; it picks backup at fatigue

        /// <summary>Fatigue level</summary>
        /// <value>This value *probably* relates to the current value, approaching a threshold that dictates fatigue. All CREs seem to have 0.</value>
        /// <remarks>I am wrong and right. To see tangible effects, see FATIGMOD.2DA</remarks>
        protected Byte fatigue;

        /// <summary>Intoxication level</summary>
        /// <value>This value *probably* relates to the current value, approaching a threshold that dictates intoxication. All CREs seem to have 0.</value>
        /// <remarks>I am wrong and right. To see tangible effects, see INTOXMOD.2DA</remarks>
        protected Byte intoxication;

        /// <summary>Luck</summary>
        /// <remarks>
        ///     I would *love* to know what th luck stat modifies. I think, from reading Gibberling, etc. that this impacts thieving skills. Maybe skill checks as a whole?
        ///     Also, I see no real AD&D 2E rules for luck, so I think that luck can be factored in to, say, the liklihood of a nat 20 or crit fail, skill checks in D20, etc.
        /// </remarks>
        protected Byte luck;

        //another divergence; 2E has 21 bytes of used & unused proficiencies; these proficiencies a split into 3-bit chunks, apparently, for multiple classes (I think this would
        //  apply only for dual classing charcters.

        //Following this appears to be a whole slew of skill bytes.
        //  In 2E this starts with a tracking skill Byte, then 32 unknown bytes at 0x84
        //  In D20, this starts with turn undead level, and proceeds with 34 unknown bytes at 0x69.
        //  I proffer that this whole section was "Proficiencies" that encompassed "non-weapon proficiencies"...
        //      This would have been 21+1+32 = 22+32 = 54 bytes for 2E
        //      This would have been perhaps used as skills for D20 and later mutated into various attributes.
        //  ANY way, there is one field of reserved bytes in 2E, and 2 in D20, so I will localize their declaration rather than do one here due to their differing nature.

        //Next is the set of String Reference entries for the soundsets. 2E has 100, D20 has 64...
        /// <summary>The list of soundset entries.</summary>
        /// <remarks>100 in 2E, 64 in D20; I would prefer a data structure that offers Int32 index and String index, but...</remarks>
        protected GenericOrderedDictionary<String, StringReference> soundSet;

        //following this is the list of levels for 2E creatures. The data structure is quite divergent at this point, and a good place for me to go to sleep...
        //  Here, the D20 format has two additional scripts, magical enchantment to overcome, feat bitfields, weapon proficiency feat levels (as well as toughness,
        //  and other stacking feats, then finally skills, Challenge rating, subrace.

        //2E has a leading gender at a later slot, but this the shared declaration is later in all structures

        /// <summary>Strength ability score</summary>
        protected Byte scoreStrength;

        //2E structures have the Strength % bonus here, but not

        /// <summary>Intelligence ability score</summary>
        protected Byte scoreIntelligence;

        /// <summary>Wisdom ability score</summary>
        protected Byte scoreWisdom;

        /// <summary>Dexterity ability score</summary>
        protected Byte scoreDexterity;

        /// <summary>Constitution ability score</summary>
        protected Byte scoreConstitution;

        /// <summary>Charisma ability score</summary>
        protected Byte scoreCharisma;

        /// <summary>Creature's current/spawning morale</summary>
        protected Byte morale;

        /// <summary>Creature's morale breaking point</summary>
        /// <remarks>I've never really seen this used. Is this to run away?</remarks>
        protected Byte moraleBreak;

        /// <summary>Time to recover morale</summary>
        protected UInt16 moraleRecoveryTime;

        //2E structures have morale, morale break, racial enemy then morale recovery time in 5 total bytes; D20 has 4 bytes.


        /// <summary>Override script</summary>
        protected ResourceReference scriptOverride;

        // this is another diversion point this splits into a fork of BG/PST/IWD

        /// <summary>This is the player hostility of the creature</summary>
        /// <value>match to Enemy-Ally value in EA.IDS</value>
        protected Byte classificationHostility;

        /// <summary>General classification of the creature</summary>
        /// <value>match to GENERAL.IDS</value>
        protected Byte classificationGeneral;

        /// <summary>Race classification of the creature</summary>
        /// <value>match to RACE.IDS</value>
        protected Byte classificationRace;

        /// <summary>Class classification of the creature</summary>
        /// <value>Match to CLASS.IDS</value>
        protected Byte classificationClass;

        /// <summary>Specific classification of the creature</summary>
        /// <value>Match to SPECIFIC.IDS</value>
        protected Byte classificationSpecific;

        /// <summary>Gender classification of the creature</summary>
        /// <value>Match to GENDER.IDS</value>
        /// <remarks>For at least BG1&2, this determines the casting voice as well as the monster summon 'gender'</remarks>
        protected Byte classificationGender;

        /// <summary>First Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        protected Byte classificationObject1;

        /// <summary>Second Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        protected Byte classificationObject2;

        /// <summary>Third Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        protected Byte classificationObject3;

        /// <summary>Fourth Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        protected Byte classificationObject4;

        /// <summary>Fifth Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        protected Byte classificationObject5;

        /// <summary>Alignment classification of the creature</summary>
        /// <value>Match to ALIGNMEN.IDS</value>
        protected Byte classificationAlignment;

        /// <summary>Global actor enumeration value</summary>
        /// <remarks>Is this an index into the .GAM file actor entries?</remarks>
        protected Int16 enumGlobal;

        /// <summary>Local/area actor enumeration value</summary>
        /// <remarks>Is this an index into the .ARE file actor entries?</remarks>
        protected Int16 enumLocal;

        /// <summary>Death script variable postfix</summary>
        /// <value>Sets the SPRITE_IS_DEAD variable on death</value>
        protected String deathVariable;

        //more deviation on spells and memorization

        /// <summary>Offset to item slots</summary>
        protected UInt32 offsetItemSlots;

        /// <summary>Offset to items</summary>
        protected UInt32 offsetItems;

        /// <summary>Count of item entries</summary>
        protected UInt32 countItems;

        /// <summary>Offset to effects</summary>
        protected UInt32 offsetEffects;

        /// <summary>Count of effect entries</summary>
        protected UInt32 countEffects;

        /// <summary>Associated dialog file</summary>
        protected ResourceReference dialog;
        #endregion

        #region Properties
        /// <summary>The full name of the creature</summary>
        public StringReference NameLong
        {
            get { return this.nameLong; }
            set { this.nameLong = value; }
        }

        /// <summary>Shot name of the creature (often displayed as the tooltip)</summary>
        public StringReference NameShort
        {
            get { return this.nameShort; }
            set { this.nameShort = value; }
        }

        /// <summary>Experience value for killing this creature</summary>
        public UInt32 ExperienceValue
        {
            get { return this.experienceValue; }
            set { this.experienceValue = value; }
        }

        /// <summary>Total XP that this creature has</summary>
        public UInt32 ExperienceTotal
        {
            get { return this.experienceTotal; }
            set { this.experienceTotal = value; }
        }

        /// <summary>The amount of gold that this creature carries (added when joining party? I thought this was an item ususally)</summary>
        public UInt32 Gold
        {
            get { return this.gold; }
            set { this.gold = value; }
        }

        /// <summary>Matches values in STATE.IDS</summary>
        public UInt32 StatusFlags
        {
            get { return this.statusFlags; }
            set { this.statusFlags = value; }
        }

        /// <summary>The current number of hit points (this can be above maximum, I believe, if effects such as from items add additional HP)</summary>
        public UInt16 HitPointsCurrent
        {
            get { return this.hitPointsCurrent; }
            set { this.hitPointsCurrent = value; }
        }

        /// <summary>The character's total hit points from hit dice rolls, permanent adjustments, etc.</summary>
        public UInt16 HitPointsMaximum
        {
            get { return this.hitPointsMaximum; }
            set { this.hitPointsMaximum = value; }
        }

        /// <summary>
        ///     This is the creature animation for the creature.
        ///     From IESDP: "There is some structure to the ordering of these entries, however, it is mostly hard-coded into the .exe, and uneditable."
        /// </summary>
        /// <remarks>Fun. I'll have to hard code the file import for IDs. Yay.</remarks>
        public UInt32 AnimationId
        {
            get { return this.animationId; }
            set { this.animationId = value; }
        }

        /// <summary>Metal color index</summary>
        public Byte ColorIndexMetal
        {
            get { return this.colorIndexMetal; }
            set { this.colorIndexMetal = value; }
        }

        /// <summary>Minor color index</summary>
        public Byte ColorIndexMinor
        {
            get { return this.colorIndexMinor; }
            set { this.colorIndexMinor = value; }
        }

        /// <summary>Major color index</summary>
        public Byte ColorIndexMajor
        {
            get { return this.colorIndexMajor; }
            set { this.colorIndexMajor = value; }
        }

        /// <summary>Skin color index</summary>
        public Byte ColorIndexSkin
        {
            get { return this.colorIndexSkin; }
            set { this.colorIndexSkin = value; }
        }

        /// <summary>Leather color index</summary>
        public Byte ColorIndexLeather
        {
            get { return this.colorIndexLeather; }
            set { this.colorIndexLeather = value; }
        }

        /// <summary>Armor color index</summary>
        public Byte ColorIndexArmor
        {
            get { return this.colorIndexArmor; }
            set { this.colorIndexArmor = value; }
        }

        /// <summary>Hair color index</summary>
        public Byte ColorIndexHair
        {
            get { return this.colorIndexHair; }
            set { this.colorIndexHair = value; }
        }

        /// <summary>Boolean flag indicating whether to use the expanded EFF structure (version 2) or the default version 1</summary>
        public Boolean UseEffectStructureVersion2
        {
            get { return this.useEffectStructureVersion2; }
            set { this.useEffectStructureVersion2 = value; }
        }

        /// <summary>Resource reference to the small portrait</summary>
        public ResourceReference PortraitSmall
        {
            get { return this.portraitSmall; }
            set { this.portraitSmall = value; }
        }

        /// <summary>Resource reference to the larger portrait</summary>
        public ResourceReference PortraitLarge
        {
            get { return this.portraitLarge; }
            set { this.portraitLarge = value; }
        }

        /// <summary>reputation value (minimum of 0)</summary>
        public Byte Reputation
        {
            get { return this.reputation; }
            set { this.reputation = value; }
        }

        /// <summary>
        ///     This byte is the value of the hide in shadows skill, that maxes out around 255.
        ///     The 2E player's handbook specifies that this value maxes out at 95. lolwut for BG2.
        ///     
        ///     This value is also unused in IWD2 (it was an unknown until someone realized it was copied code, dur), and is assumed
        ///     to be wrapped into an Int16 field for reputation in PST; I think I will instead go
        ///     with the unused, copied code line of thought for PS:T. -- also, NI reports it as separate.
        /// </summary>
        public Byte HideInShadows
        {
            get { return this.hideInShadows; }
            set { this.hideInShadows = value; }
        }

        /// <summary>The natural armor class for the creature</summary>
        public Int16 ArmorClassNatural
        {
            get { return this.armorClassNatural; }
            set { this.armorClassNatural = value; }
        }

        /// <summary>Crushing AC modifier.</summary>
        public Int16 ArmorClassModifierCrushing
        {
            get { return this.armorClassModifierCrushing; }
            set { this.armorClassModifierCrushing = value; }
        }

        /// <summary>Missile AC modifier.</summary>
        public Int16 ArmorClassModifierMissile
        {
            get { return this.armorClassModifierMissile; }
            set { this.armorClassModifierMissile = value; }
        }

        /// <summary>Piercing AC modifier.</summary>
        public Int16 ArmorClassModifierPiercing
        {
            get { return this.armorClassModifierPiercing; }
            set { this.armorClassModifierPiercing = value; }
        }

        /// <summary>Slashing AC modifier.</summary>
        public Int16 ArmorClassModifierSlashing
        {
            get { return this.armorClassModifierSlashing; }
            set { this.armorClassModifierSlashing = value; }
        }

        /// <summary>THAC0 in 2E, BAB for D20</summary>
        public Byte AttackBase
        {
            get { return this.attackBase; }
            set { this.attackBase = value; }
        }

        /// <summary>Number of attacks per round</summary>
        /// <value>
        ///     This is really annoying. It appears to go 0->1->2->3->4->5->1.5->2.5->3.5->4.5
        ///     So, 0=0 per round, 1=1, 2=2 3=3, 4=4, 5=5, 6 = 1.5, 7 = 2.5, 8 = 3.5 and 9 (Sarevok) = 4.5
        ///     
        ///     Couldn't they have just used a float? or a decimal notation?
        /// </value>
        public Byte AttacksPerRound
        {
            get { return this.attacksPerRound; }
            set { this.attacksPerRound = value; }
        }

        /// <summary>Fire damage resistance</summary>
        public Byte ResistFire
        {
            get { return this.resistFire; }
            set { this.resistFire = value; }
        }

        /// <summary>Cold damage resistance</summary>
        public Byte ResistCold
        {
            get { return this.resistCold; }
            set { this.resistCold = value; }
        }

        /// <summary>Electricity damage resistance</summary>
        public Byte ResistElectricity
        {
            get { return this.resistElectricity; }
            set { this.resistElectricity = value; }
        }

        /// <summary>Acid damage resistance</summary>
        public Byte ResistAcid
        {
            get { return this.resistAcid; }
            set { this.resistAcid = value; }
        }

        /// <summary>Magic resistance. Not magic damage resistance.</summary>
        public Byte ResistMagic
        {
            get { return this.resistMagic; }
            set { this.resistMagic = value; }
        }

        /// <summary>Magic fire damage resistance.</summary>
        /// <remarks>The difference between fire and magic fire is lame.</remarks>
        public Byte ResistFireMagic
        {
            get { return this.resistFireMagic; }
            set { this.resistFireMagic = value; }
        }

        /// <summary>Magic cold damage resistance.</summary>
        /// <remarks>The difference between cold and magic cold is lame.</remarks>
        public Byte ResistColdMagic
        {
            get { return this.resistColdMagic; }
            set { this.resistColdMagic = value; }
        }

        /// <summary>Physical slashing damage resistance</summary>
        public Byte ResistPhysicalSlashing
        {
            get { return this.resistPhysicalSlashing; }
            set { this.resistPhysicalSlashing = value; }
        }

        /// <summary>Physical crushing damage resistance</summary>
        public Byte ResistPhysicalCrushing
        {
            get { return this.resistPhysicalCrushing; }
            set { this.resistPhysicalCrushing = value; }
        }

        /// <summary>Physical piercing damage resistance</summary>
        public Byte ResistPhysicalPiercing
        {
            get { return this.resistPhysicalPiercing; }
            set { this.resistPhysicalPiercing = value; }
        }

        /// <summary>Physical missile damage resistance</summary>
        public Byte ResistPhysicalMissile
        {
            get { return this.resistPhysicalMissile; }
            set { this.resistPhysicalMissile = value; }
        }

        /// <summary>Fatigue level</summary>
        /// <value>This value *probably* relates to the current value, approaching a threshold that dictates fatigue. All CREs seem to have 0.</value>
        /// <remarks>I am wrong and right. To see tangible effects, see FATIGMOD.2DA</remarks>
        public Byte Fatigue
        {
            get { return this.fatigue; }
            set { this.fatigue = value; }
        }

        /// <summary>Intoxication level</summary>
        /// <value>This value *probably* relates to the current value, approaching a threshold that dictates intoxication. All CREs seem to have 0.</value>
        /// <remarks>I am wrong and right. To see tangible effects, see INTOXMOD.2DA</remarks>
        public Byte Intoxication
        {
            get { return this.intoxication; }
            set { this.intoxication = value; }
        }

        /// <summary>Luck</summary>
        /// <remarks>
        ///     I would *love* to know what th luck stat modifies. I think, from reading Gibberling, etc. that this impacts thieving skills. Maybe skill checks as a whole?
        ///     Also, I see no real AD&D 2E rules for luck, so I think that luck can be factored in to, say, the liklihood of a nat 20 or crit fail, skill checks in D20, etc.
        /// </remarks>
        public Byte Luck
        {
            get { return this.luck; }
            set { this.luck = value; }
        }

        /// <summary>The list of soundset entries.</summary>
        /// <remarks>100 in 2E, 64 in D20; I would prefer a data structure that offers Int32 index and String index, but...</remarks>
        public GenericOrderedDictionary<String, StringReference> SoundSet
        {
            get { return this.soundSet; }
            set { this.soundSet = value; }
        }
        
        /// <summary>Strength ability score</summary>
        public Byte ScoreStrength
        {
            get { return this.scoreStrength; }
            set { this.scoreStrength = value; }
        }

        /// <summary>Intelligence ability score</summary>
        public Byte ScoreIntelligence
        {
            get { return this.scoreIntelligence; }
            set { this.scoreIntelligence = value; }
        }

        /// <summary>Wisdom ability score</summary>
        public Byte ScoreWisdom
        {
            get { return this.scoreWisdom; }
            set { this.scoreWisdom = value; }
        }

        /// <summary>Dexterity ability score</summary>
        public Byte ScoreDexterity
        {
            get { return this.scoreDexterity; }
            set { this.scoreDexterity = value; }
        }

        /// <summary>Constitution ability score</summary>
        public Byte ScoreConstitution
        {
            get { return this.scoreConstitution; }
            set { this.scoreConstitution = value; }
        }

        /// <summary>Charisma ability score</summary>
        public Byte ScoreCharisma
        {
            get { return this.scoreCharisma; }
            set { this.scoreCharisma = value; }
        }

        /// <summary>Creature's current/spawning morale</summary>
        public Byte Morale
        {
            get { return this.morale; }
            set { this.morale = value; }
        }

        /// <summary>Creature's morale breaking point</summary>
        /// <remarks>I've never really seen this used. Is this to run away?</remarks>
        public Byte MoraleBreak
        {
            get { return this.moraleBreak; }
            set { this.moraleBreak = value; }
        }

        /// <summary>Time to recover morale</summary>
        public UInt16 MoraleRecoveryTime
        {
            get { return this.moraleRecoveryTime; }
            set { this.moraleRecoveryTime = value; }
        }

        /// <summary>Override script</summary>
        public ResourceReference ScriptOverride
        {
            get { return this.scriptOverride; }
            set { this.scriptOverride = value; }
        }

        /// <summary>This is the player hostility of the creature</summary>
        /// <value>match to Enemy-Ally value in EA.IDS</value>
        public Byte ClassificationHostility
        {
            get { return this.classificationHostility; }
            set { this.classificationHostility = value; }
        }

        /// <summary>General classification of the creature</summary>
        /// <value>match to GENERAL.IDS</value>
        public Byte ClassificationGeneral
        {
            get { return this.classificationGeneral; }
            set { this.classificationGeneral = value; }
        }

        /// <summary>Race classification of the creature</summary>
        /// <value>match to RACE.IDS</value>
        public Byte ClassificationRace
        {
            get { return this.classificationRace; }
            set { this.classificationRace = value; }
        }

        /// <summary>Class classification of the creature</summary>
        /// <value>Match to CLASS.IDS</value>
        public Byte ClassificationClass
        {
            get { return this.classificationClass; }
            set { this.classificationClass = value; }
        }

        /// <summary>Specific classification of the creature</summary>
        /// <value>Match to SPECIFIC.IDS</value>
        public Byte ClassificationSpecific
        {
            get { return this.classificationSpecific; }
            set { this.classificationSpecific = value; }
        }

        /// <summary>Gender classification of the creature</summary>
        /// <value>Match to GENDER.IDS</value>
        /// <remarks>For at least BG1&2, this determines the casting voice as well as the monster summon 'gender'</remarks>
        public Byte ClassificationGender
        {
            get { return this.classificationGender; }
            set { this.classificationGender = value; }
        }

        /// <summary>First Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        public Byte ClassificationObject1
        {
            get { return this.classificationObject1; }
            set { this.classificationObject1 = value; }
        }

        /// <summary>Second Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        public Byte ClassificationObject2
        {
            get { return this.classificationObject2; }
            set { this.classificationObject2 = value; }
        }

        /// <summary>Third Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        public Byte ClassificationObject3
        {
            get { return this.classificationObject3; }
            set { this.classificationObject3 = value; }
        }

        /// <summary>Fourth Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        public Byte ClassificationObject4
        {
            get { return this.classificationObject4; }
            set { this.classificationObject4 = value; }
        }

        /// <summary>Fifth Object classification of the creature</summary>
        /// <value>Match to OBJECT.IDS</value>
        public Byte ClassificationObject5
        {
            get { return this.classificationObject5; }
            set { this.classificationObject5 = value; }
        }

        /// <summary>Alignment classification of the creature</summary>
        /// <value>Match to ALIGNMEN.IDS</value>
        public Byte ClassificationAlignment
        {
            get { return this.classificationAlignment; }
            set { this.classificationAlignment = value; }
        }

        /// <summary>Global actor enumeration value</summary>
        /// <remarks>Is this an index into the .GAM file actor entries?</remarks>
        public Int16 EnumGlobal
        {
            get { return this.enumGlobal; }
            set { this.enumGlobal = value; }
        }

        /// <summary>Local/area actor enumeration value</summary>
        /// <remarks>Is this an index into the .ARE file actor entries?</remarks>
        public Int16 EnumLocal
        {
            get { return this.enumLocal; }
            set { this.enumLocal = value; }
        }

        /// <summary>Death script variable postfix</summary>
        /// <value>Sets the SPRITE_IS_DEAD variable on death</value>
        public String DeathVariable
        {
            get { return this.deathVariable; }
            set { this.deathVariable = value; }
        }

        /// <summary>Offset to item slots</summary>
        public UInt32 OffsetItemSlots
        {
            get { return this.offsetItemSlots; }
            set { this.offsetItemSlots = value; }
        }

        /// <summary>Offset to items</summary>
        public UInt32 OffsetItems
        {
            get { return this.offsetItems; }
            set { this.offsetItems = value; }
        }

        /// <summary>Count of item entries</summary>
        public UInt32 CountItems
        {
            get { return this.countItems; }
            set { this.countItems = value; }
        }
        
        /// <summary>Offset to effects</summary>
        public UInt32 OffsetEffects
        {
            get { return this.offsetEffects; }
            set { this.offsetEffects = value; }
        }

        /// <summary>Count of effect entries</summary>
        public UInt32 CountEffects
        {
            get { return this.countEffects; }
            set { this.countEffects = value; }
        }

        /// <summary>Associated dialog file</summary>
        public ResourceReference Dialog
        {
            get { return this.dialog; }
            set { this.dialog = value; }
        }
        #endregion
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.nameLong = new StringReference();
            this.nameShort = new StringReference();
            this.portraitLarge = new ResourceReference();
            this.portraitSmall = new ResourceReference();
            this.scriptOverride = new ResourceReference();
            this.dialog = new ResourceReference();
            this.soundSet = new GenericOrderedDictionary<String, StringReference>();

            this.InitializeSoundSet();
        }

        /// <summary>Initializes the soundset ordered dictionary</summary>
        protected abstract void InitializeSoundSet();
    }
}