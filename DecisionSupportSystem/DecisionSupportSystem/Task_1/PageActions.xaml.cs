using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Task_1
{
    public partial class PageActions : Page
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private readonly Action _action = new Action();

        #region Конструкторы

        public PageActions()
        {
            InitializeComponent();
            _baseLayer = new BaseLayer();
        }

        public PageActions(BaseLayer baseLayer)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            BindElements();
        }

        public void Show(object pageAction, string title, string taskuniq, BaseLayer baseLayer)
        {
            if (baseLayer != null) _baseLayer = baseLayer;
            _baseLayer.Task.TaskUniq = taskuniq;
            BindElements();
            NavigationWindowShower.ShowNavigationWindows(new NavigationWindow(), pageAction, title);
        }

        private void BindElements()
        {
            gridAct.DataContext = _action;
            GrdActionsLst.ItemsSource = _baseLayer.DssDbContext.Actions.Local;
        }

        #endregion

        private void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _baseLayer.BaseMethods.DeleteAction((Action) GrdActionsLst.SelectedItem);
            GrdActionsLst.Items.Refresh();
        }

        #region Обработка событий Validation.Error

        public void ActionAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _baseLayer.BaseMethods.AddAction(new Action {Name = _action.Name});
            GrdActionsLst.Items.Refresh();
        }

        public void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GrdActionsLst.Items.Count > 0)
            {
                _navigation.Navigate(new PageEvents(_baseLayer));
                ErrorCount.EntityErrorCount = 0;
            }
        }

        private void ActionAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityErrorCount == 0;
            e.Handled = true;
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }

        private void ActionValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }

        private void DataGridValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        #endregion

    }
}
