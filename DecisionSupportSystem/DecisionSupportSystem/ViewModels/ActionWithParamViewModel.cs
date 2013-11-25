using System;
using System.ComponentModel;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionWithParamViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public ActionWithParamViewModel(Action action, ActionParam actionParam,  ActionWithParamListViewModel actionWithParamListViewModel)
        {
            this.ActionWithParamListViewModel = actionWithParamListViewModel;
            this.Name = action.Name;
            this.ActionParam = actionParam;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction,this.CanAddAction);
        }

        public ActionWithParamViewModel(ActionWithParamListViewModel actionWithParamListViewModel)
        {
            var action = new Action();
            var paramValue = new ActionParam();
            this.ActionWithParamListViewModel = actionWithParamListViewModel;
            this.Name = action.Name;
            this.ActionParam = paramValue;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction, this.CanAddAction);
        }

        #region Свойства

        public ActionWithParamListViewModel ActionWithParamListViewModel { get; set; }

        public ICommand AddActionCommand { get; private set; }
        
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
                    ActionWithParamListViewModel.UpdateActions();
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
                    ActionWithParamListViewModel.UpdateActions();
                }
            }
        }
        #endregion

        #region Методы
        public void OnAddAction(object obj)
        {
            this.ActionWithParamListViewModel.AddAction(new Action { Name = this.Name }, new ActionParam { Value = this._actionParam.Value });
        }

        public bool CanAddAction(object obj)
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
                        {
                            if (string.IsNullOrEmpty(Name))
                                errormsg = "Введите название действия";
                        }
                        break;
                     case "ActionParamsValue":
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
