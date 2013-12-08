using System;
using System.Linq;
using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;


namespace DecisionSupportSystem.ViewModel
{
    public class ActionWithExtensionViewModel : ActionViewModel
    {
        public bool IsExtendable { get; set; }
        public ActionsWithExtensionsViewModel ActionsWithExtensionsViewModel { get; set; }
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

        public ActionWithExtensionViewModel(Action actionTemplate, ActionsWithExtensionsViewModel actionsWithExtensionsViewModel, IErrorCatch errorCatcher)
        {
            this.ActionsWithExtensionsViewModel = actionsWithExtensionsViewModel;
            base.EditableAction = actionTemplate;
            this.Name = actionTemplate.Name;
            base.EditableActionParams = new ObservableCollection<ActionParam>();
            base.AddActionCommand = new DelegateCommand<object>(this.OnAddAction);
            var actionParams = base.EditableAction.ActionParams.ToList();
            foreach (var actionParam in actionParams)
                this.EditableActionParams.Add(actionParam);
            base.ErrorCatcher = errorCatcher;
        }

        public override void OnAddAction(object obj)
        {
            if (base.ErrorCatcher.EntityErrorCount != 0) return;

            var actionParams = new Collection<ActionParam>();
            foreach (var actionParam in EditableAction.ActionParams)
                actionParams.Add(new ActionParam
                {
                    Action = actionParam.Action,
                    Value = actionParam.Value,
                    ActionId = actionParam.ActionId,
                    Id = actionParam.Id,
                    ActionParamName = actionParam.ActionParamName
                });
            var action = new Action
            {
                Name = EditableAction.Name,
                SavingId = EditableAction.SavingId,
                Emv = EditableAction.Emv,
                Eol = EditableAction.Eol,
                ActionParams = actionParams
            };
            this.ActionsViewModel.AddAction(action);
            
            decimal obsPeriod = ActionsWithExtensionsViewModel.BaseLayer.Task.TaskParams.ToList()[0].Value;
            for (int i = (int)obsPeriod; i > 0; i++)
            {
                action.ActionParams.ToList()[0].Value = i;
                action.ActionParams.ToList()[1].Value = i;
            }
            
        }

    }
}
