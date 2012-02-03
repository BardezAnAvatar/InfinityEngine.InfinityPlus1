using System;
using System.Collections.Generic;
using System.Drawing;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.TIS
{
    public class TisRenderTestControl : HarnessImageCollectionTestControlBase<TileSet1>
    {
        #region Constants
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.TIS.Path";
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public TisRenderTestControl() : base() { }
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