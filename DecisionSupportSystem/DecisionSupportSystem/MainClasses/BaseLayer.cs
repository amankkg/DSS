using System;
using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;
using Task = DecisionSupportSystem.DbModel.Task;

namespace DecisionSupportSystem.MainClasses
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

        private void SolveCpMaxes()
        {
            CpMaxes.Clear();
            var events = DssDbContext.Events.Local.ToList();
            var combins = DssDbContext.Combinations.Local.ToList();
            foreach (var eEvent in events)
            {
                var cpsForCurrentEvent = (combins.Where(combination => combination.Event == eEvent)
                                                      .Select(combination => combination.Cp)).ToList();
                var max = cpsForCurrentEvent.Max();
                if (max != null)
                    CpMaxes.Add( new CpMax
                    {
                        Event = eEvent,
                        Value =  (decimal) max,
                    });
            }
        }

        public void SolveThisTask()
        {
            SolveWp();
            SolveColWol(null);
            SolveEmv();
            SolveEol(null);
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
            SolveCpMaxes();
            List<Combination> combins;
            if (fictiveCombinations != null)
                combins = fictiveCombinations;
            else
                combins = DssDbContext.Combinations.Local.ToList();
            foreach (var combination in combins)
            {
                var cpMax = CpMaxes.First(i => i.Event == combination.Event).Value;
                combination.Col = cpMax - combination.Cp;
                combination.Wol = combination.Col * combination.Event.Probability;
            }
        }
        
        public void SolveEmv()
        {
            var combins = DssDbContext.Combinations.Local.ToList();
            var actions = DssDbContext.Actions.Local.ToList();
            foreach (var a in actions)
            {
                var wps = (combins.Where(c => c.Action == a).Select(c => c.Wp)).ToList();
                a.Emv = wps.Sum();
            }
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
            SolvedTaskView.Recommendation = Task.Recommendation;
            SolvedTaskView.MaxEmv = maxEmv;
        }

        public void SolveEol(List<Combination> fictiveCombinations)
        {
            List<Combination> combins;
            if (fictiveCombinations == null)
                combins = DssDbContext.Combinations.Local.ToList();
            else
                combins = fictiveCombinations;
            var actions = DssDbContext.Actions.Local.ToList();
            foreach (var a in actions)
            {
                var wols = (combins.Where(c => c.Action == a).Select(c => c.Wol)).ToList();
                a.Eol = wols.Sum();
            }
            var minEol = Convert.ToDecimal(Convert.ToDouble(DssDbContext.Actions.Local.Min(a => a.Eol)));
            SolvedTaskView.MinEol = minEol;
            BaseMethods.AddTask(Task);
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
            return DssDbContext.Events.Local.ToList();
        }
    }
     

}
