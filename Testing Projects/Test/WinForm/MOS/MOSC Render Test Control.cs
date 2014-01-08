using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic;
using Bardez.Projects.InfinityPlus1.Test.Harnesses.Mosaic;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>Performs render testing on the Direct2D render target, loading a list of bitmapped MOSC files selectable for display</summary>
    public class MoscRenderTestControl : HarnessImageTestControl
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        private const String configKey = "Test.MOSC.Path";
        #endregion


        #region Properties
        /// <summary>Exposes the configuration key to use to pull from the app.config or similar source</summary>
        protected override String ConfigKey
        {
            get { return MoscRenderTestControl.configKey; }
        }
        #endregion


        #region Image decoding & loading
        /// <summary>Opens & reads the image from the provide path</summary>
        /// <param name="path">Path to read the animation from</param>
        /// <returns>The opened & read animation</returns>
        protected override IImage ReadImage(String path)
        {
            MosaicCompressed_v1 animation = new MosaicCompressed_v1();
            using (FileStream fs = ReusableIO.OpenFile(path))
                animation.Read(fs);

            return animation;
        }
        #endregion
    }
}