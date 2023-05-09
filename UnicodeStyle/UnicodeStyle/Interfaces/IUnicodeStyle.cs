using UnicodeStyle.Models;

namespace UnicodeStyle
{
    /// <summary>
    /// The tools to style Unicode strings.
    /// </summary>
    public interface IUnicodeStyle
    {
#if WINRT
        /// <summary>
        /// Convert the string to regular style.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The styled string.</returns>
        string StyleConvert(string str);
#endif

        /// <summary>
        /// Convert the string to target type.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="style">The style you want.</param>
        /// <returns>The styled string.</returns>
        string StyleConvert(string str, UnicodeStyles style);
    }
}
