using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeStyle.Tests
{
    /// <summary>
    /// Tests the <see cref="UnicodeStyler"/> class.
    /// </summary>
    [TestFixture]
    public class UnicodeStylerTests
    {
        [Test]
        public void StyleConvertTest()
        {
            string Regular = "Hello, World!";

            using (UnicodeStyler styler = new UnicodeStyler())
            {
                Assert.AreEqual("𝐇𝐞𝐥𝐥𝐨, 𝐖𝐨𝐫𝐥𝐝!", styler.StyleConvert(Regular, UnicodeStyles.Bold));
                Assert.AreEqual("𝐻𝑒𝑙𝑙𝑜, 𝑊𝑜𝑟𝑙𝑑!", styler.StyleConvert(Regular, UnicodeStyles.Italic));
                Assert.AreEqual("𝑯𝒆𝒍𝒍𝒐, 𝑾𝒐𝒓𝒍𝒅!", styler.StyleConvert(Regular, UnicodeStyles.BoldItalic));
                Assert.AreEqual("𝖧𝖾𝗅𝗅𝗈, 𝖶𝗈𝗋𝗅𝖽!", styler.StyleConvert(Regular, UnicodeStyles.SansSerif));
                Assert.AreEqual("𝗛𝗲𝗹𝗹𝗼, 𝗪𝗼𝗿𝗹𝗱!", styler.StyleConvert(Regular, UnicodeStyles.SansSerifBold));
                Assert.AreEqual("𝘏𝘦𝘭𝘭𝘰, 𝘞𝘰𝘳𝘭𝘥!", styler.StyleConvert(Regular, UnicodeStyles.SansSerifItalic));
                Assert.AreEqual("𝙃𝙚𝙡𝙡𝙤, 𝙒𝙤𝙧𝙡𝙙!", styler.StyleConvert(Regular, UnicodeStyles.SansSerifBoldItalic));
                Assert.AreEqual("ℋℯ𝓁𝓁ℴ, 𝒲ℴ𝓇𝓁𝒹!", styler.StyleConvert(Regular, UnicodeStyles.Script));
                Assert.AreEqual("𝓗𝓮𝓵𝓵𝓸, 𝓦𝓸𝓻𝓵𝓭!", styler.StyleConvert(Regular, UnicodeStyles.ScriptBold));
                Assert.AreEqual("ℌ𝔢𝔩𝔩𝔬, 𝔚𝔬𝔯𝔩𝔡!", styler.StyleConvert(Regular, UnicodeStyles.Fraktur));
                Assert.AreEqual("𝕳𝖊𝖑𝖑𝖔, 𝖂𝖔𝖗𝖑𝖉!", styler.StyleConvert(Regular, UnicodeStyles.FrakturBold));
                Assert.AreEqual("ℍ𝕖𝕝𝕝𝕠, 𝕎𝕠𝕣𝕝𝕕!", styler.StyleConvert(Regular, UnicodeStyles.DoubleStruck));
                Assert.AreEqual("𝙷𝚎𝚕𝚕𝚘, 𝚆𝚘𝚛𝚕𝚍!", styler.StyleConvert(Regular, UnicodeStyles.Monospace));
                Assert.AreEqual("Ｈｅｌｌｏ，　Ｗｏｒｌｄ！", styler.StyleConvert(Regular, UnicodeStyles.Fullwidth));
                Assert.AreEqual("Ⓗⓔⓛⓛⓞ, Ⓦⓞⓡⓛⓓ!", styler.StyleConvert(Regular, UnicodeStyles.Circled));
                Assert.AreEqual("🅗ello, 🅦orld!", styler.StyleConvert(Regular, UnicodeStyles.InverseCircled));
                Assert.AreEqual("🄷ello, 🅆orld!", styler.StyleConvert(Regular, UnicodeStyles.Squared));
                Assert.AreEqual("🅷ello, 🆆orld!", styler.StyleConvert(Regular, UnicodeStyles.InverseSquared));
                Assert.AreEqual("🄗⒠⒧⒧⒪, 🄦⒪⒭⒧⒟!", styler.StyleConvert(Regular, UnicodeStyles.Parenthesized));
                Assert.AreEqual("ʜello, ᴡorld!", styler.StyleConvert(Regular, UnicodeStyles.SmallCapitals));
                Assert.AreEqual("ᴴᵉˡˡᵒ, ᵂᵒʳˡᵈ!", styler.StyleConvert(Regular, UnicodeStyles.Superscript));
                Assert.AreEqual("Hₑₗₗₒ, Wₒᵣₗd!", styler.StyleConvert(Regular, UnicodeStyles.Subscript));
                Assert.AreEqual("🇭ello, 🇼orld!", styler.StyleConvert(Regular, UnicodeStyles.RegionalIndicatorSymbols));
                Assert.AreEqual("󠁈󠁥󠁬󠁬󠁯󠀬󠀠󠁗󠁯󠁲󠁬󠁤󠀡", styler.StyleConvert(Regular, UnicodeStyles.Tags));
            }
        }

        [Test]
        public void AddLineTest()
        {
            string Regular = "Hello, World!";

            Assert.AreEqual("H̲e̲l̲l̲o̲,̲ ̲W̲o̲r̲l̲d̲!̲", UnicodeStyler.AddLine(Regular, UnicodeLines.Underline));
            Assert.AreEqual("H̳e̳l̳l̳o̳,̳ ̳W̳o̳r̳l̳d̳!̳", UnicodeStyler.AddLine(Regular, UnicodeLines.DoubleUnderline));
            Assert.AreEqual("H̅e̅l̅l̅o̅,̅ ̅W̅o̅r̅l̅d̅!̅", UnicodeStyler.AddLine(Regular, UnicodeLines.Overline));
            Assert.AreEqual("H̶e̶l̶l̶o̶,̶ ̶W̶o̶r̶l̶d̶!̶", UnicodeStyler.AddLine(Regular, UnicodeLines.Strikethrough));
            Assert.AreEqual("H⃦e⃦l⃦l⃦o⃦,⃦ ⃦W⃦o⃦r⃦l⃦d⃦!⃦", UnicodeStyler.AddLine(Regular, UnicodeLines.StrikethroughVertical));
            Assert.AreEqual("H̸e̸l̸l̸o̸,̸ ̸W̸o̸r̸l̸d̸!̸", UnicodeStyler.AddLine(Regular, UnicodeLines.Slashthrough));
            Assert.AreEqual("H⃫e⃫l⃫l⃫o⃫,⃫ ⃫W⃫o⃫r⃫l⃫d⃫!⃫", UnicodeStyler.AddLine(Regular, UnicodeLines.DoubleSlashthrough));
        }

        [Test]
        public void RemoveLineTest()
        {
            string Regular = "Hello, World!";

            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.Underline)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.DoubleUnderline)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.Overline)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.Strikethrough)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.StrikethroughVertical)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.Slashthrough)));
            Assert.AreEqual(Regular, UnicodeStyler.RemoveLine(UnicodeStyler.AddLine(Regular, UnicodeLines.DoubleSlashthrough)));
        }
    }
}
