using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.MapOfScreen;
using Bardez.Projects.InfinityPlus1.Test.Harnesses.MapOfScreen;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.MOS
{
    /// <summary>Performs render testing on the Direct2D render target, loading a list of bitmapped MOSC files selectable for display</summary>
    public class MoscRenderTestControl : HarnessImageTestControl<MapOfScreenCompressed_v1>
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
    }
}