using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDataLib {

    /// <summary>
    /// Variable length.
    /// </summary>
    public static class VariableLength {

        /// <summary>
        /// Read variable length.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <returns>Variable length value.</returns>
        public static uint ReadVariableLength(BinaryDataReader br) {

            //Get the temporary value.
            uint temp = (uint)br.ReadByte();

            //Get value.
            uint val = (uint)temp & 0x7F;

            //Run until MSB is not set.
            while ((temp & 0x80) > 0) {

                //Shift value to the left 7 bits.
                val <<= 7;

                //Get new temp value.
                temp = br.ReadByte();

                //Add the value to the value.
                val |= temp & 0x7F;

            }

            return val;

        }

        /// <summary>
        /// Write write variable length.
        /// </summary>
        /// <param name="bw">The writer.</param>
        /// <param name="val">Value to write.</param>
        public static void WriteVariableLength(BinaryDataWriter bw, uint val) {

            //Write the value.
            List<byte> nums = new List<byte>();
            while (val > 0) {

                //Add number.
                nums.Insert(0, (byte)(val & 0x7F));
                val >>= 7;

            }

            //Add MSB.
            for (int i = 0; i < nums.Count - 1; i++) {

                //Set MSB.
                nums[i] |= 0x80;

            }

            //Safety.
            if (nums.Count < 1) {
                nums.Add(0);
            }

            //Write the value.
            bw.Write(nums.ToArray());

        }

        /// <summary>
        /// Get the size of a variable length parameter.
        /// </summary>
        /// <param name="val">Value.</param>
        /// <returns>The size of the variable length in bytes.</returns>
        public static int CalcVariableLengthSize(uint val) {

            //Write the value.
            List<byte> nums = new List<byte>();
            while (val > 0) {

                //Add number.
                nums.Insert(0, (byte)(val & 0x7F));
                val >>= 7;

            }

            //Add MSB.
            for (int i = 0; i < nums.Count - 1; i++) {

                //Set MSB.
                nums[i] |= 0x80;

            }

            //Safety.
            if (nums.Count < 1) {
                nums.Add(0);
            }

            //Return the size.
            return nums.Count;

        }

    }

}
