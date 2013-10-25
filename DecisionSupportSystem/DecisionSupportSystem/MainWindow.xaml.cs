using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Xml;


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
            var navigationwindow = asm.GetType(d.LastChild.InnerText.Trim());
            object obj = Activator.CreateInstance(navigationwindow);
            MethodInfo methodInfo = navigationwindow.GetMethod("Show");
            methodInfo.Invoke(obj, null);
            } 
        }

     
    }
}
