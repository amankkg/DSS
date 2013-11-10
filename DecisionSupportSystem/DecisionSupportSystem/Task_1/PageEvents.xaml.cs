using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
//using BaseModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.Task_5;


namespace DecisionSupportSystem.Task_1
{ 
    public partial class PageEvents
    {
        private readonly BaseLayer _baseLayer;

        private NavigationService _navigation;
        private Event eEvent = new Event();

        private ObservableCollection<MainEvetListViewModel> EventList =
            new ObservableCollection<MainEvetListViewModel>();

        private MainEvetListViewModel _mainEvetListViewModal;
        #region Конструкторы

        private void Init()
        {
            gridEvent.DataContext = eEvent;
            _mainEvetListViewModal = new MainEvetListViewModel(_baseLayer.DssDbContext.Events.Local);
            EventC.DataContext = _mainEvetListViewModal;
        }

        public PageEvents(BaseLayer taskLayer)
        {
            InitializeComponent();
            _baseLayer = taskLayer;
            Init();
        }

        #endregion

        private void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        #region Обработка события Validation.Error
        private void EventAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityErrorCount == 0;
            e.Handled = true;
        }
        private void EventAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _mainEvetListViewModal.AddEvent(new Event
            {
                Name = eEvent.Name,
                Probability = eEvent.Probability
            });
        }
        private void EventValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
        private void NavigatePage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }
        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _navigation.Navigate(new PageCombinations(_baseLayer));
            ErrorCount.EntityErrorCount = 0;
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _navigation.Navigate(new PageActions(_baseLayer));
            ErrorCount.EntityErrorCount = 0;
        }
        #endregion
    }

}
