using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionListViewModel : BasePropertyChanged
    {
        public ActionListViewModel(BaseLayer baseLayer)
        {
            Actions = baseLayer.DssDbContext.Actions.Local;
            _baseLayer = baseLayer;
            ActionViewModels = new ObservableCollection<ActionViewModel>();
            foreach (var act in Actions)
            {
                ActionViewModels.Add(new ActionViewModel(act, this));
            }
        }

        #region Свойства
        public ObservableCollection<Action> Actions { get; set; }
        private BaseLayer _baseLayer;

        public ObservableCollection<ActionViewModel> ActionViewModels
        {
            get
            {
                return _actionViewModels;
            }
            set
            {
                if (value != this._actionViewModels)
                {
                    this._actionViewModels = value;
                    RaisePropertyChanged("ActionViewModels");
                }
            }
        }

        private ObservableCollection<ActionViewModel> _actionViewModels;

        #endregion

        #region Методы

        public void SelectAction(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                _selectedItem = FindIndexInEventListViewModels((ActionViewModel)e.AddedItems[0]);
        }

        public void AddAction(Action act)
        {
            var haveThisActInActions = Actions.Any(a => a.Name.Trim() == act.Name.Trim());
            if (haveThisActInActions) return;
            ActionViewModels.Add(new ActionViewModel(act, this));
            Actions.Add(act);
            
        }

        public void UpdateAction(ActionViewModel callActionViewModel)
        {
            if (ActionViewModels.Count != Actions.Count || !ActionViewModels.Contains(callActionViewModel)) return;
            int index = ActionViewModels.IndexOf(callActionViewModel);
            RenameSimilarActs(callActionViewModel);
            Actions[index].Name = callActionViewModel.Name;
            NavigationWindowShower.IsSaved = false;
        }

        void RenameSimilarActs(ActionViewModel callActionViewModel)
        {
            var simactslist = SearchSimilarActs(callActionViewModel.Name.Trim()).ToList();
            foreach (var action in simactslist)
            {
                string name = callActionViewModel.Name;
                ActionViewModels[Actions.IndexOf(action)].Name = name + "*";
                action.Name = name + "*";
            }
        }

        IEnumerable<Action> SearchSimilarActs(string actname)
        {
            return Actions.Where(a => a.Name.Trim() == actname).Select(a => a).ToList();
        }

        public void UpdateAllActions()
        {
            if (ActionViewModels.Count == Actions.Count)
                for (int i = 0; i < Actions.Count; i++)
                    Actions[i].Name = ActionViewModels[i].Name;
        }

        private int _selectedItem = -1;

        public void DeleteAction(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                ActionViewModels.RemoveAt(_selectedItem);
                _baseLayer.BaseMethods.DeleteAction(Actions[_selectedItem]);
                UpdateAllActions();
                NavigationWindowShower.IsSaved = false;
            }
        }

        private int FindIndexInEventListViewModels(ActionViewModel element)
        {
            for (int i = 0; i < ActionViewModels.Count; i++)
                if (ActionViewModels[i] == element)
                    return i;
            return -1;
        }

        #endregion
    }
}
