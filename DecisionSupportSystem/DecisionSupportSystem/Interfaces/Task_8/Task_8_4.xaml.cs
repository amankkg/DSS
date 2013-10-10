using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DecisionSupportSystem
{
    /// <summary>
    /// Логика взаимодействия для Page_4.xaml
    /// </summary>
    public partial class Page_4 : Page
    {
       // private InterfaceData _data;

        public Page_4()
        {
            //InitializeComponent();
        }

      /*  public Page_4(InterfaceData Idata)
        {
            InitializeComponent();
            _data = Idata;
        }
        */
        private void BtnShowSolution_Click(object sender, RoutedEventArgs e)
        {
            /*using (var db = new DSSDBContext())
            {
                var ActionsList = new List<ActionIData>
                    {
                        new ActionIData {Name = "A1", RequiredDefect = new Parameter{Name = "ksdkj", Value = 0}},
                        new ActionIData {Name = "A2", RequiredDefect = new Parameter{Name = "ksdkj", Value = 0}},
                    };
                var EventsList = new List<EventIData>
                    {
                        new EventIData {Name = "E1", Probability = 0, Defect = new Parameter{Name = "ksdkj", Value = 0}},
                        new EventIData {Name = "E2", Probability = 0, Defect = new Parameter{Name = "ksdkj", Value = 0}},
                    };
            var task = new Task{Description = "desc", Date = DateTime.Now};
                var combins = new List<Combination>();
                foreach (var a in ActionsList)
                   foreach (var ev in EventsList)
                       combins.Add(new Combination
                            {
                                Action = a,
                                Event = ev,
                                Task = task,
                            });
                foreach (var actionIData in ActionsList)
                {
                    db.Actions.Add(actionIData);
                }
                foreach (var eventIData in EventsList)
                {
                    db.Events.Add(eventIData);
                }
                db.Tasks.Add(task);
                foreach (var combination in combins)
                {
                    db.Combinations.Add(combination);
                }
                foreach (var c in combins)
                {
                    var ReqDefect = new Parameter();
                    var Defect = new Parameter();
                    ReqDefect.Name = "rdef";
                    ReqDefect.Value = (from a in ActionsList
                                       where a == c.Action
                                       select a.RequiredDefect).First().Value;
                    ReqDefect.Combination = c;
                    Defect.Name = "def";
                    Defect.Value = (from ev in EventsList
                                       where ev == c.Event
                                       select ev.Defect).First().Value;
                    Defect.Combination = c;
                    db.Parameters.Add(ReqDefect);
                    db.Parameters.Add(Defect);
                }
                db.SaveChanges();*/

            }


        }
    }

