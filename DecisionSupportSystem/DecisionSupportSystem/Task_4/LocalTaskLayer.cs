using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_4
{
    public class CombinationWithParamView
    {
        public Combination Combination { get; set; }
        public CombinParam Procent { get; set; }
        public CombinParam NominalPrice { get; set; } 
    }

    public class Task4CombinationsView
    {
        public List<CombinationWithParamView> Temps = new List<CombinationWithParamView>();
        public BaseLayer BaseLayer  = new BaseLayer();

        public Task4CombinationsView(BaseLayer baseLayer)
        {
            BaseLayer = baseLayer;
        }
        
        public void CreateCombinations()
        {
            LoadCombinations();
            var lastCombList = CreateLastCombinationList();
            var actions = BaseLayer.DssDbContext.Actions.Local.ToList();
            var events = BaseLayer.DssDbContext.Events.Local.ToList();
            foreach (var action in actions)
                foreach (var eEvent in events)
                    if (!HaveAction(action, lastCombList) || !HaveEvent(eEvent, lastCombList))
                    {
                        var combination = new Combination();
                        BaseLayer.BaseMethods.AddCombination(combination, action, eEvent, BaseLayer.Task, 0);
                        var procent = new CombinParam { Combination = combination, };
                        var nominalprice = new CombinParam { Combination = combination, };
                        BaseLayer.BaseMethods.AddCombinationParams(new List<CombinParam> { procent, nominalprice });

                        Temps.Add(new CombinationWithParamView
                        {
                            Combination = combination,
                            Procent = procent, 
                            NominalPrice = nominalprice
                        });
                    }
        }

        private void LoadCombinations()
        {
            Temps.Clear();
            var combins = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combin in combins)
            {
                var procent = combin.CombinParams.ToList()[0];
                var nominalprice = combin.CombinParams.ToList()[1];

                Temps.Add(new CombinationWithParamView
                {
                    Combination = combin,
                    Procent = procent,
                    NominalPrice = nominalprice
                });
            }
        }
        
        private List<Combination> CreateLastCombinationList()
        {
            var comb = BaseLayer.DssDbContext.Combinations.Local.ToList();
            return comb.Select(c => new Combination
                {
                    Action = c.Action, Event = c.Event
                }).ToList();
        }

        private bool HaveAction(Action act, List<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Action == act);
        }

        private bool HaveEvent(Event eEvent, List<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Event == eEvent);
        }
        
        public void SolveCp()
        {
            foreach (var temp in Temps)
            {
                temp.Combination.Cp = temp.Procent.Value*(temp.NominalPrice.Value + 100)/100;
            }
        }

    }
}
