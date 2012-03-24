using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Represents a 'Stream' of video data opcodes</summary>
    public class VideoOpcodeStream
    {
        #region Fields
        /// <summary>Represents the stream of opcode data objects</summary>
        protected List<OpcodeData> Stream { get; set; }

        /// <summary>Represents the index or position within the 'Stream' of video opcodes</summary>
        protected Int32 Position { get; set; }

        /// <summary>Palette for data reference</summary>
        public Palette Palette { get; set; }

        /// <summary>Exposes the count of video data opcodes/frames in the stream</summary>
        public Int32 Count { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public VideoOpcodeStream()
        {
            this.Stream = new List<OpcodeData>();
            this.Position = 0;
            this.Count = 0;
            this.Palette = null;
        }
        #endregion


        #region Public Interface
        /// <summary>Adds an OpcodeData object to the end of the stream</summary>
        /// <param name="opcodeData">Object to add to the stream</param>
        public virtual void AddOpcodeData(OpcodeData opcodeData)
        {
            this.Stream.Add(opcodeData);
        }

        /// <summary>Gets the next pair of VideoData and SetDecodingMap opcode data</summary>
        /// <returns>MveVideoFrame instance, or null if no valid pairs remaining</returns>
        public virtual MveVideoFrame GetNextFrame()
        {
            //get our variables
            MveVideoFrame frame = null;
            VideoData vidData = null;
            SetDecodingMap decodingBlockMap = null;

            //loops through stream until we find both a decoding map and a video data
            while ( (this.Position < this.Stream.Count) && (vidData == null || decodingBlockMap == null) )
            {
                if (this.Stream[this.Position] is VideoData)
                {
                    vidData = this.Stream[this.Position] as VideoData;
                    ++this.Count;
                }
                else if (this.Stream[this.Position] is SetDecodingMap)
                    decodingBlockMap = this.Stream[this.Position] as SetDecodingMap;
                else if (this.Stream[this.Position] is SetPalette)
                    this.Palette = (this.Stream[this.Position] as SetPalette).GeneratePalette(this.Palette);

                ++this.Position;
            }

            //instantiate the MveVideoFrame
            if (!(vidData == null || decodingBlockMap == null))
                frame = new MveVideoFrame(decodingBlockMap, vidData);

            return frame;
        }

        /// <summary>Reads the data from the input stream into the storage opcodes indexed by the manager</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadData(Stream input)
        {
            foreach (OpcodeData opcode in this.Stream)
                opcode.ReadData(input);
        }

        /// <summary>Decodes from binary data the decoding maps of the video stream</summary>
        public virtual void DecodeVideoMaps()
        {
            foreach (OpcodeData opcode in this.Stream)
                if (opcode is SetDecodingMap)
                    (opcode as SetDecodingMap).DecodeMap();
        }

        /// <summary>Resets the video stream</summary>
        public virtual void ResetStream()
        {
            this.Position = 0;
            this.Palette = null;
        }
        #endregion
    }
}