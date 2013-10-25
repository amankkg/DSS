using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSupportSystem.Interfaces.Task_5
{
    public class CpMaxTask
    {
       /* public Event Event { get; set; }
        public decimal Value { get; set; }*/
    }

   /* public class EventTask5 : Event
    {
        public Action Action { get; set; }
    }
    public class CombinationTask5 : Combination
    {
        public CombinationTask5()
        {
            Debit = new Parameter();
            Credit = new Parameter();
        }
        public Parameter Debit { get; set; }
        public Parameter Credit { get; set; }
    }
    public class FormatDataForDatagrid
    {
        public Action Action { get; set; }
        public Event Event { get; set; }
        public decimal Cp { get; set; }
        public decimal Wp { get; set; }
        public decimal Col { get; set; }
        public decimal Wol { get; set; }

        public FormatDataForDatagrid(Combination combination)
        {
            Action = combination.Action;
            Event = combination.Event;
            if (combination.ConditionalProfit != null)
                Cp = combination.ConditionalProfit.Value;
            if (combination.WeightedProfit != null)
                Wp = combination.WeightedProfit.Value;
            if (combination.ConditionalOpportunityLoss != null)
                Col = combination.ConditionalOpportunityLoss.Value;
            if (combination.WeightedOpportunityLoss != null)
                Wol = combination.WeightedOpportunityLoss.Value;
        }
    }

    public class TaskLayer
    {
        public List<Action> Actions { get; set; }
        public List<EventTask5> Events { get; set; }
        public List<CombinationTask5> Combinations { get; set; }
        public List<CpMaxTask> CpMaxes { get; set; }
        public Task Task { get; set; }
        public DSSDBEntities DbContext { get; set; }

        public TaskLayer()
        { 
            Actions = new List<Action>();
            Events = new List<EventTask5>();
            Combinations = new List<CombinationTask5>();
            CpMaxes = new List<CpMaxTask>();
            Task = new Task
                {
                    Date = DateTime.Now,
                    Comment = "Some Comment",
                    Recommendation = "Some recommendation"
                };
        }

        public void CreateActionEventCombination()
        {
            foreach (var a in Actions)
                foreach (var e in Events)
                {
                    if (e.Action==a)
                    {
                    var combin = new CombinationTask5();
                    combin.Action = a;
                    combin.Event = e;
                    combin.Task = Task;
                    Combinations.Add(combin);
                    }
                }
        }

        private void SolveCpMaxes()
        {
            foreach (var e in Events)
            {
                var cpsForCurrentEvent = (from c in Combinations
                                          where c.Event == e
                                          select c.ConditionalProfit).ToList();
                var max = cpsForCurrentEvent.Max();
                if (max != null)
                    CpMaxes.Add(new CpMaxTask
                        {
                            Event = e,
                            Value = (decimal)max,
                        });
            }
        }

        public void SolveCp()
        {
            foreach (var c in Combinations)
            {
                c.ConditionalProfit = c.Debit.Value - c.Credit.Value;
            }
        }

        private void SolveWpsColsWols()
        {
            SolveCpMaxes();
            foreach (var c in Combinations)
            {
                c.WeightedProfit = c.ConditionalProfit * c.Event.Probability;
                var cpMax = CpMaxes.First(i => i.Event == c.Event).Value;
                c.ConditionalOpportunityLoss = cpMax - c.ConditionalProfit;
                c.WeightedOpportunityLoss = c.ConditionalOpportunityLoss * c.Event.Probability;
            }
        }

        private void SolveEmvAndEol()
        {
            foreach (var a in Actions)
            {
                var wps = (from c in Combinations
                           where c.Action == a
                           select c.WeightedProfit).ToList();
                a.ExpectedMonetaryValue = wps.Sum();

                var wols = (from c in Combinations
                            where c.Action == a
                            select c.WeightedOpportunityLoss).ToList();
                a.ExpectedOpportunityLoss = wols.Sum();
            }
        }

        private void AddEventsIntoDbContext()
        {
            foreach (var e in Events)
            {
                DbContext.Events.Add(e);
            }
        }

        private void AddActionsIntoDbContext()
        {
            foreach (var a in Actions)
            {
               DbContext.Actions.Add(a);
            }
        }

        private void AddCombinationsIntoDbContext()
        {
            foreach (var c in Combinations)
            {
                DbContext.Combinations.Add(c);
            }
        }

        public void SolveAllEntities()
        {
            SolveCp();
            SolveWpsColsWols();
            SolveEmvAndEol();
        }

        public void AddAllEntitiesIntoDbContextAndSave()
        {
            using (DbContext = new DSSDBEntities())
            { 
                AddActionsIntoDbContext();
                AddEventsIntoDbContext();
                AddCombinationsIntoDbContext();
                DbContext.SaveChanges();
            }
        }
    }*/
}
