using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell2;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Spell2
{
    /// <summary>Version 2 spell</summary>
    public class Spell2 : Spell
    {
        #region Properties
        /// <summary>Exposed header</summary>
        public Spell2Header Header
        {
            get { return this.header as Spell2Header; }
            set { this.header = value; }
        }

        /// <summary>Spell's headline for a friendly-text display</summary>
        protected override String Headline
        {
            get { return "SPELL Version 1.0:"; }
        }

        /// <summary>Size of the spell's header</summary>
        protected override UInt32 HeaderSize
        {
            get { return Spell2Header.StructSize; }
        }
        #endregion


        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Spell2()
        {
            this.Initialize();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new Spell2Header();
        }
        #endregion
    }
}