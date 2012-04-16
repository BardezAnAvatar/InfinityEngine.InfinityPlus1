using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BlackIsleAnimation.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BlackIsleAnimation
{
    public class BlaskIsleAnimation1 : IInfinityFormat, IImageSet, IAnimation
    {
        #region Fields
        /// <summary>Represents the BAM file header</summary>
        public BamHeader Header { get; set; }

        /// <summary>Collection of frame entries</summary>
        public List<FrameEntry> FrameEntries { get; set; }

        /// <summary>Collection of animation entries</summary>
        public List<AnimationEntry> Animations { get; set; }

        /// <summary>The palette for all frames wthin this BAM</summary>
        public BamPaletteEntry Palette { get; set; }

        /// <summary>The lookup table of animation frame to actual frames</summary>
        public List<Int16> FrameLookupTable { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the count of frame indeces in this BAM by looping through animations</summary>
        public Int32 CountFrameIndeces
        {
            get
            {
                Int32 count = 0;
                foreach (AnimationEntry animation in this.Animations)
                    count += animation.FrameCount;

                return count;
            }
        }

        /// <summary>Exposes the minimum CenterX value found</summary>
        public Int32 MinCenterX
        {
            get
            {
                Int32 centerX = Int32.MaxValue;

                foreach (FrameEntry frame in this.FrameEntries)
                {
                    if (frame.CenterX < centerX)
                        centerX = frame.CenterX;
                }

                if (centerX == Int32.MaxValue)
                    centerX = 0;

                return centerX;
            }
        }

        /// <summary>Exposes the minimum CenterY value found</summary>
        public Int32 MinCenterY
        {
            get
            {
                Int32 centerY = Int32.MaxValue;

                foreach (FrameEntry frame in this.FrameEntries)
                {
                    if (frame.CenterY < centerY)
                        centerY = frame.CenterY;
                }

                if (centerY == Int32.MaxValue)
                    centerY = 0;

                return centerY;
            }
        }

        /// <summary>Exposes the maximum CenterX value found</summary>
        public Int32 MaxCenterX
        {
            get
            {
                Int32 centerX = Int32.MinValue;

                foreach (FrameEntry frame in this.FrameEntries)
                {
                    if (frame.CenterX > centerX)
                        centerX = frame.CenterX;
                }

                if (centerX == Int32.MinValue)
                    centerX = 0;

                return centerX;
            }
        }

        /// <summary>Exposes the maximum CenterY value found</summary>
        public Int32 MaxCenterY
        {
            get
            {
                Int32 centerY = Int32.MinValue;

                foreach (FrameEntry frame in this.FrameEntries)
                {
                    if (frame.CenterY > centerY)
                        centerY = frame.CenterY;
                }

                if (centerY == Int32.MinValue)
                    centerY = 0;

                return centerY;
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new BamHeader();
            this.FrameEntries = new List<FrameEntry>();
            this.Animations = new List<AnimationEntry>();
            this.Palette = new BamPaletteEntry();
            this.FrameLookupTable = new List<Int16>();
        }
        #endregion


        #region IO method implemetations
        #region Read Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new BamHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);
            this.ReadFrameEntries(input);
            this.ReadAnimationEntries(input);
            this.ReadPalette(input);
            this.ReadFrameLookupTable(input);
            this.ReadFrameData(input);
        }

        /// <summary>Reads frame entries from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadFrameEntries(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetFrameEntries);

            for (Int32 i = 0; i < this.Header.CountFrames; ++i)
            {
                FrameEntry frame = new FrameEntry();
                frame.Read(input);
                this.FrameEntries.Add(frame);
            }
        }

        /// <summary>Reads animation entries from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadAnimationEntries(Stream input)
        {
            //Format description states that it immediately follows
            //ReusableIO.SeekIfAble(input, this.Header.OffsetAnimationEntries);

            for (Int32 i = 0; i < this.Header.CountAnimations; ++i)
            {
                AnimationEntry animation = new AnimationEntry();
                animation.Read(input);
                this.Animations.Add(animation);
            }
        }

        /// <summary>Reads the BAM palette from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadPalette(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetPalette);
            this.Palette.Read(input);
        }

        /// <summary>Reads the frame indeces from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadFrameLookupTable(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetFrameLookupTable);

            Byte[] data = ReusableIO.BinaryRead(input, this.CountFrameIndeces * 2);

            for (Int32 i = 0; i < data.Length; i += 2)
                this.FrameLookupTable.Add(ReusableIO.ReadInt16FromArray(data, i));
        }

        /// <summary>Reads the frame data from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadFrameData(Stream input)
        {
            Int32 transparency = this.Palette.TransparencyIndex;

            for (Int32 index = 0; index < this.FrameEntries.Count; ++index)
            {
                FrameEntry frame = this.FrameEntries[index];
                ReusableIO.SeekIfAble(input, frame.OffsetData);
                FrameData data = new FrameData(frame, transparency, this.Header.RlePaletteIndex);
                data.Read(input);
                frame.Data = data;
            }
        }
        #endregion


        #region Write Methods
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.Header.Write(output);
            this.WriteFrameEntries(output);
            this.WriteAnimationEntries(output);
            this.WritePalette(output);
            this.WriteFrameLookupTable(output);
            this.WriteFrameData(output);
        }

        /// <summary>Writes the frame entries to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteFrameEntries(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetFrameEntries);

            foreach (FrameEntry frame in this.FrameEntries)
                frame.Write(output);
        }

        /// <summary>Writes the animation entries to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteAnimationEntries(Stream output)
        {
            foreach (AnimationEntry animation in this.Animations)
                animation.Write(output);
        }

        /// <summary>Writes the BAM palette to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WritePalette(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetPalette);
            this.Palette.Write(output);
        }

        /// <summary>Writes the frame indeces to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteFrameLookupTable(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetFrameLookupTable);
            foreach (Int16 offset in this.FrameLookupTable)
                ReusableIO.WriteInt16ToStream(offset, output);
        }

        /// <summary>Writes the frame data to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteFrameData(Stream output)
        {
            foreach (FrameEntry frame in this.FrameEntries)
            {
                ReusableIO.SeekIfAble(output, frame.OffsetData);
                frame.Data.Write(output);
            }
        }
        #endregion
        #endregion


        #region IImageSet Method(s)
        /// <summary>Returns the frame with the associated index in the tileset</summary>
        /// <param name="index">Index of the frame to retrieve</param>
        /// <returns>The frane at the associated index</returns>
        public Frame GetFrame(Int32 index)
        {
            FrameEntry frame = this.FrameEntries[index];
            Byte[] data = this.GetExpandedData(frame);
            
            Frame getFrame = new Frame();
            getFrame.Pixels = new PixelData(data, ScanLineOrder.TopDown, PixelFormat.RGBA_B8G8R8A8, frame.Height, frame.Width, 0, 0, 32);
            getFrame.OriginX = this.FrameEntries[index].RenderOriginX;
            getFrame.OriginY = this.FrameEntries[index].RenderOriginY;

            return getFrame;
        }

        /// <summary>Expands the tile data from the palette indexed color to a fully BGRA pixel data array</summary>
        /// <returns>A fully decompressed pixel data Byte array</returns>
        protected Byte[] GetExpandedData(FrameEntry frame)
        {
            BamPaletteEntry palette = this.Palette;
            TransparencyColorEntry[] tces = palette.ColorsAlpha;
            FrameData fdata = frame.Data;

            Byte[] pixels = new Byte[4 * frame.Width * frame.Height];

            for (Int32 dataIndex = 0; dataIndex < fdata.Data.Length; ++dataIndex)
            {
                //pre-multiply; that means 0 if transparent (already set!) or else normal + 255 alpha
                if (tces[fdata.Data[dataIndex]].Alpha != Byte.MinValue)  //Index 0 is transparent, premultiplied; everything else is opaque
                {
                    ColorEntry ce = palette.Colors[fdata.Data[dataIndex]];

                    Int32 pixelOffset = dataIndex * 4;
                    pixels[pixelOffset] = ce.Blue;
                    pixels[pixelOffset + 1] = ce.Green;
                    pixels[pixelOffset + 2] = ce.Red;
                    pixels[pixelOffset + 3] = tces[fdata.Data[dataIndex]].Alpha;
                }
            }

            return pixels;
        }

        /// <summary>IImageSet property that exposes the frame count</summary>
        public Int64 FrameCount
        {
            get { return this.FrameEntries.Count; }
        }
        #endregion


        #region IAnimation Methods
        /// <summary>Returns an IList containing an IList of indeces meant to be used in conjunction with <see cref="IImageSet.GetFrame(Int32)"/></summary>
        /// <returns>An IList with items inside being IList of Int32 indeces to the frames returned from <see cref="IImageSet.GetFrame(Int32)"/> using the same index key</returns>
        public IList<IList<Int32>> GetFrameAnimations()
        {
            IList<IList<Int32>> animations = new List<IList<Int32>>();

            foreach (AnimationEntry animation in this.Animations)
            {
                List<Int32> animationFrames = new List<Int32>();
                for (Int32 index = animation.StartFrame; index < (animation.StartFrame + animation.FrameCount); ++index)
                    animationFrames.Add(this.FrameLookupTable[index]);

                animations.Add(animationFrames);
            }

            return animations;
        }
        #endregion
    }
}