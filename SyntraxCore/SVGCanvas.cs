using SyntraxCore.Elements;
using SyntraxCore.Style;
using SyntraxCore.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyntraxCore
{
    public class SVGCanvas
    {
        private StyleConfig style;
        private List<Element> elements = new List<Element>();
        private Dictionary<string, int> tagCnt = new Dictionary<string, int>();

        public (Point, Point) GetBoundingBoxByTag(string tag)
        {
            int sx = 0, sy = 0;
            int ex = 0, ey = 0;
            foreach (Element e in elements)
            {
                if (e.IsTagged(tag))
                {
                    sx = Math.Min(sx, e.Start.X);
                    sx = Math.Min(sx, e.End.X);
                    sy = Math.Min(sy, e.Start.Y);
                    sy = Math.Min(sy, e.End.Y);

                    ex = Math.Max(ex, e.Start.X);
                    ex = Math.Max(ex, e.End.X);
                    ey = Math.Max(ey, e.Start.Y);
                    ey = Math.Max(ey, e.End.Y);
                }
            }
            return (new Point(sx, sy), new Point(ex, ey));
        }

        public string GenerateSVG()
        {
            StringBuilder sb = new StringBuilder();
            double scale = style.GetScale();

            var res = GetBoundingBoxByTag("all");

            // move to picture to (0, 0)
            MoveByTag("all", -res.Item1.X, -res.Item1.Y);
            ScaleByTag("all", scale);
            MoveByTag("all", style.GetPadding(), style.GetPadding());

            res = GetBoundingBoxByTag("all");
            var end = res.Item2;

            int w = end.X + style.GetPadding();
            int h = end.Y + style.GetPadding();

            // collect fonts
            Dictionary<string, (string, Color)> fonts = new Dictionary<string, (string, Color)>();
            fonts["title_font"] = (style.GetTitleFont(), style.GetTextColor());
            foreach (NodeStyle ns in style.GetNodeStyles())
            {
                fonts.Add(ns.Name + "_font", (ns.Font, ns.TextColor));
            }

            // header
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\n")
                    .Append("<!-- Created by Jyntrax https://github.com/atp-mipt/jsyntrax -->\n");
            sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\"\n");
            sb.Append("xmlns:xlink=\"http://www.w3.org/1999/xlink\"\n");
            sb.Append("xml:space=\"preserve\"\n");
            sb.Append("width=\"").Append(w).Append("\" ")
                    .Append("height=\"").Append(h).Append("\" ")
                    .Append("version=\"1.1\">\n");
            // styles
            sb.Append("<style type=\"text/css\">\n");
            sb.Append("<![CDATA[\n");
            // fonts
            foreach (var fontPair in fonts)
            {
                string fontName = fontPair.Key;
                string fontFamily = fontPair.getValue().f.getName();
                string fontSize = Integer.tostring((int)(fontPair.getValue().f.getSize() * scale));
                string fontWeight = "normal";
                if ((fontPair.getValue().f.getStyle() & Font.BOLD) == Font.BOLD)
                {
                    fontWeight = "bold";
                }
                string fontStyle = "normal";
                if ((fontPair.getValue().f.getStyle() & Font.ITALIC) == Font.ITALIC)
                {
                    fontStyle = "italic";
                }

                string hex = stringUtils.toHex(fontPair.getValue().s);

                sb.Append(".").Append(fontName).Append(" ");
                if (!"title_font".equals(fontName))
                {
                    sb.Append("{fill:").Append(hex).Append("; text-anchor:middle;\n");
                }
                else
                {
                    switch (style.TitlePos)
                    {
                        case TitlePosition.BottomLeft:
                        case TitlePosition.TopLeft:
                            sb.Append("{fill:").Append(hex).Append("; text-anchor:start;\n");
                            break;
                        case TitlePosition.BottomCenter:
                        case TitlePosition.TopCenter:
                            sb.Append("{fill:").Append(hex).Append("; text-anchor:middle;\n");
                            break;
                        case TitlePosition.BottomRight:
                        case TitlePosition.TopRight:
                            sb.Append("{fill:").Append(hex).Append("; text-anchor:end;\n");
                            break;
                    }
                }
                sb.Append("font-family:").Append(fontFamily).Append("; ");
                sb.Append("font-size:").Append(fontSize).Append("pt; ");
                sb.Append("font-weight:").Append(fontWeight).Append("; ");
                sb.Append("font-style:").Append(fontStyle).Append("; ");
                sb.Append("}\n");
            }
            // other fonts
            sb.Append(".label {fill: #000; text-anchor:middle; font-size:16pt; font-weight:bold; font-family:Sans;}\n");
            sb.Append(".link {fill: #0D47A1;}\n");
            sb.Append(".link:hover {fill: #0D47A1; text-decoration:underline;}\n");
            sb.Append(".link:visited {fill: #4A148C;}\n");
            // close
            sb.Append("]]>\n</style>\n");
            // defs
            sb.Append("<defs>\n");
            sb.Append("<marker id=\"arrow\" markerWidth=\"5\" markerHeight=\"4\" ")
                    .Append("refX=\"2.5\" refY=\"2\" orient=\"auto\" markerUnits=\"strokeWidth\">\n");
            string hex = StringUtils.ToHex(style.getLineColor());
            sb.Append("<path d=\"M0,0 L0.5,2 L0,4 L4.5,2 z\" fill=\"").Append(hex).Append("\" />\n");
            sb.Append("</marker>\n</defs>\n");

            // elements
            if (!style.isTransparent())
            {
                sb.Append("<rect width=\"100%\" height=\"100%\" fill=\"white\"/>\n");
            }
            foreach (Element e in elements)
            {

                if (style.isShadow())
                {
                    e.AddShadow(sb, style);
                }
                e.toSVG(sb, style);
            }
            // end
            sb.Append("</svg>\n");
            return sb.tostring();
        }

        private void ScaleByTag(string tag, double scale)
        {
            foreach (Element e in elements)
            {
                if (e.IsTagged(tag))
                {
                    e.Scale(scale);
                }
            }
        }

        private void MoveByTag(string tag, int dx, int dy)
        {
            foreach (Element e in elements)
            {
                if (e.IsTagged(tag))
                {
                    e.Start.Offset(dx, dy);
                    e.End.Offset(dx, dy);
                }
            }
        }
    }
}
