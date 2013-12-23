using System;
using System.Linq;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.CommonClasses
{
    public class LoadSolvedTask
    {
        public Task Task { get; set; }
        public DssDbEntities currentDssDbEntities { get; set; }
        private DssDbEntities tempDssDbEntities;
        public List<Combination> Combinations { get; set; }
        public List<Action> SelectedActions { get; set; }
        public List<Event> SelectedEvents { get; set; }

        public LoadSolvedTask(Task task)
        {
            Task = task;
            tempDssDbEntities = new DssDbEntities();
            currentDssDbEntities = new DssDbEntities();
            Combinations = (tempDssDbEntities.Combinations.
                Where(c => c.TaskId == task.Id)).ToList();
            if (Combinations.Count != 0)
            {
                SelectedActions = (Combinations.
                    Select(c => c.Action)).Distinct().ToList();
                SelectedEvents = (Combinations.
                    Select(c => c.Event)).Distinct().ToList();
            }
            else
            {
                SelectedActions = (tempDssDbEntities.Actions.
                    Where(a => a.SavingId == task.SavingId)).ToList();
                SelectedEvents = (tempDssDbEntities.Events.
                    Where(ev => ev.SavingId == task.SavingId)).ToList();
            }
        }

        public void AddCombinationsToCurrentDssDbEntities()
        {
            AddActionsToCurrentDssDbEntities();
            AddEventsToCurrentDssDbEntities();
            AddTaskToCurrentDssDbEntities();
            foreach (var combin in Combinations)
            {
                var combination = new Combination();

                combination.Action = GetActionById(combin.ActionId);
                combination.Event = GetEventById(combin.EventId);
                combination.Cp = combin.Cp;
                combination.Col = combin.Col;
                combination.Task = Task;
                AddCombinParamsToCurrentDssDbEntities(combin, combination);

                currentDssDbEntities.Combinations.Local.Add(combination);
            }
        }

        private void AddCombinParamsToCurrentDssDbEntities(Combination selectedCombination, Combination newCombintaion)
        {
            var combinParams = (tempDssDbEntities.CombinParams.
                Where(combp => combp.CombinationId == selectedCombination.Id)).ToList();
            foreach (var combinParam in combinParams)
            {
                var param = new CombinParam();
                param.Combination = newCombintaion;
                param.Value = combinParam.Value;
                var combinParamName = new CombinParamName { Name = combinParam.CombinParamName.Name };
                param.CombinParamName = combinParamName;

                currentDssDbEntities.CombinParams.Local.Add(param);
            }
        }

        private void AddTaskToCurrentDssDbEntities()
        {
            var task = new Task(); 

            task.Date = DateTime.Now;
            task.Comment = Task.Comment; 
            task.Deleted = Task.Deleted;
            task.TaskUniq = Task.TaskUniq;            
            task.SavingId = Task.SavingId;
            task.TreeDiagramm = Task.TreeDiagramm;
            task.Recommendation = Task.Recommendation;
            
            AddTaskParamsToCurrentDssDbEntities(Task, task);
            Task = task;

            currentDssDbEntities.Tasks.Local.Add(task);
        }

        private void AddTaskParamsToCurrentDssDbEntities(Task selectedTask, Task newTask)
        {
            var taskParams = (tempDssDbEntities.TaskParams.
                Where(tp => tp.TaskId == selectedTask.Id)).ToList();
            foreach (var taskParam in taskParams)
            {
                var param = new TaskParam();
                param.Task = newTask;
                var taskParamName = new TaskParamName {Name = taskParam.TaskParamName.Name};
                param.TaskParamName = taskParamName;
                param.Value = taskParam.Value;

                currentDssDbEntities.TaskParams.Add(param);
            }
        }

        private void AddActionsToCurrentDssDbEntities()
        {
            foreach (var act in SelectedActions)
            {
                var action = new Action();
                action.Name = act.Name;
                action.Eol = act.Eol;
                action.Emv = act.Emv;
                action.SavingId = act.SavingId;
                AddActionParamsToCurrentDssDbEntities(act, action);

                currentDssDbEntities.Actions.Local.Add(action);
            }
        }

        private void AddActionParamsToCurrentDssDbEntities(Action selectedAction, Action newAction)
        {
            var actionParams = (tempDssDbEntities.ActionParams.
                Where(ap => ap.ActionId == selectedAction.Id));
            foreach (var actionParam in actionParams)
            {
                var param = new ActionParam();
                param.Action = newAction;
                param.Value = actionParam.Value;
                var actParamName = new ActionParamName {Name = actionParam.ActionParamName.Name};
                param.ActionParamName = actParamName;

                currentDssDbEntities.ActionParams.Local.Add(param);
            }
        }

        private void AddEventsToCurrentDssDbEntities()
        {
            foreach (var ev in SelectedEvents){
                if (ev == null) continue;
                var even = new Event();
                even.Name = ev.Name;
                even.Probability = ev.Probability;
                even.SavingId = ev.SavingId;
                AddEventParamsToCurrentDssDbEntities(ev, even);

                currentDssDbEntities.Events.Add(even);
            }
        }

        private void AddEventParamsToCurrentDssDbEntities(Event selectedEvent, Event newEvent)
        {
            var eventParams = (tempDssDbEntities.EventParams.
                Where(ep => ep.EventId == selectedEvent.Id));
            foreach (var eventParam in eventParams)
            {
                var param = new EventParam();
                param.Event = newEvent;
                param.Value = eventParam.Value;
                var evParamName = new EventParamName { Name = eventParam.EventParamName.Name };
                param.EventParamName = evParamName;

                currentDssDbEntities.EventParams.Local.Add(param);
            }
        }

        private Action GetActionById(int? id)
        {
            var localActions = currentDssDbEntities.Actions.Local.ToList();
            for (int i = 0; i < SelectedActions.Count; i++)
                if (SelectedActions[i].Id == id)
                    return localActions[i];
            return null;
        }

        private Event GetEventById(int? id)
        {
            if (id != null)
            {
                var localEvents = currentDssDbEntities.Events.Local.ToList();
                for (int i = 0; i < SelectedEvents.Count; i++)
                {
                    if (SelectedEvents[i].Id == id)
                        return localEvents[i];
                }
            }
            return null;
        }
    }
}
