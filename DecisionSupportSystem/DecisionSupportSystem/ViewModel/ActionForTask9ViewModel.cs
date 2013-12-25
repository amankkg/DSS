using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DecisionSupportSystem.CommonClasses;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionForTask9ViewModel : BasePropertyChanged
    {
        public EventViewModel EventViewModel { get; set; }
        public Task Task { get; set; }
        private bool _isExtended;
        public bool IsExtended
        {
            get
            {
                return _isExtended;
            }
            set
            {
                if (value != this._isExtended)
                {
                    this._isExtended = value;
                    RaisePropertyChanged("IsExtended");
                }
            }
        }
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
                    this.EditableAction.Name = value;
                }
            }
        }
        private double _credit;
        public double Credit
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
                    EditableAction.ActionParams.ToList()[1].Value = value;
                    RaisePropertyChanged("Credit");
                }
            }
        }
        private double _creditToExtend;
        public double CreditToExtend
        {
            get
            {
                return _creditToExtend;
            }
            set
            {
                if (value != this._creditToExtend)
                {
                    this._creditToExtend = value;
                    EditableAction.ActionParams.ToList()[3].Value = value;
                    RaisePropertyChanged("CreditToExtend");
                }
            }
        }
        public ActionsForTask9ViewModel ActionsForTask9ViewModel { get; set; }
        public ICommand AddActionCommand { get; set; }

        public ActionForTask9ViewModel(Action actionTemplate, ActionsForTask9ViewModel actionsViewModel, IErrorCatch errorCatcher)
        {
            this.ActionsForTask9ViewModel = actionsViewModel;
            this.EditableAction = actionTemplate;
            this.Name = actionTemplate.Name;
            this.AddActionCommand = new DelegateCommand<object>(this.OnAddAction);
            Credit = actionTemplate.ActionParams.ToList()[1].Value;
            CreditToExtend = actionTemplate.ActionParams.ToList()[3].Value;
            base.ErrorCatcher = errorCatcher;
        }

        public virtual void OnAddAction(object obj)
        {
            if (base.ErrorCatcher.EntityErrorCount != 0) return;
            int period = Convert.ToInt32(Task.TaskParams.ToList()[0].Value);
            AddExtensions(IsExtended ? period : 1, period);
        }

        public void AddExtensions(int iter, int period)
        {

            for (int i = 0; i < iter; i++)
            {
                var actionParams = new Collection<ActionParam>();
                if (i == 0)
                {
                    EditableAction.ActionParams.ToList()[0].Value = period - i;
                    EditableAction.ActionParams.ToList()[2].Value = i;
                    EditableAction.ActionParams.ToList()[4].Value = EventViewModel.EditableEventParams.Count;
                    EditableAction.ActionParams.ToList()[5].Value = -1;
                    foreach (var actionParam in EditableAction.ActionParams)
                        actionParams.Add(new ActionParam
                            {
                                Action = actionParam.Action,
                                Value = actionParam.Value,
                                ActionId = actionParam.ActionId,
                                Id = actionParam.Id,
                                ActionParamName = actionParam.ActionParamName
                            });
                    ActionsForTask9ViewModel.AddAction(new Action
                        {
                            Name = EditableAction.Name,
                            SavingId = EditableAction.SavingId,
                            Emv = EditableAction.Emv,
                            Eol = EditableAction.Eol,
                            ActionParams = actionParams
                        });
                    EventViewModel.EditableEvent.EventParams.Add(new EventParam
                        {
                            EventParamName =
                                new EventParamName
                                    {
                                        Name = "Ожид. доход: '" + EditableAction.Name + "'"
                                    },
                            Value = 0
                        });
                    if (iter > 1)
                        EventViewModel.EditableEvent.EventParams.Add(
                            new EventParam
                    {
                        EventParamName = new EventParamName { Name = "Ожид. доход: '" + EditableAction.Name + " с расширением'" },
                        Value = 0
                    });
                    EventViewModel.UpdateEventParams();
                }
                else
                {
                    EditableAction.ActionParams.ToList()[0].Value = period - i;
                    EditableAction.ActionParams.ToList()[2].Value = i;
                    EditableAction.ActionParams.ToList()[4].Value = EventViewModel.EditableEventParams.Count-2;
                    EditableAction.ActionParams.ToList()[5].Value = EventViewModel.EditableEventParams.Count-1;
                    foreach (var actionParam in EditableAction.ActionParams)
                        actionParams.Add(new ActionParam
                        {
                            Action = actionParam.Action,
                            Value = actionParam.Value,
                            ActionId = actionParam.ActionId,
                            Id = actionParam.Id,
                            ActionParamName = actionParam.ActionParamName
                        });
                    this.ActionsForTask9ViewModel.AddAction(new Action
                        {
                            Name = EditableAction.Name + " с расширением через " + PeriodToString(i),
                            SavingId = EditableAction.SavingId,
                            Emv = EditableAction.Emv,
                            Eol = EditableAction.Eol,
                            ActionParams = actionParams
                        });
                }
            }
        }

        private string PeriodToString(int period)
        {
            if (period%10 == 1 && (period < 10 || period > 20))
                return period.ToString() + " год";
            if (period%10 > 1 && period%10 < 5 && (period < 10 || period > 20))
                return period.ToString() + " года";
            return period.ToString() + " лет";
        }
    }
}
