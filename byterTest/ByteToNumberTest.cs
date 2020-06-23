using byter.Enums;
using byter.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace byterTest
{
    [TestClass]
    public class ByteToNumberTest
    {
        [TestMethod]
        public void ToDouble()
        {
            IEnumerable<byte> binaries = null;
            Endian endian;
            double[] answer = null;
            double[] result = null;

            // Big endian test 
            answer = new double[] { 1 };
            binaries = new byte[] 
            {  0b00111111, 0b11110000, 0b00000000, 0b00000000, 0b00000000, 0b00000000, 0b00000000, 0b00000000, };
            endian = Endian.Big;
            result = ByteToNumber.ToDouble(binaries, endian);

            Assert.IsTrue(answer.SequenceEqual(result));

            // Little endian test 
            answer = new double[] { 23 };
            binaries = new byte[] 
            { 0b00000000, 0b00000000, 0b00000000, 0b00000000, 0b00000000, 0b00000000, 0b00110111, 0b01000000 };
            endian = Endian.Little;
            result = ByteToNumber.ToDouble(binaries, endian);

            Assert.IsTrue(answer.SequenceEqual(result));
        }

        [TestMethod]
        public void ToInt()
        {
            IEnumerable<byte> binaries = null;
            Endian endian;
            int[] answer = null; 
            int[] result = null;

            // Big endian test 
            answer = new int[] { 512 };
            binaries = new byte[] { 0b00000000, 0b00000000, 0b00000010, 0b00000000, };
            endian = Endian.Big;
            result = ByteToNumber.ToInt(binaries, endian);

            Assert.IsTrue(answer.SequenceEqual(result));

            // Big endian test  
            answer = new int[] { 1000 };
            binaries = new byte[] { 0b00000000, 0b00000000, 0b00000011, 0b11101000, };
            endian = Endian.Big;
            result = ByteToNumber.ToInt(binaries, endian);

            Assert.IsTrue(answer.SequenceEqual(result));

            // Little endian test 
            answer = new int[] { 1000 };
            binaries = new byte[] { 0b11101000,  0b00000011, 0b00000000, 0b00000000, };
            endian = Endian.Little;
            result = ByteToNumber.ToInt(binaries, endian);

            Assert.IsTrue(answer.SequenceEqual(result));

        }
    }
}
