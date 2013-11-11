using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_5
{
    public partial class PageEvents
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private EventsDependingActionViewModel _eventsDependingActionViewModel;
        private EventsDependingActionListViewModel _eventsDependingActionListViewModel;

        private void BindElements()
        {
            DatagridEv.ItemsSource = _eventsDependingActionListViewModel.EventsDependingActions;
            _eventsDependingActionViewModel = new EventsDependingActionViewModel(_baseLayer,
                                                                                          _eventsDependingActionListViewModel);
            EventsDependingActionControl.DataContext = _eventsDependingActionViewModel;
        }

        public PageEvents(BaseLayer baseLayer, EventsDependingActionListViewModel eventsDependingActionListViewModel)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _eventsDependingActionListViewModel = eventsDependingActionListViewModel;
            BindElements();
        }

        private void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        private void NavigatePage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _navigation.Navigate(new PageCombinations(_baseLayer, _eventsDependingActionListViewModel));
            ErrorCount.EntityErrorCount = 0;
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate(new PageActions(_baseLayer, _eventsDependingActionListViewModel));
        }
    }
}
