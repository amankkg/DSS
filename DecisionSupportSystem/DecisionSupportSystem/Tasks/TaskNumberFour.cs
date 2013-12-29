using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberFour : TaskSpecific
    {
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }

        public TaskNumberFour()
        {
            InitErrorCatchers();
        }

        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
        }
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher) { ParamsVisibility = Visibility.Visible };
        }
        protected override void CreateTaskParamsTemplate() { }
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = base.SavingID};
        }
        protected override Event CreateEventTemplate()
        {
            return new Event { Name = "Событие", Probability = 1, SavingId = base.SavingID };
        }
        protected override Combination CreateCombinationTemplate()
        {
            var combinParamNameF = new CombinParamName {Name = "Увел. проц. ставки"};
            var combinParamNameS = new CombinParamName{Name = "Увел. ном. цены"};
            return new Combination
                {
                    SavingId = base.SavingID,
                    CombinParams = new Collection<CombinParam>
                        {
                            new CombinParam{CombinParamName = combinParamNameF, Value = Random.Next(100)},
                            new CombinParam{CombinParamName = combinParamNameS, Value = Random.Next(100)}
                        }
                };
        }
        public void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            foreach (var combination in combinations)
            {
                var interestRate = combination.CombinParams.ToList()[0].Value;
                var nominalPrice = combination.CombinParams.ToList()[1].Value;
                combination.Cp = interestRate * (nominalPrice + 100) / 100;
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
            SetContentUEAtContentPageAndNavigate(new PageActionUE{DataContext = this, PrevBtnVisibility = Visibility.Hidden});
        }
        public override void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || GetEventsCount() == 0) return;
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE {DataContext = this});
        }
        public override void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithParamUE {DataContext = this});
        }
        public override void NextBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            SolveCp();
            BaseAlgorithms.SolveTask(null);
            SetContentUEAtContentPageAndNavigate(new PageSolveUE { DataContext = this });
        }
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }
        protected override int GetEventsCount()
        {
            return EventsViewModel.Events.Count;
        }
    }
}
