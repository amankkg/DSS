using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiceGameClassesLibrary;

namespace DecisionSupportSystem.Interfaces.Task_7
{
    public class Model
    {
        public string[] InitialEvents;
        public Dice game;
        public TaskSolvingParam stake;
        public EventParam bonus;
        public List<EventParamValue> bonuses;
        public CombinationParam soeg;
        public List<CombinationParamValue> soegs;

        public void SetStakeValue(decimal value)
        {
            stake.Value = value;
        }//задаем значение ставке в игре

        public void GenerateEvents()
        {
            mainmodel.events = new List<Event>();
            string name;
            decimal probability = 1 / game.Outcomes.Count;
            foreach (var item in game.Outcomes)
            {
                Event ev = new Event();
                name = "";
                foreach (var eventNumber in item)
                {
                    name += InitialEvents[eventNumber] + "-";
                }
                name = name.Remove(name.Length - 1);
                ev.Name = name;
                ev.Probability = probability;
                mainmodel.events.Add(ev);
            }
        }//генерим события из исходов игры

        public void SetEventBonuses(List<decimal> valuelist)
        {
            bonuses = new List<EventParamValue>();
            for (int i = 0; i < mainmodel.events.Count; i++)
            {
                bonuses.Add(new EventParamValue() { EventParam = bonus, Event = mainmodel.events[i], Value = valuelist[i] });
            }
        }//задаем значения бонусам при исходах

        public void GenerateActions()
        {
            mainmodel.actions = new List<Action>();
            string name;
            foreach (var stake in game.Stakes)
            {
                Action act = new Action();
                name = "";
                foreach (var item in stake)
                {
                    name += InitialEvents[item] + "-";
                }
                name = name.Remove(name.Length - 1);
                act.Name = name;
                mainmodel.actions.Add(act);
            }
        }//генерим действия из ставок игры

        public void GenerateCombinations()
        {
            mainmodel.combinations = new List<Combination>();
            foreach (var item in game.Combinations)
            {
                Combination combo = new Combination() { Event = mainmodel.events[item.AcutalOutcome], Action = mainmodel.actions[item._ChoosenStake], TaskSolving = mainmodel.solving };
                mainmodel.combinations.Add(combo);
            }
        }//генерим комбинации из комбинаций игры

        public void SetCombinationSoEGs(List<bool> valuelist)
        {
            soegs = new List<CombinationParamValue>();
            for (int i = 0; i < mainmodel.combinations.Count; i++)
            {
                soegs.Add(new CombinationParamValue() { CombinationParam = soeg, Combination = mainmodel.combinations[i], Value = Convert.ToDecimal(valuelist[i]) });
            }
        }//задаем значения параметра SoEG из комбинаций игры

        public decimal CPFunction(decimal bonusvalue, decimal soegvalue)
        {
            return stake.Value * bonusvalue * soegvalue - stake.Value;
        }//функция считает значение CP

        public void SetCPs()
        {
            mainmodel.cps = new List<ConditionalProfit>();
            foreach (var item in mainmodel.combinations)
            {
                mainmodel.cps.Add(new ConditionalProfit()
                {
                    Combination = item,
                    Value = CPFunction(Convert.ToDecimal(from x in bonuses
                                                         where x.Event == item.Event
                                                         select x.Value),
                                                         Convert.ToDecimal(from x in soegs
                                                                           where x.Combination == item
                                                                           select x.Value))
                });
            }
        }//задаем значения CP

        public Model(MainModel mainmodel)
        {   //конструктор класса, создает задачу и константу (только одна, Ставка)
            this.mainmodel = mainmodel;
            mainmodel.solving = new TaskSolving();
            stake = new TaskSolvingParam() { Name = "Ставка", TaskSolving = mainmodel.solving };
            bonus = new EventParam() { Name = "Бонус" };
            soeg = new CombinationParam() { Name = "SoEG" };
        }//конструктор класса
    }
}
