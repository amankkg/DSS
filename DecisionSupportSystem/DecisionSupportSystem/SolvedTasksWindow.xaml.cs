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
    public partial class SolvedTasksWindow
    {
        public Load Layer { get; set; }
        public TaskExample TaskExample { get; set; }

        public SolvedTasksWindow(TaskExample taskExample)
        {
            InitializeComponent();
            TaskExample = taskExample;
            RefreshTable();
            txtbTaskName.Text = taskExample.Name;
        }

        private void RefreshTable()
        {
            var loadTasksView = new LoadTasksView();
            loadTasksView.LoadTasks(TaskExample.TaskUniq);
            gridTasks.ItemsSource = loadTasksView.Tasks;
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (gridTasks.SelectedItem != null)
            {
                Layer = new Load((Task)gridTasks.SelectedItem);
                Layer.LoadCombinations();
                var asm = Assembly.GetExecutingAssembly();
                var navigationwindow = asm.GetType(TaskExample.Window);
                object obj = Activator.CreateInstance(navigationwindow);
                MethodInfo methodInfo = navigationwindow.GetMethod("Show");
                methodInfo.Invoke(obj, new[] { obj, TaskExample.Name,TaskExample.TaskUniq, Layer.BaseLayer});
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }
    }
}
