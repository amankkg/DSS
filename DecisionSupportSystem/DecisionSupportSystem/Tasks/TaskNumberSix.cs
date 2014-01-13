using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;    //библиотеки выше - для использования базовых классов .NET Framework
using DecisionSupportSystem.PageUserElements;   //для использования созданных в проекте элементов управления
using DecisionSupportSystem.ViewModel;  //для использования ViewModel'ей
using Action = DecisionSupportSystem.DbModel.Action;    //укажем, что по умолчанию класс Action будет использоваться из нашей модели БД
using CoinGameClassesLibrary;   //укажем модуль для игры подбрасывания монеты

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberSix: TaskSpecific
    {
        #region Поля
        public ActionsViewModel ActionsViewModel { get; set; }
        public EventsDepActionsViewModel EventsDepActionsViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }    //объявим ViewModel'ы
        private Coin gameCoin;  //объект, используемый для генерации исходов и подсчета в них последовательностей
        private List<int> numberOfHeadsInOutcomes, numberOfTailsInOutcomes, numberOfDoubleHeadsInOutcomes;  //списки для хранения количеств Г, Р и ГГ
        private char[] InitialEvents;   //массив хранит символы исходных событий
        private EventParamName numberOfHeads, numberOfTails, numberOfDoubleHeads;   //параметры событий задачи
        #endregion

        #region Специфика задачи
        private void GenerateActions()
        {
            ActionsViewModel.Actions.Clear();   //очищаем старый список
            ActionsViewModel.ActionViewModels.Clear();  //очищаем старые ViewModel'ы
            ActionsViewModel.AddAction(new Action
            {// добавление действия со следующими данными:
                Name = "Играть",    //название действия
                SavingId = base.SavingID    //идентификатор для временного хранения
            });
            ActionsViewModel.AddAction(new Action
            {
                Name = "Не играть",
                SavingId = base.SavingID
            });
        }//(пере)создание действий задачи, они не изменяются
        private void GenerateEvents()
        {
            EventsDepActionsViewModel = new EventsDepActionsViewModel(DssDbEntities, EventErrorCatcher);    //пересоздаем ViewModel, тем самым очищая старые события
            double probability = (double) 1 / (double) gameCoin.Outcomes.Count; //вычисляем вероятность для 1-ой группы событий
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {//цикл обходит все исходы с индексами символов для обозначения исходов
                var name = new char[(int)BaseAlgorithms.Task.TaskParams.ToList()[0].Value]; //временный массив для хранения названия текущего исхода
                for (int j = 0; j < gameCoin.Outcomes[i].Length; j++)
                {//цикл формирует название исхода во временном массиве
                    name[j] = InitialEvents[gameCoin.Outcomes[i][j]];
                }
                var ev = new Event
                {//новое событие со следующими данными:
                    Name = new string(name),    //строка имени получемая из временного массива
                    Probability = probability,  //вероятность, заранее расчитанная
                    SavingId = base.SavingID    //идентификатор решения для сохранения
                };
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfHeads, Value = numberOfHeadsInOutcomes[i] });    //значение параметра события Кол-во Г
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfTails, Value = numberOfTailsInOutcomes[i] });    //значение параметра события Кол-во Р
                ev.EventParams.Add(new EventParam() { EventParamName = numberOfDoubleHeads, Value = numberOfDoubleHeadsInOutcomes[i] });    //значение параметра события Кол-во ГГ
                EventsDepActionsViewModel.AddEvent(ActionsViewModel.Actions[0], ev);    //добавление созданного события со всеми его данными в основной список, с привязкой с действию
            }
            var eve = new Event
            {//последнее событие, для действия №2 "Не играть", создается вне цикла, не зависит от условий игры
                Name = "Ничего не выпадает",
                Probability = 1,    //вероятность всегда = 1
                SavingId = base.SavingID
            };
            eve.EventParams.Add(new EventParam() { EventParamName = numberOfHeads, Value = 0 });    //все параметры = 0
            eve.EventParams.Add(new EventParam() { EventParamName = numberOfTails, Value = 0 });
            eve.EventParams.Add(new EventParam() { EventParamName = numberOfDoubleHeads, Value = 0 });
            EventsDepActionsViewModel.AddEvent(ActionsViewModel.Actions[1], eve);   //добавление в основной список
        }//(пере)создание событий задачи
        private void GenerateCombinations()
        {
            CombinationsViewModel.Combinations.Clear(); //очищение старого списка комбинаций
            CombinationsViewModel.CombinationViewModels.Clear();    //очищение ViewModel'ей
            for (int i = 0; i < gameCoin.Outcomes.Count; i++)
            {//обходим исходы
                Combination combo = new Combination
                {//данные новой комбинации
                    Action = ActionsViewModel.Actions[0],   //действие, тут всегда действие №1 "Играть"
                    Event = EventsDepActionsViewModel.EventsDependingActions[0].EventsViewModel.Events[i],  //соотв. исходу событие
                    Task = BaseAlgorithms.Task, //идентификатор решения задачи для БД
                    SavingId = base.SavingID    //идентификатор решения для временного хранения
                };
                combo.Cp = CPFunction(EventsDepActionsViewModel.EventsDependingActions[0].EventsViewModel.Events[i].EventParams.ToList()[0].Value, EventsDepActionsViewModel.EventsDependingActions[0].EventsViewModel.Events[i].EventParams.ToList()[1].Value, EventsDepActionsViewModel.EventsDependingActions[0].EventsViewModel.Events[i].EventParams.ToList()[2].Value);   //вычисляем CP
                CombinationsViewModel.Combinations.Add(combo);  //добавляем в основной список комбинаций
            }//на каждый исход создаем комбинацию с действием №1 "Играть"
            Combination comb = new Combination
            {
                Action = ActionsViewModel.Actions[1],   //действие №2 "Не играть"
                Event = EventsDepActionsViewModel.EventsDependingActions[1].EventsViewModel.Events[0],  //событие "Нет исходов"
                Task = BaseAlgorithms.Task,
                SavingId = base.SavingID
            };//создаем комбинацию на действие №2 "Не играть"
            comb.Cp = 0;
            CombinationsViewModel.Combinations.Add(comb);   //добавляем созданную комбинацию в основной список
            CombinationsViewModel = new CombinationsViewModel(CombinationsViewModel.Combinations, CombinationErrorCatcher); //создаем ViewModel сгенерированных комбинаций
        }//(пере)создание комбинаций задачи
        private double CPFunction(double _numberOfHeads, double _numberOfTails, double _numberOfDoubleHeads)
        {
            return _numberOfHeads * BaseAlgorithms.Task.TaskParams.ToList()[1].Value - _numberOfTails * BaseAlgorithms.Task.TaskParams.ToList()[2].Value + _numberOfDoubleHeads * BaseAlgorithms.Task.TaskParams.ToList()[3].Value;
        }
        #endregion

        public TaskNumberSix()
        {
            numberOfHeads = new EventParamName() { Name = "Кол-во Г" }; //имя параметров событий
            numberOfTails = new EventParamName() { Name = "Кол-во Р" };
            numberOfDoubleHeads = new EventParamName() { Name = "Кол-во ГГ" };
            InitErrorCatchers();    //инициализация обработчика ошибок
        }//конструктор

        #region Переопределения и инициализация ViewModel'ей
        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }//инициализация ViewModel'ей
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во бросков:" }, Value = 3 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Бонус за Г:" }, Value = 1 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Плата за Р:" }, Value = 1.2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Бонус за ГГ:" }, Value = 0.25 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Обозначение исходов игры:\n  (при значении '0':  'Г/Р';\n   а иначе:  'H/T')" }, Value = 0 });
        }//переопределение параметров задачи
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }//подсчет кол-ва действий
        protected override int GetEventsCount()
        {
            return EventsDepActionsViewModel.EventsDependingActions.Count;
        }//подсчет кол-ва событий
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher)
            {
                ParamsVisibility = Visibility.Hidden
            };
        }//иниц-я ViewModel'ей комбинаций
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = base.SavingID };
        }//формат действия
        protected override Event CreateEventTemplate()
        {
            return new Event { Name = "Событие", Probability = 1, SavingId = base.SavingID };
        }//формат события
        protected override Combination CreateCombinationTemplate()
        {
            return new Combination { SavingId = this.SavingID };
        }//формат комбинации
        #endregion

        #region Навигация
        protected override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMain = new PageMainUE { DataContext = this };   //указываем объект начальной страницы
            ContentPage.Content = pageMain;
            Navigation = NavigationService.GetNavigationService(pageMain);
            ShowNavigationWindow(ContentPage);  //переход
        }//открытие главного окна
        public override void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            InitialEvents = (TaskParamsViewModel.Task.TaskParams.ToList()[4].Value == 0 ? new char[] { 'Г', 'Р' } : new char[] { 'H', 'T' });   //в зависимости от параметра задачи, указываем исходные события
            gameCoin = new Coin(InitialEvents.Length, (int)TaskParamsViewModel.Task.TaskParams.ToList()[0].Value);  //создаем объект игры в монету, передавая в его конструктор параметры игры (задачи)
            numberOfHeadsInOutcomes = gameCoin.CountSequences(0);   //считаем количество Г
            numberOfTailsInOutcomes = gameCoin.CountSequences(1);   //считаем количество Р
            numberOfDoubleHeadsInOutcomes = gameCoin.CountSequences(0, 2);  //считаем количество ГГ
            GenerateActions();  //генерируем действия
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this }; //указываем объект следующей страницы
            Navigate(); //переходим
        }//переход далее из главного окна
        public override void PrevBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageMainUE { DataContext = this };    //указываем объект предыдущей страницы
            Navigate(); //переходим
        }//переход назад из окна действий
        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateEvents();   //генерируем события
            ContentPage.Content = new PageEventDepActionsGeneratedUE { DataContext = this };    //указываем объект следующей страницы
            Navigate(); //переходим
        }//переход вперед из окна действий
        public override void PrevBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this }; //указываем объект предыдущей страницы
            Navigate(); //переходим
        }//переход назад из окна событий
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateCombinations(); //генерируем комбинации
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this };   //указываем объект следующей страницы
            Navigate(); //переходим
        }//переход вперед из окна событий
        public override void PrevBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventDepActionsGeneratedUE { DataContext = this };    //указываем объект предыдущей страницы
            Navigate(); //переходим
        }//переход назад из окна комбинаций
        public override void NextBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            BaseAlgorithms.SolveTask(null); //вызов функции решения задачи по EMV и EOL
            ContentPage.Content = new PageSolveUE { DataContext = this };   //указываем объект следующей страницы
            Navigate(); //переходим
        }//переход вперед из окна комбинаций
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageCombinationWithCpUE { DataContext = this };   //указываем объект предыдущей страницы
            Navigate(); //переходим
        }//переход назад из окна рекомендации
        #endregion
    }
}