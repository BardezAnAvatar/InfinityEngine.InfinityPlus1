using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Exposure
{
    class Trigger_Exposure
    {


        /// <summary>Represents the ID of the trigger, which references the trigger identifiers file (TRIGGER.IDS)</summary>
        protected Int32 triggerId;

        /// <summary>First integer parameter to various trigger conditions</summary>
        protected Int64 integer1;

        /// <summary>
        ///     Flags parameter which appear at first view to only be negation; if bit 0 (value 1) is set,
        ///     the code becomes !trigger() rather than trigger()
        /// </summary>
        protected UInt32 flags;

        /// <summary>Second integer parameter to various trigger conditions</summary>
        protected Int64 integer2;

        /// <summary>Third integer parameter to various trigger conditions</summary>
        protected Int64 integer3;

        /// <summary>First string parameter</summary>
        protected String string1;

        /// <summary>Second string parameter</summary>
        protected String string2;
    }
}
