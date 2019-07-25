using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// String to sequence command.
    /// </summary>
    public partial class SequenceData {

        /// <summary>
        /// Convert a command string to a sequence command.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="commandNum">Command number.</param>
        /// <param name="privateOffsets">Private offsets list.</param>
        /// <param name="publicOffsets">Public offsets list.</param>
        /// <returns>The command.</returns>
        public static SequenceCommand CommandFromString(string s, int commandNum, List<int> privateOffsets, Dictionary<string, int> publicOffsets, Dictionary<int, string> offsetList) {

            //If it is a label, return.
            if (s.Contains(":")) {
                return null;
            }

            //Command is actually valid.
            if (!s.Equals("")) {

                //Convert to lower case.
                s = s.ToLower().Replace("\t", "").TrimEnd(new char[] { ' ' });

                //Open track.
                if (s.StartsWith("opentrack") && !s.Contains("_if")) {
                    Int64[] vals = GetObjectParameters(s, "opentrack", 1);
                    offsetList.Add(commandNum, s.Split(',')[1]);
                    return new OpenTrackCommand { TrackNumber = (byte)vals[0], Offset = 0 };
                }

                //Jump.
                else if (s.StartsWith("jump") && !s.Contains("_if")) {
                    offsetList.Add(commandNum, s.Replace("jump", ""));
                    return new JumpCommand { Offset = 0 };
                }

                //Call.
                else if (s.StartsWith("call") && !s.Contains("_if")) {
                    offsetList.Add(commandNum, s.Replace("call", ""));
                    return new CallCommand { Offset = 0 };
                }

                //Return a new random command. REMOVING PARAMETERS IS NOT THOUGHT THROUGH!!!
                else if (s.Contains("_r")) {
                    string s2 = s.Remove(s.IndexOf("_r"), 2);
                    Int64[] vals = GetObjectParameters(s2, s.Substring(0, s.IndexOf("_r")));
                    s2 = s2.Substring(0, s.LastIndexOf(','));
                    s2 = s2.Substring(0, s.LastIndexOf(','));
                    var c = CommandFromString(s2, commandNum, privateOffsets, publicOffsets, offsetList);
                    c.SequenceParameterTypes.RemoveAt(c.SequenceParameterTypes.Count - 1);
                    return new RandomCommand() { SequenceCommand = c, Min = (short)vals[vals.Count() - 2], Max = (short)vals[vals.Count() - 1] };
                }
                
                //Return a new if command.
                else if (s.Contains("_if")) {
                    string s2 = s.Remove(s.IndexOf("_if"), 3);
                    return new IfCommand() { SequenceCommand = CommandFromString(s2, commandNum, privateOffsets, publicOffsets, offsetList) };
                }

                //Return a new variable command. REMOVING PARAMETERS IS NOT THOUGHT THROUGH!!!
                else if (s.Contains("_v")) {
                    string s2 = s.Remove(s.IndexOf("_v"), 2);
                    Int64[] vals = GetObjectParameters(s2, s.Substring(0, s.IndexOf("_v")));
                    s2 = s2.Substring(0, s.LastIndexOf(','));
                    var c = CommandFromString(s2, commandNum, privateOffsets, publicOffsets, offsetList);
                    c.SequenceParameterTypes.RemoveAt(c.SequenceParameterTypes.Count - 1);
                    return new VariableCommand() { SequenceCommand = c, Variable = (byte)vals[vals.Count() - 1] };
                }

                //Return a new timechange command. REMOVING PARAMETERS IS NOT THOUGHT THROUGH!!!
                else if (s.Contains("_t")) {
                    string s2 = s.Remove(s.IndexOf("_t"), 2);
                    Int64[] vals = GetObjectParameters(s2, s.Substring(0, s.IndexOf("_t")));
                    s2 = s2.Substring(0, s.LastIndexOf(','));
                    var c = CommandFromString(s2, commandNum, privateOffsets, publicOffsets, offsetList);
                    c.SequenceParameterTypes.RemoveAt(c.SequenceParameterTypes.Count - 1);
                    return new TimeChangeCommand() { SequenceCommand = c, Ticks = (ushort)vals[vals.Count() - 1] };
                }

                else if (s.Contains("_tr")) {
                    return null;
                }

                else if (s.Contains("_tv")) {
                    return null;
                }

                //Note command.
                for (int i = 0; i < 128; i++) {
                    if (s.StartsWith(((Notes)i).ToString())) {
                        Int64[] vals = GetObjectParameters(s, ((Notes)i).ToString());
                        return new NoteCommand() { Identifier = (byte)i, Velocity = (sbyte)vals[0], Length = (uint)vals[1] };
                    }
                }

                //Wait command.
                if (s.StartsWith("wait")) {
                    Int64[] vals = GetObjectParameters(s, "wait");
                    return new WaitCommand() { Length = (uint)vals[0] };
                }

                //Program.
                else if (s.StartsWith("prg")) {
                    Int64[] vals = GetObjectParameters(s, "prg");
                    return new ProgramChangeCommand() { Program = (uint)vals[0] };
                }

                //Time base.
                else if (s.StartsWith("timebase")) {
                    Int64[] vals = GetObjectParameters(s, "timebase");
                    return new TimeBaseCommand { QuarterNoteLength = (byte)vals[0] };
                }

                //Hold.
                else if (s.StartsWith("env_hold")) {
                    Int64[] vals = GetObjectParameters(s, "env_hold");
                    return new HoldCommand { Hold = (sbyte)vals[0] };
                }

                //Monophonic.
                else if (s.StartsWith("monophonic_on")) {
                    return new MonophonicCommand() { Monophonic = true };
                }

                //Monophonic.
                else if (s.StartsWith("monophonic_off")) {
                    return new MonophonicCommand() { Monophonic = false };
                }

                //Velocity range.
                else if (s.StartsWith("velocity_range")) {
                    Int64[] vals = GetObjectParameters(s, "velocity_range");
                    return new VelocityRangeCommand() { VelocityRange = (sbyte)vals[0] };
                }

                //Biquad type.
                else if (s.StartsWith("biquad_type")) {
                    Int64[] vals = GetObjectParameters(s, "biquad_type");
                    return new BiquadTypeCommand() { BiquadType = (sbyte)vals[0] };
                }

                //Biquad value.
                else if (s.StartsWith("biquad_value")) {
                    Int64[] vals = GetObjectParameters(s, "biquad_value");
                    return new BiquadValueCommand() { BiquadValue = (sbyte)vals[0] };
                }

                //Bank select.
                else if (s.StartsWith("bank_select")) {
                    Int64[] vals = GetObjectParameters(s, "bank_select");
                    return new BankSelectCommand() { BankNumber = (sbyte)vals[0] };
                }

                //Mod phase.
                else if (s.StartsWith("mod_phase")) {
                    Int64[] vals = GetObjectParameters(s, "mod_phase");
                    return new ModPhaseCommand { Phase = (sbyte)vals[0] };
                }

                //Mod curve.
                else if (s.StartsWith("mod_curve")) {
                    Int64[] vals = GetObjectParameters(s, "mod_curve");
                    return new ModCurveCommand { Curve = (ModCurveType)(byte)vals[0] };
                }

                //Front bypass.
                else if (s.StartsWith("front_bypass_on")) {
                    return new FrontBypassCommand { FrontBypass = true };
                }

                //Front bypass.
                else if (s.StartsWith("front_bypass_off")) {
                    return new FrontBypassCommand { FrontBypass = false };
                }

                //Pan.
                else if (s.StartsWith("pan")) {
                    Int64[] vals = GetObjectParameters(s, "pan");
                    return new PanCommand { TrackPan = (sbyte)vals[0] };
                }

                //Volume.
                else if (s.StartsWith("volume")) {
                    Int64[] vals = GetObjectParameters(s, "volume");
                    return new VolumeCommand { Volume = (sbyte)vals[0] };
                }

                //Main volume.
                else if (s.StartsWith("main_volume")) {
                    Int64[] vals = GetObjectParameters(s, "main_volume");
                    return new MainVolumeCommand { MainVolume = (sbyte)vals[0] };
                }

                //Transpose.
                else if (s.StartsWith("transpose")) {
                    Int64[] vals = GetObjectParameters(s, "transpose");
                    return new TransposeCommand { Transpose = (sbyte)vals[0] };
                }

                //Pitch bend.
                else if (s.StartsWith("pitchbend")) {
                    Int64[] vals = GetObjectParameters(s, "pitchbend");
                    return new PitchBendCommand { PitchBend = (sbyte)vals[0] };
                }

                //Bend range.
                else if (s.StartsWith("bendrange")) {
                    Int64[] vals = GetObjectParameters(s, "bendrange");
                    return new BendRangeCommand { BendRange = (sbyte)vals[0] };
                }

                //Voicing priority.
                else if (s.StartsWith("prio")) {
                    Int64[] vals = GetObjectParameters(s, "prio");
                    return new VoicingPriorityCommand { Priority = (sbyte)vals[0] };
                }

                //Notewait.
                else if (s.StartsWith("notewait_on")) {
                    return new NoteWaitCommand { NoteWaitMode = true };
                }

                //Notewait.
                else if (s.StartsWith("notewait_off")) {
                    return new NoteWaitCommand { NoteWaitMode = false };
                }

                //Tie
                else if (s.StartsWith("tie_on")) {
                    return new TieCommand { TieMode = true };
                }

                //Tie
                else if (s.StartsWith("tie_off")) {
                    return new TieCommand { TieMode = false };
                }

                //Porta time.
                else if (s.StartsWith("porta_time")) {
                    Int64[] vals = GetObjectParameters(s, "porta_time");
                    return new PortamentoTimeCommand { PortamentoTime = (byte)vals[0] };
                }

                //Porta on.
                else if (s.StartsWith("porta_on")) {
                    return new PortamentoEnabledCommand() { PortamentoEnabled = true };
                }

                //Porta off.
                else if (s.StartsWith("porta_off")) {
                    return new PortamentoEnabledCommand() { PortamentoEnabled = false };
                }

                //Portamento.
                else if (s.StartsWith("porta")) {
                    Int64[] vals = GetObjectParameters(s, "porta");
                    return new PortamentoCommand { Portamento = (sbyte)vals[0] };
                }

                //Mod depth.
                else if (s.StartsWith("mod_depth")) {
                    Int64[] vals = GetObjectParameters(s, "mod_depth");
                    return new ModDepthCommand { Depth = (sbyte)vals[0] };
                }

                //Mod speed.
                else if (s.StartsWith("mod_speed")) {
                    Int64[] vals = GetObjectParameters(s, "mod_speed");
                    return new ModSpeedCommand { Speed = (sbyte)vals[0] };
                }

                //Mod type.
                else if (s.StartsWith("mod_type")) {
                    Int64[] vals = GetObjectParameters(s, "mod_type");
                    return new ModTypeCommand { Type = (ModType)(byte)vals[0] };
                }

                //Mod range.
                else if (s.StartsWith("mod_range")) {
                    Int64[] vals = GetObjectParameters(s, "mod_range");
                    return new ModRangeCommand { Range = (sbyte)vals[0] };
                }

                //Attack.
                else if (s.StartsWith("attack")) {
                    Int64[] vals = GetObjectParameters(s, "attack");
                    return new AttackCommand { Attack = (sbyte)vals[0] };
                }

                //Decay.
                else if (s.StartsWith("decay")) {
                    Int64[] vals = GetObjectParameters(s, "decay");
                    return new DecayCommand { Decay = (sbyte)vals[0] };
                }

                //Sustain.
                else if (s.StartsWith("sustain")) {
                    Int64[] vals = GetObjectParameters(s, "sustain");
                    return new SustainCommand { Sustain = (sbyte)vals[0] };
                }

                //Release.
                else if (s.StartsWith("release")) {
                    Int64[] vals = GetObjectParameters(s, "release");
                    return new ReleaseCommand { Release = (sbyte)vals[0] };
                }

                //Loop start.
                else if (s.StartsWith("loop_start")) {
                    Int64[] vals = GetObjectParameters(s, "loop_start");
                    return new LoopStartCommand { Count = (byte)vals[0] };
                }

                //Volume 2.
                else if (s.StartsWith("volume2")) {
                    Int64[] vals = GetObjectParameters(s, "volume2");
                    return new Volume2Command { Volume2 = (sbyte)vals[0] };
                }

                //Print var.
                else if (s.StartsWith("printvar")) {
                    Int64[] vals = GetObjectParameters(s, "printvar");
                    return new PrintVarCommand { Var = (sbyte)vals[0] };
                }

                //Surround pan.
                else if (s.StartsWith("span")) {
                    Int64[] vals = GetObjectParameters(s, "span");
                    return new SurroundPanCommand { TrackSurroundPan = (sbyte)vals[0] };
                }

                //LPF cutoff.
                else if (s.StartsWith("lpf_cutoff")) {
                    Int64[] vals = GetObjectParameters(s, "lpf_cutoff");
                    return new LPFCutoffCommand { LPFCutoff = (sbyte)vals[0] };
                }

                //Fx send A.
                else if (s.StartsWith("fxsend_a")) {
                    Int64[] vals = GetObjectParameters(s, "fxsend_a");
                    return new FxSendACommand { FxSendA = (sbyte)vals[0] };
                }

                //Fx send B.
                else if (s.StartsWith("fxsend_b")) {
                    Int64[] vals = GetObjectParameters(s, "fxsend_b");
                    return new FxSendBCommand { FxSendB = (sbyte)vals[0] };
                }

                //Main send.
                else if (s.StartsWith("mainsend")) {
                    Int64[] vals = GetObjectParameters(s, "mainsend");
                    return new MainSendCommand { MainSend = (sbyte)vals[0] };
                }

                //Initial pan.
                else if (s.StartsWith("init_pan")) {
                    Int64[] vals = GetObjectParameters(s, "init_pan");
                    return new InitialPanCommand { NotePan = (sbyte)vals[0] };
                }

                //Mute mode.
                else if (s.StartsWith("mute")) {
                    Int64[] vals = GetObjectParameters(s, "mute");
                    return new MuteModeCommand { Mode = (MuteMode)(byte)vals[0] };
                }

                //Fx send C.
                else if (s.StartsWith("fxsend_c")) {
                    Int64[] vals = GetObjectParameters(s, "fxsend_c");
                    return new FxSendCCommand { FxSendC = (sbyte)vals[0] };
                }

                //Damper.
                else if (s.StartsWith("damper_on")) {
                    return new DamperCommand() { Damper = false };
                }

                //Damper.
                else if (s.StartsWith("damper_off")) {
                    return new DamperCommand() { Damper = false };
                }

                //Mod delay.
                else if (s.StartsWith("mod_delay")) {
                    Int64[] vals = GetObjectParameters(s, "mod_delay");
                    return new ModDelayCommand { Delay = (short)vals[0] };
                }

                //Tempo.
                else if (s.StartsWith("tempo")) {
                    Int64[] vals = GetObjectParameters(s, "tempo");
                    return new TempoChangeCommand { Tempo = (short)vals[0] };
                }

                //Sweep pitch.
                else if (s.StartsWith("sweep_pitch")) {
                    Int64[] vals = GetObjectParameters(s, "sweep_pitch");
                    return new SweepPitchCommand { SweepPitch = (short)vals[0] };
                }

                //Mod period.
                else if (s.StartsWith("mod_period")) {
                    Int64[] vals = GetObjectParameters(s, "mod_period");
                    return new ModPeriodCommand { Period = (ushort)vals[0] };
                }

                //Set var.
                else if (s.StartsWith("setvar")) {
                    Int64[] vals = GetObjectParameters(s, "setvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new SetVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Add var.
                else if (s.StartsWith("addvar")) {
                    Int64[] vals = GetObjectParameters(s, "addvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new AddVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Sub var.
                else if (s.StartsWith("subvar")) {
                    Int64[] vals = GetObjectParameters(s, "subvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new SubtractVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Mul var.
                else if (s.StartsWith("mulvar")) {
                    Int64[] vals = GetObjectParameters(s, "mulvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new MultiplyVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Div var.
                else if (s.StartsWith("divvar")) {
                    Int64[] vals = GetObjectParameters(s, "divvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new DivideVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Shift var.
                else if (s.StartsWith("shiftvar")) {
                    Int64[] vals = GetObjectParameters(s, "shiftvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new ShiftVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Random var.
                else if (s.StartsWith("randvar")) {
                    Int64[] vals = GetObjectParameters(s, "randvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new RandomVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //And var.
                else if (s.StartsWith("andvar")) {
                    Int64[] vals = GetObjectParameters(s, "andvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new AndVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Or var.
                else if (s.StartsWith("orvar")) {
                    Int64[] vals = GetObjectParameters(s, "orvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new OrVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Xor var.
                else if (s.StartsWith("xorvar")) {
                    Int64[] vals = GetObjectParameters(s, "xorvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new XorVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Not var.
                else if (s.StartsWith("notvar")) {
                    Int64[] vals = GetObjectParameters(s, "notvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new NotVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Mod var.
                else if (s.StartsWith("modvar")) {
                    Int64[] vals = GetObjectParameters(s, "modvar");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new ModVarCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare equal.
                else if (s.StartsWith("cmp_eq")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_eq");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareEqualCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare ge.
                else if (s.StartsWith("cmp_ge")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_ge");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareGreaterThanOrEqualToCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare greater than.
                else if (s.StartsWith("cmp_gt")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_gt");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareGreaterThanCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare le.
                else if (s.StartsWith("cmp_le")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_le");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareLessThanOrEqualToCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare less than.
                else if (s.StartsWith("cmp_lt")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_lt");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareLessThanCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Compare not equal.
                else if (s.StartsWith("cmp_ne")) {
                    Int64[] vals = GetObjectParameters(s, "cmp_ne");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new CompareNotEqualCommand { Variable = (byte)vals[0], Value = (short)vals[1] }
                    };
                }

                //Function.
                else if (s.StartsWith("userproc")) {
                    Int64[] vals = GetObjectParameters(s, "userproc");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new FunctionCommand { Function = (short)vals[0] }
                    };
                }

                //Mod 2 curve.
                else if (s.StartsWith("mod2_curve")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_curve");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2CurveCommand { Curve = (ModCurveType)(byte)vals[0] }
                    };
                }

                //Mod 2 phase.
                else if (s.StartsWith("mod2_phase")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_phase");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2PhaseCommand { Phase = (sbyte)vals[0] }
                    };
                }

                //Mod 2 depth.
                else if (s.StartsWith("mod2_depth")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_depth");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2DepthCommand { Depth = (sbyte)vals[0] }
                    };
                }

                //Mod 2 speed.
                else if (s.StartsWith("mod2_speed")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_speed");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2SpeedCommand { Speed = (sbyte)vals[0] }
                    };
                }

                //Mod 2 type.
                else if (s.StartsWith("mod2_type")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_type");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2TypeCommand { Type = (ModType)(byte)vals[0] }
                    };
                }

                //Mod 2 range.
                else if (s.StartsWith("mod2_range")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_range");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2RangeCommand { Range = (sbyte)vals[0] }
                    };
                }

                //Mod 2 delay.
                else if (s.StartsWith("mod2_delay")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_delay");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2DelayCommand { Delay = (short)vals[0] }
                    };
                }

                //Mod 2 period.
                else if (s.StartsWith("mod2_period")) {
                    Int64[] vals = GetObjectParameters(s, "mod2_period");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod2PeriodCommand { Period = (ushort)vals[0] }
                    };
                }

                //Mod 3 curve.
                else if (s.StartsWith("mod3_curve")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_curve");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3CurveCommand { Curve = (ModCurveType)(byte)vals[0] }
                    };
                }

                //Mod 3 phase.
                else if (s.StartsWith("mod3_phase")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_phase");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3PhaseCommand { Phase = (sbyte)vals[0] }
                    };
                }

                //Mod 3 depth.
                else if (s.StartsWith("mod3_depth")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_depth");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3DepthCommand { Depth = (sbyte)vals[0] }
                    };
                }

                //Mod 3 speed.
                else if (s.StartsWith("mod3_speed")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_speed");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3SpeedCommand { Speed = (sbyte)vals[0] }
                    };
                }

                //Mod 3 type.
                else if (s.StartsWith("mod3_type")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_type");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3TypeCommand { Type = (ModType)(byte)vals[0] }
                    };
                }

                //Mod 3 range.
                else if (s.StartsWith("mod3_range")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_range");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3RangeCommand { Range = (sbyte)vals[0] }
                    };
                }

                //Mod 3 delay.
                else if (s.StartsWith("mod3_delay")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_delay");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3DelayCommand { Delay = (short)vals[0] }
                    };
                }

                //Mod 3 period.
                else if (s.StartsWith("mod3_period")) {
                    Int64[] vals = GetObjectParameters(s, "mod3_period");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod3PeriodCommand { Period = (ushort)vals[0] }
                    };
                }

                //Mod 4 curve.
                else if (s.StartsWith("mod4_curve")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_curve");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4CurveCommand { Curve = (ModCurveType)(byte)vals[0] }
                    };
                }

                //Mod 4 phase.
                else if (s.StartsWith("mod4_phase")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_phase");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4PhaseCommand { Phase = (sbyte)vals[0] }
                    };
                }

                //Mod 4 depth.
                else if (s.StartsWith("mod4_depth")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_depth");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4DepthCommand { Depth = (sbyte)vals[0] }
                    };
                }

                //Mod 4 speed.
                else if (s.StartsWith("mod4_speed")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_speed");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4SpeedCommand { Speed = (sbyte)vals[0] }
                    };
                }

                //Mod 4 type.
                else if (s.StartsWith("mod4_type")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_type");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4TypeCommand { Type = (ModType)(byte)vals[0] }
                    };
                }

                //Mod 4 range.
                else if (s.StartsWith("mod4_range")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_range");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4RangeCommand { Range = (sbyte)vals[0] }
                    };
                }

                //Mod 4 delay.
                else if (s.StartsWith("mod4_delay")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_delay");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4DelayCommand { Delay = (short)vals[0] }
                    };
                }

                //Mod 4 period.
                else if (s.StartsWith("mod4_period")) {
                    Int64[] vals = GetObjectParameters(s, "mod4_period");
                    return new ExtendedCommand()
                    {
                        SequenceCommand = new Mod4PeriodCommand { Period = (ushort)vals[0] }
                    };
                }

                //Envelope.
                else if (s.StartsWith("envelope")) {
                    throw new Exception("Batch envelope conversion is not supported!");
                }
                else if (s.StartsWith("env_adsr")) {
                    throw new Exception("Batch envelope conversion is not supported!");
                }
                else if (s.StartsWith("env_ahdsr")) {
                    throw new Exception("Batch envelope conversion is not supported!");
                }

                //Envelope reset.
                else if (s.StartsWith("env_reset")) {
                    return new EnvelopeResetCommand();
                }

                //Loop end.
                else if (s.StartsWith("loop_end")) {
                    return new LoopEndCommand();
                }

                //Return.
                else if (s.StartsWith("ret")) {
                    return new ReturnCommand();
                }

                //Allocate track.
                else if (s.StartsWith("alloctrack")) {
                    Int64[] vals = GetObjectParameters(s, "alloctrack");
                    return new AllocateTrackCommand { AllocatedTracks = (ushort)vals[0] };
                }

                //Fin.
                else if (s.StartsWith("fin")) {
                    return new FinCommand();
                }

            }

            //Return null.
            return null;

        }

        /// <summary>
        /// Get the object parameters.
        /// </summary>
        /// <param name="c">Command string.</param>
        /// <param name="splitBy">String to split by.</param>
        /// <param name="num">Amount of numbers to fetch.</param>
        /// <returns>The parameters.</returns>
        public static Int64[] GetObjectParameters(string c, string splitBy, int num = -1) {

            //New command.
            string s = c.Replace(splitBy, "");

            //New params.
            string[] strs = s.Split(',');
            Int64[] vals;
            if (num == -1) {
                vals = new Int64[strs.Length];
            } else {
                vals = new Int64[num];
            }

            //Parse each value.
            for (int i = 0; i < vals.Length; i++) {

                //Get string.
                string str = strs[i];

                //Get value.
                if (str.Contains("*") || str.Contains("/") || str.Contains("+") || str.Contains("<") || str.Contains(">") || str.Contains("=") || str.Contains("&") || str.Contains("|") || str.Contains("{")) {
                    throw new NotImplementedException("Citric Composer's sequence assembler does not support mathematical operations or bit notation.");
                }

                //Hex.
                else if (str.StartsWith("0x")) {
                    vals[i] = Convert.ToUInt32(str.Replace("0x", ""), 16);
                }

                //Bin.
                else if (str.StartsWith("0b")) {
                    vals[i] = Convert.ToUInt32(str.Replace("0b", ""), 2);
                }

                //Num.
                else {
                    vals[i] = Convert.ToInt64(str);
                }

            }

            //Return params.
            return vals;

        }

    }

}
