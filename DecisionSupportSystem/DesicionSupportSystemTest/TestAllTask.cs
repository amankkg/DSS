using System;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.Task_4;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = DecisionSupportSystem.DbModel.Action;
using Task = DecisionSupportSystem.DbModel.Task;

namespace DesicionSupportSystemTest
{
    [TestClass]
    public class TestAllTasks
    {
        public BaseLayer BaseLayer { get; set; }
        
        [TestMethod]
        public void TestCreateCombinationsMethod()
        {
            BaseLayer = new BaseLayer();
            
            var task1 = new Task();
            BaseLayer.Task = task1;

            var act1 = new Action {Name = "A1"};
            var act2 = new Action {Name = "A2"};
            var event1 = new Event {Name = "E1", Probability = Convert.ToDecimal(0.3)};
            var event2 = new Event {Name = "E2", Probability = Convert.ToDecimal(0.7)};
            BaseLayer.DssDbContext.Actions.Local.Add(act1);
            BaseLayer.DssDbContext.Actions.Local.Add(act2);
            BaseLayer.DssDbContext.Events.Local.Add(event1);
            BaseLayer.DssDbContext.Events.Local.Add(event2);

            DecisionSupportSystem.Task_1.LocalTaskLayer.CreateCombinations(BaseLayer);

            var combins = BaseLayer.DssDbContext.Combinations.Local.ToList();
            Assert.AreEqual("A1", combins[0].Action.Name); 
        }

        [TestMethod]
        public void TestTask_4()
        {
            var baseLayer = new BaseLayer();

            var task1 = new Task();
            baseLayer.Task = task1;

            var act1 = new Action { Name = "A1" };
            var act2 = new Action { Name = "A2" };
            var event1 = new Event { Name = "E1", Probability = Convert.ToDecimal(0.65) };
            var event2 = new Event { Name = "E2", Probability = Convert.ToDecimal(0.2) };
            var event3 = new Event { Name = "E3", Probability = Convert.ToDecimal(0.15) };
            baseLayer.DssDbContext.Actions.Local.Add(act1);
            baseLayer.DssDbContext.Actions.Local.Add(act2);
            baseLayer.DssDbContext.Events.Local.Add(event1);
            baseLayer.DssDbContext.Events.Local.Add(event2);
            baseLayer.DssDbContext.Events.Local.Add(event3);

            DecisionSupportSystem.Task_4.TaskCombinationsView taskCombinationsView = new TaskCombinationsView(baseLayer);
            taskCombinationsView.CreateCombinations();

            var combins = baseLayer.DssDbContext.Combinations.Local.ToList();
            Assert.AreEqual("A1", combins[0].Action.Name, "A1-E1"); Assert.AreEqual("E1", combins[0].Event.Name, "A1-E1");
            Assert.AreEqual("A1", combins[1].Action.Name, "A1-E2"); Assert.AreEqual("E2", combins[1].Event.Name, "A1-E2");
            Assert.AreEqual("A1", combins[2].Action.Name, "A1-E3"); Assert.AreEqual("E3", combins[2].Event.Name, "A1-E3");
            Assert.AreEqual("A2", combins[3].Action.Name, "A2-E1"); Assert.AreEqual("E1", combins[3].Event.Name, "A2-E1");
            Assert.AreEqual("A2", combins[4].Action.Name, "A2-E2"); Assert.AreEqual("E2", combins[4].Event.Name, "A2-E2");
            Assert.AreEqual("A2", combins[5].Action.Name, "A2-E3"); Assert.AreEqual("E3", combins[5].Event.Name, "A2-E3");

            taskCombinationsView.CombinationWithParamViews[0].Procent.Value = Convert.ToDecimal(7.5);
            taskCombinationsView.CombinationWithParamViews[0].NominalPrice.Value = 0;
            taskCombinationsView.CombinationWithParamViews[1].Procent.Value = 8;
            taskCombinationsView.CombinationWithParamViews[1].NominalPrice.Value = 10;
            taskCombinationsView.CombinationWithParamViews[2].Procent.Value = 6;
            taskCombinationsView.CombinationWithParamViews[2].NominalPrice.Value = 5;
            taskCombinationsView.CombinationWithParamViews[3].Procent.Value = 1;
            taskCombinationsView.CombinationWithParamViews[3].NominalPrice.Value = 8;
            taskCombinationsView.CombinationWithParamViews[4].Procent.Value = 8;
            taskCombinationsView.CombinationWithParamViews[4].NominalPrice.Value = 20;
            taskCombinationsView.CombinationWithParamViews[5].Procent.Value = 6;
            taskCombinationsView.CombinationWithParamViews[5].NominalPrice.Value = 20;

            taskCombinationsView.SolveCp();
            Assert.AreEqual(Convert.ToDecimal(7.5), combins[0].Cp);
            Assert.AreEqual(Convert.ToDecimal(8.8), combins[1].Cp);
            Assert.AreEqual(Convert.ToDecimal(6.3), combins[2].Cp);
            Assert.AreEqual(Convert.ToDecimal(1.08), combins[3].Cp);
            Assert.AreEqual(Convert.ToDecimal(9.6), combins[4].Cp);
            Assert.AreEqual(Convert.ToDecimal(7.2), combins[5].Cp);
            
            baseLayer.SolveThisTask();

            Assert.AreEqual(Convert.ToDecimal(4.875), combins[0].Wp); Assert.AreEqual(0, combins[0].Col); Assert.AreEqual(0, combins[0].Wol);
            Assert.AreEqual(Convert.ToDecimal(1.76), combins[1].Wp); Assert.AreEqual(Convert.ToDecimal(0.8), combins[1].Col); Assert.AreEqual(Convert.ToDecimal(0.16), combins[1].Wol);
            Assert.AreEqual(Convert.ToDecimal(0.945), combins[2].Wp); Assert.AreEqual(Convert.ToDecimal(0.9), combins[2].Col); Assert.AreEqual(Convert.ToDecimal(0.135), combins[2].Wol);
            Assert.AreEqual(Convert.ToDecimal(0.702), combins[3].Wp); Assert.AreEqual(Convert.ToDecimal(6.42), combins[3].Col); Assert.AreEqual(Convert.ToDecimal(4.173), combins[3].Wol);
            Assert.AreEqual(Convert.ToDecimal(1.92), combins[4].Wp); Assert.AreEqual(0, combins[4].Col); Assert.AreEqual(0, combins[4].Wol);
            Assert.AreEqual(Convert.ToDecimal(1.08), combins[5].Wp); Assert.AreEqual(0, combins[5].Col); Assert.AreEqual(0, combins[5].Wol);

            var actions = baseLayer.DssDbContext.Actions.Local.ToList();
            Assert.AreEqual(Convert.ToDecimal(7.58), actions[0].Emv); Assert.AreEqual(Convert.ToDecimal(3.702), actions[1].Emv);
            Assert.AreEqual(Convert.ToDecimal(0.295), actions[0].Eol); Assert.AreEqual(Convert.ToDecimal(4.173), actions[1].Eol);
        }
    }
}
