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
    public class EventListSTypeViewModel : BasePropertyChanged
    {
        public EventListSTypeViewModel(ActionEvents actionEvents)
        {
            ActionEvents = actionEvents;
            Events = actionEvents.Events;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventSTypeViewModels = new ObservableCollection<EventSTypeViewModel>();
            foreach (var ev in Events)
            {
                EventSTypeViewModels.Add(new EventSTypeViewModel(ev, this));
            }
            Sum();
        }
        
        #region Свойства
        
        public ObservableCollection<Event> Events { get; set; }
        
        public ActionEvents ActionEvents { get; set; }

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

       
        public ObservableCollection<EventSTypeViewModel> EventSTypeViewModels
        {
            get
            {
                return _eventSTypeViewModels;
            }
            set
            {
                if (value != this._eventSTypeViewModels)
                {
                    this._eventSTypeViewModels = value;
                    RaisePropertyChanged("EventViewModels");
                }
            }
        } 

        private ObservableCollection<EventSTypeViewModel> _eventSTypeViewModels;
        #endregion

        #region Методы
        public void Sum()
        {
            if (_eventSTypeViewModels != null)
            {
                ProbabilitySumViewModel.ChangeSum(_eventSTypeViewModels.Select(ev => ev.Probability).ToList().Sum());
            }
        }
        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            try
            {
                _selectedItem = FindIndexInEventListViewModels((EventSTypeViewModel) e.AddedItems[0]);
            }
            catch
            {
                // не обработано
            }
        }

        public void AddEvent(Event ev)
        {
            EventSTypeViewModels.Add(new EventSTypeViewModel(ev, this));
            Events.Add(ev);
            ActionEvents.Events.Add(ev);
            Sum();
        }

        public void UpdateEvents()
        {
            if (EventSTypeViewModels.Count == Events.Count)
                for (int i = 0; i < Events.Count; i++)
                {
                    Events[i].Name = EventSTypeViewModels[i].Name;
                    Events[i].Probability = EventSTypeViewModels[i].Probability;
                }
        }

        private int _selectedItem = -1;

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                EventSTypeViewModels.RemoveAt(_selectedItem);
                ActionEvents.Events.RemoveAt(_selectedItem);
                UpdateEvents();
                Sum();
            }
        }

        public void ValidateEventList(object sender, ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }

        private int FindIndexInEventListViewModels(EventSTypeViewModel element)
        {
            for (int i = 0; i < EventSTypeViewModels.Count; i++)
                if (EventSTypeViewModels[i] == element)
                    return i;
            return -1;
        }
        #endregion
    }
}
