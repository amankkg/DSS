using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSupportSystem
{
    public class WeightedProfit
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


        public WeightedProfit(ConditionalProfit cp)
        {
            _id = Guid.NewGuid();
            _value = cp.Value * cp.Combination.Event.Probability;
            _conditionalProfit = cp;
        }
    }

    public class ExpectedMonetaryValue
    {
        #region закрытые
        private Guid    _id;
        private double  _value;
        private List<WeightedProfit> _weightedProfits;
        #endregion

        #region Свойства
        public Guid      Id {get { return _id; }}
        public double    Value{get { return _value; }}
        public List<WeightedProfit>     WeightedProfits{get { return _weightedProfits; }}
        #endregion


        public ExpectedMonetaryValue(List<WeightedProfit> weightedProfits)
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
        #region закрытые
        private List<WeightedProfit>        _weightedProfits;
        private List<ConditionalProfit>     _conditionalProfits;
        private List<ExpectedMonetaryValue> _expectedMonetaryValues;
        #endregion

        #region Свойства
        public List<WeightedProfit>        WeightedProfits { get { return _weightedProfits; } }
        public List<ConditionalProfit>     ConditionalProfits { get { return _conditionalProfits; } }
        public List<ExpectedMonetaryValue> ExpectedMonetaryValues { get { return _expectedMonetaryValues; } } 
        #endregion


        public SolveEmv(List<ConditionalProfit> conditionalProfits)
        {
            _conditionalProfits = conditionalProfits;
        }

        public List<ExpectedMonetaryValue> Solve(List<Action> actions)
        {
            _weightedProfits = new List<WeightedProfit>();
            _expectedMonetaryValues = new List<ExpectedMonetaryValue>();
            foreach (var cp in _conditionalProfits)
            {
                _weightedProfits.Add(new WeightedProfit(cp));
            }
            foreach (var act in actions)
            {
                var wprofitsByAction = from wp in _weightedProfits
                            where wp.ConditionalProfit.Combination.Action == act
                            select wp;
                _expectedMonetaryValues.Add(new ExpectedMonetaryValue(wprofitsByAction.ToList()));
            }
            return _expectedMonetaryValues;
        }

    }
}
