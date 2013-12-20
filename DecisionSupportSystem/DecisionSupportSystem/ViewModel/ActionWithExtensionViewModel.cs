using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionWithExtensionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        private bool _isChecked;
        public bool IsExtendable
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (value != this._isChecked)
                {
                    this._isChecked = value;
                    RaisePropertyChanged("IsExtendable");
                    ChangedExtendability();
                }
            }
        }
        public Action EditableAction { get; set; }
        public ICommand AddActionCommand { get; set; }
        public ObservableCollection<ActionParam> EditableActionParams { get; set; }
        private Action _selectedAction;
        public Action SelectedAction
        {
            get
            {
                return _selectedAction;
            }
            set
            {
                if (value != this._selectedAction)
                {
                    this._selectedAction = value;
                    RaisePropertyChanged("SelectedAction");
                }
            }
        }

        public ActionsWithExtensionsViewModel ActionsWithExtensionsViewModel { get; set; }
        public ObservableCollection<Action> Actions { get; set; }
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
                    ActionsWithExtensionsViewModel.UpdateAction(this);
                    this.EditableAction.Name = value;
                }
            }
        }

        public void ChangedExtendability()
        {
            EditableAction.ActionParams.ToList()[0].ActionParamName.Name = IsExtendable ? "Расширение через:" : "Срок:";
        }

        public ActionWithExtensionViewModel(Action actionTemplate, ActionsWithExtensionsViewModel actionsWithExtensionsViewModel, IErrorCatch errorCatcher)
        {
            this.ActionsWithExtensionsViewModel = actionsWithExtensionsViewModel;
            this.Actions = actionsWithExtensionsViewModel.Actions;
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
            if (IsExtendable)
            {
                EditableAction.Name = SelectedAction.Name + " с расширением";
                EditableAction.ExtendableAction = SelectedAction;
            }
            var actionParams = new Collection<ActionParam>();
            foreach (var actionParam in EditableAction.ActionParams)
                actionParams.Add(new ActionParam
                {
                    Action = actionParam.Action,
                    Value = actionParam.Value,
                    ActionId = actionParam.ActionId,
                    Id = actionParam.Id,
                    ActionParamName = new ActionParamName { Name = actionParam.ActionParamName.Name }
                });
            var action = new Action
            {
                Name = EditableAction.Name,
                SavingId = EditableAction.SavingId,
                Emv = EditableAction.Emv,
                Eol = EditableAction.Eol,
                ActionParams = actionParams,
                ExtendableAction = EditableAction.ExtendableAction
            };
            this.ActionsWithExtensionsViewModel.AddAction(action);
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
