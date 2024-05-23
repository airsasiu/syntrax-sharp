using SyntraxCore.Units.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Nodes
{
    public class Bullet : IUnit
    {
        public Bullet()
        {
            
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitBullet(this);
        }

        public Configuration GetConfiguration()
        {
            throw new NotImplementedException();
        }

        public Track GetTrack()
        {
            return new Line(this);
        }

        public List<IUnit> GetUnits()
        {
            throw new NotImplementedException();
        }
    }
}
