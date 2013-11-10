using System.Windows.Navigation;

namespace DecisionSupportSystem
{
    public static class NavigationWindowShower
    {
        public static void ShowNavigationWindows(NavigationWindow window, object pageAction, string title)
        {
            window.Title = title;
            window.Width = 800;
            window.Height = 600;
            window.MinWidth = 450;
            window.MinHeight = 450;
            window.Content = pageAction;
            window.Show();
        }
    }
}
