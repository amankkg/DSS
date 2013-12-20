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
            if(ErrorCatcher.EntityErrorCount != 0) return;
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
                return errormsg;
            }
        }
        #endregion
    }

     
} 
