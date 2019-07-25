using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Command type.
    /// </summary>
    public enum CommandType : byte {

        /* Note and Variable Length */

        //Notes 0x00-0x7F are note commands.
        Note = Notes.cn4,

        //Wait.
        Wait = 0x80,

        //Program.
        ProgramChange = 0x81,

        /* Control Commands */

        //Open track.
        OpenTrack = 0x88,

        //Jump.
        Jump = 0x89,

        //Call.
        Call = 0x8A,

        /* Prefix Commands */

        //Random.
        Random = 0xA0,

        //Variable.
        Variable = 0xA1,

        //If.
        If = 0xA2,

        //Time change.
        TimeChange = 0xA3,

        //Time random.
        TimeRandom = 0xA4,

        //Time variable.
        TimeVariable = 0xA5,

        /* U8 Commands */

        //Time base.
        TimeBase = 0xB0,

        //Hold.
        Hold = 0xB1,

        //Monophonic.
        Monophonic = 0xB2,

        //Velocity range.
        VelocityRange = 0xB3,

        //Biquad type.
        BiquadType = 0xB4,

        //Biquad value.
        BiquadValue = 0xB5,

        //Bank select.
        BankSelect = 0xB6,

        //B7-BC are unused.

        //Mod phase.
        ModPhase = 0xBD,

        //Mod curve.
        ModCurve = 0xBE,

        //Front bypass.
        FrontBypass = 0xBF,

        //Pan.
        Pan = 0xC0,

        //Volume.
        Volume = 0xC1,

        //Main volume.
        MainVolume = 0xC2,

        //Transpose.
        Transpose = 0xC3,

        //Pitch bend.
        PitchBend = 0xC4,

        //Bend range.
        BendRange = 0xC5,

        //Voicing priority.
        VoicingPriority = 0xC6,

        //Note Wait.
        NoteWait = 0xC7,

        //Tie.
        Tie = 0xC8,

        //Portamento.
        Portamento = 0xC9,

        //Mod depth.
        ModDepth = 0xCA,

        //Mod speed.
        ModSpeed = 0xCB,

        //Mode type.
        ModType = 0xCC,

        //Mod range.
        ModRange = 0xCD,

        //Portament enabled.
        PortamentoEnabled = 0xCE,

        //Portamento time.
        PortamentoTime = 0xCF,

        //Attack.
        Attack = 0xD0,

        //Decay.
        Decay = 0xD1,

        //Sustain.
        Sustain = 0xD2,

        //Release.
        Release = 0xD3,

        //Loop start.
        LoopStart = 0xD4,

        //Volume 2.
        Volume2 = 0xD5,

        //Print var.
        PrintVar = 0xD6,

        //Surround pan.
        SurroundPan = 0xD7,

        //LPF cutoff.
        LPFCutoff = 0xD8,

        //Fx send A.
        FxSendA = 0xD9,

        //Fx send B.
        FxSendB = 0xDA,

        //Main send.
        MainSend = 0xDB,

        //Initial pan.
        InitialPan = 0xDC,

        //Mute.
        MuteMode = 0xDD,

        //Fx send C.
        FxSendC = 0xDE,

        //Damper.
        Damper = 0xDF,

        /* S16 Commands */

        //Mod delay.
        ModDelay = 0xE0,

        //Tempo.
        TempoChange = 0xE1,

        //E2 is unused for whatever reason.

        //Sweep pitch.
        SweepPitch = 0xE3,

        //Mod period.
        ModPeriod = 0xE4,

        /* Extended Commands */

        //Extended.
        Extended = 0xF0,

        /* Other Commands */

        //Envelope reset.
        EnvelopeReset = 0xFB,

        //Loop end.
        LoopEnd = 0xFC,

        //Return.
        Return = 0xFD,

        //Allocate track.
        AllocateTrack = 0xFE,

        //Fin.
        Fin = 0xFF

    }

    /// <summary>
    /// Extended command type.
    /// </summary>
    public enum ExtendedCommandType : byte {

        /* Operation Extended Commands */

        //Set var.
        SetVar = 0x80,

        //Add var.
        AddVar = 0x81,

        //Subtract var.
        SubtractVar = 0x82,

        //Multiply var.
        MultiplyVar = 0x83,

        //Divide var.
        DivideVar = 0x84,

        //Shift var.
        ShiftVar = 0x85,

        //Random var.
        RandomVar = 0x86,

        //And var.
        AndVar = 0x87,

        //Or var.
        OrVar = 0x88,

        //Xor var.
        XorVar = 0x89,

        //Not var.
        NotVar = 0x8A,

        //Mod var.
        ModVar = 0x8B,

        /* Compare Extended Commands */

        //Compare equal.
        CompareEqual = 0x90,

        //Compare greater than or equal to.
        CompareGreaterThanOrEqualTo = 0x91,

        //Compare greater than.
        CompareGreaterThan = 0x92,

        //Compare less than or equal to.
        CompareLessThanOrEqualTo = 0x93,

        //Compare less than.
        CompareLessThan = 0x94,

        //Compare not equal.
        CompareNotEqual = 0x95,

        /* Mod 2 Extended Commands */

        //Mod 2 curve.
        Mod2Curve = 0xa0,

        //Mod 2 phase.
        Mod2Phase = 0xa1,

        //Mod 2 depth.
        Mod2Depth = 0xa2,

        //Mod 2 speed.
        Mod2Speed = 0xa3,

        //Mod 2 type.
        Mod2Type = 0xa4,

        //Mod 2 range.
        Mod2Range = 0xa5,

        /* Mod 3 Extended Commands */

        //Mod 3 curve.
        Mod3Curve = 0xa6,

        //Mod 3 phase.
        Mod3Phase = 0xa7,

        //Mod 3 depth.
        Mod3Depth = 0xa8,

        //Mod 3 speed.
        Mod3Speed = 0xa9,

        //Mod 3 type.
        Mod3Type = 0xaa,

        //Mod 3 range.
        Mod3Range = 0xab,

        /* Mod 4 Extended Commands */

        //Mod 4 curve.
        Mod4Curve = 0xac,

        //Mod 4 phase.
        Mod4Phase = 0xad,

        //Mod 4 depth.
        Mod4Depth = 0xae,

        //Mod 4 speed.
        Mod4Speed = 0xaf,

        //Mod 4 type.
        Mod4Type = 0xb0,

        //Mod 4 range.
        Mod4Range = 0xb1,

        /* Function Extended Commands */

        //Function command.
        Function = 0xE0,

        /* Mod Timing Extended Commands */

        //Mode 2 delay.
        Mod2Delay = 0xe1,

        //Mod 2 period.
        Mod2Period = 0xe2,

        //Mod 3 delay.
        Mod3Delay = 0xe3,

        //Mod 3 period.
        Mod3Period = 0xe4,

        //Mod 4 delay.
        Mod4Delay = 0xe5,

        //Mod 4 period.
        Mod4Period = 0xe6

    }

}
