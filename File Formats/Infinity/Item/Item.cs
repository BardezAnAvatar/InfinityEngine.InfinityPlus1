using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item
{
    public abstract class ItemBase : ItemSpell
    {
        protected List<ItemAbility> abilities;

        #region Properties
        public List<ItemAbility> Abilities
        {
            get { return this.abilities; }
            set { this.abilities = value; }
        }

        protected override UInt32 AbilitySize
        {
            get { return ItemAbility.StructSize; }
        }

        protected override ItemSpellAbility[] abilitiesArray
        {
            get { return this.abilities.ToArray(); }
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.InstantiateHeader();
            this.abilities = new List<ItemAbility>();
            this.effects = new List<Effect1>();
        }

        protected override void ReadAbilities(Stream input)
        {
            for (Int32 index = 0; index < this.header.CountAbilities; ++index)
            {
                ItemAbility ability = new ItemAbility();
                ability.Read(input);
                this.abilities.Add(ability);
            }
        }
    }
}