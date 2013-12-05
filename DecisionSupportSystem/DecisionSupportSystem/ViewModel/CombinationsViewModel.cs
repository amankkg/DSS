using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class CombinationsViewModel : BasePropertyChanged
    {   
        public ObservableCollection<CombinationViewModel> CombinationViewModels { get; set; }
        public ObservableCollection<Combination> Combinations { get; set; }

        public CombinationsViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            Combinations = baseLayer.DssDbContext.Combinations.Local;
            CombinationViewModels = new ObservableCollection<CombinationViewModel>();
            ErrorCatcher = errorCatcher;
            foreach (var combination in Combinations)
                CombinationViewModels.Add(new CombinationViewModel(combination, this));
        }
    }
}
