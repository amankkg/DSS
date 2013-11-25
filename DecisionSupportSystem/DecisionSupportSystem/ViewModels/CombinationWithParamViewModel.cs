using System;
using System.ComponentModel;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class CombinationWithParamViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public CombinationWithParamViewModel(Action act, Event ev, ActionParam actionParam, EventParam eventParam, CombinationWithParamListViewModel combinationWithParamListViewModel)
        {
            CombinationWithParamListViewModel = combinationWithParamListViewModel;
            this.Action = act;
            this.Event = ev;
            this.ActionParam = actionParam;
            this.EventParam = eventParam;
        }

        public CombinationWithParamListViewModel CombinationWithParamListViewModel { get; set; }

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

        private ActionParam _actionParam;
        public ActionParam ActionParam
        {
            get
            {
                return _actionParam;
            }
            set
            {
                if (value != this._actionParam)
                {
                    this._actionParam = value;
                    RaisePropertyChanged("ActionParam");
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
                    case "ActionParam":
                        if (string.IsNullOrEmpty(Event.Name))
                            errormsg = "Введите треб.брак";
                        break;
                    case "EventParam":
                        if (string.IsNullOrEmpty(Event.Name))
                            errormsg = "Введите факт.брак";
                        break;
                }
                return errormsg;
            }
        }
        #endregion

    }
}
