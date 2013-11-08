using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
//using BaseModel;
using DecisionSupportSystem.MainClasses;


namespace DecisionSupportSystem.Task_1
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
            GrdEventsLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Events.Local;
                // привязываем локальные данные таблицы Actions к датагриду
        }

        public PageEvents(BaseLayer taskLayer)
        {
            InitializeComponent();
            pagePattern.baseLayer = taskLayer;
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
            pagePattern.baseLayer.BaseMethods.AddEvent(new Event
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
                navigation.Navigate(new PageCombinations(pagePattern.baseLayer));
        }

        private void PrevPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void PrevPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GrdEventsLst.Items.Count > 0)
                navigation.Navigate(new PageActions(pagePattern.baseLayer));
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.DatagridValidationError(e);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            pagePattern.baseLayer.BaseMethods.DeleteEvent((Event)GrdEventsLst.SelectedItem);
        }
    }

}
