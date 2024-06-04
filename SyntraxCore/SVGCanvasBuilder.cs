using SyntraxCore.Elements;
using SyntraxCore.Style;
using SyntraxCore.Units;
using SyntraxCore.Units.Nodes;
using SyntraxCore.Units.Tracks;
using SyntraxCore.Units.Tracks.Loop;
using SyntraxCore.Units.Tracks.Opt;
using SyntraxCore.Units.Tracks.Stack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        private UnitEndPoint GetDiagramParseResult(IUnit unit, bool ltor)
        {
            ParseDiagram(unit, ltor);
            return EndPoint;
        }


        public void VisitBullet(Bullet unit)
        {
            string tag = Canvas.NewTag("x", "");
            int w = Style.OutLineWidth;
            int r = w + 1;
            Canvas.AddElement(
                new OvalElement(new Point(0, -r), new Point(2 * r, r),
                w, Style.BulletFillColor, tag));

            EndPoint = new UnitEndPoint(tag, new Point(2 * r, 0));
        }

        public void VisitChoice(Choice choice)
        {
            string tag = Canvas.NewTag("x", "");

            var sep = Style.VSep;
            var vsep = sep / 2;

            var n = choice.GetUnits().Count;

            if (n == 0)
            {
                EndPoint = null;
                return;
            }
            List<UnitEndPoint> res = new List<UnitEndPoint>();
            var mxw = 0;

            for (int i = 0; i < n; ++i)
            {
                res.Add(GetDiagramParseResult(choice.GetUnits()[i], Ltor));

                var box = Canvas.GetBoundingBoxByTag(res[i].Tag);
                var w = box.Item2.X - box.Item1.X;
                if (i != 0)
                {
                    w += 20;
                }
                mxw = Math.Max(mxw, w);
            }

            var x2 = sep * 2;
            var x3 = mxw + x2;
            var x4 = x3 + sep;
            var x5 = x4 + sep;

            var exy = 0;
            var btm = 0;

            for (int i = 0; i < n; ++i)
            {
                var t = res[i].Tag;
                var tepX = res[i].EndPoint.X;
                var tepY = res[i].EndPoint.Y;
                var box = Canvas.GetBoundingBoxByTag(t);
                var w = box.Item2.X - box.Item1.X;
                var dx = (mxw - w) / 2 + x2;
                if (w > 10 && dx > x2 + 10)
                {
                    dx = x2 + 10;
                }
                Canvas.MoveElement(t, dx, 0);
                tepX += dx;
                box = Canvas.GetBoundingBoxByTag(t);
                int ty0 = box.Item1.Y;
                int ty1 = box.Item2.Y;

                if (i == 0)
                {
                    Canvas.AddElement(
                            new LineElement(new Point(0, 0), new Point(dx, 0),
                                    Ltor && dx > x2 ? "last" : string.Empty, Style.LineWidth, tag));
                    Canvas.AddElement(
                            new LineElement(new Point(tepX, tepY), new Point(x5 + 1, tepY),
                                    Ltor ? string.Empty : "first", Style.LineWidth, tag));
                    exy = tepY;
                    Canvas.AddElement(
                            new ArcElement(new Point(-sep, 0), new Point(sep, sep * 2),
                                    Style.LineWidth, 90, -90, tag));
                    btm = ty1;
                }
                else
                {
                    int dy = Math.Max(btm - ty0 + vsep, 2 * sep);
                    Canvas.MoveElement(t, 0, dy);
                    tepY += dy;
                    if (dx > x2)
                    {
                        Canvas.AddElement(
                                new LineElement(new Point(x2, dy), new Point(dx, dy),
                                        Ltor ? "last" : string.Empty, Style.LineWidth, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(tepX, tepY), new Point(x3, tepY),
                                        Ltor ? string.Empty : "first", Style.LineWidth, tag));
                    }
                    int y1 = dy - 2 * sep;
                    Canvas.AddElement(
                            new ArcElement(new Point(sep, y1), new Point(sep + 2 * sep, dy),
                                    Style.LineWidth, 180, 90, tag));
                    int y2 = tepY - 2 * sep;
                    Canvas.AddElement(
                            new ArcElement(new Point(x3 - sep, y2), new Point(x4, tepY),
                                    Style.LineWidth, 270, 90, tag));
                    if (i + 1 == n)
                    {
                        Canvas.AddElement(
                                new ArcElement(new Point(x4, exy), new Point(x4 + 2 * sep, exy + 2 * sep),
                                        Style.LineWidth, 180, -90, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(sep, dy - sep), new Point(sep, sep),
                                        null, Style.LineWidth, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(x4, tepY - sep), new Point(x4, exy + sep),
                                        null, Style.LineWidth, tag));
                    }
                    btm = ty1 + dy;
                }

                // retag
                Canvas.AddElementTag(tag, t);
                Canvas.DropElementTag(t);
            }

            EndPoint = new UnitEndPoint(tag, new Point(x5, exy));
        }

        public void VisitConfiguration(Configuration unit)
        {
            UrlMap = unit.GetUrlMap();
            unit.GetTrack().Accept(this);
        }

        public void VisitIndentstack(Indentstack unit)
        {
            var sep = Style.HSep * unit.GetIndent();
            Indent = sep;
            ParseStack(unit);
        }

        public void VisitLine(Line line)
        {
            string tag = Canvas.NewTag("x", "");

            var sep = Style.HSep;
            var width = Style.LineWidth;

            Point pos = new Point(0, 0);
            var unitNum = 0;
            var unitStep = 1;
            var count = line.GetUnits().Count;

            if (!Ltor)
            {
                unitNum = count - 1;
                unitStep = -1;
            }

            for (; 0 <= unitNum && unitNum < count; unitNum += unitStep)
            {
                IUnit unit = line.GetUnits()[unitNum];
                var endPoint = GetDiagramParseResult(unit, Ltor);
                if (endPoint == null)
                {
                    continue;
                }
                if (pos.X != 0)
                {
                    int xn = pos.X + sep;
                    Canvas.MoveElement(endPoint.Tag, xn, pos.Y);
                    var l = new LineElement(new Point(pos.X - 1, pos.Y), new Point(xn, pos.Y),
                        Ltor ? "last" : "first", width, tag);
                    Canvas.AddElement(l);
                    pos = new Point(xn + endPoint.EndPoint.X, endPoint.EndPoint.Y);
                }
                else
                {
                    pos = new Point(endPoint.EndPoint.X, endPoint.EndPoint.Y);
                }
                Canvas.AddElementTag(tag, endPoint.Tag);
                Canvas.DropElementTag(endPoint.Tag);

                if (pos.X == 0)
                {
                    Canvas.AddElement(
                        new LineElement(new Point(0, 0), new Point(sep, 0),
                        null, width, tag));

                    Canvas.AddElement(
                        new LineElement(new Point(sep, 0), new Point(sep * 2, 0),
                        "last", width, tag));

                    pos.Offset(sep, 0);
                }

                EndPoint = new UnitEndPoint(tag, pos);
            }
        }

        public void VisitLoop(Loop loop)
        {
            string tag = Canvas.NewTag("x", "");
            int sep = Style.VSep;
            int vsep = sep / 2;

            // parse forward
            UnitEndPoint fEndPoint = GetDiagramParseResult(loop.GetForwardPart(), Ltor);
            string ft = fEndPoint.Tag;
            int fexx = fEndPoint.EndPoint.X;
            int fexy = fEndPoint.EndPoint.Y;
            var fBox = Canvas.GetBoundingBoxByTag(ft);
            int fx0 = fBox.Item1.X;
            int fx1 = fBox.Item2.X;
            int fy1 = fBox.Item2.Y;

            // parse backward
            UnitEndPoint bEndPoint = GetDiagramParseResult(loop.GetBackwardPart(), !Ltor);
            string bt = bEndPoint.Tag;
            int bexx = bEndPoint.EndPoint.X;
            int bexy = bEndPoint.EndPoint.Y;
            var bBox = Canvas.GetBoundingBoxByTag(bt);
            int bx0 = bBox.Item1.X;
            int by0 = bBox.Item1.Y;
            int bx1 = bBox.Item2.X;

            // move processes
            int fw = fx1 - fx0;
            int bw = bx1 - bx0;
            int dy = fy1 - by0 + vsep;
            Canvas.MoveElement(bt, 0, dy);
            bexy += dy;
            int mxx;

            if (fw > bw)
            {
                int dx;
                if (fexx < fw && fexx >= bw)
                {
                    dx = (fexx - bw) / 2;
                    Canvas.MoveElement(bt, dx, 0);
                    bexx += dx;
                    Canvas.AddElement(
                            new LineElement(new Point(0, dy), new Point(dx, dy),
                                    string.Empty, Style.LineWidth, bt));
                    Canvas.AddElement(
                            new LineElement(new Point(bexx, bexy), new Point(fexx, bexy),
                                    "first", Style.LineWidth, bt));
                }
                else
                {
                    dx = (fw - bw) / 2;
                    Canvas.MoveElement(bt, dx, 0);
                    bexx += dx;

                    Canvas.AddElement(
                            new LineElement(new Point(0, dy), new Point(dx, dy),
                                    Ltor || dx < 2 * vsep ? string.Empty : "last", Style.LineWidth, bt));
                    Canvas.AddElement(
                            new LineElement(new Point(bexx, bexy), new Point(fx1, bexy),
                                    !Ltor || dx < 2 * vsep ? string.Empty : "first", Style.LineWidth, bt));
                }
                mxx = fexx;
            }
            else if (bw > fw)
            {
                int dx = (bw - fw) / 2;
                Canvas.MoveElement(ft, dx, 0);
                fexx += dx;
                Canvas.AddElement(
                        new LineElement(new Point(0, 0), new Point(dx, fexy),
                                Ltor ? "last" : "first", Style.LineWidth, ft));
                Canvas.AddElement(
                        new LineElement(new Point(fexx, fexy), new Point(bx1, fexy),
                                null, Style.LineWidth, ft));
                mxx = bexx;
            }
            else
            {
                mxx = fexx;
            }

            // retag
            Canvas.AddElementTag(tag, bt);
            Canvas.AddElementTag(tag, ft);
            Canvas.DropElementTag(bt);
            Canvas.DropElementTag(ft);

            // move for left turnback
            Canvas.MoveElement(tag, sep, 0);

            mxx += sep;
            Canvas.AddElement(
                    new LineElement(new Point(0, 0), new Point(sep, 0),
                            null, Style.LineWidth, tag));

            DrawLeftTurnBack(tag, sep, 0, dy, Ltor ? "up" : "down");
            DrawRightTurnBack(tag, mxx, fexy, bexy, Ltor ? "down" : "up");

            int exitX = mxx + Style.MaxRadius;
            Canvas.AddElement(
                    new LineElement(new Point(mxx, fexy), new Point(exitX, fexy),
                            null, Style.LineWidth, tag));

            EndPoint = new UnitEndPoint(tag, new Point(exitX, fexy));
        }

        public void VisitNode(Node unit)
        {
            var txt = unit.ToString();
            var ns = Style.GetNodeStyle(txt);
            txt = ns.UnwrapTextContent(txt);

            var font = ns.Font;
            var fontName = ns.Name + "_font";
            var fill = ns.FillColor;
            var textColor = ns.TextColor;

            var textSize = GetTextSize(txt, font);
            var x0 = -textSize.X / 2;
            var y0 = -textSize.Y / 2;
            var x1 = x0 + textSize.X;
            var y1 = y0 + textSize.Y;

            int h = y1 - y0 + 1;
            int rad = (h + 1) / 2;

            int lft = x0;
            int rgt = x1;
            int top = y1 - 2 * rad;
            if (ns.Shape == "bubble" || ns.Shape == "hex")
            {
                lft += rad / 2 - 2;
                rgt -= rad / 2 - 2;
            }
            else
            {
                lft -= 5;
                rgt += 5;
            }

            if (lft > rgt)
            {
                lft = (x0 + x1) / 2;
                rgt = lft;
            }

            var tag = Canvas.NewTag("x", "-box");
            var href = UrlMap[txt];
            Point start = new Point(lft - rad, top);
            Point end = new Point(rgt + rad, y1);
            BubbleElementBase b;
            switch (ns.Shape)
            {
                case "bubble":
                    b = new BubbleElement(start, end, href,
                        txt, new Point(x0, y0), font, fontName, textColor, Style.OutLineWidth,
                        fill, tag);
                    break;
                case "hex":
                    b = new HexBubbleElement(start, end, href,
                        txt, new Point(x0, y0), font, fontName, textColor, Style.OutLineWidth,
                        fill, tag);
                    break;
                default:
                    start = new Point(lft, top);
                    end = new Point(rgt, y1);
                    b = new BoxBubbleElement(start, end, href,
                        txt, new Point(x0, y0), font, fontName, textColor, Style.OutLineWidth,
                        fill, tag);
                    break;
            }
            Canvas.AddElement(b);

            x0 = start.X;
            x1 = end.X;
            var width = x1 - x0;
            Canvas.MoveElement(tag, -x0, 2);
            EndPoint = new UnitEndPoint(tag, new Point(width, 0));
        }

        private Point GetTextSize(string text, MyFont font)
        {

            // todo: may need measure font size;
            return new Point(10 * text.Length, 10);
        }

        public void VisitNoneNode(NoneNode unit)
        {
            string tag = Canvas.NewTag("x", "");
            var e = new LineElement(new Point(0, 0), new Point(1, 0), null, Style.OutLineWidth, tag);
            Canvas.AddElement(e);
            EndPoint = new UnitEndPoint(tag, e.End);
        }

        public void VisitOpt(Opt opt)
        {
            Choice c = new Choice(new List<IUnit>() {
                new NoneNode(),
                new Line(opt.GetUnits()),
            });

            ParseDiagram(c, Ltor);
        }

        public void VisitOptx(Optx opt)
        {
            Choice c = new Choice(new List<IUnit>() {
                new Line(opt.GetUnits()),
                new NoneNode(),
            });

            ParseDiagram(c, Ltor);
        }

        public void VisitRightstack(Rightstack unit)
        {
            Indent = -1;
            ParseStack(unit);
        }

        public void VisitStack(MyStack stack)
        {
            Indent = 0;
            ParseStack(stack);
        }

        private void ParseStack(MyStack stack)
        {
            var tag = Canvas.NewTag("x", "");

            var sep = Style.VSep * 2;
            var btm = 0;
            var n = stack.GetUnits().Count;
            if (n == 0)
            {
                EndPoint = null;
                return;
            }
            var nextBypassY = 0;
            var bypassX = 0;
            var bypassY = 0;
            var exitX = 0;
            var exitY = 0;
            var w = 0;
            var e2 = 0;
            var e3 = 0;
            var ex2 = 0;
            var enterX = 0;
            var enterY = 0;
            var backY = 0;
            var midY = 0;
            var bypass = 0;

            for (int i = 0; i < n; ++i)
            {
                var unit = stack.GetUnits()[i];
                IUnit term;
                bypassY = nextBypassY;
                if (i != 0 && Indent >= 0 && unit.GetUnits().Count != 0 && unit is Opt)
                {
                    bypass = 1;
                    term = new Line(unit.GetUnits());
                }
                else
                {
                    bypass = 0;
                    term = unit;
                    nextBypassY = 0;
                }

                UnitEndPoint ep = GetDiagramParseResult(term, Ltor);
                var t = ep.Tag;
                int epX = ep.EndPoint.X;
                int epY = ep.EndPoint.Y;
                var box = Canvas.GetBoundingBoxByTag(t);
                int tx0 = box.Item1.X;
                int ty0 = box.Item1.Y;
                int tx1 = box.Item2.X;

                if (i == 0)
                {
                    exitX = epX;
                    exitY = epY;
                }
                else
                {
                    enterY = btm - ty0 + sep * 2 + 2;
                    if (bypass == 1)
                    {
                        nextBypassY = enterY - Style.MaxRadius;
                    }
                    if (Indent < 0)
                    {
                        w = tx1 - tx0;
                        enterX = exitX - w + sep * Indent;
                        ex2 = sep * 2 - Indent;
                        if (ex2 > enterX)
                        {
                            enterX = ex2;
                        }
                    }
                    else
                    {
                        enterX = sep * 2 + Indent;
                    }
                    backY = btm + sep + 1;

                    if (bypassY > 0)
                    {
                        midY = (bypassY + Style.MaxRadius + backY) / 2;
                        Canvas.AddElement(
                                new LineElement(new Point(bypassX, bypassY), new Point(bypassX, midY),
                                        "last", Style.LineWidth, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(bypassX, midY),
                                        new Point(bypassX, backY + Style.MaxRadius),
                                        string.Empty, Style.LineWidth, tag));
                    }

                    Canvas.MoveElement(t, enterX, enterY);
                    e2 = exitX + sep;
                    Canvas.AddElement(
                            new LineElement(new Point(exitX, exitY), new Point(e2, exitY),
                                    string.Empty, Style.LineWidth, tag));
                    DrawRightTurnBack(tag, e2, exitY, backY, "down");
                    e3 = enterX - sep;
                    bypassX = e3 - Style.MaxRadius;
                    int emid = (e2 + e3) / 2;
                    Canvas.AddElement(
                            new LineElement(new Point(e2, backY), new Point(emid, backY),
                                    "last", Style.LineWidth, tag));
                    Canvas.AddElement(
                            new LineElement(new Point(emid, backY), new Point(e3, backY),
                                    string.Empty, Style.LineWidth, tag));
                    DrawLeftTurnBack(tag, e3, backY, enterY, "down");
                    Canvas.AddElement(
                            new LineElement(new Point(e3, enterY), new Point(enterX, enterY),
                                    "last", Style.LineWidth, tag));
                    exitX = enterX + epX;
                    exitY = enterY + epY;
                }

                // retag
                Canvas.AddElementTag(tag, t);
                Canvas.DropElementTag(t);
                btm = Canvas.GetBoundingBoxByTag(tag).Item2.Y;
            }

            if (bypass == 1)
            {
                int fwdY = btm + sep + 1;
                midY = (nextBypassY + Style.MaxRadius + fwdY) / 2;
                int descenderX = exitX + Style.MaxRadius;
                Canvas.AddElement(
                        new LineElement(new Point(bypassX, nextBypassY), new Point(bypassX, midY),
                                "last", Style.LineWidth, tag));
                Canvas.AddElement(
                        new LineElement(new Point(bypassX, midY),
                                new Point(bypassX, fwdY - Style.MaxRadius),
                                string.Empty, Style.LineWidth, tag));
                Canvas.AddElement(
                        new ArcElement(new Point(bypassX, fwdY - 2 * Style.MaxRadius),
                                new Point(bypassX + 2 * Style.MaxRadius, fwdY),
                                Style.LineWidth, 180, 90, tag));
                Canvas.AddElement(
                        new ArcElement(new Point(exitX - Style.MaxRadius, exitY),
                                new Point(descenderX, exitY + 2 * Style.MaxRadius),
                                Style.LineWidth, 90, -90, tag));
                Canvas.AddElement(
                        new ArcElement(new Point(descenderX, fwdY - 2 * Style.MaxRadius),
                                new Point(descenderX + 2 * Style.MaxRadius, fwdY),
                                Style.LineWidth, 180, 90, tag));
                exitX += 2 * Style.MaxRadius;
                int halfX = (exitX + Indent) / 2;
                Canvas.AddElement(
                        new LineElement(new Point(bypassX + Style.MaxRadius, fwdY),
                                new Point(halfX, fwdY),
                                "last", Style.LineWidth, tag));
                Canvas.AddElement(
                        new LineElement(new Point(halfX, fwdY), new Point(exitX, fwdY),
                                string.Empty, Style.LineWidth, tag));
                Canvas.AddElement(
                        new LineElement(new Point(descenderX, exitY + Style.MaxRadius),
                                new Point(descenderX, fwdY - Style.MaxRadius),
                                "last", Style.LineWidth, tag));
                exitY = fwdY;
            }

            EndPoint = new UnitEndPoint(tag, new Point(exitX, exitY));
        }

        public void VisitToploop(Toploop loop)
        {
            var tag = Canvas.NewTag("x", "");
            var sep = Style.VSep;
            int vsep = sep / 2;
            var fowardEndPoint = GetDiagramParseResult(loop.GetForwardPart(), Ltor);
            var ftag = fowardEndPoint.Tag;
            var fepX = fowardEndPoint.EndPoint.X;
            var fepY = fowardEndPoint.EndPoint.Y;
            var fBox = Canvas.GetBoundingBoxByTag(ftag);
            int fx0 = fBox.Item1.X;
            int fy0 = fBox.Item1.Y;
            int fx1 = fBox.Item2.X;

            var backwardEndPoint = GetDiagramParseResult(loop.GetBackwardPart(), Ltor);
            var btag = backwardEndPoint.Tag;
            var bepX = backwardEndPoint.EndPoint.X;
            var bepY = backwardEndPoint.EndPoint.Y;
            var bBox = Canvas.GetBoundingBoxByTag(btag);
            var bx0 = bBox.Item1.X;
            var bx1 = bBox.Item2.X;
            var by1 = bBox.Item2.Y;

            var fw = fx1 - fx0;
            var bw = bx1 - bx0;
            var dy = -(by1 - fy0 + vsep);
            Canvas.MoveElement(btag, 0, dy);
            bepY += dy;

            int mxx;
            var dx = Math.Abs(fw - bw) / 2;
            if (fw > bw)
            {
                Canvas.MoveElement(btag, dx, 0);
                bepX += dx;
                Canvas.AddElement(
                    new LineElement(new Point(0, dy), new Point(dx, dy),
                    string.Empty, Style.LineWidth, btag));
                Canvas.AddElement(
                    new LineElement(new Point(bepX, bepY), new Point(fx1, bepY),
                    Ltor || dx < 2 * vsep ? string.Empty : "first", Style.LineWidth, btag));
                mxx = fepX;
            }
            else if (bw > fw)
            {
                Canvas.MoveElement(ftag, dx, 0);
                fepX += dx;
                Canvas.AddElement(
                    new LineElement(new Point(0, 0), new Point(dx, fepY),
                    string.Empty, Style.LineWidth, ftag));
                Canvas.AddElement(
                    new LineElement(new Point(fepX, fepY), new Point(bx1, fepY),
                    string.Empty, Style.LineWidth, ftag));
                mxx = bepX;
            }
            else
            {
                mxx = fepX;
            }

            Canvas.AddElementTag(tag, btag);
            Canvas.AddElementTag(tag, ftag);
            Canvas.DropElementTag(btag);
            Canvas.DropElementTag(ftag);

            Canvas.MoveElement(tag, sep, 0);
            mxx += sep;
            Canvas.AddElement(
                new LineElement(new Point(0, 0), new Point(sep, 0),
                string.Empty, Style.LineWidth, tag));

            DrawRightTurnBack(tag, sep, 0, dy, Ltor ? "up" : "down");
            DrawLeftTurnBack(tag, mxx, fepY, bepY, Ltor ? "down" : "up");

            var box = Canvas.GetBoundingBoxByTag(tag);
            var x1 = box.Item2.X;

            Canvas.AddElement(
                new LineElement(new Point(mxx, fepY), new Point(x1, fepY),
                string.Empty, Style.LineWidth, tag));

            EndPoint = new UnitEndPoint(tag, new Point(x1, fepY));
        }

        private void DrawRightTurnBack(string tag, int x, int yy0, int yy1, string flow)
        {
            var y0 = Math.Min(yy0, yy1);
            var y1 = Math.Max(yy0, yy1);

            if (y1 - y0 > 3 * Style.MaxRadius)
            {
                var xr0 = x - Style.MaxRadius;
                var xr1 = x + Style.MaxRadius;
                Canvas.AddElement(
                    new ArcElement(new Point(xr0, y0), new Point(xr1, y0 + 2 * Style.MaxRadius),
                    Style.LineWidth, 90, -90, tag));

                var yr0 = y0 + Style.MaxRadius;
                var yr1 = y1 + Style.MaxRadius;

                if (Math.Abs(yr1 - yr0) > 2 * Style.MaxRadius)
                {
                    var halfY = (yr1 + yr0) / 2;
                    if (flow == "down")
                    {
                        Canvas.AddElement(
                            new LineElement(new Point(xr1, yr0), new Point(xr1, halfY),
                            "last", Style.LineWidth, tag));
                        Canvas.AddElement(
                            new LineElement(new Point(xr1, halfY), new Point(xr1, yr1),
                            string.Empty, Style.LineWidth, tag));
                    }
                    else
                    {
                        Canvas.AddElement(
                            new LineElement(new Point(xr1, yr1), new Point(xr1, halfY),
                            "last", Style.LineWidth, tag));
                        Canvas.AddElement(
                            new LineElement(new Point(xr1, halfY), new Point(xr1, yr0),
                            string.Empty, Style.LineWidth, tag));
                    }
                }
                else
                {
                    Canvas.AddElement(
                        new LineElement(new Point(xr1, yr0), new Point(xr1, yr1),
                        string.Empty, Style.LineWidth, tag));
                }
                Canvas.AddElement(
                    new ArcElement(new Point(xr0, y1 - 2 * Style.MaxRadius), new Point(xr1, y1),
                    Style.LineWidth, 0, -90, tag));
            }
            else
            {
                var r = (y1 - y0) / 2;
                var x0 = x - r;
                var x1 = x + r;
                Canvas.AddElement(
                    new ArcElement(new Point(x0, y0), new Point(x1, y1),
                    Style.LineWidth, 90, -180, tag));
            }
        }

        private void DrawLeftTurnBack(string tag, int x, int yy0, int yy1, string flow)
        {
            int y0 = Math.Min(yy0, yy1);
            int y1 = Math.Max(yy0, yy1);

            if (y1 - y0 > 3 * Style.MaxRadius)
            {
                int xr0 = x - Style.MaxRadius;
                int xr1 = x + Style.MaxRadius;
                Canvas.AddElement(
                        new ArcElement(new Point(xr0, y0), new Point(xr1, y0 + 2 * Style.MaxRadius),
                                Style.LineWidth, 90, 90, tag));
                int yr0 = y0 + Style.MaxRadius;
                int yr1 = y1 - Style.MaxRadius;
                if (Math.Abs(yr1 - yr0) > 2 * Style.MaxRadius)
                {
                    int halfY = (yr0 + yr1) / 2;
                    if (flow == "down")
                    {
                        Canvas.AddElement(
                                new LineElement(new Point(xr0, yr0), new Point(xr0, halfY),
                                        "last", Style.LineWidth, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(xr0, halfY), new Point(xr0, yr1),
                                        string.Empty, Style.LineWidth, tag));
                    }
                    else
                    {
                        Canvas.AddElement(
                                new LineElement(new Point(xr0, yr1), new Point(xr0, halfY),
                                        "last", Style.LineWidth, tag));
                        Canvas.AddElement(
                                new LineElement(new Point(xr0, halfY), new Point(xr0, yr0),
                                        string.Empty, Style.LineWidth, tag));
                    }
                }
                else
                {
                    Canvas.AddElement(
                            new LineElement(new Point(xr0, yr0), new Point(xr0, yr1),
                                    string.Empty, Style.LineWidth, tag));
                }

                Canvas.AddElement(
                        new ArcElement(new Point(xr0, y1 - 2 * Style.MaxRadius), new Point(xr1, y1),
                                Style.LineWidth, 180, 90, tag));
            }
            else
            {
                int r = (y1 - y0) / 2;
                int x0 = x - r;
                int x1 = x + r;
                Canvas.AddElement(
                        new ArcElement(new Point(x0, y0), new Point(x1, y1),
                                Style.LineWidth, 90, 180, tag));
            }
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
