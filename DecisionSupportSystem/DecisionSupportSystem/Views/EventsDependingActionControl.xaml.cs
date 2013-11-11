using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class EventsDependingActionControl : UserControl
    {
        public EventsDependingActionControl()
        {
            InitializeComponent();
        }

        private void EventsDependingActionControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
