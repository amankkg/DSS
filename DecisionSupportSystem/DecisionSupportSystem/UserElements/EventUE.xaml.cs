using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public partial class EventUE : UserControl
    {
        public EventUE()
        {
            InitializeComponent();
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (CheckBoxGen.IsChecked == true)
            {
                TextBoxCount.Visibility = Visibility.Visible;
                LabelCount.Visibility = Visibility.Visible;
            }
            else
            {
                TextBoxCount.Visibility = Visibility.Hidden;
                LabelCount.Visibility = Visibility.Hidden;
            }
        }
    }
}
