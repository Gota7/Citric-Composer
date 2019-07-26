using CitraFileLoader;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citric_Composer {

    /// <summary>
    /// Play entries.
    /// </summary>
    public static class EntryPlayer {

        /// <summary>
        /// If paused.
        /// </summary>
        public static bool Paused;

        /// <summary>
        /// Sound is playing.
        /// </summary>
        public static bool Playing => (!Stopped && !Paused);

        /// <summary>
        /// If stopped.
        /// </summary>
        public static bool Stopped = true;

        /// <summary>
        /// Play mode.
        /// </summary>
        public static EPlayMode PlayMode;

        /// <summary>
        /// Base path.
        /// </summary>
        public static string BasePath;

        /// <summary>
        /// Current hash for playback.
        /// </summary>
        public static string CurrHash = "";

        /// <summary>
        /// Kill switch.
        /// </summary>
        public static bool Kill = false;

        /// <summary>
        /// Wave out.
        /// </summary>
        private static WaveOut waveOut = new WaveOut();

        /// <summary>
        /// Loop start.
        /// </summary>
        private static long loopStart;

        /// <summary>
        /// Loop end.
        /// </summary>
        private static long loopEnd;

        /// <summary>
        /// Play mode.
        /// </summary>
        public enum EPlayMode {
            Once, Loop, Next
        }

        /// <summary>
        /// Initialize the player.
        /// </summary>
        public static void Initialize() {
            Thread t = new Thread(Loop);
            t.Start();
        }

        /// <summary>
        /// Play a sequence.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="sequenceNumber">Sequence number to play.</param>
        public static void PlaySequence(SoundArchive a, int sequenceNumber) {

            //Set info.
            Paused = false;
            Stopped = false;

            MessageBox.Show("Not implemented yet, sowwy. :{");

        }

        /// <summary>
        /// Play a WSD entry.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="wsdNumber">WSD entry to play.</param>
        public static void PlayWsd(SoundArchive a, int wsdNumber) {

            //Try playing.
            Stop();
            try {

                //WSD.
                WaveSoundData s = a.WaveSoundDatas[wsdNumber].File.File as WaveSoundData;
                var p = s.Waves[s.DataItems[a.WaveSoundDatas[wsdNumber].WaveIndex].Notes[0].WaveIndex];
                Wave w = (a.WaveArchives[p.WarIndex].File.File as SoundWaveArchive)[p.WaveIndex];
                if (w.Wav.info.channelInfo.Count > 2) {
                    switch (w.Wav.info.encoding) {
                        case 0:
                            w.Wav.data.pcm8 = new sbyte[][] { w.Wav.data.pcm8[0], w.Wav.data.pcm8[1] };
                            break;
                        case 1:
                            w.Wav.data.pcm16 = new short[][] { w.Wav.data.pcm16[0], w.Wav.data.pcm16[1] };
                            break;
                        case 2:
                            w.Wav.data.dspAdpcm = new byte[][] { w.Wav.data.dspAdpcm[0], w.Wav.data.dspAdpcm[1] };
                            break;
                    }
                }
                for (int i = 2; i < w.Wav.info.channelInfo.Count; i++) {
                    w.Wav.info.channelInfo.RemoveAt(i);
                }
                var m = new MemoryStream(w.Riff.ToBytes());
                var n = new WaveFileReader(m);
                waveOut.Initialize(n);
                waveOut.Play();
                CurrHash = "WSD_" + wsdNumber;
                Stopped = false;
                Paused = false;

            } catch { }

        }

        /// <summary>
        /// Play a stream entry.
        /// </summary>
        /// <param name="a">Sound archive.</param>
        /// <param name="streamNumber">Stream number to play.</param>
        public static void PlayStream(SoundArchive a, int streamNumber) {

            //Try playing.
            Stop();
            try {

                //Stream.
                CitraFileLoader.Stream s = (CitraFileLoader.Stream)SoundArchiveReader.ReadFile(File.ReadAllBytes(BasePath + "/" + a.Streams[streamNumber].File.ExternalFileName));
                if (s.Stm.info.channels.Count > 2) {
                    switch (s.Stm.info.streamSoundInfo.encoding) {
                        case 0:
                            s.Stm.data.pcm8 = new sbyte[][] { s.Stm.data.pcm8[0], s.Stm.data.pcm8[1] };
                            break;
                        case 1:
                            s.Stm.data.pcm16 = new short[][] { s.Stm.data.pcm16[0], s.Stm.data.pcm16[1] };
                            break;
                        case 2:
                            s.Stm.data.dspAdpcm = new byte[][] { s.Stm.data.dspAdpcm[0], s.Stm.data.dspAdpcm[1] };
                            break;
                    }
                }
                for (int i = 2; i < s.Stm.info.channels.Count; i++) {
                    s.Stm.info.channels.RemoveAt(i);
                }
                var m = new MemoryStream(s.Riff.ToBytes());
                var n = new WaveFileReader(m);
                waveOut.Initialize(n);
                waveOut.Play();
                CurrHash = "STM_" + streamNumber;
                Stopped = false;
                Paused = false;

            } catch { }

        }

        /// <summary>
        /// Pause the player.
        /// </summary>
        public static void Pause() {

            //Set info.
            waveOut.Pause();
            Paused = true;
            Stopped = false;

        }

        /// <summary>
        /// Resume playback.
        /// </summary>
        public static void Resume() {

            //Set info.
            waveOut.Resume();
            Paused = false;
            Stopped = false;

        }

        /// <summary>
        /// Stop the player.
        /// </summary>
        public static void Stop() {

            //Set info.
            Paused = false;
            Stopped = true;
            waveOut.Stop();

        }

        /// <summary>
        /// Main loop.
        /// </summary>
        private static void Loop() {

            //While until killed.
            while (!Kill) {

                //Sleep.
                Thread.Sleep(100);

            }

        }

    }

}
