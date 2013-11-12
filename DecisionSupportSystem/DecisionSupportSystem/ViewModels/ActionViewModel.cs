using System;
using System.ComponentModel;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public ActionViewModel(Action action, ActionListViewModel actionListViewModel)
        {
            this.ActionListViewModel = actionListViewModel;
            this.Name = action.Name;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction,this.CanAddAction);
        }

        public ActionViewModel(ActionListViewModel actionListViewModel)
        {
            var action = new Action();
            this.ActionListViewModel = actionListViewModel;
            this.Name = action.Name;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction, this.CanAddAction);
        }

        #region Свойства

        public ActionListViewModel ActionListViewModel { get; set; }

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
                    ActionListViewModel.UpdateActions();
                }
            }
        }

        #endregion

        #region Методы
        public void OnAddAction(object obj)
        {
            this.ActionListViewModel.AddAction(new Action{Name = this.Name});
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
                }
                return errormsg;
            }
        }
        #endregion
    }
}
