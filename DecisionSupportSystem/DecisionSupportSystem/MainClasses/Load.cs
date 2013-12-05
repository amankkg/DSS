using System;
using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.MainClasses
{
    public class Load
    {
        public BaseLayer BaseLayer { get; set; }
        public DssDbEntities dssDbContext;
        public Task Task { get; set; }
        public List<Combination> Combinations { get; set; }
        public List<Action> Actions { get; set; }
        public List<Event> Events { get; set; }

        public Load(Task task)
        {
            BaseLayer = new BaseLayer();
            dssDbContext = new DssDbEntities();
            Task = task;
            Combinations = (dssDbContext.Combinations.Where(c => c.TaskId == task.Id)).ToList();
            if (Combinations.Count != 0)
            {
                Actions = (Combinations.Select(c => c.Action)).Distinct().ToList();
                var nullableCombinations = Combinations.Where(combination => combination.Event == null).ToList();
                foreach (var nullableCombination in nullableCombinations)
                {
                    Combinations.Remove(nullableCombination);
                }
                Events = (Combinations.Select(c => c.Event)).Distinct().ToList();
            }
            else
            {
                Actions = (dssDbContext.Actions.Where(a => a.SavingId == task.SavingId)).ToList();
                Events = (dssDbContext.Events.Where(ev => ev.SavingId == task.SavingId)).ToList();
            }
        }

        private void LoadActions()
        {
            foreach (var a in Actions)
            {
                var action = new Action
                {
                    Name = a.Name,
                    Eol = a.Eol,
                    Emv = a.Emv,
                    SavingId = a.SavingId
                };
                LoadActionParams(a, action);
                BaseLayer.BaseMethods.AddAction(action);
            }
        }

        private void LoadActionParams(Action oldAction, Action newAction)
        {
            var actionParams = (dssDbContext.ActionParams.Where(ap => ap.ActionId == oldAction.Id));
            foreach (var actionParam in actionParams)
            {
                BaseLayer.BaseMethods.AddActionParam(newAction, new ActionParam(), new ActionParamName{Name = actionParam.ActionParamName.Name}, actionParam.Value);
            }
        }

        private void LoadEvents()
        {
            foreach (var ev in Events)
            {
                if (ev != null)
                {
                    var eEvent = new Event
                        {
                            Name = ev.Name,
                            Probability = ev.Probability,
                            SavingId = ev.SavingId
                        };
                    LoadEventParams(ev, eEvent);
                    BaseLayer.BaseMethods.AddEvent(eEvent);
                }
            }
        }
        
        private void LoadEventParams(Event oldEvent, Event newEvent)
        {
            var eventParams = (dssDbContext.EventParams.Where(ep => ep.EventId == oldEvent.Id));
            foreach (var eventParam in eventParams)
            {
                BaseLayer.BaseMethods.AddEventParam( newEvent, new EventParam(), new EventParamName{Name = eventParam.EventParamName.Name}, eventParam.Value);
            }
        }

        public void LoadCombinations()
        {
            LoadActions();
            LoadEvents();
            var task = LoadTask();
            foreach (var c in Combinations)
            {
                var combination = new Combination
                {
                    Action = GetActionById(c.ActionId),
                    Event = GetEventById(c.EventId),
                    Cp = c.Cp,
                    Col = c.Col,
                    Task = task
                };
                LoadCombinParams(c, combination);
                BaseLayer.BaseMethods.AddCombination(combination);
            }
        }

        private Action GetActionById(int? id)
        {
            var localActions = BaseLayer.GetLocalActionsList();
            for (int i = 0; i < Actions.Count; i++)
                if (Actions[i].Id == id)
                    return localActions[i];
            return null;
        }
        
        private Event GetEventById(int? id)
        {
            if (id != null)
            {
                var localEvents = BaseLayer.GetLocalEventsList();
                for (int i = 0; i < Events.Count; i++)
                {
                    if (Events[i].Id == id)
                        return localEvents[i];
                }
            }
            return null;
        }

        private void LoadCombinParams(Combination oldCombination, Combination newCombintaion)
        {
            var combinParams = (dssDbContext.CombinParams.Where(combp => combp.CombinationId == oldCombination.Id)).ToList();
            foreach (var combinParam in combinParams)
            {
                BaseLayer.BaseMethods.AddCombinationParam(newCombintaion, new CombinParam(), 
                    new CombinParamName{Name = combinParam.CombinParamName.Name}, combinParam.Value);
            }
        }

        private Task LoadTask()
        {
            var task = new Task
                {
                    Comment = Task.Comment,
                    Date = DateTime.Now,
                    Recommendation = Task.Recommendation,
                    TaskUniq = Task.TaskUniq,
                    Deleted = Task.Deleted,
                    SavingId = Task.SavingId,
                    TreeDiagramm = Task.TreeDiagramm
                };
            LoadTaskParams(Task, task);
            BaseLayer.Task = task;
            SolvedTaskViewInit(task);
            BaseLayer.BaseMethods.AddTask(task);
            return task;
        }

        private void LoadTaskParams(Task oldTask, Task newTask)
        {
            var taskParams = (dssDbContext.TaskParams.Where(tp => tp.TaskId == oldTask.Id)).ToList();
            foreach (var taskParam in taskParams)
            {
                BaseLayer.BaseMethods.AddTaskParam(newTask, new TaskParam(), new TaskParamName{Name = taskParam.TaskParamName.Name}, taskParam.Value);
            }
        }

        private void SolvedTaskViewInit(Task task)
        {
            BaseLayer.SolvedTaskView.Recommendation = task.Recommendation;
            var maxEmv = Convert.ToDecimal(Convert.ToDouble(Actions.Max(a => a.Emv)));
            var minEol = Convert.ToDecimal(Convert.ToDouble(Actions.Min(a => a.Eol)));
            BaseLayer.SolvedTaskView.MaxEmv = maxEmv;
            BaseLayer.SolvedTaskView.MinEol = minEol;
            BaseLayer.SolvedTaskView.Comment = task.Comment;
        }


    }
}
