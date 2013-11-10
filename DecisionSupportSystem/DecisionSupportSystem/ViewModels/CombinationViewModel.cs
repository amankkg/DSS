using System;
using System.ComponentModel;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class CombinationViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public CombinationViewModel(Action act, Event ev, Combination comb, CombinationListViewModel combinationListViewModel)
        {
            CombinationListViewModel = combinationListViewModel;
            this.Action = act;
            this.Event = ev;
            this.Combination = comb;
        }

        public CombinationListViewModel CombinationListViewModel { get; set; }

        private Action _action;
        public Action Action
        {
            get
            {
                return _action;
            }
            set
            {
                if (value != this._action)
                {
                    this._action = value;
                    RaisePropertyChanged("Action");
                }
            }
        }

        private Event _event;
        public Event Event
        {
            get
            {
                return _event;
            }
            set
            {
                if (value != this._event)
                {
                    this._event = value;
                    RaisePropertyChanged("Event");
                }
            }
        }

        private Combination _conmbination;
        public Combination Combination
        {
            get
            {
                return _conmbination;
            }
            set
            {
                if (value != this._conmbination)
                {
                    this._conmbination = value;
                    RaisePropertyChanged("Combination");
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
                    case "Action":
                        if (string.IsNullOrEmpty(Action.Name))
                            errormsg = "Введите название действия";
                        break;
                    case "Event":
                        if (string.IsNullOrEmpty(Event.Name))
                            errormsg = "Введите название события";
                        break;
                }
                return errormsg;
            }
        }
        #endregion

    }
}
