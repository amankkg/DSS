using System.Collections.Generic;
//using BaseModel;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.MainClasses
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
            dssDbContext.TaskParams.Add(param);
        }

        public void AddAction(Action action)
        {
            if (action == null) return;
            dssDbContext.Actions.Local.Add(action);
        }

        public void AddActionParamName(ActionParamName actionParamName)
        {
            if (actionParamName == null) return;
                dssDbContext.ActionParamNames.Add(actionParamName);
        }

       

        public void AddEvent(Event eEvent)
        {
            if (eEvent == null) return;
            dssDbContext.Events.Add(eEvent);
        }

        public void AddEventParamName(EventParamName eventParamName)
        {
            if (eventParamName == null) return;
            dssDbContext.EventParamNames.Add(eventParamName);
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
        public void DeleteAction(Action action)
        {
            if (action != null)
                dssDbContext.Actions.Local.Remove(action);
        }

        public void DeleteEvent(Event eEvent)
        {
            if (eEvent != null)
                dssDbContext.Events.Local.Remove(eEvent);
        }
        #endregion

    }
}
