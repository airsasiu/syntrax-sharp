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
    public class LineElement : Element
    {
        public string Arrow { get; set; }
        public int Width { get; set; }
        public LineElement(Point start, Point end, string arrow, int width, string tag) :
            base(tag)
        {
            Start = start;
            End = end;
            Arrow = arrow;
            Width = width;
        }

        public override void AddShadow(StringBuilder sb, StyleConfig style)
        {
            //do nothing;
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            String attributes = "stroke=\"" + StringUtils.ToHex(style.LineColor)
                + "\" " + "stroke-width=\"" + Width + "\"";

            if (string.IsNullOrEmpty(Arrow))
            {
                attributes += " marker-end=\"url(#arrow)\"";
                if (Arrow == "first")
                {
                    // swap
                    var tmp = Start;
                    Start = End;
                    End = tmp;
                }
                double len = Math.Sqrt(
                        (End.X - Start.X) * (End.X - Start. X)
                                + (End.Y - Start.Y) * (End.Y - Start.Y)
                );
                len -= 4;
                double angle = Math.Atan2(End.Y - Start.Y, End.Y - Start.Y);

                End = new Point((int)(Start.X + len * Math.Cos(angle)), (int)(Start.Y + len * Math.Sin(angle)));
            }
            sb.Append("<line ")
                    .Append("x1=\"").Append(Start.X).Append("\" ")
                    .Append("y1=\"").Append(Start.Y).Append("\" ")
                    .Append("x2=\"").Append(End.X).Append("\" ")
                    .Append("y2=\"").Append(End.Y).Append("\" ")
                    .Append(attributes).Append(" />\n");
        }

        public override void Scale(double scale)
        {
            base.Scale(scale);
            Width = (int)(Width * scale);
        }
    }
}
