using System.Linq;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;
using System.Collections.ObjectModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class EventsDepActionViewModels
    {
        public ObservableCollection<EventsDependingAction> EventDepActions { get; set; }

        public EventsDepActionViewModels(BaseLayer baseLayer)
        {
            DependingActionListViewModel(baseLayer);
        }

        public void DependingActionListViewModel(BaseLayer baseLayer)
        {
            EventDepActions = new ObservableCollection<EventsDependingAction>();
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            var combins = baseLayer.DssDbContext.Combinations.Local.ToList();
            foreach (var action in actions)
            {
                var eventsDepThisAct = combins.Where(c => c.Action == action).Select(c => c.Event);
                var eventsDepThisAction = new ObservableCollection<Event>();
                foreach (var ev in eventsDepThisAct)
                    eventsDepThisAction.Add(ev);

                EventDepActions.Add(new EventsDependingAction
                {
                    Action = action,
                    EventsViewModel = new EventsViewModel(eventsDepThisAction, baseLayer)
                });
            }
        }

        public void CheckUpdatingData(BaseLayer baseLayer)
        {
            CheckDeletedActions(baseLayer);
            CheckOnNewActions(baseLayer);
            CheckDeletedEvents(baseLayer);
        }

        public void CheckOnNewActions(BaseLayer baseLayer)
        {
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            if (EventsDependingActions.Count < actions.Count)
                AddNewAction(actions, baseLayer);
        }

        public void CheckDeletedActions(BaseLayer baseLayer)
        {
            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            var deletingActions = new List<EventsDependingAction>();
            foreach (var eventsDependingAction in EventsDependingActions)
            {
                if (!actions.Contains(eventsDependingAction.Action))
                    deletingActions.Add(eventsDependingAction);
            }
            DeleteEventsDependingAction(deletingActions, baseLayer);
        }

        public void DeleteEventsDependingAction(List<EventsDependingAction> deletingActions, BaseLayer baseLayer)
        {
            foreach (var removingAction in deletingActions)
            {
                baseLayer.BaseMethods.DeleteAction(removingAction.Action);
                EventsDependingActions.Remove(removingAction);
            }
        }

        public void CheckDeletedEvents(BaseLayer baseLayer)
        {
            var events = baseLayer.DssDbContext.Events.Local.ToList(); 
            var deletingEvents = (from eventsDependingAction in EventsDependingActions 
                                  from ev in eventsDependingAction.EventListViewModel.Events 
                                  where !events.Contains(ev) 
                                  select ev).ToList();
            foreach (var deletingEvent in deletingEvents)
            {
                DeleteEvent(deletingEvent);
            }
        }

        public void DeleteEvent(Event eEvent)
        {
            EventsDependingAction eventsDependingAction = null;
            foreach (var eDepA in EventsDependingActions)
            {
                if (eDepA.EventListViewModel.Events.Any(ev => ev == eEvent))
                {
                    eventsDependingAction = eDepA;
                    break;
                }
            }
            /*if (eventsDependingAction != null)
            {
                eventsDependingAction.EventListViewModel.DeleteEvent(eEvent);
            }*/
        }

        private void AddNewAction(List<Action> actions, BaseLayer baseLayer)
        {
            var depAction = EventsDependingActions.Select(ed => ed.Action).ToList();
            foreach (var action in actions)
                if (!depAction.Contains(action))
                    EventsDependingActions.Add(new EventsDependingAction
                    {
                        Action = action,
                        EventListViewModel = new EventListViewModel(new ObservableCollection<Event>(), baseLayer)
                    });
        }
        
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