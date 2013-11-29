using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using CoinGameClassesLibrary;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Task_6
{
    public class Model
    {
        Coin game;
        List<int> numberOfHeadsInOutcomes, numberOfTailsInOutcomes, numberOfDoubleHeadsInOutcomes;
        BaseLayer baseLayer;
        Preferences preferences;
        char[] InitialEvents;
        ObservableCollection<Action> actions;
        ObservableCollection<Event> events;
        ObservableCollection<Combination> combinations;
        EventParamName numberOfHeads, numberOfTails, numberOfDoubleHeads;
        TaskParamName headBonus, tailCost, doubleHeadBonus;
        TaskParam headBonusValue, tailCostValue, doubleHeadBonusValue;

        public void GenerateActions()
        {
            actions = baseLayer.DssDbContext.Actions.Local;
            actions.Add(new Action()
                {
                    Name = "Играть"
                });
            actions.Add(new Action()
                {
                    Name = "Не играть"
                });
        }

        public void GenerateEvents()
        {
            events = baseLayer.DssDbContext.Events.Local;
            char[] name;
            decimal probability = 1m / game.Outcomes.Count;
            for (int i = 0; i < game.Outcomes.Count; i++)
            {
                name = new char[preferences.NumberOfThrowings];
                for (int j = 0; j < game.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[game.Outcomes[i][j]];
                }
                Event ev = new Event()
                {
                    Name = new string(name),
                    Probability = probability
                };
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfHeads, Value = numberOfHeadsInOutcomes[i] });
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfTails, Value = numberOfTailsInOutcomes[i] });
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfDoubleHeads, Value = numberOfDoubleHeadsInOutcomes[i] });
                events.Add(ev);
            }
            //events.Add(new Event() { Name = "Ничего не происходит", Probability = 1 });
        }

        public void GenerateCombinations()
        {
            combinations = baseLayer.DssDbContext.Combinations.Local;
            for (int i = 0; i < game.Outcomes.Count; i++)
            {
                Combination combo = new Combination()
                {
                    Action = actions[0],
                    Event = events[i],
                    Task = baseLayer.Task
                };
                combo.Cp = CPFunction(events[i].EventParams.ToList()[0].Value, events[i].EventParams.ToList()[1].Value, events[i].EventParams.ToList()[2].Value);
                combinations.Add(combo);
            }
            combinations.Add(new Combination() { Action = actions[1], /*Event = events.Last(),*/ Cp = 0, Task = baseLayer.Task });
        }

        decimal CPFunction(decimal _numberOfHeads, decimal _numberOfTails, decimal _numberOfDoubleHeads)
        {
            return _numberOfHeads * headBonusValue.Value - _numberOfTails * tailCostValue.Value + _numberOfDoubleHeads * doubleHeadBonusValue.Value;
        }

        public Model(BaseLayer _baseLayer, Preferences _preferences)
        {
            baseLayer = _baseLayer;
            preferences = _preferences;
            numberOfHeads = new EventParamName() { Name = "Кол-во Г" };
            numberOfTails = new EventParamName() { Name = "Кол-во Р" };
            numberOfDoubleHeads = new EventParamName() { Name = "Кол-во ГГ" };
            headBonus = new TaskParamName() { Name = "Бонус за Г" };
            tailCost = new TaskParamName() { Name = "Бонус за Р" };
            doubleHeadBonus = new TaskParamName() { Name = "Бонус за ГГ" };
            headBonusValue = new TaskParam() { Task = baseLayer.Task, Value = preferences.HeadBonus };
            tailCostValue = new TaskParam() { Task = baseLayer.Task, Value = preferences.TailCost };
            doubleHeadBonusValue = new TaskParam() { Task = baseLayer.Task, Value = preferences.DoubleHeadBonus };
            InitialEvents = new char[]{'Г', 'Р'};
            game = new Coin(InitialEvents.Length, preferences.NumberOfThrowings);
            numberOfHeadsInOutcomes = game.CountSequences(0);
            numberOfTailsInOutcomes = game.CountSequences(1);
            numberOfDoubleHeadsInOutcomes = game.CountSequences(0, 3);
        }
    }
}
