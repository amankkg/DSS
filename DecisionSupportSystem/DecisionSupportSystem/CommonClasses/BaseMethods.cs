using System.Linq;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.CommonClasses
{ 
    public class BaseMethods
    {
        private DssDbEntities dssDbContext;

        public BaseMethods(DssDbEntities dssDbContext)
        {
            this.dssDbContext = dssDbContext;
        }

        #region Функции добавления данных в локальный DssDbContext

        public void AddTask(Task task)
        {
            if (task == null) return;
            dssDbContext.Tasks.Local.Add(task);
        }

        public void AddTaskParam(Task task, TaskParam param, TaskParamName name, decimal value)
        {
            if (param == null || task == null || name == null) return;
            param.Task = task;
            param.TaskParamName = name;
            param.Value = value;
            dssDbContext.TaskParams.Add(param);
        }

        public void AddAction(Action action)
        {
            if (action == null) return;
            dssDbContext.Actions.Local.Add(action);
        }

        public void AddEvent(Event eEvent)
        {
            if (eEvent == null) return;
            dssDbContext.Events.Add(eEvent);
        }
        
        public void AddActionParam(Action action, ActionParam param, ActionParamName name, decimal value)
        {
            if (param == null || action == null) return;
            param.Action = action;
            param.Value = value;
            param.ActionParamName = name;
            dssDbContext.ActionParams.Local.Add(param);
        }

        public void AddEventParam( Event eEvent, EventParam param, EventParamName name, decimal value)
        {
            if (param == null || eEvent == null) return;
            param.Event = eEvent;
            param.Value = value;
            param.EventParamName = name;
            dssDbContext.EventParams.Local.Add(param);
        }

        public void AddCombination(Combination combination, Action action, Event eEvent, Task task, decimal cpValue)
        {
            combination.Cp = cpValue;
            combination.Action = action;
            combination.Event = eEvent;
            combination.Task = task;
            dssDbContext.Combinations.Local.Add(combination);
        }

        public void AddCombination(Combination combination)
        {
            if (combination != null)
                dssDbContext.Combinations.Local.Add(combination);
        }

        public void AddCombinParamNames(List<CombinParamName> combinParamNames)
        {
            if (combinParamNames == null) return;
            foreach (var combinParamName in combinParamNames)
            {
               dssDbContext.CombinParamNames.Add(combinParamName);
            }
        }

        public void AddCombinationParams(List<CombinParam> combinParams)
        {
            if (combinParams == null) return;
            foreach (var combinParam in combinParams)
            {
                dssDbContext.CombinParams.Add(combinParam);
            }
        }

        public void AddCombinationParam(Combination combination, CombinParam param, CombinParamName name, decimal value)
        {
            if (param == null || combination == null) return;
            param.Combination = combination;
            param.Value = value;
            param.CombinParamName = name;
            dssDbContext.CombinParams.Local.Add(param);
        }
        #endregion

        #region Функции удаления данных из локального DssDbContext
        public void DeleteAction(Action act)
        {
            if (act != null)
            {
                if (dssDbContext.Combinations.Local.Count > 0)
                {
                    var combinations = dssDbContext.Combinations.Local.ToList();
                    var removingCombinations =
                        combinations.Where(combination => combination.Action.Name == act.Name).ToList();

                    var removedEvents = removingCombinations.Select(removedCombination => removedCombination.Event).ToList(); 
                    DeleteEventsByInList(removedEvents);
                    foreach (var removingCombination in removingCombinations)
                        dssDbContext.Combinations.Local.Remove(removingCombination);
                }
                dssDbContext.Actions.Local.Remove(act);
            }
        }

        public void DeleteEventsByInList(List<Event> removedEvents)
        {
            if (removedEvents.Count > 0)
            {
                foreach (var removedEvent in removedEvents)
                {
                    var evCount = (dssDbContext.Combinations.Local.Where(comb => comb.Event == removedEvent)).Count();
                    if(evCount == 1)
                    DeleteEvent(removedEvent);
                }
            }
        }

        public void DeleteEvent(Event ev)
        {
            if (ev != null)
            {
                if (dssDbContext.Combinations.Local.Count > 0)
                {
                    var combinations = dssDbContext.Combinations.Local.ToList();
                    var removingCombinations =
                        combinations.Where(combination => combination.Event.Name == ev.Name).ToList();

                    foreach (var removedCombination in removingCombinations)
                        dssDbContext.Combinations.Local.Remove(removedCombination);
                }
                dssDbContext.Events.Local.Remove(ev);
            }
        }


        #endregion 

    }
}
