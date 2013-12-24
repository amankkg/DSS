using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;
using System.Collections.ObjectModel;

namespace DecisionSupportSystem.ViewModel
{
    public class EventsViewModel : BasePropertyChanged
    {
        private const int OUT_OF_RANGE = -1;
        
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<EventViewModel> EventViewModels { get; set; }
       
        private ProbabilitySumViewModel _probabilitySumViewModel;
        public ProbabilitySumViewModel ProbabilitySumViewModel
        {
            get
            {
                return _probabilitySumViewModel;
            }
            set
            {
                if (value != this._probabilitySumViewModel)
                {
                    this._probabilitySumViewModel = value;
                    RaisePropertyChanged("ProbabilitySumViewModel");
                }
            }
        }
        public Visibility ParamsVisibility { get; set; }
        public EventsViewModel(ObservableCollection<Event> events, IErrorCatch errorCatcher)
        {
            base.ErrorCatcher = errorCatcher;
            this.Events = events;
            this.EventViewModels = new ObservableCollection<EventViewModel>();
            this.ProbabilitySumViewModel = new ProbabilitySumViewModel(base.ErrorCatcher);
            foreach (var ev in this.Events)
                this.EventViewModels.Add(new EventViewModel(ev, this, base.ErrorCatcher));
        }

        public void AddEvent(Event even)
        {
            var thisEventsHaveEven = this.Events.Any(ev => ev.Name.Trim() == even.Name.Trim());
            if (thisEventsHaveEven) return;
            this.EventViewModels.Add(new EventViewModel(even, this, base.ErrorCatcher));
            this.Events.Add(even);
        }

        private int _selectedEvent = OUT_OF_RANGE;

        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                try {  this._selectedEvent = this.FindIndexInEventViewModels((EventViewModel)e.AddedItems[0]); }
                catch { this._selectedEvent = OUT_OF_RANGE; }
        }

        private int FindIndexInEventViewModels(EventViewModel element)
        {
            for (int i = 0; i < EventViewModels.Count; i++)
                if (EventViewModels[i] == element)
                    return i;
            return OUT_OF_RANGE;
        }

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedEvent <= OUT_OF_RANGE || Events.Count == 0) return;
            EventViewModels.RemoveAt(_selectedEvent);
            CRUD.DeleteEvent(Events[_selectedEvent]);
            //Events.RemoveAt(_selectedEvent);
            SumProbabilities();
        }

        public void SumProbabilities()
        {
            if (Events != null)
            {
                ProbabilitySumViewModel.ChangeSum(Events.Select(ev => ev.Probability).ToList().Sum());
                ProbabilitySumViewModel.Sum = Math.Round(ProbabilitySumViewModel.Sum, 5);
            }
        }

        public void UpdateEvent(EventViewModel callEventViewModel)
        {
            if (EventViewModels.Count != Events.Count || !EventViewModels.Contains(callEventViewModel)) return;
            int index = EventViewModels.IndexOf(callEventViewModel);
            RenameSimilarEvents(callEventViewModel);
            Events[index].Name = callEventViewModel.Name;
        }

        void RenameSimilarEvents(EventViewModel callEventViewModel)
        {
            var simeventslist = SearchSimilarEvents(callEventViewModel.Name.Trim()).ToList();
            foreach (var ev in simeventslist)
            {
                if (EventViewModels[Events.IndexOf(ev)] == callEventViewModel) continue;
                string name = callEventViewModel.Name;
                EventViewModels[Events.IndexOf(ev)].Name = name + "*";
                ev.Name = name + "*";
            }
        }

        IEnumerable<Event> SearchSimilarEvents(string eventname)
        {
            return Events.Where(ev => ev.Name.Trim() == eventname).Select(ev => ev).ToList();
        }
    }
}
