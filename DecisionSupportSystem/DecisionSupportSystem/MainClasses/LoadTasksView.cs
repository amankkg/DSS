using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.MainClasses
{
    public class LoadTasksView
    {
        public List<Task> Tasks { get; set; }

        public void LoadTasks(string taskUniq)
        {
            Tasks = new List<Task>();
            using (var dssDbContext = new DssDbEntities())
            {
                var tasks = (from task in dssDbContext.Tasks
                             where task.TaskUniq == taskUniq
                             select task).ToList();  
                foreach (var t in tasks)
                {
                    Tasks.Add(new Task{Comment = t.Comment, TaskUniq = t.TaskUniq, Id = t.Id, Recommendation = t.Recommendation, Date = t.Date});
                }
            }
        }
    }
}