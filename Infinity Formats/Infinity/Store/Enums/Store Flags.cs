using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Enums
{
    [Flags]
    public enum StoreFlags : uint /* UInt32 */
    {
        [Description("Sells items")]
        UserCanBuy      = 1U,

        [Description("Buys items")]
        UserCanSell     = 2U,

        [Description("Identifies items")]
        Identifies      = 4U,

        [Description("User can steal")]
        UserCanSteal    = 8U,

        [Description("Charity")]
        DonateMoney     = 16U,

        [Description("Heals")]
        Heals           = 32U,

        [Description("Sells drinks")]
        LiquorLicense   = 64U,

        //7 and 8 are unknown

        /********************************************************************
        *   Tavern quality seems to be used in bg1/iwd/bg2.                 *
        *   It determines which picture is shown in the 'drinks' screen.    *
        *    The pictures are named: tvrnqul0-3.                            *
        *   Source: http://forums.gibberlings3.net/index.php?showtopic=3069 *
        ********************************************************************/

        [Description("First quality bit")]
        Quality1        = 512U,

        [Description("Second quality bit")]
        Quality2        = 1024U,

        // 11 unknown

        [Description("Fences stolen goods")]
        Fence           = 4096U
    }
}