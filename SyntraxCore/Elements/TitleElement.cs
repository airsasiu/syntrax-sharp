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
    public class TitleElement : BubbleElementBase
    {
        public TitleElement(string text, MyFont textFont, string fontName, string tag) :
            base(new Point(0, 0), SVGCanvasBuilder.TextSize(text, textFont), null, text, new Point(0, 0)
                , null, fontName, Color.FromArgb(0, 0, 0), 0, Color.FromArgb(255, 255, 255), tag)
        {

        }

        public override void AddShadow(StringBuilder sb, StyleConfig style)
        {
            //do nothing;
        }

        public override void ToSVG(StringBuilder sb, StyleConfig style)
        {
            // Add text
            AddXmlText(sb, style);
        }

        public override int GetX(StyleConfig style)
        {
            int x0 = Start.X;
            int x1 = End.Y;
            int x = (x0 + x1) / 2;
            switch (style.TitlePos)
            {
                case TitlePosition.BottomLeft:
                case TitlePosition.TopLeft:
                    x = x0;
                    break;
                case TitlePosition.BottomCenter:
                case TitlePosition.TopCenter:
                    x = (x0 + x1) / 2;
                    break;
                case TitlePosition.BottomRight:
                case TitlePosition.TopRight:
                    x = x1;
                    break;
            }
            return x;
        }
    }
}
