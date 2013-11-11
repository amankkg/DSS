using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_8
{
    public partial class PageCombinations : Page
    { 
       /* private PagePattern pagePattern = new PagePattern(); // ссылка на шаблон, который хранит общие функции и поля,
        // которые могут использоваться любой страницей 
        private NavigationService navigation;  */

        public PageCombinations(BaseLayer taskLayer)
        {
           /* InitializeComponent();
            pagePattern.baseLayer = taskLayer;
            GrdCombinsLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Combinations.Local;*/
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
           /* pagePattern.baseLayer.CreateCombinForFirstType();
            GrdCombinsLst.Items.Refresh();*/
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
           /* pagePattern.DatagridValidationError(e);*/
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
           /* pagePattern.NavigatePageCanExecute(e);*/
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageSolve(pagePattern.baseLayer));*/
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
           /* pagePattern.NavigatePageCanExecute(e);*/
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
          /*  navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageEvents(pagePattern.baseLayer));*/
        }
    }
}
