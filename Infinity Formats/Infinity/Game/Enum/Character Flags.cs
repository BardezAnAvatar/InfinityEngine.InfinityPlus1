using System;
namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the flags associated with the playable character</summary>
    public enum CharacterFlags : ushort /* UInt16 */
    {
        Selected = 1,
        Dead = 0x8000,
    }
}