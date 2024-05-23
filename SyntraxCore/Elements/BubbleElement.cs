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
    public class BubbleElement : BubbleElementBase
    {
        public BubbleElement(Point start, Point end, string href, string text,
            Point textPos, MyFont font, string fontName, Color textColor, int width,
            Color fillColor, string tag) :
            base(start, end, href, text, textPos, font, fontName, textColor, width, fillColor, tag)
        {

        }

        public override void AddShadow(StringBuilder sb, StyleConfig style)
        {
            int x0 = Start.X + Width + 1;
            int y0 = Start.Y + Width + 1;
            int x1 = End.X + Width + 1;
            int y1 = End.Y + Width + 1;

            string attributes = "fill=\"" + StringUtils.ToHex(style.ShadowFillColor) + "\" "
                    + "fill-opacity=\"" + StringUtils.FillOpacity(style.ShadowFillColor) + "\"";

            int rad = (y1 - y0) / 2;
            int lft = x0 + rad;
            int rgt = x1 - rad;

            int xc = (x0 + x1) / 2;
            int yc = (y0 + y1) / 2;

            if (Math.Abs(rgt - lft) <= 1)
            {
                // Circlular bubble
                sb.Append("<circle ")
                        .Append("cx=\"").Append(xc).Append("\" ")
                        .Append("cy=\"").Append(yc).Append("\" ")
                        .Append("r=\"").Append(rad).Append("\" ")
                        .Append(attributes).Append(" />\n");
            }
            else
            {
                // Rounded bubble
                sb.Append("<path d=\"M").Append(lft).Append(",").Append(y1)
                        .Append(" A").Append(rad).Append(",").Append(rad)
                        .Append(" 0 0,1 ").Append(lft).Append(",").Append(y0)
                        .Append(" H").Append(rgt).Append(" A").Append(rad).Append(",")
                        .Append(rad).Append(" 0 0,1 ").Append(rgt).Append(",").Append(y1).Append(" z\" ")
                        .Append(attributes).Append(" />\n");
            }
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            int x0 = Start.X;
            int y0 = Start.Y;
            int x1 = End.X;
            int y1 = End.Y;

            string attributes = "stroke=\"" + StringUtils.ToHex(style.LineColor) + "\" "
                    + "stroke-width=\"" + Width + "\" "
                    + "fill=\"" + StringUtils.ToHex(this.FillColor) + "\" "
                    + "fill-opacity=\"" + StringUtils.FillOpacity(this.FillColor) + "\"";

            int rad = (y1 - y0) / 2;
            int lft = x0 + rad;
            int rgt = x1 - rad;

            int xc = (x0 + x1) / 2;
            int yc = (y0 + y1) / 2;

            if (Math.Abs(rgt - lft) <= 1)
            {
                // Circlular bubble
                sb.Append("<circle ")
                    .Append("cx=\"").Append(xc).Append("\" ")
                    .Append("cy=\"").Append(yc).Append("\" ")
                    .Append("r=\"").Append(rad).Append("\" ")
                    .Append(attributes).Append(" />\n");
            }
            else
            {
                // Rounded bubble
                sb.Append("<path d=\"M").Append(lft).Append(",").Append(y1)
                        .Append(" A").Append(rad).Append(",").Append(rad)
                        .Append(" 0 0,1 ").Append(lft).Append(",").Append(y0)
                        .Append(" H").Append(rgt).Append(" A").Append(rad).Append(",")
                        .Append(rad).Append(" 0 0,1 ").Append(rgt).Append(",").Append(y1).Append(" z\" ")
                        .Append(attributes).Append(" />\n");
            }

            // Add text
        }
    }
}
