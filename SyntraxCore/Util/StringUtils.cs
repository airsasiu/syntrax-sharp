using SyntraxCore.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SyntraxCore.Util
{
    public class StringUtils
    {
        public static MyFont FontFromString(string txt)
        {
            Regex fontRegex = new Regex("\\(\\s*'([a-zA-Z]+)'\\s*,\\s*(\\d+)\\s*,\\s*'([a-zA-Z ]+)'\\s*\\)";
            Match matcher = fontRegex.Match(txt.Trim());
            if (!matcher.Success)
            {
                throw new ArgumentException("Invalid font style in config");
            }

            string name = matcher.Groups[1].Value;

            var style = FontStyle.Plain;
            string styleText = matcher.Groups[3].Value;

            if (styleText.Contains("bold"))
            {
                style |= FontStyle.Bold;
            }

            if (styleText.Contains("italic"))
            {
                style |= FontStyle.Italic;
            }
            int size = int.Parse(matcher.Groups[2].Value);
            return new MyFont(name, style, size);
        }

        public static Color ColorFromString(string txt)
        {
            Regex colorRegex = new Regex("\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*(,\\s*(\\d+)\\s*)?\\)");
            Match matcher = colorRegex.Match(txt.Trim());
            if (!matcher.Success)
            {
                throw new ArgumentException("Invalid color style in config");
            }
            int r = int.Parse(matcher.Groups[1].Value);
            int g = int.Parse(matcher.Groups[2].Value);
            int b = int.Parse(matcher.Groups[3].Value);
            int a = 255;
            if (matcher.Groups[5].Success)
            {
                a = int.Parse(matcher.Groups[5].Value);
            }
            return Color.FromArgb(a, r, g, b);
        }
        public static string EscapeXML(string text)
        {
            return text
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;");
        }


        public static string SnakeToCamelCase(string snakeCase)
        {
            string[] parts = snakeCase.Split("_");
            StringBuilder camelCaseString = new StringBuilder(parts[0]);
            foreach (string part in parts)
            {
                camelCaseString.Append(char.ToUpper(part[0]))
                                .Append(part.Substring(1));
            }
            return camelCaseString.ToString();
        }

        public static string ToHex(Color c)
        {
            return "#" + Hex(c.R / 16) + Hex(c.R % 16)
                    + Hex(c.G / 16) + Hex(c.G % 16)
                    + Hex(c.B / 16) + Hex(c.B % 16);
        }
        private static char Hex(int val)
        {
            if (val < 10)
            {
                return (char)(val + (int)('0'));
            }
            if (val < 16)
            {
                return (char)(val - 10 + (int)('a'));
            }
            return '@';
        }

        public static double FillOpacity(Color c)
        {
            return c.A / 255.0;
        }
    }
}
