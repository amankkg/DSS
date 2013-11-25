using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_7
{
    /// <summary>
    /// Interaction logic for PageEvents.xaml
    /// </summary>
    public partial class PageEvents : Page
    {
        BaseLayer _baseLayer;
        NavigationService _navigation;
        EventListViewModel _eventListViewModel;
        Preferences _preferences;
        private Model _model;
        
        public PageEvents(BaseLayer baseLayer, Preferences preferences)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _preferences = preferences;
            _model = new Model(_baseLayer, _preferences);
            BindElements();
            ErrorCount.Reset();
        }

        public PageEvents(BaseLayer baseLayer, Preferences preferences, Model model)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _preferences = preferences;
            _model = model;
            BindElements();
            ErrorCount.Reset();
        }

        void BindElements()
        {
            _eventListViewModel = new EventListViewModel(_baseLayer);
            EventListControl.DataContext = _eventListViewModel;
        }

        void PageEventsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate(new PageOptions(_baseLayer, _preferences));
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _model.GenerateActions();
            _navigation.Navigate(new PageActions(_baseLayer, _preferences, _model));
        }
    }
}
