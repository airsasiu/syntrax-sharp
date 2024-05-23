using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxConsole
{
    public class InputArguments
    {
        public bool Help { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Style { get; set; }
        public string Title { get; set; }
        public Version Version { get; set; }
        public bool Transparent { get; set; }
        public double Scale { get; set; } = 1.0;
        public bool IsDefaultStyle { get; set; } = false;

        public InputArguments(string[] args)
        {
            
        }
    }
}
