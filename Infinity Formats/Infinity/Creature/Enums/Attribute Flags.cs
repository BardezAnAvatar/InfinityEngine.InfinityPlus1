using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    [Flags]
    public enum AttributeFlags : uint /* UInt32 */
    {
        Transparent                                 = 0x00000002,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment <deathvariable>_DEAD counter")]
        IncrementDeathVariableCounter               = 0x00000010,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment kill counter for entry in monstrous compendium")]
        IncrementCharacterTypeDeathCounter          = 0x00000020,
        
        [Description("Death counter starts with WILL_")]
        DeathCounterStartsWithKill                  = 0x00000040,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the matching faction death counter")]
        IncrementFactionDeathCounter                = 0x00000080,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the matching team death counter")]
        IncrementTeamDeathCounter                   = 0x00000100,

        Invulnerable                                = 0x00000200,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the GOOD variable")]
        IncrementVariableGood                       = 0x00000400,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the LAW variable")]
        IncrementVariableLaw                        = 0x00000800,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the LADY variable")]
        IncrementVariableLady                       = 0x00001000,

        /// <remarks>Kill() will also increment the counter. DestroySelf() will not.</remarks>
        [Description("Increment the MURDER variable")]
        IncrementVariableMurder                     = 0x00002000,

        /// <summary>Do not force creature to turn around to party when dialog begins</summary>
        [Description("Do not force creature to turn to face <GABBER>")]
        DoNotForceFacingTargetDuringDialog          = 0x00004000,

        /// <summary>Help() AI script call turns nearby creatures party-hostile</summary>
        [Description("Help() call from this creature will turn others nearby hostile")]
        HelpCallTurnsNearbyCreaturesHostileToParty  = 0x00008000,

        /// <summary>Used in save games to prevent NPC deaths from counting as duplicates</summary>
        [Description("Do not re-increment duplicated variables once dead (in save ames)")]
        DoNotIncrementGlobals                       = 0x40000000
    }
}