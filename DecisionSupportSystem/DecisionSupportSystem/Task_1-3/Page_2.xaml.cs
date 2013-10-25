using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DecisionSupportSystem.Interfaces.Task_1_3
{
    public partial class Task132
    {
        /*private TaskLayer _layerData;*/
        public Task132()
        {
            InitializeComponent();
        }

        public Task132(TaskLayer layerData)
        {/*
            InitializeComponent();
            _layerData = layerData;
            GrdEventsLst.ItemsSource = _layerData.Events;*/
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            /*var even = new Event();
            even.Name = TxtEvent.Text;
            even.Probability = Convert.ToDecimal(TxtProbability.Text);
            _layerData.Events.Add(even);
            GrdEventsLst.Items.Refresh();*/
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            /*var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task133(_layerData);
            nav.Navigate(nextPage); */
        }
    }
}
