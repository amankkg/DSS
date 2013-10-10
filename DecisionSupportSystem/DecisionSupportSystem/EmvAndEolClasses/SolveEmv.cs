using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSupportSystem
{
    public class WeightedProfitS
    {
        #region закрытые
        private Guid _id;
        private double _value;
        private ConditionalProfit _conditionalProfit;
        #endregion

        #region Свойства
        public Guid Id{get { return _id; }}

        public double Value{get { return _value; }}

        public ConditionalProfit ConditionalProfit{get { return _conditionalProfit; }}
        #endregion


        public WeightedProfitS(ConditionalProfit cp)
        {
            _id = Guid.NewGuid();
            _value = cp.Value * (double) cp.Combination.Event.Probability;
            _conditionalProfit = cp;
        }
    }

    public class ExpectedMonetaryValueS
    {
        #region закрытые
        private Guid    _id;
        private double  _value;
        private List<WeightedProfitS> _weightedProfits;
        #endregion

        #region Свойства
        public Guid      Id {get { return _id; }}
        public double    Value{get { return _value; }}
        public List<WeightedProfitS> WeightedProfits { get { return _weightedProfits; } }
        #endregion


        public ExpectedMonetaryValueS(List<WeightedProfitS> weightedProfits)
        {
            _id = Guid.NewGuid();
            _weightedProfits = weightedProfits;
            _value = 0;
            foreach (var wp in WeightedProfits)
                _value = _value + (double) wp.Value;
        }
    }

    public class SolveEmv
    {
        //коммент
        #region закрытые
        private List<WeightedProfitS> _weightedProfits;
        private List<ConditionalProfit>     _conditionalProfits;
        private List<ExpectedMonetaryValueS> _expectedMonetaryValues;
        #endregion

        #region Свойства
        public List<WeightedProfitS> WeightedProfits { get { return _weightedProfits; } }
        public List<ConditionalProfit>     ConditionalProfits { get { return _conditionalProfits; } }
        public List<ExpectedMonetaryValueS> ExpectedMonetaryValues { get { return _expectedMonetaryValues; } } 
        #endregion


        public SolveEmv(List<ConditionalProfit> conditionalProfits)
        {
            _conditionalProfits = conditionalProfits;
        }

        public List<ExpectedMonetaryValueS> Solve(List<Action> actions)
        {
            _weightedProfits = new List<WeightedProfitS>();
            _expectedMonetaryValues = new List<ExpectedMonetaryValueS>();
            foreach (var cp in _conditionalProfits)
            {
                _weightedProfits.Add(new WeightedProfitS(cp));
            }
            foreach (var act in actions)
            {
                var wprofitsByAction = from wp in _weightedProfits
                            where wp.ConditionalProfit.Combination.Action == act
                            select wp;
                _expectedMonetaryValues.Add(new ExpectedMonetaryValueS(wprofitsByAction.ToList()));
            }
            return _expectedMonetaryValues;
        }

    }
}
