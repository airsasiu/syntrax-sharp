using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    public class MyFont
    {
        public string Name { get; set; }
        public FontStyle Style { get; set; }
        public double Size { get; set; }
        public MyFont(string name, FontStyle style, double size)
        {
            Name = name;
            Style = style;
            Size = size;
        }
    }

    public enum FontStyle
    {
        Bold = 1,
        Italic = 2,
        Plain = 4,
    }
}
