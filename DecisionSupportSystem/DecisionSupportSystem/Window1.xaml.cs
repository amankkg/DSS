using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.Task_5;

namespace DecisionSupportSystem
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public ObservableCollection<MainEvetListViewModel> EvetList { get; set; }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EvetList = new ObservableCollection<MainEvetListViewModel>();
            var Events = new List<Event>
                {
                    new Event {Name = "Событие 1", Probability = Convert.ToDecimal(0.5)},
                    new Event {Name = "Событие 2", Probability = Convert.ToDecimal(0.5)}
                };
            //EvetList.Add(new MainEvetListViewModal(Events));
            var h = Events;
            grid.ItemsSource = EvetList;
        }
    }
}
