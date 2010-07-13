using System;
using System.IO;
using InfinityPlus1.Files;

namespace InfinityPlus1.Tester.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKeyHeader class</summary>
    public class TextLocationKeyHeaderTest : Tester
    {
        protected TextLocationKeyHeader header;

        public void Test()
        {
            String path;
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\Dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\Baldurs Gate Trilogy\dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialog.tlk";
            path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialog.tlk";

            Test(path);
        }

        public void Test(String Path)
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open))
                Test(stream);
        }

        public void Test(Stream Source)
        {
            header = new TextLocationKeyHeader();
            header.ReadHeader(Source);

            Console.Write(header.ToString());
        }

        public TextLocationKeyHeader Header
        {
            get { return header; }
        }
    }
}