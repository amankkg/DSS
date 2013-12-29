using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class EventDepActionViewModel : BasePropertyChanged
    {
        public Event EditableEvent { get; set; }
        public Random Random { get; set; }
        public int randomMax { get; set; }
        public ObservableCollection<EventParam> EditableEventParams { get; set; }
        public ObservableCollection<Action> Actions { get; set; }
        public ICommand AddEventCommand { get; private set; }
        public EventsDepActionsViewModel EventsDepActionsViewModel { get; set; }
        private int _actionSelectedIndex;

        private int eventsCount;

        private bool isGenerated;
        public bool IsGenerated
        {
            get
            {
                return isGenerated;
            }
            set
            {
                if (value != this.isGenerated)
                {
                    this.isGenerated = value;
                    RaisePropertyChanged("IsGenerated");
                    if (!isGenerated)
                        EventsCount = 0;
                }
            }
        }

        public int EventsCount
        {
            get
            {
                return eventsCount;
            }
            set
            {
                if (value != this.eventsCount)
                {
                    this.eventsCount = value;
                    RaisePropertyChanged("EventsCount");
                }
            }
        }
        public int Iterator = 0;

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
            EditableEventParams = new ObservableCollection<EventParam>();
            ErrorCatcher = errorCatcher;
            EventsDepActionsViewModel = eventsDepActionsViewModel;
            AddEventCommand = new DelegateCommand<object>(OnAddEvent);
            UpdateEventParams();
            Random = new Random();
        }

        public void UpdateEventParams()
        {
            EditableEventParams.Clear();
            var eventParams = this.EditableEvent.EventParams.ToList();
            foreach (var eventParam in eventParams)
                this.EditableEventParams.Add(eventParam);
            ParamsVisibility = EditableEventParams.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }

        private void OnAddEvent(object obj)
        {
            if (IsGenerated && ErrorCatcher.EntityErrorCount == 0)
            {
                Iterator = 0;
                foreach (var eventDepAction in EventsDepActionsViewModel.EventsDependingActions)
                    if (eventDepAction.Action == Actions[ActionSelectedIndex])
                    {
                        eventDepAction.EventsViewModel.Events.Clear();
                        eventDepAction.EventsViewModel.Events.Clear();
                        eventDepAction.EventsViewModel.ProbabilitySumViewModel.Sum = 0;
                        break;
                    }

                GenerateEvents();
            }
            if (ErrorCatcher.EntityErrorCount == 0)
            {
                CreateAndAddEvent();
            }

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

        public void GenerateEvents()
        {
            if (EventsCount > 1)
                while (Iterator != EventsCount)
                {
                    ErrorCatcher.EntityErrorCount = 0;
                    var k = Iterator;
                    double m = DefineM();
                    double f = Factorial(k);
                    double p = Math.Pow(m, k);
                    double ex = Math.Exp(-m);
                    double prob = p * ex / f;
                    EditableEvent.Probability = Math.Abs(prob);
                    EditableEvent.Name = String.Format("Событие {0}", Iterator = Iterator + 1);

                    CreateAndAddEvent();
                }
            else
                CreateAndAddEvent();
        }

        private double DefineM()
        {
            if (EventsCount < 10)
                return (double)EventsCount / 1000;
            return (double)EventsCount / 10;
        }

        private double Factorial(long x)
        {
            return (x == 0) ? 1 : x * Factorial(x - 1);
        }

        public void CreateAndAddEvent()
        {
            var eventParams = InitEventParams();
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

        private Collection<EventParam> InitEventParams()
        {
            var eventParams = new Collection<EventParam>();
            foreach (var eventParam in EditableEvent.EventParams)
            {
                var evParam = new EventParam
                {
                    Event = eventParam.Event,
                    EventId = eventParam.EventId,
                    Id = eventParam.Id,
                    EventParamName = eventParam.EventParamName
                };
                if (IsGenerated) evParam.Value = Random.Next(randomMax);
                else evParam.Value = eventParam.Value;
                eventParams.Add(evParam);
            }
            return eventParams;
        }
    }
}
