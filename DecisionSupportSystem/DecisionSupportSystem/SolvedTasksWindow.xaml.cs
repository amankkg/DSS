using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Reflection;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem
{
    public class LoadedTask
    {
        public List<Task> Tasks { get; set; }

        public void LoadTasks(string taskUniq)
        {
            Tasks = new List<Task>();
            using (var dssDbContext = new DssDbEntities())
            {
                var tasks = (from task in dssDbContext.Tasks
                             where task.TaskUniq == taskUniq && task.Deleted != 1
                             select task).ToList();
                foreach (var t in tasks)
                {
                    Tasks.Add(new Task { Comment = t.Comment, TaskUniq = t.TaskUniq, Id = t.Id, Recommendation = t.Recommendation, 
                                         Date = t.Date, Deleted = t.Deleted, TreeDiagramm = t.TreeDiagramm});
                }
            }
        }
    }

    public partial class SolvedTasksWindow
    {
        public Load Layer { get; set; }
        public TaskViewForMainWindows TaskViewForSolvedTaskWindow { get; set; }

        public SolvedTasksWindow(TaskViewForMainWindows taskViewForSolvedTaskWindow)
        {
            InitializeComponent();
            TaskViewForSolvedTaskWindow = taskViewForSolvedTaskWindow;
            txtbTaskName.Text = taskViewForSolvedTaskWindow.Name;
            RefreshTable();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (gridTasks.SelectedItem != null)
            {
                Layer = new Load((Task)gridTasks.SelectedItem);
                Layer.LoadCombinations();
                var asm = Assembly.GetExecutingAssembly();
                var navigationwindow = asm.GetType(TaskViewForSolvedTaskWindow.Window);
                object obj = Activator.CreateInstance(navigationwindow);
                MethodInfo methodInfo = navigationwindow.GetMethod("Show");
                methodInfo.Invoke(obj, new[] { obj, TaskViewForSolvedTaskWindow.Name, TaskViewForSolvedTaskWindow.TaskUniq, Layer.BaseLayer });
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }  
        
        private void RefreshTable()
        {
            var loadedTask = new LoadedTask();
            loadedTask.LoadTasks(TaskViewForSolvedTaskWindow.TaskUniq);
            gridTasks.ItemsSource = loadedTask.Tasks;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridTasks.SelectedItem != null)
            {
                using (DssDbEntities context = new DssDbEntities())
                {
                    var tasks = context.Tasks.Select(t => t);
                    tasks.Where(t => t.Id == ((Task) gridTasks.SelectedItem).Id).Select(t => t).First().Deleted = 1;
                    context.SaveChanges();
                    RefreshTable();
                }
            }

        }
    }
}
