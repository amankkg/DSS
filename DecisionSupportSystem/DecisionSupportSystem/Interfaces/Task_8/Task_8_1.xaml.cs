using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace DecisionSupportSystem.Interfaces.Task_8
{
    public partial class Task_8_1 : Page
    {
        private InterfaceData _data;
        
        public Task_8_1()
        {
            InitializeComponent();
            _data = new InterfaceData();
            GrdActionsLst.ItemsSource = _data.ActionIDatas;
        }

    

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var dt = new ActionIData();
            dt.Name = TxtAction.Text;
            dt.RequiredDefect = new Parameter
            {
                Name = "RDefect",
                Value = Convert.ToDecimal(TxtRequiredDefect.Text)
            };
            _data.ActionIDatas.Add(dt);
            GrdActionsLst.Items.Refresh();
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task_8_2(_data);
            nav.Navigate(nextPage);
        }
    }
}
