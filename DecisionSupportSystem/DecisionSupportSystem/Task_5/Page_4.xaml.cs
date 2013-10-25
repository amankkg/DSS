
using System.Linq;
using System.Windows;

namespace DecisionSupportSystem.Interfaces.Task_5
{
    /// <summary>
    /// Логика взаимодействия для Task_1_3_4.xaml
    /// </summary>
    public partial class Task54
    {
        /*private TaskLayer _layerData;*/

        public Task54()
        {
           InitializeComponent();
        }

       /* public Task54(TaskLayer layerData)
        {
            InitializeComponent();
            _layerData = layerData;
            
        }*/


        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
           /* _layerData.SolveAllEntities();
            GrdSolutionLst.ItemsSource = _layerData.Actions;
            var k=_layerData.Actions.Max(a => a.ExpectedMonetaryValue);
            var optimAct = _layerData.Actions.FirstOrDefault(a => a.ExpectedMonetaryValue == k).Name;
            SolveTextBlock.Text =
                string.Format(
                    "Моему клиенту рекомендуется выбрать действие {0}. Такое решение принесет ему максимальное значение средней ожидаемой прибыли равное равное {1} $. Он получит такое значение средней ожидаемой прибыли, если многогратно в пределе после бесчисленного множества раз будет выберать это действие при условии что вероятности событий не изменятся.",
                    optimAct, k);
            MAxEMV.Content = k;*/

        }
    }
}
 