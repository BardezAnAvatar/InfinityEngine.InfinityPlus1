using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>Various flags set on the creature</summary>
    /// <remarks>
    ///     These bits can be checked using the IsCreatureAreaFlag trigger,
    ///     and set using the SetCreatureAreaFlag action.
    ///     
    ///     See CREAREFL.IDS:
    ///     0x00008000       ENABLED
    ///     0x00010000       HAS_SEEN_PARTY
    ///     0x00020000       INVULNERABLE
    ///     0x00040000       NON_THREATING_ENEMY
    ///     0x00080000       NO_TALK
    ///     0x00100000       IGNORE_RETURN_TO_START_POS
    ///     0x00200000       IGNORE_AI_INHIBIT
    /// </remarks>
    [Flags]
    public enum CreatureD20Flags : uint /* Int32 */
    {
        [Description("Damage don't stop casting")]
        KeepCastingWhenDamaged          = 0x00000001U,

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

        /// <remarks>"Enabled" in IESDP, almost never set in CRE files; I think it is the same</remarks>
        [Description("Been in party")]
        BeenInParty                     = 0x00008000U,

        [Description("Seen Party (Activated in game yet?)")]
        SeenParty                       = 0x00010000U,

        [Description("Invulnerable")]
        Invulnerable                    = 0x00020000U,

        //18-23 unused

        [Description("Non-threatening Enemy")]
        NonThreateningEnemy             = 0x01000000U,

        [Description("No talk")]
        NoTalk                          = 0x02000000U,

        [Description("Ignore return to start")]
        IgnoreReturnToStart             = 0x04000000U,

        [Description("Ignore inhibit AI")]
        IgnoreInhibitAI                 = 0x08000000U,

        //[Description("")]
        //Unused1                         = 0x10000000U,

        //[Description("")]
        //Unused2                         = 0x20000000U,

        [Description("Unknown/corpse related?")]
        UnknownCorpseRelated             = 0x40000000U,

        ///// <summary>Creature is uninterruptable</summary>
        ///// <remarks>In-memory only</remarks>
        //Unused3                         = 0x80000000U,
    }
}