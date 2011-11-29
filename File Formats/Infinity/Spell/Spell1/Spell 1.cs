using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1
{
    public class Spell1 : Spell
    {
        #region Properties
        public Spell1Header Header
        {
            get { return this.header as Spell1Header; }
            set { this.header = value; }
        }

        protected override String Headline
        {
            get { return "SPELL Version 1.0:"; }
        }

        protected override UInt32 HeaderSize
        {
            get { return Spell1Header.StructSize; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public Spell1()
        {
            this.Initialize();
        }

        /// <summary>Instantiates a new header</summary>
        protected override void InstantiateHeader()
        {
            this.header = new Spell1Header();
        }
        #endregion
    }
}