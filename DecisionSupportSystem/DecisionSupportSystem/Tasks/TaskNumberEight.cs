using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberEight : TaskSpecific
    {
        protected override void CreateTaskParamsTemplate()
        {
            base.BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Премия:" } });
            base.BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Штраф:" } });
            base.BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Коэффициент преобр.:" } });
        }
        protected override Action CreateActionTemplate()
        {
            return new Action
            {
                Name = "Действие",
                SavingId = SavingID,
                ActionParams = new Collection<ActionParam>{new ActionParam
                {
                    ActionParamName = new ActionParamName{Name = "Допустимый брак"}
                }}
            };
        }
        protected override Event CreateEventTemplate()
        {
            var eventParamName = new EventParamName { Name = "Фактич. брак" };
            return new Event
            {
                Name = "Событие",
                Probability = 1,
                SavingId = this.SavingID,
                EventParams = new Collection<EventParam>
                    {
                        new EventParam{EventParamName = eventParamName
                    }}
            };
        }

        public override void SolveCP()
        {
            var combinations = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combination in combinations)
            {
                var RaznBrak = combination.Action.ActionParams.ToList()[0].Value - combination.Event.EventParams.ToList()[0].Value;
                if (RaznBrak >= 0)
                    combination.Cp = RaznBrak * BaseLayer.Task.TaskParams.ToList()[2].Value * BaseLayer.Task.TaskParams.ToList()[0].Value;
                else
                    combination.Cp = RaznBrak * BaseLayer.Task.TaskParams.ToList()[2].Value * BaseLayer.Task.TaskParams.ToList()[1].Value;
            }
        }

        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || EventsViewModel.Events.Count == 0) return;
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
    }
}
