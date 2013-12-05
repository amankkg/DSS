using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.ViewModels
{
    public class ActionListViewModelForTask9
    { 
        private BaseLayer _baseLayer;
        
        public ObservableCollection<Action> Actions { get; set; }

        public ObservableCollection<ActionViewModelForTask9> ActionViewModels { get; set; }

        public ActionListViewModelForTask9(BaseLayer baseLayer)
        {
            _baseLayer = baseLayer;
            Actions = _baseLayer.DssDbContext.Actions.Local;
            ActionViewModels = new ObservableCollection<ActionViewModelForTask9>();
            foreach (var act in Actions)
            {
               // ActionViewModels.Add(new ActionViewModelForTask9(act));
            }
        }
    }
}
