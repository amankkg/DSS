using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class ActionsWithExtensionsViewModel : ActionsViewModel
    {
        public ObservableCollection<ActionWithExtensionViewModel> ActionWithExtensionViewModels { get; set; }

        public ActionsWithExtensionsViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            base.ErrorCatcher = errorCatcher;
            base.BaseLayer = baseLayer;
            base.Actions = base.BaseLayer.DssDbContext.Actions.Local;
            this.ActionWithExtensionViewModels = new ObservableCollection<ActionWithExtensionViewModel>();
            foreach (var action in base.Actions)
                this.ActionWithExtensionViewModels.Add(new ActionWithExtensionViewModel(action, this, base.ErrorCatcher));
        }

        public void AddActions(List<Action> addedActions)
        {
            if (addedActions.Count == 0) return;
            var thisActionsHaveAct = Actions.Any(a => a.Name.Trim() == addedActions[0].Name.Trim());
            if (thisActionsHaveAct) return;
            foreach (var addedAction in addedActions)
            {
                ActionViewModels.Add(new ActionWithExtensionViewModel(addedAction, this, base.ErrorCatcher));
                Actions.Add(addedAction);
            }
        }

        public void SelectAction(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                try
                {
                    _selectedAction = FindIndexInActionViewModels((ActionWithExtensionViewModel)e.AddedItems[0]);
                }
                catch
                {
                    _selectedAction = OUT_OF_RANGE;
                }
        }

        private int FindIndexInActionViewModels(ActionWithExtensionViewModel element)
        {
            for (int i = 0; i < ActionViewModels.Count; i++)
                if (ActionWithExtensionViewModels[i] == element)
                    return i;
            return OUT_OF_RANGE;
        }

        public void DeleteAction(object sender, RoutedEventArgs e)
        {
            if (_selectedAction <= OUT_OF_RANGE || Actions.Count == 0) return;
            ActionWithExtensionViewModels.RemoveAt(_selectedAction);
            BaseLayer.BaseMethods.DeleteAction(Actions[_selectedAction]);
        }

        public void UpdateAction(ActionWithExtensionViewModel callActionViewModel)
        {
            if (ActionWithExtensionViewModels.Count != Actions.Count || !ActionWithExtensionViewModels.Contains(callActionViewModel)) return;
            int index = ActionWithExtensionViewModels.IndexOf(callActionViewModel);
            RenameSimilarActs(callActionViewModel);
            Actions[index].Name = callActionViewModel.EditableAction.Name;
        }

        void RenameSimilarActs(ActionWithExtensionViewModel callActionViewModel)
        {
            var simactslist = SearchSimilarActs(callActionViewModel.Name.Trim()).ToList();
            foreach (var action in simactslist)
            {
                string name = callActionViewModel.Name;
                ActionWithExtensionViewModels[Actions.IndexOf(action)].Name = name + "*";
                action.Name = name + "*";
            }
        }

        IEnumerable<Action> SearchSimilarActs(string actname)
        {
            return Actions.Where(a => a.Name.Trim() == actname).Select(a => a).ToList();
        }
    }
}
