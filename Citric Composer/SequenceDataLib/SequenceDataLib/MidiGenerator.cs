using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace SequenceDataLib {

    /// <summary>
    /// Midi manager.
    /// </summary>
    public static class MidiGenerator {

        /// <summary>
        /// Ticks per MID quarter note.
        /// </summary>
        public const int TICKS_PER_MID_QNOTE = 480;

        /// <summary>
        /// Ticks per sequence quarter note.
        /// </summary>
        public const int TICKS_PER_SEQ_QNOTE = 48;

        /// <summary>
        /// Convert MID to sequence ticks.
        /// </summary>
        /// <param name="numTicks">Number of MID ticks.</param>
        /// <returns>Number of sequence ticks.</returns>
        public static int Mid2SeqTicks(int numTicks) {
            return numTicks / 10;
        }

        /// <summary>
        /// Convert sequence to MID ticks.
        /// </summary>
        /// <param name="numTicks">Number of sequence ticks.</param>
        /// <returns>Number of sequence ticks.</returns>
        public static long Seq2MidTicks(uint numTicks) {
            return numTicks * 10;
        }

        /// <summary>
        /// Convert a sequence to an MIDI.
        /// </summary>
        /// <param name="d">Sequence data.</param>
        /// <returns>An midi file.</returns>
        public static MidiFile Sequence2Midi(SequenceData d) {

            //New midi file.
            MidiFile m = new MidiFile();
            m.Chunks.Add(new TrackChunk());

            //Read each command.
            int nextWaitAmount = 0;
            foreach (SequenceCommand c in d.Commands) {
                TrackChunk currTrack = (TrackChunk)m.Chunks[m.Chunks.Count - 1];
                SequenceCommand2MidiCommand(c, m, currTrack, ref nextWaitAmount);
            }

            //Return the file.
            return m;

        }

        /// <summary>
        /// Convert the sequence command to an MIDI one.
        /// </summary>
        /// <param name="c">The sequence command.</param>
        /// <param name="m">The out MIDI file.</param>
        /// <param name="t"></param>
        /// <param name="nextWaitAmount">Time to wait for the next command.</param>
        public static void SequenceCommand2MidiCommand(SequenceCommand c, MidiFile m, TrackChunk t, ref int nextWaitAmount) {

            //Switch type.
            switch ((CommandType)c.Identifier) {

                //Note.
                case CommandType commandType when (c.Identifier >= 0 && c.Identifier <= 0x7F):
                    t.Events.Add(new NoteOnEvent(c.Identifier.To7Bit(), (c as NoteCommand).Velocity.To7Bit()) { DeltaTime = nextWaitAmount });
                    t.Events.Add(new NoteOffEvent(c.Identifier.To7Bit(), (c as NoteCommand).Velocity.To7Bit()) { DeltaTime = Seq2MidTicks((c as NoteCommand).Length) });
                    break;

                //Wait.
                case CommandType.Wait:
                    nextWaitAmount = (int)(c as WaitCommand).Length;
                    break;

            }

            //Wait time is now 0.
            if ((CommandType)c.Identifier != CommandType.Wait) {
                nextWaitAmount = 0;
            }

        }

    }

}
