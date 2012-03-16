using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses
{
    public interface IAudioTester
    {
        /// <summary>Method exposing a stop command</summary>
        void StopPlayback();
    }
}