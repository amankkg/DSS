using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModels
{
    public class CombinationWithParamListViewModel : BasePropertyChanged
    {
        public CombinationWithParamListViewModel(BaseLayer baseLayer)
        {
            Combinations = baseLayer.DssDbContext.Combinations.Local;
            CombinationWithParamViewModels = new ObservableCollection<CombinationWithParamViewModel>();
            foreach (var combination in Combinations)
            {
                CombinationWithParamViewModels.Add(new CombinationWithParamViewModel(combination.Action, combination.Event, combination.Action.ActionParams.ToList()[0], combination.Event.EventParams.ToList()[0], this));
            }
        }

        private ObservableCollection<CombinationWithParamViewModel> _combinationWithParamViewModels;
        public ObservableCollection<CombinationWithParamViewModel> CombinationWithParamViewModels
        {
            get
            {
                return _combinationWithParamViewModels;
            }
            set
            {
                if (value != this._combinationWithParamViewModels)
                {
                    this._combinationWithParamViewModels = value;
                    RaisePropertyChanged("CombinationWithParamViewModels");
                }
            }
        }
        
        public ObservableCollection<Combination> Combinations { get; set; }

       

    }
}
 