using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    public class NodeStyle
    {
        public string Name { get; set; } = "unknown";
        public string Shape { get; set; } = "bubble";
        public MyFont Font { get; set; } = new MyFont("Sans", FontStyle.Bold, 14);
        public Color TextColor { get; set; } = Color.FromArgb(0, 0, 0);
        public Color FillColor { get; set; } = Color.FromArgb(144, 164, 174);
        public string Pattern { get; set; } = string.Empty;

        public bool Match(string txt)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                return true;
            }
            return Regex.IsMatch(txt, Pattern);
        }

        public string UnwrapTextContent(string txt)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                return txt;
            }
            StringBuilder sb = new StringBuilder();
            var matches = Regex.Matches(txt, Pattern);

            if(matches.Count == 0)
            {
                return txt;
            }
            foreach (var item in matches.Skip(1))
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
