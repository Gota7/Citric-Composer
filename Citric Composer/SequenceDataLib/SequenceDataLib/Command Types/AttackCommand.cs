using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Attack command.
    /// </summary>
    public class AttackCommand : SequenceCommand {

        /// <summary>
        /// Set defauls.
        /// </summary>
        public AttackCommand() {

            //Set identifier.
            Identifier = (byte)CommandType.Attack;

            //Parameter types.
            SequenceParameterTypes = new List<SequenceParameterType>() { SequenceParameterType.s8 };

            //Set parameters.
            Parameters = new object[SequenceParameterTypes.Count];

            //Set thing.
            Attack = 127;

        }

        /// <summary>
        /// Attack.
        /// </summary>
        public sbyte Attack {
            get { return (sbyte)Parameters[0]; }
            set { Parameters[0] = value; }
        }

    }

}
