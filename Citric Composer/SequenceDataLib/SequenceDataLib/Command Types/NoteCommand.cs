using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Note command.
    /// </summary>
    public class NoteCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public NoteCommand() {

            //Default note.
            Identifier = (byte)Notes.cn4;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8, SequenceParameterType.VariableLength };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set velocity and length.
            Velocity = 127;
            Length = 48;

        }

        /// <summary>
        /// Velocity.
        /// </summary>
        public sbyte Velocity {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

        /// <summary>
        /// Length of the note. Variable length.
        /// </summary>
        public uint Length {
            get { return (uint)Parameters[1]; }
            set { Parameters[1] = value; }
        }

    }

}
