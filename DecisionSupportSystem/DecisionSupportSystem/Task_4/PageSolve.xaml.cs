using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_4
{
    public partial class PageSolve
    {
        private BaseLayer _baseLayer; 
        private NavigationService navigation;
        private Task4CombinationsView localTaskLayer;

        private void Init()
        {
            GrdSolutionLst.ItemsSource = _baseLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = _baseLayer.SolvedTaskView;
        }

        public PageSolve(BaseLayer baseLayer, Task4CombinationsView task4CombinationsView)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            localTaskLayer = task4CombinationsView;
            Init();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            localTaskLayer.SolveCp();
            _baseLayer.SolveWpColWol();
            _baseLayer.SolveEmvEol();
            GrdSolutionLst.Items.Refresh();
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
