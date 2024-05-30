using SyntraxCore.Elements;
using SyntraxCore.Style;
using SyntraxCore.Units;
using SyntraxCore.Units.Nodes;
using SyntraxCore.Units.Tracks;
using SyntraxCore.Units.Tracks.Loop;
using SyntraxCore.Units.Tracks.Opt;
using SyntraxCore.Units.Tracks.Stack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore
{
    public class SVGCanvasBuilder : IVisitor
    {
        public Dictionary<string, string> UrlMap { get; set; }
        public SVGCanvas Canvas { get; set; }
        public StyleConfig Style { get; set; }
        public string Title { get; set; }
        public int Indent { get; set; }
        public bool Ltor { get; set; }
        public UnitEndPoint EndPoint { get; set; }

        public SVGCanvasBuilder()
        {
            Style = new StyleConfig(1, false);
            UrlMap = new Dictionary<string, string>();
        }

        public SVGCanvasBuilder WithStyle(StyleConfig style)
        {
            Style = style;
            return this;
        }

        public SVGCanvasBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public SVGCanvas GenerateSVG(IUnit root)
        {
            Canvas = new SVGCanvas(Style);
            ParseDiagram(new Line(new List<IUnit>() { new Bullet(), root, new Bullet() }), true);
            if (string.IsNullOrEmpty(Title))
            {
                return Canvas;
            }
            string tag = Canvas.GetCanvasTag();
            (Point, Point) bbox = Canvas.GetBoundingBoxByTag(tag);
            string titleTag = Canvas.NewTag("x", "-title");
            var element = new TitleElement(Title, Style.TitleFont, "title_font", titleTag);
            Canvas.AddElement(element);
            switch (Style.TitlePos)
            {
                case TitlePosition.BottomLeft:
                case TitlePosition.TopLeft:
                    Canvas.MoveElement(titleTag, 2 * Style.Padding, 0);
                    break;
                case TitlePosition.BottomCenter:
                case TitlePosition.TopCenter:
                    Canvas.MoveElement(titleTag, (bbox.Item1.X + bbox.Item2.X - element.End.X) / 2 - 2 * Style.Padding, 0);
                    break;
                case TitlePosition.BottomRight:
                case TitlePosition.TopRight:
                    Canvas.MoveElement(titleTag, bbox.Item2.X - element.End.X - 2 * Style.Padding, 0);
                    break;
            }

            switch (Style.TitlePos)
            {
                case TitlePosition.TopLeft:
                case TitlePosition.TopCenter:
                case TitlePosition.TopRight:
                    Canvas.MoveElement(titleTag, 0, element.End.Y + 2 * Style.Padding);
                    break;
                case TitlePosition.BottomLeft:
                case TitlePosition.BottomCenter:
                case TitlePosition.BottomRight:
                    Canvas.MoveElement(titleTag, 0, bbox.Item2.Y + 2 * Style.Padding);
                    break;
            }
            return Canvas;
        }

        private void ParseDiagram(IUnit unit, bool ltor)
        {
            if (unit == null)
            {
                EndPoint = null;
                return;
            }
            Ltor = ltor;
            unit.Accept(this);
        }


        public void VisitBullet(Bullet unit)
        {
            throw new NotImplementedException();
        }

        public void VisitChoice(Choice unit)
        {
            throw new NotImplementedException();
        }

        public void VisitConfiguration(Configuration unit)
        {
            throw new NotImplementedException();
        }

        public void VisitIndentstack(Indentstack unit)
        {
            throw new NotImplementedException();
        }

        public void VisitLine(Line unit)
        {
            throw new NotImplementedException();
        }

        public void VisitLoop(Loop unit)
        {
            throw new NotImplementedException();
        }

        public void VisitNode(Node unit)
        {
            throw new NotImplementedException();
        }

        public void VisitNoneNode(NoneNode unit)
        {
            throw new NotImplementedException();
        }

        public void VisitOpt(Opt unit)
        {
            throw new NotImplementedException();
        }

        public void VisitOptx(Optx unit)
        {
            throw new NotImplementedException();
        }

        public void VisitRightstack(Rightstack unit)
        {
            throw new NotImplementedException();
        }

        public void VisitStack(MyStack unit)
        {
            throw new NotImplementedException();
        }

        public void VisitToploop(Toploop unit)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitEndPoint
    {
        public string Tag { get; set; }
        public Point EndPoint { get; set; }
        public UnitEndPoint(string tag, Point endPoint)
        {
            Tag = tag;
            EndPoint = endPoint;
        }
    }
}
