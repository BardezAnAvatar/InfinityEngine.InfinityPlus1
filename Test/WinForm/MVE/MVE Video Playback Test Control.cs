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

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.Read(fs);
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        mve.ReadChunkOpcodes(fs);
                    else
                        goto BreakPoint;
                }

                MveManager manager = new MveManager(mve);

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.CollectOpcodeIndex();
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.ReadData(fs);
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.DecodeVideoMaps();
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                        manager.InitializeCoders();
                    else
                        goto BreakPoint;
                }

                lock (this.abortLock)
                {
                    if (filePath == this.currentMoviePath)
                    {
                        if (this.VideoController != null)
                            this.VideoController.StopVideoPlayback();

                        this.VideoController = manager;
                    }
                    else
                        goto BreakPoint;
                }

            BreakPoint:
                ;
            }
        }
        #endregion
    }
}