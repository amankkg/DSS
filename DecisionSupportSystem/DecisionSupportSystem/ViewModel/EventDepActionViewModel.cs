using System.Collections.ObjectModel;
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
                if (value != this._actionSelectedIndex)
                {
                    this._actionSelectedIndex = value;
                    RaisePropertyChanged("ActionSelectedIndex");
                }
            }
        }

        public EventDepActionViewModel(BaseLayer baseLayer, Event eventTemplate, EventsDepActionsViewModel eventsDepActionsViewModel, IErrorCatch errorCatcher)
        {
            this.Actions = baseLayer.DssDbContext.Actions.Local;
            this.EditableEvent = eventTemplate;
            base.ErrorCatcher = errorCatcher;
            this.EventsDepActionsViewModel = eventsDepActionsViewModel;
            AddEventCommand = new DelegateCommand<object>(this.OnAddEvent);
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
            this.EventsDepActionsViewModel.AddEvent(
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
