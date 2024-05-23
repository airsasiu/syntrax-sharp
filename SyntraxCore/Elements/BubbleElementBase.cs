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
    public abstract class BubbleElementBase : Element
    {
        public string Href { get; set; }
        public string Text { get; set; }
        public Point TextPos { get; set; }
        public MyFont Font { get; set; }
        public string FontName { get; set; }
        public Color TextColor { get; set; }
        public int Width { get; set; }
        public Color FillColor { get; set; }
        public BubbleElementBase(Point start, Point end, string href, string text,
            Point textPos, MyFont font, string fontName, Color textColor, int width,
            Color fillColor, string tag) : base(tag)
        {
            Start = start;
            End = end;
            Href = href;
            Text = text;
            TextPos = textPos;
            Font = font;
            FontName = fontName;
            TextColor = textColor;
            Width = width;
            FillColor = fillColor;
        }

        public virtual int GetX(StyleConfig style)
        {
            return (Start.X + End.X) / 2;
        }

        public void AddXmlText(StringBuilder sb, StyleConfig style)
        {
            int ys = Start.Y;
            int ye = End.Y;
            int x = GetX(style);
            int y = (ys + ye) / 2 + (int)(Math.Abs(TextPos.Y) * 0.25 + style.Scale * 2);

            string txt = StringUtils.EscapeXML(Text);
            if (string.IsNullOrEmpty(Href))
            {
                sb.Append("<text class=\"").Append(FontName).Append("\" x=\"").Append(x)
                    .Append("\" y=\"").Append(y).Append("\">").Append(txt).Append("</text>\n");
            }
            else
            {
                string link = StringUtils.EscapeXML(Href);
                sb.Append("<a xlink:href=\"").Append(link).Append("\" target=\"_parent\">")
                    .Append("<text class=\"").Append(FontName).Append(" link\" x=\"").Append(x)
                    .Append("\" y=\"").Append(y).Append("\">").Append(txt).Append("</text></a>\n");
            }
        }

        public override void Scale(double scale)
        {
            base.Scale(scale);
            Width = (int)(Width * scale);
            TextPos.Offset((int)(TextPos.X * scale), (int)(TextPos.Y * scale));
            if (Font != null)
            {

            }
        }
    }
}
