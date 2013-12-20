using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.PageUserElements
{
    public partial class PageActionUE : UserControl
    {
        public Visibility PrevBtnVisibility { get; set; }
        public PageActionUE()
        {
            InitializeComponent();
        }

        private void PageActionUE_OnLoaded(object sender, RoutedEventArgs e)
        {
            btnprev.Visibility = PrevBtnVisibility;
        }
    }
}
