using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace DecisionSupportSystem.Task_8
{
    public partial class PageActions : Page
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;
        private ActionWithParamListViewModel _actionWithParamListViewModel;

        #region Конструкторы

        private void BindElements()
        {
            _actionWithParamListViewModel = new ActionWithParamListViewModel(_baseLayer);
            ActionListControl.DataContext = _actionWithParamListViewModel;
            ActionControl.DataContext = new ActionWithParamViewModel(_actionWithParamListViewModel);
        }

        public PageActions()
        {
            InitializeComponent();
            _baseLayer = new BaseLayer();
            ErrorCount.Reset();
        }

        public PageActions(BaseLayer baseLayer)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            BindElements();
            ErrorCount.Reset();
        }

        public void Show(object pageAction, string title, string taskuniq, BaseLayer baseLayer)
        {
            if (baseLayer != null) _baseLayer = baseLayer;
            _baseLayer.Task.TaskUniq = taskuniq;
            BindElements();
            NavigationWindowShower.ShowNavigationWindows(new NavigationWindow(), pageAction, title);
        }
        #endregion

        private void Close(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Желаете сохранить изменения?", "!", MessageBoxButton.YesNo) == Dia);
            _baseLayer.Save();
        }
        
        private void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        #region Обработка событий Validation.Error

        public void NextPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_actionWithParamListViewModel.ActionWithParamViewModels.Count > 0)
            {
                _navigation.Navigate(new PageEvents(_baseLayer));
                ErrorCount.EntityErrorCount = 0;
            }
        }

        private void NextPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }
        #endregion
    }
}
