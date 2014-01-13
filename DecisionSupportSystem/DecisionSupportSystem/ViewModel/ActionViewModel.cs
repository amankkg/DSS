using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public Action EditableAction { get; set; }
        public Random Random { get; set; }
        public int randomMax { get; set; }
        private int actionsCount;

        private bool isGenerated;
        public bool IsGenerated
        {
            get
            {
                return isGenerated;
            }
            set
            {
                if (value != this.isGenerated)
                {
                    this.isGenerated = value;
                    RaisePropertyChanged("IsGenerated");
                    if (!isGenerated)
                        ActionsCount = 0;
                }
            }
        }

        public int ActionsCount
        {
            get
            {
                return actionsCount;
            }
            set
            {
                if (value != this.actionsCount)
                {
                    this.actionsCount = value;
                    RaisePropertyChanged("ActionsCount");
                }
            }
        }
        public int Iterator = 0;
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

        private Visibility _paramsVisibility;
        public Visibility ParamsVisibility
        {
            get
            {
                return _paramsVisibility;
            }
            set
            {
                if (value != this._paramsVisibility)
                {
                    this._paramsVisibility = value;
                    RaisePropertyChanged("ParamsVisibility");
                }
            }
        }

        public ObservableCollection<ActionParam> EditableActionParams { get; set; }
        public ActionsViewModel ActionsViewModel { get; set; }
        public ICommand AddActionCommand { get; set; }
      
        public ActionViewModel()
        {
            Random = new Random();
        }

        public ActionViewModel(Action actionTemplate, ActionsViewModel actionsViewModel, IErrorCatch errorCatcher)
        {
            ActionsViewModel = actionsViewModel;
            EditableAction = actionTemplate;
            Name = actionTemplate.Name;
            EditableActionParams = new ObservableCollection<ActionParam>();
            AddActionCommand = new DelegateCommand<object>(this.OnAddAction);
            var actionParams = EditableAction.ActionParams.ToList();
            foreach (var actionParam in actionParams)
                EditableActionParams.Add(actionParam);
            if(EditableActionParams.Count == 0)
                ParamsVisibility = Visibility.Hidden;
            ErrorCatcher = errorCatcher;
            Random = new Random();
        }
        
        public virtual void OnAddAction(object obj)
        {
            if (IsGenerated)
                GenerateActions();
            else
            {
                if (ErrorCatcher.EntityErrorCount != 0) return;
                CreateAndAddAction(EditableAction);
            }
        }

        public void GenerateActions()
        {
            if (ActionsCount <= 0) return;
            while (Iterator != ActionsCount)
            {
                ErrorCatcher.EntityErrorCount = 0;
                EditableAction.Name = String.Format("Действие {0}", Iterator = Iterator + 1);
                CreateAndAddAction(EditableAction);
            }
        }

        public void CreateAndAddAction(Action EditableAction)
        {
            var actionParams = InitActionParams();
            ActionsViewModel.AddAction(new Action{
                    Name = EditableAction.Name, 
                    SavingId = EditableAction.SavingId, 
                    Emv = EditableAction.Emv,
                    Eol = EditableAction.Eol,
                    ActionParams = actionParams
                });
        }

        private Collection<ActionParam> InitActionParams()
        {
            var actionParams = new Collection<ActionParam>();
            foreach (var actionParam in EditableAction.ActionParams)
            {
                var actParam = new ActionParam
                    {
                        Action = actionParam.Action,
                        ActionId = actionParam.ActionId,
                        Id = actionParam.Id,
                        ActionParamName = actionParam.ActionParamName
                    };
                if (IsGenerated) actParam.Value = Random.Next(randomMax);
                else actParam.Value = actionParam.Value;
                actionParams.Add(actParam);
            }
            return actionParams;
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
