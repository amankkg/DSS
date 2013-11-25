using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace DecisionSupportSystem.Tree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Main: Window
    { 
        public Main()
        {
            InitializeComponent();
        }


        private Point point; 
        private bool _canmove;
        private Control _control;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*acts = new List<Action>();
            actionControls =new List<ActionControl>();
            var act1 = new Action {Name = "A1", Credit = 5000};
            acts.Add(act1);
            var actC1 = new ActionControl();
            actC1.Name = "a1";
            actC1.DataContext = act1;
            actC1.Margin = new Thickness(10,10,0,0);
            actC1.MouseDown += Control_MouseDown;
            actC1.MouseUp += Control_MouseUp;
            actionControls.Add(actC1);
            var act2 = new Action { Name = "A2", Credit = 1000 };
            acts.Add(act2);
            var actC2 = new ActionControl();
            actC2.DataContext = act2;
            actC2.Name = "a2";
            actC2.MouseDown += Control_MouseDown;
            actC2.MouseUp += Control_MouseUp;
            actionControls.Add(actC2);
            var act3 = new Action { Name = "A3", Credit = 4200 };
            acts.Add(act3);
            var actC3 = new ActionControl();
            actC3.DataContext = act3;
            actC3.Name = "a3";
            actC3.MouseDown += Control_MouseDown;
            actC3.MouseUp += Control_MouseUp;
            actionControls.Add(actC3);

            List<EventOrigin> eventOrigins = new List<EventOrigin>();
            eventOrigins.Add(new EventOrigin
                {
                    Name = "E1",
                    Probability = Convert.ToDecimal(0.25),
                });
            eventOrigins.Add(new EventOrigin
                {
                    Name = "E2",
                    Probability = Convert.ToDecimal(0.75),
                });

            events = new List<Event>();
            eventControls = new List<EventControl>();
            foreach (var action in acts)
                foreach (var eventOrigin in eventOrigins)
                {
                    Event eEvent = new Event();
                    EventControl eventControl = new EventControl();
                    eventControl.MouseDown += Control_MouseDown;
                    eventControl.MouseUp += Control_MouseUp;
                    eEvent.ParentAction = action;
                    eEvent.EventOrigin = eventOrigin;
                    eventControl.DataContext = eEvent;
                    events.Add(eEvent);
                    action.ChildEvents.Add(eEvent);
                    eventControls.Add(eventControl);
                }
            dLines = new List<DLine>();*/
           
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           /* foreach (var actionControl in actionControls)
            {
                Gridt.Children.Add(actionControl);
            }*/
        } 
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          /*  foreach (var eventc in eventControls)
            {
                Gridt.Children.Add(eventc);
            }*/
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
             
            /*foreach (var actionControl in actionControls)
            {
                foreach (var eventControl in eventControls)
                {
                    if (((Event)eventControl.DataContext).ParentAction == actionControl.DataContext)
                    {
                        DLine dLine = new DLine();
                        dLine.ActionControl = actionControl;
                        dLine.EventControl = eventControl;
                        dLine.ActionIsMain = true;
                        dLines.Add(dLine);
                        var line = dLine.GetLine();
                        Gridt.Children.Add(line);
                    }
                }
            }*/
        }

        public void Draw(Event eEvent, Action action)
        {/*
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
                        dLines.Add(dLine);
                        Gridt.Children.Add(dLine.GetLine());
                        break;
                    }
                }
            }
            foreach (var dLine in dLines)
            {
                dLine.RefreshLine();
            }*/
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
           /* if (_canmove)
            {   
                point = Mouse.GetPosition(Gridt);
                _control.Margin = new Thickness(point.X-50, point.Y-35, 0, 0);
                if (dLines.Count>0)
                    foreach (var dLine in dLines)
                    {
                        dLine.RefreshLine();
                    }
            }*/
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

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            /*AddDepedence win = new AddDepedence();
            win.acts = acts;
            win.events = events;
            win.Main = this;
            win.Show();*/
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
           /* foreach (var even in events)
            {
                even.SolveWp();
            }
            foreach (var action in acts)
            {
                if (action.ParentEvent == null)
                {
                    action.SolveEmv();
                }
            }*/
        }

    }
}
