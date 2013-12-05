using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class EventDepActionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public EventDepActionViewModel(BaseLayer baseLayer, EventDepActionViewModel eventDepActionsViewModel)
        {
            var ev = new Event();
            this.Name = ev.Name;
            this.Probability = ev.Probability;
            Actions = baseLayer.DssDbContext.Actions.Local.ToList();
            EventsWithActionListViewModel = eventDepActionsViewModel;
            AddEventCommand = new DelegateCommand<object>(this.OnAddEvent, this.CanAddEvent);
        }

        public EventDepActionViewModel EventsWithActionListViewModel { get; set; }

        public ICommand AddEventCommand { get; private set; }

        private void OnAddEvent(object obj)
        {
            this.EventsWithActionListViewModel.AddEvent(
                Actions[ActionSelectedIndex], 
                new Event
                    {
                        Name = this.Name, Probability = this.Probability
                    });
        }

        private bool CanAddEvent(object obj)
        {
            return ErrorCount.EntityErrorCount == 0;
        }
        

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
        
        private List<Action> _actions;
        public List<Action> Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                if (value != this._actions)
                {
                    this._actions = value;
                    RaisePropertyChanged("Actions");
                }
            }
        }

        public Event Event { get; set; }

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
                }
            }
        }
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