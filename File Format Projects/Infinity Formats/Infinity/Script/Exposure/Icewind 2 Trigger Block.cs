using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.IWD2
{
    /// <summary>A Baldur's Gate BioWare script's trigger block container</summary>
    /// <remarks>
    ///     The trigger block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG, BG2, IWD, IWD2:
    ///     [Int] x 5, [String] x 2, OBject (sic)
    /// </remarks>
    public class Icewind2TriggerBlock : TriggerBlock
    {
        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
           // this.objBlock = new Icewind2ObjectBlock();
        }
        #endregion
    }
}