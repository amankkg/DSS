using System.Collections.ObjectModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.DbModel
{
    public class ActionEvents
    {
        public Action Action { get; set; }
        public ObservableCollection<Event> Events { get; set; }
    }
}