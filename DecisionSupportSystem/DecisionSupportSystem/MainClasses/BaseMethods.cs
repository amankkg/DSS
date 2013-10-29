using System.Collections.Generic;
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

        public void AddTaskParam(TaskParam param, Task task, TaskParamName name)
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

        public void AddActionParam(ActionParam param)
        {
            if (param == null) return;
            dssDbContext.ActionParams.Local.Add(new ActionParam
                {
                    Action = param.Action,
                    ActionParamName = param.ActionParamName,
                    Value = param.Value
                });
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

        public void AddEventParam(EventParam param, Event eEvent, EventParamName name)
        {
            if (param == null || eEvent == null || name == null) return;
            param.Event = eEvent;
            param.EventParamName = name;
            dssDbContext.Events.Local.Add(eEvent);
        }

        public void AddCombination(Combination combination, Action action, Event eEvent, Task task, decimal cpValue)
        {
            combination.Cp = cpValue;
            combination.Action = action;
            combination.Event = eEvent;
            combination.Task = task;
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
