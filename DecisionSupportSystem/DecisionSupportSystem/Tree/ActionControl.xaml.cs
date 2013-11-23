using System.Windows;
using System.Windows.Controls;


namespace DecisionSupportSystem.Tree
{
    public partial class ActionControl : UserControl
    {
        public ActionControl()
        {
            InitializeComponent();
        }

        private void Expander_OnExpanded(object sender, RoutedEventArgs e)
        {
            this.Height = 145;
        }

        private void expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 86;
        }
    }
}
