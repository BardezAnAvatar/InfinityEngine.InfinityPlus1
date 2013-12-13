using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Flags for walls a maze area has</summary>
    public enum  MazeWalls : ushort /* UInt16 */
    {
        East = 1,
        West = 2,
        North = 4,
        South = 8,
    }
}