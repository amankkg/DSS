using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionSupportSystem.SolvingTree
{
    public class SolvingLayer
    {
        public List<DLine> DLines = new List<DLine>();
        public List<Event> Events = new List<Event>();
        public List<Action> Actions = new List<Action>();
        public List<EventOrigin> EventOrigins = new List<EventOrigin>(); 
    }
}
