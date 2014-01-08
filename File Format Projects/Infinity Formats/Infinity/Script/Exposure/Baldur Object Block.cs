using System;
using System.IO;

using Bardez.Projects.ReusableCode;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Baldur
{
    /// <summary>A BioWare script's object block container</summary>
    /// <remarks>
    ///     The object block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG Series:  [Int] x 12, [String] x 1
    /// </remarks>
    public class BaldurObjectBlock : ObjectBlock
    {
    }
}