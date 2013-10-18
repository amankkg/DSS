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


namespace DecisionSupportSystem.Interfaces.Task_5
{
    public partial class Task52
    {
        private TaskLayer _layerData;
        public Task52()
        {
            InitializeComponent();
            
        }

        public Task52(TaskLayer layerData)
        {
            InitializeComponent();
            _layerData = layerData;
            Acts.ItemsSource = _layerData.Actions;
          
            GrdEventsLst.ItemsSource = _layerData.Events;
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var even = new EventTask5();
            even.Name = TxtEvent.Text;
            even.Probability = Convert.ToDecimal(TxtProbability.Text);
            even.Action = (Action)Acts.SelectedItem;
            _layerData.Events.Add(even);
            GrdEventsLst.Items.Refresh();
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task53(_layerData);
            nav.Navigate(nextPage); 
        }
    }
}
