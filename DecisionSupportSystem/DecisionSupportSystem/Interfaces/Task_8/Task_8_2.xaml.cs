using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.Interfaces.Task_8;


namespace DecisionSupportSystem.Interfaces.Task_8
{
    /// <summary>
    /// Логика взаимодействия для Page_2.xaml
    /// </summary>
    public partial class Task_8_2 : Page
    {
        public Task_8_2()
        {
            InitializeComponent();
        }

        private InterfaceData _data;

        public Task_8_2(InterfaceData Idata)
        {
            InitializeComponent();
            _data = Idata;
            GrdEventsLst.ItemsSource = _data.EventIDatas;
        }
        
      

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (GrdEventsLst.SelectedItem != null)
            {

            }
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var dt = new EventIData();
            dt.Name = TxtEvent.Text;
            dt.Probability = Convert.ToDecimal(TxtProbability.Text);
            dt.Defect = new Parameter
            {
                Name = "Defect",
                Value = Convert.ToDecimal(TxtProductDefect.Text)
            };
            _data.EventIDatas.Add(dt);
            GrdEventsLst.Items.Refresh();
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task_8_3(_data);
            nav.Navigate(nextPage);  
        }
    }
}
