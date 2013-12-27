using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;
using DiceGameClassesLibrary;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberSeven: TaskSpecific
    {
        #region ViewModel'ы
        public ActionsViewModel ActionsViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; } 
        #endregion

        #region Свойства, специфика задачи
        private Dice gameDice;
        private char[] InitialEvents;
        private char[] evenoddNames = new char[] { 'Ч', 'Н' };
        private char[] numericNames = new char[] { '1', '2', '3', '4', '5', '6' };
        private EventParamName bonus;
        private CombinParamName SoEG; 
        #endregion

        public TaskNumberSeven()
        {
            bonus = new EventParamName() { Name = "Бонус" };
            SoEG = new CombinParamName() { Name = "Совпадение" };
            InitErrorCatchers();
        }

        #region Методы, специфика задачи
        private void GenerateEvents()
        {
            EventsViewModel.Events.Clear();
            EventsViewModel.EventViewModels.Clear();
            char[] name;
            double probability = (double)1 / (double)gameDice.Outcomes.Count;
            for (int i = 0; i < gameDice.Outcomes.Count; i++)
            {
                name = new char[(int)BaseAlgorithms.Task.TaskParams.ToList()[0].Value];
                for (int j = 0; j < gameDice.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[gameDice.Outcomes[i][j]];
                }
                Event ev = new Event()
                {
                    Name = new string(name),
                    Probability = probability,
                    SavingId = base.SavingID
                };
                ev.EventParams.Add(new EventParam() { EventParamName = bonus });
                EventsViewModel.AddEvent(ev);
            }
        }
        private void GenerateActions()
        {
            ActionsViewModel.Actions.Clear();
            ActionsViewModel.ActionViewModels.Clear();
            string name;
            for (int i = 0; i < gameDice.Stakes.Count; i++)
            {
                name = "";
                for (int j = 0; j < gameDice.Stakes[i].Length; j++)
                {
                    name = name + EventsViewModel.Events[gameDice.Stakes[i][j]].Name + " ";
                }
                name = name.Remove(name.Length - 1);
                ActionsViewModel.AddAction(
                    new Action()
                    {
                        Name = name,
                        SavingId = base.SavingID
                    });
            }
        }
        private void GenerateCombinations()
        {
            CombinationsViewModel.Combinations.Clear();
            CombinationsViewModel.CombinationViewModels.Clear();
            for (int i = 0; i < gameDice.StakeOutcomeCombinations.Count; i++)
            {
                Combination combo = new Combination()
                {
                    Event = EventsViewModel.Events[gameDice.StakeOutcomeCombinations[i].AcutalOutcome],
                    Action = ActionsViewModel.Actions[gameDice.StakeOutcomeCombinations[i]._ChoosenStake],
                    Task = BaseAlgorithms.Task,
                    SavingId = base.SavingID
                };
                combo.CombinParams.Add(new CombinParam() { CombinParamName = SoEG, Value = Convert.ToDouble(gameDice.StakeOutcomeCombinations[i].SoEG) });//задаем значения параметра SoEG из комбинаций игры
                combo.Cp = CPFunction(combo.Event.EventParams.ToList()[0].Value, combo.CombinParams.ToList()[0].Value);
                CombinationsViewModel.Combinations.Add(combo);
            }
            CombinationsViewModel = new CombinationsViewModel(CombinationsViewModel.Combinations, CombinationErrorCatcher);
        }
        private double CPFunction(double bonusvalue, double soegvalue)
        {
            return TaskParamsViewModel.Task.TaskParams.ToList()[2].Value * bonusvalue * soegvalue - TaskParamsViewModel.Task.TaskParams.ToList()[2].Value;
        } 
        #endregion

        #region Переопределение, специфика задачи
        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher);
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
        }
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }
        protected override int GetEventsCount()
        {
            return EventsViewModel.Events.Count;
        }
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
            return new Combination { SavingId = this.SavingID };
        }
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во бросков:" }, Value = 2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во исходов на ставку:" }, Value = 2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Сумма ставки:" }, Value = 1 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Режим: 0 - Ч/Н, иначе - 1/2/3/4/5/6:" }, Value = 0 });
        } 
        #endregion

        #region Навигация
        protected override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMain = new PageMainUE { DataContext = this };
            ContentPage.Content = pageMain;
            Navigation = NavigationService.GetNavigationService(pageMain);
            ShowNavigationWindow(ContentPage);
        }
        public override void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            InitialEvents = (TaskParamsViewModel.Task.TaskParams.ToList()[3].Value == 0 ? InitialEvents = evenoddNames : numericNames);
            gameDice = new Dice(InitialEvents.Length, (int)TaskParamsViewModel.Task.TaskParams.ToList()[0].Value, (int)TaskParamsViewModel.Task.TaskParams.ToList()[1].Value);
            GenerateEvents();
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageMainUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateActions();
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageSolveUE { DataContext = this };
            BaseAlgorithms.SolveTask(null);
            Navigate();
        }
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        } 
        #endregion
    }
}
