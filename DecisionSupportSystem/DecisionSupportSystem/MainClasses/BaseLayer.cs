using System;
using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;
using Task = DecisionSupportSystem.DbModel.Task;
//using BaseModel;
/*using Action = BaseModel.Action;
using Task = BaseModel.Task;*/

namespace DecisionSupportSystem.MainClasses
{
    public class BaseLayer
    {
        #region Свойства
        public DssDbEntities DssDbContext { get; set; }

        public Task Task { get; set; }
        public TaskView TaskView { get; set; }
        public BaseMethods BaseMethods { get; set; }
        public IList<CpMax> CpMaxes { get; set; }
        public IList<ActionForSecondType> ActionsForSecondType { get; set; }
        #endregion

        public BaseLayer()
        {
            DssDbContext = new DssDbEntities();
            BaseMethods = new BaseMethods(DssDbContext);
            Task = new Task();
            TaskView = new TaskView();
            CpMaxes = new List<CpMax>();
            ActionsForSecondType = new List<ActionForSecondType>();
        }

        #region Создание комбинаций
        // Создание комбинации действие-событие для задач 1-го типа (события не зависят от действий)
        public void CreateCombinForFirstType()
        {
            DssDbContext.Combinations.Local.Clear();
            var acts = DssDbContext.Actions.Local.ToList();
            var even = DssDbContext.Events.Local.ToList();
            foreach (var action in acts)
                foreach (var eEvent in even)
                    BaseMethods.AddCombination(new Combination(), action, eEvent, Task, 0);
        }

        // Создание комбинации действие-событие для задач 2-го типа (события зависят от действий)
        public void CreateCombinForSecondType()
        {
            foreach (var action in ActionsForSecondType)
                foreach (var eEvent in action.Events)
                    BaseMethods.AddCombination(new Combination(), action, eEvent, Task, 0);
        }
        #endregion

        private void SolveCpMaxes()
        {
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

        public void SolveWpColWol()
        {
            SolveCpMaxes();
            var combins = DssDbContext.Combinations.Local.ToList();
            foreach (var combination in combins)
            {
                combination.Wp = combination.Cp*combination.Event.Probability;
                var cpMax = CpMaxes.First(i => i.Event == combination.Event).Value;
                combination.Col = cpMax - combination.Cp;
                combination.Wol = combination.Col * combination.Event.Probability;
            }
        }

        public void SolveEmvEol()
        {
            var combins = DssDbContext.Combinations.Local.ToList();
            var actions = DssDbContext.Actions.Local.ToList();
            foreach (var a in actions)
            {
                var wps = (combins.Where(c => c.Action == a).Select(c => c.Wp)).ToList();
                a.Emv = wps.Sum();

                var wols = (combins.Where(c => c.Action == a).Select(c => c.Wol)).ToList();
                a.Eol = wols.Sum();
            }
            var maxEmv = Convert.ToDecimal(Convert.ToDouble(DssDbContext.Actions.Local.Max(a => a.Emv)));
            var minEol = Convert.ToDecimal(Convert.ToDouble(DssDbContext.Actions.Local.Min(a => a.Eol)));
            var optimalActName = DssDbContext.Actions.Local.FirstOrDefault(a => a.Emv == maxEmv).Name;
            Task.Date = DateTime.Now;
            Task.Recommendation = string.Format(
                "Рекомендуется выбрать действие '{0}'. " +
                "Такое решение принесет максимальное значение средней ожидаемой прибыли равное '{1}' $. " +
                "Такое значение средней ожидаемой прибыли ожидается, " +
                "если многократно (бесчисленное множество раз) будет выбрано это действие при условии, " +
                "что вероятности событий будут неизменны.",
                optimalActName, maxEmv);
            TaskView.Recommendation = Task.Recommendation;
            TaskView.MaxEmv = maxEmv;
            TaskView.MinEol = minEol;
            BaseMethods.AddTask(Task);
        }

        public void Save()
        {
            DssDbContext.SaveChanges();
        }

        public List<Task> GetSolvedTasks(string taskUniq)
        {
            return DssDbContext.Tasks.Where(x => x.TaskUniq == taskUniq).ToList();
        }

        public List<Action> GetActions()
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
