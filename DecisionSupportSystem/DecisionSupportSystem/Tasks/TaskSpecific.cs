using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using System.Collections.ObjectModel;
using DecisionSupportSystem.CommonClasses;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskSpecific
    {
        #region Поля
        protected NavigationService _navigation;
        protected bool IsSaved = false;
        #endregion
        #region Свойства
        public BaseLayer BaseLayer { get; set; }
        public ActionsViewModel ActionsViewModel { get; set; }
        public ActionViewModel ActionViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public EventsDepActionsViewModel EventsDepActionsViewModel { get; set; }
        public EventDepActionViewModel EventDepActionViewModel { get; set; }
        public ActionWithExtensionViewModel ActionWithExtensionViewModel { get; set; }
        public ActionsWithExtensionsViewModel ActionsWithExtensionsViewModel { get; set; }
        public EventWithExtensionViewModel EventWithExtensionViewModel { get; set; }
        public EventsWithExtensionsViewModel EventsWithExtensionsViewModel { get; set; }
        public ErrorCatcher TaskParamErrorCatcher { get; set; }
        public ErrorCatcher ActionErrorCatcher { get; set; }
        public ErrorCatcher EventErrorCatcher { get; set; }
        public ErrorCatcher CombinationErrorCatcher { get; set; }
        protected string WindowTitle { get; set; }
        public Guid SavingID = Guid.NewGuid();
        #endregion
        #region Окна
        public Page ContentPage { get; set; }
        public NavigationWindow NavigationWindow { get; set; } 
        #endregion

        public TaskSpecific()
        {
            ActionErrorCatcher = new ErrorCatcher();
            EventErrorCatcher = new ErrorCatcher();
            CombinationErrorCatcher = new ErrorCatcher();
            TaskParamErrorCatcher = new ErrorCatcher();
        }

        #region Методы
        // Метод вызывающий запуск приложения с главного окна
        public void Show(object pageAction, string title, string taskuniq, BaseLayer baseLayer)
        {
            WindowTitle = title;
            if (baseLayer == null)
            {
                BaseLayer = new BaseLayer();
                BaseLayer.Task.TaskUniq = taskuniq;
                CreateTaskParamsTemplate();
            }
            else BaseLayer = baseLayer;
            BaseLayer.Task.SavingId = SavingID;
            InitViewModels();
            ShowPageMain();
        }
        public virtual void ShowPageMain()
        {
            this.ContentPage = new Page();
            var pageMainUe = new PageMainUE { DataContext = this };
            ContentPage.Content = pageMainUe;
            this._navigation = NavigationService.GetNavigationService(pageMainUe.Parent);
            this.ShowNavigationWindow(ContentPage);
        }
        protected void ShowNavigationWindow(Page contentPage)
        {
            NavigationWindow = new NavigationWindow
                {
                    Title = WindowTitle,
                    Content = contentPage,
                    Width = 800,
                    Height = 550,
                    ShowsNavigationUI = false,
                };
            NavigationWindow.Closing += NavigationWindow_Closing;
            NavigationWindow.Show();
        }
        protected virtual void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(BaseLayer, ActionErrorCatcher);
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher);
            EventsViewModel = new EventsViewModel(BaseLayer, EventErrorCatcher);
            EventViewModel = new EventViewModel(CreateEventTemplate(), EventsViewModel, EventErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseLayer, TaskParamErrorCatcher);
            EventsDepActionsViewModel = new EventsDepActionsViewModel(BaseLayer, EventErrorCatcher);
            EventDepActionViewModel = new EventDepActionViewModel(BaseLayer, CreateEventTemplate(), EventsDepActionsViewModel, EventErrorCatcher);
        }

        // Создание комбинаций
        public virtual void CreateCombinations()
        {
            var lastCombinations = GetLastCombinations(BaseLayer);
            var actions = BaseLayer.DssDbContext.Actions.Local;
            var events = BaseLayer.DssDbContext.Events.Local;
            foreach (var action in actions)
                foreach (var even in events)
                    if (!HaveAction(action, lastCombinations) || !HaveEvent(even, lastCombinations))
                    {
                        BaseLayer.BaseMethods.AddCombination(CreateCombinationTemplate(), action, even, BaseLayer.Task, 0);
                    }
            InitCombinationViewModel();
        }
        protected virtual void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(BaseLayer, CombinationErrorCatcher);
        }
        protected List<Combination> GetLastCombinations(BaseLayer baseLayer)
        {
            return baseLayer.DssDbContext.Combinations.Local.ToList();
        }
        protected bool HaveAction(Action action, IEnumerable<Combination> lastCombinations)
        {
            return lastCombinations.Any(combination => combination.Action == action);
        }
        protected bool HaveEvent(Event eEvent, IEnumerable<Combination> lastCombinations)
        {
            return lastCombinations.Any(combination => combination.Event == eEvent);
        }

        // Создание шаблонов данных 
        protected virtual void CreateTaskParamsTemplate()
        {
            BaseLayer.Task.TaskParams.Add(new TaskParam{TaskParamName = new TaskParamName{Name = "Премия"}});
        }
        protected virtual Action CreateActionTemplate()
        {
            return new Action{Name = "Действие", SavingId = SavingID,
            ActionParams = new Collection<ActionParam>{new ActionParam
                {
                    ActionParamName = new ActionParamName{Name = "Допустимый брак"}
                }}};
        }
        protected virtual Event CreateEventTemplate()
        {
            return new Event{Name = "Событие", Probability = Convert.ToDecimal(0.5), SavingId = this.SavingID};
        }
        protected virtual Combination CreateCombinationTemplate()
        {
            return new Combination{SavingId = this.SavingID};
        }
        
        // Функция вычисления условной прибыли
        public virtual void SolveCP()
        {

        }
        #endregion
        #region Обработка событий
        protected void Navigate()
        {
            if (_navigation != null) _navigation.Navigate(ContentPage);
        }

        public virtual void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            if (TaskParamErrorCatcher.EntityErrorCount != 0) return;
            ContentPage.Content = new PageActionUE { DataContext = this };
            Navigate();
        }
        public virtual void PrevBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = new PageMainUE{DataContext = this};
            Navigate();
        }
        public virtual void NextBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || ActionsViewModel.Actions.Count == 0) return;
            ContentPage.Content = new PageEventUE { DataContext = this };
            Navigate();
        }
        public virtual void PrevBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionUE { DataContext = this };
            Navigate(); 
        }
        public virtual void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || EventsViewModel.Events.Count == 0) return;
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this };
            Navigate();
        }
        public virtual void PrevBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventUE { DataContext = this };
            Navigate();
        }
        public virtual void NextBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            SolveCP();
            BaseLayer.SolveThisTask(null);
            ContentPage.Content = new PageSolveUE { DataContext = this };
            Navigate();
        }
        public virtual void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this }; 
            Navigate();
        }
        public virtual void SaveBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            BaseLayer.Save();
            IsSaved = true;
        }

        protected void NavigationWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!IsSaved)
            {
                var result = MessageBox.Show("Cохранить текущее решение?", "Внимание",
                                             MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    BaseLayer.Task.Date = DateTime.Now;
                    BaseLayer.BaseMethods.AddTask(BaseLayer.Task);
                    BaseLayer.Save();
                }
                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }
        #endregion       
    }
}
