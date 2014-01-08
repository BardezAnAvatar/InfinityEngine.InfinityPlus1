using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's object block container</summary>
    /// <remarks>
    ///     The object block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG Series:
    ///     [Int] x 12, [String] x 1
    ///     IWD:
    ///     [Int] x 12, [Point-tangle] x 1, [String] x 1
    ///     IWD2:
    ///     [Int] x 13, [Point-tangle] x 1, [String] x 1, [Int] x 2
    ///     PST:
    ///     [Int] x 14, [Point-tangle] x 1, [String] x 1
    ///
    ///     Note, also, that the trigger's object can apparently have nothing following
    ///     its closing newline and function, but the action's objects require a closing newline
    /// </remarks>
    public class ObjectBlock
    {
        #region Fields
        /// <summary>Collection of characters that preceed the opening token</summary>
        protected IList<Char> preceedingOpenToken;

        /// <summary>Collection of characters that represent the opening token</summary>
        protected IList<Char> openToken;

        /// <summary>Collection of characters that preceed the closing token</summary>
        protected IList<Char> preceedingCloseToken;

        /// <summary>Collection of characters that represent the closing token</summary>
        protected IList<Char> closeToken;

        /// <summary>Collection of characters that make up the parameters for the object</summary>
        protected IList<Char> parameters;
        #endregion
    }
}