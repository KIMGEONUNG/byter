using Byter.Enums;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using byter.Utilities;
using byter.Enums;

namespace Byter
{
    class Program
    {
        public enum Representations
        {
            Binary,
            Decimal,
            Hexadecimal,
            Ascii,
            UTF8,
            INTEGER,
            DOUBLE,
        }

        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "input file path")]
            public string InputPath { get; set; }

            [Option('r', "range", Required = false, Default =null, HelpText = "Set the inspection byte range. For example, if you want to inspect from 5 to 8, the argument is \"5-8\". Note that the start number is zero")]
            public string Range { get; set; }

            [Option('b',"binary", Required = false, HelpText = "binary representation",SetName = nameof(Representations.Binary))]
            public bool Binary { get; set; }

            [Option('d', "decimal", Required = false, HelpText = "decimal representation", SetName = nameof(Representations.Decimal))]
            public bool Decimal { get; set; }

            [Option('x', "hexadecimal", Required = false, HelpText = "hexadecimal representation", SetName = nameof(Representations.Hexadecimal))]
            public bool Hexadecimal { get; set; }

            [Option("ascii", Required = false, HelpText = "ascii representation", SetName = nameof(Representations.Ascii))]
            public bool Ascii { get; set; }

            [Option("utf8", Required = false, HelpText = "utf8 representation", SetName = nameof(Representations.UTF8))]
            public bool UTF8 { get; set; }

            [Option("int", Required = false, HelpText = "interger data representation", SetName = nameof(Representations.INTEGER))]
            public bool INT { get; set; }

            [Option("double", Required = false, HelpText = "double data representation", SetName = nameof(Representations.DOUBLE))]
            public bool DOUBLE { get; set; }

            [Option("endian", Required = false, HelpText = "Only for --int or --double, the target is \"big\" or \"little\", and the default is \"big\"", Default ="big")]
            public string endian  { get; set; }

            [Option('s', "summery", Required = false, HelpText = "print summary")]
            public bool Summary { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       if(!File.Exists(o.InputPath))
                       {
                           Console.WriteLine("No such file");
                           return;
                       }

                       byte[] buf;
                       using (MemoryStream data = new MemoryStream())
                       {
                           using (Stream file = File.OpenRead(o.InputPath))
                           {
                               file.CopyTo(data);
                               buf = data.ToArray();

                               if (o.Range != null)
                               {
                                   string rangeStr = o.Range;
                                   Regex regex = new Regex(@"^(\d+)-(\d+)");
                                   if (!regex.IsMatch(rangeStr))
                                   {
                                       Console.WriteLine("Invalid range text format. Check the \"help\" text.");
                                       return;
                                   }
                                   Match match = regex.Match(rangeStr);
                                   var groups = match.Groups;

                                   if (groups.Count != 3)
                                   {
                                       throw new InvalidOperationException($"Regex math count is not 3. but {groups.Count}");
                                   }

                                   int start = Convert.ToInt32(groups[1].Value);
                                   int end = Convert.ToInt32(groups[2].Value);

                                   if (end < start)
                                   {
                                       Console.WriteLine("The end range value must be bigger than the start range value");
                                       return;
                                   }

                                   buf = buf.ToList().GetRange(start, end - start + 1).ToArray();

                               }
                           }
                       }

                       if (o.Summary)
                       {
                           int pad = 10;
                           int length = buf.Length;
                           BOMs bom = BOMDetector.DetectBOM(buf);
                           Console.WriteLine($"{nameof(length).PadRight(pad)} : {length}");
                           Console.WriteLine($"{nameof(bom).PadRight(pad)} : {bom}");
                       }

                       int baseBit = int.MinValue;
                       Func<string, string> converter = null;
                       if (o.Binary)
                       {
                           baseBit = 2;
                           converter = (s) => (s.PadLeft(8, '0'));
                       }
                       else if (o.Decimal)
                       {
                           baseBit = 10;
                           converter = (s) => (s.PadLeft(3, '0'));
                       }
                       else if (o.Hexadecimal)
                       {
                           baseBit = 16;
                           converter = (s) => ("0x" + s.PadLeft(2, '0'));
                       }
                       else if (o.Ascii)
                       {
                           string contents = Encoding.ASCII.GetString(buf);
                           Console.WriteLine(contents);
                           return;
                       }
                       else if (o.UTF8)
                       {
                           string contents = Encoding.UTF8.GetString(buf);
                           Console.WriteLine(contents);
                           return;
                       }
                       else if (o.INT)
                       {
                           if (buf.Length % 4 != 0)
                           {
                               Console.WriteLine(" if you want to use integer data representation, the length of bytes must be multiples of 4.");
                               return;
                           }

                           int[] vals = null;
                           if (o.endian == "big")
                           {
                                vals = ByteToNumber.ToInt(buf, Endian.Big);
                           }
                           else if (o.endian == "little")
                           {
                                vals = ByteToNumber.ToInt(buf, Endian.Little);
                           }
                           else
                           {
                               Console.WriteLine("endian option must be \"big\" or \"little\"");
                               return;
                           }

                           foreach (var val in vals)
                           {
                               Console.Write(val);
                               Console.Write(" ");
                           }

                           return;
                       }
                       else if (o.DOUBLE)
                       {
                           if (buf.Length % 8 != 0)
                           {
                               Console.WriteLine(" if you want to use integer data representation, the length of bytes must be multiples of 8.");
                               return;
                           }

                           double[] vals = null;
                           if (o.endian == "big")
                           {
                                vals = ByteToNumber.ToDouble(buf, Endian.Big);
                           }
                           else if (o.endian == "little")
                           {
                                vals = ByteToNumber.ToDouble(buf, Endian.Little);
                           }
                           else
                           {
                               Console.WriteLine("endian option must be \"big\" or \"little\"");
                               return;
                           }

                           foreach (var val in vals)
                           {
                               Console.Write(val);
                               Console.Write(" ");
                           }
                           return;
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
