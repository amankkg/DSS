using System.Linq;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.CommonClasses
{ 
    public static class CRUD
    {
        public static DssDbEntities DssDbEntities;

        #region Функции добавления данных в локальный DssDbContext

        public static void AddTask(Task task)
        {
            if (task == null) return;
            DssDbEntities.Tasks.Local.Add(task);
        }

        public static void AddTaskParam(Task task, TaskParam param, TaskParamName name, double value)
        {
            if (param == null || task == null || name == null) return;
            param.Task = task;
            param.TaskParamName = name;
            param.Value = value;
            DssDbEntities.TaskParams.Add(param);
        }

        public static void AddAction(Action action)
        {
            if (action == null) return;
            DssDbEntities.Actions.Local.Add(action);
        }

        public static void AddEvent(Event eEvent)
        {
            if (eEvent == null) return;
            DssDbEntities.Events.Add(eEvent);
        }

        public static void AddActionParam(Action action, ActionParam param, ActionParamName name, double value)
        {
            if (param == null || action == null) return;
            param.Action = action;
            param.Value = value;
            param.ActionParamName = name;
            DssDbEntities.ActionParams.Local.Add(param);
        }

        public static void AddEventParam(Event eEvent, EventParam param, EventParamName name, double value)
        {
            if (param == null || eEvent == null) return;
            param.Event = eEvent;
            param.Value = value;
            param.EventParamName = name;
            DssDbEntities.EventParams.Local.Add(param);
        }

        public static void AddCombination(Combination combination, Action action, Event eEvent, Task task, double cpValue)
        {
            combination.Cp = cpValue;
            combination.Action = action;
            combination.Event = eEvent;
            combination.Task = task;
            DssDbEntities.Combinations.Local.Add(combination);
        }

        public static void AddCombination(Combination combination)
        {
            if (combination != null)
                DssDbEntities.Combinations.Local.Add(combination);
        }

        public static void AddCombinParamNames(List<CombinParamName> combinParamNames)
        {
            if (combinParamNames == null) return;
            foreach (var combinParamName in combinParamNames)
            {
               DssDbEntities.CombinParamNames.Add(combinParamName);
            }
        }

        public static void AddCombinationParams(List<CombinParam> combinParams)
        {
            if (combinParams == null) return;
            foreach (var combinParam in combinParams)
            {
                DssDbEntities.CombinParams.Add(combinParam);
            }
        }

        public static void AddCombinationParam(Combination combination, CombinParam param, CombinParamName name, double value)
        {
            if (param == null || combination == null) return;
            param.Combination = combination;
            param.Value = value;
            param.CombinParamName = name;
            DssDbEntities.CombinParams.Local.Add(param);
        }
        #endregion

        #region Функции удаления данных из локального DssDbContext
        public static void DeleteAction(Action act)
        {
            if (act != null)
            {
                if (DssDbEntities.Combinations.Local.Count > 0)
                {
                    var combinations = DssDbEntities.Combinations.Local.ToList();
                    var removingCombinations =
                        combinations.Where(combination => combination.Action.Name == act.Name).ToList();

                    var removedEvents = removingCombinations.Select(removedCombination => removedCombination.Event).ToList(); 
                    DeleteEventsByInList(removedEvents);
                    foreach (var removingCombination in removingCombinations)
                        DssDbEntities.Combinations.Local.Remove(removingCombination);
                }
                DssDbEntities.Actions.Local.Remove(act);
            }
        }

        public static void DeleteEventsByInList(List<Event> removedEvents)
        {
            if (removedEvents.Count > 0)
            {
                foreach (var removedEvent in removedEvents)
                {
                    var evCount = (DssDbEntities.Combinations.Local.Where(comb => comb.Event == removedEvent)).Count();
                    if(evCount == 1)
                    DeleteEvent(removedEvent);
                }
            }
        }

        public static void DeleteEvent(Event ev)
        {
            if (ev != null)
            {
                if (DssDbEntities.Combinations.Local.Count > 0)
                {
                    var combinations = DssDbEntities.Combinations.Local.ToList();
                    var removingCombinations =
                        combinations.Where(combination => combination.Event.Name == ev.Name).ToList();

                    foreach (var removedCombination in removingCombinations)
                        DssDbEntities.Combinations.Local.Remove(removedCombination);
                }
                DssDbEntities.Events.Local.Remove(ev);
            }
        }


        #endregion 

    }
}
