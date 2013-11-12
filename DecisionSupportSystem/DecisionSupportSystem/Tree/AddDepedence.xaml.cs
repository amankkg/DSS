using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DecisionSupportSystem.Tree
{
    /// <summary>
    /// Логика взаимодействия для AddDepedence.xaml
    /// </summary>
    public partial class AddDepedence : Window
    {

        public IEnumerable<Event> events;
        public List<Action> acts;
        public Tree Main;

        public AddDepedence()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            MainAction.ItemsSource = acts;
        }

        private void MainAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Events.ItemsSource = ((Action) MainAction.SelectedItem).ChildEvents;
            SActions.ItemsSource = acts.Where(action => action != (Action) MainAction.SelectedItem);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Event)Events.SelectedItem).ChildActions.Add(((Action)SActions.SelectedItem));
            ((Action) SActions.SelectedItem).ParentEvent = (Event) Events.SelectedItem;
            Main.Draw(((Event)Events.SelectedItem), ((Action)SActions.SelectedItem));
            this.Close();
        }
    }
}
