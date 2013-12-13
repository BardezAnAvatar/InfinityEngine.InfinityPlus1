using System;
using System.Collections.Generic;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkTable
{
    /// <summary>This enumerator describes the language of the Infinity Engine game running</summary>
    public enum InfinityEngineLanguage : short /* Int16 */
    {
        Unspecified = 0,
        English = 0,
        French,         // = 1
        Spanish,        // = 2
        German,         // = 3
        Italian,        // = 4

        //Italian values are all sorts of weird.
        /*
        Italian = 17004,    //BG Italian M, F   (15) 1111-0011-1110-0100 (0)
        Italian = 22182,    //BG2 Italian M     (15) 0100-0010-0110-1100 (0)
        Italian = 28604,    //BG2 Italian F     (15) 0101-0110-1010-0110 (0)
        Italian = 0xF3E4,   //IWD Dialog M      (15) 0110-1111-1011-1100 (0)
        Italian = 0,        //IWD Dialog F      (15) 0000-0000-0000-0000 (0)

        ItalianMask = 17004 & 22182 & 28604 & 0xF3E4,
        */


        MaskAll = -1,
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