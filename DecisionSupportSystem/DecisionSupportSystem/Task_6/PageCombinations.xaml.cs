using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_6
{
    /// <summary>
    /// Interaction logic for PageCombinations.xaml
    /// </summary>
    public partial class PageCombinations : Page
    {
        BaseLayer baseLayer;
        Preferences preferences;
        Model model;
        NavigationService navigation;
        CombinationListViewModel combinationListViewModel;

        public PageCombinations(BaseLayer _baseLayer, Preferences _preferences, Model _model)
        {
            InitializeComponent();
            baseLayer = _baseLayer;
            preferences = _preferences;
            model = _model;
            combinationListViewModel = new CombinationListViewModel(baseLayer);
            CombinationListControl.DataContext = combinationListViewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //navigation.Navigate(new PageSolution(baseLayer, model, preferences));
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageEvents(baseLayer, preferences, model));
        }
    }
}
