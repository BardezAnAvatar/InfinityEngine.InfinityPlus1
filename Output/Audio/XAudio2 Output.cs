using System;
using System.Collections.Generic;
using System.Threading;

using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.Win32.Audio;
using Bardez.Projects.InfinityPlus1.Output.Audio.Enums;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    public class XAudio2Output : AudioOutput, IDisposable
    {
        #region Fields
        /// <summary>Private lock not accessable by other memory</summary>
        /// <remarks>Static instantiation</remarks>
        private static Object singletonLock = new Object();

        /// <summary>Singleton instance of XAudio2Output</summary>
        private static XAudio2Output singleton;

        /// <summary>XAudio2 constant indicating that an action is to be immediate</summary>
        private const UInt32 CommitNow = 0U;

        /// <summary>XAudio2 constant indicating that the stream is expected to close after this buffer is finished.</summary>
        private const UInt32 EndOfStream = 0x0040U;

        /// <summary>List of voices that can be re-referenced by index</summary>
        private List<XAudio2VoiceReference> sourceVoices;

        /// <summary>List of voices that can be re-referenced by index</summary>
        private List<XAudio2VoiceReference> destinationVoices;

        /// <summary>XAudio2 interface object</summary>
        protected XAudio2Interface XAudio2;
        #endregion

        /// <summary>Singleton instance accessor</summary>
        public static XAudio2Output Instance
        {
            get
            {
                lock (XAudio2Output.singletonLock)
                {
                    XAudio2Output.Instantiate();
                    return XAudio2Output.singleton;
                }
            }
        }

        /// <summary>Halts all playback.</summary>
        /// <remarks>Intended to 'emergency' execution stops.</remarks>
        public void HaltPlayback()
        {
            //loop through all voices
            for (Int32 voiceIndex = 0; voiceIndex < this.sourceVoices.Count; ++voiceIndex)
            {
                XAudio2VoiceReference voiceReference = this.sourceVoices[voiceIndex];
                if (voiceReference != null && voiceReference.Reference != null)
                {
                    SourceVoice voice = voiceReference.Reference as SourceVoice;
                    voice.Stop(0U, XAudio2Output.CommitNow);
                    if (voiceReference.State == XAudio2VoiceState.InUseEnding)
                        this.DisposeSourceVoice(voiceIndex);
                }
            }
        }

        /// <summary>Sets the singleton instance of the XAudio2 playback engine</summary>
        private static void Instantiate()
        {
            if (XAudio2Output.singleton == null)
                XAudio2Output.singleton = new XAudio2Output();
        }

        /// <summary>Singleton constructor</summary>
        private XAudio2Output()
        {
            this.sourceVoices = new List<XAudio2VoiceReference>();
            this.destinationVoices = new List<XAudio2VoiceReference>();
            this.XAudio2 = XAudio2Interface.NewInstance();

            //also, register an output MasteringVoice, since to use it you will almost certainly always need one.
            //Also, you can be lame and assume that index 0 will always be a valid Mastering Voice

            //this should probably use the default system rendering, and submisions submit sample rate transformations as needed, so void list of parameters
            MasteringVoice masteringVoice = this.XAudio2.CreateMasteringVoice();
            this.destinationVoices.Add(new XAudio2VoiceReference(masteringVoice));
        }

        /// <summary>Disposal code</summary>
        public void Dispose()
        {
            if (this.sourceVoices != null)
                for (Int32 i = 0; i < this.sourceVoices.Count; ++i)
                {
                    if (this.sourceVoices[i] != null)
                        this.sourceVoices[i].Reference = null;

                    this.sourceVoices[i] = null;
                }

            if (this.destinationVoices != null)
                for (Int32 i = 0; i < this.destinationVoices.Count; ++i)
                {
                    if (this.destinationVoices[i] != null)
                        this.destinationVoices[i].Reference = null;

                    this.destinationVoices[i] = null;
                }


            if (this.XAudio2 != null)
            {
                this.XAudio2.Dispose();
                this.XAudio2 = null;
            }
        }

        /// <summary>Creates an instance of the underlying/associated audio type and returns a unique identifier for re-reference</summary>
        /// <param name="inputData">PCM info describing the source</param>
        /// <returns>An Int64 usable to re-refernce the playback object</returns>
        /// <remarks>This methods assumes that the playback will be re-referenced.</remarks>
        public override Int32 CreatePlayback(WaveFormatEx inputData)
        {
            Int32 sourceIndex = -1;
            lock (XAudio2Output.singletonLock)
            {

                //register for callback
                XAudio2InterfaceCallback callback = new XAudio2InterfaceCallback(this);

                SourceVoice voice = this.XAudio2.CreateSourceVoice(inputData, 0U, 2.0f, callback);
                sourceIndex = this.sourceVoices.Count;
                this.sourceVoices.Add(new XAudio2VoiceReference(voice));
            }
            return sourceIndex;
        }

        /// <summary>Submit data for immediate playback to the playback device, indicating whether or not it should finalize after playback</summary>
        /// <param name="data">Data to submit</param>
        /// <param name="source">Source to submit data to</param>
        /// <param name="destination">Destination to output submission to</param>
        /// <param name="expectMore">Flag indicating whether to expect more data</param>
        /// <remarks>Does no looping or anything, but it is possible to set that all up.</remarks>
        public override void SubmitData(Byte[] data, Int32 source, Int32 destination, Boolean expectMore = false)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference srcVoice = this.sourceVoices[source];
                XAudio2VoiceReference dstVoice = this.destinationVoices[destination];

                //error checking
                if (srcVoice == null)
                    throw new ArgumentException(String.Format("The unique identifier for the source Voice {0} was null", source));

                if (dstVoice == null)
                    throw new ArgumentException(String.Format("The unique identifier for the destination Voice {0} was null", destination));

                if (expectMore && (srcVoice.State == XAudio2VoiceState.InUseEnding || srcVoice.State == XAudio2VoiceState.Depleted))
                    throw new InvalidOperationException("Cannot submit a buffer to a sealed source voice.");

                //main processing
                SourceVoice srcVoiceInstance = (srcVoice.Reference as SourceVoice);
                srcVoiceInstance.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, dstVoice.Reference) });

                if (!expectMore && srcVoice.State != XAudio2VoiceState.InUseEnding)
                    srcVoice.State = XAudio2VoiceState.InUseEnding;
                else if (expectMore && srcVoice.State == XAudio2VoiceState.NotSubmitted)
                    srcVoice.State = XAudio2VoiceState.InUsePersisting;

                AudioBuffer buffer = new AudioBuffer(expectMore ? 0 : XAudio2Output.EndOfStream, data, 0, 0, 0, 0, 0, new IntPtr(source));
                srcVoiceInstance.SubmitSourceBuffer(buffer, null);

                srcVoiceInstance.Start();
            }
        }

        /// <summary>Submit data for playback to the playback device, indicating whether or not it should finalize after this buffer and whether or not to play immediately</summary>
        /// <param name="data">Data to submit</param>
        /// <param name="source">Source to submit data to</param>
        /// <param name="destination">Destination to output submission to</param>
        /// <param name="expectMore">Flag indicating whether to expect more data</param>
        /// <param name="startPlayback">Flag indicating whether start playback</param>
        /// <remarks>Does no looping or anything, but it is possible to set that all up.</remarks>
        public void SubmitData(Byte[] data, Int32 source, Int32 destination, Boolean expectMore = false, Boolean startPlayback = true)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference srcVoice = this.sourceVoices[source];
                XAudio2VoiceReference dstVoice = this.destinationVoices[destination];

                //error checking
                if (srcVoice == null)
                    throw new ArgumentException(String.Format("The unique identifier for the source Voice {0} was null", source));

                if (dstVoice == null)
                    throw new ArgumentException(String.Format("The unique identifier for the destination Voice {0} was null", destination));

                if (expectMore && (srcVoice.State == XAudio2VoiceState.InUseEnding || srcVoice.State == XAudio2VoiceState.Depleted))
                    throw new InvalidOperationException("Cannot submit a buffer to a sealed source voice.");

                //main processing
                SourceVoice srcVoiceInstance = (srcVoice.Reference as SourceVoice);
                srcVoiceInstance.SetOutputVoices(new VoiceSendDescriptor[] { new VoiceSendDescriptor(0U, dstVoice.Reference) });

                if (!expectMore && srcVoice.State != XAudio2VoiceState.InUseEnding)
                    srcVoice.State = XAudio2VoiceState.InUseEnding;
                else if (expectMore && srcVoice.State == XAudio2VoiceState.NotSubmitted)
                    srcVoice.State = XAudio2VoiceState.InUsePersisting;

                AudioBuffer buffer = new AudioBuffer(expectMore ? 0 : XAudio2Output.EndOfStream, data, 0, 0, 0, 0, 0, new IntPtr(source));
                srcVoiceInstance.SubmitSourceBuffer(buffer, null);

                if (startPlayback)
                    srcVoiceInstance.Start();
            }
        }

        /// <summary>Submit data for playback to the playback device, indicating whether or not it should finalize after this buffer and whether or not to play immediately</summary>
        /// <param name="data">Data to submit</param>
        /// <param name="source">Source to submit data to</param>
        /// <param name="expectMore">Flag indicating whether to expect more data</param>
        /// <param name="startPlayback">Flag indicating whether start playback</param>
        /// <remarks>Does no looping or anything, and intended to be used after a preliminary SubmitData(...) call</remarks>
        public void SubmitSubsequentData(Byte[] data, Int32 source, Boolean expectMore = true, Boolean startPlayback = false)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference srcVoice = this.sourceVoices[source];

                //error checking
                if (srcVoice == null)
                    throw new ArgumentException(String.Format("The unique identifier for the source Voice {0} was null", source));

                if (expectMore && (srcVoice.State == XAudio2VoiceState.InUseEnding || srcVoice.State == XAudio2VoiceState.Depleted))
                    throw new InvalidOperationException("Cannot submit a buffer to a sealed source voice.");

                //main processing
                SourceVoice srcVoiceInstance = (srcVoice.Reference as SourceVoice);

                if (!expectMore && srcVoice.State != XAudio2VoiceState.InUseEnding)
                    srcVoice.State = XAudio2VoiceState.InUseEnding;
                else if (expectMore && srcVoice.State == XAudio2VoiceState.NotSubmitted)
                    srcVoice.State = XAudio2VoiceState.InUsePersisting;

                AudioBuffer buffer = new AudioBuffer(expectMore ? 0 : XAudio2Output.EndOfStream, data, 0, 0, 0, 0, 0, new IntPtr(source));
                srcVoiceInstance.SubmitSourceBuffer(buffer, null);

                if (startPlayback)
                    srcVoiceInstance.Start();
            }
        }

        /// <summary>Starts playback of the source voice</summary>
        /// <param name="source">Source to submit data to</param>
        /// <remarks>Does no looping or anything, but it is possible to set that all up.</remarks>
        public void StartPlayback(Int32 source)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference srcVoice = this.sourceVoices[source];

                //error checking
                if (srcVoice == null)
                    throw new ArgumentException(String.Format("The instance of the unique identifier for the source Voice {0} was null", source));

                if (srcVoice.Reference == null)
                    throw new ArgumentException(String.Format("The instance referenced by the unique identifier for the source Voice {0} was null", source));

                SourceVoice srcVoiceInstance = (srcVoice.Reference as SourceVoice);
                srcVoiceInstance.Start();
            }
        }

        /// <summary>Gets the state of the voice inquired about</summary>
        /// <param name="key">Key to query</param>
        /// <returns>A VoiceState instance, or null if any of the Source Voice references were null along the way</returns>
        public VoiceState GetSourceVoiceState(Int32 key)
        {
            /*
            if (srcVoice == null)
                throw new NullReferenceException("The reference to the XAudio2VoiceReference object was null");
            else if (srcVoice.Reference == null)
                throw new NullReferenceException("The reference within the XAudio2VoiceReference object for the Voice was null");
            */

            VoiceState state = null;
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference srcVoice = this.sourceVoices[key];
                if (srcVoice != null && srcVoice.Reference != null)
                    state = (srcVoice.Reference as SourceVoice).GetState();
            }
            return state;
        }

        #region Callbacks
        /// <summary>Callback for when playback finishes on a buffer.</summary>
        /// <param name="context">IntPtr that contains an Int32, the meaning of which is the index to the Voice in question being played back</param>
        internal void OnBufferEnd(IntPtr context)
        {
            Int32 voiceIndex = context.ToInt32();
            XAudio2VoiceReference reference = this.sourceVoices[voiceIndex];

            if (reference != null)
            {
                SourceVoice voice = reference.Reference as SourceVoice;
                if (voice == null)
                    throw new InvalidCastException(String.Format("The expected voice was not of the type SourceVoice, but rather {0}.", reference.Reference.GetType().ToString()));

                //Boolean isRunning = true;

                //make sure that the voice is finished. I think this is going to be unnecessary, but I copied it from examples and will leave it for the time being.
                //while (isRunning)
                //{
                //    VoiceState state = voice.GetState();

                //    //break out of everything, since the next buffer will clear everything out
                //    if (state.BuffersQueued > 1)
                //        return;

                //    isRunning = (state.BuffersQueued > 0);
                //    Thread.Sleep(10);
                //}

                //now we are certain it is done.
                VoiceState state = voice.GetState();
                if (state.BuffersQueued == 0 && (reference.State == XAudio2VoiceState.Depleted || reference.State == XAudio2VoiceState.InUseEnding))
                    this.DisposeSourceVoice(voiceIndex);
            }
        }
        #endregion

        /// <summary>Disposes of a source voice</summary>
        /// <param name="voiceIndex">Index of the voice to dispose of</param>
        protected void DisposeSourceVoice(Int32 voiceIndex)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2VoiceReference voiceReference = this.sourceVoices[voiceIndex];

                if (voiceReference != null)
                {
                    SourceVoice voice = voiceReference.Reference as SourceVoice;

                    if (voiceReference.State == XAudio2VoiceState.InUseEnding)
                    {
                        voiceReference.State = XAudio2VoiceState.Depleted;
                        voice.Dispose();                        //we are done with the voice
                        voiceReference.Reference = null;        //we are done with the instance.
                        this.sourceVoices[voiceIndex] = null;   //The index must persist, so as to not ruin other registered Voices, but is disposed, so set the reference to null
                    }
                }
            }
        }
    }
}