using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionSupportSystem
{
    public class ConditionalOpportLoss
    {
        #region
        private Guid _id;
        private double _value;
        private ConditionalProfit _conditionalProfit;
        #endregion
        
        #region Свойства
        public Guid Id{get { return _id; }}
        public double Value{get { return _value; }}
        public ConditionalProfit ConditionalProfit{get { return _conditionalProfit; }}
        public static List<ConditionalProfit> ConditionalProfits { get; set; } 
        #endregion 

        public ConditionalOpportLoss(ConditionalProfit conditionalProfit)
        {
            _id = Guid.NewGuid();
            _conditionalProfit = conditionalProfit;
            var cpsForCurrentEvent = (from cp in ConditionalProfits
                     where cp.Combination.Event == conditionalProfit.Combination.Event
                     select cp.Value).ToList();
            _value = cpsForCurrentEvent.Max() - conditionalProfit.Value;
        }
    }

    public class WeightedOpportLoss
    {
        #region
        private Guid _id;
        private double _value;
        private ConditionalOpportLoss _conditionalOpportLoss;
        #endregion

        #region Свойства
        public Guid Id{get { return _id; }}
        public double Value { get { return _value; } }
        public ConditionalOpportLoss ConditionalOpportLoss { get { return _conditionalOpportLoss; } }
        #endregion

        public WeightedOpportLoss(ConditionalOpportLoss col)
        {
            _id = Guid.NewGuid();
            _value = col.Value * col.ConditionalProfit.Combination.Event.Probability;
            _conditionalOpportLoss = col;
        }
    }

    public class ExpectedOpportLoss
    {
        #region
        private Guid    _id;
        private double  _value;
        private List<WeightedOpportLoss> _weightedOpportLosses;
        private Action _action;
        #endregion

        #region Свойства
        public Guid      Id {get { return _id; }}
        public double    Value{get { return _value; }}
        public List<WeightedOpportLoss> WeightedOpportLosses { get { return _weightedOpportLosses; } }
        public Action Action { get { return _action; } }
        #endregion

        public ExpectedOpportLoss(List<WeightedOpportLoss> weightedOpportLosses, Action action)
        {
            _id = Guid.NewGuid();
            _weightedOpportLosses = weightedOpportLosses;
            _value = 0;
            _action = action;
            foreach (var wol in WeightedOpportLosses)
                _value = _value +  wol.Value;
        }
    }
    
    public class SolveEol
    {
        #region
        private List<ConditionalProfit>     _conditionalProfits;
        private List<ConditionalOpportLoss> _conditionalOpportLosses; 
        private List<WeightedOpportLoss>    _weightedOpportLosses;
        private List<ExpectedOpportLoss>    _expectedOpportLosses;
        #endregion
        
        #region Свойства
        public List<ConditionalProfit>     ConditionalProfits { get { return _conditionalProfits; } }
        public List<ConditionalOpportLoss> ConditionalOpportLosses { get { return _conditionalOpportLosses; } }
        public List<WeightedOpportLoss>    WeightedOpportLosses { get { return _weightedOpportLosses; } }
        public List<ExpectedOpportLoss>    ExpectedOpportLosses{ get { return _expectedOpportLosses; } }
        #endregion
        
        public SolveEol(List<ConditionalProfit> conditionalProfits)
        {
            _conditionalProfits = conditionalProfits;
            ConditionalOpportLoss.ConditionalProfits = conditionalProfits;
        }

        public List<ExpectedOpportLoss> Solve(List<Action> actions)
        {
            _conditionalOpportLosses = new List<ConditionalOpportLoss>();
            _weightedOpportLosses = new List<WeightedOpportLoss>();
            _expectedOpportLosses = new List<ExpectedOpportLoss>();
            foreach (var cp in _conditionalProfits)
            {
                _conditionalOpportLosses.Add(new ConditionalOpportLoss(cp));
            }
            foreach (var col in _conditionalOpportLosses)
            {
                _weightedOpportLosses.Add(new WeightedOpportLoss(col));
            }
            foreach (var act in actions)
            {
                var wolsByAction = from wol in _weightedOpportLosses
                            where wol.ConditionalOpportLoss.ConditionalProfit.Combination.Action == act
                            select wol;
                _expectedOpportLosses.Add(new ExpectedOpportLoss(wolsByAction.ToList(), act));
            }
            return _expectedOpportLosses;
        }
    }
}
