using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for actors</summary>
    [Flags]
    public enum ActorFlags : uint /* UInt32 */
    {
        /// <summary>A CRE resource is embedded.</summary>
        [Description("A CRE resource is embedded.")]
        EmbeddedCre         = 1U,

        /// <summary>Override the CRE's scripts.</summary>
        [Description("Override the CRE's scripts.")]
        OverrideScriptNames  = 8U,
    }
}