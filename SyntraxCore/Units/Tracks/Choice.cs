using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks
{
    public class Choice : ComplexTrack
    {
        public Choice(List<IUnit> units) : base(units)
        {

        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitChoice(this);
        }
    }
}
