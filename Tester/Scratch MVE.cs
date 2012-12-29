using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.Direct2D;
using Direct2D = Bardez.Projects.DirectX.Direct2D;
using ExternalPixelEnums = Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Output.Visual;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    /// <summary>Scratch Windows Form to quickly test code</summary>
    public partial class ScratchMve : Form
    {
        #region Fields
        protected MveManager MVE { get; set; }
        protected Int32 frameKey;
        protected Int32 frameCountNumber;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ScratchMve()
        {
            this.InitializeComponent();
            this.frameKey = -1;
            this.frameCountNumber = 0;
        }
        #endregion

        /// <summary>Generic control action that will perform an Invoke action (such as a setter) on a control</summary>
        /// <param name="c">Control to query for Invoke</param>
        /// <param name="action">Action to perform</param>
        protected virtual void ControlAction(Control c, Action action)
        {
            if (c.InvokeRequired)
                c.Invoke(action);
            else
                action();
        }

        /// <summary>Form Load event handler</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        private void Scratch_Load(Object sender, EventArgs e)
        {
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        //private void JpegLoad()
        //{
        //    String bmpPath = ConfigurationManager.AppSettings["Test.JPEG.Path"];
        //    JpegJfifInterchange jpeg;
        //    using (FileStream fs = new FileStream(bmpPath, FileMode.Open, FileAccess.Read))
        //        jpeg = JpegJfifParser.ParseJpegFromStream(fs, new ResizeDelegateInteger(NearestNeighborIntegerSpace.NearestNeighborResampleInteger));

        //    Application.DoEvents();

        //    jpeg.Decode();
        //    Frame frame = jpeg.GetFrame();
        //    Byte[] data = frame.Pixels.GetPixelData(ExternalPixelEnums.PixelFormat.RGB_B8G8R8, ScanLineOrder.TopDown, 0, 0);

        //    Int32 key = this.direct2dRenderControl1.AddFrameResource(frame);
        //    this.direct2dRenderControl1.SetRenderFrame(key);
        //}

        protected virtual void ReadMveMovies()
        {
            String mveConfig = "Test.MVE.Path";
            IList<String> paths = ConfigurationHandlerMulti.GetSettingValues(mveConfig);

            Dictionary<String, MveChunkOpcodeIndexer> MveChunksWithOpcodes = new Dictionary<string,MveChunkOpcodeIndexer>();

            foreach (String path in paths)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    MveChunkOpcodeIndexer mve = new MveChunkOpcodeIndexer();
                    mve.Read(fs);
                    MveChunksWithOpcodes.Add(path, mve);
                }
            }

            //app events
            Application.DoEvents();

            //read opcode data
            foreach (KeyValuePair<String, MveChunkOpcodeIndexer> mvePair in MveChunksWithOpcodes)
            {
                using (FileStream fs = new FileStream(mvePair.Key, FileMode.Open, FileAccess.Read))
                    mvePair.Value.ReadChunkOpcodes(fs);
            }

            Dictionary<String, MveManager> management = new Dictionary<String, MveManager>();
            foreach (KeyValuePair<String, MveChunkOpcodeIndexer> mvePair in MveChunksWithOpcodes)
            {
                MveManager mveManager = new MveManager(MveChunksWithOpcodes[mvePair.Key]);
                mveManager.CollectOpcodeIndex();
                using (FileStream fs = new FileStream(mvePair.Key, FileMode.Open, FileAccess.Read))
                    mveManager.ReadData(fs);


                mveManager.InitializeCoders();

                //informational
                mveManager.DecodeVideoMaps();

                management.Add(mvePair.Key, mveManager);
            }

            //this.MveVideoLinqReporting(management);
        }

        protected virtual void ReadMveMovie()
        {
            String mveConfig = "Test.MVE.Path";
            String path = ConfigurationHandlerMulti.GetSettingValue(mveConfig);

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                MveChunkOpcodeIndexer mve = new MveChunkOpcodeIndexer();
                mve.Read(fs);
                mve.ReadChunkOpcodes(fs);
                this.MVE = new MveManager(mve);
                this.MVE.CollectOpcodeIndex();
                this.MVE.ReadData(fs);
                this.MVE.DecodeVideoMaps();
            }

            this.frameKey = -1;
            this.MVE.InitializeCoders();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReadMveMovie();
            this.pnlButtons.Visible = true;

            //reporting
            //this.ReadMveMovies();
        }

        private void btnNextFrame_Click(object sender, EventArgs e)
        {
            this.FetchNextVideoFrame();
        }

        protected virtual void FetchNextVideoFrame()
        {
            if (this.MVE != null)
            {
                IMultimediaImageFrame frame = this.MVE.GetNextFrame();
                this.ProcessNextVideoFrame(frame);
            }
        }

        protected virtual void ProcessNextVideoFrame(IMultimediaImageFrame frame)
        {
            if (this.MVE != null)
            {
                if (frame == null)
                    this.StopPlayback();
                else
                {
                    //get the new one
                    this.frameKey = this.direct2dRenderControl1.AddFrameResource(frame);

                    //render
                    this.direct2dRenderControl1.SetRenderFrameAndRender(this.frameKey, true);

                    //display Frame number
                    ++this.frameCountNumber;

                    //display
                    this.ControlAction(this.labelFrameNumDisplay, () => { this.labelFrameNumDisplay.Text = this.frameCountNumber.ToString(); });
                }
            }
        }

        protected virtual void StopPlayback()
        {
            this.MVE.StopVideoPlayback();
            this.MVE.PlayFrame -= new Action<IMultimediaImageFrame>(this.ProcessNextVideoFrame);
        }

        protected virtual void btnPlay_Click(Object sender, EventArgs e)
        {
            this.MVE.PlayFrame += new Action<IMultimediaImageFrame>(this.ProcessNextVideoFrame);
            this.MVE.StartVideoPlayback();
        }

        protected virtual void TimerExpired(IMultimediaImageFrame frame)
        {
            this.FetchNextVideoFrame();
        }

        protected virtual void btnReset_Click(Object sender, EventArgs e)
        {
            this.MVE.ResetVideo();
            this.frameCountNumber = 0;
        }

        protected virtual void PlayWithLinq(Dictionary<String, MveChunkOpcodeIndexer> mveChunksWithOpcodes)
        {
            //linq!
            var y = from pair in mveChunksWithOpcodes
                    select new { Path = pair.Key, Mve = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes) };

            var b = from pair in y
                    from m in pair.Mve
                    select new { Path = pair.Path, OpCode = m.OpCode };

            var r = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.CreateTimer
                    group code by code.Path into g
                    where g.Count() > 1
                    select g.Key;

            var h = from pair in y
                    from m in pair.Mve
                    where m.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.CreateTimer
                    select new { Path = pair.Path, Opcode = m };

            //var z = MveChunksWithOpcodes.SelectMany(mve => mve.ChunkIndexCollection).SelectMany(chunk => chunk.Opcodes);
            //var q = from opcode in z
            //        group opcode by opcode.OpCode into v
            //        orderby v.Count<Opcode>()
            //        select new { OpCode = v.Key, Count = v.Count<Opcode>(), Value = (Byte)v.Key };

            var d = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.SetDecodingMap
                    select new { Path = code.Path, Opcode = code.OpCode };

            var v = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.VideoData
                    select new { Path = code.Path, Opcode = code.OpCode };

            var a = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.AudioSilence || code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.AudioSamples
                    select new { Path = code.Path, Opcode = code.OpCode };

            var count = from pair in mveChunksWithOpcodes
                        select new
                        {
                            Path = pair.Key,
                            FrameRate = (pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.CreateTimer).Last().Data as CreateTimer),
                            AudioBuffer = (pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeAudioBuffers).Last().Data as InitializeAudioBuffers),
                            VideoBuffer = (pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeVideoBuffers).Last().Data as InitializeVideoBuffers),
                            DecodeCount = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.SetDecodingMap).Count(),
                            VideoDataCount = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.VideoData).Count(),
                            AudioBufferCount = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeAudioBuffers).Count(),
                            VideoBufferCount = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes).Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeVideoBuffers).Count(),
                            SampleCount = pair.Value.ChunkIndexCollection.SelectMany(chunk => chunk.Opcodes)
                                .Where(o => o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.AudioSamples || o.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.AudioSilence)
                                .Where(samples => ((samples.Data as AudioStream).StreamChannel & FileFormats.External.Interplay.MVE.Enum.AudioStreamChannels.Channel00) == FileFormats.External.Interplay.MVE.Enum.AudioStreamChannels.Channel00)
                                .Sum(samples => (samples.Data as AudioStream).UncompressedSampleDataLength)
                        };




            var s = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeVideoBuffers
                    group code by code.Path into g
                    where g.Count() > 1
                    select g.Key;

            var w = from pair in y
                    from m in pair.Mve
                    where m.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeVideoBuffers
                    select new { Path = pair.Path, Opcode = m };


            var f = from code in b
                    where code.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeAudioBuffers
                    group code by code.Path into g
                    where g.Count() > 1
                    select g.Key;

            var u = from pair in y
                    from m in pair.Mve
                    where m.OpCode == FileFormats.External.Interplay.MVE.Enum.OpcodeTypes.InitializeAudioBuffers
                    select new { Path = pair.Path, Opcode = m };
        }

        protected virtual void btnStop_Click(object sender, EventArgs e)
        {
            this.StopPlayback();
        }
        
        /*
        protected virtual void MveVideoLinqReporting(Dictionary<String, MveManager> management)
        {
            
            //decode
            //var decodeOpcodes = from manager in management
            //                    from opcode in manager.Value.VideoStream.Stream
            //                    where opcode is SetDecodingMap
            //                    select new { Path = manager.Key, Code = opcode as SetDecodingMap };

            ////select file, block encoding for counts
            //var decodeBlockMaps = from decode in decodeOpcodes
            //                      from blockEncoding in decode.Code.BlockEncoding
            //                      select new { Path = decode.Path, Encoding = blockEncoding };

            ////select
            //var decodeMapsCount = from encoding in decodeBlockMaps
            //                      group encoding by encoding into groupedEncoding
            //                      select new { Path = groupedEncoding.Key.Path, Encoding = groupedEncoding.Key.Encoding, Count = groupedEncoding.Count() };
            

            var decodeOpcodes = from manager in management
                                from opcode in manager.Value.VideoStream.Stream
                                where opcode is SetDecodingMap
                                select new { Path = manager.Key, Code = opcode as SetDecodingMap };

            var decodeMapsCount = from decode in decodeOpcodes
                                  from blockEncoding in decode.Code.BlockEncoding
                                  group decode by new { decode.Path, Encoding = blockEncoding } into groupedEncoding
                                  select new { Path = groupedEncoding.Key.Path, groupedEncoding.Key.Encoding, Count = groupedEncoding.Count() };
        
            List<String> toStrings = new List<String>();
            foreach (var v in decodeMapsCount)
                toStrings.Add(v.ToString());
        }
        */

        /*
        private void MveOpcodeLinqReporting(Dictionary<String, MveManager> management)
        {
            //collect opcode versions
            var opcodeCollections = from pair in management
                                    select pair.Value.MveIndex.ChunkIndexCollection.SelectMany(opcode => opcode.Opcodes);

            var opcodes = opcodeCollections.SelectMany(opcode => opcode);

            var results =   from opcode in opcodes
                            group opcode by new { opcode.OpCode, opcode.Version } into report
                            select new { Opcode = report.Key.OpCode, Value = (Int32)report.Key.OpCode, HexValue = String.Format("0x{0:X2}", ((Int32)report.Key.OpCode)), Version = report.Key.Version, Count = report.Count() };

            //collect opcodes by file
            var fileCodes = from pair in management
                            select new { File = pair.Key, Opcodes = pair.Value.MveIndex.ChunkIndexCollection.SelectMany(op => op.Opcodes) };

            var fileReport = from file in fileCodes
                             from code in file.Opcodes
                             group file by new { File = file.File, Opcode = code.OpCode, Version = code.Version } into bang
                             select new { bang.Key.File, bang.Key.Opcode, Value = (Int32)bang.Key.Opcode, HexValue = String.Format("0x{0:X2}", ((Int32)bang.Key.Opcode)), bang.Key.Version, Count = bang.Count() };

            //video 3.0 info
            //var videoOpcodes = from file in fileCodes
            //                   from code in file.Opcodes
            //                   where code.OpCode == OpcodeTypes.VideoData
            //                   select new
            //                   {
            //                       file.File,
            //                       code.OpCode,
            //                       //Value = (Int32)code.OpCode,
            //                       //HexValue = String.Format("0x{0:X2}", ((Int32)code.OpCode)),
            //                       code.Version,
            //                       Data = (code.Data as VideoData),
            //                       DataLen = (code.Data as VideoData).Data.Length,
            //                       Byte00 = (code.Data as VideoData).Data[0],
            //                       Byte01 = (code.Data as VideoData).Data[1],
            //                       Byte02 = (code.Data as VideoData).Data[2],
            //                       Byte03 = (code.Data as VideoData).Data[3],
            //                       Byte04 = (code.Data as VideoData).Data[4],
            //                       Byte05 = (code.Data as VideoData).Data[5],
            //                       Byte06 = (code.Data as VideoData).Data[6],
            //                       Byte07 = (code.Data as VideoData).Data[7],
            //                       Byte08 = (code.Data as VideoData).Data[8],
            //                       Byte09 = (code.Data as VideoData).Data[9],
            //                       Byte10 = (code.Data as VideoData).Data[10],
            //                       Byte11 = (code.Data as VideoData).Data[11],
            //                       Byte12 = (code.Data as VideoData).Data[12],
            //                       Byte13 = (code.Data as VideoData).Data[13],
            //                       Byte14 = ((code.Data as VideoData).Data.Length > 14) ? (Byte?)((code.Data as VideoData).Data[14]) : null,
            //                       Byte15 = ((code.Data as VideoData).Data.Length > 15) ? (Byte?)((code.Data as VideoData).Data[15]) : null,
            //                       Byte16 = ((code.Data as VideoData).Data.Length > 16) ? (Byte?)((code.Data as VideoData).Data[16]) : null,
            //                       Byte17 = ((code.Data as VideoData).Data.Length > 17) ? (Byte?)((code.Data as VideoData).Data[17]) : null,
            //                       Byte18 = ((code.Data as VideoData).Data.Length > 18) ? (Byte?)((code.Data as VideoData).Data[18]) : null,
            //                       Byte19 = ((code.Data as VideoData).Data.Length > 19) ? (Byte?)((code.Data as VideoData).Data[19]) : null,
            //                   };
            
            var videoCodes = from pair in management
                             from frames in pair.Value.VideoFrames
                             select new { File = pair.Key, Frame = frames };

            var data = from frame in videoCodes
                       select new
                       {
                           frame.File,
                           DataLen = (frame.Frame.Data as VideoData).Data.Length,
                           Byte00 = (frame.Frame.Data as VideoData).Data[0],
                           Byte01 = (frame.Frame.Data as VideoData).Data[1],
                           Byte02 = (frame.Frame.Data as VideoData).Data[2],
                           Byte03 = (frame.Frame.Data as VideoData).Data[3],
                           Byte04 = (frame.Frame.Data as VideoData).Data[4],
                           Byte05 = (frame.Frame.Data as VideoData).Data[5],
                           Byte06 = (frame.Frame.Data as VideoData).Data[6],
                           Byte07 = (frame.Frame.Data as VideoData).Data[7],
                           Byte08 = (frame.Frame.Data as VideoData).Data[8],
                           Byte09 = (frame.Frame.Data as VideoData).Data[9],
                           Byte10 = (frame.Frame.Data as VideoData).Data[10],
                           Byte11 = (frame.Frame.Data as VideoData).Data[11],
                           Byte12 = (frame.Frame.Data as VideoData).Data[12],
                           Byte13 = (frame.Frame.Data as VideoData).Data[13],
                           Byte14 = ((frame.Frame.Data as VideoData).Data.Length > 14) ? (Byte?)((frame.Frame.Data as VideoData).Data[14]) : null,
                           Byte15 = ((frame.Frame.Data as VideoData).Data.Length > 15) ? (Byte?)((frame.Frame.Data as VideoData).Data[15]) : null,
                           Byte16 = ((frame.Frame.Data as VideoData).Data.Length > 16) ? (Byte?)((frame.Frame.Data as VideoData).Data[16]) : null,
                           Byte17 = ((frame.Frame.Data as VideoData).Data.Length > 17) ? (Byte?)((frame.Frame.Data as VideoData).Data[17]) : null,
                           Byte18 = ((frame.Frame.Data as VideoData).Data.Length > 18) ? (Byte?)((frame.Frame.Data as VideoData).Data[18]) : null,
                           Byte19 = ((frame.Frame.Data as VideoData).Data.Length > 19) ? (Byte?)((frame.Frame.Data as VideoData).Data[19]) : null,
                       };

            List<String> videoData = new List<String>();
            foreach (var videoFrameData in data)
                videoData.Add(videoFrameData.ToString());
        }
        */
    }
}