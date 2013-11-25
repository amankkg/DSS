using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_8
{
    public partial class PageEvents
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private EventWithParamListViewModel _eventWithParamListViewModel;

        #region Конструктор

        private void BindElements()
        {
            _eventWithParamListViewModel = new EventWithParamListViewModel(_baseLayer);
            EventListControl.DataContext = _eventWithParamListViewModel;
            EventControl.DataContext = new EventWithParamViewModel(_eventWithParamListViewModel);
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
            if (_eventWithParamListViewModel.EventWithParamViewModels.Count > 0)
            {
                _navigation.Navigate(new PageCombinations(_baseLayer));
                ErrorCount.EntityErrorCount = 0;
            }
        }

        #endregion


    }
}
