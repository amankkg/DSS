using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DecisionSupportSystem.Task_1;


namespace DesicionSupportSystemTest
{
    public class CombinationTest
    {
        public string ActionName { get; set; }
        public string EventName { get; set; }
    }

    [TestClass]
    public class Task1LocalTaskLayerTest
    {
        [TestMethod]
        public void Task1CreateCombinationsMethodTest()
        {
            // входным параметром для данного метода выступает 
            // объект (экземпляр) класса BaseLayer 
            // см. класс DecisionSupportSystem.Task_1.LocalTaskLayer метод CreateCombinations(BaseLayer baseLayer)
            // далее выполняется заполнение этого объекта (baseLayer) необходимыми для теста данными
            var baseLayer = new BaseLayer();
            baseLayer.Task = new Task();
            baseLayer.DssDbContext.Actions.Local.Add(new Action { Name = "A1" });
            baseLayer.DssDbContext.Actions.Local.Add(new Action { Name = "A2" });
            baseLayer.DssDbContext.Events.Local.Add(new Event { Name = "E1" });
            baseLayer.DssDbContext.Events.Local.Add(new Event { Name = "E2" });
            // заполнение здесь заканчивается страшного ничего нет :)))
            // baseLayer имеет ссылку (DssDbContext) на нашу модель БД (DbModel)
            // используя эту ссылку мы добавили по две записи в таблицы Actions и Events
            
            // далее создаем список комбинаций действие событие, которые мы ожидаем получить
            var expected = new List<CombinationTest>();
            expected.Add(new CombinationTest { ActionName = "A1", EventName = "E1" });
            expected.Add(new CombinationTest { ActionName = "A1", EventName = "E2" });
            expected.Add(new CombinationTest { ActionName = "A2", EventName = "E1" });
            expected.Add(new CombinationTest { ActionName = "A2", EventName = "E2" });
            
            // ключевой момент вызов нашей тестируемой функции 
            LocalTaskLayer.CreateCombinations(baseLayer);
            
            // все наша функция отработала
            // получаем комбинации используя ту же ссылку на нашу модель БД
            // список actual будет содержать список сгенерированных комбинации действие событие
            var actual = baseLayer.DssDbContext.Combinations.Local.ToList();

            // ПРОВЕРКА количества созданных комбинаций 
            Assert.AreEqual(4, actual.Count);

            // ПРОВЕРКА правильности созданных комбинаций 
            if(actual.Count == 4)
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expected[i].ActionName, actual[i].Action.Name);
                Assert.AreEqual(expected[i].EventName, actual[i].Event.Name);
            }
        }

        [TestMethod]
        public void Task1LoadCombinationsMethodTest()
        {
            var baseLayer = new BaseLayer();
            // добавляем одну запись в таблицу Combinations
            baseLayer.DssDbContext.Combinations.Local.Add(new Combination());

            // вызываем тестируюмую функцию
            LocalTaskLayer.LoadCombinations(baseLayer);

            // проверяем увеличилось ли количество записей в списке CombinationsList класса LocalTaskLayer
            Assert.AreEqual(1, LocalTaskLayer.CombinationsList.Count);
        }

        [TestMethod]
        public void Task1HaveActionMethodTest()
        {
            var baseLayer = new BaseLayer();
            // добавляем две комбинации в таблицу Combinations
            // событие null потому, что проверяется наличие только действия
            baseLayer.DssDbContext.Combinations.Local.Add(new Combination
                {
                    Action = new Action(),
                    Event = null
                });
            baseLayer.DssDbContext.Combinations.Local.Add(new Combination
                {
                    Action = new Action(),
                    Event = null
                });

            // в случае если действие act уже присутствовало при создании комбинаций действие-событие т.е. данное действие имеется в таблице Combinations
            var act = baseLayer.DssDbContext.Combinations.Local[0].Action;
            // вызываем тестируемый метод
            bool actual = LocalTaskLayer.HaveAction(act, baseLayer.DssDbContext.Combinations.Local);
            // если в таблице Combinations имеется запись содержащий действие act метод возвратит true 
            Assert.AreEqual(true, actual, "Ошибка, так как данное действие присутствует в таблице Combinations");


            // случае если было добавлено новое действие, проверяем имеется ли это действие в таблице Combinations
            act = new Action();
            actual = LocalTaskLayer.HaveAction(act, baseLayer.DssDbContext.Combinations.Local);
            // если в таблице Combinations не имеется запись метод возвратит false
            Assert.AreEqual(false, actual, "Ошибка, так как данного действия нет в таблице Combinations");
        }
    }
}
