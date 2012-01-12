using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Enums
{
    public enum StoreType : uint /* UInt32 */
    {
        Store = 0U,
        Tavern,
        Inn,
        Temple,

        [Description("Container (IWD series only)")]
        ContainerIWD,
        Container
    }
}