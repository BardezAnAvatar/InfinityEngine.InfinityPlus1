using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Output.DirectX
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component.RiffHeader class.</summary>
    public class XAudio2Test : TesterBase
    {
        protected XAudio2Interface XAudio { get; set; }

        #region Construction
        /// <summary>Default constructor</summary>
        public XAudio2Test()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        /// <remarks>No implementation due to lack of initialization to this test class</remarks>
        protected override void InitializeTestData(Object sender, EventArgs e) { }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            this.TestDeviceDetails();
        }

        /// <summary>Tests the class in question</summary>
        public void TestDeviceDetails()
        {
            using (this.XAudio = XAudio2Interface.NewInstance())
            {
                StringBuilder buffer = new StringBuilder();

                UInt32 deviceCount = this.XAudio.GetDeviceCount();
                buffer.Append(StringFormat.ToStringAlignment("Number of XAudio2 devices", 0));
                buffer.AppendLine(deviceCount.ToString());

                for (UInt32 i = 0; i < deviceCount; ++i)
                {
                    DeviceDetails details = this.XAudio.GetDeviceDetails(i);
                    buffer.AppendLine(StringFormat.ToStringAlignment(String.Format("Device {0} details", i), 0));
                    buffer.Append(details.ToDescriptionString());
                }

                this.DoPostMessage(new MessageEventArgs(buffer.ToString(), "Output", "Devices"));
            }
        }
    }
}