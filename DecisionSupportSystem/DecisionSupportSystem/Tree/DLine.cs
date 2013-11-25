using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DecisionSupportSystem.Tree
{
    public class DLine
    {
        public Line Line { get; set; }
        public ActionControl ActionControl { get; set; }
        public EventControl EventControl { get; set; }
        public bool ActionIsMain { get; set; }

        public Line GetLine()
        {
            if (ActionIsMain)
                Line = new Line
                    {
                        X1 = ActionControl.Margin.Left + 155,
                        Y1 = ActionControl.Margin.Top + 70,
                        X2 = EventControl.Margin.Left + 2,
                        Y2 = EventControl.Margin.Top + 70,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 1,
                    };
            else
                Line = new Line
                    { 
                        X1 = EventControl.Margin.Left + 138,
                        Y1 = EventControl.Margin.Top + 70,
                        X2 = ActionControl.Margin.Left + 2,
                        Y2 = ActionControl.Margin.Top + 70,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 1,
                    };
            return Line;
        }

        public void RefreshLine()
        {
            if (ActionIsMain)
            {
                Line.X1 = ActionControl.Margin.Left + 155;
                Line.Y1 = ActionControl.Margin.Top + 70;
                Line.X2 = EventControl.Margin.Left + 2;
                Line.Y2 = EventControl.Margin.Top + 70;
            }
            else
            {
                Line.X1 = EventControl.Margin.Left + 138;
                Line.Y1 = EventControl.Margin.Top + 70;
                Line.X2 = ActionControl.Margin.Left + 2;
                Line.Y2 = ActionControl.Margin.Top + 70;
            }
        }
    }
}
