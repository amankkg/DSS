using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;    //выше - необходимые библиотеки .NET Framework
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;  //необходимые компоненты из проекта
using Action = DecisionSupportSystem.DbModel.Action;    //переопределение оператора по умолчанию, т.к. совпадают имена
using DiceGameClassesLibrary;   //подключения модуля для получения исходов и ставок игры

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberSeven: TaskSpecific
    {
        #region ViewModel'ы
        public ActionsViewModel ActionsViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public CombinationsViewModel CombinationsViewModel { get; set; }
        public TaskParamsViewModel TaskParamsViewModel { get; set; }
        #endregion  //необходимые ViewModel'ы

        #region Свойства, специфика задачи
        private Dice gameDice;  //объект класса для генерации данных задачи из начальных данных
        private char[] InitialEvents;   //массив для хранения символов исходных событий
        private char[] evenoddNames;    //один из вариантов именования исходных событий: E/O или Ч/Н
        private char[] numericNames;    //один из вариантов именования исходных событий: 1/2/3/4/5/6
        private EventParamName bonus;   //параметр события - ставка, сущность, не значение
        private CombinParamName SoEG;   //параметр комбинации - совпадение, сущность, не значение
        #endregion

        public TaskNumberSeven()
        {
            bonus = new EventParamName() { Name = "Бонус" };    //свойства параметра
            SoEG = new CombinParamName() { Name = "Совпадение" };   //свойства параметра
            InitErrorCatchers();    //инициализация обработчика ошибок
        }

        #region Методы, специфика задачи
        private void GenerateEvents()
        {
            EventsViewModel.Events.Clear(); //очищаем от старых событий
            EventsViewModel.EventViewModels.Clear();    //очищаем их ViewModel'и
            char[] name;    //хранилище имени
            double probability = (double)1 / (double)gameDice.Outcomes.Count;   //вероятность симметричной игральной кости
            Random random = new Random();   //рандомизатор значений бонуса по умолчанию
            for (int i = 0; i < gameDice.Outcomes.Count; i++)
            {
                name = new char[(int)BaseAlgorithms.Task.TaskParams.ToList()[0].Value]; //длина имени = кол-во бросков
                for (int j = 0; j < gameDice.Outcomes[i].Length; j++)
                {
                    name[j] = InitialEvents[gameDice.Outcomes[i][j]];   //задаем символы по индексам начальных событий из исходов
                }
                Event ev = new Event()
                {//новое событие и его свойства
                    Name = new string(name),
                    Probability = probability,
                    SavingId = base.SavingID    //идентификатор для сохранения
                };
                ev.EventParams.Add(new EventParam() { EventParamName = bonus, Value = (double)random.Next(100) / 10 }); //добавление параметра
                EventsViewModel.AddEvent(ev);   //добавление в основной список
            }
        }   //(ре)генерация событий
        private void GenerateActions()
        {
            ActionsViewModel.Actions.Clear();
            ActionsViewModel.ActionViewModels.Clear();  //очищаем старые списки
            string name;    //строка-имя
            for (int i = 0; i < gameDice.Stakes.Count; i++)
            {
                name = "";  //обнуляем имя
                for (int j = 0; j < gameDice.Stakes[i].Length; j++)
                {
                    name = name + EventsViewModel.Events[gameDice.Stakes[i][j]].Name + " "; //конкатенация имени из имен исходов (событий)
                }
                name = name.Remove(name.Length - 1);    //обрезка лишнего пробела в ходе цикла выше
                ActionsViewModel.AddAction(
                    new Action()
                    {
                        Name = name,
                        SavingId = base.SavingID
                    }); //добавление в основной список нового настроенного действия
            }
        }   //(ре)генерация действий
        private void GenerateCombinations()
        {
            CombinationsViewModel.Combinations.Clear();
            CombinationsViewModel.CombinationViewModels.Clear();
            for (int i = 0; i < gameDice.StakeOutcomeCombinations.Count; i++)
            {
                Combination combo = new Combination()
                {
                    Event = EventsViewModel.Events[gameDice.StakeOutcomeCombinations[i].AcutalOutcome],
                    Action = ActionsViewModel.Actions[gameDice.StakeOutcomeCombinations[i]._ChoosenStake],
                    Task = BaseAlgorithms.Task,
                    SavingId = base.SavingID
                };
                combo.CombinParams.Add(new CombinParam() { CombinParamName = SoEG, Value = Convert.ToDouble(gameDice.StakeOutcomeCombinations[i].SoEG) });//задаем значения параметра SoEG из комбинаций игры
                combo.Cp = CPFunction(combo.Event.EventParams.ToList()[0].Value, combo.CombinParams.ToList()[0].Value);
                CombinationsViewModel.Combinations.Add(combo);
            }
            CombinationsViewModel = new CombinationsViewModel(CombinationsViewModel.Combinations, CombinationErrorCatcher);
        }   //(ре)генерация комбинаций
        private double CPFunction(double bonusvalue, double soegvalue)
        {
            return TaskParamsViewModel.Task.TaskParams.ToList()[2].Value * bonusvalue * soegvalue - TaskParamsViewModel.Task.TaskParams.ToList()[2].Value;
        }   //условная прибыль
        #endregion

        #region Переопределение, специфика задачи
        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher) { ParamsVisibility = Visibility.Hidden };
            EventsViewModel = new EventsViewModel(DssDbEntities.Events.Local, EventErrorCatcher);
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
            TaskParamsViewModel = new TaskParamsViewModel(BaseAlgorithms.Task, TaskParamErrorCatcher);
        }   //инициализация ViewModel'ей
        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher);
        }   //инициализация ViewModel комбинации
        protected override int GetActionsCount()
        {
            return ActionsViewModel.Actions.Count;
        }   //подсчет действий
        protected override int GetEventsCount()
        {
            return EventsViewModel.Events.Count;
        }   //подсчет событий
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = base.SavingID };
        }   //шабон действия
        protected override Event CreateEventTemplate()
        {
            return new Event { Name = "Событие", Probability = 1, SavingId = base.SavingID };
        }   //шаблон события
        protected override Combination CreateCombinationTemplate()
        {
            return new Combination { SavingId = this.SavingID };
        }   //шаблон комбинации
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во бросков:" }, Value = 2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Кол-во исходов на ставку:" }, Value = 2 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Сумма ставки:" }, Value = 1 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Режим игры:\n  (при значении '0':  'Чет/Нечет';\n   а иначе:  '1/2/3/4/5/6')" }, Value = 0 });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Обозначение исходов для игры\nв режиме 'Чет/Нечет':\n  (при значении '0':  'Ч/Н';\n   а иначе:  'E/O')" }, Value = 0 });
        }   //шаблон параметров задачи
        #endregion

        #region Навигация
        protected override void ShowPageMain()
        {
            ContentPage = new Page();
            var pageMain = new PageMainUE { DataContext = this };
            ContentPage.Content = pageMain;
            Navigation = NavigationService.GetNavigationService(pageMain);
            ShowNavigationWindow(ContentPage);
        }   //открытие первого окна, параметров задачи
        public override void NextBtnClick_OnPageMain(object sender, RoutedEventArgs e)
        {
            evenoddNames = (TaskParamsViewModel.Task.TaskParams.ToList()[4].Value == 0 ? new char[] { 'Ч', 'Н' } : new char[] { 'E', 'O' });    //выбор символов для исходных событий
            numericNames = new char[] { '1', '2', '3', '4', '5', '6' }; //выбор символов для исходных событий
            InitialEvents = (TaskParamsViewModel.Task.TaskParams.ToList()[3].Value == 0 ? InitialEvents = evenoddNames : numericNames); //присвоение выбранных символов
            gameDice = new Dice(InitialEvents.Length, (int)TaskParamsViewModel.Task.TaskParams.ToList()[0].Value, (int)TaskParamsViewModel.Task.TaskParams.ToList()[1].Value);  //генерация результатов игры из исходных данных
            GenerateEvents();
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }   //"далее" с окна параметров
        public override void PrevBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageMainUE { DataContext = this };
            Navigate();
        }   //"назад" с окна событий
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateActions();
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }   //"далее" с окна событий
        public override void PrevBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageEventGeneratedUE { DataContext = this };
            Navigate();
        }   //"назад" с окна действий
        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            GenerateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }   //"далее" с окна действий
        public override void PrevBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionGeneratedUE { DataContext = this };
            Navigate();
        }   //"назад" с окна комбинаций
        public override void NextBtnClick_OnPageCombinations(object sender, System.Windows.RoutedEventArgs e)
        {
            BaseAlgorithms.SolveTask(null);
            ContentPage.Content = new PageSolveUE { DataContext = this };
            Navigate();
        }   //"далее" с окна комбинаций
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }    //"назад" с окна рекомендации
        #endregion
    }
}
