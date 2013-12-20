using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class EventsWithExtensionsViewModel : BasePropertyChanged
    {
        private const int OUT_OF_RANGE = -1;
        public BaseLayer BaseLayer { get; set; }
        
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<EventWithExtensionViewModel> EventWithExtensionViewModels { get; set; }
       
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

        public EventsWithExtensionsViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            base.ErrorCatcher = errorCatcher;
            this.BaseLayer = baseLayer;
            this.Events = this.BaseLayer.DssDbContext.Events.Local;
            this.EventWithExtensionViewModels = new ObservableCollection<EventWithExtensionViewModel>();
            this.ProbabilitySumViewModel = new ProbabilitySumViewModel(base.ErrorCatcher);
            foreach (var ev in this.Events)
                this.EventWithExtensionViewModels.Add(new EventWithExtensionViewModel(ev, this, base.ErrorCatcher));
        }

        public EventsWithExtensionsViewModel(ObservableCollection<Event> events, BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            this.Events = events;
            base.ErrorCatcher = errorCatcher;
            this.BaseLayer = baseLayer;
            this.EventWithExtensionViewModels = new ObservableCollection<EventWithExtensionViewModel>();
            this.ProbabilitySumViewModel = new ProbabilitySumViewModel(base.ErrorCatcher);
            foreach (var ev in this.Events)
                this.EventWithExtensionViewModels.Add(new EventWithExtensionViewModel(ev, this, base.ErrorCatcher));
        }

        public void AddEvent(Event even)
        {
            var thisEventsHaveEven = this.Events.Any(ev => ev.Name.Trim() == even.Name.Trim());
            if (thisEventsHaveEven) return;
            this.EventWithExtensionViewModels.Add(new EventWithExtensionViewModel(even, this, base.ErrorCatcher));
            this.Events.Add(even);
        }

        private int _selectedEvent = OUT_OF_RANGE;

        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                try { this._selectedEvent = this.FindIndexInEventViewModels((EventWithExtensionViewModel)e.AddedItems[0]); }
                catch { this._selectedEvent = OUT_OF_RANGE; }
        }

        private int FindIndexInEventViewModels(EventWithExtensionViewModel element)
        {
            for (int i = 0; i < EventWithExtensionViewModels.Count; i++)
                if (EventWithExtensionViewModels[i] == element)
                    return i;
            return OUT_OF_RANGE;
        }

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedEvent <= OUT_OF_RANGE || Events.Count == 0) return;
            EventWithExtensionViewModels.RemoveAt(_selectedEvent);
            BaseLayer.BaseMethods.DeleteEvent(Events[_selectedEvent]);
            Events.RemoveAt(_selectedEvent);
            SumProbabilities();
        }

        public void SumProbabilities()
        {
            if (Events != null)
                ProbabilitySumViewModel.ChangeSum(Events.Select(ev => ev.Probability).ToList().Sum());
        }

        public void UpdateEvent(EventWithExtensionViewModel callEventViewModel)
        {
            if (EventWithExtensionViewModels.Count != Events.Count || !EventWithExtensionViewModels.Contains(callEventViewModel)) return;
            int index = EventWithExtensionViewModels.IndexOf(callEventViewModel);
            RenameSimilarEvents(callEventViewModel);
            Events[index].Name = callEventViewModel.Name;
        }

        public void UpdateEventParams(EventWithExtensionViewModel callEventViewModel)
        {
            foreach (var eventWithExtensionViewModel in EventWithExtensionViewModels)
            {
                if (eventWithExtensionViewModel != callEventViewModel)
                {
                    eventWithExtensionViewModel.IsExtendableEvent = false;
                }
            }
        }

        void RenameSimilarEvents(EventWithExtensionViewModel callEventViewModel)
        {
            var simeventslist = SearchSimilarEvents(callEventViewModel.Name.Trim()).ToList();
            foreach (var ev in simeventslist)
            {
                if (EventWithExtensionViewModels[Events.IndexOf(ev)] == callEventViewModel) continue;
                string name = callEventViewModel.Name;
                EventWithExtensionViewModels[Events.IndexOf(ev)].Name = name + "*";
                ev.Name = name + "*";
            }
        }

        IEnumerable<Event> SearchSimilarEvents(string eventname)
        {
            return Events.Where(ev => ev.Name.Trim() == eventname).Select(ev => ev).ToList();
        }
    }
}
