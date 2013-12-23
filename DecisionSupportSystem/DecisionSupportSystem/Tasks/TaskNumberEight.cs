using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberEight : TaskSpecific
    {
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; } 
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }

        public TaskNumberEight()
        {
            InitErrorCatchers();
        }

        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher){ParamsVisibility = Visibility.Visible};
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher) { ParamsVisibility = Visibility.Visible };
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher){ParamsVisibility = Visibility.Hidden};
        }
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Премия:" } });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Штраф:" } });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Пункт процента:" } });
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
                SavingId = SavingID,
                EventParams = new Collection<EventParam>
                    {
                        new EventParam{EventParamName = eventParamName
                    }}
            };
        }

        protected override Combination CreateCombinationTemplate()
        {
            return new Combination { SavingId = this.SavingID };
        }

        public void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            foreach (var combination in combinations)
            {
                var raznBrak = combination.Action.ActionParams.ToList()[0].Value - combination.Event.EventParams.ToList()[0].Value;
                if (raznBrak >= 0)
                    combination.Cp = raznBrak * BaseAlgorithms.Task.TaskParams.ToList()[2].Value * BaseAlgorithms.Task.TaskParams.ToList()[0].Value;
                else
                    combination.Cp = raznBrak * BaseAlgorithms.Task.TaskParams.ToList()[2].Value * BaseAlgorithms.Task.TaskParams.ToList()[1].Value;
            }
        }

        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }
       
        protected override int GetEventsCount()
        {
            return EventsViewModel.Events.Count;
        }

        public override void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || GetEventsCount() == 0) return;
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE { DataContext = this });
        }

        public override void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE { DataContext = this });
        }
        public override void NextBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            SolveCp();
            BaseAlgorithms.SolveTask(null);
            SetContentUEAtContentPageAndNavigate(new PageSolveUE { DataContext = this });
        }
    }
}
