using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_1
{
    public partial class PageCombinations : Page
    { 
        private BaseLayer _baseLayer;
        private NavigationService navigation;

        public PageCombinations(BaseLayer taskLayer)
        {
            InitializeComponent();
            _baseLayer = taskLayer;
            GrdCombinsLst.ItemsSource = _baseLayer.DssDbContext.Combinations.Local;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
            _baseLayer.CreateCombinForFirstType();
            GrdCombinsLst.Items.Refresh();
        }

        #region Обработка событий Validation.Error
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
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageSolve(_baseLayer));
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageEvents(_baseLayer));
        }
        #endregion
    }
}
