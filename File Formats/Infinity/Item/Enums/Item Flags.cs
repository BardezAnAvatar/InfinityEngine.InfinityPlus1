using System;
using System.ComponentModel;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums
{
    [Flags]
    public enum ItemFlags : uint /* UInt32 */
    {
        /// <summary>This is a critical, plot-centric item. It cannot be sold (except in BG1).</summary>
        [Description("Critical/Quest item/Unsellable")]
        CriticalItem    =     1,  // Cannot sell, except BG1

        /// <summary>This item requires two hands to use.</summary>
        [Description("Two-handed")]
        TwoHanded       =     2,

        /// <summary>This item can be moved. If not, the only way to ditch it is via editor or quest.</summary>
        Movable         =     4,

        /// <summary>Does this item get displayed? I *think* this means for within the GUI.</summary>
        Displayable     =     8,

        /// <summary>This item is cursed and cannot be unequipped, except by Remove Curse.</summary>
        Cursed          =    16,

        /// <summary>
        ///     This overlay cannot be scribed into the spellbook. IESDP only defines this for PS:T,
        ///     but it is Unknown for version1, 2 item headers.
        ///     It is called "Not Copyable" in NearInfinity, which makes sense, now, in the light
        ///     of PS:T, after seeing that flag for years and not understanding it. :§
        /// </summary>
        [Description("Cannot scribe spell")]
        CannotScribe    =    32, //PST Only? see IESDP

        /// <summary>This item is magical. I think this means for magic weapons.</summary>
        Magical         =    64,

        /// <summary>
        ///     This item is a ranged weapon.
        ///     IESDP labels this as Unknown for PS:T, but there were no bows.
        /// </summary>
        Bow             =   128,

        /// <summary>This weapon is coated with or forged from (alchemical) silver.</summary>
        Silver          =   256,

        /// <summary>This weapon is forged from cold iron. Effective against demons.</summary>
        [Description("Cold iron")]
        ColdIron        =   512,

        /// <summary>The item is stolen, and unsellable. Or, two-handed animation (?) Or, Steel (PS:T)? DLTCEP suggests 'lefthanded (buggy)'</summary>
        Stolen          =  1024,

        /// <summary>
        ///     This item is conversable. A side effet, I believe, is that you cannot put it in a container.
        /// </summary>
        Conversable     =  2048,

        /// <summary>This item is pulsating. Whatever that means. DLTCEP suggests this is PS:T only.</summary>
        Pulsating       =  4096
    }
}