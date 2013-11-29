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

namespace DecisionSupportSystem.Task_6
{
    /// <summary>
    /// Interaction logic for PageOptions.xaml
    /// </summary>
    public partial class PageOptions : Page
    {
        BaseLayer baseLayer;
        Preferences preferences;
        NavigationService navigation;

        public PageOptions()
        {
            InitializeComponent();
            baseLayer = new BaseLayer();
            preferences = new Preferences();
            preferences.NumberOfThrowings = 3;
            preferences.HeadBonus = 1m;
            preferences.TailCost = 1.2m;
            preferences.DoubleHeadBonus = 0.25m;
        }

        public PageOptions(BaseLayer _baseLayer, Preferences _preferences)
        {
            InitializeComponent();
            baseLayer = _baseLayer;
            preferences = _preferences;
        }

        private void PageOptionsOnLoaded(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            ComboBox_NumberOfThrowings.SelectedIndex = preferences.NumberOfThrowings - 1;
            TextBox_HeadBonus.Text = preferences.HeadBonus.ToString();
            TextBox_TailCost.Text = preferences.TailCost.ToString();
            TextBox_DoubleHeadBonus.Text = preferences.DoubleHeadBonus.ToString();
        }

        public void Show(object pageOptions, string title, string taskuniq, BaseLayer _baseLayer)
        {
            if (_baseLayer != null)
            {
                baseLayer = _baseLayer;
            }
            baseLayer.Task.TaskUniq = taskuniq;
            NavigationWindowShower.ShowNavigationWindows(new NavigationWindow(), pageOptions, title, _baseLayer, null);
        }

        private void ComboBox_NumberOfThrowings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            preferences.NumberOfThrowings = ComboBox_NumberOfThrowings.SelectedIndex + 1;
        }

        private void TextBox_HeadBonus_TextChanged(object sender, TextChangedEventArgs e)
        {
            preferences.HeadBonus = Convert.ToDecimal(TextBox_HeadBonus.Text);
        }

        private void TextBox_TailCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            preferences.TailCost = Convert.ToDecimal(TextBox_TailCost.Text);
        }

        private void TextBox_DoubleHeadBonus_TextChanged(object sender, TextChangedEventArgs e)
        {
            preferences.DoubleHeadBonus = Convert.ToDecimal(TextBox_DoubleHeadBonus.Text);
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageActions(baseLayer, preferences));
        }
    }
}
