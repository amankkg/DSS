using System.Linq;
using DecisionSupportSystem.DbModel;
using Task = DecisionSupportSystem.DbModel.Task;

namespace DecisionSupportSystem.MainClasses
{
    public class LoadLayer
    {
        public BaseLayer BaseLayer { get; set; }
        public DssDbEntities dssDbContext;
        
        public LoadLayer()
        {
            BaseLayer = new BaseLayer();
            dssDbContext = new DssDbEntities();
        }

        private void LoadActions(Task task)
        {
            var actions = (dssDbContext.Combinations.Where(c => c.TaskId == task.Id).Select(c => c.Action)).Distinct();
            foreach (var a in actions)
            {
                var action = new Action
                    {
                        Id = a.Id, Name = a.Name, Eol = a.Eol, Emv = a.Emv
                    };
                LoadActionParams(action);
                BaseLayer.DssDbContext.Actions.Local.Add(action);
            }
        }
        
        private Action GetActionById(int id)
        {
            return BaseLayer.DssDbContext.Actions.Local.FirstOrDefault(action => action.Id == id);
        }

        private void LoadActionParams(Action action)
        {
            var actionParams = (from ap in dssDbContext.ActionParams
                                where ap.ActionId == action.Id
                                select ap);
            foreach (var actionParam in actionParams)
            {
                BaseLayer.DssDbContext.ActionParams.Local.Add(new ActionParam{Action = action, ActionParamName = null, Value = actionParam.Value});
            }
        }

        private void LoadEvents(Task task)
        {
            var events = (from c in dssDbContext.Combinations
                          where c.TaskId == task.Id
                          select c.Event).Distinct();
            
            foreach (var ev in events)
            {
                var eEvent = new Event
                    {
                        Id = ev.Id,Name = ev.Name,Probability = ev.Probability
                    };
                LoadEventParams(eEvent);
                BaseLayer.DssDbContext.Events.Local.Add(eEvent);
            }
        }

        private Event GetEventById(int id)
        {
            return BaseLayer.DssDbContext.Events.Local.FirstOrDefault(eEvent => eEvent.Id == id);
        }

        private void LoadEventParams(Event eEvent)
        {
            var eventParams = (from ep in dssDbContext.EventParams
                               where ep.EventId== eEvent.Id
                               select ep);
            foreach (var eventParam in eventParams)
            {
                BaseLayer.DssDbContext.EventParams.Local.Add(new EventParam{Event = eEvent, EventParamName = null, Value = eventParam.Value});
            }
        }

        private void LoadCombinations(Task task)
        {
             var combins = (from c in dssDbContext.Combinations
                                     where c.TaskId == task.Id
                                     select c).ToList();
            LoadActions(task);
            LoadEvents(task);
            foreach (var c in combins)
            {
                var combination = new Combination
                    {
                        Id = c.Id,
                        Action = GetActionById(c.ActionId),
                        Event = GetEventById(c.EventId),
                        Cp = c.Cp, Col = c.Col
                    };
                LoadCombinParams(combination);
                BaseLayer.DssDbContext.Combinations.Local.Add(combination);
            }
        }
        
        private void LoadCombinParams(Combination combination)
        {
            var combinParams = (from combp in dssDbContext.CombinParams
                                where combp.CombinationId == combination.Id
                                select combp).ToList();
            foreach (var combinParam in combinParams)
            {
                BaseLayer.DssDbContext.CombinParams.Local.Add(new CombinParam { Combination = combination, CombinParamName = null, Value = combinParam.Value});
            }
        }

        public void LoadTask(Task task)
        { 
             LoadCombinations(task);
             BaseLayer.Task = task;
        }
    }
}
