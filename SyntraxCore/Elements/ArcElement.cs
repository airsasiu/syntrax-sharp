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
    public class ArcElement : Element
    {
        private int _width;
        private int _startAngle;
        private int _extentAngle;
        public ArcElement(Point start, Point end, int width, int startAngle, int extentAngle, string tag)
            : base(tag)
        {
            Start = start;
            End = end;
            _width = width;
            _startAngle = startAngle;
            _extentAngle = extentAngle % 360;
        }

        public override void AddShadow(StringBuilder sb, StyleConfig style)
        {
            base.AddShadow(sb, style);
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            Point center = new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2);
            int rad = (End.X - Start.X) / 2;
            int stop = (_startAngle + _extentAngle) % 360;
            if (_extentAngle < 0)
            {
                int t = _startAngle;
                _startAngle = stop;
                stop = t;
            }

            double startRad = ToRadians(_startAngle);
            double stopRad = ToRadians(stop);

            string attributes = "stroke=\"" + StringUtils.ToHex(style.LineColor)+ "\" "
                + "stroke-width=\"" + _width + "\" fill=\"none\"";

            int xs = (int)(center.X + rad * Math.Cos(startRad));
            int ys = (int)(center.Y + rad * Math.Sin(startRad));
            int xe = (int)(center.X + rad * Math.Cos(stopRad));
            int ye = (int)(center.Y + rad * Math.Sin(stopRad));

            sb.Append("<path d=\"M").Append(xs).Append(",").Append(ys)
                .Append(" A").Append(rad).Append(",").Append(rad)
                .Append(" 0 0,0 ").Append(xe).Append(",").Append(ye)
                .Append("\" ").Append(attributes).Append("/>\n");
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
