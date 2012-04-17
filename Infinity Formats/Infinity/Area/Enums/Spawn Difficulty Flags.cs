using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Difficulty spawning flags</summary>
    [Flags]
    public enum SpawnDifficultyFlags : byte /* Byte */
    {
        /// <summary>Spawns for difficulty level 1.</summary>
        [Description("Spawns for difficulty level 1.")]
        DifficultyLevel1    = 1,

        /// <summary>Spawns for difficulty level 2.</summary>
        [Description("Spawns for difficulty level 2.")]
        DifficultyLevel2    = 2,

        /// <summary>Spawns for difficulty level 3.</summary>
        [Description("Spawns for difficulty level 3.")]
        DifficultyLevel3    = 4,
    }
}