using System;
using System.IO;
using InfinityPlus1.Files;

namespace InfinityPlus1.Tester.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKeyStringReference class</summary>
    public class TextLocationKeyStringReferenceEntryTest : ITester
    {
        protected TextLocationKeyStringReference entry;

        public void Test()
        {
            String path;
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\Dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\Baldurs Gate Trilogy\dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialog.tlk";

            path = @"D:\Games\Icewind Dale\dialog.tlk";

            Test(path);
        }

        public void Test(String DialoguePath)
        {
            using (FileStream stream = new FileStream(DialoguePath, FileMode.Open))
                Test(stream);
        }

        public void Test(Stream Source)
        {
            TextLocationKeyHeaderTest headerTester = new TextLocationKeyHeaderTest();
            headerTester.Test(Source);

            entry = new TextLocationKeyStringReference();
            entry.ReadStringReferenceFull(Source, headerTester.Header.StringsReferenceOffset, headerTester.Header.CultureReference);

            Console.Write(entry.ToString());
        }

        public TextLocationKeyStringReference StringReference
        {
            get { return entry; }
        }
    }
}