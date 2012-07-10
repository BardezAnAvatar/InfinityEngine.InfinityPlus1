using System;
using System.IO;

namespace Bardez.Projects.Utility.DataContainers
{
    /// <summary>This interface is a base type for a Byte collection data container, either a Stream or an Array&lt;Byte&gt; or an IntPtr.</summary>
    public interface IDataContainer : IDisposable
    {
        #region Properties
        /// <summary>Exposes the length of the contained data</summary>
        Int32 Length { get; }
        #endregion


        #region Data Accessors
        /// <summary>Exposes an array of the data</summary>
        /// <returns>A Byte array of the data being represented</returns>
        /// <remarks>This method may consume underlying data</remarks>
        Byte[] GetArray();

        /// <summary>Exposes a Stream representing the underlying data</summary>
        /// <returns>A <see cref="Stream"/> object representing the underlying data</returns>
        Stream GetStream();

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <param name="length">Length to read</param>
        /// <returns>Small Byte[] of data read</returns>
        Byte[] ReadBytes(Int32 offset, Int32 length);

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <returns>Small Byte[] of data read</returns>
        Byte ReadByte(Int32 offset);
        #endregion
    }
}