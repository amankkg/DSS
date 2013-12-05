using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public Action EditableAction { get; set; }
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
                    ActionsViewModel.UpdateAction(this);
                    this.EditableAction.Name = value;
                }
            }
        }
        public ObservableCollection<ActionParam> EditableActionParams { get; set; }
        public ActionsViewModel ActionsViewModel { get; set; }
        public ICommand AddActionCommand { get; private set; }

        public ActionViewModel(Action actionTemplate, ActionsViewModel actionsViewModel, IErrorCatch errorCatcher)
        {
            this.ActionsViewModel = actionsViewModel;
            this.EditableAction = actionTemplate;
            this.Name = actionTemplate.Name;
            this.EditableActionParams = new ObservableCollection<ActionParam>();
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction);
            var actionParams = this.EditableAction.ActionParams.ToList();
            foreach (var actionParam in actionParams)
                this.EditableActionParams.Add(actionParam);
            base.ErrorCatcher = errorCatcher;
        }

        public void OnAddAction(object obj)
        {
            if (base.ErrorCatcher.EntityErrorCount != 0) return;
            var actionParams = new Collection<ActionParam>();
            foreach (var actionParam in EditableAction.ActionParams)
                actionParams.Add(new ActionParam {
                        Action = actionParam.Action,
                        Value = actionParam.Value,
                        ActionId = actionParam.ActionId,
                        Id = actionParam.Id,
                        ActionParamName = actionParam.ActionParamName
                });
            this.ActionsViewModel.AddAction(new Action
                {
                    Name = EditableAction.Name, 
                    SavingId = EditableAction.SavingId, 
                    Emv = EditableAction.Emv,
                    Eol = EditableAction.Eol,
                    ActionParams = actionParams
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
                        errormsg = "Введите название действия";
                    }
                }
                return errormsg;
            }
        }
        #endregion
    } 
}
