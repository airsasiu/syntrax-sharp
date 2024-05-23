

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

        public StyleConfig()
        {

        }

        public float GetScale()
        {
            return 1.0f;
        }

        internal List<NodeStyle> GetNodeStyles()
        {
            throw new NotImplementedException();
        }

        internal int GetPadding()
        {
            return 5;
        }

        internal Color GetTextColor()
        {
            throw new NotImplementedException();
        }

        internal string GetTitleFont()
        {
            throw new NotImplementedException();
        }
    }
}
