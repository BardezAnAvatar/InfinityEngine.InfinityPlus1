using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR;

namespace Scratch
{
    internal class PvrScratch
    {
        internal void TestPvr()
        {
            //this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\A0041n00.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\A0900N00.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\A001800.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\A007107.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\A100000.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\bgback.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\GUITUT.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\MOS0000.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\mos0136.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\O4000N00.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\O730002.pvrz");
            this.Uncompress(@"N:\Test Data\Infinity Engine\Example files\PVRZ\BG2EE\mos0138.pvrz");

            //var pixelData = compressed.File.GetFrame();
        }

        internal void Uncompress(String fileName)
        {
            PowerVrCompressed compressed = new PowerVrCompressed();
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                compressed.Read(file);
            }

            using (FileStream output = new FileStream(fileName.Replace("pvrz", "pvr"), FileMode.Create, FileAccess.Write))
                compressed.File.Write(output);
        }
    }
}