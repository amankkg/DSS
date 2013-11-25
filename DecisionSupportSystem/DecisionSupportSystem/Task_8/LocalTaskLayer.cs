using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_8
{
    public class TaskParams : BasePropertyChanged
    {
        public Task Task { get; set; }

        public TaskParams()
        {
            Shtraf = new TaskParam();
            Premia = new TaskParam();
        }

        private TaskParam _shtraf;
        public TaskParam Shtraf
        {
            get
            {
                return _shtraf;
            }
            set
            {
                if (value != this._shtraf)
                {
                    this._shtraf = value;
                    RaisePropertyChanged("Shtraf");
                }
            }
        }

        private TaskParam _premia;
        public TaskParam Premia
        {
            get
            {
                return _premia;
            }
            set
            {
                if (value != this._premia)
                {
                    this._premia = value;
                    RaisePropertyChanged("Premia");
                }
            }
        }

        public void InitTask()
        {
            Task.TaskParams.Add(Shtraf);
            Task.TaskParams.Add(Premia);
        }
    }

    public static class LocalTaskLayer
    {
        public static ObservableCollection<Combination> CombinationsList = new ObservableCollection<Combination>();
        public static TaskParams taskParams = new TaskParams();
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

        public static void SolveCP(BaseLayer baseLayer)
        {
            var shtraf = baseLayer.Task.TaskParams.ToList()[0].Value;
            var premia =  baseLayer.Task.TaskParams.ToList()[1].Value;
            var combintions = baseLayer.DssDbContext.Combinations.Local.ToList();
            foreach (var combination in combintions)
            {
                var actParam = combination.Action.ActionParams.ToList()[0].Value;
                var evParam = combination.Event.EventParams.ToList()[0].Value;
                var RaznBrak = actParam - evParam;
                if (RaznBrak >= 0)
                {
                    combination.Cp = RaznBrak * 10 * premia;
                }
                else
                
                    combination.Cp = RaznBrak * 10 * shtraf;
                
            }
            
        }
    }
}
