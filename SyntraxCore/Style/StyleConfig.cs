

using IniParser;
using IniParser.Model;
using System.Drawing;

namespace SyntraxCore.Style
{
    public class StyleConfig
    {
        public int LineWidth { get; set; } = 2;
        public int OutLineWidth { get; set; } = 2;
        public int Padding { get; set; } = 5;
        public Color LineColor { get; set; } = Color.FromArgb(0, 0, 0);
        public int MaxRadius { get; set; } = 9;
        public int HSep { get; set; } = 17;
        public int VSep { get; set; } = 9;
        public bool Arrows { get; set; } = true;
        public TitlePosition TitlePos { get; set; } = TitlePosition.TopLeft;
        public Color BulletFillColor { get; set; } = Color.FromArgb(255, 255, 255);
        public Color TextColor { get; set; } = Color.FromArgb(0, 0, 0);
        public bool Shadow { get; set; } = true;
        public Color ShadowFillColor { get; set; } = Color.FromArgb(0, 0, 0, 127);
        public MyFont TitleFont { get; set; } = new MyFont("Sans", FontStyle.Bold, 23);
        public double Scale { get; set; }
        public bool Transparency { get; set; }
        public NodeStyle DefNodeStyle { get; set; } = new NodeStyle();
        public List<NodeStyle> NodeStyles { get; set; } = new List<NodeStyle>()
        {
            new NodeBubbleStyle(),
            new NodeBoxStyle(),
            new NodeTokenStyle(),
            new NodeHexStyle(),
        };

        public StyleConfig(double scale, bool transparency)
        {
            Scale = scale;
            Transparency = transparency;

            var parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile("syntrax-sharp.ini");
            if (iniData != null)
            {
                ParseStyleArgs(iniData);
            }
        }

        public StyleConfig(double scale, bool transparency, string stylePath)
        {
            Scale = scale;
            Transparency = transparency;
            var parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile(stylePath);
            if (iniData != null)
            {
                ParseStyleArgs(iniData);
            }
            foreach (var section in iniData.Sections)
            {
                if (section.SectionName == "style")
                {
                    continue;
                }
                NodeStyle ns = new NodeStyle();
                ns.Name = section.SectionName;
                ParseNodeStyle(iniData, ns);
                NodeStyles.Add(ns);
            }
        }

        private void ParseStyleArgs(IniData iniData)
        {
            if (iniData.Sections.ContainsSection("style") == false)
            {
                return;
            }
            LineWidth = int.Parse(iniData["style"]["line_width"]);
            OutLineWidth = int.Parse(iniData["style"]["outline_width"]);
            Padding = int.Parse(iniData["style"]["padding"]);
            LineColor = GetRGBColor(iniData["style"]["line_color"]);
            MaxRadius = int.Parse(iniData["style"]["max_radius"]);
            HSep = int.Parse(iniData["style"]["h_sep"]);
            VSep = int.Parse(iniData["style"]["v_sep"]);
            Arrows = bool.Parse(iniData["style"]["arrows"]);
            TitlePos = Enum.Parse<TitlePosition>(iniData["style"]["title_pos"]);
            BulletFillColor = GetRGBColor(iniData["style"]["bullet_fill"]);
            TextColor = GetRGBColor(iniData["style"]["text_color"]);
            Shadow = bool.Parse(iniData["style"]["shadow"]);
            ShadowFillColor = GetRGBAColor(iniData["style"]["text_color"]);
            TitleFont = GetFont(iniData["style"]["title_font"]);
        }

        private void ParseNodeStyle(IniData iniData, NodeStyle ns)
        {
            ns.Pattern = iniData[ns.Name]["pattern"];
            ns.Shape = iniData[ns.Name]["shape"];
            ns.Font = GetFont(iniData[ns.Name]["font"]);
            ns.TextColor = GetRGBColor(iniData[ns.Name]["text_color"]);
        }

        private MyFont GetFont(string fontStr)
        {
            var n = fontStr.Length;
            var strs = fontStr.Substring(1, n - 2).Split();
            return new MyFont(strs[0], Enum.Parse<FontStyle>(strs[2]), double.Parse(strs[1]));
        }

        private Color GetRGBColor(string color)
        {
            var colors = color.Split(',');
            return Color.FromArgb(int.Parse(colors[0].Substring(1)),
                int.Parse(colors[1]),
                int.Parse(string.Join("", colors[2].Reverse().Skip(1).Reverse())));
        }

        private Color GetRGBAColor(string color)
        {
            var colors = color.Split(',');
            return Color.FromArgb(
                int.Parse(string.Join("", colors[3].Reverse().Skip(1).Reverse())),
                int.Parse(colors[0].Substring(1)),
                int.Parse(colors[1]),
                int.Parse(colors[2]));
        }

        public NodeStyle GetNodeStyle(string txt)
        {
            foreach (var ns in NodeStyles)
            {
                if (ns.Match(txt))
                {
                    return ns;
                }
            }
            return DefNodeStyle;
        }
    }
}
