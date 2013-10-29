using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;


namespace DecisionSupportSystem.SolvingTree
{ 
    public partial class PageEvents
    {
        private SolvingLayer layer; // ссылка на шаблон, который хранит общие функции и поля,
                                                                // которые могут использоваться любой страницей 
        private NavigationService navigation;  
        private EventOrigin eventOrigin = new EventOrigin();

        #region Конструкторы

        private void Init()
        {
            gridEvent.DataContext = eventOrigin; // указываем датаконтекст гриду, который содержит текстбокс и кнопку
            GrdEventsLst.ItemsSource = layer.EventOrigins;
                // привязываем локальные данные таблицы Actions к датагриду
        }

        public PageEvents(SolvingLayer Layer)
        {
            InitializeComponent();
            layer = Layer;
            Init();
        }

        #endregion

        private void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new Tree(layer));
        }

        private void BtnPrevClick(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageActions(layer));
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            layer.EventOrigins.Add(new EventOrigin {Name = eventOrigin.Name, Probability = eventOrigin.Probability});
            GrdEventsLst.Items.Refresh();
        }
    }

}
