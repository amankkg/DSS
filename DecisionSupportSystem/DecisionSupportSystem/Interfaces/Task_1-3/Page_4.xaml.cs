
using System.Windows;

namespace DecisionSupportSystem.Interfaces.Task_1_3
{
    /// <summary>
    /// Логика взаимодействия для Task_1_3_4.xaml
    /// </summary>
    public partial class Task134
    {
        private TaskLayer _layerData;

        public Task134()
        {
           InitializeComponent();
        }

        public Task134(TaskLayer layerData)
        {
            InitializeComponent();
            _layerData = layerData;
            
        }


        private void BtnShowSolution_OnClick(object sender, RoutedEventArgs e)
        {
            _layerData.SolveAllEntities();
            GrdSolutionLst.ItemsSource = _layerData.Actions;
        }
    }
}
