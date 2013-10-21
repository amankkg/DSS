using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiceGameClassesLibrary;

namespace DecisionSupportSystem.Interfaces.Task_no._7
{
    public class TaskModel
    {
        public static string[] InitialEvents;
        public static Dice game;
        public static List<Event> events;
        public static List<Action> actions;
        public static List<Combination> combinations;

        public void GenerateEvents()
        {
            events = new List<Event>();
            string name;
            decimal probability = 1/game.Outcomes.Count;
            foreach (var item in game.Outcomes)
            {
                Event ev = new Event();
                name = "";
                foreach (var eventNumber in item.Events)
                {
                    name += InitialEvents[eventNumber] + "-";
                }
                name = name.Remove(name.Length - 1);
                ev.Name = name;
                ev.Probability = probability;
                events.Add(ev);
            }
        }

        public void GenerateActions()
        {
            actions = new List<Action>();
            string name;
            foreach (var stake in game.Stakes)
            {
                Action act = new Action();
                name = "";
                foreach (var item in stake)
                {
                    name += InitialEvents[item] + "-";
                }
                name = name.Remove(name.Length - 1);
                act.Name = name;
                actions.Add(act);
            }
        }
    }
}
