using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class CombinationsListControlWithParam : UserControl
    {
        public CombinationsListControlWithParam()
        {
            InitializeComponent();
        }

        private void CombinationListValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
