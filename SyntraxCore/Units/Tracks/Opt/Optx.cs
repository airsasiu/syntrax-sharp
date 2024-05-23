using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Opt
{
    public class Optx : Opt
    {
        public Optx(List<IUnit> units) : base(units)
        {

        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitOptx(this);
        }
    }
}
