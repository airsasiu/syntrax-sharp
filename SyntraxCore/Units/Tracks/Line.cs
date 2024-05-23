using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks
{
    public class Line : Track
    {
        public Line(List<IUnit> units) : base(units)
        {

        }

        public Line(IUnit unit) : base(new List<IUnit> { unit })
        {

        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitLine(this);
        }
    }
}
