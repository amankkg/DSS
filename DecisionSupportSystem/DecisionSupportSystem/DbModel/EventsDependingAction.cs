using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.ViewModel;

namespace DecisionSupportSystem.DbModel
{
    public class EventsDependingAction
    {
        public Action Action { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
    }
}