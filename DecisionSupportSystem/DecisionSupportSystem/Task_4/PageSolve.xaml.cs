using System;
using System.Linq;
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
            GrdSolutionLst.ItemsSource = pagePattern.baseTaskLayer.DssDbContext.Actions.Local;
        }

        public PageSolve(BaseLayer taskLayer, Task4CombinationsView task4CombinationsView)
        {
            InitializeComponent();
            pagePattern.baseTaskLayer = taskLayer;
            localTaskLayuer = task4CombinationsView;
            Init();
        }

        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            localTaskLayuer.SolveCp();
            pagePattern.baseTaskLayer.SolveWpColWol();
            pagePattern.baseTaskLayer.SolveEmvEol();
            var k = Convert.ToDecimal(Convert.ToDouble(pagePattern.baseTaskLayer.DssDbContext.Actions.Local.Max(a => a.Emv)));
            var optimAct = pagePattern.baseTaskLayer.DssDbContext.Actions.Local.FirstOrDefault(a => a.Emv == k).Name;
            SolveTextBlock.Text =
            string.Format(
                "Рекомендуется выбрать действие '{0}'. Такое решение принесет максимальное значение средней ожидаемой прибыли равное {1} $. Такое значение средней ожидаемой прибыли ожидается, если многогратно в пределе после бесчисленного множества раз будет выбрано это действие при условии, что вероятности событий будут неизменны.",
                optimAct, k);
            MaxEMV.Content = k;
            GrdSolutionLst.Items.Refresh();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new PageCombinations(pagePattern.baseTaskLayer, localTaskLayuer));
        }
    }
}
