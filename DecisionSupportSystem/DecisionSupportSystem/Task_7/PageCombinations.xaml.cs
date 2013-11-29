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

namespace DecisionSupportSystem.Task_7
{
    /// <summary>
    /// Interaction logic for PageCombinations.xaml
    /// </summary>
    public partial class PageCombinations : Page
    {
        BaseLayer _baseLayer;
        Preferences _preferences;
        NavigationService _navigation;
        CombinationListViewModel _combinationListViewModel;
        Model _model;

        public PageCombinations(BaseLayer baseLayer, Preferences preferences, Model model)
        {
            InitializeComponent();
            _baseLayer = baseLayer;
            _preferences = preferences;
            _model = model;
            _combinationListViewModel = new CombinationListViewModel(_baseLayer);
            CombinationListControl.DataContext = _combinationListViewModel;
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageActions(_baseLayer, _preferences, _model));
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            _navigation.Navigate(new PageSolution(_baseLayer, _preferences, _model));
        }
    }
}
