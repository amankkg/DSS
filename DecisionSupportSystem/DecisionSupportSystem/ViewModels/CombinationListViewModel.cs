using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModels
{
    public class CombinationListViewModel : BasePropertyChanged
    {
        public CombinationListViewModel(BaseLayer baseLayer)
        {
            Combinations = baseLayer.DssDbContext.Combinations.Local;
            CombinationViewModels = new ObservableCollection<CombinationViewModel>();
            foreach (var combination in Combinations)
            {
                CombinationViewModels.Add(new CombinationViewModel(combination.Action, combination.Event, combination, this));
            }
        }

        private ObservableCollection<CombinationViewModel> _combinationViewModels;
        public ObservableCollection<CombinationViewModel> CombinationViewModels
        {
            get
            {
                return _combinationViewModels;
            }
            set
            {
                if (value != this._combinationViewModels)
                {
                    this._combinationViewModels = value;
                    RaisePropertyChanged("CombinationViewModels");
                }
            }
        }
        
        public ObservableCollection<Combination> Combinations { get; set; }

       

    }
}
 