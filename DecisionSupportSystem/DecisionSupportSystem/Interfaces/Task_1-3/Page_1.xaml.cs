using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace DecisionSupportSystem.Interfaces.Task_1_3
{
    public partial class Task131
    {
        private TaskLayer _layerData;
        public Task131()
        {
            InitializeComponent();
            _layerData = new TaskLayer();
            GrdActionsLst.ItemsSource = _layerData.Actions;
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var action = new Action();
            action.Name = TxtAction.Text;
            _layerData.Actions.Add(action);
            GrdActionsLst.Items.Refresh();
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task132(_layerData);
            nav.Navigate(nextPage); 
        }
    }
}
