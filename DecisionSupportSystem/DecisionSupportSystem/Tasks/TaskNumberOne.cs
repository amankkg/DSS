using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberOne : TaskSpecific
    {
        protected override Action CreateActionTemplate()
        {
            return new Action {Name = "Действие", SavingId = base.SavingID};
        }
        protected override Event CreateEventTemplate()
        {
            return new Event{Name = "Событие", Probability = 1, SavingId = base.SavingID};
        }
        protected override void CreateTaskParamsTemplate() 
        { }
        public override void SolveCP() 
        { }
    }
}
