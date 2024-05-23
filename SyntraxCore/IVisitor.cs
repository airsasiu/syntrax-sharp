using SyntraxCore.Units.Nodes;
using SyntraxCore.Units.Tracks;
using SyntraxCore.Units.Tracks.Loop;
using SyntraxCore.Units.Tracks.Opt;
using SyntraxCore.Units.Tracks.Stack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyntraxCore
{
    public interface IVisitor
    {
        void VisitLine(Line unit);
        void VisitLoop(Loop unit);
        void VisitToploop(Toploop unit);
        void VisitChoice(Choice unit);
        void VisitOpt(Opt unit);
        void VisitOptx(Optx unit);
        void VisitStack(MyStack unit);
        void VisitRightstack(Rightstack unit);
        void VisitIndentstack(Indentstack unit);
        void VisitBullet(Bullet unit);
        void VisitNode(Node unit);
        void VisitNoneNode(NoneNode unit);
        void VisitConfiguration(Configuration unit);
    }
}
