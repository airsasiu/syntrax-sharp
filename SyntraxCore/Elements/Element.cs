using SyntraxCore.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Elements
{
    public class Element
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        private HashSet<string> tags = new HashSet<string>();
        public Element(string tag)
        {
            tags.Add(tag);
        }

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        public void DelTag(string tag)
        {
            tags.Remove(tag);
        }

        public bool IsTagged(string tag)
        {
            if (tag == "all")
            {
                return true;
            }
            return tags.Contains(tag);
        }

        public virtual void AddShadow(StringBuilder sb, StyleConfig style) { }

        public virtual void ToSVG(StringBuilder sb, StyleConfig style) { }

        public virtual void Scale(double scale)
        {
            Start = new Point((int)(Start.X * scale), (int)(Start.Y * scale));
            End = new Point((int)(End.X * scale), (int)(End.Y * scale));
        }

        public string GetAnyTag()
        {
            return tags.FirstOrDefault();
        }
    }
}
