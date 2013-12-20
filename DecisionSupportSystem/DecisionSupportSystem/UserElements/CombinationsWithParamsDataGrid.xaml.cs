using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public partial class CombinationsWithParamsDataGrid : UserControl
    {
        public CombinationsWithParamsDataGrid()
        {
            InitializeComponent();
        }

        private void CombinationsWithParamsDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[2].Visibility = datagrid.ParamsVisibility;
        }
    }
}
