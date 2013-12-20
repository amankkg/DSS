using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class TaskParamsViewModel : BasePropertyChanged
    {
        public Task Task { get; set; }
       
        public TaskParamsViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            this.Task = baseLayer.Task;
            ErrorCatcher = errorCatcher;
        }

    }
}
