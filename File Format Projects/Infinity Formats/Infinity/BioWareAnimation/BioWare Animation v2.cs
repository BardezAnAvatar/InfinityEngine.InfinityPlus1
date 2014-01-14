using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.BamMos;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation
{
    /// <summary>Wrapper class for a BAM v2 file</summary>
    public class BioWareAnimation_v2 : IInfinityFormat, IImageSet, IAnimation
    {
        #region Fields
        /// <summary>Represents the BAM file header</summary>
        public BamHeader_v2 Header { get; set; }

        /// <summary>Collection of frame entries</summary>
        public List<FrameEntry_v2> FrameEntries { get; set; }

        /// <summary>Collection of animation entries</summary>
        public List<AnimationEntry> Animations { get; set; }

        /// <summary>Collection of the BAM file's data blocks</summary>
        public List<DataBlock> DataBlocks { get; set; }

        /// <summary>Collection of related files to this BAM, keyed by the data blocks' RelatedFileName</summary>
        public Dictionary<String, PowerVrCompressed> RelatedFiles { get; set; }
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

                foreach (FrameEntry_v2 frame in this.FrameEntries)
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

                foreach (FrameEntry_v2 frame in this.FrameEntries)
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

                foreach (FrameEntry_v2 frame in this.FrameEntries)
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

                foreach (FrameEntry_v2 frame in this.FrameEntries)
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
            this.Header = new BamHeader_v2();
            this.FrameEntries = new List<FrameEntry_v2>();
            this.Animations = new List<AnimationEntry>();
            this.DataBlocks = new List<DataBlock>();
            this.RelatedFiles = null;
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
                this.Header = new BamHeader_v2();
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
            this.ReadDataBlocks(input);
        }

        /// <summary>Reads frame entries from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadFrameEntries(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetFrameEntries);

            for (Int32 i = 0; i < this.Header.CountFrames; ++i)
            {
                FrameEntry_v2 frame = new FrameEntry_v2();
                frame.Read(input);
                this.FrameEntries.Add(frame);
            }
        }

        /// <summary>Reads animation entries from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadAnimationEntries(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetAnimationEntries);
            for (Int32 i = 0; i < this.Header.CountAnimations; ++i)
            {
                AnimationEntry animation = new AnimationEntry();
                animation.Read(input);
                this.Animations.Add(animation);
            }
        }

        /// <summary>Reads data blocks form the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadDataBlocks(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.Header.OffsetDataBlocks);
            for (Int32 i = 0; i < this.Header.CountDataBlocks; ++i)
            {
                DataBlock block = new DataBlock();
                block.Read(input);
                this.DataBlocks.Add(block);
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
            this.WriteDataBlocks(output);
        }

        /// <summary>Writes the frame entries to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteFrameEntries(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetFrameEntries);

            foreach (FrameEntry_v2 frame in this.FrameEntries)
                frame.Write(output);
        }

        /// <summary>Writes the animation entries to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteAnimationEntries(Stream output)
        {
            foreach (AnimationEntry animation in this.Animations)
                animation.Write(output);
        }

        /// <summary>Writes the BAM data blocks to the output Stream</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void WriteDataBlocks(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.Header.OffsetDataBlocks);
            foreach (DataBlock block in this.DataBlocks)
                block.Write(output);
        }
        #endregion
        #endregion


        #region IImageSet Method(s)
        /// <summary>Returns the frame with the associated index in the tileset</summary>
        /// <param name="index">Index of the frame to retrieve</param>
        /// <returns>The frane at the associated index</returns>
        public IMultimediaImageFrame GetFrame(Int32 index)
        {
            FrameEntry_v2 frame = this.FrameEntries[index];

            //TODO: compose an image together from data blocks

            Byte[] data = null;

            Int64 oX = this.FrameEntries[index].RenderOriginX;
            Int64 oY = this.FrameEntries[index].RenderOriginY;
            PixelData pd = new PixelData(data, ScanLineOrder.TopDown, PixelFormat.RGBA_B8G8R8A8, frame.Height, frame.Width, 0, 0, 32, oX, oY);
            IMultimediaImageFrame getFrame = new BasicImageFrame(pd);

            return getFrame;
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
                    animationFrames.Add(index);

                animations.Add(animationFrames);
            }

            return animations;
        }
        #endregion
    }
}