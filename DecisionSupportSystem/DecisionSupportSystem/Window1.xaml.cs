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

        public ObservableCollection<MainEvetListViewModal> EvetList { get; set; }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EvetList = new ObservableCollection<MainEvetListViewModal>();
            EvetList.Add(new MainEvetListViewModal(1));
            EvetList.Add(new MainEvetListViewModal(2));
            grid.ItemsSource = EvetList;
        }
    }
}
