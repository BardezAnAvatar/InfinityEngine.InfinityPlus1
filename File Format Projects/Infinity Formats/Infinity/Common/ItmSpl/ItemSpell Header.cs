using System;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl
{
    /// <summary>This class is the implementation of version 1 item and spell headers.</summary>
    /// <remarks>
    ///     The values of these file types that are not shared here are obviously parallels
    ///     of one another. The binary setup is the same, the offsets are the same, but in
    ///     the end, implementation ended up differing. Those with a (not-so-) keen eye can see that
    ///     these originally were the same file format (just like CRE and CHR), but in the
    ///     implementation of the engine, had to eventually be branched apart. This is
    ///     illustrated where bitfields are separate until later versions caused the meanings
    ///     to overlap. I would like to implement it similarly, BUT the two being separate as
    ///     they are leaves it impractical to have generic enumerators or x-wide integers with
    ///     multiple uses. Example: Minimum stats are shared, but some are shorts, some are bytes;
    ///     the icon for items and spells is the same field, but the spell's icon's last character is replaced; etc.
    /// </remarks>
    public abstract class ItemSpellHeader : InfinityFormat
    {
        #region Fields
        /// <summary>String reference to unidentified name</summary>
        public StringReference NameUnidentified { get; set; }

        /// <summary>String reference to identified name</summary>
        public StringReference NameIdentified { get; set; }

        /// <summary>String reference to unidentified description</summary>
        public StringReference DescriptionUnidentified { get; set; }

        /// <summary>String reference to identified description</summary>
        public StringReference DescriptionIdentified { get; set; }

        /// <summary>The maximum count of this item in one stack</summary>
        public UInt16 StackSize { get; set; }

        /// <summary>Lore or Knowledge(arcana) [no, Spellcraft] to identify item.</summary>
        public UInt16 IdentifyThreshold { get; set; }

        /// <summary>Animation associated with this item on the ground</summary>
        public ResourceReference GroundIcon { get; set; }

        /// <summary>Weight in pounds of this item</summary>
        public UInt32 Weight { get; set; }

        /// <summary>BAM resource for the item's icon</summary>
        public ResourceReference DescriptionIcon { get; set; }

        /// <summary>Level of enchantment when calculating magical enchantment to hit creatures with damage resistance. Not attack or damage bonus.</summary>
        public UInt32 Enchantment { get; set; }

        /// <summary>Offset to the item abilities</summary>
        public UInt32 OffsetAbilities { get; set; }

        /// <summary>Count of the extended headers</summary>
        public UInt16 CountAbilities { get; set; }

        /// <summary>Offset to the features</summary>
        public UInt32 OffsetAbilityEffects { get; set; }

        /// <summary>Offset to the equipped/cast features</summary>
        public UInt16 IndexEquippedEffects { get; set; }

        /// <summary>Count of the equipped/cast features</summary>
        public UInt16 CountEquippedEffects { get; set; }
        #endregion
    }
}