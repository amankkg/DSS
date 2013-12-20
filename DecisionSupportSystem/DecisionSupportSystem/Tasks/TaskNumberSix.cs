using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;
using CoinGameClassesLibrary;

namespace DecisionSupportSystem.Tasks
{
    class TaskNumberSix: TaskSpecific
    {
        #region Поля

        public PageOptionsTask6ViewModel PageOptionsTask6ViewModel { get; set; }

        private Coin gameCoin;
        private List<int> numberOfHeadsInOutcomes, numberOfTailsInOutcomes, numberOfDoubleHeadsInOutcomes;
        private char[] InitialEvents;
        private ObservableCollection<Action> actions;
        private ObservableCollection<Event> events;
        private ObservableCollection<Combination> combinations;
        private EventParamName numberOfHeads, numberOfTails, numberOfDoubleHeads;
        private TaskParamName numberOfThrowings, headBonus, tailCost, doubleHeadBonus;
        private TaskParam numberOfThrowingsValue, headBonusValue, tailCostValue, doubleHeadBonusValue;

        #endregion
        #region Методы
        public void GenerateActions()
        {
            actions = BaseLayer.DssDbContext.Actions.Local;
            actions.Clear();
            actions.Add(new Action
            {
                Name = "Играть"
            });
            actions.Add(new Action
            {
                Name = "Не играть"
            });
        }
        public void GenerateEvents()
        {
            events = BaseLayer.DssDbContext.Events.Local;
            events.Clear();
            decimal probability = 1m / gameCoin.Outcomes.Count;
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {
                var name = new char[(int)BaseLayer.Task.TaskParams.ToList()[0].Value];
                for (int j = 0; j < gameCoin.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[gameCoin.Outcomes[i][j]];
                }
                var ev = new Event
                {
                    Name = new string(name),
                    Probability = probability
                };
                ev.EventParams.Add(new EventParam { EventParamName = numberOfHeads, Value = numberOfHeadsInOutcomes[i] });
                ev.EventParams.Add(new EventParam { EventParamName = numberOfTails, Value = numberOfTailsInOutcomes[i] });
                ev.EventParams.Add(new EventParam { EventParamName = numberOfDoubleHeads, Value = numberOfDoubleHeadsInOutcomes[i] });
                events.Add(ev);
            }
            //events.Add(new Event() { Name = "Ничего не происходит", Probability = 1 });
        }
        public void GenerateCombinations()
        {
            combinations = BaseLayer.DssDbContext.Combinations.Local;
            combinations.Clear();
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {
                Combination combo = new Combination
                {
                    Action = actions[0],
                    Event = events[i],
                    Task = BaseLayer.Task
                };
                combo.Cp = CPFunction(events[i].EventParams.ToList()[0].Value, events[i].EventParams.ToList()[1].Value, events[i].EventParams.ToList()[2].Value);
                combinations.Add(combo);
            }
            //combinations.Add(new Combination() { Action = actions[1], /*Event = new Event() { Name = "-", Probability = 1, EventParams = new List<EventParam> { new EventParam { Value = 0 }, new EventParam { Value = 0 }, new EventParam { Value = 0 }, } },*/ Cp = 0, Task = baseLayer.Task });
        }
        decimal CPFunction(decimal _numberOfHeads, decimal _numberOfTails, decimal _numberOfDoubleHeads)
        {
            return _numberOfHeads * headBonusValue.Value - _numberOfTails * tailCostValue.Value + _numberOfDoubleHeads * doubleHeadBonusValue.Value;
        }
	    #endregion

        public override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageOptions = new PageOptionsTask6 {DataContext = this};
            ContentPage.Content = pageOptions;
            _navigation = NavigationService.GetNavigationService(pageOptions);
            ShowNavigationWindow(ContentPage);
        }

        protected override void InitViewModels()
        {
            PageOptionsTask6ViewModel = new PageOptionsTask6ViewModel(BaseLayer,TaskParamErrorCatcher);
        }

        protected override void CreateTaskParamsTemplate()
        {
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во бросков" } });
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во Г" } });
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во Р" } });
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во ГГ" } });
        }
    }
}
