using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// For conversion.
    /// </summary>
    public abstract partial class SequenceCommand {

        /// <summary>
        /// Convert the command to a sequence string.
        /// </summary>
        /// <param name="publicLabels">Public labels.</param>
        /// <returns>The command as a string.</returns>
        public string ToSeqString(Dictionary<string, int> publicLabels) {

            //Switch the command type.
            switch ((CommandType)Identifier) {

                //Note.
                case CommandType commandType when(Identifier >= 0 && Identifier <= 0x7F):
                    return ((Notes)(this as NoteCommand).Identifier).ToString() + " " + (this as NoteCommand).Velocity + ", " + (this as NoteCommand).Length;

                //Wait.
                case CommandType.Wait:
                    return "wait\t" + (this as WaitCommand).Length;

                //Program change.
                case CommandType.ProgramChange:
                    return "prg " + (this as ProgramChangeCommand).Program;

                //Open track.
                case CommandType.OpenTrack:
                    string lbl0 = "_label_" + (this as OpenTrackCommand).Offset;
                    for (int i = 0; i < publicLabels.Values.Count; i++) {
                        if (publicLabels.Values.ElementAt(i) == (int)(this as OpenTrackCommand).Offset.Value) {
                            lbl0 = publicLabels.Keys.ElementAt(i);
                        }
                    }
                    return "opentrack\t" + (this as OpenTrackCommand).TrackNumber + ", " + lbl0;

                //Jump.
                case CommandType.Jump:
                    string lbl1 = "_label_" + (this as JumpCommand).Offset;
                    for (int i = 0; i < publicLabels.Values.Count; i++) {
                        if (publicLabels.Values.ElementAt(i) == (int)(this as JumpCommand).Offset.Value) {
                            lbl1 = publicLabels.Keys.ElementAt(i);
                        }
                    }
                    return "jump\t" + lbl1;

                //Call.
                case CommandType.Call:
                    string lbl2 = "_label_" + (this as CallCommand).Offset;
                    for (int i = 0; i < publicLabels.Values.Count; i++) {
                        if (publicLabels.Values.ElementAt(i) == (int)(this as CallCommand).Offset.Value) {
                            lbl2 = publicLabels.Keys.ElementAt(i);
                        }
                    }
                    return "call\t" + lbl2;

                //Prefix command.
                case CommandType.Random:
                case CommandType.Variable:
                case CommandType.If:
                case CommandType.TimeChange:
                case CommandType.TimeRandom:
                case CommandType.TimeVariable:
                    return PrefixToSeqString(this, publicLabels);

                //Time base.
                case CommandType.TimeBase:
                    return "timebase " + (this as TimeBaseCommand).QuarterNoteLength;

                //Hold.
                case CommandType.Hold:
                    return "env_hold " + (this as HoldCommand).Hold;

                //Monophonic.
                case CommandType.Monophonic:
                    return (this as MonophonicCommand).Monophonic ? "monophonic_on" : "monophonic_off";

                //Velocity range.
                case CommandType.VelocityRange:
                    return "velocity_range " + (this as VelocityRangeCommand).VelocityRange;

                //Biquad type.
                case CommandType.BiquadType:
                    return "biquad_type " + (this as BiquadTypeCommand).BiquadType;

                //Biquad value.
                case CommandType.BiquadValue:
                    return "biquad_value " + (this as BiquadValueCommand).BiquadValue;

                //Bank select.
                case CommandType.BankSelect:
                    return "bank_select " + (this as BankSelectCommand).BankNumber;

                //Mod phrase.
                case CommandType.ModPhase:
                    return "mod_phase " + (this as ModPhaseCommand).Phase;

                //Mod curve.
                case CommandType.ModCurve:
                    return "mod_curve " + (this as ModCurveCommand).Curve;

                //Front bypass.
                case CommandType.FrontBypass:
                    return (this as FrontBypassCommand).FrontBypass ? "front_bypass_on" : "front_bypass_off";

                //Pan.
                case CommandType.Pan:
                    return "pan " + (this as PanCommand).TrackPan;

                //Volume.
                case CommandType.Volume:
                    return "volume\t" + (this as VolumeCommand).Volume;

                //Main volume.
                case CommandType.MainVolume:
                    return "main_volume " + (this as MainVolumeCommand).MainVolume;

                //Transpose.
                case CommandType.Transpose:
                    return "transpose " + (this as TransposeCommand).Transpose;

                //Pitch bend.
                case CommandType.PitchBend:
                    return "pitchbend " + (this as PitchBendCommand).PitchBend;

                //Bend range.
                case CommandType.BendRange:
                    return "bendrange " + (this as BendRangeCommand).BendRange;

                //Voicing priority.
                case CommandType.VoicingPriority:
                    return "prio " + (this as VoicingPriorityCommand).Priority;

                //Note wait.
                case CommandType.NoteWait:
                    return (this as NoteWaitCommand).NoteWaitMode ? "notewait_on" : "notewait_off";

                //Tie.
                case CommandType.Tie:
                    return (this as TieCommand).TieMode ? "tieon" : "tieoff";

                //Portamento.
                case CommandType.Portamento:
                    return "porta " + (this as PortamentoCommand).Portamento;

                //Mod depth.
                case CommandType.ModDepth:
                    return "mod_depth " + (this as ModDepthCommand).Depth;

                //Mod speed.
                case CommandType.ModSpeed:
                    return "mod_speed " + (this as ModSpeedCommand).Speed;

                //Mod type.
                case CommandType.ModType:
                    return "mod_type " + (byte)(this as ModTypeCommand).Type;

                //Mod range.
                case CommandType.ModRange:
                    return "mod_range " + (this as ModRangeCommand).Range;

                //Portamento enabled.
                case CommandType.PortamentoEnabled:
                    return (this as PortamentoEnabledCommand).PortamentoEnabled ? "porta_on" : "porta_off";

                //Portamento time.
                case CommandType.PortamentoTime:
                    return "porta_time " + (this as PortamentoTimeCommand).PortamentoTime;

                //Attack.
                case CommandType.Attack:
                    return "attack " + (this as AttackCommand).Attack;

                //Decay.
                case CommandType.Decay:
                    return "decay " + (this as DecayCommand).Decay;

                //Sustain.
                case CommandType.Sustain:
                    return "sustain " + (this as SustainCommand).Sustain;

                //Release.
                case CommandType.Release:
                    return "release " + (this as ReleaseCommand).Release;

                //Loop start.
                case CommandType.LoopStart:
                    return "loop_start " + (this as LoopStartCommand).Count;

                //Volume 2.
                case CommandType.Volume2:
                    return "volume2 " + (this as Volume2Command).Volume2;

                //Print var.
                case CommandType.PrintVar:
                    return "printvar " + (this as PrintVarCommand).Var;

                //Surround pan.
                case CommandType.SurroundPan:
                    return "span " + (this as SurroundPanCommand).TrackSurroundPan;

                //LPF cutoff.
                case CommandType.LPFCutoff:
                    return "lpf_cutoff " + (this as LPFCutoffCommand).LPFCutoff;

                //Fx send A.
                case CommandType.FxSendA:
                    return "fxsend_a " + (this as FxSendACommand).FxSendA;

                //Fx send B.
                case CommandType.FxSendB:
                    return "fxsend_b " + (this as FxSendBCommand).FxSendB;

                //Main send.
                case CommandType.MainSend:
                    return "mainsend" + (this as MainSendCommand).MainSend;

                //Initial pan.
                case CommandType.InitialPan:
                    return "init_pan " + (this as InitialPanCommand).NotePan;

                //Mute mode.
                case CommandType.MuteMode:
                    return "mute " + (this as MuteModeCommand).Mode;

                //Fx send C.
                case CommandType.FxSendC:
                    return "fxsend_c " + (this as FxSendCCommand).FxSendC;

                //Damper.
                case CommandType.Damper:
                    return (this as DamperCommand).Damper ? "damper_on" : "damper_off";

                //Mod delay.
                case CommandType.ModDelay:
                    return "mod_delay " + (this as ModDelayCommand).Delay;

                //Tempo change.
                case CommandType.TempoChange:
                    return "tempo\t" + (this as TempoChangeCommand).Tempo;

                //Sweep pitch.
                case CommandType.SweepPitch:
                    return "sweep_pitch " + (this as SweepPitchCommand).SweepPitch;

                //Mod period.
                case CommandType.ModPeriod:
                    return "mod_period " + (this as ModPeriodCommand).Period;

                //Extended command.
                case CommandType.Extended:
                    return ExtendedToSeqString((this as ExtendedCommand).SequenceCommand);

                //Envelope reset.
                case CommandType.EnvelopeReset:
                    return "env_reset";

                //Loop end.
                case CommandType.LoopEnd:
                    return "loop_end";

                //Return.
                case CommandType.Return:
                    return "ret";

                //Allocate track.
                case CommandType.AllocateTrack:
                    return "alloctrack 0b" + Convert.ToString((this as AllocateTrackCommand).AllocatedTracks, 2);

                //Fin.
                case CommandType.Fin:
                    return "fin";

            }

            //Last case.
            return "";

        }

        /// <summary>
        /// Return the string of an extended command.
        /// </summary>
        /// <param name="c">The extended command.</param>
        /// <returns>The extended command as a string,</returns>
        public string ExtendedToSeqString(SequenceCommand c) {

            //Switch the type.
            switch ((ExtendedCommandType)c.Identifier) {

                //Set var.
                case ExtendedCommandType.SetVar:
                    return "setvar " + (c as SetVarCommand).Variable + ", " + (c as SetVarCommand).Value;

                //Add var.
                case ExtendedCommandType.AddVar:
                    return "addvar " + (c as AddVarCommand).Variable + ", " + (c as AddVarCommand).Value;

                //Subtract var.
                case ExtendedCommandType.SubtractVar:
                    return "subvar " + (c as SubtractVarCommand).Variable + ", " + (c as SubtractVarCommand).Value;

                //Multiply var.
                case ExtendedCommandType.MultiplyVar:
                    return "mulvar " + (c as MultiplyVarCommand).Variable + ", " + (c as MultiplyVarCommand).Value;

                //Divide var.
                case ExtendedCommandType.DivideVar:
                    return "divvar " + (c as DivideVarCommand).Variable + ", " + (c as DivideVarCommand).Value;

                //Shift var.
                case ExtendedCommandType.ShiftVar:
                    return "shiftvar " + (c as ShiftVarCommand).Variable + ", " + (c as ShiftVarCommand).Value;

                //Random var.
                case ExtendedCommandType.RandomVar:
                    return "randvar " + (c as RandomVarCommand).Variable + ", " + (c as RandomVarCommand).Value;

                //And var.
                case ExtendedCommandType.AndVar:
                    return "andvar " + (c as AndVarCommand).Variable + ", " + (c as AndVarCommand).Value;

                //Or var.
                case ExtendedCommandType.OrVar:
                    return "orvar " + (c as OrVarCommand).Variable + ", " + (c as OrVarCommand).Value;

                //Xor var.
                case ExtendedCommandType.XorVar:
                    return "xorvar " + (c as XorVarCommand).Variable + ", " + (c as XorVarCommand).Value;

                //Not var.
                case ExtendedCommandType.NotVar:
                    return "notvar " + (c as NotVarCommand).Variable + ", " + (c as NotVarCommand).Value;

                //Mod var.
                case ExtendedCommandType.ModVar:
                    return "modvar " + (c as ModVarCommand).Variable + ", " + (c as ModVarCommand).Value;

                //Compare equal.
                case ExtendedCommandType.CompareEqual:
                    return "cmp_eq " + (c as CompareEqualCommand).Variable + ", " + (c as CompareEqualCommand).Value;

                //Compare greater than or equal to.
                case ExtendedCommandType.CompareGreaterThanOrEqualTo:
                    return "cmp_ge " + (c as CompareGreaterThanOrEqualToCommand).Variable + ", " + (c as CompareGreaterThanOrEqualToCommand).Value;

                //Compare greater than.
                case ExtendedCommandType.CompareGreaterThan:
                    return "cmp_gt " + (c as CompareGreaterThanCommand).Variable + ", " + (c as CompareGreaterThanCommand).Value;

                //Compare less than or equal to.
                case ExtendedCommandType.CompareLessThanOrEqualTo:
                    return "cmp_le " + (c as CompareLessThanOrEqualToCommand).Variable + ", " + (c as CompareLessThanOrEqualToCommand).Value;

                //Compare less than.
                case ExtendedCommandType.CompareLessThan:
                    return "cmp_lt " + (c as CompareLessThanCommand).Variable + ", " + (c as CompareLessThanCommand).Value;

                //Compare not equal.
                case ExtendedCommandType.CompareNotEqual:
                    return "cmp_ne " + (c as CompareNotEqualCommand).Variable + ", " + (c as CompareNotEqualCommand).Value;

                //Mod 2 curve.
                case ExtendedCommandType.Mod2Curve:
                    return "mod2_curve " + ((byte)(c as Mod2CurveCommand).Curve);

                //Mod 2 phase.
                case ExtendedCommandType.Mod2Phase:
                    return "mod2_phase " + (c as Mod2PhaseCommand).Phase;

                //Mod 2 depth.
                case ExtendedCommandType.Mod2Depth:
                    return "mod2_depth " + (c as Mod2DepthCommand).Depth;

                //Mod 2 speed.
                case ExtendedCommandType.Mod2Speed:
                    return "mod2_speed " + (c as Mod2SpeedCommand).Speed;

                //Mod 2 type.
                case ExtendedCommandType.Mod2Type:
                    return "mod2_type " + ((byte)(c as Mod2TypeCommand).Type);

                //Mod 2 range.
                case ExtendedCommandType.Mod2Range:
                    return "mod2_range " + (c as Mod2RangeCommand).Range;

                //Mod 3 curve.
                case ExtendedCommandType.Mod3Curve:
                    return "mod3_curve " + ((byte)(c as Mod3CurveCommand).Curve);

                //Mod 3 phase.
                case ExtendedCommandType.Mod3Phase:
                    return "mod3_phase " + (c as Mod3PhaseCommand).Phase;

                //Mod 3 depth.
                case ExtendedCommandType.Mod3Depth:
                    return "mod3_depth " + (c as Mod3DepthCommand).Depth;

                //Mod 3 speed.
                case ExtendedCommandType.Mod3Speed:
                    return "mod3_speed " + (c as Mod3SpeedCommand).Speed;

                //Mod 3 type.
                case ExtendedCommandType.Mod3Type:
                    return "mod3_type " + ((byte)(c as Mod3TypeCommand).Type);

                //Mod 3 range.
                case ExtendedCommandType.Mod3Range:
                    return "mod3_range " + (c as Mod3RangeCommand).Range;

                //Mod 4 curve.
                case ExtendedCommandType.Mod4Curve:
                    return "mod4_curve " + ((byte)(c as Mod4CurveCommand).Curve);

                //Mod 4 phase.
                case ExtendedCommandType.Mod4Phase:
                    return "mod4_phase " + (c as Mod4PhaseCommand).Phase;

                //Mod 4 depth.
                case ExtendedCommandType.Mod4Depth:
                    return "mod4_depth " + (c as Mod4DepthCommand).Depth;

                //Mod 4 speed.
                case ExtendedCommandType.Mod4Speed:
                    return "mod4_speed " + (c as Mod4SpeedCommand).Speed;

                //Mod 4 type.
                case ExtendedCommandType.Mod4Type:
                    return "mod4_type " + ((byte)(c as Mod4TypeCommand).Type);

                //Mod 4 range.
                case ExtendedCommandType.Mod4Range:
                    return "mod4_range " + (c as Mod4RangeCommand).Range;

                //Function.
                case ExtendedCommandType.Function:
                    return "userproc " + (c as FunctionCommand).Function;

                //Mod 2 delay.
                case ExtendedCommandType.Mod2Delay:
                    return "mod2_delay " + (c as Mod2DelayCommand).Delay;

                //Mod 2 period.
                case ExtendedCommandType.Mod2Period:
                    return "mod2_period " + (c as Mod2PeriodCommand).Period;

                //Mod 3 delay.
                case ExtendedCommandType.Mod3Delay:
                    return "mod3_delay " + (c as Mod3DelayCommand).Delay;

                //Mod 3 period.
                case ExtendedCommandType.Mod3Period:
                    return "mod3_period " + (c as Mod3PeriodCommand).Period;

                //Mod 4 delay.
                case ExtendedCommandType.Mod4Delay:
                    return "mod4_delay " + (c as Mod4DelayCommand).Delay;

                //Mod 4 period.
                case ExtendedCommandType.Mod4Period:
                    return "mod4_period " + (c as Mod4PeriodCommand).Period;

                //Last case.
                default:
                    return "";

            }

        }

        /// <summary>
        /// Return the string of a prefix command.
        /// </summary>
        /// <param name="c">The prefix command.</param>
        /// <returns>The prefix command as a string,</returns>
        public string PrefixToSeqString(SequenceCommand c, Dictionary<string, int> publicLabels) {

            //Random.
            if ((CommandType)c.Identifier == CommandType.Random) {

                //Get string.
                RemoveNullFromCommand(c);
                string s = (c as RandomCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");

                //If no space, add at end.
                if (!s.Contains(" ")) {
                    return s + "_r " + (c as RandomCommand).Min + ", " + (c as RandomCommand).Max;
                }

                //Add after first space.
                s = s.Substring(0, s.IndexOf(" ")) + "_r " + s.Substring(s.IndexOf(" "), s.LastIndexOf(" ") + 1 - s.IndexOf(" "));
                return s + (c as RandomCommand).Min + ", " + (c as RandomCommand).Max;

            }

            //Variable.
            if ((CommandType)c.Identifier == CommandType.Variable) {

                //Get string.
                RemoveNullFromCommand(c);
                string s = (c as VariableCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");

                //If no space, add at end.
                if (!s.Contains(" ")) {
                    return s + "_v " + (c as VariableCommand).Variable;
                }

                //Add after first space.
                s = s.Substring(0, s.IndexOf(" ")) + "_v " + s.Substring(s.IndexOf(" "), s.LastIndexOf(" ") + 1 - s.IndexOf(" "));
                return s + (c as VariableCommand).Variable;

            }

            //If.
            if ((CommandType)c.Identifier == CommandType.If) {

                //Get string.
                string s = (c as IfCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");

                //If no space, add at end.
                if (!s.Contains(" ")) {
                    return s + "_if";
                }

                //Add after first space.
                return s.Substring(0, s.IndexOf(" ")) + "_if" + s.Substring(s.IndexOf(" "), s.Length - s.IndexOf(" "));

            }

            //Time change.
            if ((CommandType)c.Identifier == CommandType.TimeChange) {

                //Get string.
                RemoveNullFromCommand(c);
                string s = (c as TimeChangeCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");
                return s + "_t " + (c as TimeChangeCommand).Ticks;

            }

            //Time random.
            if ((CommandType)c.Identifier == CommandType.TimeRandom) {

                //Get string.
                RemoveNullFromCommand(c);
                string s = (c as TimeChangeRandomCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");
                return s + "_tr " + (c as TimeChangeRandomCommand).Min + ", " + (c as TimeChangeRandomCommand).Max;

            }

            //Time variable.
            if ((CommandType)c.Identifier == CommandType.TimeVariable) {

                //Get string.
                RemoveNullFromCommand(c);
                string s = (c as TimeChangeVariableCommand).SequenceCommand.ToSeqString(publicLabels).Replace("\t", " ");
                return s + "_tv " + (c as TimeChangeVariableCommand).VariableNumber;

            }

            //Last resort.
            return "";

        }

        /// <summary>
        /// Remove null parameters from a command.
        /// </summary>
        /// <param name="c">The command.</param>
        public static void RemoveNullFromCommand(SequenceCommand c) {

            for (int i = 0; i < c.Parameters.Length; i++) {

                if (c.Parameters[i] == null) {
                    c.Parameters[i] = (short)0;
                } else if (c.Parameters[i] as SequenceCommand != null) {
                    RemoveNullFromCommand(c.Parameters[i] as SequenceCommand);
                }

            }

        }

    }

}
