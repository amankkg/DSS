using System.Collections.ObjectModel;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class CombinationsViewModel : BasePropertyChanged
    {   
        public ObservableCollection<CombinationViewModel> CombinationViewModels { get; set; }
        public ObservableCollection<Combination> Combinations { get; set; }
        public Visibility ParamsVisibility { get; set; }
        public CombinationsViewModel(ObservableCollection<Combination> combinations, IErrorCatch errorCatcher)
        {
            Combinations = combinations;
            CombinationViewModels = new ObservableCollection<CombinationViewModel>();
            ErrorCatcher = errorCatcher;
            foreach (var combination in Combinations)
                CombinationViewModels.Add(new CombinationViewModel(combination, this));
        }
    }
}
