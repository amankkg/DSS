using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModels
{
    public class EventListViewModel : BasePropertyChanged
    {
        public EventListViewModel(ObservableCollection<Event> events, BaseLayer baseLayer)
        {
            Events = events;
            _baseLayer = baseLayer;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventViewModels = new ObservableCollection<EventViewModel>();
            foreach (var ev in Events)
            {
                EventViewModels.Add(new EventViewModel(ev, this));
            }
            Sum();
        }

        public EventListViewModel(BaseLayer baseLayer)
        {
            Events = baseLayer.DssDbContext.Events.Local;
            _baseLayer = baseLayer;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventViewModels = new ObservableCollection<EventViewModel>();
            foreach (var ev in Events)
            {
                EventViewModels.Add(new EventViewModel(ev, this));
            }
            Sum();
        }
        
        #region Свойства
        
        public ObservableCollection<Event> Events { get; set; }
        
        public BaseLayer BaseLayer {
            get { return _baseLayer; }
            set { _baseLayer = value; }
        }
        private BaseLayer _baseLayer;

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

        private ProbabilitySumViewModel _probabilitySumViewModel;

       
        public ObservableCollection<EventViewModel> EventViewModels
        {
            get
            {
                return _eventViewModels;
            }
            set
            {
                if (value != this._eventViewModels)
                {
                    this._eventViewModels = value;
                    RaisePropertyChanged("EventViewModels");
                }
            }
        } 

        private ObservableCollection<EventViewModel> _eventViewModels;
        #endregion

        #region Методы
        public void Sum()
        {
            if (_eventViewModels != null)
            {
                ProbabilitySumViewModel.ChangeSum(_eventViewModels.Select(ev => ev.Probability).ToList().Sum());
            }
        }
        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            try
            {
                _selectedItem = FindIndexInEventListViewModels((EventViewModel) e.AddedItems[0]);
            }
            catch
            {
                // не обработано
            }
        }

        public void AddEvent(Event ev)
        {
            var haveThisEvInEvents = Events.Any(e => e.Name.Trim() == ev.Name.Trim());
            if (haveThisEvInEvents) return;
            EventViewModels.Add(new EventViewModel(ev, this));
            Events.Add(ev);
            _baseLayer.DssDbContext.Events.Local.Add(ev);
            Sum();
        }

        public void UpdateEvent(EventViewModel callEventViewModel)
        {
            if (EventViewModels.Count != Events.Count || !EventViewModels.Contains(callEventViewModel)) return;
            int index = EventViewModels.IndexOf(callEventViewModel);
            RenameSimilarEvents(callEventViewModel);
            Events[index].Name = callEventViewModel.Name;
            Events[index].Probability = callEventViewModel.Probability;
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


        public void UpdateAllEvents()
        {
            if (EventViewModels.Count == Events.Count)
                for (int i = 0; i < Events.Count; i++)
                {
                    Events[i].Name = EventViewModels[i].Name;
                    Events[i].Probability = EventViewModels[i].Probability;
                }
        }

        private int _selectedItem = -1;

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                EventViewModels.RemoveAt(_selectedItem);
                _baseLayer.BaseMethods.DeleteEvent(Events[_selectedItem]);
                Events.RemoveAt(_selectedItem);
                UpdateAllEvents();
                Sum();
            }
        }

        public void ValidateEventList(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        private int FindIndexInEventListViewModels(EventViewModel element)
        {
            for (int i = 0; i < EventViewModels.Count; i++)
                if (EventViewModels[i] == element)
                    return i;
            return -1;
        }
        #endregion
    }
}
