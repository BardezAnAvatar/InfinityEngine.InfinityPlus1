using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.Multimedia.MediaBase.Frame.Video;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Represents a base class for UI harnesses that display images</summary>
    public abstract partial class HarnessMovieTestControl<MovieFormat> : HarnessVideoTestControl<MovieFormat> where MovieFormat : IMovie
    {
        #region Fields
        /// <summary>Flag indicating whether or not to render audio data</summary>
        protected Boolean RenderAudio { get; set; }
        #endregion


        #region Controls
        protected CheckBox chkbxRenderAudio;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessMovieTestControl() : base()
        {          
            this.RenderAudio = true;
        }

        /// <summary>Overridden Initialize Component for added controls</summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.AddedInitializeComponent();
        }

        /// <summary>Added Initialize Component logic for added controls</summary>
        protected virtual void AddedInitializeComponent()
        {
            this.chkbxRenderAudio = new CheckBox();

            this.pnlLeftButtons.SuspendLayout();
            this.SuspendLayout();

            //resize panel
            this.pnlLeftButtons.Size = new System.Drawing.Size(378, 41);

            // 
            // chkbxRenderAudio
            // 
            this.chkbxRenderAudio.AutoSize = true;
            this.chkbxRenderAudio.Checked = true;
            this.chkbxRenderAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxRenderAudio.Location = new System.Drawing.Point(290, 11);
            this.chkbxRenderAudio.Name = "chkbxRenderAudio";
            this.chkbxRenderAudio.Size = new System.Drawing.Size(90, 17);
            this.chkbxRenderAudio.TabIndex = 41;
            this.chkbxRenderAudio.Text = "Render audio";
            this.chkbxRenderAudio.UseVisualStyleBackColor = true;
            this.chkbxRenderAudio.CheckedChanged += new System.EventHandler(this.chkbxRenderAudio_CheckedChanged);

            //Attach controls
            this.pnlLeftButtons.Controls.Add(this.chkbxRenderAudio);

            //resume
            this.pnlLeftButtons.ResumeLayout(false);
            this.pnlLeftButtons.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion


        #region Event handlers
        /// <summary>Event handler for when the "Render Audio" checkbox changes</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void chkbxRenderAudio_CheckedChanged(Object sender, EventArgs e)
        {
            this.RenderAudio = this.chkbxRenderAudio.Checked;
        }
        #endregion


        #region Video display
        /// <summary>Ceases video (and possibly audio) playback</summary>
        protected override void StopPlayback()
        {
            if (this.VideoController != null)
            {
                this.VideoController.StopVideoPlayback();
                this.VideoController.ClearTimerElapsed();
                this.StopAudioPlayback();
            }
        }
        #endregion


        #region Audio Control
        /// <summary>Halts audio playback</summary>
        protected abstract void StopAudioPlayback();

        /// <summary>Starts audio playback</summary>
        protected abstract void StartAudioPlayback();
        #endregion
    }
}