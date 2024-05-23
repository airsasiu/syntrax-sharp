using ExCSS;
using SyntraxCore.Units;
using SyntraxCore.Units.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore
{
    public class Configuration : IUnit
    {
        private Track _track;
        private Dictionary<string, string> _urlMap;
        public Configuration(Track track, Dictionary<string, string> urlMap)
        {
            _track = track;
            _urlMap = urlMap;
        }
        public Configuration(Track track)
        {
            _track = track;
            _urlMap = new Dictionary<string, string>();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitConfiguration(this);
        }

        public Configuration GetConfiguration()
        {
            return this;
        }

        public Track GetTrack()
        {
            return _track;
        }

        public List<IUnit> GetUnits()
        {
            return new List<IUnit>();
        }

        public Dictionary<string, string> GetUrlMap()
        {
            return _urlMap;
        }
    }
}
