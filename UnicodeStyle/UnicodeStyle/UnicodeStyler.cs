// Source : <http://www.babelstone.co.uk/Unicode/text.js>
// Title : text.js
// Author : Andrew West
// Date Created : 2014-07-05
// Latest Version : 2021-12-05 (update for Unicode 14.0f; added superscript Latin capital letters)

// Creative Commons License:
// CC BY-SA 3.0f <http://creativecommons.org/licenses/by-sa/3.0f/>
// text.js by Andrew West is licensed under a Creative Commons Attribution-ShareAlike 3.0f Unported License

using System;
using UnicodeStyle.Models;

namespace UnicodeStyle
{
    /// <summary>
    /// The tools to style Unicode strings.
    /// </summary>
    public sealed class UnicodeStyler : IDisposable
    {
        // UTF-8 Constants
        private const ushort ReplacementCharacter = 0xFFFD; // U+FFFD REPLACEMENT CHARACTER
        private const int CharactersPerPlane = 0x10000;
        private const ushort HighSurrogateFirst = 0xD800;   // U+D800
        private const ushort HighSurrogateLast = 0xDBFF;    // U+DBFF
        private const ushort LowSurrogateFirst = 0xDC00;    // U+DC00
        private const ushort LowSurrogateLast = 0xDFFF;     // U+DFFF
        private const ushort HalfShift = 10;
        private const int HalfBase = 0x10000;               // 0x10000;
        private const ushort HalfMask = 0x03FF;

        // Other constants
        private const ushort BasicLatinChars = 95;
        private const ushort BasicLatinFirst = 0x0020;      // U+0020
        private const ushort BasicLatinLast = 0x007E;       // U+007E
        private const ushort MathLatinRange = 52;
        private const int MathLatinFirst = 0x1D400;         // U+1D400
        private const int MathLatinLast = 0x1D6A3;          // U+1D6A3
        private const ushort MathGreekRange = 58;
        private const int MathGreekFirst = 0x1D6A8;         // U+1D6A8
        private const int MathGreekLast = 0x1D7C9;          // U+1D7C9
        private const ushort MathDigitsRange = 10;
        private const int MathDigitsFirst = 0x1D7CE;        // U+1D7CE
        private const int MathDigitsLast = 0x1D7FF;         // U+1D7FF
        private const ushort LatinDigitsOffset = 16;
        private const ushort LatinCapitalOffset = 33;
        private const ushort LatinSmallOffset = 65;
        private const ushort GreekChars = 58;
        private const ushort FirstTarget = 0x00B2;          // U+00B2
        private const ushort TotalStyles = 24;
        private const ushort GreekStyles = 7;               // Actually only 5, but we include S/S and S/S Italic in the table for ease of mapping

        private int[][] LatinMapping = new int[BasicLatinChars][]
        {
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0020 },            // U+0020: SPACE
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF01, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0021 },            // U+0021: EXCLAMATION MARK
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0022 },            // U+0022: QUOTATION MARK
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF03, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0023 },            // U+0023: NUMBER SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF04, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0024 },            // U+0024: DOLLAR SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF05, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0025 },            // U+0025: PERCENT SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF06, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0026 },            // U+0026: AMPERSAND
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF07, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0027 },            // U+0027: APOSTROPHE
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF08, 0, 0, 0, 0, 0, 0, 0x207D, 0x208D, 0, 0xE0028 },  // U+0028: LEFT PARENTHESIS
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF09, 0, 0, 0, 0, 0, 0, 0x207E, 0x208E, 0, 0xE0029 },  // U+0029: RIGHT PARENTHESIS
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0A, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE002A },            // U+002A: ASTERISK
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0B, 0, 0, 0, 0, 0, 0, 0x207A, 0x208A, 0, 0xE002B },  // U+002B: PLUS SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0C, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE002C },            // U+002C: COMMA
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0D, 0, 0, 0, 0, 0, 0, 0x207B, 0x208B, 0, 0xE002D },  // U+002D: HYPHEN-MINUS
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE002E },            // U+002E: FULL STOP
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF0F, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE002F },            // U+002F: SOLIDUS
            new int[] { 0x1D7CE, 0, 0, 0x1D7E2, 0x1D7EC, 0, 0, 0, 0, 0, 0, 0x1D7D8, 0x1D7F6, 0xFF10, 0x24EA, 0x24FF, 0, 0, 0, 0, 0x2070, 0x2080, 0, 0xE0030 },      // U+0030: DIGIT ZERO
            new int[] { 0x1D7CF, 0, 0, 0x1D7E3, 0x1D7ED, 0, 0, 0, 0, 0, 0, 0x1D7D9, 0x1D7F7, 0xFF11, 0x2460, 0x2776, 0, 0, 0x2474, 0, 0xB9, 0x2081, 0, 0xE0031 },   // U+0031: DIGIT ONE
            new int[] { 0x1D7D0, 0, 0, 0x1D7E4, 0x1D7EE, 0, 0, 0, 0, 0, 0, 0x1D7DA, 0x1D7F8, 0xFF12, 0x2461, 0x2777, 0, 0, 0x2475, 0, 0xB2, 0x2082, 0, 0xE0032 },   // U+0032: DIGIT TWO
            new int[] { 0x1D7D1, 0, 0, 0x1D7E5, 0x1D7EF, 0, 0, 0, 0, 0, 0, 0x1D7DB, 0x1D7F9, 0xFF13, 0x2462, 0x2778, 0, 0, 0x2476, 0, 0xB3, 0x2083, 0, 0xE0033 },   // U+0033: DIGIT THREE
            new int[] { 0x1D7D2, 0, 0, 0x1D7E6, 0x1D7F0, 0, 0, 0, 0, 0, 0, 0x1D7DC, 0x1D7FA, 0xFF14, 0x2463, 0x2779, 0, 0, 0x2477, 0, 0x2074, 0x2084, 0, 0xE0034 }, // U+0034: DIGIT FOUR
            new int[] { 0x1D7D3, 0, 0, 0x1D7E7, 0x1D7F1, 0, 0, 0, 0, 0, 0, 0x1D7DD, 0x1D7FB, 0xFF15, 0x2464, 0x277A, 0, 0, 0x2478, 0, 0x2075, 0x2085, 0, 0xE0035 }, // U+0035: DIGIT FIVE
            new int[] { 0x1D7D4, 0, 0, 0x1D7E8, 0x1D7F2, 0, 0, 0, 0, 0, 0, 0x1D7DE, 0x1D7FC, 0xFF16, 0x2465, 0x277B, 0, 0, 0x2479, 0, 0x2076, 0x2086, 0, 0xE0036 }, // U+0036: DIGIT SIX
            new int[] { 0x1D7D5, 0, 0, 0x1D7E9, 0x1D7F3, 0, 0, 0, 0, 0, 0, 0x1D7DF, 0x1D7FD, 0xFF17, 0x2466, 0x277C, 0, 0, 0x247A, 0, 0x2077, 0x2087, 0, 0xE0037 }, // U+0037: DIGIT SEVEN
            new int[] { 0x1D7D6, 0, 0, 0x1D7EA, 0x1D7F4, 0, 0, 0, 0, 0, 0, 0x1D7E0, 0x1D7FE, 0xFF18, 0x2467, 0x277D, 0, 0, 0x247B, 0, 0x2078, 0x2088, 0, 0xE0038 }, // U+0038: DIGIT EIGHT
            new int[] { 0x1D7D7, 0, 0, 0x1D7EB, 0x1D7F5, 0, 0, 0, 0, 0, 0, 0x1D7E1, 0x1D7FF, 0xFF19, 0x2468, 0x277E, 0, 0, 0x247C, 0, 0x2079, 0x2089, 0, 0xE0039 }, // U+0039: DIGIT NINE
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1A, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE003A },            // U+003A: COLON
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1B, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE003B },            // U+003B: SEMICOLON
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1C, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE003C },            // U+003C: LESS-THAN SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1D, 0, 0, 0, 0, 0, 0, 0x207C, 0x208C, 0, 0xE003D },  // U+003D: EQUALS SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE003E },            // U+003E: GREATER-THAN SIGN
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF1F, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE003F },            // U+003F: QUESTION MARK
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0040 },            // U+0040: COMMERCIAL AT
            new int[] { 0x1D400, 0x1D434, 0x1D468, 0x1D5A0, 0x1D5D4, 0x1D608, 0x1D63C, 0x1D49C, 0x1D4D0, 0x1D504, 0x1D56C, 0x1D538, 0x1D670, 0xFF21, 0x24B6, 0x1F150, 0x1F130, 0x1F170, 0x1F110, 0x1D00, 0x1D2C, 0, 0x1F1E6, 0xE0041 }, // U+0041: LATIN CAPITAL LETTER A
            new int[] { 0x1D401, 0x1D435, 0x1D469, 0x1D5A1, 0x1D5D5, 0x1D609, 0x1D63D, 0x212C, 0x1D4D1, 0x1D505, 0x1D56D, 0x1D539, 0x1D671, 0xFF22, 0x24B7, 0x1F151, 0x1F131, 0x1F171, 0x1F111, 0x299, 0x1D2E, 0, 0x1F1E7, 0xE0042 },   // U+0042: LATIN CAPITAL LETTER B
            new int[] { 0x1D402, 0x1D436, 0x1D46A, 0x1D5A2, 0x1D5D6, 0x1D60A, 0x1D63E, 0x1D49E, 0x1D4D2, 0x212D, 0x1D56E, 0x2102, 0x1D672, 0xFF23, 0x24B8, 0x1F152, 0x1F132, 0x1F172, 0x1F112, 0x1D04, 0xA7F2, 0, 0x1F1E8, 0xE0043 },   // U+0043: LATIN CAPITAL LETTER C
            new int[] { 0x1D403, 0x1D437, 0x1D46B, 0x1D5A3, 0x1D5D7, 0x1D60B, 0x1D63F, 0x1D49F, 0x1D4D3, 0x1D507, 0x1D56F, 0x1D53B, 0x1D673, 0xFF24, 0x24B9, 0x1F153, 0x1F133, 0x1F173, 0x1F113, 0x1D05, 0x1D30, 0, 0x1F1E9, 0xE0044 }, // U+0044: LATIN CAPITAL LETTER D
            new int[] { 0x1D404, 0x1D438, 0x1D46C, 0x1D5A4, 0x1D5D8, 0x1D60C, 0x1D640, 0x2130, 0x1D4D4, 0x1D508, 0x1D570, 0x1D53C, 0x1D674, 0xFF25, 0x24BA, 0x1F154, 0x1F134, 0x1F174, 0x1F114, 0x1D07, 0x1D31, 0, 0x1F1EA, 0xE0045 },  // U+0045: LATIN CAPITAL LETTER E
            new int[] { 0x1D405, 0x1D439, 0x1D46D, 0x1D5A5, 0x1D5D9, 0x1D60D, 0x1D641, 0x2131, 0x1D4D5, 0x1D509, 0x1D571, 0x1D53D, 0x1D675, 0xFF26, 0x24BB, 0x1F155, 0x1F135, 0x1F175, 0x1F115, 0xA730, 0xA7F3, 0, 0x1F1EB, 0xE0046 },  // U+0046: LATIN CAPITAL LETTER F
            new int[] { 0x1D406, 0x1D43A, 0x1D46E, 0x1D5A6, 0x1D5DA, 0x1D60E, 0x1D642, 0x1D4A2, 0x1D4D6, 0x1D50A, 0x1D572, 0x1D53E, 0x1D676, 0xFF27, 0x24BC, 0x1F156, 0x1F136, 0x1F176, 0x1F116, 0x262, 0x1D33, 0, 0x1F1EC, 0xE0047 },  // U+0047: LATIN CAPITAL LETTER G
            new int[] { 0x1D407, 0x1D43B, 0x1D46F, 0x1D5A7, 0x1D5DB, 0x1D60F, 0x1D643, 0x210B, 0x1D4D7, 0x210C, 0x1D573, 0x210D, 0x1D677, 0xFF28, 0x24BD, 0x1F157, 0x1F137, 0x1F177, 0x1F117, 0x29C, 0x1D34, 0, 0x1F1ED, 0xE0048 },     // U+0048: LATIN CAPITAL LETTER H
            new int[] { 0x1D408, 0x1D43C, 0x1D470, 0x1D5A8, 0x1D5DC, 0x1D610, 0x1D644, 0x2110, 0x1D4D8, 0x2111, 0x1D574, 0x1D540, 0x1D678, 0xFF29, 0x24BE, 0x1F158, 0x1F138, 0x1F178, 0x1F118, 0x26A, 0x1D35, 0, 0x1F1EE, 0xE0049 },    // U+0049: LATIN CAPITAL LETTER I
            new int[] { 0x1D409, 0x1D43D, 0x1D471, 0x1D5A9, 0x1D5DD, 0x1D611, 0x1D645, 0x1D4A5, 0x1D4D9, 0x1D50D, 0x1D575, 0x1D541, 0x1D679, 0xFF2A, 0x24BF, 0x1F159, 0x1F139, 0x1F179, 0x1F119, 0x1D0A, 0x1D36, 0, 0x1F1EF, 0xE004A }, // U+004A: LATIN CAPITAL LETTER J
            new int[] { 0x1D40A, 0x1D43E, 0x1D472, 0x1D5AA, 0x1D5DE, 0x1D612, 0x1D646, 0x1D4A6, 0x1D4DA, 0x1D50E, 0x1D576, 0x1D542, 0x1D67A, 0xFF2B, 0x24C0, 0x1F15A, 0x1F13A, 0x1F17A, 0x1F11A, 0x1D0B, 0x1D37, 0, 0x1F1F0, 0xE004B }, // U+004B: LATIN CAPITAL LETTER K
            new int[] { 0x1D40B, 0x1D43F, 0x1D473, 0x1D5AB, 0x1D5DF, 0x1D613, 0x1D647, 0x2112, 0x1D4DB, 0x1D50F, 0x1D577, 0x1D543, 0x1D67B, 0xFF2C, 0x24C1, 0x1F15B, 0x1F13B, 0x1F17B, 0x1F11B, 0x29F, 0x1D38, 0, 0x1F1F1, 0xE004C },   // U+004C: LATIN CAPITAL LETTER L
            new int[] { 0x1D40C, 0x1D440, 0x1D474, 0x1D5AC, 0x1D5E0, 0x1D614, 0x1D648, 0x2133, 0x1D4DC, 0x1D510, 0x1D578, 0x1D544, 0x1D67C, 0xFF2D, 0x24C2, 0x1F15C, 0x1F13C, 0x1F17C, 0x1F11C, 0x1D0D, 0x1D39, 0, 0x1F1F2, 0xE004D },  // U+004D: LATIN CAPITAL LETTER M
            new int[] { 0x1D40D, 0x1D441, 0x1D475, 0x1D5AD, 0x1D5E1, 0x1D615, 0x1D649, 0x1D4A9, 0x1D4DD, 0x1D511, 0x1D579, 0x2115, 0x1D67D, 0xFF2E, 0x24C3, 0x1F15D, 0x1F13D, 0x1F17D, 0x1F11D, 0x274, 0x1D3A, 0, 0x1F1F3, 0xE004E },   // U+004E: LATIN CAPITAL LETTER N
            new int[] { 0x1D40E, 0x1D442, 0x1D476, 0x1D5AE, 0x1D5E2, 0x1D616, 0x1D64A, 0x1D4AA, 0x1D4DE, 0x1D512, 0x1D57A, 0x1D546, 0x1D67E, 0xFF2F, 0x24C4, 0x1F15E, 0x1F13E, 0x1F17E, 0x1F11E, 0x1D0F, 0x1D3C, 0, 0x1F1F4, 0xE004F }, // U+004F: LATIN CAPITAL LETTER O
            new int[] { 0x1D40F, 0x1D443, 0x1D477, 0x1D5AF, 0x1D5E3, 0x1D617, 0x1D64B, 0x1D4AB, 0x1D4DF, 0x1D513, 0x1D57B, 0x2119, 0x1D67F, 0xFF30, 0x24C5, 0x1F15F, 0x1F13F, 0x1F17F, 0x1F11F, 0x1D18, 0x1D3E, 0, 0x1F1F5, 0xE0050 },  // U+0050: LATIN CAPITAL LETTER P
            new int[] { 0x1D410, 0x1D444, 0x1D478, 0x1D5B0, 0x1D5E4, 0x1D618, 0x1D64C, 0x1D4AC, 0x1D4E0, 0x1D514, 0x1D57C, 0x211A, 0x1D680, 0xFF31, 0x24C6, 0x1F160, 0x1F140, 0x1F180, 0x1F120, 0xA7AF, 0xA7F4, 0, 0x1F1F6, 0xE0051 },  // U+0051: LATIN CAPITAL LETTER Q
            new int[] { 0x1D411, 0x1D445, 0x1D479, 0x1D5B1, 0x1D5E5, 0x1D619, 0x1D64D, 0x211B, 0x1D4E1, 0x211C, 0x1D57D, 0x211D, 0x1D681, 0xFF32, 0x24C7, 0x1F161, 0x1F141, 0x1F181, 0x1F121, 0x280, 0x1D3F, 0, 0x1F1F7, 0xE0052 },     // U+0052: LATIN CAPITAL LETTER R
            new int[] { 0x1D412, 0x1D446, 0x1D47A, 0x1D5B2, 0x1D5E6, 0x1D61A, 0x1D64E, 0x1D4AE, 0x1D4E2, 0x1D516, 0x1D57E, 0x1D54A, 0x1D682, 0xFF33, 0x24C8, 0x1F162, 0x1F142, 0x1F182, 0x1F122, 0xA731, 0, 0, 0x1F1F8, 0xE0053 },      // U+0053: LATIN CAPITAL LETTER S
            new int[] { 0x1D413, 0x1D447, 0x1D47B, 0x1D5B3, 0x1D5E7, 0x1D61B, 0x1D64F, 0x1D4AF, 0x1D4E3, 0x1D517, 0x1D57F, 0x1D54B, 0x1D683, 0xFF34, 0x24C9, 0x1F163, 0x1F143, 0x1F183, 0x1F123, 0x1D1B, 0x1D40, 0, 0x1F1F9, 0xE0054 }, // U+0054: LATIN CAPITAL LETTER T
            new int[] { 0x1D414, 0x1D448, 0x1D47C, 0x1D5B4, 0x1D5E8, 0x1D61C, 0x1D650, 0x1D4B0, 0x1D4E4, 0x1D518, 0x1D580, 0x1D54C, 0x1D684, 0xFF35, 0x24CA, 0x1F164, 0x1F144, 0x1F184, 0x1F124, 0x1D1C, 0x1D41, 0, 0x1F1FA, 0xE0055 }, // U+0055: LATIN CAPITAL LETTER U
            new int[] { 0x1D415, 0x1D449, 0x1D47D, 0x1D5B5, 0x1D5E9, 0x1D61D, 0x1D651, 0x1D4B1, 0x1D4E5, 0x1D519, 0x1D581, 0x1D54D, 0x1D685, 0xFF36, 0x24CB, 0x1F165, 0x1F145, 0x1F185, 0x1F125, 0x1D20, 0x2C7D, 0, 0x1F1FB, 0xE0056 }, // U+0056: LATIN CAPITAL LETTER V
            new int[] { 0x1D416, 0x1D44A, 0x1D47E, 0x1D5B6, 0x1D5EA, 0x1D61E, 0x1D652, 0x1D4B2, 0x1D4E6, 0x1D51A, 0x1D582, 0x1D54E, 0x1D686, 0xFF37, 0x24CC, 0x1F166, 0x1F146, 0x1F186, 0x1F126, 0x1D21, 0x1D42, 0, 0x1F1FC, 0xE0057 }, // U+0057: LATIN CAPITAL LETTER W
            new int[] { 0x1D417, 0x1D44B, 0x1D47F, 0x1D5B7, 0x1D5EB, 0x1D61F, 0x1D653, 0x1D4B3, 0x1D4E7, 0x1D51B, 0x1D583, 0x1D54F, 0x1D687, 0xFF38, 0x24CD, 0x1F167, 0x1F147, 0x1F187, 0x1F127, 0, 0, 0, 0x1F1FD, 0xE0058 },           // U+0058: LATIN CAPITAL LETTER X
            new int[] { 0x1D418, 0x1D44C, 0x1D480, 0x1D5B8, 0x1D5EC, 0x1D620, 0x1D654, 0x1D4B4, 0x1D4E8, 0x1D51C, 0x1D584, 0x1D550, 0x1D688, 0xFF39, 0x24CE, 0x1F168, 0x1F148, 0x1F188, 0x1F128, 0x28F, 0, 0, 0x1F1FE, 0xE0059 },       // U+0059: LATIN CAPITAL LETTER Y
            new int[] { 0x1D419, 0x1D44D, 0x1D481, 0x1D5B9, 0x1D5ED, 0x1D621, 0x1D655, 0x1D4B5, 0x1D4E9, 0x2128, 0x1D585, 0x2124, 0x1D689, 0xFF3A, 0x24CF, 0x1F169, 0x1F149, 0x1F189, 0x1F129, 0x1D22, 0, 0, 0x1F1FF, 0xE005A },        // U+005A: LATIN CAPITAL LETTER Z
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF3B, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE005B },            // U+005B: LEFT SQUARE BRACKET
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF3C, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE005C },            // U+005C: REVERSE SOLIDUS
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF3D, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE005D },            // U+005D: RIGHT SQUARE BRACKET
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF3E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE005E },            // U+005E: CIRCUMFLEX ACCENT
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF3F, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE005F },            // U+005F: LOW LINE
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE0060 },            // U+0060: GRAVE ACCENT
            new int[] { 0x1D41A, 0x1D44E, 0x1D482, 0x1D5BA, 0x1D5EE, 0x1D622, 0x1D656, 0x1D4B6, 0x1D4EA, 0x1D51E, 0x1D586, 0x1D552, 0x1D68A, 0xFF41, 0x24D0, 0, 0, 0, 0x249C, 0, 0x1D43, 0x2090, 0, 0xE0061 },  // U+0061: LATIN SMALL LETTER A
            new int[] { 0x1D41B, 0x1D44F, 0x1D483, 0x1D5BB, 0x1D5EF, 0x1D623, 0x1D657, 0x1D4B7, 0x1D4EB, 0x1D51F, 0x1D587, 0x1D553, 0x1D68B, 0xFF42, 0x24D1, 0, 0, 0, 0x249D, 0, 0x1D47, 0, 0, 0xE0062 },       // U+0062: LATIN SMALL LETTER B
            new int[] { 0x1D41C, 0x1D450, 0x1D484, 0x1D5BC, 0x1D5F0, 0x1D624, 0x1D658, 0x1D4B8, 0x1D4EC, 0x1D520, 0x1D588, 0x1D554, 0x1D68C, 0xFF43, 0x24D2, 0, 0, 0, 0x249E, 0, 0x1D9C, 0, 0, 0xE0063 },       // U+0063: LATIN SMALL LETTER C
            new int[] { 0x1D41D, 0x1D451, 0x1D485, 0x1D5BD, 0x1D5F1, 0x1D625, 0x1D659, 0x1D4B9, 0x1D4ED, 0x1D521, 0x1D589, 0x1D555, 0x1D68D, 0xFF44, 0x24D3, 0, 0, 0, 0x249F, 0, 0x1D48, 0, 0, 0xE0064 },       // U+0064: LATIN SMALL LETTER D
            new int[] { 0x1D41E, 0x1D452, 0x1D486, 0x1D5BE, 0x1D5F2, 0x1D626, 0x1D65A, 0x212F, 0x1D4EE, 0x1D522, 0x1D58A, 0x1D556, 0x1D68E, 0xFF45, 0x24D4, 0, 0, 0, 0x24A0, 0, 0x1D49, 0x2091, 0, 0xE0065 },   // U+0065: LATIN SMALL LETTER E
            new int[] { 0x1D41F, 0x1D453, 0x1D487, 0x1D5BF, 0x1D5F3, 0x1D627, 0x1D65B, 0x1D4BB, 0x1D4EF, 0x1D523, 0x1D58B, 0x1D557, 0x1D68F, 0xFF46, 0x24D5, 0, 0, 0, 0x24A1, 0, 0x1DA0, 0, 0, 0xE0066 },       // U+0066: LATIN SMALL LETTER F
            new int[] { 0x1D420, 0x1D454, 0x1D488, 0x1D5C0, 0x1D5F4, 0x1D628, 0x1D65C, 0x210A, 0x1D4F0, 0x1D524, 0x1D58C, 0x1D558, 0x1D690, 0xFF47, 0x24D6, 0, 0, 0, 0x24A2, 0, 0x1D4D, 0, 0, 0xE0067 },        // U+0067: LATIN SMALL LETTER G
            new int[] { 0x1D421, 0x210E, 0x1D489, 0x1D5C1, 0x1D5F5, 0x1D629, 0x1D65D, 0x1D4BD, 0x1D4F1, 0x1D525, 0x1D58D, 0x1D559, 0x1D691, 0xFF48, 0x24D7, 0, 0, 0, 0x24A3, 0, 0x2B0, 0x2095, 0, 0xE0068 },    // U+0068: LATIN SMALL LETTER H
            new int[] { 0x1D422, 0x1D456, 0x1D48A, 0x1D5C2, 0x1D5F6, 0x1D62A, 0x1D65E, 0x1D4BE, 0x1D4F2, 0x1D526, 0x1D58E, 0x1D55A, 0x1D692, 0xFF49, 0x24D8, 0, 0, 0, 0x24A4, 0, 0x2071, 0x1D62, 0, 0xE0069 },  // U+0069: LATIN SMALL LETTER I
            new int[] { 0x1D423, 0x1D457, 0x1D48B, 0x1D5C3, 0x1D5F7, 0x1D62B, 0x1D65F, 0x1D4BF, 0x1D4F3, 0x1D527, 0x1D58F, 0x1D55B, 0x1D693, 0xFF4A, 0x24D9, 0, 0, 0, 0x24A5, 0, 0x2B2, 0x2C7C, 0, 0xE006A },   // U+006A: LATIN SMALL LETTER J
            new int[] { 0x1D424, 0x1D458, 0x1D48C, 0x1D5C4, 0x1D5F8, 0x1D62C, 0x1D660, 0x1D4C0, 0x1D4F4, 0x1D528, 0x1D590, 0x1D55C, 0x1D694, 0xFF4B, 0x24DA, 0, 0, 0, 0x24A6, 0, 0x1D4F, 0x2096, 0, 0xE006B },  // U+006B: LATIN SMALL LETTER K
            new int[] { 0x1D425, 0x1D459, 0x1D48D, 0x1D5C5, 0x1D5F9, 0x1D62D, 0x1D661, 0x1D4C1, 0x1D4F5, 0x1D529, 0x1D591, 0x1D55D, 0x1D695, 0xFF4C, 0x24DB, 0, 0, 0, 0x24A7, 0, 0x2E1, 0x2097, 0, 0xE006C },   // U+006C: LATIN SMALL LETTER L
            new int[] { 0x1D426, 0x1D45A, 0x1D48E, 0x1D5C6, 0x1D5FA, 0x1D62E, 0x1D662, 0x1D4C2, 0x1D4F6, 0x1D52A, 0x1D592, 0x1D55E, 0x1D696, 0xFF4D, 0x24DC, 0, 0, 0, 0x24A8, 0, 0x1D50, 0x2098, 0, 0xE006D },  // U+006D: LATIN SMALL LETTER M
            new int[] { 0x1D427, 0x1D45B, 0x1D48F, 0x1D5C7, 0x1D5FB, 0x1D62F, 0x1D663, 0x1D4C3, 0x1D4F7, 0x1D52B, 0x1D593, 0x1D55F, 0x1D697, 0xFF4E, 0x24DD, 0, 0, 0, 0x24A9, 0, 0x207F, 0x2099, 0, 0xE006E },  // U+006E: LATIN SMALL LETTER N
            new int[] { 0x1D428, 0x1D45C, 0x1D490, 0x1D5C8, 0x1D5FC, 0x1D630, 0x1D664, 0x2134, 0x1D4F8, 0x1D52C, 0x1D594, 0x1D560, 0x1D698, 0xFF4F, 0x24DE, 0, 0, 0, 0x24AA, 0, 0x1D52, 0x2092, 0, 0xE006F },   // U+006F: LATIN SMALL LETTER O
            new int[] { 0x1D429, 0x1D45D, 0x1D491, 0x1D5C9, 0x1D5FD, 0x1D631, 0x1D665, 0x1D4C5, 0x1D4F9, 0x1D52D, 0x1D595, 0x1D561, 0x1D699, 0xFF50, 0x24DF, 0, 0, 0, 0x24AB, 0, 0x1D56, 0x209A, 0, 0xE0070 },  // U+0070: LATIN SMALL LETTER P
            new int[] { 0x1D42A, 0x1D45E, 0x1D492, 0x1D5CA, 0x1D5FE, 0x1D632, 0x1D666, 0x1D4C6, 0x1D4FA, 0x1D52E, 0x1D596, 0x1D562, 0x1D69A, 0xFF51, 0x24E0, 0, 0, 0, 0x24AC, 0, 0x107A5, 0, 0, 0xE0071 },      // U+0071: LATIN SMALL LETTER Q
            new int[] { 0x1D42B, 0x1D45F, 0x1D493, 0x1D5CB, 0x1D5FF, 0x1D633, 0x1D667, 0x1D4C7, 0x1D4FB, 0x1D52F, 0x1D597, 0x1D563, 0x1D69B, 0xFF52, 0x24E1, 0, 0, 0, 0x24AD, 0, 0x2B3, 0x1D63, 0, 0xE0072 },   // U+0072: LATIN SMALL LETTER R
            new int[] { 0x1D42C, 0x1D460, 0x1D494, 0x1D5CC, 0x1D600, 0x1D634, 0x1D668, 0x1D4C8, 0x1D4FC, 0x1D530, 0x1D598, 0x1D564, 0x1D69C, 0xFF53, 0x24E2, 0, 0, 0, 0x24AE, 0, 0x2E2, 0x209B, 0, 0xE0073 },   // U+0073: LATIN SMALL LETTER S
            new int[] { 0x1D42D, 0x1D461, 0x1D495, 0x1D5CD, 0x1D601, 0x1D635, 0x1D669, 0x1D4C9, 0x1D4FD, 0x1D531, 0x1D599, 0x1D565, 0x1D69D, 0xFF54, 0x24E3, 0, 0, 0, 0x24AF, 0, 0x1D57, 0x209C, 0, 0xE0074 },  // U+0074: LATIN SMALL LETTER T
            new int[] { 0x1D42E, 0x1D462, 0x1D496, 0x1D5CE, 0x1D602, 0x1D636, 0x1D66A, 0x1D4CA, 0x1D4FE, 0x1D532, 0x1D59A, 0x1D566, 0x1D69E, 0xFF55, 0x24E4, 0, 0, 0, 0x24B0, 0, 0x1D58, 0x1D64, 0, 0xE0075 },  // U+0075: LATIN SMALL LETTER U
            new int[] { 0x1D42F, 0x1D463, 0x1D497, 0x1D5CF, 0x1D603, 0x1D637, 0x1D66B, 0x1D4CB, 0x1D4FF, 0x1D533, 0x1D59B, 0x1D567, 0x1D69F, 0xFF56, 0x24E5, 0, 0, 0, 0x24B1, 0, 0x1D5B, 0x1D65, 0, 0xE0076 },  // U+0076: LATIN SMALL LETTER V
            new int[] { 0x1D430, 0x1D464, 0x1D498, 0x1D5D0, 0x1D604, 0x1D638, 0x1D66C, 0x1D4CC, 0x1D500, 0x1D534, 0x1D59C, 0x1D568, 0x1D6A0, 0xFF57, 0x24E6, 0, 0, 0, 0x24B2, 0, 0x2B7, 0, 0, 0xE0077 },        // U+0077: LATIN SMALL LETTER W
            new int[] { 0x1D431, 0x1D465, 0x1D499, 0x1D5D1, 0x1D605, 0x1D639, 0x1D66D, 0x1D4CD, 0x1D501, 0x1D535, 0x1D59D, 0x1D569, 0x1D6A1, 0xFF58, 0x24E7, 0, 0, 0, 0x24B3, 0, 0x2E3, 0x2093, 0, 0xE0078 },   // U+0078: LATIN SMALL LETTER X
            new int[] { 0x1D432, 0x1D466, 0x1D49A, 0x1D5D2, 0x1D606, 0x1D63A, 0x1D66E, 0x1D4CE, 0x1D502, 0x1D536, 0x1D59E, 0x1D56A, 0x1D6A2, 0xFF59, 0x24E8, 0, 0, 0, 0x24B4, 0, 0x2B8, 0, 0, 0xE0079 },        // U+0079: LATIN SMALL LETTER Y
            new int[] { 0x1D433, 0x1D467, 0x1D49B, 0x1D5D3, 0x1D607, 0x1D63B, 0x1D66F, 0x1D4CF, 0x1D503, 0x1D537, 0x1D59F, 0x1D56B, 0x1D6A3, 0xFF5A, 0x24E9, 0, 0, 0, 0x24B5, 0, 0x1DBB, 0, 0, 0xE007A },       // U+007A: LATIN SMALL LETTER Z
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF5B, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE007B },            // U+007B: LEFT CURLY BRACKET
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF5C, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE007C },            // U+007C: VERTICAL LINE
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF5D, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE007D },            // U+007D: RIGHT CURLY BRACKET
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF5E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xE007E }             // U+007E: TILDE
        };

        private int[][] GreekMapping = new int[GreekChars][]
        {
            new int[] { 0x1D6A8, 0x1D6E2, 0x1D71C, 0, 0x1D756, 0, 0x1D790 },    // U+0391, GREEK CAPITAL LETTER ALPHA
            new int[] { 0x1D6A9, 0x1D6E3, 0x1D71D, 0, 0x1D757, 0, 0x1D791 },    // U+0392, GREEK CAPITAL LETTER BETA
            new int[] { 0x1D6AA, 0x1D6E4, 0x1D71E, 0, 0x1D758, 0, 0x1D792 },    // U+0393, GREEK CAPITAL LETTER GAMMA
            new int[] { 0x1D6AB, 0x1D6E5, 0x1D71F, 0, 0x1D759, 0, 0x1D793 },    // U+0394, GREEK CAPITAL LETTER DELTA
            new int[] { 0x1D6AC, 0x1D6E6, 0x1D720, 0, 0x1D75A, 0, 0x1D794 },    // U+0395, GREEK CAPITAL LETTER EPSILON
            new int[] { 0x1D6AD, 0x1D6E7, 0x1D721, 0, 0x1D75B, 0, 0x1D795 },    // U+0396, GREEK CAPITAL LETTER ZETA
            new int[] { 0x1D6AE, 0x1D6E8, 0x1D722, 0, 0x1D75C, 0, 0x1D796 },    // U+0397, GREEK CAPITAL LETTER ETA
            new int[] { 0x1D6AF, 0x1D6E9, 0x1D723, 0, 0x1D75D, 0, 0x1D797 },    // U+0398, GREEK CAPITAL LETTER THETA
            new int[] { 0x1D6B0, 0x1D6EA, 0x1D724, 0, 0x1D75E, 0, 0x1D798 },    // U+0399, GREEK CAPITAL LETTER IOTA
            new int[] { 0x1D6B1, 0x1D6EB, 0x1D725, 0, 0x1D75F, 0, 0x1D799 },    // U+039A, GREEK CAPITAL LETTER KAPPA
            new int[] { 0x1D6B2, 0x1D6EC, 0x1D726, 0, 0x1D760, 0, 0x1D79A },    // U+039B, GREEK CAPITAL LETTER LAMDA
            new int[] { 0x1D6B3, 0x1D6ED, 0x1D727, 0, 0x1D761, 0, 0x1D79B },    // U+039C, GREEK CAPITAL LETTER MU
            new int[] { 0x1D6B4, 0x1D6EE, 0x1D728, 0, 0x1D762, 0, 0x1D79C },    // U+039D, GREEK CAPITAL LETTER NU
            new int[] { 0x1D6B5, 0x1D6EF, 0x1D729, 0, 0x1D763, 0, 0x1D79D },    // U+039E, GREEK CAPITAL LETTER XI
            new int[] { 0x1D6B6, 0x1D6F0, 0x1D72A, 0, 0x1D764, 0, 0x1D79E },    // U+039F, GREEK CAPITAL LETTER OMICRON
            new int[] { 0x1D6B7, 0x1D6F1, 0x1D72B, 0, 0x1D765, 0, 0x1D79F },    // U+03A0, GREEK CAPITAL LETTER PI
            new int[] { 0x1D6B8, 0x1D6F2, 0x1D72C, 0, 0x1D766, 0, 0x1D7A0 },    // U+03A1, GREEK CAPITAL LETTER RHO
            new int[] { 0x1D6B9, 0x1D6F3, 0x1D72D, 0, 0x1D767, 0, 0x1D7A1 },    // U+03F4, GREEK CAPITAL THETA SYMBOL
            new int[] { 0x1D6BA, 0x1D6F4, 0x1D72E, 0, 0x1D768, 0, 0x1D7A2 },    // U+03A3, GREEK CAPITAL LETTER SIGMA
            new int[] { 0x1D6BB, 0x1D6F5, 0x1D72F, 0, 0x1D769, 0, 0x1D7A3 },    // U+03A4, GREEK CAPITAL LETTER TAU
            new int[] { 0x1D6BC, 0x1D6F6, 0x1D730, 0, 0x1D76A, 0, 0x1D7A4 },    // U+03A5, GREEK CAPITAL LETTER UPSILON
            new int[] { 0x1D6BD, 0x1D6F7, 0x1D731, 0, 0x1D76B, 0, 0x1D7A5 },    // U+03A6, GREEK CAPITAL LETTER PHI
            new int[] { 0x1D6BE, 0x1D6F8, 0x1D732, 0, 0x1D76C, 0, 0x1D7A6 },    // U+03A7, GREEK CAPITAL LETTER CHI
            new int[] { 0x1D6BF, 0x1D6F9, 0x1D733, 0, 0x1D76D, 0, 0x1D7A7 },    // U+03A8, GREEK CAPITAL LETTER PSI
            new int[] { 0x1D6C0, 0x1D6FA, 0x1D734, 0, 0x1D76E, 0, 0x1D7A8 },    // U+03A9, GREEK CAPITAL LETTER OMEGA
            new int[] { 0x1D6C1, 0x1D6FB, 0x1D735, 0, 0x1D76F, 0, 0x1D7A9 },    // U+2207, NABLA
            new int[] { 0x1D6C2, 0x1D6FC, 0x1D736, 0, 0x1D770, 0, 0x1D7AA },    // U+03B1, GREEK SMALL LETTER ALPHA
            new int[] { 0x1D6C3, 0x1D6FD, 0x1D737, 0, 0x1D771, 0, 0x1D7AB },    // U+03B2, GREEK SMALL LETTER BETA
            new int[] { 0x1D6C4, 0x1D6FE, 0x1D738, 0, 0x1D772, 0, 0x1D7AC },    // U+03B3, GREEK SMALL LETTER GAMMA
            new int[] { 0x1D6C5, 0x1D6FF, 0x1D739, 0, 0x1D773, 0, 0x1D7AD },    // U+03B4, GREEK SMALL LETTER DELTA
            new int[] { 0x1D6C6, 0x1D700, 0x1D73A, 0, 0x1D774, 0, 0x1D7AE },    // U+03B5, GREEK SMALL LETTER EPSILON
            new int[] { 0x1D6C7, 0x1D701, 0x1D73B, 0, 0x1D775, 0, 0x1D7AF },    // U+03B6, GREEK SMALL LETTER ZETA
            new int[] { 0x1D6C8, 0x1D702, 0x1D73C, 0, 0x1D776, 0, 0x1D7B0 },    // U+03B7, GREEK SMALL LETTER ETA
            new int[] { 0x1D6C9, 0x1D703, 0x1D73D, 0, 0x1D777, 0, 0x1D7B1 },    // U+03B8, GREEK SMALL LETTER THETA
            new int[] { 0x1D6CA, 0x1D704, 0x1D73E, 0, 0x1D778, 0, 0x1D7B2 },    // U+03B9, GREEK SMALL LETTER IOTA
            new int[] { 0x1D6CB, 0x1D705, 0x1D73F, 0, 0x1D779, 0, 0x1D7B3 },    // U+03BA, GREEK SMALL LETTER KAPPA
            new int[] { 0x1D6CC, 0x1D706, 0x1D740, 0, 0x1D77A, 0, 0x1D7B4 },    // U+03BB, GREEK SMALL LETTER LAMDA
            new int[] { 0x1D6CD, 0x1D707, 0x1D741, 0, 0x1D77B, 0, 0x1D7B5 },    // U+03BC, GREEK SMALL LETTER MU
            new int[] { 0x1D6CE, 0x1D708, 0x1D742, 0, 0x1D77C, 0, 0x1D7B6 },    // U+03BD, GREEK SMALL LETTER NU
            new int[] { 0x1D6CF, 0x1D709, 0x1D743, 0, 0x1D77D, 0, 0x1D7B7 },    // U+03BE, GREEK SMALL LETTER XI
            new int[] { 0x1D6D0, 0x1D70A, 0x1D744, 0, 0x1D77E, 0, 0x1D7B8 },    // U+03BF, GREEK SMALL LETTER OMICRON
            new int[] { 0x1D6D1, 0x1D70B, 0x1D745, 0, 0x1D77F, 0, 0x1D7B9 },    // U+03C0, GREEK SMALL LETTER PI
            new int[] { 0x1D6D2, 0x1D70C, 0x1D746, 0, 0x1D780, 0, 0x1D7BA },    // U+03C1, GREEK SMALL LETTER RHO
            new int[] { 0x1D6D3, 0x1D70D, 0x1D747, 0, 0x1D781, 0, 0x1D7BB },    // U+03C2, GREEK SMALL LETTER FINAL SIGMA
            new int[] { 0x1D6D4, 0x1D70E, 0x1D748, 0, 0x1D782, 0, 0x1D7BC },    // U+03C3, GREEK SMALL LETTER SIGMA
            new int[] { 0x1D6D5, 0x1D70F, 0x1D749, 0, 0x1D783, 0, 0x1D7BD },    // U+03C4, GREEK SMALL LETTER TAU
            new int[] { 0x1D6D6, 0x1D710, 0x1D74A, 0, 0x1D784, 0, 0x1D7BE },    // U+03C5, GREEK SMALL LETTER UPSILON
            new int[] { 0x1D6D7, 0x1D711, 0x1D74B, 0, 0x1D785, 0, 0x1D7BF },    // U+03C6, GREEK SMALL LETTER PHI
            new int[] { 0x1D6D8, 0x1D712, 0x1D74C, 0, 0x1D786, 0, 0x1D7C0 },    // U+03C7, GREEK SMALL LETTER CHI
            new int[] { 0x1D6D9, 0x1D713, 0x1D74D, 0, 0x1D787, 0, 0x1D7C1 },    // U+03C8, GREEK SMALL LETTER PSI
            new int[] { 0x1D6DA, 0x1D714, 0x1D74E, 0, 0x1D788, 0, 0x1D7C2 },    // U+03C9, GREEK SMALL LETTER OMEGA
            new int[] { 0x1D6DB, 0x1D715, 0x1D74F, 0, 0x1D789, 0, 0x1D7C3 },    // U+2202, PARTIAL DIFFERENTIAL
            new int[] { 0x1D6DC, 0x1D716, 0x1D750, 0, 0x1D78A, 0, 0x1D7C4 },    // U+03F5, GREEK LUNATE EPSILON SYMBOL
            new int[] { 0x1D6DD, 0x1D717, 0x1D751, 0, 0x1D78B, 0, 0x1D7C5 },    // U+03D1, GREEK THETA SYMBOL
            new int[] { 0x1D6DE, 0x1D718, 0x1D752, 0, 0x1D78C, 0, 0x1D7C6 },    // U+03F0, GREEK KAPPA SYMBOL
            new int[] { 0x1D6DF, 0x1D719, 0x1D753, 0, 0x1D78D, 0, 0x1D7C7 },    // U+03D5, GREEK PHI SYMBOL
            new int[] { 0x1D6E0, 0x1D71A, 0x1D754, 0, 0x1D78E, 0, 0x1D7C8 },    // U+03F1, GREEK RHO SYMBOL
            new int[] { 0x1D6E1, 0x1D71B, 0x1D755, 0, 0x1D78F, 0, 0x1D7C9 }     // U+03D6, GREEK PI SYMBOL
        };

        private ushort[] GreekCharacters = new ushort[GreekChars]
        {
            0x0391,     // GREEK CAPITAL LETTER ALPHA
            0x0392,     // GREEK CAPITAL LETTER BETA
            0x0393,     // GREEK CAPITAL LETTER GAMMA
            0x0394,     // GREEK CAPITAL LETTER DELTA
            0x0395,     // GREEK CAPITAL LETTER EPSILON
            0x0396,     // GREEK CAPITAL LETTER ZETA
            0x0397,     // GREEK CAPITAL LETTER ETA
            0x0398,     // GREEK CAPITAL LETTER THETA
            0x0399,     // GREEK CAPITAL LETTER IOTA
            0x039A,     // GREEK CAPITAL LETTER KAPPA
            0x039B,     // GREEK CAPITAL LETTER LAMDA
            0x039C,     // GREEK CAPITAL LETTER MU
            0x039D,     // GREEK CAPITAL LETTER NU
            0x039E,     // GREEK CAPITAL LETTER XI
            0x039F,     // GREEK CAPITAL LETTER OMICRON
            0x03A0,     // GREEK CAPITAL LETTER PI
            0x03A1,     // GREEK CAPITAL LETTER RHO
            0x03F4,     // GREEK CAPITAL THETA SYMBOL
            0x03A3,     // GREEK CAPITAL LETTER SIGMA
            0x03A4,     // GREEK CAPITAL LETTER TAU
            0x03A5,     // GREEK CAPITAL LETTER UPSILON
            0x03A6,     // GREEK CAPITAL LETTER PHI
            0x03A7,     // GREEK CAPITAL LETTER CHI
            0x03A8,     // GREEK CAPITAL LETTER PSI
            0x03A9,     // GREEK CAPITAL LETTER OMEGA
            0x2207,     // NABLA
            0x03B1,     // GREEK SMALL LETTER ALPHA
            0x03B2,     // GREEK SMALL LETTER BETA
            0x03B3,     // GREEK SMALL LETTER GAMMA
            0x03B4,     // GREEK SMALL LETTER DELTA
            0x03B5,     // GREEK SMALL LETTER EPSILON
            0x03B6,     // GREEK SMALL LETTER ZETA
            0x03B7,     // GREEK SMALL LETTER ETA
            0x03B8,     // GREEK SMALL LETTER THETA
            0x03B9,     // GREEK SMALL LETTER IOTA
            0x03BA,     // GREEK SMALL LETTER KAPPA
            0x03BB,     // GREEK SMALL LETTER LAMDA
            0x03BC,     // GREEK SMALL LETTER MU
            0x03BD,     // GREEK SMALL LETTER NU
            0x03BE,     // GREEK SMALL LETTER XI
            0x03BF,     // GREEK SMALL LETTER OMICRON
            0x03C0,     // GREEK SMALL LETTER PI
            0x03C1,     // GREEK SMALL LETTER RHO
            0x03C2,     // GREEK SMALL LETTER FINAL SIGMA
            0x03C3,     // GREEK SMALL LETTER SIGMA
            0x03C4,     // GREEK SMALL LETTER TAU
            0x03C5,     // GREEK SMALL LETTER UPSILON
            0x03C6,     // GREEK SMALL LETTER PHI
            0x03C7,     // GREEK SMALL LETTER CHI
            0x03C8,     // GREEK SMALL LETTER PSI
            0x03C9,     // GREEK SMALL LETTER OMEGA
            0x2202,     // PARTIAL DIFFERENTIAL
            0x03F5,     // GREEK LUNATE EPSILON SYMBOL
            0x03D1,     // GREEK THETA SYMBOL
            0x03F0,     // GREEK KAPPA SYMBOL
            0x03D5,     // GREEK PHI SYMBOL
            0x03F1,     // GREEK RHO SYMBOL
            0x03D6      // GREEK PI SYMBOL
        };

#if WINRT
        /// <summary>
        /// Convert the string to regular style.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The styled string.</returns>
        public string StyleConvert(string str) => StyleConvert(str, UnicodeStyles.Regular);
#endif

        /// <summary>
        /// Convert the string to target type.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="style">The style you want.</param>
        /// <returns>The styled string.</returns>
        public string StyleConvert(string str, UnicodeStyles style
#if !WINRT
            = UnicodeStyles.Regular
#endif
            )
        {
            string input = str;

            if (input.Length > 0)
            {
                input = ToRegular(input);

                int index = (int)style;

                string output = index == -1 ? input : ToStyled(input, index);
                return output;
            }

            return input;
        }

        private string ToRegular(string input)
        {
            string output = string.Empty;
            int hi = 0;

            for (int i = 0; i < input.Length; i++)
            {
                int cp = input[i];

                switch (cp)
                {
                    case >= HighSurrogateFirst and <= HighSurrogateLast:
                        if (hi != 0)
                        {
                            output += ReplacementCharacter;
                        }
                        hi = cp;
                        cp = 0;
                        break;

                    case >= LowSurrogateFirst and <= LowSurrogateLast:
                        if (hi == 0)
                        {
                            output += ReplacementCharacter;
                        }
                        else
                        {
                            cp = FromSurrogates(hi, cp);
                            hi = 0;
                        }
                        break;

                    default:
                        if (hi != 0)
                        {
                            output += ReplacementCharacter;
                            hi = 0;
                        }
                        break;
                }

                if (cp >= FirstTarget)
                {
                    int row = 0;
                    bool isLatin = true;

                    switch (cp)
                    {
                        case >= MathLatinFirst and <= MathLatinLast:
                            row = (cp - MathLatinFirst) % MathLatinRange;
                            if (row < 0x1A)
                            {
                                row += LatinCapitalOffset;
                            }
                            else
                            {
                                row -= 0x1A;
                                row += LatinSmallOffset;
                            }
                            break;

                        case >= MathGreekFirst and <= MathGreekLast:
                            row = (cp - MathGreekFirst) % MathGreekRange;
                            cp = GreekCharacters[row];
                            isLatin = false;
                            break;

                        case >= MathDigitsFirst and <= MathDigitsLast:
                            row = LatinDigitsOffset + ((cp - MathDigitsFirst) % MathDigitsRange);
                            break;

                        // U+1D6A4 MATHEMATICAL ITALIC SMALL DOTLESS I
                        case 0x1D6A4:
                            cp = 0x131;
                            isLatin = false;
                            break;

                        // U+1D6A5 MATHEMATICAL ITALIC SMALL DOTLESS J
                        case 0x1D6A5:
                            cp = 0x237;
                            isLatin = false;
                            break;

                        // U+1D7CA MATHEMATICAL BOLD CAPITAL DIGAMMA
                        case 0x1D7CA:
                            cp = 0x3DC;
                            isLatin = false;
                            break;

                        // U+1D7CB MATHEMATICAL BOLD SMALL DIGAMMA
                        case 0x1D7CB:
                            cp = 0x3DD;
                            isLatin = false;
                            break;
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
            string output = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                int cp = input[i];
                int result = 0;

                if (cp is >= BasicLatinFirst and <= BasicLatinLast)
                {
                    int offset = cp - BasicLatinFirst;
                    result = LatinMapping[offset][style];
                }
                else
                {
                    switch (style)
                    {
                        case > 1 and < GreekStyles:
                            bool greek = cp is (>= 0x0391 and <= 0x03C9)
                                or (>= 0x03D1 and <= 0x03D6)
                                or (>= 0x03F0 and <= 0x03F5)
                                or (>= 0x2202 and <= 0x2207);

                            int offset = 0;

                            if (greek)
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

                            if (greek)
                            {
                                result = GreekMapping[offset][style];
                            }
                            break;

                        // Special Cases
                        case 0:
                            switch (cp)
                            {
                                // U+1D7CA MATHEMATICAL BOLD CAPITAL DIGAMMA
                                case 0x3DC:
                                    result = 0x1D7CA;
                                    break;

                                // U+1D7CB MATHEMATICAL BOLD SMALL DIGAMMA
                                case 0x3DD:
                                    result = 0x1D7CB;
                                    break;
                            }
                            break;

                        case 1:
                            switch (cp)
                            {
                                // U+1D6A4 MATHEMATICAL ITALIC SMALL DOTLESS I
                                case 0x131:
                                    result = 0x1D6A4;
                                    break;

                                // U+1D6A5 MATHEMATICAL ITALIC SMALL DOTLESS J
                                case 0x237:
                                    result = 0x1D6A5;
                                    break;
                            }
                            break;
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
            if ((hi is >= HighSurrogateFirst and <= HighSurrogateLast)
                && (lo is >= LowSurrogateFirst and <= LowSurrogateLast))
            {
                cp = ((hi - HighSurrogateFirst) << HalfShift) + (lo - LowSurrogateFirst) + HalfBase;
            }

            return cp;
        }

        /// <summary>
        /// Dispose the styler.
        /// </summary>
        /// <param name="disposing">Is disposing?</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                LatinMapping = null;
                GreekMapping = null;
                GreekCharacters = null;
            }
        }

        /// <summary>
        /// Dispose the styler.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
