using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;


namespace DecisionSupportSystem.Task_8
{ 
    public partial class PageEvents
    {
        private PagePattern pagePattern = new PagePattern(); // ссылка на шаблон, который хранит общие функции и поля,
                                                                // которые могут использоваться любой страницей 
        private NavigationService navigation;  
        private Event eEvent = new Event();

        #region Конструкторы

        private void Init()
        {
            gridEvent.DataContext = eEvent; // указываем датаконтекст гриду, который содержит текстбокс и кнопку
            GrdEventsLst.ItemsSource = pagePattern.baseTaskLayer.DssDbContext.Events.Local;
                // привязываем локальные данные таблицы Actions к датагриду
        }

        public PageEvents(BaseTaskLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseTaskLayer = taskLayer;
            Init();
        }

        #endregion

        private void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        private void EventAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.EntityAddCanExecute(e);
        }

        private void EventAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            pagePattern.baseTaskLayer.BaseMethods.AddEvent(new Event
                {
                    Name = eEvent.Name,
                    Probability = eEvent.Probability
                });
            GrdEventsLst.Items.Refresh();
        }

        private void EventValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.EntityValidationError(e);
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GrdEventsLst.Items.Count > 0)
                navigation.Navigate(new PageCombinations(pagePattern.baseTaskLayer));
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GrdEventsLst.Items.Count > 0)
                navigation.Navigate(new PageActions(pagePattern.baseTaskLayer));
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.DatagridValidationError(e);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            pagePattern.baseTaskLayer.BaseMethods.DeleteEvent((Event)GrdEventsLst.SelectedItem);
        }
    }

}
