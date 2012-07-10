using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bardez.Projects.InfinityPlus1.Utility.DataContainers
{
    /// <summary>Wrapping class for an unmanaged IntPtr data source</summary>
    /// <remarks>Does *NOT* own the underlying data.</remarks>
    public class PointerDataContainer : IDataContainer
    {
        #region Fields
        /// <summary>The underlying unmanaged pointer to data</summary>
        protected IntPtr Pointer { get; set; }

        /// <summary>Exposes the length of the contained data</summary>
        public Int32 Length { get; set; }
        #endregion


        #region Construction

        #endregion


        #region Data Accessors
        /// <summary>Exposes an array of the data</summary>
        /// <returns>A Byte array of the data being represented</returns>
        /// <remarks>This method may consume underlying data</remarks>
        Byte[] GetArray();

        /// <summary>Exposes a Stream representing the underlying data</summary>
        /// <returns>A <see cref="Stream"/> object representing the underlying data</returns>
        Stream GetStream();

        /// <summary>Exposes a pointer to the head of the underling data</summary>
        /// <param name="length">Length of the underlying data, exposed to the caller</param>
        /// <returns>a pointer to the head of the underlying data</returns>
        /// <remarks>This method may consume underlying data</remarks>
        IntPtr GetPointer(out Int32 length);
        #endregion
    }
}