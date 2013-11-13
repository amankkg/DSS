using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_5
{
    public partial class PageCombinations
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private LocalTaskLayer _localTaskLayer;
        private EventsDependingActionListViewModel _eventsDependingActionListViewModel;

        public PageCombinations(BaseLayer baseLayer, EventsDependingActionListViewModel eventsDependingActionListViewModel)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _eventsDependingActionListViewModel = eventsDependingActionListViewModel;
            _localTaskLayer = new LocalTaskLayer(baseLayer, eventsDependingActionListViewModel);
            _localTaskLayer.CreateCombinations();
            GrdCombinsLst.ItemsSource = _localTaskLayer.CombinationWithParamViews;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
            GrdCombinsLst.Items.Refresh();
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageSolve(_baseLayer, _localTaskLayer, 
                new EventsDependingActionListViewModel(_baseLayer)));
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageEvents(_baseLayer, _eventsDependingActionListViewModel));
        }
    }
}
