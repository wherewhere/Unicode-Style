using NUnit.Framework;
using UnicodeStyle.Models;

namespace UnicodeStyle.Tests
{
    /// <summary>
    /// Tests the <see cref="UnicodeStyle"/> class.
    /// </summary>
    [TestFixture]
    public class UnicodeStyleTests
    {
        /// <summary>
        /// Tests the <see cref="UnicodeStyle.StyleConvert(string, UnicodeStyles)"/> method.
        /// </summary>
        [TestCase("Hello, World!", UnicodeStyles.Regular)]
        [TestCase("𝐇𝐞𝐥𝐥𝐨, 𝐖𝐨𝐫𝐥𝐝!", UnicodeStyles.Bold)]
        [TestCase("𝐻𝑒𝑙𝑙𝑜, 𝑊𝑜𝑟𝑙𝑑!", UnicodeStyles.Italic)]
        [TestCase("𝑯𝒆𝒍𝒍𝒐, 𝑾𝒐𝒓𝒍𝒅!", UnicodeStyles.BoldItalic)]
        [TestCase("𝖧𝖾𝗅𝗅𝗈, 𝖶𝗈𝗋𝗅𝖽!", UnicodeStyles.SansSerif)]
        [TestCase("𝗛𝗲𝗹𝗹𝗼, 𝗪𝗼𝗿𝗹𝗱!", UnicodeStyles.SansSerifBold)]
        [TestCase("𝘏𝘦𝘭𝘭𝘰, 𝘞𝘰𝘳𝘭𝘥!", UnicodeStyles.SansSerifItalic)]
        [TestCase("𝙃𝙚𝙡𝙡𝙤, 𝙒𝙤𝙧𝙡𝙙!", UnicodeStyles.SansSerifBoldItalic)]
        [TestCase("ℋℯ𝓁𝓁ℴ, 𝒲ℴ𝓇𝓁𝒹!", UnicodeStyles.Script)]
        [TestCase("𝓗𝓮𝓵𝓵𝓸, 𝓦𝓸𝓻𝓵𝓭!", UnicodeStyles.ScriptBold)]
        [TestCase("ℌ𝔢𝔩𝔩𝔬, 𝔚𝔬𝔯𝔩𝔡!", UnicodeStyles.Fraktur)]
        [TestCase("𝕳𝖊𝖑𝖑𝖔, 𝖂𝖔𝖗𝖑𝖉!", UnicodeStyles.FrakturBold)]
        [TestCase("ℍ𝕖𝕝𝕝𝕠, 𝕎𝕠𝕣𝕝𝕕!", UnicodeStyles.DoubleStruck)]
        [TestCase("𝙷𝚎𝚕𝚕𝚘, 𝚆𝚘𝚛𝚕𝚍!", UnicodeStyles.Monospace)]
        [TestCase("Ｈｅｌｌｏ，　Ｗｏｒｌｄ！", UnicodeStyles.Fullwidth)]
        [TestCase("Ⓗⓔⓛⓛⓞ, Ⓦⓞⓡⓛⓓ!", UnicodeStyles.Circled)]
        [TestCase("🅗ello, 🅦orld!", UnicodeStyles.InverseCircled)]
        [TestCase("🄷ello, 🅆orld!", UnicodeStyles.Squared)]
        [TestCase("🅷ello, 🆆orld!", UnicodeStyles.InverseSquared)]
        [TestCase("🄗⒠⒧⒧⒪, 🄦⒪⒭⒧⒟!", UnicodeStyles.Parenthesized)]
        [TestCase("ʜello, ᴡorld!", UnicodeStyles.SmallCapitals)]
        [TestCase("ᴴᵉˡˡᵒ, ᵂᵒʳˡᵈ!", UnicodeStyles.Superscript)]
        [TestCase("Hₑₗₗₒ, Wₒᵣₗd!", UnicodeStyles.Subscript)]
        [TestCase("🇭ello, 🇼orld!", UnicodeStyles.RegionalIndicatorSymbols)]
        [TestCase("󠁈󠁥󠁬󠁬󠁯󠀬󠀠󠁗󠁯󠁲󠁬󠁤󠀡", UnicodeStyles.Tags)]
        public void StyleConvertTest(string result, UnicodeStyles style)
        {
            string Regular = "Hello, World!";
            using UnicodeStyle UnicodeStyle = new();
            Assert.AreEqual(result, UnicodeStyle.StyleConvert(Regular, style));
        }
    }
}
