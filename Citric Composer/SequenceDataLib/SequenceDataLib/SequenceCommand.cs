using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Sequence command.
    /// </summary>
    public abstract partial class SequenceCommand {

        /// <summary>
        /// Identifier.
        /// </summary>
        public byte Identifier;

        /// <summary>
        /// How to read the sequence parameters.
        /// </summary>
        public List<SequenceParameterType> SequenceParameterTypes = new List<SequenceParameterType>();

        /// <summary>
        /// Raw parameters.
        /// </summary>
        public object[] Parameters = new object[0];

        /// <summary>
        /// The size in bytes that the command occupies.
        /// </summary>
        public int ByteSize {

            get {

                int ret = 1;
                int paramCount = 0;
                foreach (SequenceParameterType t in SequenceParameterTypes) {

                    switch (t) {

                        case SequenceParameterType.boolean:
                            ret++;
                            break;

                        case SequenceParameterType.s16:
                            ret += 2;
                            break;

                        case SequenceParameterType.s32:
                            ret += 4;
                            break;

                        case SequenceParameterType.s64:
                            ret += 8;
                            break;

                        case SequenceParameterType.s8:
                            ret++;
                            break;

                        case SequenceParameterType.SeqCommand:
                            ret += ((SequenceCommand)Parameters[paramCount]).ByteSize;
                            break;

                        case SequenceParameterType.u16:
                            ret += 2;
                            break;

                        case SequenceParameterType.u24:
                            ret += 3;
                            break;

                        case SequenceParameterType.u32:
                            ret += 4;
                            break;

                        case SequenceParameterType.u64:
                            ret += 8;
                            break;

                        case SequenceParameterType.u8:
                            ret++;
                            break;

                        case SequenceParameterType.VariableLength:
                            ret += VariableLength.CalcVariableLengthSize((uint)Parameters[paramCount]);
                            break;

                    }

                    //Increment param count.
                    paramCount++;

                }

                return ret;

            }

        }

        /// <summary>
        /// Sequence command type.
        /// </summary>
        public enum SequenceCommandType {
            Normal, Extended
        }

        /// <summary>
        /// Write the command.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw, List<SequenceCommand> commands) {

            //Write the identifier.
            bw.Write(Identifier);

            //Write each parameter.
            int count = 0;
            foreach (var t in SequenceParameterTypes) {

                //Write the item.
                switch (t) {

                    //U8.
                    case SequenceParameterType.u8:
                        bw.Write((byte)Parameters[count]);
                        break;

                    //U16.
                    case SequenceParameterType.u16:
                        bw.Write((UInt16)Parameters[count]);
                        break;

                    //U24.
                    case SequenceParameterType.u24:

                        //Uses an offset.
                        if ((CommandType)Identifier == CommandType.Call || (CommandType)Identifier == CommandType.Jump || (CommandType)Identifier == CommandType.OpenTrack) {
                            new UInt24((uint)SequenceData.CommandIndexToOffset(commands, (int)((UInt24)Parameters[count]).Value)).Write(bw);
                        }

                        //Normal.
                        else {
                            ((UInt24)Parameters[count]).Write(bw);
                        }
                        break;

                    //U32.
                    case SequenceParameterType.u32:
                        bw.Write((UInt32)Parameters[count]);
                        break;

                    //U32.
                    case SequenceParameterType.u64:
                        bw.Write((UInt64)Parameters[count]);
                        break;

                    //S8.
                    case SequenceParameterType.s8:
                        bw.Write((sbyte)Parameters[count]);
                        break;

                    //S16.
                    case SequenceParameterType.s16:
                        bw.Write((Int16)Parameters[count]);
                        break;

                    //S32.
                    case SequenceParameterType.s32:
                        bw.Write((Int32)Parameters[count]);
                        break;

                    //S32.
                    case SequenceParameterType.s64:
                        bw.Write((Int64)Parameters[count]);
                        break;

                    //Boolean.
                    case SequenceParameterType.boolean:
                        bw.Write((bool)Parameters[count]);
                        break;

                    //Variable length.
                    case SequenceParameterType.VariableLength:
                        VariableLength.WriteVariableLength(bw, (uint)Parameters[count]);
                        break;

                    //Sequence command.
                    case SequenceParameterType.SeqCommand:
                        ((SequenceCommand)Parameters[count]).Write(bw, commands);
                        break;

                }

                //Increment count.
                count++;

            }

        }

        /// <summary>
        /// Read parameters, but only for prefix commands.
        /// </summary>
        /// <param name="br">The reader</param>
        public void ReadPrefixParameters(BinaryDataReader br) {

            //Read the objects.
            int count = 0;
            foreach (var t in SequenceParameterTypes) {

                //Switch the type.
                switch (t) {

                    //U8.
                    case SequenceParameterType.u8:
                        Parameters[count] = br.ReadByte();
                        break;

                    //U16.
                    case SequenceParameterType.u16:
                        Parameters[count] = br.ReadUInt16();
                        break;

                    //U24.
                    case SequenceParameterType.u24:
                        Parameters[count] = new UInt24(br);
                        break;

                    //U32.
                    case SequenceParameterType.u32:
                        Parameters[count] = br.ReadUInt32();
                        break;

                    //U64.
                    case SequenceParameterType.u64:
                        Parameters[count] = br.ReadUInt64();
                        break;

                    //S8.
                    case SequenceParameterType.s8:
                        Parameters[count] = br.ReadSByte();
                        break;

                    //S16.
                    case SequenceParameterType.s16:
                        Parameters[count] = br.ReadInt16();
                        break;

                    //S32.
                    case SequenceParameterType.s32:
                        Parameters[count] = br.ReadInt32();
                        break;

                    //S64.
                    case SequenceParameterType.s64:
                        Parameters[count] = br.ReadInt64();
                        break;

                    //Boolean.
                    case SequenceParameterType.boolean:
                        Parameters[count] = br.ReadBoolean();
                        break;

                    //Variable length.
                    case SequenceParameterType.VariableLength:
                        Parameters[count] = VariableLength.ReadVariableLength(br);
                        break;

                    //Sequence command.
                    case SequenceParameterType.SeqCommand:
                        Parameters[count] = Read(br);
                        break;

                }

                //Increase count.
                count++;

            }

        }

        /// <summary>
        /// Read a sequence command.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="type">Type of sequence command.</param>
        /// <returns>The read sequence command.</returns>
        public static SequenceCommand Read(BinaryDataReader br, SequenceCommandType type = SequenceCommandType.Normal) {

            //Get sequence command.
            SequenceCommand s = null;
            switch (type) {
                case SequenceCommandType.Normal:
                    s = NewFromType(br.ReadByte(), (int)br.Position);
                    break;
                case SequenceCommandType.Extended:
                    s = NewFromTypeExtended(br.ReadByte(), (int)br.Position);
                    break;
            }

            //Read the objects.
            int count = 0;
            foreach (var t in s.SequenceParameterTypes) {

                //Switch the type.
                switch (t) {

                    //U8.
                    case SequenceParameterType.u8:
                        s.Parameters[count] = br.ReadByte();
                        break;

                    //U16.
                    case SequenceParameterType.u16:
                        s.Parameters[count] = br.ReadUInt16();
                        break;

                    //U24.
                    case SequenceParameterType.u24:
                        s.Parameters[count] = new UInt24(br);
                        break;

                    //U32.
                    case SequenceParameterType.u32:
                        s.Parameters[count] = br.ReadUInt32();
                        break;

                    //U64.
                    case SequenceParameterType.u64:
                        s.Parameters[count] = br.ReadUInt64();
                        break;

                    //S8.
                    case SequenceParameterType.s8:
                        s.Parameters[count] = br.ReadSByte();
                        break;

                    //S16.
                    case SequenceParameterType.s16:
                        s.Parameters[count] = br.ReadInt16();
                        break;

                    //S32.
                    case SequenceParameterType.s32:
                        s.Parameters[count] = br.ReadInt32();
                        break;

                    //S64.
                    case SequenceParameterType.s64:
                        s.Parameters[count] = br.ReadInt64();
                        break;

                    //Boolean.
                    case SequenceParameterType.boolean:
                        s.Parameters[count] = br.ReadBoolean();
                        break;

                    //Variable length.
                    case SequenceParameterType.VariableLength:
                        s.Parameters[count] = VariableLength.ReadVariableLength(br);
                        break;

                    //Sequence command.
                    case SequenceParameterType.SeqCommand:

                        //The type of sequence command embedded depends on the command type.
                        switch ((CommandType)s.Identifier) {

                            //Extended.
                            case CommandType.Extended:
                                s.Parameters[count] = Read(br, SequenceCommandType.Extended);
                                break;

                            //Random.
                            case CommandType.Random:
                                s.Parameters[count] = NewFromType(br.ReadByte(), (int)br.Position);

                                //Extended.
                                if (((SequenceCommand)s.Parameters[count]).Identifier == (byte)CommandType.Extended) {

                                    //Read a new extended command.
                                    ((SequenceCommand)s.Parameters[count]).Parameters[0] = NewFromTypeExtended(br.ReadByte(), (int)br.Position);

                                    //Remove the last parameter of the sequence command.
                                    ((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).SequenceParameterTypes.RemoveAt(((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).SequenceParameterTypes.Count - 1);

                                    //Read the parameters.
                                    ((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).ReadPrefixParameters(br);

                                }

                                //Regular.
                                else {

                                    //Remove the last parameter, since it is replaced with the min and max.
                                    ((SequenceCommand)s.Parameters[count]).SequenceParameterTypes.RemoveAt(((SequenceCommand)s.Parameters[count]).SequenceParameterTypes.Count - 1);
                                    ((SequenceCommand)s.Parameters[count]).ReadPrefixParameters(br);

                                }
                                break;

                            //Variable.
                            case CommandType.Variable:
                                s.Parameters[count] = NewFromType(br.ReadByte(), (int)br.Position);

                                //Extended.
                                if (((SequenceCommand)s.Parameters[count]).Identifier == (byte)CommandType.Extended) {

                                    //Read a new extended command.
                                    ((SequenceCommand)s.Parameters[count]).Parameters[0] = NewFromTypeExtended(br.ReadByte(), (int)br.Position);

                                    //Remove the last parameter of the sequence command.
                                    ((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).SequenceParameterTypes.RemoveAt(((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).SequenceParameterTypes.Count - 1);

                                    //Read the parameters.
                                    ((SequenceCommand)((SequenceCommand)s.Parameters[count]).Parameters[0]).ReadPrefixParameters(br);

                                }

                                //Regular.
                                else {

                                    //Remove the last parameter, since it is replaced with the min and max.
                                    ((SequenceCommand)s.Parameters[count]).SequenceParameterTypes.RemoveAt(((SequenceCommand)s.Parameters[count]).SequenceParameterTypes.Count - 1);
                                    ((SequenceCommand)s.Parameters[count]).ReadPrefixParameters(br);

                                }
                                break;

                            //Last resort.
                            default:
                                s.Parameters[count] = Read(br);
                                break;

                        }
                        break;

                }

                //Increase count.
                count++;

            }

            //Return the command.
            return s;

        }

        /// <summary>
        /// New sequence from a type.
        /// </summary>
        /// <param name="identifier">Indentifier byte.</param>
        /// <param name="pos">Position of the reader.</param>
        /// <returns>New constructed sequence command.</returns>
        public static SequenceCommand NewFromType(byte identifier, int pos = -1) {

            //Switch from the type.
            switch ((CommandType)identifier) {

                //Note.
                case CommandType c when (identifier >= 0 && identifier <= 0x7F):
                    return new NoteCommand() { Identifier = identifier };

                //Wait.
                case CommandType.Wait:
                    return new WaitCommand();

                //Program change.
                case CommandType.ProgramChange:
                    return new ProgramChangeCommand();

                //Open track.
                case CommandType.OpenTrack:
                    return new OpenTrackCommand();

                //Jump.
                case CommandType.Jump:
                    return new JumpCommand();

                //Call.
                case CommandType.Call:
                    return new CallCommand();

                //Random.
                case CommandType.Random:
                    return new RandomCommand();

                //Variable.
                case CommandType.Variable:
                    return new VariableCommand();

                //If.
                case CommandType.If:
                    return new IfCommand();

                //Time change.
                case CommandType.TimeChange:
                    return new TimeChangeCommand();

                //Time random.
                case CommandType.TimeRandom:
                    return new TimeChangeRandomCommand();

                //Time variable.
                case CommandType.TimeVariable:
                    return new TimeChangeVariableCommand();

                //Time base.
                case CommandType.TimeBase:
                    return new TimeBaseCommand();

                //Hold.
                case CommandType.Hold:
                    return new HoldCommand();

                //Monophonic.
                case CommandType.Monophonic:
                    return new MonophonicCommand();

                //Velocity range.
                case CommandType.VelocityRange:
                    return new VelocityRangeCommand();

                //Biquad type.
                case CommandType.BiquadType:
                    return new BiquadTypeCommand();

                //Biquad value.
                case CommandType.BiquadValue:
                    return new BiquadValueCommand();

                //Bank select.
                case CommandType.BankSelect:
                    return new BankSelectCommand();

                //Mod phase.
                case CommandType.ModPhase:
                    return new ModPhaseCommand();

                //Mod curve.
                case CommandType.ModCurve:
                    return new ModCurveCommand();

                //Front bypass.
                case CommandType.FrontBypass:
                    return new FrontBypassCommand();

                //Mod depth.
                case CommandType.ModDepth:
                    return new ModDepthCommand();

                //Mod speed.
                case CommandType.ModSpeed:
                    return new ModSpeedCommand();

                //Mod type.
                case CommandType.ModType:
                    return new ModTypeCommand();

                //Mod range.
                case CommandType.ModRange:
                    return new ModRangeCommand();

                //Pan.
                case CommandType.Pan:
                    return new PanCommand();

                //Volume.
                case CommandType.Volume:
                    return new VolumeCommand();

                //Print var.
                case CommandType.PrintVar:
                    return new PrintVarCommand();

                //Main volume.
                case CommandType.MainVolume:
                    return new MainVolumeCommand();

                //Transpose.
                case CommandType.Transpose:
                    return new TransposeCommand();

                //Pitch bend.
                case CommandType.PitchBend:
                    return new PitchBendCommand();

                //Bend range.
                case CommandType.BendRange:
                    return new BendRangeCommand();

                //Voicing priority.
                case CommandType.VoicingPriority:
                    return new VoicingPriorityCommand();

                //Note wait.
                case CommandType.NoteWait:
                    return new NoteWaitCommand();

                //Tie mode.
                case CommandType.Tie:
                    return new TieCommand();

                //Portamento.
                case CommandType.Portamento:
                    return new PortamentoCommand();

                //Portamento enabled.
                case CommandType.PortamentoEnabled:
                    return new PortamentoEnabledCommand();

                //Portamento time.
                case CommandType.PortamentoTime:
                    return new PortamentoTimeCommand();

                //Attack.
                case CommandType.Attack:
                    return new AttackCommand();

                //Decay.
                case CommandType.Decay:
                    return new DecayCommand();

                //Sustain.
                case CommandType.Sustain:
                    return new SustainCommand();

                //Release.
                case CommandType.Release:
                    return new ReleaseCommand();

                //Loop start.
                case CommandType.LoopStart:
                    return new LoopStartCommand();

                //Volume 2.
                case CommandType.Volume2:
                    return new Volume2Command();

                //Surround pan.
                case CommandType.SurroundPan:
                    return new SurroundPanCommand();

                //LPF cutoff.
                case CommandType.LPFCutoff:
                    return new LPFCutoffCommand();

                //Fx send A.
                case CommandType.FxSendA:
                    return new FxSendACommand();

                //Fx send B.
                case CommandType.FxSendB:
                    return new FxSendBCommand();

                //Main send.
                case CommandType.MainSend:
                    return new MainSendCommand();

                //Initial pan.
                case CommandType.InitialPan:
                    return new InitialPanCommand();

                //Mute mode.
                case CommandType.MuteMode:
                    return new MuteModeCommand();

                //Fx send C.
                case CommandType.FxSendC:
                    return new FxSendCCommand();

                //Damper.
                case CommandType.Damper:
                    return new DamperCommand();

                //Mod delay.
                case CommandType.ModDelay:
                    return new ModDelayCommand();

                //Mod period.
                case CommandType.ModPeriod:
                    return new ModPeriodCommand();

                //Tempo change.
                case CommandType.TempoChange:
                    return new TempoChangeCommand();

                //Sweep pitch.
                case CommandType.SweepPitch:
                    return new SweepPitchCommand();

                //Extended.
                case CommandType.Extended:
                    return new ExtendedCommand();

                //Envelope reset.
                case CommandType.EnvelopeReset:
                    return new EnvelopeResetCommand();

                //Loop end.
                case CommandType.LoopEnd:
                    return new LoopEndCommand();

                //Return.
                case CommandType.Return:
                    return new ReturnCommand();

                //Allocate track.
                case CommandType.AllocateTrack:
                    return new AllocateTrackCommand();

                //Fin.
                case CommandType.Fin:
                    return new FinCommand();

                //Unknown type.
                default:
                    if (pos != -1) {
                        throw new Exception("Unknown command 0x" + identifier.ToString("X") + " at 0x" + pos.ToString("X") + "!");
                    } else {
                        throw new Exception("Unknown command 0x" + identifier.ToString("X"));
                    }

            }

        }

        /// <summary>
        /// New sequence from a type.
        /// </summary>
        /// <param name="identifier">Indentifier byte.</param>
        /// <param name="pos">Position of the reader.</param>
        /// <returns>New constructed sequence command.</returns>
        public static SequenceCommand NewFromTypeExtended(byte identifier, int pos = -1) {

            //Switch the type.
            switch ((ExtendedCommandType)identifier) {

                //Set var.
                case ExtendedCommandType.SetVar:
                    return new SetVarCommand();

                //Add var.
                case ExtendedCommandType.AddVar:
                    return new AddVarCommand();

                //Subtract var.
                case ExtendedCommandType.SubtractVar:
                    return new SubtractVarCommand();

                //Multiply var.
                case ExtendedCommandType.MultiplyVar:
                    return new MultiplyVarCommand();

                //Divide var.
                case ExtendedCommandType.DivideVar:
                    return new DivideVarCommand();

                //Shift var.
                case ExtendedCommandType.ShiftVar:
                    return new ShiftVarCommand();

                //Random var.
                case ExtendedCommandType.RandomVar:
                    return new RandomVarCommand();

                //And var.
                case ExtendedCommandType.AndVar:
                    return new AndVarCommand();

                //Or var.
                case ExtendedCommandType.OrVar:
                    return new OrVarCommand();

                //Xor var.
                case ExtendedCommandType.XorVar:
                    return new XorVarCommand();

                //Not var.
                case ExtendedCommandType.NotVar:
                    return new NotVarCommand();

                //Mod var.
                case ExtendedCommandType.ModVar:
                    return new ModVarCommand();

                //Compare equal.
                case ExtendedCommandType.CompareEqual:
                    return new CompareEqualCommand();

                //Compare greater than or equal to.
                case ExtendedCommandType.CompareGreaterThanOrEqualTo:
                    return new CompareGreaterThanOrEqualToCommand();

                //Compare greater than.
                case ExtendedCommandType.CompareGreaterThan:
                    return new CompareGreaterThanCommand();

                //Compare less than or equal to.
                case ExtendedCommandType.CompareLessThanOrEqualTo:
                    return new CompareLessThanOrEqualToCommand();

                //Compare less than.
                case ExtendedCommandType.CompareLessThan:
                    return new CompareLessThanCommand();

                //Compare not equal to.
                case ExtendedCommandType.CompareNotEqual:
                    return new CompareNotEqualCommand();

                //Mod 2 curve.
                case ExtendedCommandType.Mod2Curve:
                    return new Mod2CurveCommand();

                //Mod 2 phase.
                case ExtendedCommandType.Mod2Phase:
                    return new Mod2PhaseCommand();

                //Mod 2 depth.
                case ExtendedCommandType.Mod2Depth:
                    return new Mod2DepthCommand();

                //Mod 2 speed.
                case ExtendedCommandType.Mod2Speed:
                    return new Mod2SpeedCommand();

                //Mod 2 type.
                case ExtendedCommandType.Mod2Type:
                    return new Mod2TypeCommand();

                //Mod 2 range.
                case ExtendedCommandType.Mod2Range:
                    return new Mod2RangeCommand();

                //Mod 3 curve.
                case ExtendedCommandType.Mod3Curve:
                    return new Mod3CurveCommand();

                //Mod 3 phase.
                case ExtendedCommandType.Mod3Phase:
                    return new Mod3PhaseCommand();

                //Mod 3 depth.
                case ExtendedCommandType.Mod3Depth:
                    return new Mod3DepthCommand();

                //Mod 3 speed.
                case ExtendedCommandType.Mod3Speed:
                    return new Mod3SpeedCommand();

                //Mod 3 type.
                case ExtendedCommandType.Mod3Type:
                    return new Mod3TypeCommand();

                //Mod 3 range.
                case ExtendedCommandType.Mod3Range:
                    return new Mod3RangeCommand();

                //Mod 4 curve.
                case ExtendedCommandType.Mod4Curve:
                    return new Mod4CurveCommand();

                //Mod 4 phase.
                case ExtendedCommandType.Mod4Phase:
                    return new Mod4PhaseCommand();

                //Mod 4 depth.
                case ExtendedCommandType.Mod4Depth:
                    return new Mod4DepthCommand();

                //Mod 4 speed.
                case ExtendedCommandType.Mod4Speed:
                    return new Mod4SpeedCommand();

                //Mod 4 type.
                case ExtendedCommandType.Mod4Type:
                    return new Mod4TypeCommand();

                //Mod 4 range.
                case ExtendedCommandType.Mod4Range:
                    return new Mod4RangeCommand();

                //Function command.
                case ExtendedCommandType.Function:
                    return new FunctionCommand();

                //Mod 2 delay.
                case ExtendedCommandType.Mod2Delay:
                    return new Mod2DelayCommand();

                //Mod 2 period.
                case ExtendedCommandType.Mod2Period:
                    return new Mod2PeriodCommand();

                //Mod 3 delay.
                case ExtendedCommandType.Mod3Delay:
                    return new Mod3DelayCommand();

                //Mod 3 period.
                case ExtendedCommandType.Mod3Period:
                    return new Mod3PeriodCommand();

                //Mod 4 delay.
                case ExtendedCommandType.Mod4Delay:
                    return new Mod4DelayCommand();

                //Mod 4 period.
                case ExtendedCommandType.Mod4Period:
                    return new Mod4PeriodCommand();

                //Unknown type.
                default:
                    if (pos != -1) {
                        throw new Exception("Unknown command 0x" + identifier.ToString("X") + " at 0x" + pos.ToString("X") + "!");
                    } else {
                        throw new Exception("Unknown command 0x" + identifier.ToString("X"));
                    }

            }

        }

    }

}
