using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_1
{
    public partial class PageEvents
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private EventListViewModel _eventListViewModel;

        #region Конструктор

        private void BindElements()
        {
            _eventListViewModel = new EventListViewModel(_baseLayer);
            EventListControl.DataContext = _eventListViewModel;
            EventControl.DataContext = new EventViewModel(_eventListViewModel);
        }

        public PageEvents(BaseLayer baseLayer)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            BindElements();
        }

        #endregion

        private void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate(new PageActions(_baseLayer));
        }

        #region Обработка события Validation.Error

        private void NavigatePage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }
        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_eventListViewModel.EventViewModels.Count > 0)
            {
                //_navigation.Navigate(new PageCombinations(_baseLayer));
                ErrorCount.EntityErrorCount = 0;
            }
        }

        #endregion


    }
}
