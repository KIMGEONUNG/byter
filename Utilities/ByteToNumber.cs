using byter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

namespace byter.Utilities
{
    /// <summary>
    /// Byte data to numeric data like int, double ,etc
    /// </summary>
    public class ByteToNumber
    {
        public static int[] ToInt(IEnumerable<byte> _binaries, Endian endian)
        {
            byte[] binaries = _binaries.ToArray();
            
            if (binaries.Length % 4 != 0)
            {
                throw new InvalidOperationException("If you want to convert to int data, the number of bytes must be multiples of 4");
            }

            List<int> result = new List<int>();
            for (int i = 0; i < binaries.Length; i++)
            {
                byte b1 = binaries[i++];
                byte b2 = binaries[i++];
                byte b3 = binaries[i++];
                byte b4 = binaries[i];

                string part1 = Convert.ToString(b1, 2).PadLeft(8,'0');
                string part2 = Convert.ToString(b2, 2).PadLeft(8,'0');
                string part3 = Convert.ToString(b3, 2).PadLeft(8,'0');
                string part4 = Convert.ToString(b4, 2).PadLeft(8,'0');

                string stack = string.Empty;
                int num;
                switch (endian)
                {
                    case Endian.Big:
                        stack = part1 + part2 + part3 + part4;
                        num = Convert.ToInt32(stack, 2);
                        result.Add(num);
                        break;
                    case Endian.Little:
                        stack = part4 + part3 + part2 + part1;
                        num = Convert.ToInt32(stack, 2);
                        result.Add(num);
                        break;
                    default:
                        break;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Ust Signed 64-bit IEEE double-precision floating point number
        /// </summary>
        /// <param name="_binaries"></param>
        /// <param name="endian"></param>
        /// <returns></returns>
        public static double[] ToDouble(IEnumerable<byte> _binaries, Endian endian)
        {
            byte[] binaries = _binaries.ToArray();
            
            if (binaries.Length % 8 != 0)
            {
                throw new InvalidOperationException("If you want to convert to int data, the number of bytes must be multiples of 8");
            }

            List<double> result = new List<double>();
            for (int i = 0; i < binaries.Length; i++)
            {
                byte b1 = binaries[i++];
                byte b2 = binaries[i++];
                byte b3 = binaries[i++];
                byte b4 = binaries[i++];
                byte b5 = binaries[i++];
                byte b6 = binaries[i++];
                byte b7 = binaries[i++];
                byte b8 = binaries[i];

                string part1 = Convert.ToString(b1, 2).PadLeft(8,'0');
                string part2 = Convert.ToString(b2, 2).PadLeft(8,'0');
                string part3 = Convert.ToString(b3, 2).PadLeft(8,'0');
                string part4 = Convert.ToString(b4, 2).PadLeft(8,'0');
                string part5 = Convert.ToString(b5, 2).PadLeft(8,'0');
                string part6 = Convert.ToString(b6, 2).PadLeft(8,'0');
                string part7 = Convert.ToString(b7, 2).PadLeft(8,'0');
                string part8 = Convert.ToString(b8, 2).PadLeft(8,'0');

                string stack = string.Empty;
                double num;
                switch (endian)
                {
                    case Endian.Big:
                        stack = part1 + part2 + part3 + part4 + part5 + part6 + part7 + part8;
                        num = BitConverter.Int64BitsToDouble(Convert.ToInt64(stack, 2));
                        result.Add(num);
                        break;
                    case Endian.Little:
                        stack = part8 +part7 +part6 +part5 +part4 + part3 + part2 + part1;
                        num = BitConverter.Int64BitsToDouble(Convert.ToInt64(stack, 2));
                        result.Add(num);
                        break;
                    default:
                        break;
                }
            }

            return result.ToArray();
        }
    }
}
