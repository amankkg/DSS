using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class EventsDepActionsViewModel : BasePropertyChanged
    {
        public ObservableCollection<EventsDependingAction> EventsDependingActions { get; set; }
        public DssDbEntities DssDbEntities { get; set; }

        public EventsDepActionsViewModel(DssDbEntities dssDbEntities, IErrorCatch errorCatcher)
        {
            DssDbEntities = dssDbEntities;
            ErrorCatcher = errorCatcher;
            InitEventsDependingActions();
        }

        public void InitEventsDependingActions()
        {
            EventsDependingActions = new ObservableCollection<EventsDependingAction>();
            var actions = DssDbEntities.Actions.Local;
            var combins = DssDbEntities.Combinations.Local;
            foreach (var action in actions){
                var eventsDepCombinsAction = combins.Where(c => c.Action == action).Select(c => c.Event);
                var eventsDepAction = new ObservableCollection<Event>();
                foreach (var ev in eventsDepCombinsAction)
                    eventsDepAction.Add(ev);

                EventsDependingActions.Add(new EventsDependingAction{
                    Action = action,
                    EventsViewModel = new EventsViewModel(eventsDepAction, base.ErrorCatcher){ParamsVisibility = Visibility.Hidden}
                });
            }
        }

        public void AddEvent(Action action, Event even)
        {
            foreach (var eventDepAction in EventsDependingActions)
                if (eventDepAction.Action == action)
                    eventDepAction.EventsViewModel.AddEvent(even);
        }

        public void CheckForUpdatedData()
        {
            CheckForDeletedAction();
            CheckForNewAction();
            CheckForDeletedEvent();
        }

        public void CheckForNewAction()
        {
            var actions = DssDbEntities.Actions.Local;
            if (EventsDependingActions.Count < actions.Count)
                AddNewAction(actions);
        }

        private void AddNewAction(ObservableCollection<Action> actions)
        {
            var lastAction = EventsDependingActions.Select(ed => ed.Action).ToList();
            foreach (var action in actions)
                if (!lastAction.Contains(action))
                    EventsDependingActions.Add(new EventsDependingAction
                    {
                       Action = action,
                       EventsViewModel = new EventsViewModel(new ObservableCollection<Event>(), ErrorCatcher)
                    });
        }

        public void CheckForDeletedEvent()
        {
            var events = DssDbEntities.Events.Local.ToList();
            var deletingEvents = (from eventsDependingAction in EventsDependingActions
                                  from ev in eventsDependingAction.EventsViewModel.Events
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
                if (eDepA.EventsViewModel.Events.Any(ev => ev == eEvent))
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

        public void CheckForDeletedAction()
        {
            var actions = DssDbEntities.Actions.Local.ToList();
            var deletingActions = new List<EventsDependingAction>();
            foreach (var eventsDependingAction in EventsDependingActions)
            {
                if (!actions.Contains(eventsDependingAction.Action))
                    deletingActions.Add(eventsDependingAction);
            }
            DeleteActions(deletingActions);
        }

        public void DeleteActions(List<EventsDependingAction> deletingActions)
        {
            foreach (var removingAction in deletingActions)
            {
                CRUD.DeleteAction(removingAction.Action);
                EventsDependingActions.Remove(removingAction);
            }
        }

    }
}
