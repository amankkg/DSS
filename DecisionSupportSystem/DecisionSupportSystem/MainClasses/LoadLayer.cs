using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using Task = DecisionSupportSystem.DbModel.Task;

namespace DecisionSupportSystem.MainClasses
{
    public class LoadLayer
    {
        public BaseLayer BaseLayer { get; set; }
        public List<Task> Tasks { get; set; }
        public DssDbEntities dssDbContext;
        
        public LoadLayer()
        {
            dssDbContext = new DssDbEntities();
            BaseLayer = new BaseLayer();
            Tasks = new List<Task>();
        }

        public void LoadTasks(string taskUniq)
        {
            Tasks = (from task in dssDbContext.Tasks
                    where task.TaskUniq == taskUniq
                    select task).ToList();
        }

        public void LoadTask(Task task)
        { 
            dssDbContext.Tasks.Local.Add(task);
            var combins = (from c in dssDbContext.Combinations
                                     where c.TaskId == task.Id
                                     select c).ToList();

            foreach (var combination in combins)
            {
                dssDbContext.Combinations.Local.Add(combination);
            }
            LoadActions();
            LoadEvents();
            BaseLayer.DssDbContext = dssDbContext;
        }
        
        private void LoadActions()
        {
            var actions = (from c in dssDbContext.Combinations.Local
                           select c.Action).Distinct();
            foreach (var action in actions)
            {
                dssDbContext.Actions.Local.Add(action);
            }
        }

        private void LoadEvents()
        {
            var events = (from c in dssDbContext.Combinations.Local
                           select c.Event).Distinct();
            foreach (var eEvent in events)
            {
                dssDbContext.Events.Local.Add(eEvent);
            }
        }
    }
}
