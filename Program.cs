using Byter.Enums;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byter
{
    class Program
    {
        public enum Representations
        {
            Binary,
            Decimal,
            Hexadecimal,
        }

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "input file path")]
            public string InputPath { get; set; }

            [Option('b',"binary", Required = false, HelpText = "binary representation",SetName = nameof(Representations.Binary))]
            public bool Binary { get; set; }
            [Option('d', "decimal", Required = false, HelpText = "decimal representation", SetName = nameof(Representations.Decimal))]
            public bool Decimal { get; set; }
            [Option('x', "hexadecimal", Required = false, HelpText = "hexadecimal representation", SetName = nameof(Representations.Hexadecimal))]
            public bool Hexadecimal { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       if(!File.Exists(o.InputPath))
                       {
                           Console.WriteLine("No such file");
                       }

                       byte[] buf;
                       using (MemoryStream data = new MemoryStream())
                       {
                           using (Stream file = File.OpenRead(o.InputPath))
                           {
                               file.CopyTo(data);
                               buf = data.ToArray();
                           }
                       }

                       int pad = 10;
                       int length = buf.Length;
                       BOMs bom = BOMDetector.DetectBOM(buf);
                       Console.WriteLine($"{nameof(length).PadRight(pad)} : {length}");
                       Console.WriteLine($"{nameof(bom).PadRight(pad)} : {bom}");

                       int baseBit = int.MinValue;
                       Func<string, string> converter = null;
                       if (o.Binary)
                       {
                           baseBit = 2;
                           converter = (s) => (s.PadLeft(8, '0'));
                       }
                       else if(o.Decimal)
                       {
                           baseBit = 10;
                           converter = (s) => (s.PadLeft(3, '0'));
                       }
                       else if(o.Hexadecimal)
                       {
                           baseBit = 16;
                           converter = (s) => ("0x" + s.PadLeft(2, '0'));
                       }
                       else
                       {
                           baseBit = 16;
                           converter = (s) => ("0x" + s.PadLeft(2, '0'));
                       }

                       foreach (var b in buf)
                       {
                           var val = converter(Convert.ToString(b, baseBit));
                           Console.Write(val + " ");
                       }
                       Console.WriteLine();
                   });
        }
    }
}
