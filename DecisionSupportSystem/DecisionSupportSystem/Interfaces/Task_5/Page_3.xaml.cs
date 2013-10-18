using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace DecisionSupportSystem.Interfaces.Task_5
{
    public partial class Task53
    {
        private TaskLayer _layerData;

        public Task53()
        {
            InitializeComponent();
        }

        public Task53(TaskLayer layerData)
        {
            InitializeComponent();
            _layerData = layerData;
        }

        private void BtnShowCombination_OnClick(object sender, RoutedEventArgs e)
        {
            _layerData.CreateActionEventCombination();
           /* var combinListForBind = _layerData.Combinations.Select(
                                c => new FormatDataForDatagrid(c)).ToList();*/
            GrdCombinsLst.ItemsSource = _layerData.Combinations;
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task54(_layerData);
            nav.Navigate(nextPage);
        }
    }
}
