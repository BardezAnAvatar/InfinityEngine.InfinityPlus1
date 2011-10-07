using System;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Creature2_2
{
    /// <summary>Class that contains the offset to and count of known spells in a creature version 2.2 file</summary>
	public class D20KnownSpellOffsetData
	{
        #region Members
        /// <summary>Data offset</summary>
        protected UInt32 offset;

        /// <summary>Instance count</summary>
        protected UInt32 count;
        #endregion

        #region Properties
        /// <summary>Data offset</summary>
        public UInt32 Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }

        /// <summary>Instance count</summary>
        public UInt32 Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        /// <summary>Total size at offset</summary>
        public UInt32 Size
        {
            get { return (this.count * CreatureD20KnownSpell.StructSize) + CreatureD20SpellMemorization.StructSize; }
        }

        /// <summary>Total size at offset</summary>
        public UInt32 OffsetEnd
        {
            get { return this.offset + this.Size; }
        }
        #endregion

        #region Constructors
        /// <summary>Default constructor</summary>
        public D20KnownSpellOffsetData() { }

        /// <summary>Count constructor</summary>
        public D20KnownSpellOffsetData(UInt32 countValue)
        {
            this.count = countValue;
        }
        #endregion
    }
}