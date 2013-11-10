using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class CombinationsListControl : UserControl
    {
        public CombinationsListControl()
        {
            InitializeComponent();
        }

        private void CombinationListValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
