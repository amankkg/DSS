using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberNine : TaskSpecific
    {
        public ActionForTask9ViewModel ActionForTask9ViewModel { get; set; }
        public ActionsForTask9ViewModel ActionsViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }
        public TaskNumberNine()
        {
            InitErrorCatchers();
        }
        protected override void InitViewModels()
        {   
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher);
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
            ActionsViewModel = new ActionsForTask9ViewModel(DssDbEntities, ActionErrorCatcher);
            ActionForTask9ViewModel = new ActionForTask9ViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher)
                {EventViewModel = EventViewModel, Task = BaseAlgorithms.Task};
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        } 
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher) { ParamsVisibility = Visibility.Hidden }; 
        }
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Период:" } });
        }

        protected override Action CreateActionTemplate()
        {
            return new Action
            {
                Name = "Действие",
                SavingId = SavingID,
                ActionParams = new Collection<ActionParam>{
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Срок:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Затрата:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Срок расширения:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Затрата на расширение:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Доход предпр:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Доход расшир:"}
                        }
                }
            };
        }
        protected override Event CreateEventTemplate()
        {
            return new Event
            {
                Name = "Событие",
                Probability = 1,
                SavingId = this.SavingID,
                EventParams = new Collection<EventParam>()
            };
        }
        protected override Combination CreateCombinationTemplate()
        {
            return new Combination
            {
                SavingId = base.SavingID,
                CombinParams = new Collection<CombinParam>()
            };
        }

        public virtual void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            foreach (var combination in combinations)
            {
                if (combination.Action.ActionParams.ToList()[5].Value == -1)
                    combination.Cp = combination.Action.ActionParams.ToList()[0].Value*
                                     combination.Event.EventParams.ToList()[
                                         Convert.ToInt32(combination.Action.ActionParams.ToList()[4].Value)].Value -
                                     combination.Action.ActionParams.ToList()[1].Value;
                else
                    combination.Cp = combination.Action.ActionParams.ToList()[0].Value*
                                     combination.Event.EventParams.ToList()[
                                         Convert.ToInt32(combination.Action.ActionParams.ToList()[4].Value)].Value +
                                     combination.Action.ActionParams.ToList()[2].Value*
                                     combination.Event.EventParams.ToList()[
                                         Convert.ToInt32(combination.Action.ActionParams.ToList()[5].Value)].Value -
                                     combination.Action.ActionParams.ToList()[1].Value -
                                     combination.Action.ActionParams.ToList()[3].Value;
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
        public override void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            if (TaskParamErrorCatcher.EntityErrorCount != 0) return;
            SetContentUEAtContentPageAndNavigate(new PageActionForTask9UE { DataContext = this });
        }
        public override void NextBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || GetActionsCount() == 0) return;
            SetContentUEAtContentPageAndNavigate(new PageEventUE {DataContext = this});
        }
        public override void PrevBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageActionForTask9UE {DataContext = this});
        }
        public override void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || GetEventsCount() == 0) return;
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE {DataContext = this});
        }
        public override void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithCpUE {DataContext = this});
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
