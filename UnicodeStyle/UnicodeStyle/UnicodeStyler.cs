// Source : <http://www.babelstone.co.uk/Unicode/yi.js>
// For use by :
//	Unicode Text Styler at <http://www.babelstone.co.uk/Unicode/text.html>
// Title : text.js
// Author : Andrew West
// Date Created : 2014-07-05
// Latest Version : 2021-12-05 (update for Unicode 14.0f; added superscript Latin capital letters)
//

// Creative Commons Licence:
// CC BY-SA 3.0f <http://creativecommons.org/licenses/by-sa/3.0f/>
// text.js by Andrew West is licensed under a Creative Commons Attribution-ShareAlike 3.0f Unported License

using System;

namespace UnicodeStyle
{
    /// <summary>
    /// Styler of Unicode.
    /// </summary>
    public class UnicodeStyler : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Unicode Lines char.
        /// </summary>
        public static char[] UnicodeLines = new char[] { '̲', '̳', '̅', '̶', '⃦', '̸', '⃫' };

        // UTF-8 Constants
        private const ushort ReplacementCharacter = 0xFFFD;  // U+FFFD REPLACEMENT CHARACTER
        private const int CharactersPerPlane = 65536;
        private const ushort HighSurrogateFirst = 0xD800;        // U+D800
        private const ushort HighSurrogateLast = 0xDBFF;     // U+DBFF
        private const ushort LowSurrogateFirst = 0xDC00;     // U+DC00
        private const ushort LowSurrogateLast = 0xDFFF;      // U+DFFF
        private const ushort HalfShift = 10;
        private const int HalfBase = 65536;       // 0x10000;
        private const ushort HalfMask = 0x03FF;

        // Other constants
        private const ushort BasicLatinChars = 95;
        private const ushort BasicLatinFirst = 32;       // U+0020
        private const ushort BasicLatinLast = 126;       // U+007E
        private const ushort MathLatinRange = 52;
        private const int MathLatinFirst = 119808;    // U+1D400
        private const int MathLatinLast = 120483; // U+1D6A3
        private const ushort MathGreekRange = 58;
        private const int MathGreekFirst = 120488;    // U+1D6A8
        private const int MathGreekLast = 120777; // U+1D7C9
        private const ushort MathDigitsRange = 10;
        private const int MathDigitsFirst = 120782;   // U+1D7CE
        private const int MathDigitsLast = 120831;    // U+1D7FF
        private const ushort LatinDigitsOffset = 16;
        private const ushort LatinCapitalOffset = 33;
        private const ushort LatinSmallOffset = 65;
        private const ushort GreekChars = 58;
        private const ushort FirstTarget = 178;          // U+00B2
        private const ushort TotalStyles = 24;
        private const ushort GreekStyles = 7;        // Actually only 5, but we include S/S and S/S Italic in the table for ease of mapping

        /// <summary>
        /// Styles of Unicode.
        /// </summary>
        public string[] Styles;

        private void InitStyles()
        {
            Styles = new string[TotalStyles];
            Styles[0] = "Bold";
            Styles[1] = "Italic";
            Styles[2] = "BoldItalic";
            Styles[3] = "SansSerif";
            Styles[4] = "SansSerifBold";
            Styles[5] = "SansSerifItalic";
            Styles[6] = "SansSerifBoldItalic";
            Styles[7] = "Script";
            Styles[8] = "ScriptBold";
            Styles[9] = "Fraktur";
            Styles[10] = "FrakturBold";
            Styles[11] = "DoubleStruck";
            Styles[12] = "Monospace";
            Styles[13] = "Fullwidth";
            Styles[14] = "Circled";
            Styles[15] = "InverseCircled";
            Styles[16] = "Squared";
            Styles[17] = "InverseSquared";
            Styles[18] = "Parenthesized";
            Styles[19] = "SmallCapitals";
            Styles[20] = "Superscript";
            Styles[21] = "Subscript";
            Styles[22] = "RegionalIndicatorSymbols";
            Styles[23] = "Tags";
        }

        private int[][] LatinMapping;

        private void InitLatinMapping()
        {
            LatinMapping = new int[BasicLatinChars][];
            LatinMapping[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12288, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917536 };        // U+0020: SPACE
            LatinMapping[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65281, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917537 };        // U+0021: EXCLAMATION MARK
            LatinMapping[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65282, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917538 };        // U+0022: QUOTATION MARK
            LatinMapping[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65283, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917539 };        // U+0023: NUMBER SIGN
            LatinMapping[4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65284, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917540 };        // U+0024: DOLLAR SIGN
            LatinMapping[5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65285, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917541 };        // U+0025: PERCENT SIGN
            LatinMapping[6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65286, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917542 };        // U+0026: AMPERSAND
            LatinMapping[7] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65287, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917543 };        // U+0027: APOSTROPHE
            LatinMapping[8] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65288, 0, 0, 0, 0, 0, 0, 8317, 8333, 0, 917544 };      // U+0028: LEFT PARENTHESIS
            LatinMapping[9] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65289, 0, 0, 0, 0, 0, 0, 8318, 8334, 0, 917545 };      // U+0029: RIGHT PARENTHESIS
            LatinMapping[10] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65290, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917546 };       // U+002A: ASTERISK
            LatinMapping[11] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65291, 0, 0, 0, 0, 0, 0, 8314, 8330, 0, 917547 };     // U+002B: PLUS SIGN
            LatinMapping[12] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65292, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917548 };       // U+002C: COMMA
            LatinMapping[13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65293, 0, 0, 0, 0, 0, 0, 8315, 8331, 0, 917549 };     // U+002D: HYPHEN-MINUS
            LatinMapping[14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65294, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917550 };       // U+002E: FULL STOP
            LatinMapping[15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65295, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917551 };       // U+002F: SOLIDUS
            LatinMapping[16] = new int[] { 120782, 0, 0, 120802, 120812, 0, 0, 0, 0, 0, 0, 120792, 120822, 65296, 9450, 9471, 0, 0, 0, 0, 8304, 8320, 0, 917552 };      // U+0030: DIGIT ZERO
            LatinMapping[17] = new int[] { 120783, 0, 0, 120803, 120813, 0, 0, 0, 0, 0, 0, 120793, 120823, 65297, 9312, 10102, 0, 0, 9332, 0, 185, 8321, 0, 917553 };       // U+0031: DIGIT ONE
            LatinMapping[18] = new int[] { 120784, 0, 0, 120804, 120814, 0, 0, 0, 0, 0, 0, 120794, 120824, 65298, 9313, 10103, 0, 0, 9333, 0, 178, 8322, 0, 917554 };       // U+0032: DIGIT TWO
            LatinMapping[19] = new int[] { 120785, 0, 0, 120805, 120815, 0, 0, 0, 0, 0, 0, 120795, 120825, 65299, 9314, 10104, 0, 0, 9334, 0, 179, 8323, 0, 917555 };       // U+0033: DIGIT THREE
            LatinMapping[20] = new int[] { 120786, 0, 0, 120806, 120816, 0, 0, 0, 0, 0, 0, 120796, 120826, 65300, 9315, 10105, 0, 0, 9335, 0, 8308, 8324, 0, 917556 };      // U+0034: DIGIT FOUR
            LatinMapping[21] = new int[] { 120787, 0, 0, 120807, 120817, 0, 0, 0, 0, 0, 0, 120797, 120827, 65301, 9316, 10106, 0, 0, 9336, 0, 8309, 8325, 0, 917557 };      // U+0035: DIGIT FIVE
            LatinMapping[22] = new int[] { 120788, 0, 0, 120808, 120818, 0, 0, 0, 0, 0, 0, 120798, 120828, 65302, 9317, 10107, 0, 0, 9337, 0, 8310, 8326, 0, 917558 };      // U+0036: DIGIT SIX
            LatinMapping[23] = new int[] { 120789, 0, 0, 120809, 120819, 0, 0, 0, 0, 0, 0, 120799, 120829, 65303, 9318, 10108, 0, 0, 9338, 0, 8311, 8327, 0, 917559 };      // U+0037: DIGIT SEVEN
            LatinMapping[24] = new int[] { 120790, 0, 0, 120810, 120820, 0, 0, 0, 0, 0, 0, 120800, 120830, 65304, 9319, 10109, 0, 0, 9339, 0, 8312, 8328, 0, 917560 };      // U+0038: DIGIT EIGHT
            LatinMapping[25] = new int[] { 120791, 0, 0, 120811, 120821, 0, 0, 0, 0, 0, 0, 120801, 120831, 65305, 9320, 10110, 0, 0, 9340, 0, 8313, 8329, 0, 917561 };      // U+0039: DIGIT NINE
            LatinMapping[26] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65306, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917562 };       // U+003A: COLON
            LatinMapping[27] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65307, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917563 };       // U+003B: SEMICOLON
            LatinMapping[28] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65308, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917564 };       // U+003C: LESS-THAN SIGN
            LatinMapping[29] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65309, 0, 0, 0, 0, 0, 0, 8316, 8332, 0, 917565 };     // U+003D: EQUALS SIGN
            LatinMapping[30] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65310, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917566 };       // U+003E: GREATER-THAN SIGN
            LatinMapping[31] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65311, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917567 };       // U+003F: QUESTION MARK
            LatinMapping[32] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65312, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917568 };       // U+0040: COMMERCIAL AT
            LatinMapping[33] = new int[] { 119808, 119860, 119912, 120224, 120276, 120328, 120380, 119964, 120016, 120068, 120172, 120120, 120432, 65313, 9398, 127312, 127280, 127344, 127248, 7424, 7468, 0, 127462, 917569 };        // U+0041: LATIN CAPITAL LETTER A
            LatinMapping[34] = new int[] { 119809, 119861, 119913, 120225, 120277, 120329, 120381, 8492, 120017, 120069, 120173, 120121, 120433, 65314, 9399, 127313, 127281, 127345, 127249, 665, 7470, 0, 127463, 917570 };       // U+0042: LATIN CAPITAL LETTER B
            LatinMapping[35] = new int[] { 119810, 119862, 119914, 120226, 120278, 120330, 120382, 119966, 120018, 8493, 120174, 8450, 120434, 65315, 9400, 127314, 127282, 127346, 127250, 7428, 42994, 0, 127464, 917571 };       // U+0043: LATIN CAPITAL LETTER C
            LatinMapping[36] = new int[] { 119811, 119863, 119915, 120227, 120279, 120331, 120383, 119967, 120019, 120071, 120175, 120123, 120435, 65316, 9401, 127315, 127283, 127347, 127251, 7429, 7472, 0, 127465, 917572 };        // U+0044: LATIN CAPITAL LETTER D
            LatinMapping[37] = new int[] { 119812, 119864, 119916, 120228, 120280, 120332, 120384, 8496, 120020, 120072, 120176, 120124, 120436, 65317, 9402, 127316, 127284, 127348, 127252, 7431, 7473, 0, 127466, 917573 };      // U+0045: LATIN CAPITAL LETTER E
            LatinMapping[38] = new int[] { 119813, 119865, 119917, 120229, 120281, 120333, 120385, 8497, 120021, 120073, 120177, 120125, 120437, 65318, 9403, 127317, 127285, 127349, 127253, 42800, 42995, 0, 127467, 917574 };        // U+0046: LATIN CAPITAL LETTER F
            LatinMapping[39] = new int[] { 119814, 119866, 119918, 120230, 120282, 120334, 120386, 119970, 120022, 120074, 120178, 120126, 120438, 65319, 9404, 127318, 127286, 127350, 127254, 610, 7475, 0, 127468, 917575 };     // U+0047: LATIN CAPITAL LETTER G
            LatinMapping[40] = new int[] { 119815, 119867, 119919, 120231, 120283, 120335, 120387, 8459, 120023, 8460, 120179, 8461, 120439, 65320, 9405, 127319, 127287, 127351, 127255, 668, 7476, 0, 127469, 917576 };       // U+0048: LATIN CAPITAL LETTER H
            LatinMapping[41] = new int[] { 119816, 119868, 119920, 120232, 120284, 120336, 120388, 8464, 120024, 8465, 120180, 120128, 120440, 65321, 9406, 127320, 127288, 127352, 127256, 618, 7477, 0, 127470, 917577 };     // U+0049: LATIN CAPITAL LETTER I
            LatinMapping[42] = new int[] { 119817, 119869, 119921, 120233, 120285, 120337, 120389, 119973, 120025, 120077, 120181, 120129, 120441, 65322, 9407, 127321, 127289, 127353, 127257, 7434, 7478, 0, 127471, 917578 };        // U+004A: LATIN CAPITAL LETTER J
            LatinMapping[43] = new int[] { 119818, 119870, 119922, 120234, 120286, 120338, 120390, 119974, 120026, 120078, 120182, 120130, 120442, 65323, 9408, 127322, 127290, 127354, 127258, 7435, 7479, 0, 127472, 917579 };        // U+004B: LATIN CAPITAL LETTER K
            LatinMapping[44] = new int[] { 119819, 119871, 119923, 120235, 120287, 120339, 120391, 8466, 120027, 120079, 120183, 120131, 120443, 65324, 9409, 127323, 127291, 127355, 127259, 671, 7480, 0, 127473, 917580 };       // U+004C: LATIN CAPITAL LETTER L
            LatinMapping[45] = new int[] { 119820, 119872, 119924, 120236, 120288, 120340, 120392, 8499, 120028, 120080, 120184, 120132, 120444, 65325, 9410, 127324, 127292, 127356, 127260, 7437, 7481, 0, 127474, 917581 };      // U+004D: LATIN CAPITAL LETTER M
            LatinMapping[46] = new int[] { 119821, 119873, 119925, 120237, 120289, 120341, 120393, 119977, 120029, 120081, 120185, 8469, 120445, 65326, 9411, 127325, 127293, 127357, 127261, 628, 7482, 0, 127475, 917582 };       // U+004E: LATIN CAPITAL LETTER N
            LatinMapping[47] = new int[] { 119822, 119874, 119926, 120238, 120290, 120342, 120394, 119978, 120030, 120082, 120186, 120134, 120446, 65327, 9412, 127326, 127294, 127358, 127262, 7439, 7484, 0, 127476, 917583 };        // U+004F: LATIN CAPITAL LETTER O
            LatinMapping[48] = new int[] { 119823, 119875, 119927, 120239, 120291, 120343, 120395, 119979, 120031, 120083, 120187, 8473, 120447, 65328, 9413, 127327, 127295, 127359, 127263, 7448, 7486, 0, 127477, 917584 };      // U+0050: LATIN CAPITAL LETTER P
            LatinMapping[49] = new int[] { 119824, 119876, 119928, 120240, 120292, 120344, 120396, 119980, 120032, 120084, 120188, 8474, 120448, 65329, 9414, 127328, 127296, 127360, 127264, 42927, 42996, 0, 127478, 917585 };        // U+0051: LATIN CAPITAL LETTER Q
            LatinMapping[50] = new int[] { 119825, 119877, 119929, 120241, 120293, 120345, 120397, 8475, 120033, 8476, 120189, 8477, 120449, 65330, 9415, 127329, 127297, 127361, 127265, 640, 7487, 0, 127479, 917586 };       // U+0052: LATIN CAPITAL LETTER R
            LatinMapping[51] = new int[] { 119826, 119878, 119930, 120242, 120294, 120346, 120398, 119982, 120034, 120086, 120190, 120138, 120450, 65331, 9416, 127330, 127298, 127362, 127266, 42801, 0, 0, 127480, 917587 };      // U+0053: LATIN CAPITAL LETTER S
            LatinMapping[52] = new int[] { 119827, 119879, 119931, 120243, 120295, 120347, 120399, 119983, 120035, 120087, 120191, 120139, 120451, 65332, 9417, 127331, 127299, 127363, 127267, 7451, 7488, 0, 127481, 917588 };        // U+0054: LATIN CAPITAL LETTER T
            LatinMapping[53] = new int[] { 119828, 119880, 119932, 120244, 120296, 120348, 120400, 119984, 120036, 120088, 120192, 120140, 120452, 65333, 9418, 127332, 127300, 127364, 127268, 7452, 7489, 0, 127482, 917589 };        // U+0055: LATIN CAPITAL LETTER U
            LatinMapping[54] = new int[] { 119829, 119881, 119933, 120245, 120297, 120349, 120401, 119985, 120037, 120089, 120193, 120141, 120453, 65334, 9419, 127333, 127301, 127365, 127269, 7456, 11389, 0, 127483, 917590 };       // U+0056: LATIN CAPITAL LETTER V
            LatinMapping[55] = new int[] { 119830, 119882, 119934, 120246, 120298, 120350, 120402, 119986, 120038, 120090, 120194, 120142, 120454, 65335, 9420, 127334, 127302, 127366, 127270, 7457, 7490, 0, 127484, 917591 };        // U+0057: LATIN CAPITAL LETTER W
            LatinMapping[56] = new int[] { 119831, 119883, 119935, 120247, 120299, 120351, 120403, 119987, 120039, 120091, 120195, 120143, 120455, 65336, 9421, 127335, 127303, 127367, 127271, 0, 0, 0, 127485, 917592 };      // U+0058: LATIN CAPITAL LETTER X
            LatinMapping[57] = new int[] { 119832, 119884, 119936, 120248, 120300, 120352, 120404, 119988, 120040, 120092, 120196, 120144, 120456, 65337, 9422, 127336, 127304, 127368, 127272, 655, 0, 0, 127486, 917593 };        // U+0059: LATIN CAPITAL LETTER Y
            LatinMapping[58] = new int[] { 119833, 119885, 119937, 120249, 120301, 120353, 120405, 119989, 120041, 8488, 120197, 8484, 120457, 65338, 9423, 127337, 127305, 127369, 127273, 7458, 0, 0, 127487, 917594 };       // U+005A: LATIN CAPITAL LETTER Z
            LatinMapping[59] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65339, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917595 };       // U+005B: LEFT SQUARE BRACKET
            LatinMapping[60] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65340, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917596 };       // U+005C: REVERSE SOLIDUS
            LatinMapping[61] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65341, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917597 };       // U+005D: RIGHT SQUARE BRACKET
            LatinMapping[62] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65342, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917598 };       // U+005E: CIRCUMFLEX ACCENT
            LatinMapping[63] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65343, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917599 };       // U+005F: LOW LINE
            LatinMapping[64] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65344, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917600 };       // U+0060: GRAVE ACCENT
            LatinMapping[65] = new int[] { 119834, 119886, 119938, 120250, 120302, 120354, 120406, 119990, 120042, 120094, 120198, 120146, 120458, 65345, 9424, 0, 0, 0, 9372, 0, 7491, 8336, 0, 917601 };      // U+0061: LATIN SMALL LETTER A
            LatinMapping[66] = new int[] { 119835, 119887, 119939, 120251, 120303, 120355, 120407, 119991, 120043, 120095, 120199, 120147, 120459, 65346, 9425, 0, 0, 0, 9373, 0, 7495, 0, 0, 917602 };     // U+0062: LATIN SMALL LETTER B
            LatinMapping[67] = new int[] { 119836, 119888, 119940, 120252, 120304, 120356, 120408, 119992, 120044, 120096, 120200, 120148, 120460, 65347, 9426, 0, 0, 0, 9374, 0, 7580, 0, 0, 917603 };     // U+0063: LATIN SMALL LETTER C
            LatinMapping[68] = new int[] { 119837, 119889, 119941, 120253, 120305, 120357, 120409, 119993, 120045, 120097, 120201, 120149, 120461, 65348, 9427, 0, 0, 0, 9375, 0, 7496, 0, 0, 917604 };     // U+0064: LATIN SMALL LETTER D
            LatinMapping[69] = new int[] { 119838, 119890, 119942, 120254, 120306, 120358, 120410, 8495, 120046, 120098, 120202, 120150, 120462, 65349, 9428, 0, 0, 0, 9376, 0, 7497, 8337, 0, 917605 };        // U+0065: LATIN SMALL LETTER E
            LatinMapping[70] = new int[] { 119839, 119891, 119943, 120255, 120307, 120359, 120411, 119995, 120047, 120099, 120203, 120151, 120463, 65350, 9429, 0, 0, 0, 9377, 0, 7584, 0, 0, 917606 };     // U+0066: LATIN SMALL LETTER F
            LatinMapping[71] = new int[] { 119840, 119892, 119944, 120256, 120308, 120360, 120412, 8458, 120048, 120100, 120204, 120152, 120464, 65351, 9430, 0, 0, 0, 9378, 0, 7501, 0, 0, 917607 };       // U+0067: LATIN SMALL LETTER G
            LatinMapping[72] = new int[] { 119841, 8462, 119945, 120257, 120309, 120361, 120413, 119997, 120049, 120101, 120205, 120153, 120465, 65352, 9431, 0, 0, 0, 9379, 0, 688, 8341, 0, 917608 };     // U+0068: LATIN SMALL LETTER H
            LatinMapping[73] = new int[] { 119842, 119894, 119946, 120258, 120310, 120362, 120414, 119998, 120050, 120102, 120206, 120154, 120466, 65353, 9432, 0, 0, 0, 9380, 0, 8305, 7522, 0, 917609 };      // U+0069: LATIN SMALL LETTER I
            LatinMapping[74] = new int[] { 119843, 119895, 119947, 120259, 120311, 120363, 120415, 119999, 120051, 120103, 120207, 120155, 120467, 65354, 9433, 0, 0, 0, 9381, 0, 690, 11388, 0, 917610 };      // U+006A: LATIN SMALL LETTER J
            LatinMapping[75] = new int[] { 119844, 119896, 119948, 120260, 120312, 120364, 120416, 120000, 120052, 120104, 120208, 120156, 120468, 65355, 9434, 0, 0, 0, 9382, 0, 7503, 8342, 0, 917611 };      // U+006B: LATIN SMALL LETTER K
            LatinMapping[76] = new int[] { 119845, 119897, 119949, 120261, 120313, 120365, 120417, 120001, 120053, 120105, 120209, 120157, 120469, 65356, 9435, 0, 0, 0, 9383, 0, 737, 8343, 0, 917612 };       // U+006C: LATIN SMALL LETTER L
            LatinMapping[77] = new int[] { 119846, 119898, 119950, 120262, 120314, 120366, 120418, 120002, 120054, 120106, 120210, 120158, 120470, 65357, 9436, 0, 0, 0, 9384, 0, 7504, 8344, 0, 917613 };      // U+006D: LATIN SMALL LETTER M
            LatinMapping[78] = new int[] { 119847, 119899, 119951, 120263, 120315, 120367, 120419, 120003, 120055, 120107, 120211, 120159, 120471, 65358, 9437, 0, 0, 0, 9385, 0, 8319, 8345, 0, 917614 };      // U+006E: LATIN SMALL LETTER N
            LatinMapping[79] = new int[] { 119848, 119900, 119952, 120264, 120316, 120368, 120420, 8500, 120056, 120108, 120212, 120160, 120472, 65359, 9438, 0, 0, 0, 9386, 0, 7506, 8338, 0, 917615 };        // U+006F: LATIN SMALL LETTER O
            LatinMapping[80] = new int[] { 119849, 119901, 119953, 120265, 120317, 120369, 120421, 120005, 120057, 120109, 120213, 120161, 120473, 65360, 9439, 0, 0, 0, 9387, 0, 7510, 8346, 0, 917616 };      // U+0070: LATIN SMALL LETTER P
            LatinMapping[81] = new int[] { 119850, 119902, 119954, 120266, 120318, 120370, 120422, 120006, 120058, 120110, 120214, 120162, 120474, 65361, 9440, 0, 0, 0, 9388, 0, 67493, 0, 0, 917617 };        // U+0071: LATIN SMALL LETTER Q
            LatinMapping[82] = new int[] { 119851, 119903, 119955, 120267, 120319, 120371, 120423, 120007, 120059, 120111, 120215, 120163, 120475, 65362, 9441, 0, 0, 0, 9389, 0, 691, 7523, 0, 917618 };       // U+0072: LATIN SMALL LETTER R
            LatinMapping[83] = new int[] { 119852, 119904, 119956, 120268, 120320, 120372, 120424, 120008, 120060, 120112, 120216, 120164, 120476, 65363, 9442, 0, 0, 0, 9390, 0, 738, 8347, 0, 917619 };       // U+0073: LATIN SMALL LETTER S
            LatinMapping[84] = new int[] { 119853, 119905, 119957, 120269, 120321, 120373, 120425, 120009, 120061, 120113, 120217, 120165, 120477, 65364, 9443, 0, 0, 0, 9391, 0, 7511, 8348, 0, 917620 };      // U+0074: LATIN SMALL LETTER T
            LatinMapping[85] = new int[] { 119854, 119906, 119958, 120270, 120322, 120374, 120426, 120010, 120062, 120114, 120218, 120166, 120478, 65365, 9444, 0, 0, 0, 9392, 0, 7512, 7524, 0, 917621 };      // U+0075: LATIN SMALL LETTER U
            LatinMapping[86] = new int[] { 119855, 119907, 119959, 120271, 120323, 120375, 120427, 120011, 120063, 120115, 120219, 120167, 120479, 65366, 9445, 0, 0, 0, 9393, 0, 7515, 7525, 0, 917622 };      // U+0076: LATIN SMALL LETTER V
            LatinMapping[87] = new int[] { 119856, 119908, 119960, 120272, 120324, 120376, 120428, 120012, 120064, 120116, 120220, 120168, 120480, 65367, 9446, 0, 0, 0, 9394, 0, 695, 0, 0, 917623 };      // U+0077: LATIN SMALL LETTER W
            LatinMapping[88] = new int[] { 119857, 119909, 119961, 120273, 120325, 120377, 120429, 120013, 120065, 120117, 120221, 120169, 120481, 65368, 9447, 0, 0, 0, 9395, 0, 739, 8339, 0, 917624 };       // U+0078: LATIN SMALL LETTER X
            LatinMapping[89] = new int[] { 119858, 119910, 119962, 120274, 120326, 120378, 120430, 120014, 120066, 120118, 120222, 120170, 120482, 65369, 9448, 0, 0, 0, 9396, 0, 696, 0, 0, 917625 };      // U+0079: LATIN SMALL LETTER Y
            LatinMapping[90] = new int[] { 119859, 119911, 119963, 120275, 120327, 120379, 120431, 120015, 120067, 120119, 120223, 120171, 120483, 65370, 9449, 0, 0, 0, 9397, 0, 7611, 0, 0, 917626 };     // U+007A: LATIN SMALL LETTER Z
            LatinMapping[91] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65371, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917627 };       // U+007B: LEFT CURLY BRACKET
            LatinMapping[92] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65372, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917628 };       // U+007C: VERTICAL LINE
            LatinMapping[93] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65373, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917629 };       // U+007D: RIGHT CURLY BRACKET
            LatinMapping[94] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65374, 0, 0, 0, 0, 0, 0, 0, 0, 0, 917630 };       // U+007E: TILDE
        }

        private int[][] GreekMapping;

        private void InitGreekMapping()
        {
            GreekMapping = new int[GreekChars][];
            GreekMapping[0] = new int[] { 120488, 120546, 120604, 0, 120662, 0, 120720 };       // U+0391, GREEK CAPITAL LETTER ALPHA
            GreekMapping[1] = new int[] { 120489, 120547, 120605, 0, 120663, 0, 120721 };       // U+0392, GREEK CAPITAL LETTER BETA
            GreekMapping[2] = new int[] { 120490, 120548, 120606, 0, 120664, 0, 120722 };       // U+0393, GREEK CAPITAL LETTER GAMMA
            GreekMapping[3] = new int[] { 120491, 120549, 120607, 0, 120665, 0, 120723 };       // U+0394, GREEK CAPITAL LETTER DELTA
            GreekMapping[4] = new int[] { 120492, 120550, 120608, 0, 120666, 0, 120724 };       // U+0395, GREEK CAPITAL LETTER EPSILON
            GreekMapping[5] = new int[] { 120493, 120551, 120609, 0, 120667, 0, 120725 };       // U+0396, GREEK CAPITAL LETTER ZETA
            GreekMapping[6] = new int[] { 120494, 120552, 120610, 0, 120668, 0, 120726 };       // U+0397, GREEK CAPITAL LETTER ETA
            GreekMapping[7] = new int[] { 120495, 120553, 120611, 0, 120669, 0, 120727 };       // U+0398, GREEK CAPITAL LETTER THETA
            GreekMapping[8] = new int[] { 120496, 120554, 120612, 0, 120670, 0, 120728 };       // U+0399, GREEK CAPITAL LETTER IOTA
            GreekMapping[9] = new int[] { 120497, 120555, 120613, 0, 120671, 0, 120729 };       // U+039A, GREEK CAPITAL LETTER KAPPA
            GreekMapping[10] = new int[] { 120498, 120556, 120614, 0, 120672, 0, 120730 };      // U+039B, GREEK CAPITAL LETTER LAMDA
            GreekMapping[11] = new int[] { 120499, 120557, 120615, 0, 120673, 0, 120731 };      // U+039C, GREEK CAPITAL LETTER MU
            GreekMapping[12] = new int[] { 120500, 120558, 120616, 0, 120674, 0, 120732 };      // U+039D, GREEK CAPITAL LETTER NU
            GreekMapping[13] = new int[] { 120501, 120559, 120617, 0, 120675, 0, 120733 };      // U+039E, GREEK CAPITAL LETTER XI
            GreekMapping[14] = new int[] { 120502, 120560, 120618, 0, 120676, 0, 120734 };      // U+039F, GREEK CAPITAL LETTER OMICRON
            GreekMapping[15] = new int[] { 120503, 120561, 120619, 0, 120677, 0, 120735 };      // U+03A0, GREEK CAPITAL LETTER PI
            GreekMapping[16] = new int[] { 120504, 120562, 120620, 0, 120678, 0, 120736 };      // U+03A1, GREEK CAPITAL LETTER RHO
            GreekMapping[17] = new int[] { 120505, 120563, 120621, 0, 120679, 0, 120737 };      // U+03F4, GREEK CAPITAL THETA SYMBOL
            GreekMapping[18] = new int[] { 120506, 120564, 120622, 0, 120680, 0, 120738 };      // U+03A3, GREEK CAPITAL LETTER SIGMA
            GreekMapping[19] = new int[] { 120507, 120565, 120623, 0, 120681, 0, 120739 };      // U+03A4, GREEK CAPITAL LETTER TAU
            GreekMapping[20] = new int[] { 120508, 120566, 120624, 0, 120682, 0, 120740 };      // U+03A5, GREEK CAPITAL LETTER UPSILON
            GreekMapping[21] = new int[] { 120509, 120567, 120625, 0, 120683, 0, 120741 };      // U+03A6, GREEK CAPITAL LETTER PHI
            GreekMapping[22] = new int[] { 120510, 120568, 120626, 0, 120684, 0, 120742 };      // U+03A7, GREEK CAPITAL LETTER CHI
            GreekMapping[23] = new int[] { 120511, 120569, 120627, 0, 120685, 0, 120743 };      // U+03A8, GREEK CAPITAL LETTER PSI
            GreekMapping[24] = new int[] { 120512, 120570, 120628, 0, 120686, 0, 120744 };      // U+03A9, GREEK CAPITAL LETTER OMEGA
            GreekMapping[25] = new int[] { 120513, 120571, 120629, 0, 120687, 0, 120745 };      // U+2207, NABLA
            GreekMapping[26] = new int[] { 120514, 120572, 120630, 0, 120688, 0, 120746 };      // U+03B1, GREEK SMALL LETTER ALPHA
            GreekMapping[27] = new int[] { 120515, 120573, 120631, 0, 120689, 0, 120747 };      // U+03B2, GREEK SMALL LETTER BETA
            GreekMapping[28] = new int[] { 120516, 120574, 120632, 0, 120690, 0, 120748 };      // U+03B3, GREEK SMALL LETTER GAMMA
            GreekMapping[29] = new int[] { 120517, 120575, 120633, 0, 120691, 0, 120749 };      // U+03B4, GREEK SMALL LETTER DELTA
            GreekMapping[30] = new int[] { 120518, 120576, 120634, 0, 120692, 0, 120750 };      // U+03B5, GREEK SMALL LETTER EPSILON
            GreekMapping[31] = new int[] { 120519, 120577, 120635, 0, 120693, 0, 120751 };      // U+03B6, GREEK SMALL LETTER ZETA
            GreekMapping[32] = new int[] { 120520, 120578, 120636, 0, 120694, 0, 120752 };      // U+03B7, GREEK SMALL LETTER ETA
            GreekMapping[33] = new int[] { 120521, 120579, 120637, 0, 120695, 0, 120753 };      // U+03B8, GREEK SMALL LETTER THETA
            GreekMapping[34] = new int[] { 120522, 120580, 120638, 0, 120696, 0, 120754 };      // U+03B9, GREEK SMALL LETTER IOTA
            GreekMapping[35] = new int[] { 120523, 120581, 120639, 0, 120697, 0, 120755 };      // U+03BA, GREEK SMALL LETTER KAPPA
            GreekMapping[36] = new int[] { 120524, 120582, 120640, 0, 120698, 0, 120756 };      // U+03BB, GREEK SMALL LETTER LAMDA
            GreekMapping[37] = new int[] { 120525, 120583, 120641, 0, 120699, 0, 120757 };      // U+03BC, GREEK SMALL LETTER MU
            GreekMapping[38] = new int[] { 120526, 120584, 120642, 0, 120700, 0, 120758 };      // U+03BD, GREEK SMALL LETTER NU
            GreekMapping[39] = new int[] { 120527, 120585, 120643, 0, 120701, 0, 120759 };      // U+03BE, GREEK SMALL LETTER XI
            GreekMapping[40] = new int[] { 120528, 120586, 120644, 0, 120702, 0, 120760 };      // U+03BF, GREEK SMALL LETTER OMICRON
            GreekMapping[41] = new int[] { 120529, 120587, 120645, 0, 120703, 0, 120761 };      // U+03C0, GREEK SMALL LETTER PI
            GreekMapping[42] = new int[] { 120530, 120588, 120646, 0, 120704, 0, 120762 };      // U+03C1, GREEK SMALL LETTER RHO
            GreekMapping[43] = new int[] { 120531, 120589, 120647, 0, 120705, 0, 120763 };      // U+03C2, GREEK SMALL LETTER FINAL SIGMA
            GreekMapping[44] = new int[] { 120532, 120590, 120648, 0, 120706, 0, 120764 };      // U+03C3, GREEK SMALL LETTER SIGMA
            GreekMapping[45] = new int[] { 120533, 120591, 120649, 0, 120707, 0, 120765 };      // U+03C4, GREEK SMALL LETTER TAU
            GreekMapping[46] = new int[] { 120534, 120592, 120650, 0, 120708, 0, 120766 };      // U+03C5, GREEK SMALL LETTER UPSILON
            GreekMapping[47] = new int[] { 120535, 120593, 120651, 0, 120709, 0, 120767 };      // U+03C6, GREEK SMALL LETTER PHI
            GreekMapping[48] = new int[] { 120536, 120594, 120652, 0, 120710, 0, 120768 };      // U+03C7, GREEK SMALL LETTER CHI
            GreekMapping[49] = new int[] { 120537, 120595, 120653, 0, 120711, 0, 120769 };      // U+03C8, GREEK SMALL LETTER PSI
            GreekMapping[50] = new int[] { 120538, 120596, 120654, 0, 120712, 0, 120770 };      // U+03C9, GREEK SMALL LETTER OMEGA
            GreekMapping[51] = new int[] { 120539, 120597, 120655, 0, 120713, 0, 120771 };      // U+2202, PARTIAL DIFFERENTIAL
            GreekMapping[52] = new int[] { 120540, 120598, 120656, 0, 120714, 0, 120772 };      // U+03F5, GREEK LUNATE EPSILON SYMBOL
            GreekMapping[53] = new int[] { 120541, 120599, 120657, 0, 120715, 0, 120773 };      // U+03D1, GREEK THETA SYMBOL
            GreekMapping[54] = new int[] { 120542, 120600, 120658, 0, 120716, 0, 120774 };      // U+03F0, GREEK KAPPA SYMBOL
            GreekMapping[55] = new int[] { 120543, 120601, 120659, 0, 120717, 0, 120775 };      // U+03D5, GREEK PHI SYMBOL
            GreekMapping[56] = new int[] { 120544, 120602, 120660, 0, 120718, 0, 120776 };      // U+03F1, GREEK RHO SYMBOL
            GreekMapping[57] = new int[] { 120545, 120603, 120661, 0, 120719, 0, 120777 };      // U+03D6, GREEK PI SYMBOL
        }

        private ushort[] GreekCharacters;

        private void InitGreekCharacters()
        {
            GreekCharacters = new ushort[GreekChars];
            GreekCharacters[0] = 0x0391;        // GREEK CAPITAL LETTER ALPHA
            GreekCharacters[1] = 0x0392;        // GREEK CAPITAL LETTER BETA
            GreekCharacters[2] = 0x0393;        // GREEK CAPITAL LETTER GAMMA
            GreekCharacters[3] = 0x0394;        // GREEK CAPITAL LETTER DELTA
            GreekCharacters[4] = 0x0395;        // GREEK CAPITAL LETTER EPSILON
            GreekCharacters[5] = 0x0396;        // GREEK CAPITAL LETTER ZETA
            GreekCharacters[6] = 0x0397;        // GREEK CAPITAL LETTER ETA
            GreekCharacters[7] = 0x0398;        // GREEK CAPITAL LETTER THETA
            GreekCharacters[8] = 0x0399;        // GREEK CAPITAL LETTER IOTA
            GreekCharacters[9] = 0x039A;        // GREEK CAPITAL LETTER KAPPA
            GreekCharacters[10] = 0x039B;       // GREEK CAPITAL LETTER LAMDA
            GreekCharacters[11] = 0x039C;       // GREEK CAPITAL LETTER MU
            GreekCharacters[12] = 0x039D;       // GREEK CAPITAL LETTER NU
            GreekCharacters[13] = 0x039E;       // GREEK CAPITAL LETTER XI
            GreekCharacters[14] = 0x039F;       // GREEK CAPITAL LETTER OMICRON
            GreekCharacters[15] = 0x03A0;       // GREEK CAPITAL LETTER PI
            GreekCharacters[16] = 0x03A1;       // GREEK CAPITAL LETTER RHO
            GreekCharacters[17] = 0x03F4;       // GREEK CAPITAL THETA SYMBOL
            GreekCharacters[18] = 0x03A3;       // GREEK CAPITAL LETTER SIGMA
            GreekCharacters[19] = 0x03A4;       // GREEK CAPITAL LETTER TAU
            GreekCharacters[20] = 0x03A5;       // GREEK CAPITAL LETTER UPSILON
            GreekCharacters[21] = 0x03A6;       // GREEK CAPITAL LETTER PHI
            GreekCharacters[22] = 0x03A7;       // GREEK CAPITAL LETTER CHI
            GreekCharacters[23] = 0x03A8;       // GREEK CAPITAL LETTER PSI
            GreekCharacters[24] = 0x03A9;       // GREEK CAPITAL LETTER OMEGA
            GreekCharacters[25] = 0x2207;       // NABLA
            GreekCharacters[26] = 0x03B1;       // GREEK SMALL LETTER ALPHA
            GreekCharacters[27] = 0x03B2;       // GREEK SMALL LETTER BETA
            GreekCharacters[28] = 0x03B3;       // GREEK SMALL LETTER GAMMA
            GreekCharacters[29] = 0x03B4;       // GREEK SMALL LETTER DELTA
            GreekCharacters[30] = 0x03B5;       // GREEK SMALL LETTER EPSILON
            GreekCharacters[31] = 0x03B6;       // GREEK SMALL LETTER ZETA
            GreekCharacters[32] = 0x03B7;       // GREEK SMALL LETTER ETA
            GreekCharacters[33] = 0x03B8;       // GREEK SMALL LETTER THETA
            GreekCharacters[34] = 0x03B9;       // GREEK SMALL LETTER IOTA
            GreekCharacters[35] = 0x03BA;       // GREEK SMALL LETTER KAPPA
            GreekCharacters[36] = 0x03BB;       // GREEK SMALL LETTER LAMDA
            GreekCharacters[37] = 0x03BC;       // GREEK SMALL LETTER MU
            GreekCharacters[38] = 0x03BD;       // GREEK SMALL LETTER NU
            GreekCharacters[39] = 0x03BE;       // GREEK SMALL LETTER XI
            GreekCharacters[40] = 0x03BF;       // GREEK SMALL LETTER OMICRON
            GreekCharacters[41] = 0x03C0;       // GREEK SMALL LETTER PI
            GreekCharacters[42] = 0x03C1;       // GREEK SMALL LETTER RHO
            GreekCharacters[43] = 0x03C2;       // GREEK SMALL LETTER FINAL SIGMA
            GreekCharacters[44] = 0x03C3;       // GREEK SMALL LETTER SIGMA
            GreekCharacters[45] = 0x03C4;       // GREEK SMALL LETTER TAU
            GreekCharacters[46] = 0x03C5;       // GREEK SMALL LETTER UPSILON
            GreekCharacters[47] = 0x03C6;       // GREEK SMALL LETTER PHI
            GreekCharacters[48] = 0x03C7;       // GREEK SMALL LETTER CHI
            GreekCharacters[49] = 0x03C8;       // GREEK SMALL LETTER PSI
            GreekCharacters[50] = 0x03C9;       // GREEK SMALL LETTER OMEGA
            GreekCharacters[51] = 0x2202;       // PARTIAL DIFFERENTIAL
            GreekCharacters[52] = 0x03F5;       // GREEK LUNATE EPSILON SYMBOL
            GreekCharacters[53] = 0x03D1;       // GREEK THETA SYMBOL
            GreekCharacters[54] = 0x03F0;       // GREEK KAPPA SYMBOL
            GreekCharacters[55] = 0x03D5;       // GREEK PHI SYMBOL
            GreekCharacters[56] = 0x03F1;       // GREEK RHO SYMBOL
            GreekCharacters[57] = 0x03D6;       // GREEK PI SYMBOL
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// Initializes a new instance of the <see cref="UnicodeStyler"/> class.
        /// </summary>
        public UnicodeStyler()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
            InitGreekCharacters();
            InitGreekMapping();
            InitLatinMapping();
            InitStyles();
        }

        /// <summary>
        /// Style the string.
        /// </summary>
        /// <param name="str">The string to style.</param>
        /// <param name="style">The style you want.</param>
        /// <returns>The styled string.</returns>
        public string StyleConvert(string str, UnicodeStyles style = UnicodeStyles.Regular)
        {
            string input = str;

            int length = input.Length;

            if (length > 0)
            {
                input = ToRegular(input);

                int index = (int)style;

                string output = index == -1 ? input : ToStyled(input, index);
                return output;
            }

            return input;
        }

        /// <summary>
        /// Style string.
        /// </summary>
        /// <param name="str">The string to style.</param>
        /// <param name="style">The style you want.</param>
        /// <returns>The styled string.</returns>
        public string StyleConvert(string str, string style = "")
        {
            string input = str;

            int length = input.Length;

            if (length > 0)
            {
                input = ToRegular(input);

                int index = -1;

                for (int i = 0; i < TotalStyles; i++)
                {
                    if (Styles[i] == style)
                    {
                        index = i;
                        break;
                    }
                }

                string output = index == -1 ? input : ToStyled(input, index);
                return output;
            }

            return input;
        }

        private string ToRegular(string input)
        {
            string output = "";
            int hi = 0;

            for (int i = 0; i < input.Length; i++)
            {
                int cp = input[i];

                if ((cp >= HighSurrogateFirst) && (cp <= HighSurrogateLast))
                {
                    if (hi != 0)
                    {
                        output += ReplacementCharacter;
                    }
                    hi = cp;
                    cp = 0;
                }
                else if ((cp >= LowSurrogateFirst) && (cp <= LowSurrogateLast))
                {
                    if (hi == 0)
                    {
                        output += ReplacementCharacter;
                    }
                    else
                    {
                        cp = FromSurrogates(hi, cp);
                        hi = 0;
                    }
                }
                else
                {
                    if (hi != 0)
                    {
                        output += ReplacementCharacter;
                        hi = 0;
                    }
                }

                if (cp >= FirstTarget)
                {
                    int row = 0;
                    bool isLatin = true;

                    if ((cp >= MathLatinFirst) && (cp <= MathLatinLast))
                    {
                        row = (cp - MathLatinFirst) % MathLatinRange;

                        if (row < 26)
                        {
                            row += LatinCapitalOffset;
                        }
                        else
                        {
                            row -= 26;
                            row += LatinSmallOffset;
                        }
                    }
                    else if ((cp >= MathGreekFirst) && (cp <= MathGreekLast))
                    {
                        row = (cp - MathGreekFirst) % MathGreekRange;
                        cp = GreekCharacters[row];
                        isLatin = false;
                    }
                    else if ((cp >= MathDigitsFirst) && (cp <= MathDigitsLast))
                    {
                        row = LatinDigitsOffset + ((cp - MathDigitsFirst) % MathDigitsRange);
                    }
                    else if (cp == 120484)  // U+1D6A4 MATHEMATICAL ITALIC SMALL DOTLESS I
                    {
                        cp = 305;
                        isLatin = false;
                    }
                    else if (cp == 120485)  // U+1D6A5 MATHEMATICAL ITALIC SMALL DOTLESS J
                    {
                        cp = 567;
                        isLatin = false;
                    }
                    else if (cp == 120778)  // U+1D7CA MATHEMATICAL BOLD CAPITAL DIGAMMA
                    {
                        cp = 988;
                        isLatin = false;
                    }
                    else if (cp == 120779)  // U+1D7CB MATHEMATICAL BOLD SMALL DIGAMMA
                    {
                        cp = 989;
                        isLatin = false;
                    }

                    if (isLatin)
                    {
                        for (int j = row; j < BasicLatinChars; j++)
                        {
                            for (int k = 0; k < TotalStyles; k++)
                            {
                                if (LatinMapping[j][k] == cp)
                                {
                                    cp = BasicLatinFirst + j;
                                    j = BasicLatinChars;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (cp != 0)
                {
                    if (cp > CharactersPerPlane)
                    {
                        int[] a = ToSurrogates(cp);
                        output += (char)a[0];
                        output += (char)a[1];
                    }
                    else
                    {
                        output += (char)cp;
                    }
                }
            }

            return output;
        }

        private string ToStyled(string input, int style)
        {
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                int cp = input[i];
                int result = 0;

                if ((cp >= BasicLatinFirst) && (cp <= BasicLatinLast))
                {
                    int offset = cp - BasicLatinFirst;
                    result = LatinMapping[offset][style];
                }
                else if (style < GreekStyles)
                {
                    bool greek = false;

                    if (cp is >= 0x0391 and <= 0x03C9)
                    {
                        greek = true;
                    }
                    else if (cp is >= 0x03D1 and <= 0x03D6)
                    {
                        greek = true;
                    }
                    else if (cp is >= 0x03F0 and <= 0x03F5)
                    {
                        greek = true;
                    }
                    else if (cp is >= 0x2202 and <= 0x2207)
                    {
                        greek = true;
                    }

                    int offset = 0;

                    if (greek == true)
                    {
                        greek = false;

                        for (int j = 0; j < MathGreekRange; j++)
                        {
                            if (GreekCharacters[j] == cp)
                            {
                                greek = true;
                                offset = j;
                                break;
                            }
                        }
                    }

                    if (greek == true)
                    {
                        result = GreekMapping[offset][style];
                    }

                    // Special Cases
                    if ((cp == 988) && (style == 0))    // U+1D7CA MATHEMATICAL BOLD CAPITAL DIGAMMA
                    {
                        result = 120778;
                    }

                    if ((cp == 989) && (style == 0))    // U+1D7CB MATHEMATICAL BOLD SMALL DIGAMMA
                    {
                        result = 120779;
                    }

                    if ((cp == 305) && (style == 1))    // U+1D6A4 MATHEMATICAL ITALIC SMALL DOTLESS I
                    {
                        result = 120484;
                    }

                    if ((cp == 567) && (style == 1))    // U+1D6A5 MATHEMATICAL ITALIC SMALL DOTLESS J
                    {
                        result = 120485;
                    }
                }

                if (result == 0)
                {
                    output += (char)cp;
                }
                else
                {
                    if (result > CharactersPerPlane)
                    {
                        int[] a = ToSurrogates(result);
                        output += (char)a[0];
                        output += (char)a[1];
                    }
                    else
                    {
                        output += (char)result;
                    }
                }
            }

            return output;
        }

        private int[] ToSurrogates(int cp)
        {
            int hi = cp;
            int lo = 0;

            if (cp > CharactersPerPlane)
            {
                cp -= CharactersPerPlane;
                hi = HighSurrogateFirst | ((cp >>> HalfShift) & HalfMask);
                lo = LowSurrogateFirst | (cp & HalfMask);
            }

            return new int[] { hi, lo };
        }

        private int FromSurrogates(int hi, int lo)
        {
            int cp = ReplacementCharacter;
            if ((hi >= HighSurrogateFirst) && (hi <= HighSurrogateLast) && (lo >= LowSurrogateFirst) && (lo <= LowSurrogateLast))
            {
                cp = ((hi - HighSurrogateFirst) << HalfShift) + (lo - LowSurrogateFirst) + HalfBase;
            }

            return cp;
        }

        /// <summary>
        /// Add Unicode Lines.
        /// </summary>
        /// <param name="str">The string you want to add lines.</param>
        /// <param name="lines">Lines you want to add.</param>
        /// <returns>The lines-added string.</returns>
        public static string AddLine(string str, params UnicodeLines[] lines)
        {
            string input = RemoveLine(str, lines);

            if (lines != null)
            {
                string output = string.Empty;
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    output += c;
                    for (int j = 0; j < lines.Length; j++)
                    {
                        UnicodeLines line = lines[j];
                        output += (char)line;
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
        /// <returns>The lines-removeed string.</returns>
        public static string RemoveLine(string str)
        {
            string input = str;
            string output = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                bool isline = false;
                char word = input[i];
                for (int j = 0; j < UnicodeLines.Length; j++)
                {
                    char line = UnicodeLines[j];
                    if (line == word)
                    {
                        isline = true;
                        break;
                    }
                }
                if (!isline) { output += word; }
            }

            return output;
        }

        /// <summary>
        /// Remove Unicode Lines.
        /// </summary>
        /// <param name="str">The string you want to remove lines.</param>
        /// <param name="lines">Lines you want to remove.</param>
        /// <returns>The lines-removeed string.</returns>
        public static string RemoveLine(string str, params UnicodeLines[] lines)
        {
            string input = str;
            string output = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                bool isline = false;
                char word = input[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    UnicodeLines line = lines[j];
                    if ((char)line == word)
                    {
                        isline = true;
                        break;
                    }
                }
                if (!isline) { output += word; }
            }

            return output;
        }

        /// <summary>
        /// Dispose the styler.
        /// </summary>
        /// <param name="disposing">Is disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                Styles = null;
                LatinMapping = null;
                GreekMapping = null;
                GreekCharacters = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose the styler.
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
