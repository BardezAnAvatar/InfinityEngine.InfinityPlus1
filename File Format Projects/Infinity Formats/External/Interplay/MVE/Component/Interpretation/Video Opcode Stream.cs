using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation
{
    /// <summary>Represents a 'Stream' of video data opcodes</summary>
    public class VideoOpcodeStream
    {
        #region Fields
        /// <summary>Represents the collection of Interpretable chunks and their opcodes</summary>
        /// <remarks>Not to be confused with MVE format chunks. These are searate.</remarks>
        protected List<MveInterpretableChunk> Chunks { get; set; }

        /// <summary>Represents the playback index or position within the 'Stream' of video frames</summary>
        protected Int32 positionPlayback;

        /// <summary>Represents the decoding index or position within the 'Stream' of video frames</summary>
        protected Int32 positionDecoding;

        /// <summary>Represents the current frame number in the decoding context</summary>
        protected Int32 currentFrameDecoding;

        /// <summary>Represents the current frame number in the decoding context</summary>
        protected Int32 currentFramePlayback;

        /// <summary>Flag indicating whether audio is playing. Set to false initially; video streams must activate audio.</summary>
        protected Boolean AudioPlaying { get; set; }

        /// <summary>Palette for data reference</summary>
        public Palette Palette { get; set; }

        /// <summary>Local event to raise to whatever processor that the audio stream has been started, and to start fetching audio data</summary>
        private event Action audioStreamStarted;

        /// <summary>Exposes the count of video frames in the chunk stream</summary>
        public Int32 FrameCount { get; set; }

        /// <summary>Exposes the start frame of audio in this stream</summary>
        public Int32 AudioStartFrame { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the current frame number in the decoding context</summary>
        public Int32 CurrentFrameDecoding
        {
            get { return this.currentFrameDecoding; }
        }

        /// <summary>Exposes the current frame number in the playback context</summary>
        public Int32 CurrentFramePlayback
        {
            get { return this.currentFramePlayback; }
        }
        #endregion


        #region Events
        /// <summary>Public-facing event indicating that the stream has read a start command</summary>
        public event Action AudioStreamStarted
        {
            add { this.audioStreamStarted += value; }
            remove { this.audioStreamStarted -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public VideoOpcodeStream()
        {
            this.Chunks = new List<MveInterpretableChunk>();
            this.FrameCount = 0;
            this.AudioStartFrame = -1;
            this.ResetStream();
        }
        #endregion


        #region Public Interface
        /// <summary>Gets the next pair of VideoData and SetDecodingMap opcode data to decode</summary>
        /// <returns>MveVideoFrame instance, or null if no valid pairs remaining</returns>
        public virtual MveVideoFrame FetchNextFrameDecode()
        {
            return this.FetchNextFrame(ref this.positionDecoding, ref this.currentFrameDecoding, false);
        }

        /// <summary>Gets the next pair of VideoData and SetDecodingMap opcode data to decode</summary>
        /// <returns>MveVideoFrame instance, or null if no valid pairs remaining</returns>
        public virtual MveVideoFrame FetchNextFrameOutput()
        {
            return this.FetchNextFrame(ref this.positionPlayback, ref this.currentFramePlayback, true);
        }

        /// <summary>Fetches the next MveVideoFrame from the stream</summary>
        /// <param name="position">position variable for the stream</param>
        /// <param name="currentFrame">The current frame counter to increment</param>
        /// <param name="isOutput">Flag indcating whether the means is for output, indicating how to treat frame numbers and palettes</param>
        /// <returns>MveVideoFrame instance, or null if no valid pairs remaining</returns>
        protected virtual MveVideoFrame FetchNextFrame(ref Int32 position, ref Int32 currentFrame, Boolean isOutput)
        {
            //get our variables
            MveVideoFrame frame = null;

            //do we do anything?
            while (position < this.Chunks.Count)
            {
                MveInterpretableChunk chunk = this.Chunks[position];

                if (chunk is MveVideoFrame)
                {
                    frame = chunk as MveVideoFrame;
                    ++currentFrame;
                }
                else if (chunk is MvePalette && !isOutput)
                    this.Palette = (chunk as MvePalette).Palette.GeneratePalette(null);
                else if (chunk is MveInitializeAudio && isOutput)
                {
                    if (this.audioStreamStarted != null)
                        this.audioStreamStarted();

                    this.AudioPlaying = true;
                }

                ++position;

                if (frame != null)
                    break;
            }

            return frame;
        }

        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            foreach (MveInterpretableChunk chunk in this.Chunks)
            {
                if (chunk is MveVideoFrame)
                {
                    (chunk as MveVideoFrame).DecodingMap.ReadData(input);
                    (chunk as MveVideoFrame).Data.ReadData(input);
                }
                else if (chunk is MvePalette)
                    (chunk as MvePalette).Palette.ReadData(input);
            }
        }

        /// <summary>Decodes from binary data the decoding maps of the video stream</summary>
        public virtual void DecodeVideoMaps()
        {
            foreach (MveInterpretableChunk chunk in this.Chunks)
                if (chunk is MveVideoFrame)
                    (chunk as MveVideoFrame).DecodingMap.DecodeMap();
        }

        /// <summary>Resets the video stream</summary>
        public virtual void ResetStream()
        {
            this.positionPlayback = 0;
            this.positionDecoding = 0;
            this.currentFrameDecoding = 0;
            this.currentFramePlayback = 0;
            this.Palette = null;
            this.AudioPlaying = false;
        }

        /// <summary>Adds a video chunk and its supporting opcodes to the stream</summary>
        /// <param name="chunk">Interpretable data chunk to add to the stream</param>
        public virtual void AddChunk(MveInterpretableChunk chunk)
        {
            this.Chunks.Add(chunk);

            if (chunk is MveVideoFrame)
                this.FrameCount++;
            else if (chunk is MveInitializeAudio && this.AudioStartFrame == -1)
                this.AudioStartFrame = this.FrameCount; //set it to the count, which is also effectively the next index
        }
        #endregion
    }
}