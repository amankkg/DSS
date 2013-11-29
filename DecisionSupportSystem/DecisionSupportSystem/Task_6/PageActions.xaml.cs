using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_6
{
    /// <summary>
    /// Логика взаимодействия для PageActions.xaml
    /// </summary>
    public partial class PageActions : Page
    {
        BaseLayer baseLayer;
        NavigationService navigation;
        ActionListViewModel actionListViewModel;
        Preferences preferences;
        Model model;

        public PageActions(BaseLayer _baseLayer, Preferences _preferences)
        {
            InitializeComponent();
            baseLayer = _baseLayer;
            preferences = _preferences;
            model = new Model(baseLayer, preferences);
            model.GenerateActions();
            BindElements();
        }

        public PageActions(BaseLayer _baseLayer, Preferences _preferences, Model _model)
        {
            InitializeComponent();
            baseLayer = _baseLayer;
            preferences = _preferences;
            model = _model;
            BindElements();
        }

        void BindElements()
        {
            actionListViewModel = new ActionListViewModel(baseLayer);
            ActionListControl.DataContext = actionListViewModel;
        }

        void PageActionsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        void PageActionsOnLoaded()
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageOptions(baseLayer, preferences));
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            model.GenerateEvents();
            navigation.Navigate(new PageEvents(baseLayer, preferences, model));
        }


    }
}
