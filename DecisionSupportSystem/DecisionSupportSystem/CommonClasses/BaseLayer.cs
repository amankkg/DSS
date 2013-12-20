using System;
using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;
using Task = DecisionSupportSystem.DbModel.Task;

namespace DecisionSupportSystem.CommonClasses
{
    public class BaseLayer
    {
        #region Свойства
        public DssDbEntities DssDbContext { get; set; }

        public Task Task { get; set; }
        public SolvedTaskView SolvedTaskView { get; set; }
        public BaseMethods BaseMethods { get; set; }
        public IList<CpMax> CpMaxes { get; set; }
        #endregion

        public BaseLayer()
        {
            DssDbContext = new DssDbEntities();
            BaseMethods = new BaseMethods(DssDbContext);
            Task = new Task();
            SolvedTaskView = new SolvedTaskView();
            CpMaxes = new List<CpMax>();
        }

        private void FindCpMaxes()
        {
            CpMaxes.Clear();
            var events = DssDbContext.Events.Local.ToList();
            foreach (var eEvent in events)
            {
                var currentCPsByEvent = GetCurrentCPsByEventFromCombinations(eEvent);
                var max = currentCPsByEvent.Max();
                if (max != null)
                    CpMaxes.Add( new CpMax {Event = eEvent, Value = (decimal)max,});
            }
        }
        
        private IEnumerable<decimal?> GetCurrentCPsByEventFromCombinations(Event eEvent)
        {
            var combins = DssDbContext.Combinations.Local.ToList();
            return (combins.Where(combination => combination.Event == eEvent)
                           .Select(combination => combination.Cp)).ToList();
        }

        public void SolveThisTask(List<Combination> fictiveCombinations)
        {
            SolveWp();
            SolveColWol(fictiveCombinations);
            SolveEmv();
            SolveEol(fictiveCombinations);
        }
        
        private void SolveWp()
        {
            var combins = DssDbContext.Combinations.Local.ToList();
            foreach (var combination in combins)
                combination.Wp = combination.Cp*combination.Event.Probability;
        }

        private void SolveColWol(List<Combination> fictiveCombinations)
        {
            FindCpMaxes();
            List<Combination> combins;
            if (fictiveCombinations != null)
                combins = fictiveCombinations;
            else
                combins = DssDbContext.Combinations.Local.ToList();
            SolveCol(combins);
            SolveWol(combins);
        }
        
        private void SolveCol(IEnumerable<Combination> combinations)
        {
            foreach (var combination in combinations)
            {
                var cpMaxByEvent = GetCpMaxByEvent(combination.Event);
                combination.Col = cpMaxByEvent - combination.Cp;
            }
        }

        private decimal? GetCpMaxByEvent(Event eEvent)
        {
            return CpMaxes.First(i => i.Event == eEvent).Value; 
        }

        private void SolveWol(IEnumerable<Combination> combinations)
        {
            foreach (var combination in combinations)
                combination.Wol = combination.Col*combination.Event.Probability;
        }

        public void SolveEmv()
        {
            var actions = DssDbContext.Actions.Local.ToList();
            foreach (var action in actions)
            {
                var wps = GetWpsByActionFromCombinations(action);
                action.Emv = wps.Sum();
            }
            InitSolvingReport();
        }

        private IEnumerable<decimal?> GetWpsByActionFromCombinations(Action action)
        {
            var combins = DssDbContext.Combinations.Local.ToList();
            return (combins.Where(c => c.Action == action).Select(c => c.Wp)).ToList();
        }

        private void InitSolvingReport()
        {
            var maxEmv = Convert.ToDecimal(Convert.ToDouble(DssDbContext.Actions.Local.Max(a => a.Emv)));
            var optimalActName = DssDbContext.Actions.Local.FirstOrDefault(a => a.Emv == maxEmv).Name;
            Task.Date = DateTime.Now;
            Task.Recommendation = string.Format(
                "Рекомендуется выбрать действие '{0}'. " +
                "Такое решение принесет максимальное значение средней ожидаемой прибыли равное '{1}' $. " +
                "Такое значение средней ожидаемой прибыли ожидается, " +
                "если многократно (бесчисленное множество раз) будет выбрано это действие при условии, " +
                "что вероятности событий будут неизменны.",
                optimalActName, maxEmv);
            Task.MaxEmv = maxEmv;
        }

        public void SolveEol(List<Combination> fictiveCombinations)
        {
            var actions = DssDbContext.Actions.Local.ToList();
            foreach (var action in actions)
            {
                var wols = GetWolsByActionFromCombinations(action, fictiveCombinations);
                action.Eol = wols.Sum();
            }
            var minEol = Convert.ToDecimal(Convert.ToDouble(DssDbContext.Actions.Local.Min(a => a.Eol)));
            Task.MinEol = minEol;
            BaseMethods.AddTask(Task);
        }

        private IEnumerable<decimal?> GetWolsByActionFromCombinations(Action action, List<Combination> fictiveCombinations)
        {           
            List<Combination> combins;
            combins = fictiveCombinations ?? DssDbContext.Combinations.Local.ToList();
            return (combins.Where(c => c.Action == action).Select(c => c.Wol)).ToList();
        }

        public void Save()
        {
            DssDbContext.SaveChanges();
        }

        public List<Task> GetSolvedTasksFromDb(string taskUniq)
        {
            return DssDbContext.Tasks.Where(x => x.TaskUniq == taskUniq).ToList();
        }

        public List<Action> GetActionsFromDb()
        {
            return DssDbContext.Actions.Select(a => a).ToList();
        }

        public List<Action> GetLocalActionsList()
        {
            return DssDbContext.Actions.Local.ToList();
        }

        public List<Event> GetLocalEventsList()
        {
            var events = DssDbContext.Events.Local;
            return events.Where(ev => ev != null).Select(ev => ev).ToList();
        }
    }
     

}
