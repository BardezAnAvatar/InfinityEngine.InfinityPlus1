using System;

namespace Bardez.Projects.InfinityPlus1.Test
{
    /// <summary>Event Arguments containing test event parameters</summary>
    public class TestEventArgs : EventArgs
    {
        /// <summary>Message to be posted to the event</summary>
        public String Path { get; set; }

        /// <summary>Default constructor</summary>
        public TestEventArgs()
        {
            this.Path = null;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="path">Path to test</param>
        public TestEventArgs(String path)
        {
            this.Path = path;
        }
    }
}