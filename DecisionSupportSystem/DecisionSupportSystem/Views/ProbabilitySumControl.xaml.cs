using System.Windows.Controls;


namespace DecisionSupportSystem.Views
{
    public partial class ProbabilitySumControl : UserControl
    {
        public ProbabilitySumControl()
        {
            InitializeComponent();
        }

        private void ProbabilityValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
