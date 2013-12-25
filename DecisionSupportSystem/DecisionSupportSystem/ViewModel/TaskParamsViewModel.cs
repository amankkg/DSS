using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class TaskParamsViewModel : BasePropertyChanged
    {
        public Task Task { get; set; }
       
        public TaskParamsViewModel(Task task, IErrorCatch errorCatcher)
        {
            this.Task = task;
            ErrorCatcher = errorCatcher;
        }

    }
}
