using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TIS
{
    public class TisRenderTestControl : HarnessImageCollectionTestControlBase
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.TIS.Path";
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public TisRenderTestControl() : base() { }
        #endregion


        #region Image decoding/loading
        /// <summary>Opens & reads the animation from the provide path</summary>
        /// <param name="path">Path to read the image set from</param>
        /// <returns>The opened & read animation</returns>
        protected override IImageSet ReadImageSet(String path)
        {
            TileSet1 animation = new TileSet1();
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
            return ConfigurationHandlerMulti.GetSettingValues(TisRenderTestControl.configKey);
        }
        #endregion
    }
}