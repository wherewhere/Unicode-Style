using System.Text;

namespace UnicodeStyle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Regular: Hello, World!");
            foreach (var style in UnicodeStyler.Styles)
            {
                Console.WriteLine($"{style}: {UnicodeStyler.StyleConvert("Hello, World!", style)}");
            }

            string? a = Console.ReadLine();
            int[] b = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = Convert.ToInt32(a[i]);
            }

            char[] c = new char[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                c[i] = Convert.ToChar(b[i]);
            }

            Console.ReadKey(true);
        }
    }
}