using SyntraxCore.Style;
using SyntraxCore.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Elements
{
    public class OvalElement : Element
    {
        public Color FillColor { get; set; }
        public int Width { get; set; }
        public OvalElement(Point start, Point end, int width, Color fillColor, string tag) :
            base(tag)
        {
            Start = start;
            End = end;
            FillColor = fillColor;
            Width = width;
        }

        public override void AddShadow(StringBuilder sb, StyleConfig style)
        {
            //do nothing;
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            int x0 = Start.X;
            int y0 = Start.Y;
            int x1 = End.X;
            int y1 = End.Y;

            String attributes = "stroke=\"" + StringUtils.ToHex(style.LineColor) + "\" "
                    + "stroke-width=\"" + Width + "\" "
                    + "fill=\"" + StringUtils.ToHex(FillColor) + "\"";

            int xc = (x0 + x1) / 2;
            int yc = (y0 + y1) / 2;
            int rad = (x1 - x0) / 2;

            sb.Append("<circle cx=\"").Append(xc).Append("\" cy=\"").Append(yc)
                    .Append("\" r=\"").Append(rad).Append("\" ")
                    .Append(attributes).Append("/>\n");
        }

        public override void Scale(double scale)
        {
            base.Scale(scale);
            Width = (int)(Width * scale);
        }
    }
}
