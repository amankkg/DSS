using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberFive : TaskSpecific
    {
        public List<Combination> FictiveCombinations { get; set; }
        protected override void CreateTaskParamsTemplate() { }
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = base.SavingID };
        }
        protected override Event CreateEventTemplate()
        {
            return new Event { Name = "Событие", Probability = 1, SavingId = base.SavingID };
        }
        protected override Combination CreateCombinationTemplate()
        {
            var combinParamNameF = new CombinParamName { Name = "Доход" };
            var combinParamNameS = new CombinParamName { Name = "Расход" };
            return new Combination
            {
                SavingId = base.SavingID,
                CombinParams = new Collection<CombinParam>
                        {
                            new CombinParam{CombinParamName = combinParamNameF},
                            new CombinParam{CombinParamName = combinParamNameS}
                        }
            };
        }
        public override void CreateCombinations()
        {
            var lastCombinations = GetLastCombinations(BaseLayer);
            var actions = EventsDepActionsViewModel.EventsDependingActions;
            foreach (var eventsDependingAction in actions)
            {
                var events = eventsDependingAction.EventsViewModel.Events;
                if (events.Count == 0) events.Add(null);
                foreach (var even in events)
                {
                    if (!HaveAction(eventsDependingAction.Action, lastCombinations) || !HaveEvent(even, lastCombinations))
                    {
                        BaseLayer.BaseMethods.AddCombination(CreateCombinationTemplate(), eventsDependingAction.Action, even, BaseLayer.Task, 0);
                    }
                }
            }
            InitCombinationViewModel();
            CreateFictiveCombinations();
        }

        public void CreateFictiveCombinations()
        {
            FictiveCombinations = new List<Combination>();
            var combins = BaseLayer.DssDbContext.Combinations.Local.ToList();
            foreach (var action in BaseLayer.DssDbContext.Actions.Local)
                foreach (var even in BaseLayer.DssDbContext.Events.Local)
                {
                    var combination = combins.Select(c => c).Where(c => c.Action == action && c.Event == even).ToList();
                    if (combination.Count == 0)
                        FictiveCombinations.Add(new Combination
                        {
                            Action = action,
                            Event = even,
                            Cp = 0
                        });
                    else
                        FictiveCombinations.Add(combination[0]);
                }
        }

        public override void SolveCP()
        {
            var combinations = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combination in combinations)
            {
                combination.Cp = combination.CombinParams.ToList()[0].Value - combination.CombinParams.ToList()[1].Value;
            }
        }

        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || ActionsViewModel.Actions.Count == 0) return;
            EventsDepActionsViewModel.CheckForUpdatedData();
            ContentPage.Content = new PageEventsDepActionUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventsDepActionUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            SolveCP();
            BaseLayer.SolveThisTask(FictiveCombinations);
            ContentPage.Content = new PageSolveUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || EventsDepActionsViewModel.EventsDependingActions.Count == 0) return;
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
    }
}
