using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using System.Collections.ObjectModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;

namespace DecisionSupportSystem.ViewModel
{
    public class EventViewModel : BasePropertyChanged, IDataErrorInfo
    {
        private Event _editableEvent;
        public Event EditableEvent
        {
            get { 
                EventsViewModel.SumProbabilities();
                  return _editableEvent; }
            set {
                if (value != this._editableEvent){
                        this._editableEvent = value;
                        RaisePropertyChanged("EditableEvent");}
                }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    RaisePropertyChanged("Name");
                    EventsViewModel.UpdateEvent(this);
                    this.EditableEvent.Name = value;
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
                if (value != this._paramsVisibility)
                {
                    this._paramsVisibility = value;
                    RaisePropertyChanged("ParamsVisibility");
                }
            }
        }

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
        public ObservableCollection<EventParam> EditableEventParams { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public ICommand AddEventCommand { get; private set; }

        public EventViewModel(Event eventTemplate, EventsViewModel eventsViewModel, IErrorCatch errorCatcher)
        {
            ErrorCatcher = errorCatcher;
            this.EventsViewModel = eventsViewModel;
            this.EditableEvent = eventTemplate;
            this.Name = eventTemplate.Name;
            this.EditableEventParams = new ObservableCollection<EventParam>();
            this.AddEventCommand = new DelegateCommand<object>(this.OnAddEvent);
            UpdateEventParams();
        }
        
        public void UpdateEventParams()
        {
            EditableEventParams.Clear();
            var eventParams = this.EditableEvent.EventParams.ToList();
            foreach (var eventParam in eventParams)
                this.EditableEventParams.Add(eventParam);
            ParamsVisibility = EditableEventParams.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }

        public void OnAddEvent(object obj)
        {
            if (IsGenerated && ErrorCatcher.EntityErrorCount == 0)
            {
                Iterator = 0;
                EventsViewModel.ProbabilitySumViewModel.Sum = 0;
                EventsViewModel.Events.Clear();
                EventsViewModel.EventViewModels.Clear();
                GenerateEvents();
            }
            if (ErrorCatcher.EntityErrorCount == 0)
            {
                CreateAndAddEvent();
            }
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
                return (double) EventsCount / 1000;
                return (double)EventsCount / 10;
        }

        private double Factorial(long x)
         {
             return (x == 0) ? 1 : x * Factorial(x - 1);
         }

        public void CreateAndAddEvent()
        {
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
            this.EventsViewModel.AddEvent(new Event
            {
                Name = EditableEvent.Name,
                Probability = EditableEvent.Probability,
                EventParams = eventParams,
                SavingId = EditableEvent.SavingId
            });
        }

        #region реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        errormsg = "Введите название события";
                    }
                }
                if (columnName == "EventsCount")
                {
                    if (EventsCount > 170)
                    {
                        errormsg = "Количество событий не должно превышать 170.";
                    }
                    if (EventsCount < 0)
                    {
                        errormsg = "Количество событий не должно быть меньше 0.";
                    }
                }
                return errormsg;
            }
        }
        #endregion
    }

     
} 
