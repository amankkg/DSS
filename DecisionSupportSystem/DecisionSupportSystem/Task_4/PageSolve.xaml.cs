using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_4
{
    /// <summary>
    /// Логика взаимодействия для PageSolve.xaml
    /// </summary>
    public partial class PageSolve : Page
    {
        private PagePattern pagePattern = new PagePattern(); 
        private NavigationService navigation;
        private Task4CombinationsView localTaskLayuer;

        private void Init()
        {
            GrdSolutionLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = pagePattern.baseLayer.TaskView;
        }

        public PageSolve(BaseLayer taskLayer, Task4CombinationsView task4CombinationsView)
        {
            InitializeComponent();
            pagePattern.baseLayer = taskLayer;
            localTaskLayuer = task4CombinationsView;
            Init();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            localTaskLayuer.SolveCp();
            pagePattern.baseLayer.SolveWpColWol();
            pagePattern.baseLayer.SolveEmvEol();
            GrdSolutionLst.Items.Refresh();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageCombinations(pagePattern.baseLayer));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            pagePattern.baseLayer.Save();
        }
    }
}
