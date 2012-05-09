using System;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.Music;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MUS
{
    /// <summary>User control for testing the MUS file class</summary>
    public class MusicPlaylistTestControl : HarnessFileBaseTestControlBase<PlaylistTest>
    {
        /// <summary>Boolean indicating whether audio should play back, used primarily during the testing loop on test cases</summary>
        protected Boolean playingAudio;

        #region Additional Conrol(s)
        /// <summary>Button to interrupt playlist playback</summary>
        protected Button btnInterruptPlayback;

        /// <summary>Button to stop playback</summary>
        protected Button btnStopPlayback;
        #endregion

        /// <summary>Default constructor</summary>
        public MusicPlaylistTestControl()
        {
            this.InitializeComponent();
            this.Harness = new PlaylistTest();
            this.InitializeControlFields();
            this.playingAudio = false;
        }

        /// <summary>InitializeComponent for the render audio control</summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            /* btnInterruptPlayback */
            this.btnInterruptPlayback = new System.Windows.Forms.Button();
            this.btnInterruptPlayback.Location = new System.Drawing.Point(270, 3);
            this.btnInterruptPlayback.Name = "btnInterruptPlayback";
            this.btnInterruptPlayback.Size = new System.Drawing.Size(96, 30);
            this.btnInterruptPlayback.TabIndex = 5;
            this.btnInterruptPlayback.Text = "Interrupt";
            this.btnInterruptPlayback.UseVisualStyleBackColor = true;
            this.btnInterruptPlayback.Click += new EventHandler(this.btnInterruptPlayback_Click);
            this.pnlTest.Controls.Add(this.btnInterruptPlayback);

            /* btnStopPlayback */
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnStopPlayback.Location = new System.Drawing.Point(372, 3);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(96, 30);
            this.btnStopPlayback.TabIndex = 6;
            this.btnStopPlayback.Text = "Stop Playback";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new EventHandler(this.btnStopPlayback_Click);
            this.pnlTest.Controls.Add(this.btnStopPlayback);
        }

        /// <summary>Interrupts the playlist playback, introducing the fadeout</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Parameters for the event</param>
        protected virtual void btnInterruptPlayback_Click(Object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.RunInterruptThread));
            thread.Name = "Interrupt thread";
            thread.Start();
        }

        /// <summary>Void method to raise the interrupt code in a separate thread</summary>
        protected virtual void RunInterruptThread()
        {
            this.Harness.InterruptPlayback();
        }

        /// <summary>Event handler for the Stop Playback button click event</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Parameters for the event</param>
        protected virtual void btnStopPlayback_Click(Object sender, EventArgs e)
        {
            this.playingAudio = false;
            this.Harness.StopPlayback();
        }

        /// <summary>Void method to raise the testing in a separate thread</summary>
        protected override void RunTestThread()
        {
            foreach (Object item in this.chklbTestItems.CheckedItems)
                if (this.playingAudio)  //added test to see if playback was ever stoped, blocking further file playback
                    this.Harness.DoTest(this, new TestEventArgs(item as String));

            //not applicable for running output
            //this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing ended: {0}", DateTime.Now.ToShortTimeString()))));
        }

        /// <summary>Handler for Test Selected click event</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">EventArgs for the click event</param>
        protected override void btnTestSelected_Click(Object sender, EventArgs e)
        {
            this.playingAudio = true;
            base.btnTestSelected_Click(sender, e);
        }
    }
}