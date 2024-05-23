using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks
{
    public abstract class Track : IUnit
    {
        private List<IUnit> _units = new List<IUnit>();

        public Track(List<IUnit> units)
        {
            _units = units;
        }

        public virtual void Accept(IVisitor visitor)
        {
            //do nothing;
        }

        public Configuration GetConfiguration()
        {
            throw new NotImplementedException();
        }

        public Track GetTrack()
        {
            return this;
        }

        public List<IUnit> GetUnits()
        {
            return _units;
        }

        public override string ToString()
        {
            return "< " + nameof(Track)
            + "[ " + string.Join("\n", _units.Select(u => u.ToString()))
            + " ]" + " >";
        }
    }
}
