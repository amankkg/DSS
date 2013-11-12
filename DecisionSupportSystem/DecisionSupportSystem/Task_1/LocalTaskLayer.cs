using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_1
{
    public static class LocalTaskLayer
    {
        public static ObservableCollection<Combination> CombinationsList = new ObservableCollection<Combination>();
        public static void CreateCombinations(BaseLayer baseLayer)
        {
            LoadCombinations(baseLayer);
            var lastCombinationList = CreateLastCombinationList(baseLayer);
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            var events = baseLayer.DssDbContext.Events.Local.ToList();
            foreach (var act in actions)
                foreach (var ev in events)
                    if (!HaveAction(act, lastCombinationList) || !HaveEvent(ev, lastCombinationList))
                    {
                        baseLayer.BaseMethods.AddCombination(new Combination(), act, ev, baseLayer.Task, 0);
                    }
        }

        public static void LoadCombinations(BaseLayer baseLayer)
        {
            CombinationsList = baseLayer.DssDbContext.Combinations.Local;
        }

        public static List<Combination> CreateLastCombinationList(BaseLayer baseLayer)
        {
            return baseLayer.DssDbContext.Combinations.Local.ToList();
        }

        public static bool HaveAction(Action act, IEnumerable<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Action == act);
        }

        public static bool HaveEvent(Event eEvent, IEnumerable<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Event == eEvent);
        }
    }
}
