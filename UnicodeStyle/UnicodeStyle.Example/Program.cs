using System;
using System.Text;
using UnicodeStyle.Models;

namespace UnicodeStyle
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Regular: Hello, World!");

            using (UnicodeStyler styler = new())
            {
                foreach (object style in Enum.GetValues(typeof(UnicodeStyles)))
                {
                    UnicodeStyles type = (UnicodeStyles)style;
                    Console.WriteLine($"{type}: {styler.StyleConvert("Hello, World!", type)}");
                }
            }

            //for (ushort line = 0x0300; line <= 0x20F0; line++)
            //{
            //    UnicodeLines type = (UnicodeLines)line;
            //    Console.WriteLine($"{type}: {UnicodeLiner.AddLine("Hello, World!", type)}");
            //}

            Console.WriteLine($"Underline: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.Underline)}");
            Console.WriteLine($"Double Underline: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.DoubleUnderline)}");
            Console.WriteLine($"Overline: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.Overline)}");
            Console.WriteLine($"Strikethrough: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.LongStrokeOverlay)}");
            Console.WriteLine($"Strikethrough Vertical: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.StrikethroughVertical)}");
            Console.WriteLine($"Slashthrough: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.LongSlashOverlay)}");
            Console.WriteLine($"Double Slashthrough: {UnicodeLiner.AddLine("Hello, World!", UnicodeLines.LongDoubleSolidusOverlay)}");

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
            //while (true)
            //{
            //    string a = Console.ReadLine();
            //    string[] b = a.Split(',');
            //    Console.WriteLine(string.Join(", ", b.Select((x) => x.Trim() != "0" ? $"0x{Convert.ToString(Convert.ToInt64(x.Trim()), 16).ToUpperInvariant()}" : "0")));
            //}
        }
    }
}