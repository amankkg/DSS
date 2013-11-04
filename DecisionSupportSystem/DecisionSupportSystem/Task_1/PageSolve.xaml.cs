using System.Windows;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_1
{
    public partial class PageSolve
    {
        private PagePattern pagePattern = new PagePattern(); 
        private NavigationService navigation;

        private void Init()
        {
            GrdSolutionLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = pagePattern.baseLayer.TaskView;
        }

        public PageSolve(BaseLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseLayer = taskLayer;
            Init();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
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
