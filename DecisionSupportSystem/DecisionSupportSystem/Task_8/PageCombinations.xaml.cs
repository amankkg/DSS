using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_8
{
    public partial class PageCombinations : Page
    { 
        private PagePattern pagePattern = new PagePattern(); // ссылка на шаблон, который хранит общие функции и поля,
        // которые могут использоваться любой страницей 
        private NavigationService navigation;  

        public PageCombinations(BaseTaskLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseTaskLayer = taskLayer;
            GrdCombinsLst.ItemsSource = pagePattern.baseTaskLayer.DssDbContext.Combinations.Local;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
            pagePattern.baseTaskLayer.CreateCombinForFirstType();
            GrdCombinsLst.Items.Refresh();
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.DatagridValidationError(e);
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageSolve(pagePattern.baseTaskLayer));
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageEvents(pagePattern.baseTaskLayer));
        }
    }
}
