using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE video playback</summary>
    public class MveVideoPlaybackTestControl : HarnessVideoTestControl<MveManager>
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.MVE.Path";
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected override String ConfigKey
        {
            get { return MveVideoPlaybackTestControl.configKey; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveVideoPlaybackTestControl() : base() { }
        #endregion


        #region Movie decoding
        /// <summary>
        ///     Method to decode a single movie file, in a multi-threaded environment. It will abort decoding if a change in
        ///     the selected movie occurs.
        /// </summary>
        /// <param name="filePath">String of a filepath, casted as an Object for use by ThreadPool</param>
        protected override void DecodeSingleMovie(String filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MveChunkOpcodeIndexer mve = new MveChunkOpcodeIndexer();

                //read the mve chunks
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.Read(fs);
                    else
                        goto BreakPoint;
                }

                //read the mve opcodes
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.ReadChunkOpcodes(fs);
                    else
                        goto BreakPoint;
                }

                //instantiate a new MVE manager
                MveManager manager = new MveManager(mve);

                //collect manager's opcodes
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.CollectOpcodeIndex();
                    else
                        goto BreakPoint;
                }

                //read data for manager's opcodes
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.ReadData(fs);
                    else
                        goto BreakPoint;
                }

                //decode manager's video maps
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.DecodeVideoMaps();
                    else
                        goto BreakPoint;
                }

                //initialize the manager's coders
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.InitializeCoders();
                    else
                        goto BreakPoint;
                }

                //dispose current coontroller and assign it manager
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                    {
                        if (this.VideoController != null)
                        {
                            this.VideoController.StopVideoPlayback();
                            this.VideoController.Dispose();
                        }

                        this.VideoController = manager;
                    }
                    else
                        goto BreakPoint;
                }

                //start decoding audio and video
                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                    {
                        if (this.VideoController != null)
                        {
                            this.VideoController.PreemptivelyStartDecodingAudio();
                            this.VideoController.PreemptivelyStartDecodingVideo();
                        }
                    }
                    else
                        goto BreakPoint;
                }

                //skip the error-out condition
                goto Finish;

                //label for an error position that requires cleanup
            BreakPoint:
                if (this.VideoController != null)
                    this.VideoController.Dispose();

            Finish:
                ;
            }
        }
        #endregion
    }
}