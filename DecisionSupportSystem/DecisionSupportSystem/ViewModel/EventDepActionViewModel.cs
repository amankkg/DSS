using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;

namespace DecisionSupportSystem.ViewModel
{
    public class EventDepActionViewModel : BasePropertyChanged
    {
        public Event EditableEvent { get; set; }
        public ObservableCollection<Action> Actions { get; set; }
        public ICommand AddEventCommand { get; private set; }
        public EventsDepActionsViewModel EventsDepActionsViewModel { get; set; }
        private int _actionSelectedIndex;

        public int ActionSelectedIndex
        {
            get
            {
                return _actionSelectedIndex;
            }
            set
            {
                if (value != _actionSelectedIndex)
                {
                    _actionSelectedIndex = value;
                    RaisePropertyChanged("ActionSelectedIndex");
                }
            }
        }

        private Visibility _paramsVisibility;
        public Visibility ParamsVisibility
        {
            get
            {
                return _paramsVisibility;
            }
            set
            {
                if (value != _paramsVisibility)
                {
                    _paramsVisibility = value;
                    RaisePropertyChanged("ParamsVisibility");
                }
            }
        }

        public EventDepActionViewModel(ObservableCollection<Action> actions, Event eventTemplate, 
                                       EventsDepActionsViewModel eventsDepActionsViewModel, IErrorCatch errorCatcher)
        {
            Actions = actions;
            EditableEvent = eventTemplate;
            if (EditableEvent.EventParams.Count == 0)
                ParamsVisibility = Visibility.Hidden;
            ErrorCatcher = errorCatcher;
            EventsDepActionsViewModel = eventsDepActionsViewModel;
            AddEventCommand = new DelegateCommand<object>(OnAddEvent);
        }

        private void OnAddEvent(object obj)
        {
            if (ErrorCatcher.EntityErrorCount != 0) return;
            var eventParams = new Collection<EventParam>();
            foreach (var eventParam in EditableEvent.EventParams)
                eventParams.Add(new EventParam
                {
                    Event = eventParam.Event,
                    Value = eventParam.Value,
                    EventId = eventParam.EventId,
                    Id = eventParam.Id,
                    EventParamName = eventParam.EventParamName
                });
                EventsDepActionsViewModel.AddEvent(
                Actions[ActionSelectedIndex],
               new Event
               {
                   Name = EditableEvent.Name,
                   Probability = EditableEvent.Probability,
                   EventParams = eventParams,
                   SavingId = EditableEvent.SavingId
               });
        }
    }
}
