using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModels
{
    public class EventsDepActionListViewModel
    {
        public BaseLayer BaseLayer { get; set; }
        public ObservableCollection<ActionEvents> ActionEventsList { get; set; }

        public EventsDepActionListViewModel(BaseLayer baseLayer)
        {
            BaseLayer = baseLayer;
/*            if (BaseLayer.DssDbContext.)
            {
                
            }*/
            var actions = BaseLayer.DssDbContext.Actions.Local;
            var combinations = baseLayer.DssDbContext.Combinations.Local;

            if (combinations.Count > 0)
            { 
                
            }
        }

        public void UpdateEvents()
        {
            
        }
    }
}