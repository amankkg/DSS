using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для EventsListControl.xaml
    /// </summary>
    public partial class EventsListControl : UserControl
    {
        public EventsListControl()
        {
            InitializeComponent();
        }

        private void EventListControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
