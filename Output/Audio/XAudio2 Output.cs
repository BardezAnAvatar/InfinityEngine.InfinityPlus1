using System;
using System.Collections.Generic;
using System.Threading;

using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.Win32.Audio;
using Bardez.Projects.InfinityPlus1.Output.Audio.Enums;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>Program interface class for XAUdio2</summary>
    /// <remarks>
    ///     Your application MUST explicitly call dispose or else the objects referenced by XAudio2 could dispose out of order,
    ///     simultaneously, etc. I may work on a different disposal approach in the future, but for now, Dispose of this object
    ///     if ever referenced.
    /// </remarks>
    public class XAudio2Output : AudioOutput, IDisposable
    {
        #region Fields
        /// <summary>Private lock not accessable by other memory</summary>
        /// <remarks>Static instantiation</remarks>
        private static Object singletonLock = new Object();

        /// <summary>Singleton instance of XAudio2Output</summary>
        private static XAudio2Output singleton = null;

        /// <summary>XAudio2 constant indicating that an action is to be immediate</summary>
        private const UInt32 CommitNow = 0U;

        /// <summary>XAudio2 constant indicating that the stream is expected to close after this buffer is finished.</summary>
        private const UInt32 EndOfStream = 0x0040U;

        /// <summary>List of voices that can be re-referenced by index</summary>
        private List<XAudio2SourceVoiceReference<SourceVoice>> sourceVoices;

        /// <summary>List of voices that can be re-referenced by index</summary>
        private List<XAudio2VoiceReference<Voice>> destinationVoices;

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
                XAudio2SourceVoiceReference<SourceVoice> voiceReference = this.sourceVoices[voiceIndex];
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
            this.sourceVoices = new List<XAudio2SourceVoiceReference<SourceVoice>>();
            this.destinationVoices = new List<XAudio2VoiceReference<Voice>>();
            this.XAudio2 = XAudio2Interface.NewInstance();

            //also, register an output MasteringVoice, since to use it you will almost certainly always need one.
            //Also, you can be lame and assume that index 0 will always be a valid Mastering Voice

            //this should probably use the default system rendering, and submisions submit sample rate transformations as needed, so void list of parameters
            MasteringVoice masteringVoice = this.XAudio2.CreateMasteringVoice();
            this.destinationVoices.Add(new XAudio2VoiceReference<Voice>(masteringVoice));
        }

        /// <summary>Disposal code</summary>
        public void Dispose()
        {
            lock (XAudio2Output.singletonLock)  //do not attempt to dispose more than once
            {
                if (this.sourceVoices != null)
                    for (Int32 i = 0; i < this.sourceVoices.Count; ++i)
                    {
                        if (this.sourceVoices[i] != null)
                        {
                            this.sourceVoices[i].Dispose();
                            this.sourceVoices[i] = null;
                        }
                    }

                //dispose of submix voices, then mastering voices
                this.DisposeVoiceType(this.destinationVoices, typeof(SubmixVoice));
                this.DisposeVoiceType(this.destinationVoices, typeof(MasteringVoice));

                if (this.XAudio2 != null)
                {
                    this.XAudio2.Dispose();
                    this.XAudio2 = null;
                }
            }
        }

        /// <summary>Disposal</summary>
        /// <remarks>Finalize()</remarks>
        ~XAudio2Output()
        {
            this.Dispose();
        }

        /// <summary>Disposes of a voice set (Source, Master, Submix)</summary>
        /// <param name="voices">List of voices to Dispose</param>
        /// <param name="voiceType">Type of voice to dispose</param>
        internal void DisposeVoiceType(List<XAudio2VoiceReference<Voice>> voices, Type voiceType)
        {
            if (voices != null)
            {
                for (Int32 i = 0; i < voices.Count; ++i)
                {
                    if (voices[i] != null && voices[i].ReferenceIsOfType(voiceType))
                    {
                        voices[i].Dispose();
                        voices[i] = null;
                    }
                }
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
                VoiceCallback callback = new VoiceCallback();

                SourceVoice voice = this.XAudio2.CreateSourceVoice(inputData, 0U, 2.0f, callback);
                sourceIndex = this.sourceVoices.Count;
                this.sourceVoices.Add(new XAudio2SourceVoiceReference<SourceVoice>(voice));
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
                XAudio2SourceVoiceReference<SourceVoice> srcVoice = this.sourceVoices[source];
                XAudio2VoiceReference<Voice> dstVoice = this.destinationVoices[destination];

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
                XAudio2SourceVoiceReference<SourceVoice> srcVoice = this.sourceVoices[source];
                XAudio2VoiceReference<Voice> dstVoice = this.destinationVoices[destination];

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
                XAudio2SourceVoiceReference<SourceVoice> srcVoice = this.sourceVoices[source];

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
                XAudio2SourceVoiceReference<SourceVoice> srcVoice = this.sourceVoices[source];

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
            VoiceState state = null;
            lock (XAudio2Output.singletonLock)
            {
                XAudio2SourceVoiceReference<SourceVoice> srcVoice = this.sourceVoices[key];
                if (srcVoice != null && srcVoice.Reference != null)
                    state = (srcVoice.Reference as SourceVoice).GetState();
            }
            return state;
        }

        #region Callback Passthrough
        /// <summary>Adds an event handler to request further data</summary>
        /// <param name="sourcKey">Source key of object to add data to</param>
        /// <param name="handler">Delegate to add more data</param>
        public override void AddSourceNeedDataEventhandler(Int32 sourcKey, AudioNeedsMoreDataHandler handler)
        {
            this.sourceVoices[sourcKey].NeedMoreSampleData += handler;
        }
        #endregion

        /// <summary>Disposes of a source voice</summary>
        /// <param name="voiceIndex">Index of the voice to dispose of</param>
        protected void DisposeSourceVoice(Int32 voiceIndex)
        {
            lock (XAudio2Output.singletonLock)
            {
                XAudio2SourceVoiceReference<SourceVoice> voiceReference = this.sourceVoices[voiceIndex];

                if (voiceReference != null)
                {
                    SourceVoice voice = voiceReference.Reference as SourceVoice;

                    if (voiceReference.State == XAudio2VoiceState.InUseEnding)
                    {
                        voiceReference.State = XAudio2VoiceState.Depleted;
                        voice.Dispose();                        //we are done with the voice
                        this.sourceVoices[voiceIndex] = null;   //The index must persist, so as to not ruin other registered Voices, but is disposed, so set the reference to null
                    }
                }
            }
        }
    }
}