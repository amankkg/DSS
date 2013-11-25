using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;
using Event = DecisionSupportSystem.DbModel.Event;

namespace DecisionSupportSystem.ViewModels
{
    public class EventWithParamViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public EventWithParamViewModel(Event ev, EventParam eventParam, EventWithParamListViewModel eventWithParamListViewModel)
        {
            this.EventWithParamListViewModel = eventWithParamListViewModel;
            this.Name = ev.Name;
            this.Probability = ev.Probability;
            this.EventParam = eventParam;
            this.AddEventCommand = new DelegateCommand<object>(this.OnAddEvent, this.CanAddEvent);
        }

        public EventWithParamViewModel(EventWithParamListViewModel eventWithParamListViewModel)
        {
            var ev = new Event();
            var paramValue = new EventParam();
            this.EventWithParamListViewModel = eventWithParamListViewModel;
            this.Name = ev.Name;
            this.Probability = ev.Probability;
            this.EventParam = paramValue;
            this.AddEventCommand = new DelegateCommand<object>(this.OnAddEvent, this.CanAddEvent);
        }
        #region Свойства
        
        public ICommand AddEventCommand { get; private set; }

        public EventWithParamListViewModel EventWithParamListViewModel { get; set; }

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
                    EventWithParamListViewModel.UpdateEvents();
                }
            }
        }

        private decimal _probability;
        public decimal Probability
        {
            get
            {
                return _probability;
            }
            set
            {
                if (value != this._probability)
                {
                    this._probability = value;
                    RaisePropertyChanged("Probability");
                    EventWithParamListViewModel.UpdateEvents();
                    EventWithParamListViewModel.Sum();
                }
            }
        }

        private EventParam _eventParam;
        public EventParam EventParam
        {
            get
            {
                return _eventParam;
            }
            set
            {
                if (value != this._eventParam)
                {
                    this._eventParam = value;
                    RaisePropertyChanged("EventParam");
                    EventWithParamListViewModel.UpdateEvents();
                }
            }
        }
        #endregion

        #region Методы
        private void OnAddEvent(object obj)
        {
            this.EventWithParamListViewModel.AddEvent(new Event { Name = this.Name, Probability = this.Probability}, new EventParam { Value = this._eventParam.Value });
        }

        private bool CanAddEvent(object obj)
        {
            return ErrorCount.EntityErrorCount == 0;
        }
        
        #endregion

        #region Реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            errormsg = "Введите название события";
                        break;
                    case "Probability":
                        {
                            if (Probability > 1)
                                errormsg = "Вероятность не должна превышать 1";
                            if (Probability == 0)
                                errormsg = "Введите вероятность.";
                        }
                        break;
                    case "EventParamsValue":
                        {
                            if (string.IsNullOrEmpty(Name))
                                errormsg = "Введите название параметра";
                        }
                        break;
                }
                return errormsg;
            }
        }
        #endregion
    }
}