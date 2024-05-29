using SyntraxCore.Style;
using SyntraxCore.Units.Nodes;
using SyntraxCore.Units.Tracks;
using SyntraxCore.Units.Tracks.Loop;
using SyntraxCore.Units.Tracks.Opt;
using SyntraxCore.Units.Tracks.Stack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore
{
    public class SVGCanvasBuilder : IVisitor
    {
        private Dictionary<string, string> _urlMap = new Dictionary<string, string>();
        private StyleConfig _style;
        private SVGCanvas _canvas;

        public SVGCanvasBuilder()
        {
            _style = new StyleConfig(1, false);
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
}
