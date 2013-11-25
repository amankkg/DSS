using System.Windows.Controls;

namespace DecisionSupportSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для EventsListControlWithParam.xaml
    /// </summary>
    public partial class EventsListControlWithParam : UserControl
    {
        public EventsListControlWithParam()
        {
            InitializeComponent();
        }

        private void EventListControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
    }
}
