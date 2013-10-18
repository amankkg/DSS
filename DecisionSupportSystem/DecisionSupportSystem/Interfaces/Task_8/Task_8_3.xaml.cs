using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DecisionSupportSystem.Interfaces.Task_8
{
    public partial class Task_8_3 : Page
    {
        public Task_8_3()
        {
            InitializeComponent();
        }

        private InterfaceData _data;

        public Task_8_3(InterfaceData Idata)
        {
            InitializeComponent();
            _data = Idata;
        }
        
        private void BtnShowCombination_Click(object sender, RoutedEventArgs e)
        {
            var tempData = _data;
            tempData.Constants.Add(new Constant
                {
                    Name = "Премия", Value = Convert.ToDecimal(TxtBonus.Text)
                });
            tempData.Constants.Add(new Constant
                {
                    Name = "Штраф", Value = Convert.ToDecimal(TxtFine.Text)
                });
            tempData.SolveAll();
            var combinListForBind = tempData.Combinations.Select(
                c => new FormatDataForDatagrid(c)).ToList();

            GrdCombinsLst.ItemsSource = combinListForBind;
        }



        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            var nextPage = new Task_8_4(_data);
            nav.Navigate(nextPage); 
        }
    }
}
