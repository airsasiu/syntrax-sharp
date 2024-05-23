using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    internal class NodeBoxStyle : NodeStyle
    {
        public NodeBoxStyle()
        {
            Name = "box";
            Shape = "box";
            Pattern = "^/(.*)";
            Font = new MyFont("Times", FontStyle.Italic, 14);
            FillColor = Color.FromArgb(144, 164, 174);
        }
    }
}
