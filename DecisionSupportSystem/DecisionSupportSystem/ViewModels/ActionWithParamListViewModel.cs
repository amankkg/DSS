using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionWithParamListViewModel : BasePropertyChanged
    {
        public ActionWithParamListViewModel(BaseLayer baseLayer)
        {
            Actions = baseLayer.DssDbContext.Actions.Local;
            ActionParams = baseLayer.DssDbContext.ActionParams.Local;
            _baseLayer = baseLayer;
            ActionWithParamViewModels = new ObservableCollection<ActionWithParamViewModel>();
            foreach (var act in Actions)
            {
                ActionWithParamViewModels.Add(new ActionWithParamViewModel(act, act.ActionParams.ToList()[0], this));
            }
        }

        #region Свойства
        public ObservableCollection<Action> Actions { get; set; }
        public ObservableCollection<ActionParam> ActionParams { get; set; }
        private BaseLayer _baseLayer;

        public ObservableCollection<ActionWithParamViewModel> ActionWithParamViewModels
        {
            get
            {
                return _actionWithParamViewModels;
            }
            set
            {
                if (value != this._actionWithParamViewModels)
                {
                    this._actionWithParamViewModels = value;
                    RaisePropertyChanged("ActionWithParamViewModels");
                }
            }
        }

        private ObservableCollection<ActionWithParamViewModel> _actionWithParamViewModels;

        #endregion

        #region Методы

        public void SelectAction(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                _selectedItem = FindIndexInEventListViewModels((ActionWithParamViewModel)e.AddedItems[0]);
        }

        public void AddAction(Action act, ActionParam actionParam)
        {
            ActionWithParamViewModels.Add(new ActionWithParamViewModel(act, actionParam, this));
            Actions.Add(act);
            actionParam.Action = act;
            ActionParams.Add(actionParam);
        }

        public void UpdateActions()
        {
            if (ActionWithParamViewModels.Count == Actions.Count)
                for (int i = 0; i < Actions.Count; i++)
                    Actions[i].Name = ActionWithParamViewModels[i].Name;
        }

        private int _selectedItem = -1;

        public void DeleteAction(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                ActionWithParamViewModels.RemoveAt(_selectedItem);
                _baseLayer.BaseMethods.DeleteAction(Actions[_selectedItem]);
                UpdateActions();
            }
        }

        private int FindIndexInEventListViewModels(ActionWithParamViewModel element)
        {
            for (int i = 0; i < ActionWithParamViewModels.Count; i++)
                if (ActionWithParamViewModels[i] == element)
                    return i;
            return -1;
        }

        #endregion
    }
}
