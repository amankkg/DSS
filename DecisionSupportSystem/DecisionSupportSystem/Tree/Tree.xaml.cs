using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace DecisionSupportSystem.Tree
{
    /// <summary>
    /// Логика взаимодействия для Tree.xaml
    /// </summary>
    public partial class Tree : Page
    {
        private SolvingLayer layer;
        private NavigationService navigation;  
        private Point point;
        private bool _canmove;
        private Control _control;
        public List<EventControl> eventControls = new List<EventControl>();
        public List<ActionControl> actionControls = new List<ActionControl>();
        public Tree(SolvingLayer Layer)
        {
            layer = Layer;
            InitializeComponent();
        }

        public void ShowElements()
        {
            ActionControl actC;
            int x = 0;
            int X0 = 10;
            int Y0 = 10;
            foreach (var act in layer.Actions)
            {
                x++;

                actC = new ActionControl
                {
                    Name = "act" + x.ToString(),
                    DataContext = act,
                    Margin = new Thickness(X0, Y0, 0, 0)
                };
                actC.MouseDown += Control_MouseDown;
                actC.MouseUp += Control_MouseUp;
                Y0 += 250;
                actionControls.Add(actC);
            }
            X0 = 200;
            Y0 = 10;
            eventControls = new List<EventControl>();
            foreach (var action in layer.Actions)
                foreach (var eventOrigin in layer.EventOrigins)
                {
                    Event eEvent = new Event();
                    EventControl eventControl = new EventControl();
                    eventControl.MouseDown += Control_MouseDown;
                    eventControl.MouseUp += Control_MouseUp;
                    eEvent.ParentAction = action;
                    eEvent.EventOrigin = eventOrigin;
                    eventControl.DataContext = eEvent;
                    eventControl.Margin = new Thickness(X0, Y0, 0, 0);
                    layer.Events.Add(eEvent);
                    action.ChildEvents.Add(eEvent);
                    eventControls.Add(eventControl);
                    Y0 += 200;
                }
            Addlines();
            AddeventsControl();
            AddactionControl();
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowElements();
        }

        private void AddactionControl()
        {
            foreach (var actionControl in actionControls)
            {
                Gridt.Children.Add(actionControl);
            }
        }

        private void AddeventsControl()
        {
            foreach (var eventc in eventControls)
            {
                Gridt.Children.Add(eventc);
            }
        }

        private void Addlines()
        {

            foreach (var actionControl in actionControls)
            {
                foreach (var eventControl in eventControls)
                {
                    if (((Event)eventControl.DataContext).ParentAction == actionControl.DataContext)
                    {
                        DLine dLine = new DLine();
                        dLine.ActionControl = actionControl;
                        dLine.EventControl = eventControl;
                        dLine.ActionIsMain = true;
                        layer.DLines.Add(dLine);
                        var line = dLine.GetLine();
                        Gridt.Children.Add(line);
                    }
                }
            }
        }

        public void Draw(Event eEvent, Action action)
        {
            foreach (var actionControl in actionControls)
            {
                foreach (var eventControl in eventControls)
                {
                    if (eEvent == eventControl.DataContext && action == actionControl.DataContext)
                    {
                        DLine dLine = new DLine();
                        dLine.ActionControl = actionControl;
                        dLine.EventControl = eventControl;
                        dLine.ActionIsMain = false;
                        layer.DLines.Add(dLine);
                        Gridt.Children.Add(dLine.GetLine());
                        break;
                    }
                }
            }
            foreach (var dLine in layer.DLines)
            {
                dLine.RefreshLine();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_canmove)
            {
                point = Mouse.GetPosition(Gridt);
                _control.Margin = new Thickness(point.X - 50, point.Y - 35, 0, 0);
                if (layer.DLines.Count > 0)
                    foreach (var dLine in layer.DLines)
                    {
                        dLine.RefreshLine();
                    }
            }
        }

        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _canmove = true;
            _control = (Control)sender;
        }

        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _canmove = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            navigation = NavigationService.GetNavigationService(this);
            ShowElements();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            AddDepedence win = new AddDepedence();
            win.acts = layer.Actions;
            win.events = layer.Events;
            win.Main = this;
            win.Show();
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            foreach (var even in layer.Events)
            {
                even.SolveWp();
            }
            foreach (var action in layer.Actions)
            {
                if (action.ParentEvent == null)
                {
                    action.SolveEmv();
                }
            }
        }

        private void BtnPrevClick(object sender, RoutedEventArgs e)
        {
            navigation.Navigate(new PageEvents(layer));
        }
    }
}
