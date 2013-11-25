using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_7
{
    /// <summary>
    /// Interaction logic for PageSolution.xaml
    /// </summary>
    public partial class PageSolution : Page
    {
        BaseLayer _baseLayer;
        Preferences _preferences;
        NavigationService _navigation;
        Model _model;

        public PageSolution(BaseLayer baseLayer, Preferences preferences, Model model)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _preferences = preferences;
            _model = model;
            BindElements();
        }

        private void BindElements()
        {
            GrdSolutionList.ItemsSource = _baseLayer.DssDbContext.Actions.Local;
            GrdTask.DataContext = _baseLayer.SolvedTaskView;
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageCombinations(_baseLayer, _preferences, _model));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _baseLayer.Save();
        }
    }
}
