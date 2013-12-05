using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionViewModelForTask9 : BasePropertyChanged, IDataErrorInfo
    {
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

        private ActionParam _credit;
        public ActionParam Credit
        {
            get
            {
                return _credit;
            }
            set
            {
                if (value != this._credit)
                {
                    this._credit = value;
                    RaisePropertyChanged("Credit");
                }
            }
        }

        private ActionParam _isExtensable;
        public ActionParam IsExtendable
        {
            get
            {
                return _isExtensable;
            }
            set
            {
                if (value != this._isExtensable)
                {
                    this._isExtensable = value;
                    RaisePropertyChanged("IsExtendable");
                }
            }
        }

        private ActionParam _period;
        public ActionParam Period
        {
            get
            {
                return _period;
            }
            set
            {
                if (value != this._period)
                {
                    this._period = value;
                    RaisePropertyChanged("Period");
                }
            }
        }

        public ActionViewModelForTask9 Extension { get; set; }
        public ActionListViewModelForTask9 ActionListViewModel { get; set; }
        public ICommand AddActionCommand { get; private set; }

        public ActionViewModelForTask9(Action action, ActionParam credit, ActionParam isExtensable, ActionParam period, ActionListViewModelForTask9 actionListViewModel)
        {
            this.ActionListViewModel = actionListViewModel;
            this.Name = action.Name;
            this.Credit = credit;
            this.IsExtendable = isExtensable;
            this.Period = period;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction,this.CanAddAction);
        }

        #region Методы
        public void OnAddAction(object obj)
        {
            //this.ActionListViewModel.AddAction(new Action { Name = this.Name });
            NavigationWindowShower.IsSaved = false;
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
