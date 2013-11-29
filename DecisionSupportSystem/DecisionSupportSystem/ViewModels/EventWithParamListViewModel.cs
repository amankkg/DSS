using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using Event = DecisionSupportSystem.DbModel.Event;

namespace DecisionSupportSystem.ViewModels
{
    public class EventWithParamListViewModel : BasePropertyChanged
    {
     /*   public EventWithParamListViewModel(ObservableCollection<Event> events, BaseLayer baseLayer)
        {
            Events = events;
            EventParams = baseLayer.DssDbContext.EventParams.Local;
            _baseLayer = baseLayer;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventWithParamViewModels = new ObservableCollection<EventWithParamViewModel>();
            foreach (var ev in Events)
            {
                EventWithParamViewModels.Add(new EventWithParamViewModel(ev, ev.EventParams.ToList()[0], this));
            }
            Sum();
        }*/

        public EventWithParamListViewModel(BaseLayer baseLayer)
        {
            Events = baseLayer.DssDbContext.Events.Local;
            EventParams = baseLayer.DssDbContext.EventParams.Local;
            _baseLayer = baseLayer;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventWithParamViewModels = new ObservableCollection<EventWithParamViewModel>();
            foreach (var ev in Events)
            {
                EventWithParamViewModels.Add(new EventWithParamViewModel(ev, ev.EventParams.ToList()[0], this));
            }
            Sum();
        }
        
        #region Свойства
        
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<EventParam> EventParams { get; set; }
        private BaseLayer _baseLayer;

        public BaseLayer BaseLayer {
            get { return _baseLayer; }
            set { _baseLayer = value; }
        }

        private ObservableCollection<EventWithParamViewModel> _eventWithParamViewModels;
        public ObservableCollection<EventWithParamViewModel> EventWithParamViewModels
        {
            get
            {
                return _eventWithParamViewModels;
            }
            set
            {
                if (value != this._eventWithParamViewModels)
                {
                    this._eventWithParamViewModels = value;
                    RaisePropertyChanged("EventWithParamViewModels");
                }
            }
        }

       
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

       
        #endregion

        #region Методы
        public void Sum()
        {
            if (_eventWithParamViewModels != null)
            {
                ProbabilitySumViewModel.ChangeSum(_eventWithParamViewModels.Select(ev => ev.Probability).ToList().Sum());
            }
        }

        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                _selectedItem = FindIndexInEventListViewModels((EventWithParamViewModel)e.AddedItems[0]);
        }

        public void UpdateEvent(EventWithParamViewModel callEventViewModel)
        {
            if (EventWithParamViewModels.Count != Events.Count || !EventWithParamViewModels.Contains(callEventViewModel)) return;
            int index = EventWithParamViewModels.IndexOf(callEventViewModel);
            RenameSimilarEvents(callEventViewModel);
            Events[index].Name = callEventViewModel.Name;
            Events[index].Probability = callEventViewModel.Probability;
            NavigationWindowShower.IsSaved = false;
        }

        public void AddEvent(Event ev, EventParam eventParam)
        {
            var haveThisEvInEvents = Events.Any(e => e.Name.Trim() == ev.Name.Trim());
            if (haveThisEvInEvents) return;
            EventWithParamViewModels.Add(new EventWithParamViewModel(ev, eventParam, this));
            Events.Add(ev);
            eventParam.Event = ev;
            EventParams.Add(eventParam);
            _baseLayer.DssDbContext.Events.Local.Add(ev);
            Sum();
            NavigationWindowShower.IsSaved = false;
        }

        public void UpdateEvents()
        {
            if (EventWithParamViewModels.Count == Events.Count)
                for (int i = 0; i < Events.Count; i++)
                {
                    Events[i].Name = EventWithParamViewModels[i].Name;
                    Events[i].Probability = EventWithParamViewModels[i].Probability;

                }
        }

        private int _selectedItem = -1;

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                EventWithParamViewModels.RemoveAt(_selectedItem);
                _baseLayer.BaseMethods.DeleteEvent(Events[_selectedItem]);
                Events.RemoveAt(_selectedItem);
                UpdateEvents();
                NavigationWindowShower.IsSaved = false;
                Sum();
            }
        }

        public void ValidateEventList(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        private int FindIndexInEventListViewModels(EventWithParamViewModel element)
        {
            for (int i = 0; i < EventWithParamViewModels.Count; i++)
                if (EventWithParamViewModels[i] == element)
                    return i;
            return -1;
        }

        void RenameSimilarEvents(EventWithParamViewModel callEventWithParamViewModel)
        {
            var simeventslist = SearchSimilarEvents(callEventWithParamViewModel.Name.Trim()).ToList();
            foreach (var ev in simeventslist)
            {
                if (EventWithParamViewModels[Events.IndexOf(ev)] == callEventWithParamViewModel) continue;
                string name = callEventWithParamViewModel.Name;
                EventWithParamViewModels[Events.IndexOf(ev)].Name = name + "*";
                ev.Name = name + "*";
            }
        }

        IEnumerable<Event> SearchSimilarEvents(string eventname)
        {
            return Events.Where(ev => ev.Name.Trim() == eventname).Select(ev => ev).ToList();
        }


        #endregion
    }
}
