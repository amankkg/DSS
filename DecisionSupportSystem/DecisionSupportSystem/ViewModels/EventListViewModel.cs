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
                _selectedItem = FindIndexInEventListViewModels((EventViewModel)e.AddedItems[0]);
        }

        public void AddEvent(Event ev)
        {
            EventViewModels.Add(new EventViewModel(ev, this));
            Events.Add(ev);
            Sum();
        }

        public void UpdateEvents()
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
                UpdateEvents();
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
