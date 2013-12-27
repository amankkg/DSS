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
using CoinGameClassesLibrary;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberSix: TaskSpecific
    {
        #region Поля
        public ActionsViewModel ActionsViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }
        private Coin gameCoin;
        private List<int> numberOfHeadsInOutcomes, numberOfTailsInOutcomes, numberOfDoubleHeadsInOutcomes;
        private char[] InitialEvents = new char[] { 'Г', 'Р' };
        private EventParamName numberOfHeads, numberOfTails, numberOfDoubleHeads;
        #endregion

        #region Проверка изменений при навигации
        private void checkIfGameRulesChanged()
        {
            for (int i = 0; i < keptTaskParams.Length; i++)
            {
                if (keptTaskParams[i] != TaskParamsViewModel.Task.TaskParams.ToList()[i].Value)
                {
                    gameRulesChanged = true;
                    needInEventsReGeneration = true;
                    needInCombinationsReGeneration = true;
                    needInTaskReSolving = true;
                    break;
                }
            }
        }
        private bool gameRulesChanged = true;
        private bool needInEventsReGeneration = true;
        private bool needInCombinationsReGeneration = true;
        private bool needInTaskReSolving = true;
        double[] keptTaskParams = new double[4]; 
        #endregion

        #region Методы генерации событий и действий
        private void GenerateActions()
        {
            ActionsViewModel.Actions.Clear();
            ActionsViewModel.ActionViewModels.Clear();
            ActionsViewModel.AddAction(new Action
            {
                Name = "Играть",
                SavingId = base.SavingID
            });
            ActionsViewModel.AddAction(new Action
            {
                Name = "Не играть",
                SavingId = base.SavingID
            });
        }
        private void GenerateEvents()
        {
            EventsViewModel.Events.Clear();
            EventsViewModel.EventViewModels.Clear();
            double probability = (double) 1 / (double) gameCoin.Outcomes.Count;
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {
                var name = new char[(int)BaseAlgorithms.Task.TaskParams.ToList()[0].Value];
                for (int j = 0; j < gameCoin.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[gameCoin.Outcomes[i][j]];
                }
                var ev = new Event
                {
                    Name = new string(name),
                    Probability = probability,
                    SavingId = base.SavingID
                };
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfHeads, Value = numberOfHeadsInOutcomes[i] });
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfTails, Value = numberOfTailsInOutcomes[i] });
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfDoubleHeads, Value = numberOfDoubleHeadsInOutcomes[i] });
                EventsViewModel.AddEvent(ev);
            }
        }
        private void GenerateCombinations()
        {
            CombinationsViewModel.Combinations.Clear();
            CombinationsViewModel.CombinationViewModels.Clear();
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {
                Combination combo = new Combination
                {
                    Action = ActionsViewModel.Actions[0],
                    Event = EventsViewModel.Events[i],
                    Task = BaseAlgorithms.Task,
                    SavingId = base.SavingID
                };
                combo.Cp = CPFunction(EventsViewModel.Events[i].EventParams.ToList()[0].Value, EventsViewModel.Events[i].EventParams.ToList()[1].Value, EventsViewModel.Events[i].EventParams.ToList()[2].Value);
                CombinationsViewModel.Combinations.Add(combo);
            }
            CombinationsViewModel = new CombinationsViewModel(CombinationsViewModel.Combinations, CombinationErrorCatcher);
        }
        private double CPFunction(double _numberOfHeads, double _numberOfTails, double _numberOfDoubleHeads)
        {
            return _numberOfHeads * BaseAlgorithms.Task.TaskParams.ToList()[1].Value - _numberOfTails * BaseAlgorithms.Task.TaskParams.ToList()[2].Value + _numberOfDoubleHeads * BaseAlgorithms.Task.TaskParams.ToList()[3].Value;
        }
        #endregion

        public TaskNumberSix()
        {
            numberOfHeads = new EventParamName() { Name = "Кол-во Г" };
            numberOfTails = new EventParamName() { Name = "Кол-во Р" };
            numberOfDoubleHeads = new EventParamName() { Name = "Кол-во ГГ" };
            InitErrorCatchers();
        }

        #region Переопределения и инициализация ViewModel'ей
        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher);
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во бросков:" }, Value = 3 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Бонус за Г:" }, Value = 1 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Плата за Р:" }, Value = 1.2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Бонус за ГГ:" }, Value = 0.25 });
        }
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }
        protected override int GetEventsCount()
        {
            return EventsViewModel.Events.Count;
        }
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher)
            {
                ParamsVisibility = Visibility.Hidden
            };
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
            checkIfGameRulesChanged();
            if (gameRulesChanged)
            {
                gameCoin = new Coin(InitialEvents.Length, (int)TaskParamsViewModel.Task.TaskParams.ToList()[0].Value);
                numberOfHeadsInOutcomes = gameCoin.CountSequences(0);
                numberOfTailsInOutcomes = gameCoin.CountSequences(1);
                numberOfDoubleHeadsInOutcomes = gameCoin.CountSequences(0, 2);
                GenerateActions();
                gameRulesChanged = false;
            }
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            for (int i = 0; i < keptTaskParams.Length; i++)
            {
                keptTaskParams[i] = TaskParamsViewModel.Task.TaskParams.ToList()[i].Value;
            }
            ContentPage.Content = new PageMainUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            if (needInEventsReGeneration)
            {
                GenerateEvents();
                needInEventsReGeneration = false;
            }
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            if (needInCombinationsReGeneration)
            {
                GenerateCombinations();
                needInCombinationsReGeneration = false;
            }
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            if (needInTaskReSolving)
            {
                BaseAlgorithms.SolveTask(null);
                needInTaskReSolving = false;
            }
            ContentPage.Content = new PageSolveUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this };
            Navigate();
        }
        #endregion
    }
}