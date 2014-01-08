using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represent the flags for expansion variables</summary>
    public enum Expansion : uint /* UInt32 */
    {
        /// <remarks>BG2 source code says Baldur</remarks>
        None = 0,   //Restrict to BG1 XP limit?

        /// <remarks>BG2 source code says Baldur_MP1 (mission pack 1)</remarks>
        TotSC = 1,  //Restrict to TotSC XP limit?

        /// <remarks>BG2 source code says Baldur_MP2 (mission pack 2)</remarks>
        UnknownBaldursGateExpansion = 2,    //Value should not be found

        /// <remarks>
        ///     GemRB suggests this is IWD (confirmed); Also IWD2;
        ///     IESDP says both xnewarea.2da processing to be done and xnewarea.2da processing complete
        ///
        ///     BG2 source code says that this is SoA
        /// </remarks>
        IWD = 3,    //Restrict to IWD XP limit?

        /// <remarks>BG2 source code says Baldur2_addin</remarks>
        ToBSoA = 4,     //Seems to be in a ToB game without ToB Active yet

        /// <remarks>BG2 source code says Baldur2_addon</remarks>
        ToB = 5,        //ToB Active?
    }
}