using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Note wait command.
    /// </summary>
    public class NoteWaitCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public NoteWaitCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.NoteWait;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.boolean };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            NoteWaitMode = true;

        }

        /// <summary>
        /// Note wait mode. Waits for the execution of the future notes to stop before continuing when enabled.
        /// </summary>
        public bool NoteWaitMode {
            get { return (bool)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
