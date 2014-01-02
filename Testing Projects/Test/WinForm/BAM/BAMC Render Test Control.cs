using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.BAM
{
    /// <summary>Rendering test animation control for BAMC files</summary>
    public class BamcRenderTestControl : HarnessAnimationCollectionTestControlBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.BAMC.Path";
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BamcRenderTestControl() : base() { }
        #endregion


        #region Image decoding/loading
        /// <summary>Opens & reads the animation from the provide path</summary>
        /// <param name="path">Path to read the animation from</param>
        /// <returns>The opened & read animation</returns>
        protected override IAnimation ReadAnimation(String path)
        {
            BioWareAnimationCompressed_v1 animation = new BioWareAnimationCompressed_v1();
            using (FileStream fs = ReusableIO.OpenFile(path))
                animation.Read(fs);

            return animation;
        }
        #endregion


        #region Helper methods
        /// <summary>Gets the paths to test from the config file</summary>
        /// <returns>An IList of Strings for file paths</returns>
        protected override IList<String> GetPaths()
        {
            return ConfigurationHandlerMulti.GetSettingValues(BamcRenderTestControl.configKey);
        }

        /// <summary>Overridable method to set detail of a frame from within DecodeSingleImageCollection</summary>
        /// <param name="frame">ImageReference to set details for</param>
        /// <param name="container">ImageCollection format that contains animations and frames</param>
        /// <param name="frameIndex">Frame number setting the details for</param>
        protected override void SetFrameDetails(ImageReference frame, IAnimation container, Int32 frameIndex)
        {
            if (!(container is BioWareAnimationCompressed_v1))
                throw new InvalidCastException("This class should be receiving a BioWareAnimationCompressed_v1 IAnimation in the container parameter, but could not cast it as such.");

            BioWareAnimationCompressed_v1 bam = container as BioWareAnimationCompressed_v1;

            frame.RenderOriginX = bam.File.MaxCenterX + frame.RenderOriginX;
            frame.RenderOriginY = bam.File.MaxCenterY + frame.RenderOriginY;
        }
        #endregion
    }
}