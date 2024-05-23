using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Tracks.Stack
{
    public class Indentstack : MyStack
    {
        private int _indent;
        public Indentstack(int indent, List<IUnit> units) : base(units)
        {
            _indent = indent;
        }

        public int GetIndent()
        {
            return _indent;
        }

        public override string ToString()
        {
            return "< " + GetType().Name
                    + ", indent = " + _indent + " [ "
                    + string.Join("\n", GetUnits().Select(u => u.ToString()))
                    + " ]" + " >";
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitIndentstack(this);
        }
    }
}
