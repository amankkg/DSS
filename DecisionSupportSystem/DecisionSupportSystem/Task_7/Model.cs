using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DiceGameClassesLibrary;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_7
{
    public class Model
    {
        BaseLayer _baseLayer { get; set; }
        Preferences _preferences;
        char[] InitialEvents;
        ObservableCollection<Event> events { get; set; }
        ObservableCollection<Action> actions { get; set; }
        ObservableCollection<Combination> combinations { get; set; }
        Dice game;
        TaskParamName stake;
        TaskParam stakeValue;
        EventParamName bonus;
        List<EventParam> bonuses;
        CombinParamName soeg;

        public void SetStakeValue(decimal value)
        {
            stakeValue.Value = value;
        }//задаем значение ставке в игре

        public void GenerateEvents()
        {
            events = _baseLayer.DssDbContext.Events.Local;
            char[] name;
            decimal probability = 1m / game.Outcomes.Count;
            for (int i = 0; i < game.Outcomes.Count; i++)
            {
                name = new char[_preferences.numberofthrowings];
                for (int j = 0; j < game.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[game.Outcomes[i][j]];
                }
                Event ev = new Event()
                {
                    Name = new string(name),
                    Probability = probability
                };
                ev.EventParams.Add(new EventParam() { EventParamName = bonus });
                events.Add(ev);
            }
        }//генерим события из исходов игры

        /*public void SetEventBonuses(List<decimal> valuelist)
        {
            foreach (var ev in events)
	            {
		            ev.EventParams.Add(new EventParam(){EventParamName = bonus});
	            }

                        bonuses = new List<EventParamValue>();
            for (int i = 0; i < mainmodel.events.Count; i++)
            {
                bonuses.Add(new EventParamValue() { EventParam = bonus, Event = mainmodel.events[i], Value = valuelist[i] });
            }
        }//задаем значения бонусам при исходах*/

        public void GenerateActions()
        {
            actions = _baseLayer.DssDbContext.Actions.Local;
            string name;
            for (int i = 0; i < game.Stakes.Count; i++)
            {
                name = "";
                for (int j = 0; j < game.Stakes[i].Length; j++)
                {
                    name = name + events[game.Stakes[i][j]].Name + " ";
                }
                name = name.Remove(name.Length - 1);
                actions.Add(new Action()
                {
                    Name = name
                });
            }
        }//генерим действия из ставок игры

        public void GenerateCombinations()
        {
            combinations = _baseLayer.DssDbContext.Combinations.Local;
            for (int i = 0; i < game.StakeOutcomeCombinations.Count; i++)
            {
                Combination combo = new Combination()
                {
                    Event = events[game.StakeOutcomeCombinations[i].AcutalOutcome],
                    Action = actions[game.StakeOutcomeCombinations[i]._ChoosenStake],
                    Task = _baseLayer.Task
                };
                combo.CombinParams.Add(new CombinParam() { CombinParamName = soeg, Value = Convert.ToDecimal(game.StakeOutcomeCombinations[i].SoEG) });//задаем значения параметра SoEG из комбинаций игры
                //combo.Cp = CPFunction(combo.Event.EventParams.ToList()[0].Value, combo.CombinParams.ToList()[0].Value);
                combinations.Add(combo);
            }
        }//генерим комбинации из комбинаций игры

        public decimal CPFunction(decimal bonusvalue, decimal soegvalue)
        {
            return stakeValue.Value * bonusvalue * soegvalue - stakeValue.Value;
        }//функция считает значение CP

        public Model(BaseLayer baseLayer, Preferences preferences)
        {   //конструктор класса, создает задачу и константу (только одна, Ставка)
            _baseLayer = baseLayer;
            _preferences = preferences;
            stake = new TaskParamName() { Name = "Ставка" };
            stakeValue = new TaskParam { Task = _baseLayer.Task };
            bonus = new EventParamName() { Name = "Бонус" };
            soeg = new CombinParamName() { Name = "SoEG" };

            InitialEvents = preferences.evenoddGame
                                ? InitialEvents = preferences.evenoddNames
                                : InitialEvents = preferences.numericNames;
            game = new Dice(InitialEvents.Length, preferences.numberofthrowings); //, preferences.numberofoutcomesperstake);
            GenerateEvents();
        }//конструктор класса*/     
    }
}
