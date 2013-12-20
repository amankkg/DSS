using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public partial class ActionWithExtensionUE : UserControl
    {
        public ActionWithExtensionUE()
        {
            InitializeComponent();
            ActionCmbx.Margin = ActionTxtb.Margin;
            ActionCmbx.Width = ActionTxtb.Width;
            ActionCmbx.Visibility = Visibility.Hidden;
        }
        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (ExtendChbx.IsChecked == true)
            {
                ActionTxtb.Visibility = Visibility.Hidden;
                ActionCmbx.Visibility = Visibility.Visible;
            }
            else
            {
                ActionCmbx.Visibility = Visibility.Hidden;
                ActionTxtb.Visibility = Visibility.Visible;
            }
        }

    }
}
