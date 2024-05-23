using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Loop
{
    public class Loop : ComplexTrack
    {
        public Loop(List<IUnit> units) : base(units)
        {
            if (units.Count != 2)
                throw new LoopNotTwoArgsException();
        }

        public Track GetForwardPart()
        {
            return GetUnits()[0].GetTrack();
        }

        public Track GetBackwardPart()
        {
            return GetUnits()[1].GetTrack();
        }

        public bool IsForwardNull()
        {
            return GetForwardPart() == null;
        }

        public bool IsBackwardNull()
        {
            return GetBackwardPart() == null;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitLoop(this);
        }
    }
}
