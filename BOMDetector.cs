using Byter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Byter
{
    /// <summary>
    /// Inspect about Byte order mark
    /// </summary>
    public class BOMDetector
    {
        
        public static byte[] UTF_8 => new byte[] { 0xEF, 0xBB, 0xBF };
        
        public static byte[] UTF_16_BE_BYTES => new byte[] { 0xFE, 0xFF };
        
        public static byte[] UTF_16_LE_BYTES => new byte[] { 0xFF, 0xFE };
        
        public static byte[] UTF_32_BE_BYTES => new byte[] { 0x00, 0x00, 0xFE, 0xFF };
        
        public static byte[] UTF_32_LE_BYTES => new byte[] { 0xFF, 0xFE, 0x00, 0x00, };
        
        public static byte[] UTF_7_BYTES_1 => new byte[] { 0x2B, 0x2F, 0x76, 0x38, };
        
        public static byte[] UTF_7_BYTES_2 => new byte[] { 0x2B, 0x2F, 0x76, 0x39, };
        
        public static byte[] UTF_7_BYTES_3 => new byte[] { 0x2B, 0x2F, 0x76, 0x2B, };
        
        public static byte[] UTF_7_BYTES_4 => new byte[] { 0x2B, 0x2F, 0x76, 0x2F, };
        
        public static byte[] UTF_7_BYTES_5 => new byte[] { 0x2B, 0x2F, 0x76, 0x38, 0x2D, };
        
        public static byte[] UTF_1_BYTES => new byte[] { 0xF7, 0x64, 0x4C, };
        
        public static byte[] UTF_EBCDIC_BYTES => new byte[] { 0xDD, 0x73, 0x66, 0x73, };
        
        public static byte[] SCSU_BYTES => new byte[] { 0x0E, 0xFE, 0xFF, };
        
        public static byte[] BOCU_1_BYTES => new byte[] { 0xFB, 0xEE, 0x28, };
        
        public static byte[] GB_18030_BYTES => new byte[] { 0x84, 0x31, 0x95, 0x33, };

        /// <summary>
        /// The key is BOM type and the value is BOM bytes 
        /// </summary>
        private static Dictionary<BOMs, byte[]> GetBOMBytes()
        {
            Dictionary<BOMs, byte[]> rs = new Dictionary<BOMs, byte[]>();
            rs[BOMs.UTF_8] = UTF_8;
            rs[BOMs.UTF_16_BE] = UTF_16_BE_BYTES;
            rs[BOMs.UTF_16_LE] = UTF_16_LE_BYTES;
            rs[BOMs.UTF_32_BE] = UTF_32_BE_BYTES;
            rs[BOMs.UTF_32_LE] = UTF_32_LE_BYTES;
            rs[BOMs.UTF_7_CASE1] = UTF_7_BYTES_1;
            rs[BOMs.UTF_7_CASE2] = UTF_7_BYTES_2;
            rs[BOMs.UTF_7_CASE3] = UTF_7_BYTES_3;
            rs[BOMs.UTF_7_CASE4] = UTF_7_BYTES_4;
            rs[BOMs.UTF_7_CASE5] = UTF_7_BYTES_5;
            rs[BOMs.UTF_1] = UTF_1_BYTES;
            rs[BOMs.UTF_EBCDIC] = UTF_EBCDIC_BYTES;
            rs[BOMs.SCSU] = SCSU_BYTES;
            rs[BOMs.BOCU_1] = BOCU_1_BYTES;
            rs[BOMs.GB_18030] = GB_18030_BYTES;

            return rs;
    }

    /// <summary>
    /// Detect BOM from byte sequnce
    /// </summary>
    /// <param name="bytes">target inspected bytes</param>
    /// <returns>type of BOM</returns>
    public static BOMs DetectBOM(IEnumerable<byte> bytes)
        {
            Dictionary<BOMs, byte[]> bomDic = GetBOMBytes();

            foreach (KeyValuePair<BOMs, byte[]> item in bomDic)
            {
                var bom = item.Key;
                var bomBytes = item.Value;

                bool match = IsBOMMatch(bomBytes, bytes);
                if (match)
                {
                    return bom;
                }
            }

            return BOMs.None;
        }

        private static bool IsBOMMatch(IEnumerable<byte> bom , IEnumerable<byte> bytes)
        {
            int bomCount = bom.Count();
            int byteCount = bytes.Count();

            if (byteCount < bomCount)
            {
                return false;
            }

            for (int i = 0; i < bomCount; i++)
            {
                byte b0 = bom.ElementAtOrDefault(i);
                byte b1 = bytes.ElementAtOrDefault(i);

                if (b0 != b1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
