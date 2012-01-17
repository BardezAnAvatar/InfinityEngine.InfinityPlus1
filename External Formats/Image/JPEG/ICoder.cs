using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents an interface for Huffman decode and Arithmetic decode</summary>
    public interface ICoder
    {
        /// <summary>Gets a single Byte from the decoder</summary>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>A Decoded Byte</returns>
        Byte Decode(ref Boolean halt);

        /// <summary>Decodes DC coefficient of a quantized 8x8 64 value block from the decoder</summary>
        /// <param name="pred">Prediction of the DC</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The block's DC coefficient</returns>
        Int32 DecodeSequentialDC(ref Int32 pred, ref Boolean halt);

        /// <summary>Decodes AC coefficient of a quantized 8x8 64 value block from the decoder</summary>
        /// <param name="emptyBlock">64-value array representing the block. index 0 has the DC coefficient</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        void DecodeSequentialACs(Int32[] emptyBlock, ref Boolean halt);

        /// <summary>Runs the Decode procedures on a DC coefficient</summary>
        /// <param name="pred">Prediction of the DC</param>
        /// <param name="successiveApproximation">Number of bits to shift</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The DC coefficient differential</returns>
        Int32 DecodeFirstOrderDC(ref Int32 pred, Int32 successiveApproximation, ref Boolean halt);

        /// <summary>Runs the Decode procedures on a DC coefficient</summary>
        /// <param name="existing">Existing DC value</param>
        /// <param name="successiveApproximation">Number of bits to shift</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The DC coefficient differential</returns>
        Int32 DecodeSuccessiveProgressiveDC(Int32 existing, Int32 successiveApproximation, ref Boolean halt);

        /// <summary>Decodes AC coefficients from the Huffman bit stream</summary>
        /// <param name="existingBlock">Initialized 64-byte coefficient block</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <param name="startSelector">First coefficient to read</param>
        /// <param name="endSelector">Last coefficient to read</param>
        /// <param name="successiveApproximation">Current shift to perform during successive approximation</param>
        void DecodeFirstOrderProgressiveACs(Int32[] existingBlock, ref Boolean halt, Int32 startSelector, Int32 endSelector, Int32 successiveApproximation);

        /// <summary>Decodes AC coefficients' next order bits from the Huffman bit stream</summary>
        /// <param name="existingBlock">Initialized 64-byte coefficient block</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <param name="startSelector">First coefficient to read</param>
        /// <param name="endSelector">Last coefficient to read</param>
        /// <param name="successiveApproximation">Current shift to perform during successive approximation</param>
        /// <returns>The count of 0-runs following this block</returns>
        void DecodeSuccessiveProgressiveACs(Int32[] existingBlock, ref Boolean halt, Int32 startSelector, Int32 endSelector, Int32 successiveApproximation);

        /// <summary>Rsets the coding stream buffers, after a seek has been performed elsewhere in the application.</summary>
        void ResetCodingStream();
    }
}