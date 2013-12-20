using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;

namespace DecisionSupportSystem.ViewModel
{
    public class EventWithExtensionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        private Event _editableEvent;
        public Event EditableEvent
        {
            get
            {
                EventsWithExtensionsViewModel.SumProbabilities();
                return _editableEvent;
            }
            set
            {
                if (value != this._editableEvent)
                {
                    this._editableEvent = value;
                    RaisePropertyChanged("EditableEvent");
                }
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
                    EventsWithExtensionsViewModel.UpdateEvent(this);
                    this.EditableEvent.Name = value;
                }
            }
        }

        private decimal _isExtendableEventParam;
        public decimal IsExtendableEventParam
        {
            get
            {
                return _isExtendableEventParam;
            }
            set
            {
                if (value != this._isExtendableEventParam)
                {
                    this._isExtendableEventParam = value;
                    this.EditableEvent.EventParams.ToList()[0].Value = value;
                    RaisePropertyChanged("IsExtendableEventParam");
                }
            }
        }

        private bool _isExtendableEvent;
        public bool IsExtendableEvent
        {
            get
            {
                return _isExtendableEvent;
            }
            set
            {
                if (value != this._isExtendableEvent)
                {
                    IsExtendableEventParam = value ? 1 : 0;
                    this._isExtendableEvent = value;
                    RaisePropertyChanged("IsExtendableEvent");

                }
            }
        }
        public EventsWithExtensionsViewModel EventsWithExtensionsViewModel { get; set; }
        public ICommand AddEventCommand { get; private set; }

        public EventWithExtensionViewModel(Event eventTemplate, EventsWithExtensionsViewModel eventsWithExtensionsViewModel, IErrorCatch errorCatcher)
        {
            ErrorCatcher = errorCatcher;
            this.EventsWithExtensionsViewModel = eventsWithExtensionsViewModel;
            this.EditableEvent = eventTemplate;
            this.Name = eventTemplate.Name;
            this.AddEventCommand = new DelegateCommand<object>(this.OnAddEvent);
            this.IsExtendableEventParam = eventTemplate.EventParams.ToList()[0].Value;
            this.IsExtendableEvent = Convert.ToBoolean(IsExtendableEventParam);
        }

        public void OnAddEvent(object obj)
        {
            bool eventsHaveExtendEvent = false;
            if (IsExtendableEvent)
            {
                var events = EventsWithExtensionsViewModel.BaseLayer.DssDbContext.Events.Local.ToList();
                eventsHaveExtendEvent = events.Any(ev => ev.EventParams.ToList()[0].Value == 1);
            }
            if (ErrorCatcher.EntityErrorCount != 0 || eventsHaveExtendEvent) return;
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
            this.EventsWithExtensionsViewModel.AddEvent(new Event
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
