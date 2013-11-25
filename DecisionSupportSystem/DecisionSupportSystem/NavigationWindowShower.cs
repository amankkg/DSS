using System;
using System.ComponentModel;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.Task_1;
using MessageBox = System.Windows.MessageBox;

namespace DecisionSupportSystem
{
    public static class NavigationWindowShower
    {
        private static BaseLayer _baseLayer;
        private static ITaskLayer _taskLayer;
        public static bool IsSaved = true;
        public static void ShowNavigationWindows(NavigationWindow window, object pageAction, string title, BaseLayer baseLayer, ITaskLayer taskLayer)
        {
            _baseLayer = baseLayer;
            _taskLayer = taskLayer;
            window.Title = title;
            window.Width = 800;
            window.Height = 600;
            window.MinWidth = 450;
            window.MinHeight = 450;
            window.Content = pageAction;
            window.Closing += window_Closing;
            window.Show();
        }

        static void window_Closing(object sender, CancelEventArgs e)
        {       
            var actions = _baseLayer.DssDbContext.Actions.Local;
            if (actions.Count > 0 && !IsSaved)
            {
                var result = MessageBox.Show("Cохранить текущее решение?", "Внимание",
                                             System.Windows.MessageBoxButton.YesNoCancel);
                
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    _baseLayer.DssDbContext.CombinParams.Local.Clear();
                    LocalTaskLayer.CreateFictiveCombinations(_baseLayer, _taskLayer);
                    _baseLayer.Task.Date = DateTime.Now;
                    _baseLayer.Task.Recommendation = "Задача решена не полностью.";
                    _baseLayer.BaseMethods.AddTask(_baseLayer.Task);
                    _baseLayer.Save();
                }
                if (result == System.Windows.MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}
