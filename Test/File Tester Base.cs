using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Test
{
    /// <summary>Abstract base type that relies filepaths to test</summary>
    public abstract class FileTesterBase : TesterBase
    {
        /// <summary>List of file paths to test</summary>
        public List<String> FilePaths { get; set; }

        /// <summary>Initializes the test class, to be called in the constructor</summary>
        protected override void InitializeInstance()
        {
            base.InitializeInstance();
            this.FilePaths = new List<String>();
        }

        /// <summary>Tests multiple instances of test cases</summary>
        protected override void TestMulti()
        {
            foreach (String path in this.FilePaths)
                this.DoTest(new TestEventArgs(path));
        }
    }
}