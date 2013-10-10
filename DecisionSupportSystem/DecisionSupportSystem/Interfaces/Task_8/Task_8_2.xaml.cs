using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace DecisionSupportSystem
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

       /* private InterfaceData _data;

        public Page_2(InterfaceData Idata)
        {
            InitializeComponent();
            _data = Idata;
            GrdEventsLst.ItemsSource = _data.EventIDatas;
        }
        */
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
           /* var dt = new EventIData();
            dt.Name = TxtEvent.Text;
            dt.Probability = Convert.ToDecimal(TxtProbability.Text);
            dt.Defect = new Parameter
                {
                    Name = "Defect",
                    Value = Convert.ToDecimal(TxtProductDefect.Text)
                };
            _data.EventIDatas.Add(dt);
            GrdEventsLst.Items.Refresh();*/
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
           /* var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Page_3(_data);
            nav.Navigate(nextPage);  */
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
           /* if (GrdEventsLst.SelectedItem != null)
            {

            }*/
        }
    }
}
