using System;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Enums
{
    /// <summary>This enum represents the types of rooms available for the night</summary>
    [Flags]
    public enum AvailableRooms : uint /* UInt32 */
    {
        Peasant     = 1U,
        Merchant    = 2U,
        Noble       = 4U,
        Royal       = 8U
    }
}