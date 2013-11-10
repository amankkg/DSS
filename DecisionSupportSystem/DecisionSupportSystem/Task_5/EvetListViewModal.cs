using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DecisionSupportSystem.DbModel;
using Microsoft.Practices.Prism.Commands;

namespace DecisionSupportSystem.Task_5
{
    public class ProbabilitySumViewModel : BasePropertyChanged, IDataErrorInfo
    {
        private decimal _sum;
        public decimal Sum
        { 
            get
            {
                return _sum;
            }
            set
            {
                if (value != this._sum)
                {
                    this._sum = value;
                    RaisePropertyChanged("Sum");
                }
            }
        }

        public void ChangeSum(decimal sum)
        {
            Sum = sum;
        }

        #region реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                switch (columnName)
                {
                    case "Sum":
                        {
                            if (Sum != 1)
                                errormsg = "Сумма вероятностей должно равняться 1.";
                        }
                        break;
                }
                return errormsg;
            }
        }
        #endregion
    }

    public class EventListViewModel : BasePropertyChanged, IDataErrorInfo
    {
        public MainEvetListViewModel MainEventListModel { get; set; }
    
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    RaisePropertyChanged("Name");
                    MainEventListModel.UpdateEvents();
                }
            }
        }

        private decimal _probability;
        public decimal Probability
        {
            get
            {
                return _probability;
            }
            set
            {
                if (value != this._probability)
                {
                    this._probability = value;
                    RaisePropertyChanged("Probability");
                    MainEventListModel.UpdateEvents();
                    MainEventListModel.Sum();
                }
            }
        }

        public EventListViewModel(string name, decimal probability, MainEvetListViewModel mainEventListModal)
        {
            MainEventListModel = mainEventListModal;
            Name = name;
            Probability = probability;
        }

        #region реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            errormsg = "Введите название события";
                        break;
                    case "Probability":
                        {
                            if (Probability > 1)
                                errormsg = "Вероятность не должна превышать 1";
                            if (Probability == 0)
                                errormsg = "Введите вероятность.";
                        }
                        break;
                }
                return errormsg;
            }
        }
        #endregion
    }

    public class MainEvetListViewModel : BasePropertyChanged
    {
        public MainEvetListViewModel(ObservableCollection<Event> events)
        {
            Events = events;
            ProbabilitySumViewModel = new ProbabilitySumViewModel();
            EventListViewModels = new ObservableCollection<EventListViewModel>();
            foreach (var ev in Events)
            {
                EventListViewModels.Add(new EventListViewModel(ev.Name, ev.Probability, this));
            }
            Sum();
        }

        public void Sum()
        {
            if (EventListViewModels != null)
            {
                ProbabilitySumViewModel.ChangeSum(EventListViewModels.Select(ev => ev.Probability).ToList().Sum());
            }
        }
        
        private ObservableCollection<EventListViewModel> _eventListViewModels;
        private ProbabilitySumViewModel _probabilitySumViewModel;
        
        public ObservableCollection<Event> Events { get; set; }
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
        public ObservableCollection<EventListViewModel> EventListViewModels
        {
            get
            {
                return _eventListViewModels;
            }
            set
            {
                if (value != this._eventListViewModels)
                {
                    this._eventListViewModels = value;
                    RaisePropertyChanged("EventListViewModels");
                }
            }
        }
        
        public void SelectEvent(object sender, SelectionChangedEventArgs e)
        { 
            if (e.AddedItems.Count > 0)
                _selectedItem = FindIndexInEventListViewModels((EventListViewModel)e.AddedItems[0]);
        }

        public void AddEvent(Event ev)
        {
            EventListViewModels.Add(new EventListViewModel(ev.Name, ev.Probability, this));
            Events.Add(ev);
            Sum();
        }
 
        public void UpdateEvents()
        {
            if (EventListViewModels.Count == Events.Count)
            for (int i = 0; i < Events.Count; i++)
            {
                Events[i].Name = EventListViewModels[i].Name;
                Events[i].Probability = EventListViewModels[i].Probability;
            }
        }

        private int _selectedItem = -1;

        public void DeleteEvent(object sender, RoutedEventArgs e)
        {
            if (_selectedItem > -1)
            {
                EventListViewModels.RemoveAt(_selectedItem);
                Events.RemoveAt(_selectedItem);
                UpdateEvents();
                Sum();
            }
        }


        private int FindIndexInEventListViewModels(EventListViewModel element)
        {
            for (int i = 0; i < EventListViewModels.Count; i++)
                if (EventListViewModels[i] == element)
                    return i;
            return -1;
        }

       
    }
}
