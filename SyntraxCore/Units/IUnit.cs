using SyntraxCore.Units.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units
{
    public interface IUnit
    {
        void Accept(IVisitor visitor);

        List<IUnit> GetUnits();

        Configuration GetConfiguration();

        Track GetTrack();
    }
}
