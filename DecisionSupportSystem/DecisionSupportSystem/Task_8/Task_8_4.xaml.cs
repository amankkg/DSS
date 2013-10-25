using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DecisionSupportSystem.Interfaces.Task_8
{
    /// <summary>
    /// Логика взаимодействия для Page_4.xaml
    /// </summary>
    public partial class Task_8_4 : Page
    {
       /*private InterfaceData _data;*/

        public Task_8_4()
        {
            InitializeComponent();
        }
/*
        public Task_8_4(InterfaceData Idata)
        {
            InitializeComponent();
            _data = Idata;
        }*/

        private void BtnShowSolution_Click(object sender, RoutedEventArgs e)
        {
           /* //_data.SolveAll();
            GrdSolutionLst.ItemsSource = _data.ActionIDatas;
            var k = _data.ActionIDatas.Max(a => a.ExpectedMonetaryValue);
            var optimAct = _data.ActionIDatas.FirstOrDefault(a => a.ExpectedMonetaryValue == k).Name;
            SolveTextBlock.Text =
                string.Format(
                    "Моему клиенту рекомендуется выбрать действие {0}. Такое решение принесет ему максимальное значение средней ожидаемой прибыли равное равное {1} $. Он получит такое значение средней ожидаемой прибыли, если многогратно в пределе после бесчисленного множества раз будет выберать это действие при условии что вероятности событий не изменятся.",
                    optimAct, k);
            MAxEMV.Content = k;*/
        }
        
        

        }
    }

