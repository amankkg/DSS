using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSupportSystem.Interfaces.Task_1_3
{
    public class CpMaxTask
    {
        public Event Event { get; set; }
        public decimal Value { get; set; }
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
        public List<Event> Events { get; set; }
        public List<Combination> Combinations { get; set; }
        public List<CpMaxTask> CpMaxes { get; set; }
        public Task Task { get; set; }
        public DSSDBEntities DbContext { get; set; }

        public TaskLayer()
        { 
            Actions = new List<Action>();
            Events = new List<Event>();
            Combinations = new List<Combination>();
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
                    var combin = new Combination();
                    combin.Action = a;
                    combin.Event = e;
                    combin.Task = Task;
                    Combinations.Add(combin);
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
    }
}
