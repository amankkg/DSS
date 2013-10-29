using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_1
{
    /// <summary>
    /// Логика взаимодействия для PageSolve.xaml
    /// </summary>
    public partial class PageSolve
    {
        private PagePattern pagePattern = new PagePattern(); 
        private NavigationService navigation;

        private void Init()
        {
            GrdSolutionLst.ItemsSource = pagePattern.baseTaskLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = pagePattern.baseTaskLayer.TaskView;
        }

        public PageSolve(BaseLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseTaskLayer = taskLayer;
            Init();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            pagePattern.baseTaskLayer.SolveWpColWol();
            pagePattern.baseTaskLayer.SolveEmvEol();
            GrdSolutionLst.Items.Refresh();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageCombinations(pagePattern.baseTaskLayer));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            pagePattern.baseTaskLayer.Save();
        }
    }
}
