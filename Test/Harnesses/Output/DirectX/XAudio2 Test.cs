using System;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.Output.DirectX
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component.RiffHeader class.</summary>
    public class XAudio2Test : ITester
    {
        protected XAudio2Interface xaudio;

        public XAudio2Interface XAudio
        {
            get { return this.xaudio; }
        }

        /// <summary>Tests the class in question</summary>
        public void Test()
        {
            using (this.xaudio = XAudio2Interface.NewInstance())
            {
                StringBuilder buffer = new StringBuilder();

                UInt32 deviceCount = this.xaudio.GetDeviceCount();
                buffer.Append("Number of XAudio2 devices: ");
                buffer.AppendLine(deviceCount.ToString());

                for (UInt32 i = 0; i < deviceCount; ++i)
                {
                    DeviceDetails details = this.xaudio.GetDeviceDetails(i);
                    buffer.AppendLine(String.Format("Device {0} details:", i));
                    buffer.AppendLine(details.ToDescriptionString());
                }

                Interceptor.WriteMessage(buffer.ToString());
            }

            //String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.Riff.RiffPath").ToArray();
            //this.TestMulti(paths);
        }
        
        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);

            //using (FileStream dest = new FileStream(path + ".rewrite", FileMode.Create, FileAccess.Write))
            //    this.TestWrite(dest);
        }

        /// <summary>Tests the code </summary>
        /// <param name="paths"></param>
        public void TestMulti(String[] paths)
        {
            foreach (String path in paths)
            {
                this.Test(path, false);
            }
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
        //    this.riffFile = new RiffFile();
        //    this.riffFile.Read(source);

            //Console.Write(this.chunk.ToString());

            //if (prompt)
            //{
            //    Console.Write("Press [Enter] to continue...");
            //    Console.ReadLine();
            //}
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            //this.chunk.Write(destination);
        }
    }
}