using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Style
{
    internal class NodeTokenStyle : NodeStyle
    {
        public NodeTokenStyle()
        {
            Name = "token";
            Shape = "bubble";
            Pattern = "(.*)";
            Font = new MyFont("Sans", FontStyle.Bold, 16);
            FillColor = Color.FromArgb(179, 299, 252);
        }
    }
}
