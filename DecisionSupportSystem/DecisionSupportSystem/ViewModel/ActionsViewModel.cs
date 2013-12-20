using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionsViewModel : BasePropertyChanged
    {
        protected const int OUT_OF_RANGE = -1;
        public BaseLayer BaseLayer;
        public ObservableCollection<Action> Actions { get; set; }
        public ObservableCollection<ActionViewModel> ActionViewModels { get; set; }
        public Visibility ParamsVisibility { get; set; }
        public ActionsViewModel(){}

        public ActionsViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            base.ErrorCatcher = errorCatcher;
            this.BaseLayer = baseLayer;
            this.Actions = this.BaseLayer.DssDbContext.Actions.Local;
            this.ActionViewModels = new ObservableCollection<ActionViewModel>();
            foreach (var action in this.Actions)
                this.ActionViewModels.Add(new ActionViewModel(action, this, base.ErrorCatcher));
        }

        public virtual void AddAction(Action act)
        {
            var thisActionsHaveAct = Actions.Any(a => a.Name.Trim() == act.Name.Trim());
            if (thisActionsHaveAct) return;
            ActionViewModels.Add(new ActionViewModel(act, this, base.ErrorCatcher));
            Actions.Add(act);
        }

        protected int _selectedAction = OUT_OF_RANGE;
        public void SelectAction(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                try {
                    _selectedAction = FindIndexInActionViewModels((ActionViewModel) e.AddedItems[0]);
                    }
                catch {
                    _selectedAction = OUT_OF_RANGE;
                      }
        }
        
        private int FindIndexInActionViewModels(ActionViewModel element)
        {
            for (int i = 0; i < ActionViewModels.Count; i++)
                if (ActionViewModels[i] == element)
                    return i;
            return OUT_OF_RANGE;
        }

        public void DeleteAction(object sender, RoutedEventArgs e)
        {
            if (_selectedAction <= OUT_OF_RANGE || Actions.Count == 0) return;
            ActionViewModels.RemoveAt(_selectedAction);
            BaseLayer.BaseMethods.DeleteAction(Actions[_selectedAction]);
        }

        public void UpdateAction(ActionViewModel callActionViewModel)
        {
            if (ActionViewModels.Count != Actions.Count || !ActionViewModels.Contains(callActionViewModel)) return;
            int index = ActionViewModels.IndexOf(callActionViewModel);
            RenameSimilarActs(callActionViewModel);
            Actions[index].Name = callActionViewModel.EditableAction.Name;
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
    }
}
