using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Stack
{
    public class MyStack : ComplexTrack
    {
        public MyStack(List<IUnit> units) : base(units)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitStack(this);
        }
    }
}
