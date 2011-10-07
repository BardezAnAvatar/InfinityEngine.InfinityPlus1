using System;
using System.Collections.Generic;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey
{
    /// <summary>This enumerator describes the language of the Infinity Engine game running</summary>
    public enum InfinityEngineLanguage : short /* Int16 */
    {
        English = 0,
        French,     // = 1
        Spanish,    // ?
        German      // = 3
    }

    /// <summary>This enumerator describes the flags assigned to a String Reference entry</summary>
    [Flags]
    public enum InfinityEngineStringReferenceFlags : short /* Int16 */
    {
        None = 0,
        Text = 1,
        Sound = 2,
        Tags = 4
    }
}