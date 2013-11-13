using System.Windows;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_
{
    public partial class PageSolve
    {
        private BaseLayer _baseLayer; 
        private NavigationService navigation;

        private void BindElements()
        {
            SolutionDataGrid.ItemsSource = _baseLayer.DssDbContext.Actions.Local;
            TaskGrid.DataContext = _baseLayer.SolvedTaskView;
        }

        public PageSolve(BaseLayer taskLayer)
        {
            InitializeComponent();
            _baseLayer = taskLayer;
            BindElements();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            _baseLayer.SolveThisTask();
            SolutionDataGrid.Items.Refresh();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageCombinations(_baseLayer));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _baseLayer.Save();
        }
    }
}
