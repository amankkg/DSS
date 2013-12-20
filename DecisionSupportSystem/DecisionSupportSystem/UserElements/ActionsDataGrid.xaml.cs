using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public partial class ActionsDataGrid : UserControl
    {
        public ActionsDataGrid()
        { 
            InitializeComponent();
        }

        private void ActionsDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            myDatagrid.Columns[1].Visibility = myDatagrid.ParamsVisibility;
        }

    }
}
