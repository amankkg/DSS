using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class ActionControl
    {
        public ActionControl()
        {
            InitializeComponent();
        }

        private void ActionValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
