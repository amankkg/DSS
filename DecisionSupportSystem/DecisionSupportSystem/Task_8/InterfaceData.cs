using System;
using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Interfaces.Task_8
{
    /*
   public class ActionIData : Action
   {
       public Parameter RequiredDefect { get; set; }
   }

   public class EventIData : Event
   {
       public Parameter Defect { get; set; }
   }
    
   public class CombinationIData : Combination
   {
       public ActionIData ActionIData { get; set; }
       public EventIData EventIData { get; set; }
   }
   
   public class CpMax
   {
       public EventIData EventIData { get; set; }
       public decimal Value { get; set; }
   }

   public class FormatDataForDatagrid
   {
       public Action Action { get; set; }
       public Event Event { get; set; }
       public Parameter ReqDefect { get; set; }
       public Parameter Defect { get; set; }
       public decimal Cp { get; set; }
       public decimal Wp { get; set; }
       public decimal Col { get; set; }
       public decimal Wol { get; set; }

       public FormatDataForDatagrid(CombinationIData combination)
       {
           Action = combination.ActionIData;
           Event = combination.EventIData;
           ReqDefect = combination.Parameters.ElementAt(0);
           Defect = combination.Parameters.ElementAt(1);
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
    
   public class InterfaceData
    {
        #region Свойства
       // public DSSDBContext DssDbContext { get; set; }
        public Task Task { get; set; }
        public List<ActionIData> ActionIDatas { get; set; }
        public List<EventIData> EventIDatas { get; set; }
        public List<CombinationIData> Combinations { get; set; }
        public List<Constant> Constants { get; set; }
        public List<CpMax> CpMaxes { get; set; }
        #endregion
        
        public InterfaceData()
        {
            ActionIDatas = new List<ActionIData>();
            EventIDatas = new List<EventIData>();
            Combinations = new List<CombinationIData>();
            Constants = new List<Constant>();
            CpMaxes = new List<CpMax>();
            Task = new Task
                {
                    Date = DateTime.Now,
                    //Description = "Задача №8"
                };
        }

       private void CreateCombinationWitnParameters()
       {
            foreach (var a in ActionIDatas)
                foreach (var e in EventIDatas)
                {
                    var combination = new CombinationIData
                        {
                            ActionIData = a, EventIData = e
                        };
                    combination.Parameters.Add(new Parameter
                        {
                            Name = a.RequiredDefect.Name,
                            Value = a.RequiredDefect.Value
                        });
                    combination.Parameters.Add(new Parameter
                        {
                            Name = e.Defect.Name,
                            Value = e.Defect.Value
                        });
                    Combinations.Add(combination);
                }
       }

       private void SolveConditionalAndWeigtedProfits()
       {
           foreach (var c in Combinations)
           {
               var diffDefect = c.EventIData.Defect.Value - 
                                                    c.ActionIData.RequiredDefect.Value;
               // вычисляется условная прибыль
                    if (diffDefect >= 0)
                        c.ConditionalProfit = - diffDefect * 10 * Constants[1].Value;
                    else
                        c.ConditionalProfit = - diffDefect * 10 * Constants[0].Value;
                // вычисляется взвешанная прибыль
                c.WeightedProfit = c.ConditionalProfit * c.EventIData.Probability;
           }
       }

       private void SolveCpMaxes()
       {
           foreach (var e in EventIDatas)
           {
               var cpsForCurrentEvent = (from c in Combinations
                                         where c.EventIData == e
                                         select c.ConditionalProfit).ToList();
               var max = cpsForCurrentEvent.Max();
               if (max != null)
               { 
                   CpMaxes.Add(new CpMax
                                {
                                 EventIData = e,
                                 Value = (decimal) max
                                });
  
               }
           }
       }

       private void SolveConditionalAndWeigtedOppLosses()
       {
           SolveCpMaxes();
           foreach (var c in Combinations)
           {
               // вычисляется условная потеря
               var cpMax = CpMaxes.First(i => i.EventIData == c.EventIData).Value;
               c.ConditionalOpportunityLoss = cpMax - c.ConditionalProfit;

               // вычисляется взвешанная потеря
               c.WeightedOpportunityLoss = c.ConditionalOpportunityLoss*c.EventIData.Probability;
           }
       }
       
       private void SolveEmvAndEol()
       {
           foreach (var a in ActionIDatas)
           {
               var wps = (from c in Combinations
                          where c.ActionIData == a
                          select c.WeightedProfit).ToList();
               a.ExpectedMonetaryValue = wps.Sum();
               var wols = (from c in Combinations
                           where c.ActionIData == a
                           select c.WeightedOpportunityLoss).ToList();
               a.ExpectedOpportunityLoss = wols.Sum();
           }
       }

       public void SolveAll()
       {
           CreateCombinationWitnParameters();
           SolveConditionalAndWeigtedProfits();
           SolveConditionalAndWeigtedOppLosses();
           SolveEmvAndEol();
       }

       /* private void AddCombinationsIntoDbContext()
        {
            foreach (var c in Combinations)
            {
              DssDbContext.Combinations.Add(c);
            }
        }
       
        private void AddActionsIntoDbContext()
        {
            var i = 0;
            foreach (var a in ActionIDatas)
            {
                var action = new Action { Name = a.Name };
                for (int j = 0; j < Combinations.Count; j++)
                    if (j >= i && j < i + EventIDatas.Count)
                        action.Combinations.Add(Combinations[j]);
                i += EventIDatas.Count;
                DssDbContext.Actions.Add(action);
            }
        }

        private void AddEventsIntoDbContext()
        {
             var index = 0;
                foreach (var e in EventIDatas)
                {
                    var i = index;
                    var even = new Event{ Name = e.Name, Probability = e.Probability};
                    for (int j = 0; j < Combinations.Count; j++)
                        if (j == i)
                        {
                            even.Combinations.Add(Combinations[j]);
                            i += EventIDatas.Count;
                        }
                    index++;
                    DssDbContext.Events.Add(even);
                }
        }

       private void AddTasksIntoDbContext()
       {
            foreach (var consT in Constants)
                Task.Constants.Add(new Constant{Name = consT.Name, Value = consT.Value});
            foreach (var c in Combinations)
                Task.Combinations.Add(c);
                DssDbContext.Tasks.Add(Task);
       }

       public void AddAllDatasIntoModel()
        {
            using (DssDbContext = new DSSDBContext())
            {
                AddCombinationsIntoDbContext();
                AddActionsIntoDbContext();
                AddEventsIntoDbContext();
                AddTasksIntoDbContext();
                //DssContext.SaveChanges();
            }
        }
        
    }*/
}
