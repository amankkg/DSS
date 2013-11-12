using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Tree
{
    public partial class PageActions
    {
        private SolvingLayer layer;  // ссылка на шаблон, который хранит общие функции и поля,
        private NavigationService navigation;                 // которые могут использоваться любой страницей 
        private Action action = new Action();
        #region Конструкторы

        private void Init()
        {
            gridAct.DataContext = action;              // указываем датаконтекст гриду, который содержит текстбокс и кнопку
            GrdActionsLst.ItemsSource = layer.Actions; // привязываем локальные данные таблицы Actions к датагриду
        }

        public PageActions()
        {
            InitializeComponent();
            layer = new SolvingLayer();
        }

        public PageActions(SolvingLayer Layer)
        { 
            InitializeComponent();
            layer = Layer;
        }
        #endregion
         
        private void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            Init();
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            if (GrdActionsLst.Items.Count > 0)
                navigation.Navigate(new PageEvents(layer));
        }

        private void AddActionClick(object sender, RoutedEventArgs e)
        {
            layer.Actions.Add(new Action{Name = action.Name, Credit = action.Credit});
            GrdActionsLst.Items.Refresh();
        }
    }
}
