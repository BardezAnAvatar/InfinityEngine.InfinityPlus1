using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Bardez.Projects.Configuration;
using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Music;
using Bardez.Projects.InfinityPlus1.Output.Audio;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Utility.UiInterceptor;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Test.Music
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Infinity.Music.Playlist class.</summary>
    public class PlaylistTest : ITester
    {
        protected XAudio2Output output;
        protected Playlist playlist;
        protected Int32 outputSoundKey;

        /// <summary>Main test entry point</summary>
        public void Test()
        {
            this.output = XAudio2Output.Instance;

            String[] paths = ConfigurationHandlerMulti.GetSettingValues("Test.MUS.Path").ToArray();
            //String path = ConfigurationHandlerMulti.GetSettingValue("Test.ACM.AcmPath");
            //this.Test(path, false);
            this.TestMulti(paths);

            //dispose
            this.output.Dispose();
        }

        /// <summary>Tests the code </summary>
        /// <param name="paths">Paths of fils to test</param>
        public void TestMulti(String[] paths)
        {
            foreach (String path in paths)
                this.Test(path, false);
        }

        /// <summary>Tests a single file</summary>
        /// <param name="path">File to open and read, then replicate</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt between read and write</param>
        public void Test(String path, Boolean prompt)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                this.Test(stream, prompt);

            this.playlist.RootFilePath = new FileInfo(path).DirectoryName;
            this.TestPlayback();

            //using (FileStream dest = new FileStream(path + ".rewrite", FileMode.Create, FileAccess.Write))
            //    this.TestWrite(dest);
        }

        /// <summary>Tests the read and ToString() methods of the structure</summary>
        /// <param name="source">Source Stream to read from</param>
        /// <param name="prompt">Boolean indicating whether or not to prompt for pressing [Enter] to continue</param>
        public void Test(Stream source, Boolean prompt)
        {
            this.playlist = new Playlist();
            Console.WriteLine("Reading playlist...");
            this.playlist.Read(source);
            Console.WriteLine("Finished reading playlist...");

            Interceptor.WriteMessage(this.playlist.ToString());

            if (prompt)
            {
                Interceptor.WaitForInput();
            }
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        public void TestWrite(Stream destination)
        {
            this.playlist.Write(destination);
        }

        /// <summary>Tests audio playback, allowing for an interrupt to occur by hitting enter.</summary>
        public void TestPlayback()
        {
            Console.WriteLine("Decoding playlist files...");
            this.playlist.ReadPlayListItems();  //tell it to load everything
            Console.WriteLine("Finished decoding...");

            this.output = XAudio2Output.Instance;

            //load up the initial Source voice
            this.outputSoundKey = output.CreatePlayback(this.playlist.WaveFormat);

            //Adjust callback(s)
            this.output.AddSourceNeedDataEventhandler(this.outputSoundKey, new AudioNeedsMoreDataHandler(this.NeedsMoreSamples));

            //submit first data
            this.NeedsMoreSamples();

            //play audio & Let the sound play
            this.output.StartPlayback(this.outputSoundKey);

            Interceptor.WaitForInput("Press [Enter]/[OK] to interrupt and terminate playback...");
            
            this.playlist.Interrupt();
            Console.WriteLine("Playback interrupted...");

            //wait until finished.
            this.WaitUntilFinished();

            Console.WriteLine("Playback completed.");
        }
        
        /// <summary>Callback event handler that sends more data to the output buffer</summary>
        public void NeedsMoreSamples()
        {
            Byte[] samples = this.playlist.GetNext();
            this.output.SubmitSubsequentData(samples, this.outputSoundKey, !this.playlist.Interrupted);
        }

        /// <summary>Waits for the playlist to finish playback buffer</summary>
        protected void WaitUntilFinished()
        {
            //play audio & Let the sound play
            Boolean isRunning = true;
            while (isRunning)
            {
                VoiceState state = output.GetSourceVoiceState(this.outputSoundKey);
                isRunning = (state != null) && (state.BuffersQueued > 0);
                Thread.Sleep(10);
            }
        }
    }
}