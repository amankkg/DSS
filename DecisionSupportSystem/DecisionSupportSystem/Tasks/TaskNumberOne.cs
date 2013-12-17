using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberOne : TaskSpecific
    {
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }

        public TaskNumberOne()
        {
            InitErrorCatchers();
        }

        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(BaseLayer, ActionErrorCatcher){ParamsVisibility = Visibility.Hidden};
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsViewModel = new EventsViewModel(BaseLayer, EventErrorCatcher){ParamsVisibility = Visibility.Hidden};
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
        }
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(BaseLayer, CombinationErrorCatcher){ParamsVisibility = Visibility.Hidden};
        }
        protected override void CreateTaskParamsTemplate()
        {
            return;
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
