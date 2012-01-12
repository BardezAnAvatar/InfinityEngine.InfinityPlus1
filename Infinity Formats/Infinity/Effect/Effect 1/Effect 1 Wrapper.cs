using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1
{
    public class Effect1Wrapper : EffectWrapper
    {
        protected Effect1 effect;

        /// <summary>Should interface with an Effect1</summary>
        public override IInfinityFormat Effect
        {
            get { return this.effect; }
            set { this.effect = value as Effect1; }
        }
        /// <summary>efault constructor</summary>
        public Effect1Wrapper()
        {
            this.effect = null;
        }

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.effect = new Effect1();
        }
    }
}