using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Units.Nodes
{
    public class NoneNode : Node
    {
        public NoneNode() : base("")
        {

        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitNoneNode(this);
        }
    }
}
