using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_5
{
    public partial class PageSolve
    {
        private BaseLayer _baseLayer; 
        private NavigationService _navigation;
        private LocalTaskLayer _localTaskLayer;
        private EventsDependingActionListViewModel _eventsDependingActionListViewModel;

        private void BindElements()
        {
            GrdSolutionLst.ItemsSource = _baseLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = _baseLayer.SolvedTaskView;
        }

        public PageSolve(BaseLayer baseLayer, LocalTaskLayer localTaskLayer,
            EventsDependingActionListViewModel eventsDependingActionListViewModel)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _localTaskLayer = localTaskLayer;
            _eventsDependingActionListViewModel =
                eventsDependingActionListViewModel;
            BindElements();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            _localTaskLayer.SolveCp();
            _baseLayer.SolveThisTask(_localTaskLayer.FictiveCombinationsList);
            GrdSolutionLst.Items.Refresh();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageCombinations(_baseLayer, _eventsDependingActionListViewModel));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _baseLayer.Save();
        }
    }
}
