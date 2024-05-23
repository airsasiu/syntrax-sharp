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
    public class BoxBubbleElement : BubbleElementBase
    {
        public BoxBubbleElement(Point start, Point end, string href, string text,
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

            sb.Append("<rect x=\"").Append(x0).Append("\" y=\"").Append(y0)
                    .Append("\" width=\"").Append(x1 - x0).Append("\" height=\"").Append(y1 - y0)
                    .Append("\" ").Append(attributes).Append(" />\n");
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            int x0 = Start.X;
            int y0 = Start.Y;
            int x1 = Start.X;
            int y1 = Start.Y;

            string attributes = "stroke=\"" + StringUtils.ToHex(style.LineColor) + "\" "
                    + "stroke-width=\"" + this.Width + "\" "
                    + "fill=\"" + StringUtils.ToHex(this.FillColor) + "\" "
                    + "fill-opacity=\"" + StringUtils.FillOpacity(this.FillColor) + "\"";

            sb.Append("<rect x=\"").Append(x0).Append("\" y=\"").Append(y0)
                    .Append("\" width=\"").Append(x1 - x0).Append("\" height=\"").Append(y1 - y0)
                    .Append("\" ").Append(attributes).Append(" />\n");

            // Add text
            AddXmlText(sb, style);
        }
    }
}
