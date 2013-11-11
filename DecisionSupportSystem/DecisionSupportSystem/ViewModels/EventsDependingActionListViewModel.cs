using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModels
{
    public class EventsDependingActionListViewModel
    {
        public EventsDependingActionListViewModel(BaseLayer baseLayer)
        {
            EventsDependingActions = new ObservableCollection<EventsDependingAction>();
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            var combins = baseLayer.DssDbContext.Combinations.Local.ToList();
            foreach (var action in actions)
            {
                    var eventsDepThisAction = combins.Where(c => c.Action == action).Select(c => c.Event).ToList();
                    EventsDependingActions.Add(new EventsDependingAction
                        {
                            Action = action,
                            EventListViewModel = new EventListViewModel(eventsDepThisAction, baseLayer)
                        });
            }
        }

        public void CheckOnNewActions(BaseLayer baseLayer)
        {
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            if (EventsDependingActions.Count < actions.Count)
                AddNewAction(actions, baseLayer);
        }

        private void AddNewAction(List<Action> actions, BaseLayer baseLayer)
        {
            var depAction = EventsDependingActions.Select(ed => ed.Action).ToList();
            foreach (var action in actions)
                if (!depAction.Contains(action))
                    EventsDependingActions.Add(new EventsDependingAction
                    {
                        Action = action,
                        EventListViewModel = new EventListViewModel(new List<Event>(), baseLayer)
                    });
        }

        public ObservableCollection<EventsDependingAction> EventsDependingActions { get; set; }
         
        public void AddEvent(Action act, Event ev)
        {
            foreach (var eventDepAction in EventsDependingActions)
                if (eventDepAction.Action == act)
                {
                    eventDepAction.EventListViewModel.AddEvent(ev);
                }
        }
    }
}