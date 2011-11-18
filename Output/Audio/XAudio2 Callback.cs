using System;

using Bardez.Projects.DirectX.XAudio2;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>XAudio2 Voice Callback manager</summary>
    internal class XAudio2InterfaceCallback : VoiceCallback
    {
        /// <summary>XAudio2 output engine to call back to</summary>
        protected XAudio2Output Engine { get; set; }

        /// <summary>Definition constructor</summary>
        /// <param name="engine">XAudio2 engine instance to call back to</param>
        public XAudio2InterfaceCallback(XAudio2Output engine)
        {
            this.Engine = engine;
        }

        #region Callbacks
        /// <summary>Called when the voice finishes processing a buffer</summary>
        /// <param name="bufferContext">Pointer to a buffer context, defined and used by the client software.</param>
        public override void OnBufferEnd(IntPtr bufferContext)
        {
            this.Engine.BufferEndHandler(bufferContext);
        }
        #endregion
    }
}