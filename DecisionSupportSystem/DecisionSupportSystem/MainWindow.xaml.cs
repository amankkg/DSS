 using System;
using System.Reflection;
using System.Windows;
 using System.Windows.Navigation;
 using System.Xml;


namespace DecisionSupportSystem
{
    public partial class MainWindow
    {
        private NavigationService navigation;

        public MainWindow()
        {
            InitializeComponent();
        }


        private XmlElement GetTaskType()
        {
            if (gridTasks.SelectedItem != null)
            {
                return (XmlElement)gridTasks.SelectedItem;
            }
            return null;
        }

        private void downloadSolvedBtn_Click(object sender, RoutedEventArgs e)
        {
            var element = GetTaskType();
            if (element != null)
            {
                var taskEx = new TaskExample
                    {
                        Name = element.ChildNodes[0].InnerText, 
                        TaskUniq = element.ChildNodes[3].InnerText, 
                        Window = element.ChildNodes[4].InnerText
                    };

                navigation = NavigationService.GetNavigationService(this);
                navigation.Navigate(new SolvedTasksWindow(taskEx));
            }
        }

        private void SolveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var element = GetTaskType();
            if (element != null)
            {
            var asm = Assembly.GetExecutingAssembly();
                try
                {
                    var navigationwindow = asm.GetType(element.ChildNodes[4].InnerText.Trim());
                    object obj = Activator.CreateInstance(navigationwindow);
                    MethodInfo methodInfo = navigationwindow.GetMethod("Show");
                    methodInfo.Invoke(obj, new[] { obj, element.ChildNodes[0].InnerText.Trim(), element.ChildNodes[3].InnerText.Trim(), null });
                }
                catch (Exception)
                {
                    MessageBox.Show("Указанный модуль " + element.LastChild.InnerText.Trim() + " не найден.");
                }

            } 
        }

     
    }
}
