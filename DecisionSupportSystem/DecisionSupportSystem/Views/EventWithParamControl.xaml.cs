using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    public partial class EventControlWithParam : UserControl
    {
        public EventControlWithParam()
        {
            InitializeComponent();
        }

        private void EventControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
