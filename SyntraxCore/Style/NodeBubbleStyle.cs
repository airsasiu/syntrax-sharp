using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    internal class NodeBubbleStyle : NodeStyle
    {
        public NodeBubbleStyle()
        {
            Name = "bubble";
            Shape = "bubble";
            Pattern = "^(\\w.*)";
            Font = new MyFont("Sans", FontStyle.Bold, 14);
            TextColor = Color.FromArgb(0, 0, 0);
            FillColor = Color.FromArgb(179, 229, 252);
        }
    }
}
