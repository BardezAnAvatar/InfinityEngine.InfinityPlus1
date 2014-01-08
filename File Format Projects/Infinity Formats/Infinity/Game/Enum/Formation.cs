using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Enumeration for the formations</summary>
    public enum Formation : short /* Int16 */
    {
        //TODO: run the game and come up with better/supplementary names

        Follow      = 0,
        T           = 1,
        Gather      = 2,
        FourAndTwo  = 3,
        ThreeByTwo  = 4,
        Protect     = 5,
        TwoByThree  = 6,
        Rank        = 7,
        V           = 8,
        Wedge       = 9,
        S           = 10,
        Line        = 11,
        None        = 12,
    }
}
