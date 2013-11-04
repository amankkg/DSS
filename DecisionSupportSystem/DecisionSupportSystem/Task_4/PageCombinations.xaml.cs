using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_4
{
    public partial class PageCombinations : Page
    {  
        private PagePattern pagePattern = new PagePattern(); // ссылка на шаблон, который хранит общие функции и поля,
        // которые могут использоваться любой страницей 
        private NavigationService navigation;
        private Task4CombinationsView localTaskLayer;

        public PageCombinations(BaseLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseLayer = taskLayer;
            localTaskLayer = new Task4CombinationsView(taskLayer);
            localTaskLayer.CreateCombinations();
            GrdCombinsLst.ItemsSource = localTaskLayer.Temps;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
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
            navigation.Navigate(new PageSolve(pagePattern.baseLayer, localTaskLayer));
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageEvents(pagePattern.baseLayer));
        }
    }
}
