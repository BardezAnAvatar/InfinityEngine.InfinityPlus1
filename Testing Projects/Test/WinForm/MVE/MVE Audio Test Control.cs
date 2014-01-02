using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie;
using Bardez.Projects.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE file audio streams</summary>
    public class MveAudioTestControl : HarnessAudioCollectionTestControlBase
    {
        #region Properties
        /// <summary>The Harness casted as its intended type</summary>
        protected MveAudioPlaybackTest HarnessMVE
        {
            get { return this.Harness as MveAudioPlaybackTest; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveAudioTestControl() : base()
        {
            this.Harness = new MveAudioPlaybackTest();
            this.InitializeControlFields();
        }
        #endregion


        #region Control
        /// <summary>Stops playback</summary>
        protected override void StopPlayback()
        {
            this.HarnessMVE.StopPlayback();
        }
        #endregion
    }
}