using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class ActionWithParamControl
    {
        public ActionWithParamControl()
        {
            InitializeComponent();
        }

        private void ActionValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }

    }
}
