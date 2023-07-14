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

            using (UnicodeStyle UnicodeStyle = new())
            {
                foreach (object style in Enum.GetValues(typeof(UnicodeStyles)))
                {
                    UnicodeStyles type = (UnicodeStyles)style;
                    Console.WriteLine($"{type}: {UnicodeStyle.StyleConvert("Hello, World!", type)}");
                }
            }

            Console.WriteLine($"Underline: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.Underline)}");
            Console.WriteLine($"Double Underline: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.DoubleUnderline)}");
            Console.WriteLine($"Overline: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.Overline)}");
            Console.WriteLine($"Strikethrough: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.LongStrokeOverlay)}");
            Console.WriteLine($"Strikethrough Vertical: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.StrikethroughVertical)}");
            Console.WriteLine($"Slashthrough: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.LongSlashOverlay)}");
            Console.WriteLine($"Double Slashthrough: {UnicodeLine.AddLine("Hello, World!", UnicodeLines.LongDoubleSolidusOverlay)}");

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}