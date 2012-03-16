using System;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses
{
    /// <summary>Event Arguments containing a message to post</summary>
    public class AudioTestEventArgs : TestEventArgs
    {
        #region
        /// <summary>Channel number to test</summary>
        public Int32 Channel { get; set; }
        #endregion

        /// <summary>Default constructor</summary>
        public AudioTestEventArgs() : base()
        {
            this.Path = null;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="path">Path to test</param>
        public AudioTestEventArgs(String path, Int32 channelNumber) : base(path)
        {
            this.Channel = channelNumber;
        }
    }
}