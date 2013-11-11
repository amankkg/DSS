using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_4
{
    public class CombinationWithParamView : BasePropertyChanged
    {
        private Combination _combination;
        public Combination Combination
        {
            get
            {
                return _combination;
            }
            set
            {
                if (value != this._combination)
                {
                    this._combination = value;
                    RaisePropertyChanged("Combination");
                }
            }
        }
        public CombinParam Procent { get; set; }
        public CombinParam NominalPrice { get; set; } 
    }

    public class TaskCombinationsView
    {
        public List<CombinationWithParamView> CombinationWithParamViews = new List<CombinationWithParamView>();
        public BaseLayer BaseLayer  = new BaseLayer();

        public TaskCombinationsView(){}

        public TaskCombinationsView(BaseLayer baseLayer)
        {
            BaseLayer = baseLayer;
        }
        
        public virtual void CreateCombinations()
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

                        CombinationWithParamViews.Add(new CombinationWithParamView
                        {
                            Combination = combination,
                            Procent = procent, 
                            NominalPrice = nominalprice
                        });
                    }
        }

        protected void LoadCombinations()
        {
            CombinationWithParamViews.Clear();
            var combins = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combin in combins)
            {
                var procent = combin.CombinParams.ToList()[0];
                var nominalprice = combin.CombinParams.ToList()[1];

                CombinationWithParamViews.Add(new CombinationWithParamView
                {
                    Combination = combin,
                    Procent = procent,
                    NominalPrice = nominalprice
                });
            }
        }
        
        protected List<Combination> CreateLastCombinationList()
        {
            var comb = BaseLayer.DssDbContext.Combinations.Local.ToList();
            return comb.Select(c => new Combination
                {
                    Action = c.Action, Event = c.Event
                }).ToList();
        }

        protected bool HaveAction(Action act, List<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Action == act);
        }

        protected bool HaveEvent(Event eEvent, List<Combination> lastCombList)
        {
            return lastCombList.Any(combination => combination.Event == eEvent);
        }
        
        public virtual void SolveCp()
        {
            foreach (var temp in CombinationWithParamViews)
            {
                temp.Combination.Cp = temp.Procent.Value*(temp.NominalPrice.Value + 100)/100;
            }
        }

    }
}
