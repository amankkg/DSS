using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.CommonClasses;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionsForTask9ViewModel : BasePropertyChanged
    {
        protected const int OUT_OF_RANGE = -1;
        public BaseLayer BaseLayer;
        public ObservableCollection<Action> Actions { get; set; }
        public ObservableCollection<ActionForTask9ViewModel> ActionForTask9ViewModels { get; set; }
        public Visibility ParamsVisibility { get; set; }
        public ActionsForTask9ViewModel(){}

        public ActionsForTask9ViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            base.ErrorCatcher = errorCatcher;
            this.BaseLayer = baseLayer;
            this.Actions = this.BaseLayer.DssDbContext.Actions.Local;
            this.ActionForTask9ViewModels = new ObservableCollection<ActionForTask9ViewModel>();
            foreach (var action in this.Actions)
                this.ActionForTask9ViewModels.Add(new ActionForTask9ViewModel(action, this, base.ErrorCatcher));
        }

        public virtual void AddAction(Action act)
        {
            var thisActionsHaveAct = Actions.Any(a => a.Name.Trim() == act.Name.Trim());
            if (thisActionsHaveAct) return;
            ActionForTask9ViewModels.Add(new ActionForTask9ViewModel(act, this, base.ErrorCatcher));
            Actions.Add(act);
        }

        protected int _selectedAction = OUT_OF_RANGE;
        public void SelectAction(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                try {
                    _selectedAction = FindIndexInActionViewModels((ActionForTask9ViewModel)e.AddedItems[0]);
                    }
                catch {
                    _selectedAction = OUT_OF_RANGE;
                      }
        }

        private int FindIndexInActionViewModels(ActionForTask9ViewModel element)
        {
            for (int i = 0; i < ActionForTask9ViewModels.Count; i++)
                if (ActionForTask9ViewModels[i] == element)
                    return i;
            return OUT_OF_RANGE;
        }

        public void DeleteAction(object sender, RoutedEventArgs e)
        {
            if (_selectedAction <= OUT_OF_RANGE || Actions.Count == 0) return;
            ActionForTask9ViewModels.RemoveAt(_selectedAction);
            BaseLayer.BaseMethods.DeleteAction(Actions[_selectedAction]);
        }

        public void UpdateAction(ActionForTask9ViewModel callActionViewModel)
        {
            if (ActionForTask9ViewModels.Count != Actions.Count || !ActionForTask9ViewModels.Contains(callActionViewModel)) return;
            int index = ActionForTask9ViewModels.IndexOf(callActionViewModel);
            RenameSimilarActs(callActionViewModel);
            Actions[index].Name = callActionViewModel.EditableAction.Name;
        }

        void RenameSimilarActs(ActionForTask9ViewModel callActionViewModel)
        {
            var simactslist = SearchSimilarActs(callActionViewModel.Name.Trim()).ToList();
            foreach (var action in simactslist)
            {
                string name = callActionViewModel.Name;
                ActionForTask9ViewModels[Actions.IndexOf(action)].Name = name + "*";
                action.Name = name + "*";
            }
        }

        IEnumerable<Action> SearchSimilarActs(string actname)
        {
            return Actions.Where(a => a.Name.Trim() == actname).Select(a => a).ToList();
        }
    }
}
