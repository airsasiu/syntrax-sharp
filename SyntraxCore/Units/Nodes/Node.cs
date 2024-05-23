using SyntraxCore.Style;
using SyntraxCore.Units.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Nodes
{
    public class Node : IUnit
    {
        public StyleConfig Style{ get; set; }
        public string Text { get; set; }
        public bool IsLink { get; set; }

        public Node(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }

        public virtual void Accept(IVisitor visitor)
        {
            visitor.VisitNode(this);
        }

        public virtual List<IUnit> GetUnits()
        {
            throw new NotImplementedException();
        }

        public virtual Configuration GetConfiguration()
        {
            throw new NotImplementedException();
        }

        public Track GetTrack()
        {
            return new Line(this);
        }

    }
}
