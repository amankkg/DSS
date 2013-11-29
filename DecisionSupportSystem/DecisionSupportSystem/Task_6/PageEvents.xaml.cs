using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;
namespace DecisionSupportSystem.Task_6
{
    /// <summary>
    /// Interaction logic for PageEvents.xaml
    /// </summary>
    public partial class PageEvents : Page
    {
        BaseLayer baseLayer;
        Model model;
        Preferences preferences;
        NavigationService navigation;
        EventListViewModel eventListViewModel;

        public PageEvents(BaseLayer _baseLayer, Preferences _preferences, Model _model)
        {
            InitializeComponent();
            baseLayer = _baseLayer;
            model = _model;
            preferences = _preferences;
            BindElements();
        }

        void BindElements()
        {
            eventListViewModel = new EventListViewModel(baseLayer);
            EventListControl.DataContext = eventListViewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageActions(baseLayer, preferences, model));
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            model.GenerateCombinations();
            navigation.Navigate(new PageCombinations(baseLayer, preferences, model));
        }

    }
}
