using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Item
{
    /// <summary>Base class for an item</summary>
    public abstract class ItemBase : ItemSpell
    {
        #region Fields
        /// <summary>Collection of item abilities associated with this item</summary>
        public List<ItemAbility> ItemAbilities { get; set; }
        #endregion


        #region Properties
        /// <summary>Size of one of this spell's abilities</summary>
        protected override UInt32 AbilitySize
        {
            get { return ItemAbility.StructSize; }
        }

        /// <summary>Collection of abilities associated with this item</summary>
        protected override IList<ItemSpellAbility> Abilities
        {
            get { return this.ItemAbilities.ToArray(); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.InstantiateHeader();
            this.ItemAbilities = new List<ItemAbility>();
            this.Effects = new List<Effect1>();
        }
        #endregion


        #region IO method implemetations
        /// <summary>Reads the abilities from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadAbilities(Stream input)
        {
            for (Int32 index = 0; index < this.header.CountAbilities; ++index)
            {
                ItemAbility ability = new ItemAbility();
                ability.Read(input);
                this.ItemAbilities.Add(ability);
            }
        }
        #endregion
    }
}