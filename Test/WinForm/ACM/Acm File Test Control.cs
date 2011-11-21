using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation;
using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.ACM
{
    /// <summary>User control for testing the ACM file class</summary>
    public class AcmFileTestControl : HarnessBaseTestControlBase<AcmFileTest>
    {
        /// <summary>Boolean indicating whether audio should play back, used primarily during the testing loop on test cases</summary>
        protected Boolean playingAudio;

        #region Additional Conrol(s)
        /// <summary>Checkbox for whether or not to render audio</summary>
        protected CheckBox chkbxRenderAudio;
        protected Button btnStopPlayback;
        #endregion

        /// <summary>Default constructor</summary>
        public AcmFileTestControl()
        {
            this.InitializeComponent();
            this.Harness = new AcmFileTest();
            this.InitializeControlFields();
            this.chkbxRenderAudio.CheckedChanged += new EventHandler(chkbxRenderAudio_CheckedChanged);
            this.playingAudio = false;
        }

        /// <summary>Event handler for when the render audio checkbox is (de)selected</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Standard EventArgs parameter</param>
        protected virtual void chkbxRenderAudio_CheckedChanged(Object sender, EventArgs e)
        {
            this.Harness.RenderAudio = this.chkbxRenderAudio.Checked;
        }

        /// <summary>InitializeComponent for the render audio control</summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            /* btnStopPlayback */
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnStopPlayback.Location = new System.Drawing.Point(260, 3);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(96, 30);
            this.btnStopPlayback.TabIndex = 5;
            this.btnStopPlayback.Text = "Stop Playback";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new EventHandler(this.btnStopPlayback_Click);
            this.pnlTest.Controls.Add(this.btnStopPlayback);

            /* chkbxRenderAudio */
            this.chkbxRenderAudio = new System.Windows.Forms.CheckBox();
            this.chkbxRenderAudio.AutoSize = true;
            this.chkbxRenderAudio.Checked = true;
            this.chkbxRenderAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxRenderAudio.Location = new System.Drawing.Point(362, 11);
            this.chkbxRenderAudio.Name = "chkbxRenderAudio";
            this.chkbxRenderAudio.Size = new System.Drawing.Size(80, 17);
            this.chkbxRenderAudio.TabIndex = 3;
            this.chkbxRenderAudio.Text = "Render audio";
            this.chkbxRenderAudio.UseVisualStyleBackColor = true;
            this.pnlTest.Controls.Add(this.chkbxRenderAudio);
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
                if (this.playingAudio)  //added test to see if playback was ever interrupted
                    this.Harness.DoTest(this, new TestEventArgs(item as String));

            this.PostMessage(this, new LogEventArgs(new LogItem(LogType.Informational, String.Format("Testing ended: {0}", DateTime.Now.ToShortTimeString()))));
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