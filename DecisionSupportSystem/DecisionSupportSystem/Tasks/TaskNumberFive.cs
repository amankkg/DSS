using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.CommonClasses;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberFive : TaskSpecific
    {
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsDepActionsViewModel EventsDepActionsViewModel { get; set; }
        public EventDepActionViewModel EventDepActionViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }

        public TaskNumberFive()
        {
            InitErrorCatchers();
        }

        public List<Combination> FictiveCombinations { get; set; }   
        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher)
            {ParamsVisibility = Visibility.Hidden};
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsDepActionsViewModel = new EventsDepActionsViewModel(DssDbEntities, EventErrorCatcher)
            {ParamsVisibility = Visibility.Hidden};
            EventDepActionViewModel = new EventDepActionViewModel(DssDbEntities.Actions.Local, CreateEventTemplate(), EventsDepActionsViewModel, EventErrorCatcher);
        }

        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
        }

        protected override void CreateTaskParamsTemplate() { }
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = SavingID };
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
                            new CombinParam{CombinParamName = combinParamNameF, Value = Random.Next(1000, 15000)},
                            new CombinParam{CombinParamName = combinParamNameS, Value = Random.Next(1000, 15000)}
                        }
            };
        }

        protected override void CreateCombinations()
        {
            var combinations = DssDbEntities.Combinations.Local.ToList();
            var actions = EventsDepActionsViewModel.EventsDependingActions;
            foreach (var eventsDependingAction in actions)
            {
                var events = eventsDependingAction.EventsViewModel.Events;
                if (events.Count == 0) events.Add(null);
                foreach (var even in events)
                {
                    if (!ActionContainsInCombinations(eventsDependingAction.Action, combinations) 
                        || !EventContainsInCombinations(even, combinations))
                    {
                        CRUD.AddCombination(CreateCombinationTemplate(), eventsDependingAction.Action, even, BaseAlgorithms.Task, 0);
                    }
                }
            }
            InitCombinationViewModel();
            CreateFictiveCombinations();
        }

        public void CreateFictiveCombinations()
        {
            FictiveCombinations = new List<Combination>();
            var combins = DssDbEntities.Combinations.Local.ToList();
            foreach (var action in DssDbEntities.Actions.Local)
                foreach (var even in DssDbEntities.Events.Local)
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
        public virtual void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            foreach (var combination in combinations)
            {
                var debit = combination.CombinParams.ToList()[0].Value;
                var credit = combination.CombinParams.ToList()[1].Value;
                combination.Cp = debit - credit;
            }
        }

        protected override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMainUe = new PageActionUE { DataContext = this, PrevBtnVisibility = Visibility.Hidden };
            ContentPage.Content = pageMainUe;
            Navigation = NavigationService.GetNavigationService(pageMainUe.Parent);
            ShowNavigationWindow(ContentPage);
        }
        
        public override void PrevBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageActionUE { DataContext = this, PrevBtnVisibility = Visibility.Hidden });
        }
        public override void NextBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || GetActionsCount() == 0) return;
            EventsDepActionsViewModel.CheckForUpdatedData();
            SetContentUEAtContentPageAndNavigate(new PageEventsDepActionUE {DataContext = this});
        }
        public override void PrevBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageEventsDepActionUE { DataContext = this });
        }
        public override void NextBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            SolveCp();
            BaseAlgorithms.SolveTask(FictiveCombinations);
            SetContentUEAtContentPageAndNavigate(new PageSolveUE { DataContext = this });
        }
        public override void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || GetEventsCount() == 0) return;
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE { DataContext = this });
        }
        public override void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE { DataContext = this });
        }  
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }
        protected override int GetEventsCount()
        {
            return EventsDepActionsViewModel.EventsDependingActions.Count;
        }
    }
}
