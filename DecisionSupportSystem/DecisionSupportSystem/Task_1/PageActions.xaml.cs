using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_1
{
    public partial class PageActions
    {
        private PagePattern pagePattern = new PagePattern();  // ссылка на шаблон, который хранит общие функции и поля,
        private NavigationService navigation;                 // которые могут использоваться любой страницей 
        private Action action = new Action();
        #region Конструкторы
       

        private void Init()
        {
            gridAct.DataContext = action;                                                      // указываем датаконтекст гриду, который содержит текстбокс и кнопку
            GrdActionsLst.ItemsSource = pagePattern.baseLayer.DssDbContext.Actions.Local;  // привязываем локальные данные таблицы Actions к датагриду
        }

        public PageActions()
        {
            InitializeComponent();
            pagePattern.baseLayer = new BaseLayer();  // так как это первая страница создаем новый объект BaseTaskLayer
        }

        public PageActions(BaseLayer taskLayer)
        { 
            InitializeComponent();
            pagePattern.baseLayer = taskLayer;
            Init();
        }
        #endregion

        public void Show(object obj, string title, string taskuniq, BaseLayer baseLayer)
        {
            if (baseLayer != null)
            pagePattern.baseLayer = baseLayer;
            pagePattern.baseLayer.Task.TaskUniq = taskuniq;
            Init();
            var wind = new NavigationWindow();
            wind.Content = obj; wind.Title = title; wind.Width = 800; wind.Height = 600; // определяем свойства окна
            wind.Show();
        }

        private void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        public void ActionAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            pagePattern.baseLayer.BaseMethods.AddAction(new Action {Name = action.Name});
            GrdActionsLst.Items.Refresh();
        }

        public void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GrdActionsLst.Items.Count > 0)
                navigation.Navigate(new PageEvents(pagePattern.baseLayer));
        }

        private void ActionAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.EntityAddCanExecute(e);
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            pagePattern.NavigatePageCanExecute(e);
        }

        private void ActionValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.EntityValidationError(e);
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            pagePattern.DatagridValidationError(e);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            pagePattern.baseLayer.BaseMethods.DeleteAction((Action) GrdActionsLst.SelectedItem);
            GrdActionsLst.Items.Refresh();
        }
    }
}
