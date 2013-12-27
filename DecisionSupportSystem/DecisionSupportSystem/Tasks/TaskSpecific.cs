using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using DecisionSupportSystem.PageUserElements;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public abstract class TaskSpecific
    {   
        #region Поля
        protected NavigationService Navigation;
        protected bool IsSaved;
        #endregion
        #region Свойства
        public BaseAlgorithms BaseAlgorithms { get; set; }
        public DssDbEntities DssDbEntities { get; set; }
        protected ErrorCatcher TaskParamErrorCatcher { get; set; }
        protected ErrorCatcher ActionErrorCatcher { get; set; }
        protected ErrorCatcher EventErrorCatcher { get; set; }
        protected ErrorCatcher CombinationErrorCatcher { get; set; }
        protected string WindowTitle { get; set; }
        protected Guid SavingID = Guid.NewGuid();
        #endregion
        #region Окна
        protected Page ContentPage { get; set; }
        protected NavigationWindow NavigationWindow { get; set; } 
        #endregion

        protected abstract void CreateTaskParamsTemplate();
        protected abstract Action CreateActionTemplate();
        protected abstract Event CreateEventTemplate();
        protected abstract Combination CreateCombinationTemplate();

        protected void InitErrorCatchers()
        {
            ActionErrorCatcher = new ErrorCatcher();
            EventErrorCatcher = new ErrorCatcher();
            CombinationErrorCatcher = new ErrorCatcher();
            TaskParamErrorCatcher = new ErrorCatcher();
        }

        public void InitBaseLayerAndShowMainPage(string title, string taskuniq, DssDbEntities dssDbEntities)
        {
            WindowTitle = title;
            BaseAlgorithms = new BaseAlgorithms();
            if (dssDbEntities == null)
            {
                DssDbEntities = new DssDbEntities();
                BaseAlgorithms.Task = new Task();
                CreateTaskParamsTemplate();
            }
            else
            {
                DssDbEntities = dssDbEntities;
                BaseAlgorithms.Task = DssDbEntities.Tasks.Local.ToList()[0];
            }            
            BaseAlgorithms.Entities = DssDbEntities;
            BaseAlgorithms.Task.TaskUniq = taskuniq;
            BaseAlgorithms.Task.SavingId = SavingID;
            CRUD.DssDbEntities = DssDbEntities;
            
            InitViewModels();
            ShowPageMain();
        }  
        
        protected abstract void InitViewModels();

        protected virtual void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMainUe = new PageMainUE { DataContext = this };
            ContentPage.Content = pageMainUe;
            Navigation = NavigationService.GetNavigationService(pageMainUe.Parent);
            ShowNavigationWindow(ContentPage);
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

        protected virtual void CreateCombinations()
        {
            var combinations = DssDbEntities.Combinations.Local.ToList();
            var actions = DssDbEntities.Actions.Local;
            var events = DssDbEntities.Events.Local;
            foreach (var action in actions)
                foreach (var even in events)
                    if (!ActionContainsInCombinations(action, combinations) || !EventContainsInCombinations(even, combinations))
                    {
                        CRUD.AddCombination(CreateCombinationTemplate(), action, even, BaseAlgorithms.Task, 0);
                    }
            InitCombinationViewModel();
        }
        protected abstract void InitCombinationViewModel();

        protected bool ActionContainsInCombinations(Action action, IEnumerable<Combination> combinations)
        {
            return combinations.Any(combination => combination.Action == action);
        }
        protected bool EventContainsInCombinations(Event eEvent, IEnumerable<Combination> combinations)
        {
            return combinations.Any(combination => combination.Event == eEvent);
        }
        
        public virtual void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            if (TaskParamErrorCatcher.EntityErrorCount != 0) return;
            SetContentUEAtContentPageAndNavigate(new PageActionUE { DataContext = this });
        }
        public virtual void PrevBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageMainUE { DataContext = this });
        }
        public virtual void NextBtnClick_OnPageActions(object sender, RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || GetActionsCount() == 0) return;
            SetContentUEAtContentPageAndNavigate(new PageEventUE { DataContext = this });
        }
        protected abstract int GetActionsCount();
        public virtual void PrevBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageActionUE { DataContext = this });
        }
        public virtual void NextBtnClick_OnPageEvents(object sender, RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || GetEventsCount() == 0) return;
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithCpUE { DataContext = this });
        }
        protected abstract int GetEventsCount();
        public virtual void PrevBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            SetContentUEAtContentPageAndNavigate(new PageEventUE { DataContext = this });
        }
        public virtual void NextBtnClick_OnPageCombinations(object sender, RoutedEventArgs e)
        {
            if (CombinationErrorCatcher.EntityGroupErrorCount != 0) return;
            BaseAlgorithms.SolveTask(null);
            SetContentUEAtContentPageAndNavigate(new PageSolveUE { DataContext = this });
        }
        public virtual void PrevBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            CreateCombinations();
            SetContentUEAtContentPageAndNavigate(new PageCombinationWithCpUE { DataContext = this });
        }
        protected virtual void SetContentUEAtContentPageAndNavigate(UserControl content)
        {
            ContentPage.Content = content;
            Navigate();
        }
        protected void Navigate()
        {
            if (Navigation != null) 
                Navigation.Navigate(ContentPage);
        }
        public virtual void SaveBtnClick_OnPageSolve(object sender, RoutedEventArgs e)
        {
            BaseAlgorithms.Save();
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
                    BaseAlgorithms.Task.Date = DateTime.Now;
                    CRUD.AddTask(BaseAlgorithms.Task);
                    BaseAlgorithms.Save();
                }
                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }
 
    }
}
