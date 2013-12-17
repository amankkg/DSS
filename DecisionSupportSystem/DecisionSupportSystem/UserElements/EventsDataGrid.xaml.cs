using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public partial class EventsDataGrid : UserControl
    {
        public EventsDataGrid()
        {
            InitializeComponent();
        }

        private void EventsDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns[2].Visibility = dataGrid.ParamsVisibility;
        }
    }
}
