using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Loop
{
    public class Toploop : Loop
    {
        public Toploop(List<IUnit> units) : base(units)
        {
        }


        public override void Accept(IVisitor visitor)
        {
            visitor.VisitToploop(this);
        }
    }
}
