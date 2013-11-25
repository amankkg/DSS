using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_8
{
    public partial class PageCombinations : Page
    {
        private BaseLayer _baseLayer;
        private NavigationService navigation;
        private CombinationWithParamListViewModel _combinationListViewModel;

        public PageCombinations(BaseLayer baseLayer)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            LocalTaskLayer.taskParams.Task = baseLayer.Task;
            grid_taskParam.DataContext = LocalTaskLayer.taskParams;
            LocalTaskLayer.CreateCombinations(_baseLayer);
            _combinationListViewModel = new CombinationWithParamListViewModel(_baseLayer);
            CombinationListControl.DataContext = _combinationListViewModel;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
            LocalTaskLayer.CreateCombinations(_baseLayer);
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageEvents(_baseLayer));
        }

        #region Обработка событий Validation.Error

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           
            if (ErrorCount.EntityErrorCount == 0)
            { 
                LocalTaskLayer.taskParams.InitTask();
                navigation = NavigationService.GetNavigationService(this);
                navigation.Navigate(new PageSolve(_baseLayer));
            }
        }
        #endregion

        private void Validation(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
