using NUnit.Framework;
using UnicodeStyle.Models;

namespace UnicodeStyle.Tests
{
    /// <summary>
    /// Tests the <see cref="UnicodeLine"/> class.
    /// </summary>
    [TestFixture]
    public class UnicodeLineTests
    {
        /// <summary>
        /// Tests the <see cref="UnicodeLine.AddLine(string, UnicodeLines[])"/> method.
        /// </summary>
        [TestCase("H̲e̲l̲l̲o̲,̲ ̲W̲o̲r̲l̲d̲!̲", UnicodeLines.Underline)]
        [TestCase("H̳e̳l̳l̳o̳,̳ ̳W̳o̳r̳l̳d̳!̳", UnicodeLines.DoubleUnderline)]
        [TestCase("H̅e̅l̅l̅o̅,̅ ̅W̅o̅r̅l̅d̅!̅", UnicodeLines.Overline)]
        [TestCase("H̶e̶l̶l̶o̶,̶ ̶W̶o̶r̶l̶d̶!̶", UnicodeLines.LongStrokeOverlay)]
        [TestCase("H⃦e⃦l⃦l⃦o⃦,⃦ ⃦W⃦o⃦r⃦l⃦d⃦!⃦", UnicodeLines.StrikethroughVertical)]
        [TestCase("H̸e̸l̸l̸o̸,̸ ̸W̸o̸r̸l̸d̸!̸", UnicodeLines.LongSlashOverlay)]
        [TestCase("H⃫e⃫l⃫l⃫o⃫,⃫ ⃫W⃫o⃫r⃫l⃫d⃫!⃫", UnicodeLines.LongDoubleSolidusOverlay)]
        public void AddLineTest(string result, UnicodeLines line)
        {
            string Regular = "Hello, World!";
            Assert.That(UnicodeLine.AddLine(Regular, line), Is.EqualTo(result));
        }

        /// <summary>
        /// Tests the <see cref="UnicodeLine.RemoveLine(string)"/> method.
        /// </summary>
        [TestCase(UnicodeLines.Underline)]
        [TestCase(UnicodeLines.DoubleUnderline)]
        [TestCase(UnicodeLines.Overline)]
        [TestCase(UnicodeLines.LongStrokeOverlay)]
        [TestCase(UnicodeLines.StrikethroughVertical)]
        [TestCase(UnicodeLines.LongSlashOverlay)]
        [TestCase(UnicodeLines.LongDoubleSolidusOverlay)]
        public void RemoveLineTest(UnicodeLines line)
        {
            string regular = "Hello, World!";
            Assert.That(UnicodeLine.RemoveLine(UnicodeLine.AddLine(regular, line)), Is.EqualTo(regular));
        }
    }
}
