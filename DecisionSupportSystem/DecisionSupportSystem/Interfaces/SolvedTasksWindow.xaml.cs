using System;
using System.Reflection;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem
{
    /// <summary>
    /// Логика взаимодействия для SolvedTasksWindow.xaml
    /// </summary>
    public partial class SolvedTasksWindow : Window
    {
        public LoadLayer Layer { get; set; }

        public SolvedTasksWindow()
        {
            InitializeComponent();
            Layer = new LoadLayer();
            Layer.LoadTasks("task1");
            gridTasks.ItemsSource = Layer.Tasks;
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (gridTasks.SelectedItem != null)
            {
                Layer.BaseLayer.DssDbContext.Combinations.Local.Clear();
                Layer.BaseLayer.DssDbContext.Actions.Local.Clear();
                Layer.BaseLayer.DssDbContext.Events.Local.Clear();
                Layer.LoadTask((Task)gridTasks.SelectedItem);
                var asm = Assembly.GetExecutingAssembly();
                var navigationwindow = asm.GetType("DecisionSupportSystem.Task_1.PageActions");
                object obj = Activator.CreateInstance(navigationwindow);
                MethodInfo methodInfo = navigationwindow.GetMethod("Show");
                methodInfo.Invoke(obj, new[] { obj, "Задача №1","task1", Layer.BaseLayer});
            }
        }
    }
}
