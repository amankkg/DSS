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

namespace DecisionSupportSystem.Interfaces.Task_8
{
    /// <summary>
    /// Логика взаимодействия для MainPage1_3.xaml
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
        {
          //  InitializeComponent(); 
        }
 
        private void BtnEmv_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav;
            nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task_8_1();
            nav.Navigate(nextPage); 
        }
    }
}
