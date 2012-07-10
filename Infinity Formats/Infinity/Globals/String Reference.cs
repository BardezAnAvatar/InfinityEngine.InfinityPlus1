using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals
{
    /// <summary>Represents an index into the dialog.tlk file, referencing a string</summary>
    public class StringReference
    {
        #region Constants
        /// <summary>Constant value mask for the tlk file string reference</summary>
        protected const Int32 StrRefMask = 0x00FFFFFF;
        #endregion


        #region Fields
        /// <summary>Index of the dialog.tlk string</summary>
        protected Int32 stringReferenceIndex;
        #endregion


        #region Properties
        /// <summary>Index of the dialog.tlk string</summary>
        public Int32 StringReferenceIndex
        {
            get { return this.stringReferenceIndex; }
            set { this.stringReferenceIndex = value; }
        }

        /// <summary>String ref within a given TLK file</summary>
        public Int32 TlkStringIndex
        {
            get { return this.stringReferenceIndex & StringReference.StrRefMask; }
            set
            {
                Int32 tlk = (this.stringReferenceIndex & ~StringReference.StrRefMask);
                Int32 strRef = (value & StringReference.StrRefMask);
                this.stringReferenceIndex = tlk | strRef;
            }
        }

        /// <summary>Index of a specified TLK file</summary>
        public Int32 TlkFileReference
        {
            get { return (this.stringReferenceIndex & ~StringReference.StrRefMask) >> 24; }
            set
            {
                Int32 strRef = (this.stringReferenceIndex & StringReference.StrRefMask);
                Int32 tlk = (value & 0xFF) << 24;
                this.stringReferenceIndex = tlk | strRef;
            }
        }
        #endregion
    }
}