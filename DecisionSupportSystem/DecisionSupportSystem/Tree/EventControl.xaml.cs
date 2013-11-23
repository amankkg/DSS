using System.Windows;
using System.Windows.Controls;


namespace DecisionSupportSystem.Tree
{
     
    public partial class EventControl : UserControl
    {
        public EventControl()
        {
            InitializeComponent();
        }

        private void Expander_OnExpanded(object sender, RoutedEventArgs e)
        {
            this.Height = 175;
        }

        private void expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 95;
        }
    }
}
