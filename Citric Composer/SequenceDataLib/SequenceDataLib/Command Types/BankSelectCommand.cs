using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Bank select command.
    /// </summary>
    public class BankSelectCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public BankSelectCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.BankSelect;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            BankNumber = 0;

        }

        /// <summary>
        /// Bank number. Is between 0-3.
        /// </summary>
        public sbyte BankNumber {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
