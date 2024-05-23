using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Opt
{
    public class Opt : ComplexTrack
    {
        public Opt(List<IUnit> units) : base(units)
        {

        }

        public override string ToString()
        {
            return "< " + GetType().Name
            + "[ " + string.Join("\n", GetUnits().Select(u => u.ToString()))
            + " ]" + " >";
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitOpt(this);
        }
    }
}
