using System.Text;

namespace UnicodeStyle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Regular: Hello, World!");

            using (UnicodeStyler? styler = new())
            {
                foreach (string? style in styler.Styles)
                {
                    Console.WriteLine($"{style}: {styler.StyleConvert("Hello, World!", style)}");
                }
            }

            Console.WriteLine($"Underline: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.Underline)}");
            Console.WriteLine($"Double Underline: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.DoubleUnderline)}");
            Console.WriteLine($"Overline: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.Overline)}");
            Console.WriteLine($"Strikethrough: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.Strikethrough)}");
            Console.WriteLine($"Strikethrough Vertical: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.StrikethroughVertical)}");
            Console.WriteLine($"Slashthrough: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.Slashthrough)}");
            Console.WriteLine($"Double Slashthrough: {UnicodeStyler.AddLine("Hello, World!", UnicodeLines.DoubleSlashthrough)}");

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}