using System.Linq;
using System.Collections.Generic;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesicionSupportSystemTest
{
    [TestClass]
    public class BaseAlgorithmsTest
    {
        [TestMethod]
        public void CalculateWpWithUnNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            var combinations = InitSourceDataForCalculateWpTest(bAlgorithms);

            bAlgorithms.CalculateWp(combinations);
            var actual = bAlgorithms.Entities.Combinations.Local.ToList()[0].Wp;
            Assert.AreEqual(50, actual);
        }

        [TestMethod]
        public void CalculateWpWithNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitSourceDataForCalculateWpTest(bAlgorithms);

            bAlgorithms.CalculateWp(null);
            var actual = bAlgorithms.Entities.Combinations.Local.ToList()[0].Wp;
            Assert.AreEqual(50, actual);
        }

        private IEnumerable<Combination> InitSourceDataForCalculateWpTest(BaseAlgorithms baseAlgorithms)
        {
            var combination = new Combination { Cp = 100, Event = new Event { Probability = 0.5 } };
            baseAlgorithms.Entities.Combinations.Local.Add(combination);
            return baseAlgorithms.Entities.Combinations.Local;
        }

        [TestMethod]
        public void FindCpMaxesTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitSourceDataForCalculateColTest(bAlgorithms);

            bAlgorithms.FindCpMaxes();
            Assert.AreEqual(120, bAlgorithms.CpMaxes[0].Value);
            Assert.AreEqual(450, bAlgorithms.CpMaxes[1].Value);
        }

        [TestMethod]
        public void CalculateColWithUnNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            var combinations = InitSourceDataForCalculateColTest(bAlgorithms);

            bAlgorithms.CalculateCol(combinations);
            var combinationsList = combinations.ToList();
            Assert.AreEqual(20, combinationsList[0].Col);
            Assert.AreEqual(0, combinationsList[1].Col);
            Assert.AreEqual(250, combinationsList[2].Col);
            Assert.AreEqual(0, combinationsList[3].Col);
        }

        [TestMethod]
        public void CalculateColWithNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitSourceDataForCalculateColTest(bAlgorithms);

            bAlgorithms.CalculateCol(null);
            var combinationsList = bAlgorithms.Entities.Combinations.Local.ToList();
            Assert.AreEqual(20, combinationsList[0].Col);
            Assert.AreEqual(0, combinationsList[1].Col);
            Assert.AreEqual(250, combinationsList[2].Col);
            Assert.AreEqual(0, combinationsList[3].Col);
        }

        private IEnumerable<Combination> InitSourceDataForCalculateColTest(BaseAlgorithms baseAlgorithms)
        {
            var combinations = baseAlgorithms.GetCombinations();
            var event1 = new Event();
            var event2 = new Event();
            combinations.Add(new Combination { Cp = 100, Event = event1 });
            combinations.Add(new Combination { Cp = 120, Event = event1 });
            combinations.Add(new Combination { Cp = 200, Event = event2 });
            combinations.Add(new Combination { Cp = 450, Event = event2 });
            return combinations;
        }

        [TestMethod]
        public void CalculateWolWithUnNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            var combinations = InitCSourceDataForCalculateWolTest(bAlgorithms);

            bAlgorithms.CalculateWol(combinations);
            var combinationsList = combinations.ToList();
            Assert.AreEqual(50, combinationsList[0].Wol);
        }
        
        [TestMethod]
        public void CalculateWolWithNullableArgTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitCSourceDataForCalculateWolTest(bAlgorithms);

            bAlgorithms.CalculateWol(null);
            var combinationsList = bAlgorithms.Entities.Combinations.Local.ToList();
            Assert.AreEqual(50, combinationsList[0].Wol);
        }

        private IEnumerable<Combination> InitCSourceDataForCalculateWolTest(BaseAlgorithms baseAlgorithms)
        {
            var combination = new Combination { Col = 100, Event = new Event { Probability = 0.5 } };
            baseAlgorithms.Entities.Combinations.Local.Add(combination);
            return baseAlgorithms.Entities.Combinations.Local;
        }

        [TestMethod]
        public void CalculateEmvTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitCSourceDataForCalculateEmvEolTest(bAlgorithms);
            var actions = bAlgorithms.GetActions().ToList();

            bAlgorithms.CalculateEmv();
            Assert.AreEqual(130, actions[0].Emv);
            Assert.AreEqual(30, actions[1].Emv);
        }

        [TestMethod]
        public void CalculateEolTest()
        {
            var bAlgorithms = new BaseAlgorithms { Entities = new DssDbEntities() };
            InitCSourceDataForCalculateEmvEolTest(bAlgorithms);
            var actions = bAlgorithms.GetActions().ToList();

            bAlgorithms.CalculateEol(null);
            Assert.AreEqual(70, actions[0].Eol);
            Assert.AreEqual(810, actions[1].Eol);
        }

        private void InitCSourceDataForCalculateEmvEolTest(BaseAlgorithms baseAlgorithms)
        {
            var action1 = new Action();
            var action2 = new Action();
            var actions = baseAlgorithms.GetActions();
            actions.Add(action1);
            actions.Add(action2);
            var combinations = baseAlgorithms.GetCombinations();
            combinations.Add(new Combination { Wp = 100, Wol = 40, Action = action1 });
            combinations.Add(new Combination { Wp = 30, Wol = 30, Action = action1 });
            combinations.Add(new Combination { Wp = -10, Wol = 10, Action = action2 });
            combinations.Add(new Combination { Wp = 40, Wol = 800, Action = action2 });
        }
    }
}
