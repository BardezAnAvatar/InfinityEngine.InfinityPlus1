using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums
{
    /// <summary>Various flags set on the creature</summary>
    /// <remarks>For multi-classed characters, none of the original class are set; only for dual- and single- class</remarks>
    [Flags]
    public enum Creature2eFlags : uint /* Int32 */
    {
        [Description("Show Long Name in tooltip")]
        ShowLongName                    = 0x00000001U,

        [Description("Do not persist a corpse")]
        NoCorpse                        = 0x00000002U,

        [Description("Corpse is permanent")]
        PermanentCorpse                 = 0x00000004U,

        [Description("Original class was Fighter")]
        OriginalClassFighter            = 0x00000008U,

        [Description("Original class was Mage")]
        OriginalClassMage               = 0x00000010U,

        [Description("Original class was Cleric")]
        OriginalClassCleric             = 0x00000020U,

        [Description("Original class was Thief")]
        OriginalClassThief               = 0x00000040U,

        [Description("Original class was Druid")]
        OriginalClassDruid              = 0x00000080U,

        [Description("Original class was Ranger")]
        OriginalClassRanger             = 0x00000100U,

        [Description("Fallen Paladin")]
        FallenPaladin                   = 0x00000200U,

        [Description("Fallen Ranger")]
        FallenRanger                    = 0x00000400U,

        Exportable                      = 0x00000800U,

        [Description("Hide injuries in tooltip")]
        HideInjuryStatusInTooltip       = 0x00001000U,

        /// <remarks>
        ///     A byte that is hacked in, per Gibberlings 3 discussion, almost exclusively 0,
        ///         in ITM does 'alternate damage'.
        ///     THIS flag is called quest critical/affected by alternate damage.
        ///     ... Is this instead simply an invincibility flag (i.e.: for Elminster)?
        /// </remarks>
        [Description("Quest Critical creature / Affected by alternate damage")]
        QuestCritical                   = 0x00002000U,

        [Description("Activates cannot be used by NPC triggers")]
        ActivatesCannotBeUsedByNpcs     = 0x00004000U,

        [Description("Been in party")]
        BeenInParty                     = 0x00008000U,

        [Description("Restore item in hand")]
        RestoreItemInHand               = 0x00010000U,

        [Description("Unsets 'Restore item in hand' Flag")]
        UnsetRestoreItemInHandFlag      = 0x00020000U,

        //18-23 unused

        [Description("Related to random walk (ea)")]
        RandomWalkEnemyAlly             = 0x01000000U,

        [Description("Related to random walk (general)")]
        RandomWalkGeneral               = 0x02000000U,

        [Description("Related to random walk (race)")]
        RandomWalkRace                  = 0x04000000U,

        [Description("Related to random walk (class)")]
        RandomWalkClass                 = 0x08000000U,

        [Description("Related to random walk (specific)")]
        RandomWalkSpecific              = 0x10000000U,

        [Description("Related to random walk (gender)")]
        RandomWalkGender                = 0x20000000U,

        [Description("Related to random walk (alignment)")]
        RandomWalkAlignment             = 0x40000000U,

        /// <summary>Creature is uninterruptable</summary>
        /// <remarks>In-memory only</remarks>
        Uninterruptable                 = 0x80000000U,
    }
}