using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components
{
    /// <summary>
    ///     This class is an abstract entity for shared members between the
    ///     tileset resource entry and the regular resource entry.
    /// </summary>
    public abstract class Biff1FileEntry : IInfinityFormat
    {
        #region Fields
        /// <summary>
        ///     The resource1 locator to match to the KEY file entry.
        ///     Only the subtype is matched upon, depending on the resource1 type.
        /// </summary>
        public ResourceLocator1 ResourceLocator { get; set; }

        /// <summary>Offset within the BIFF (from teh start of the file) to the start of the resource1 data.</summary>
        public Int32 OffsetResource { get; set; }

        /// <summary>Short indicating the type of the resource1</summary>
        public ResourceType TypeResource { get; set; }

        /// <summary>Unknown data field assumed to be binary padding to wrap the structure to 4-byte size</summary>
        protected Int16 UnknownPadding { get; set; }
	    #endregion


        #region Properties
        /// <summary>This property exposes the length of the resource1 data entry.</summary>
        public abstract UInt32 SizeResource { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.ResourceLocator = new ResourceLocator1();
        }
        #endregion


        #region IInfinityFormat I/O methods
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream object from which to read from</param>
        public abstract void ReadBody(Stream input);

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="input">Stream object to write to</param>
        public abstract void Write(Stream output);
        #endregion
    }
}