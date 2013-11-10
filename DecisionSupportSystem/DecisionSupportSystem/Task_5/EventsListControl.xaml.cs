using System.Windows.Controls;

namespace DecisionSupportSystem.Task_5
{
    public partial class EventsListControl : UserControl
    {
        public EventsListControl()
        {
            InitializeComponent();
        }

        private void EventValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
