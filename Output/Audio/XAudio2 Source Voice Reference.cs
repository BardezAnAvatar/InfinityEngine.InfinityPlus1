using System;

using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Output.Audio.Enums;
using Bardez.Projects.Win32;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>Keeps a Voice reference and some state data</summary>
    internal class XAudio2SourceVoiceReference<VoiceType> : XAudio2VoiceReference<VoiceType> where VoiceType : SourceVoice
    {
        /// <summary>Defintion constructor</summary>
        /// <param name="voiceInstance">Instance of a voice object to reference</param>
        /// <remarks>Expects the reference to be valid and virgin, unexposed to playback so far</remarks>
        internal XAudio2SourceVoiceReference(VoiceType voiceInstance) : base(voiceInstance)
        {
            this.RegisterCallbacks();
        }

        #region Events
        /// <summary>Public event handler for getting more sample data</summary>
        public event AudioNeedsMoreDataHandler NeedMoreSampleData;
        #endregion

        #region Callback
        /// <summary>Registers local handlers to callback events</summary>
        protected void RegisterCallbacks()
        {
            if (this.Reference != null)
            {
                this.Reference.Callback.BufferEnd += new XAudio2VoiceBufferEndHandler(this.BufferEndHandler);
                this.Reference.Callback.BufferStart += new XAudio2VoiceBufferStartHandler(this.BufferStartHandler);
                this.Reference.Callback.Error += new XAudio2VoiceErrorHandler(this.ErrorHandler);
                this.Reference.Callback.LoopEnd += new XAudio2VoiceLoopEndHandler(this.LoopEndHandler);
                this.Reference.Callback.ProcessingPassEnd += new XAudio2VoiceProcessingPassEndHandler(this.ProcessingPassEndHandler);
                this.Reference.Callback.ProcessingPassStart += new XAudio2VoiceProcessingPassStartHandler(this.ProcessingPassStartHandler);
                this.Reference.Callback.StreamEnd += new XAudio2VoiceStreamEndHandler(this.StreamEndHandler);
            }
        }

        #region Voice callbacks
        /// <summary>Delegate for when a voice buffer finishes</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void BufferEndHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice buffer starts</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void BufferStartHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice loop ends</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        internal virtual void LoopEndHandler(IntPtr bufferContext)
        {
        }

        /// <summary>Delegate for when a voice stream ends</summary>
        internal virtual void StreamEndHandler()
        {
        }

        /// <summary>Delegate for when a voice error is raised</summary>
        /// <param name="bufferContext">IntPtr for a buffer context</param>
        /// <param name="error">Error encountered</param>
        internal virtual void ErrorHandler(IntPtr bufferContext, ResultCode error)
        {
        }

        /// <summary>Delegate for when a voice processing pass ends</summary>
        internal virtual void ProcessingPassEndHandler()
        {
        }

        /// <summary>Delegate for when a voice processing pass starts</summary>
        /// <param name="bytesRequired">The number of bytes that need to be submitted to avoid voice starvation</param>
        internal virtual void ProcessingPassStartHandler(UInt32 bytesRequired)
        {
            if (bytesRequired > 0 && (this.State != XAudio2VoiceState.InUseEnding && this.State != XAudio2VoiceState.Depleted))
                this.NeedMoreSampleData(); //Raise the event
        }
        #endregion

        /// <summary>Callback for when playback finishes on a buffer.</summary>
        /// <param name="context">IntPtr that contains an Int32, the meaning of which is the index to the Voice in question being played back</param>
        internal void NeedsMoreDataHandler(IntPtr context)
        {
            Int32 voiceIndex = context.ToInt32();

            SourceVoice voice = this.Reference as SourceVoice;
            if (voice == null)
                throw new InvalidCastException(String.Format("The expected voice was not of the type SourceVoice, but rather {0}.", this.Reference.GetType().ToString()));

            //now we are certain it is done.
            VoiceState state = voice.GetState();
            if (state.BuffersQueued == 0 && this.State == XAudio2VoiceState.InUsePersisting)   //state is ready for more data
                this.RaiseNeedMore();   //raise event
        }

        /// <summary>Adds an event handler to request further data</summary>
        /// <param name="handler">Delegate to add more data</param>
        public void AddSourceNeedDataEventhandler(AudioNeedsMoreDataHandler handler)
        {
            this.NeedMoreSampleData += handler;
        }

        ///// <summary>Callback for when playback finishes on a buffer, to destroy the voice.</summary>
        ///// <param name="context">IntPtr that contains an Int32, the meaning of which is the index to the Voice in question being played back</param>
        ///// <remarks>Per MSDN, callback destruction is verboten!</remarks>
        //internal void DestroyVoiceHandler(IntPtr context)
        //{
        //    SourceVoice voice = this.Reference as SourceVoice;
        //    if (voice == null)
        //        throw new InvalidCastException(String.Format("The expected voice was not of the type SourceVoice, but rather {0}.", this.Reference.GetType().ToString()));

        //    //now we are certain it is done.
        //    VoiceState state = voice.GetState();

        //    if (state.BuffersQueued == 0 && (this.State == XAudio2VoiceState.Depleted || this.State == XAudio2VoiceState.InUseEnding))
        //        this.DisposeSourceVoice(voiceIndex);
        //}
        
        /// <summary>Raises the NeedMore event</summary>
        public void RaiseNeedMore()
        {
            this.NeedMoreSampleData();
        }
        #endregion

        #region Destruction
        /// <summary>Disposal code</summary>
        /// <remarks>Finalize()</remarks>
        ~XAudio2SourceVoiceReference()
        {
            this.Disposal();
        }
        #endregion
    }
}