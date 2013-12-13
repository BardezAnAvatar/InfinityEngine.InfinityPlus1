using System;
using System.Collections.Generic;
using System.Drawing;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation;
using Bardez.Projects.InfinityPlus1.Test.WinForm;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BAM
{
    /// <summary>Rendering test animation control for BAM files</summary>
    public class BamRenderTestControl : HarnessAnimationCollectionTestControlBase<BioWareAnimation1>
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.BAM.Path";
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BamRenderTestControl() : base() { }
        #endregion


        #region Helper methods
        /// <summary>Gets the paths to test from the config file</summary>
        /// <returns>An IList of Strings for file paths</returns>
        protected override IList<String> GetPaths()
        {
            return ConfigurationHandlerMulti.GetSettingValues(BamRenderTestControl.configKey);
        }

        /// <summary>Overridable method to set detail of a frame from within DecodeSingleImageCollection</summary>
        /// <param name="frame">ImageReference to set details for</param>
        /// <param name="container">ImageCollection format that contains animations and frames</param>
        /// <param name="frameIndex">Frame number setting the details for</param>
        protected override void SetFrameDetails(ImageReference frame, BioWareAnimation1 container, Int32 frameIndex)
        {
            frame.RenderOriginX = container.MaxCenterX + frame.RenderOriginX;
            frame.RenderOriginY = container.MaxCenterY + frame.RenderOriginY;
        }
        #endregion
    }
}