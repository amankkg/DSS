using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class EventControl : UserControl
    {
        public EventControl()
        {
            InitializeComponent();
        }

        private void EventControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
