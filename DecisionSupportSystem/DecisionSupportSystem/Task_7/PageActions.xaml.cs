using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_7
{
    /// <summary>
    /// Interaction logic for PageActions.xaml
    /// </summary>
    public partial class PageActions : Page
    {
        BaseLayer _baseLayer;
        NavigationService _navigation;
        ActionListViewModel _actionListViewModel;
        Preferences _preferences;
        Model _model;

        public PageActions(BaseLayer baseLayer, Preferences preferences, Model model)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _preferences = preferences;
            _model = model;
            BindElements();
            ErrorCount.Reset();
        }
        
        public void Show(object pageAction, string title, string taskuniq, BaseLayer baseLayer)
        {
            if (baseLayer == null)
            {
                _baseLayer = baseLayer;
            }
            _baseLayer.Task.TaskUniq = taskuniq;
            BindElements();
            NavigationWindowShower.ShowNavigationWindows(new NavigationWindow(), pageAction, title, _baseLayer, null);
        }
        
        void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate(new PageEvents(_baseLayer, _preferences, _model));
        }
    
        void BindElements()
        {
            _actionListViewModel = new ActionListViewModel(_baseLayer);
            ActionListControl.DataContext = _actionListViewModel;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _model.GenerateCombinations();
            _navigation.Navigate(new PageCombinations(_baseLayer, _preferences, _model));
        }

    }
}
