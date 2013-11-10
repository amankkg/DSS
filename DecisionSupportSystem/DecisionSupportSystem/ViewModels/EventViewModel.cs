using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;

namespace DecisionSupportSystem.ViewModels
{
    public class EventViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public EventViewModel(Event ev, EventListViewModel eventListViewModel)
        {
            this.EventListViewModel = eventListViewModel;
            this.Name = ev.Name;
            this.Probability = ev.Probability;
            this.AddEventCommand = new DelegateCommand<object>(this.OnAddEvent, this.CanAddEvent);
        }

        #region Свойства
        
        public ICommand AddEventCommand { get; private set; }
        
        public EventListViewModel EventListViewModel { get; set; }

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
                    EventListViewModel.UpdateEvents();
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
                    EventListViewModel.UpdateEvents();
                    EventListViewModel.Sum();
                }
            }
        }
        #endregion

        #region Методы
        private void OnAddEvent(object obj)
        {
            this.EventListViewModel.AddEvent(new Event{Name = this.Name, Probability = this.Probability});
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
                }
                return errormsg;
            }
        }
        #endregion
    }
}