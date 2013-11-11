using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.DbModel
{
    public class EventsDependingAction
    {
        public Action Action { get; set; }
        public EventListViewModel EventListViewModel { get; set; }
    }
}