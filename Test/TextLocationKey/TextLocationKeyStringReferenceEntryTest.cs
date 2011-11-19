using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.TextLocationKey;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;

namespace Bardez.Projects.InfinityPlus1.Test.TextLocationKey
{
    /// <summary>This class tests the usable methods in the InfinityPlus1.Files.TextLocationKeyStringReference class</summary>
    public class TextLocationKeyStringReferenceEntryTest : ITester
    {
        protected TextLocationKeyStringReference entry;

        public TextLocationKeyStringReference StringReference
        {
            get { return this.entry; }
        }

        public void Test()
        {
            String path = ConfigurationHandler.GetSettingValue("Test.Tlk1.Tlk1Path");
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

            Interceptor.WriteMessage(entry.ToString());
        }
    }
}