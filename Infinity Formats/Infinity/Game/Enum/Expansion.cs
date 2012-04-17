using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represent the flags for expansion variables</summary>
    public enum Expansion : uint /* UInt32 */
    {
        None = 0,   //Restrict to BG1 XP limit?
        TotSC = 1,  //Restrict to TotSC XP limit?
        SoA = 2,    //Restrict to BG2 XP limit?

        /// <remarks>
        ///     GemRB suggests this is IWD (confirmed); Also IWD2;
        ///     IESDP says both xnewarea.2da processing to be done and xnewarea.2da processing complete
        /// </remarks>
        IWD = 3,    //Restrict to IWD XP limit?

        SoAMain = 4,    //Seems to be in a ToB game without ToB Active yet
        ToB = 5,    //ToB Active?
    }
}