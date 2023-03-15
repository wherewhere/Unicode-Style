using UnicodeStyle.Models;

namespace UnicodeStyle
{
    /// <summary>
    /// The tools to add lines for Unicode strings.
    /// </summary>
    public static class UnicodeLiner
    {
        private const ushort CombiningFirst = 0x0300;   // U+0300
        private const ushort CombiningLast = 0x20F0;    // U+20F0

        private const ushort HighSurrogateFirst = 0xD800;   // U+D800
        private const ushort HighSurrogateLast = 0xDBFF;    // U+DBFF

        /// <summary>
        /// Add Unicode Combining Diacritical Marks.
        /// </summary>
        /// <param name="str">The string you want to add lines.</param>
        /// <param name="lines">Lines you want to add.</param>
        /// <returns>The lines-added string.</returns>
        public static string AddLine(string str, params UnicodeLines[] lines) => AddLine(str, false, lines);

        /// <summary>
        /// Add Unicode Combining Diacritical Marks.
        /// </summary>
        /// <param name="str">The string you want to add lines.</param>
        /// <param name="removeLine">Remove the lines you want to add if exist in the string.</param>
        /// <param name="lines">Lines you want to add.</param>
        /// <returns>The lines-added string.</returns>
        public static string AddLine(string str, bool removeLine, params UnicodeLines[] lines)
        {
            string input = removeLine ? RemoveLine(str, lines) : str;

            if (lines != null)
            {
                string output = string.Empty;
                for (int i = 0; i < input.Length; i++)
                {
                    ushort cp = input[i];
                    output += (char)cp;
                    if (cp is < CombiningFirst or (> CombiningLast and < HighSurrogateFirst) or > HighSurrogateLast)
                    {
                        for (int j = 0; j < lines.Length; j++)
                        {
                            UnicodeLines line = lines[j];
                            output += (char)line;
                        }
                    }
                }
                return output;
            }

            return input;
        }

        /// <summary>
        /// Remove Unicode Lines.
        /// </summary>
        /// <param name="str">The string you want to remove lines.</param>
        /// <returns>The lines-removed string.</returns>
        public static string RemoveLine(string str)
        {
            string input = str;
            string output = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                ushort word = input[i];
                if (word is < CombiningFirst or > CombiningLast)
                {
                    output += (char)word;
                }
            }

            return output;
        }

        /// <summary>
        /// Remove Unicode Lines.
        /// </summary>
        /// <param name="str">The string you want to remove lines.</param>
        /// <param name="lines">Lines you want to remove.</param>
        /// <returns>The lines-removed string.</returns>
        public static string RemoveLine(string str, params UnicodeLines[] lines)
        {
            string input = str;
            string output = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                bool isLine = false;
                char word = input[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    UnicodeLines line = lines[j];
                    if ((char)line == word)
                    {
                        isLine = true;
                        break;
                    }
                }
                if (!isLine) { output += word; }
            }

            return output;
        }
    }
}
