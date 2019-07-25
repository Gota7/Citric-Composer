using SequenceDataLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SequenceEmulation {

    /// <summary>
    /// Sequence Parser.
    /// </summary>
    public static class SequenceParser {

        /// <summary>
        /// A random random.
        /// </summary>
        private static Random random = new Random();

        public static void ExecuteCommand(SequenceCommand c, Track t, SequencePlayer p) {

            //Note command.
            if (c.Identifier >= 0 && c.Identifier <= 127) {

                //Play note.
                

            }

            //Switch the command type.
            switch ((CommandType)c.Identifier) {

                //Wait the provided amount of ticks.
                case CommandType.Wait:
                    Thread.Sleep(TickTime.Ticks2Milliseconds((c as WaitCommand).Length));
                    break;

                //Program change.
                case CommandType.ProgramChange:
                    t.InstrumentNumber = (int)(c as ProgramChangeCommand).Program;
                    break;

                //Open track.
                case CommandType.OpenTrack:
                    OpenTrackCommand openTrackCommand = (c as OpenTrackCommand);
                    p.TracksOpen[openTrackCommand.TrackNumber] = true;
                    p.Tracks[openTrackCommand.TrackNumber] = new Track(p.Tracks[0]);
                    p.Tracks[openTrackCommand.TrackNumber].Offset = (int)openTrackCommand.Offset.Value;
                    break;

                //Jump.
                case CommandType.Jump:
                    t.Offset = (int)(c as JumpCommand).Offset.Value;
                    break;

                //Call.
                case CommandType.Call:
                    t.ReturnToOffsets.Push(t.Offset + 1);
                    t.Offset = (int)(c as CallCommand).Offset.Value;
                    break;

                //Time base.
                case CommandType.TimeBase:
                    t.TimeBase = (c as TimeBaseCommand).QuarterNoteLength;
                    break;

                //Hold.
                case CommandType.Hold:
                    t.Hold = (c as HoldCommand).Hold;
                    break;

                //Monophonic.
                case CommandType.Monophonic:
                    t.Monophonic = (c as MonophonicCommand).Monophonic;
                    break;

                //Velocity range.
                case CommandType.VelocityRange:
                    t.VelocityRange = (c as VelocityRangeCommand).VelocityRange;
                    break;

                //Biquad type.
                case CommandType.BiquadType:
                    t.BiquadType = (c as BiquadTypeCommand).BiquadType;
                    break;

                //Biquad value.
                case CommandType.BiquadValue:
                    t.BiquadValue = (c as BiquadValueCommand).BiquadValue;
                    break;

                //Bank select.
                case CommandType.BankSelect:
                    t.BankNumber = (c as BankSelectCommand).BankNumber;
                    break;

                //TODO!

                //Sweep pitch.

                //Mod period.

                //Extended command.
                case CommandType.Extended:
                    ExecuteExtendedCommand((c as ExtendedCommand).SequenceCommand, t, p);
                    break;

                //Envelope reset. TODO!

                //Loop end. TODO!

                //Return from a call.
                case CommandType.Return:
                    t.Offset = t.ReturnToOffsets.Pop();
                    break;

                //Allocate track does nothing.

                //Fin. Close the track.
                case CommandType.Fin:
                    int trackIndexToClose = Array.IndexOf(p.Tracks, t);
                    p.TracksOpen[trackIndexToClose] = false;
                    break;

                //Prefix commands are to do later.

            }

            //Increment the command number, if not moving.
            if ((c as OpenTrackCommand != null) || (c as JumpCommand != null) || (c as CallCommand != null)) {
                t.Offset++;
            }

        }

        public static void ExecuteExtendedCommand(SequenceCommand c, Track t, SequencePlayer p) {

            //Switch the extended command type.
            switch ((ExtendedCommandType)c.Identifier) {

                //Set var.
                case ExtendedCommandType.SetVar:
                    SetVar((c as SetVarCommand).Value, (c as SetVarCommand).Variable, t, p);
                    break;

                //Add var.
                case ExtendedCommandType.AddVar:
                    SetVar((short)(GetVar((c as AddVarCommand).Variable, t, p) + (c as AddVarCommand).Value), (c as AddVarCommand).Variable, t, p);
                    break;

                //Sub var.
                case ExtendedCommandType.SubtractVar:
                    SetVar((short)(GetVar((c as SubtractVarCommand).Variable, t, p) - (c as SubtractVarCommand).Value), (c as SubtractVarCommand).Variable, t, p);
                    break;

                //Multiply var.
                case ExtendedCommandType.MultiplyVar:
                    SetVar((short)(GetVar((c as MultiplyVarCommand).Variable, t, p) * (c as MultiplyVarCommand).Value), (c as MultiplyVarCommand).Variable, t, p);
                    break;

                //Divide var.
                case ExtendedCommandType.DivideVar:
                    SetVar((short)(GetVar((c as DivideVarCommand).Variable, t, p) / (c as DivideVarCommand).Value), (c as DivideVarCommand).Variable, t, p);
                    break;

                //Shift var.
                case ExtendedCommandType.ShiftVar:
                    if ((c as ShiftVarCommand).Value > 0) {
                        SetVar((short)(GetVar((c as ShiftVarCommand).Variable, t, p) << (c as ShiftVarCommand).Value), (c as ShiftVarCommand).Variable, t, p);
                    } else {
                        SetVar((short)(GetVar((c as ShiftVarCommand).Variable, t, p) >> Math.Abs((c as ShiftVarCommand).Value)), (c as ShiftVarCommand).Variable, t, p);
                    }
                    break;

                //Random var.
                case ExtendedCommandType.RandomVar:
                    if ((c as RandomVarCommand).Value >= 0) {
                        SetVar((short)random.Next(0, (c as RandomVarCommand).Value), (c as RandomVarCommand).Variable, t, p);
                    } else {
                        SetVar((short)random.Next((c as RandomVarCommand).Value, 0), (c as RandomVarCommand).Variable, t, p);
                    }
                    break;

                //And var.
                case ExtendedCommandType.AndVar:
                    SetVar((short)(GetVar((c as AndVarCommand).Variable, t, p) & (c as AndVarCommand).Value), (c as AndVarCommand).Variable, t, p);
                    break;

                //Or var.
                case ExtendedCommandType.OrVar:
                    SetVar((short)(GetVar((c as OrVarCommand).Variable, t, p) | (c as OrVarCommand).Value), (c as OrVarCommand).Variable, t, p);
                    break;

                //Xor var.
                case ExtendedCommandType.XorVar:
                    SetVar((short)(GetVar((c as XorVarCommand).Variable, t, p) ^ (c as XorVarCommand).Value), (c as XorVarCommand).Variable, t, p);
                    break;

                //Not var.
                case ExtendedCommandType.NotVar:
                    SetVar((short)~(GetVar((c as NotVarCommand).Variable, t, p) & (c as NotVarCommand).Value), (c as NotVarCommand).Variable, t, p);
                    break;

                //Mod var.
                case ExtendedCommandType.ModVar:
                    SetVar((short)(GetVar((c as ModVarCommand).Variable, t, p) % (c as ModVarCommand).Value), (c as ModVarCommand).Variable, t, p);
                    break;

                //Compare equal.
                case ExtendedCommandType.CompareEqual:
                    t.ConditionFlag = GetVar((c as CompareEqualCommand).Variable, t, p) == (c as CompareEqualCommand).Value;
                    break;

                //Compare greater than or equal to.
                case ExtendedCommandType.CompareGreaterThanOrEqualTo:
                    t.ConditionFlag = GetVar((c as CompareGreaterThanOrEqualToCommand).Variable, t, p) >= (c as CompareGreaterThanOrEqualToCommand).Value;
                    break;
                    
                //Compare greater than.
                case ExtendedCommandType.CompareGreaterThan:
                    t.ConditionFlag = GetVar((c as CompareGreaterThanCommand).Variable, t, p) > (c as CompareGreaterThanCommand).Value;
                    break;

                //Compare less than or equal to.
                case ExtendedCommandType.CompareLessThanOrEqualTo:
                    t.ConditionFlag = GetVar((c as CompareLessThanOrEqualToCommand).Variable, t, p) <= (c as CompareLessThanOrEqualToCommand).Value;
                    break;

                //Compare less than.
                case ExtendedCommandType.CompareLessThan:
                    t.ConditionFlag = GetVar((c as CompareLessThanCommand).Variable, t, p) < (c as CompareLessThanCommand).Value;
                    break;

                //Compare not equal.
                case ExtendedCommandType.CompareNotEqual:
                    t.ConditionFlag = GetVar((c as CompareNotEqualCommand).Variable, t, p) != (c as CompareNotEqualCommand).Value;
                    break;

                //TODO: MOD STUFF!!!

            }

        }

        /// <summary>
        /// Get a variable.
        /// </summary>
        /// <param name="varNum">Var number.</param>
        /// <param name="t">Track.</param>
        /// <param name="p">Sequence player.</param>
        /// <returns>The variable.</returns>
        public static short GetVar(int varNum, Track t, SequencePlayer p) {

            //0-15 are local.
            if (varNum >= 0 && varNum <= 15) {
                return t.Variables[varNum];
            }

            //16-31 are global.
            else if (varNum >= 16 && varNum <= 31) {
                return p.GlobalVariables[varNum - 16];
            }

            //32-47 are sequence.
            else if (varNum >= 32 && varNum <= 47) {
                return p.SequenceVariables[varNum - 32];
            }

            //Null?
            else {
                return -1;
            }

        }

        /// <summary>
        /// Set a variable.
        /// </summary>
        /// <param name="value">Value to change variable to.</param>
        /// <param name="varNum">Var number.</param>
        /// <param name="t">Track.</param>
        /// <param name="p">Sequence player.</param>
        public static void SetVar(short value, int varNum, Track t, SequencePlayer p) {

            //0-15 are local.
            if (varNum >= 0 && varNum <= 15) {
                t.Variables[varNum] = value;
            }

            //16-31 are global.
            else if (varNum >= 16 && varNum <= 31) {
                p.GlobalVariables[varNum - 16] = value;
            }

            //32-47 are sequence.
            else if (varNum >= 32 && varNum <= 47) {
                p.SequenceVariables[varNum - 32] = value;
            }

        }

    }

}
