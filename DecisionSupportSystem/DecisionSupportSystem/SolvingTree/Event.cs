using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DecisionSupportSystem.SolvingTree
{
    public class Event : INotifyPropertyChanged
    {
        public EventOrigin EventOrigin { get; set; }
        private decimal _wp;
        private decimal _cp;
        public decimal Cp
        {
            get { return _cp; }
            set
            {
                if (value != this._cp)
                {
                    this._cp = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public decimal Wp 
        {
            get { return _wp; } 
            set
            {
                if (value != this._wp)
                {
                    this._wp = value;
                    NotifyPropertyChanged();
                }
            } 
        } 
        public decimal YearCount { get; set; }
        public Action ParentAction { get; set; }
        public List<Action> ChildActions { get; set; }

        public Event()
        {
            ChildActions = new List<Action>();
        }

        public void SolveWp()
        {
            if (ParentAction.ParentEvent != null)
                Wp = (Cp * YearCount - ParentAction.Credit) * EventOrigin.Probability + ParentAction.ParentEvent.Wp;
            else
                Wp = (Cp * YearCount - ParentAction.Credit) * EventOrigin.Probability;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
