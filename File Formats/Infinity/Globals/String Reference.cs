using System;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Globals
{
    /// <summary>Represents an index into the dialog.tlk file, referencing a string</summary>
    public class StringReference
    {
        /// <summary>Index of the dialog.tlk string</summary>
        protected Int32 stringReferenceIndex;

        /// <summary>Index of the dialog.tlk string</summary>
        public Int32 StringReferenceIndex
        {
            get { return this.stringReferenceIndex; }
            set { this.stringReferenceIndex = value; }
        }
    }
}