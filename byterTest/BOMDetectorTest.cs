using byter.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace byterTest
{
    [TestClass]
    public class BOMDetectorTest
    {
        [TestMethod]
        public void DetectBOM()
        {
            BOMDetector bd;
            byte[] bts;

            bts = new byte[]{ 0xFF, 0xFE, 0x00, 0x00,  };
            bd = new BOMDetector();

        }
    }
}
