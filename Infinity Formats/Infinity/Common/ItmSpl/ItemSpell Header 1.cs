using System;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl
{
    /// <summary>This class is the implementation of version 1 items and overlay headers.</summary>
    /// <remarks>
    ///     The values of these file types that are not shared here are obviously parallels
    ///     of one another. The binary setup is the same, the offsets are the same, but in
    ///     the end, implementation ended up differing. Those with a keen eye can see that
    ///     these originally were the same file format (just like CRE and CHR), but in the
    ///     implementation of the engine, had to eventually be branched apart. This is
    ///     illustrated where bitfields are separate until later versions caused the meanings
    ///     to overlap. I would like to implement it similarly, BUT the two being separate as
    ///     they are leaves it impractical to have generic enumerators or x-wide integers with
    ///     multiple uses.
    /// </remarks>
    public abstract class ItemSpellHeader1 : InfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 0;    //here for signature purposes

        #region Members
        /// <summary>String reference to unidentified name</summary>
        protected StringReference nameUnidentified;

        /// <summary>String reference to identified name</summary>
        protected StringReference nameIdentified;

        /// <summary>String reference to unidentified description</summary>
        protected StringReference descriptionUnidentified;

        /// <summary>String reference to identified description</summary>
        protected StringReference descriptionIdentified;

        /// <summary>Offset to the item abilities</summary>
        protected UInt32 offsetAbilities;

        /// <summary>Count of the extended headers</summary>
        protected UInt16 countAbilities;

        /// <summary>Offset to the features</summary>
        protected UInt32 offsetAbilityEffects;

        //These should be separate, really...
        /// <summary>Offset to the equipped/cast features</summary>
        protected UInt16 indexEquippedEffects;

        /// <summary>Count of the equipped/cast features</summary>
        protected UInt16 countEquippedEffects;
        #endregion
        
        #region Properties
        public StringReference NameUnidentified
        {
            get { return this.nameUnidentified; }
            set { this.nameUnidentified = value; }
        }

        public StringReference NameIdentified
        {
            get { return this.nameIdentified; }
            set { this.nameIdentified = value; }
        }

        public StringReference DescriptionUnidentified
        {
            get { return this.descriptionUnidentified; }
            set { this.descriptionUnidentified = value; }
        }

        public StringReference DescriptionIdentified
        {
            get { return this.descriptionIdentified; }
            set { this.descriptionIdentified = value; }
        }

        public UInt32 OffsetAbilities
        {
            get { return this.offsetAbilities; }
            set { this.offsetAbilities = value; }
        }

        public UInt16 CountAbilities
        {
            get { return this.countAbilities; }
            set { this.countAbilities = value; }
        }

        public UInt32 OffsetAbilityEffects
        {
            get { return this.offsetAbilityEffects; }
            set { this.offsetAbilityEffects = value; }
        }

        public UInt16 IndexEquippedEffects
        {
            get { return this.indexEquippedEffects; }
            set { this.indexEquippedEffects = value; }
        }

        public UInt16 CountEquippedEffects
        {
            get { return this.countEquippedEffects; }
            set { this.countEquippedEffects = value; }
        }
        #endregion
    }
}