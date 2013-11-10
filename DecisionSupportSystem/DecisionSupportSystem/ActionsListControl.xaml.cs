using System.Windows.Controls;

namespace DecisionSupportSystem
{
    public partial class ActionsListControl
    {
        public ActionsListControl()
        {
            InitializeComponent();
        }

        private void ActionListControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
