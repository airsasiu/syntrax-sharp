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
        private StyleConfig _style;
        private List<Element> _elements = new List<Element>();
        public Dictionary<string, int> _tagCnt = new Dictionary<string, int>();

        public SVGCanvas(StyleConfig style)
        {
            _style = style;
        }

        public string NewTag(string prefix, string suffix)
        {
            string f = prefix + "___" + suffix;
            int value = 0;
            if (_tagCnt.ContainsKey(f))
            {
                value = _tagCnt[f];
            }
            _tagCnt[f]++;
            return prefix + value + suffix;
        }

        public void AddElement(Element element)
        {
            _elements.Add(element);
        }

        public void AddElementTag(string addTag, string tag)
        {
            foreach (Element e in _elements)
            {
                if (e.IsTagged(tag))
                {
                    e.AddTag(addTag);
                }
            }
        }

        public void DropElementTag(string tag)
        {
            foreach (Element e in _elements)
            {
                if (e.IsTagged(tag))
                {
                    e.DelTag(tag);
                }
            }
        }

        public void MoveElement(string tag, int dx, int dy)
        {
            foreach (Element e in _elements)
            {
                if (e.IsTagged(tag))
                {
                    e.Start.Offset(dx, dy);
                    e.End.Offset(dx, dy);
                }
            }
        }

        public void ScaleElement(string tag, double scale)
        {
            foreach(Element e in _elements)
            {
                if (e.IsTagged(tag))
                {
                    e.Scale(scale);
                }
            }
        }

        public string GetCanvasTag()
        {
            return _elements.FirstOrDefault()?.GetAnyTag();
        }


        public (Point, Point) GetBoundingBoxByTag(string tag)
        {
            int sx = 0, sy = 0;
            int ex = 0, ey = 0;
            foreach (Element e in _elements)
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
            double scale = _style.Scale;

            var res = GetBoundingBoxByTag("all");

            // move to picture to (0, 0)
            MoveElement("all", -res.Item1.X, -res.Item1.Y);
            ScaleElement("all", scale);
            MoveElement("all", _style.Padding, _style.Padding);

            res = GetBoundingBoxByTag("all");
            var end = res.Item2;

            int w = end.X + _style.Padding;
            int h = end.Y + _style.Padding;

            // collect fonts
            Dictionary<string, (MyFont, Color)> fonts = new Dictionary<string, (MyFont, Color)>();
            fonts["title_font"] = (_style.TitleFont, _style.TextColor);
            foreach (NodeStyle ns in _style.NodeStyles)
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
                string fontFamily = fontPair.Value.Item1.Name;
                string fontSize = (fontPair.Value.Item1.Size * scale).ToString();
                string fontWeight = "normal";
                if ((fontPair.Value.Item1.Style & FontStyle.Bold) == FontStyle.Bold)
                {
                    fontWeight = "bold";
                }
                string fontStyle = "normal";
                if ((fontPair.Value.Item1.Style & FontStyle.Italic) == FontStyle.Italic)
                {
                    fontStyle = "italic";
                }

                string hexColor = StringUtils.ToHex(fontPair.Value.Item2);

                sb.Append(".").Append(fontName).Append(" ");
                if (fontName != "title_font")
                {
                    sb.Append("{fill:").Append(hexColor).Append("; text-anchor:middle;\n");
                }
                else
                {
                    switch (_style.TitlePos)
                    {
                        case TitlePosition.BottomLeft:
                        case TitlePosition.TopLeft:
                            sb.Append("{fill:").Append(hexColor).Append("; text-anchor:start;\n");
                            break;
                        case TitlePosition.BottomCenter:
                        case TitlePosition.TopCenter:
                            sb.Append("{fill:").Append(hexColor).Append("; text-anchor:middle;\n");
                            break;
                        case TitlePosition.BottomRight:
                        case TitlePosition.TopRight:
                            sb.Append("{fill:").Append(hexColor).Append("; text-anchor:end;\n");
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
            if (!_style.Transparency)
            {
                sb.Append("<rect width=\"100%\" height=\"100%\" fill=\"white\"/>\n");
            }
            foreach (Element e in _elements)
            {

                if (_style.Shadow)
                {
                    e.AddShadow(sb, _style);
                }
                e.ToSVG(sb, _style);
            }
            // end
            sb.Append("</svg>\n");
            return sb.ToString();
        }
    }
}
