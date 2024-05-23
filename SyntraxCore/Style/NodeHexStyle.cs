using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    internal class NodeHexStyle : NodeStyle
    {
        public NodeHexStyle()
        {
            Name = "hex";
            Shape = "hex";
            Pattern = "\\w(.*)";
            Font = new MyFont("Sans", FontStyle.Bold, 14);
            FillColor = Color.FromArgb(255, 0, 0, 127);
        }
    }
}
