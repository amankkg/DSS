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
        public BaseLayer BaseLayer { get; set; }
        protected ErrorCatcher TaskParamErrorCatcher { get; set; }
        protected ErrorCatcher ActionErrorCatcher { get; set; }
        protected ErrorCatcher EventErrorCatcher { get; set; }
        protected ErrorCatcher CombinationErrorCatcher { get; set; }
        protected string WindowTitle { get; set; }
        protected readonly Guid SavingID = Guid.NewGuid();
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

        public void InitBaseLayerAndShowMainPage(string title, string taskuniq, BaseLayer baseLayer)
        {
            WindowTitle = title;
            if (baseLayer == null)
                InitNewBaseLayer(taskuniq);
            else 
                BaseLayer = baseLayer;
            BaseLayer.Task.SavingId = SavingID;
            InitViewModels();
            ShowPageMain();
        } 
        protected abstract void InitViewModels();

        protected void InitNewBaseLayer(string taskuniq)
        {
                BaseLayer = new BaseLayer();
                BaseLayer.Task.TaskUniq = taskuniq;
                CreateTaskParamsTemplate();
        }

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
            var combinations = GetCombinations(BaseLayer);
            var actions = BaseLayer.DssDbContext.Actions.Local;
            var events = BaseLayer.DssDbContext.Events.Local;
            foreach (var action in actions)
                foreach (var even in events)
                    if (!ActionContainsInCombinations(action, combinations) || !EventContainsInCombinations(even, combinations))
                    {
                        BaseLayer.BaseMethods.AddCombination(CreateCombinationTemplate(), action, even, BaseLayer.Task, 0);
                    }
            InitCombinationViewModel();
        }
        protected abstract void InitCombinationViewModel();

        protected List<Combination> GetCombinations(BaseLayer baseLayer)
        {
            return baseLayer.DssDbContext.Combinations.Local.ToList();
        }
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
            BaseLayer.SolveThisTask(null);
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
            if (Navigation != null) Navigation.Navigate(ContentPage);
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
 
    }
}
