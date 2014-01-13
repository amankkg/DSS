using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberTwo : TaskSpecific
    {
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }

        public TaskNumberTwo()
        {
            InitErrorCatchers();
        }

        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher){
                    ParamsVisibility = Visibility.Hidden
                };
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher){
                    ParamsVisibility = Visibility.Hidden
                };
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }

        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher){
                    ParamsVisibility = Visibility.Hidden
                };
        }

        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Количество действий:" } });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Количество событий:" } });
        }
        protected override Action CreateActionTemplate()
        {
            return new Action {Name = "Действие", SavingId = base.SavingID};
        }
        protected override Event CreateEventTemplate()
        {
            return new Event{Name = "Событие", Probability = 1, SavingId = base.SavingID};
        }
        protected override Combination CreateCombinationTemplate()
        {
            return new Combination { SavingId = this.SavingID };
        }

        protected override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMainUe = new PageActionUE { DataContext = this, PrevBtnVisibility = Visibility.Hidden};
            ContentPage.Content = pageMainUe;
            Navigation = NavigationService.GetNavigationService(pageMainUe.Parent);
            ShowNavigationWindow(ContentPage);
        }

        public override void PrevBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionUE { DataContext = this, PrevBtnVisibility = Visibility.Hidden };
            Navigate(); 
        }
        public override void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            if (TaskParamErrorCatcher.EntityErrorCount != 0) return;
            SetContentUEAtContentPageAndNavigate(new PageActionUE { DataContext = this });
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
