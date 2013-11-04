using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_8
{
    /// <summary>
    /// Логика взаимодействия для PageSolve.xaml
    /// </summary>
    public partial class PageSolve : Page
    {
        private PagePattern pagePattern = new PagePattern(); 
        private NavigationService navigation;

        private void Init()
        {
            GrdSolutionLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Actions.Local;
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
            var k = Convert.ToDecimal(Convert.ToDouble(pagePattern.baseLayer.DssDbContext.Actions.Local.Max(a => a.Emv)));
            var optimAct = pagePattern.baseLayer.DssDbContext.Actions.Local.FirstOrDefault(a => a.Emv == k).Name;
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
            navigation.Navigate(new PageCombinations(pagePattern.baseLayer));
        }
    }
}
