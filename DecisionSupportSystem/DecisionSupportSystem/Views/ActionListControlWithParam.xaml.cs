using System.Windows.Controls;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для ActionListControlWithParam.xaml
    /// </summary>
    public partial class ActionListControlWithParam : UserControl
    {
        public ActionListControlWithParam()
        {
            InitializeComponent();
        }

        private void ActionListControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        private void EventListControlValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
    }
}
