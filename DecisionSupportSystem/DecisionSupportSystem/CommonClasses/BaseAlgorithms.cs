using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.CommonClasses
{
    public class BaseAlgorithms
    {
        public DssDbEntities Entities { get; set; }
        public IList<CpMax> CpMaxes { get; set; }
        public Task Task { get; set; }
        public double MaxEmv { get; set; }
        public double MinEol { get; set; }

        public void SolveTask(IEnumerable<Combination> combinations)
        {
            CalculateWp(combinations);
            CalculateCol(combinations);
            CalculateWol(combinations);
            CalculateEmv();
            CalculateEol(combinations);
            InitTaskProperties();
        }

        public void CalculateWp(IEnumerable<Combination> combinations)
        {
            combinations = combinations ?? GetCombinations();
            foreach (var comb in combinations)
                comb.Wp = comb.Cp*comb.Event.Probability;
        }

        public ObservableCollection<Combination> GetCombinations()
        {
            return Entities.Combinations.Local;
        }

        public void CalculateCol(IEnumerable<Combination> combinations)
        {
            FindCpMaxes();
            combinations = combinations ?? GetCombinations();
            foreach (var combination in combinations)
            {
                var cpmax = GetCpMaxByEvent(combination.Event);
                combination.Col = cpmax - combination.Cp;
            }
        }

        private double? GetCpMaxByEvent(Event ev)
        {
            return CpMaxes.First(cpmax => cpmax.Event == ev).Value;
        }

        public void FindCpMaxes()
        {
            CpMaxes = new List<CpMax>();
            var events = GetEvents();
            foreach (var ev in events)
            {
                var cps = GetCpsByEventFromCombinations(ev);
                var cpMax = cps.Max();
                if (cpMax != null)
                    CpMaxes.Add(new CpMax{Event = ev, Value = cpMax});
            }
        }

        public ObservableCollection<Event> GetEvents()
        {
            return Entities.Events.Local;
        }

        private IEnumerable<double?> GetCpsByEventFromCombinations(Event ev)
        {
            var combinations = GetCombinations();
            return (combinations.
                    Where(combination => combination.Event == ev).
                    Select(combination => combination.Cp)).ToList();
        }

        public void CalculateWol(IEnumerable<Combination> combinations)
        {
            combinations = combinations ?? GetCombinations();
            foreach (var comb in combinations)
                comb.Wol = comb.Col * comb.Event.Probability;
        }

        public void CalculateEmv()
        {
            var actions = GetActions();
            foreach (var action in actions)
            {
                var wps = GetWpsByActionFromCombinations(action);
                action.Emv = wps.Sum();
            }
        }

        public ObservableCollection<Action> GetActions()
        {
            return Entities.Actions.Local;
        }

        private IEnumerable<double?> GetWpsByActionFromCombinations(Action action)
        {
            var combinations = GetCombinations();
            return (combinations.
                Where(combin => combin.Action == action).
                Select(combin => combin.Wp)).ToList();
        }

        public void CalculateEol(IEnumerable<Combination> combinations)
        {
            var actions = GetActions();
            foreach (var action in actions)
            {
                var wols = GetWolsByActionFromCombinations(action, combinations);
                action.Eol = wols.Sum();
            }
        }
        
        private IEnumerable<double?> GetWolsByActionFromCombinations(Action action, 
            IEnumerable<Combination> combinations)
        {
            combinations = combinations ?? GetCombinations();
            return (combinations.
                Where(combin => combin.Action == action).
                Select(combin => combin.Wol)).ToList();
        }

        private void InitTaskProperties()
        {
            var actions = GetActions();
            Task.MaxEmv = actions.Max(act => act.Emv);
            Task.MinEol = actions.Min(act => act.Eol);
            var optimalActName = actions.First(act => act.Emv == Task.MaxEmv).Name;
            Task.Date = DateTime.Now;
            Task.Recommendation = string.Format(
                "Рекомендуется выбрать действие '{0}'. Это решение принесет" +
                " максимальное значение средней ожидаемой прибыли равное '{1}' $, " +
                " миниммальное значение средней ожидаемой потери равное '{2}' $. " +
                "Такие значения средних ожидаемых потерь и прибылей ожидаются, " +
                "при многократном выборе этого действие при условии, " +
                "что вероятности событий будут неизменны.",
                optimalActName, Task.MaxEmv, Task.MinEol);
        }

        public void Save()
        {
            Entities.SaveChanges();
        }
    }
}
