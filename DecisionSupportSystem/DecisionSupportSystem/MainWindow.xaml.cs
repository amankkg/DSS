 using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Xml;
using DecisionSupportSystem.Task_1;


namespace DecisionSupportSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public class TaskExample
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void downloadSolvedBtn_Click(object sender, RoutedEventArgs e)
        {
            var solvedWindow = new SolvedTasksWindow();
            solvedWindow.Show();
        }

        private void SolveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (gridTasks.SelectedItem != null)
            {
            var asm = Assembly.GetExecutingAssembly();
            var d = (XmlElement)gridTasks.SelectedItem;
                try
                {
                    var navigationwindow = asm.GetType(d.ChildNodes[4].InnerText.Trim());
                    object obj = Activator.CreateInstance(navigationwindow);
                    MethodInfo methodInfo = navigationwindow.GetMethod("Show");
                    methodInfo.Invoke(obj, new[] { obj, d.ChildNodes[0].InnerText.Trim(), d.ChildNodes[3].InnerText.Trim(), null });
                }
                catch (Exception)
                {
                    MessageBox.Show("Указанный модуль " + d.LastChild.InnerText.Trim() + " не найден.");
                }

            } 
        }

     
    }
}
