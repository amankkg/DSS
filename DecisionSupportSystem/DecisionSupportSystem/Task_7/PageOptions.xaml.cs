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
using DiceGameClassesLibrary;

namespace DecisionSupportSystem.Task_7
{
    /// <summary>
    /// Interaction logic for PageOptions.xaml
    /// </summary>
    public partial class PageOptions : Page
    {
        private BaseLayer _baseLayer;
        private NavigationService _navigation;

        private readonly Preferences _preferences;

        #region Constructors
        public PageOptions()
        {
            InitializeComponent();
            _baseLayer = new BaseLayer();

            #region Filling defaults
            _preferences = new Preferences
                {
                    evenoddNames = new char[] { 'E', 'O' },
                    numericNames = new char[] { '1', '2', '3', '4', '5', '6' },
                    evenoddGame = true,
                    numberofthrowings = 2,
                    numberofoutcomesperstake = 2,
                    amountofstakevalue = 1
                };

            #endregion
        }

        public PageOptions(BaseLayer baseLayer, Preferences preferences)
        {
            InitializeComponent();
            _baseLayer = new BaseLayer();
            _preferences = preferences;
        }

        public void Show(object pageOptions, string title, string taskuniq, BaseLayer baseLayer)
        {
            if (baseLayer != null)
            {
                _baseLayer = baseLayer;
            }
            _baseLayer.Task.TaskUniq = taskuniq;
            NavigationWindowShower.ShowNavigationWindows(new NavigationWindow(), pageOptions, title, _baseLayer, null);
        }

        private void PageOptionsOnLoaded(object sender, RoutedEventArgs e)
        {
            _navigation = NavigationService.GetNavigationService(this);
            FillControls();
        }

        private void FillControls()
        {
            var nOfThrowings = new int[7];
            for (var i = 2; i < 9; i++)
            {
                nOfThrowings[i-2] = i;
            }
            ComboBox_NumberOfThrowings.ItemsSource = nOfThrowings;
            ComboBox_NumberOfThrowings.SelectedIndex = 0;
            TextBox_AmountOfStakeValue.Text = _preferences.amountofstakevalue.ToString();
            ToggleSwitch_EvenOddGame.IsChecked = _preferences.evenoddGame;
        } 
        #endregion

        private void ComboBox_NumberOfThrowings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var nOfOutcomesPerStake = new int[Convert.ToInt32(ComboBox_NumberOfThrowings.SelectedItem)];
            for (int i = 2; i < nOfOutcomesPerStake.Length; i++)
            {
                nOfOutcomesPerStake[i] = i;
            }
            ComboBox_NumberOfOutcomesPerStake.ItemsSource = nOfOutcomesPerStake;
            ComboBox_NumberOfOutcomesPerStake.SelectedIndex = 0;

            if (this._preferences != null)
            {
                _preferences.numberofthrowings = Convert.ToInt32(ComboBox_NumberOfThrowings.SelectedItem); 
            }
        }

        private void ComboBox_NumberOfOutcomesPerStake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._preferences != null)
            {
                _preferences.numberofoutcomesperstake = Convert.ToInt32(ComboBox_NumberOfOutcomesPerStake.SelectedIndex); 
            }
        }

        private void TextBox_AmountOfStakeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._preferences != null)
            {
                _preferences.amountofstakevalue = Convert.ToInt32(TextBox_AmountOfStakeValue.Text);
            }
        }

        private void ToggleSwitch_EvenOddGame_Checked(object sender, RoutedEventArgs e)
        {
            if (this._preferences != null)
            {
                _preferences.evenoddGame = true;
            }
            Datagrid_InitialEvents.ItemsSource = _preferences.evenoddNames;
        }

        private void ToggleSwitch_EvenOddGame_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this._preferences != null)
            {
                _preferences.evenoddGame = false;
            }
            Datagrid_InitialEvents.ItemsSource = _preferences.numericNames;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            _navigation.Navigate(new PageEvents(_baseLayer, _preferences));
        }
    }
}
