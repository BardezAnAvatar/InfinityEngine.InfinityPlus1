using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bardez.Projects.InfinityPlus1.Logic.Infinity.Assets;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.TestClasses.InfinityLogic.Assets
{
    [TestClass]
    public class InfinityAssetManagerTest
    {
        [TestMethod]
        public void InfinityAssetManager_Debug_Test()
        {
            InfinityAssetManager manager = new InfinityAssetManager(@"\Test Data\Infinity Engine\Installs\Baldur's Gate\Baldur's Gate (TotSC) (v. 1.3.5512)");
            var
            //var = manager.GetAssetTree_GroupedOverridden();           //view 1
            //var = manager.GetAssetTree_GroupedAllInstances();         //view 2
            //var = manager.GetAssetTree_LocationInstances();           //view 3
            var = manager.GetAssetTree_LocationGroupedOverridden();     //view 4
        }
    }
}