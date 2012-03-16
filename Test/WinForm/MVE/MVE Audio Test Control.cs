using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Movie;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MVE
{
    /// <summary>User control for testing the MVE file audio streams</summary>
    public class MveAudioTestControl : HarnessAudioCollectionTestControlBase<MveAudioPlaybackTest>
    {
        /// <summary>Default constructor</summary>
        public MveAudioTestControl() : base()
        {
            this.Harness = new MveAudioPlaybackTest();
            this.InitializeControlFields();
        }
    }
}