using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DecisionSupportSystem.SolvingTree
{
    public class Action : INotifyPropertyChanged
    {
        public string Name { get;set;}
        public decimal Credit { get; set; }
        private decimal _emv;
        public decimal Emv
        {
            get { return _emv; }
            set
            {
                if (value != this._emv)
                {
                    this._emv = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Event ParentEvent { get; set; }
        public List<Event> ChildEvents { get; set; }

        public Action()
        {
            ChildEvents = new List<Event>();
        }

        private decimal CalcEmv(Action act)
        {
            foreach (var childEvent in act.ChildEvents)
            {
                if (childEvent.ChildActions.Count == 0)
                {
                    act.Emv = act.Emv + childEvent.Wp;
                }
                else
                {
                    foreach (var action in childEvent.ChildActions)
                    {
                        act.Emv = act.Emv + CalcEmv(action);
                    }
                }
            }
            return act.Emv;
        }

        public void SolveEmv()
        {
            Emv = CalcEmv(this);
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
