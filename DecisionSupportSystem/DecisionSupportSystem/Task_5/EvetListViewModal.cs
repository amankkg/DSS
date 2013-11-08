using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Task_5
{
    public class ProbabilitySumViewModal : BasePropertyChanged
    {
        private int _sum;
        public int Sum
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
        public ProbabilitySumViewModal()
        {
            Sum = 1;
        }
         
        public void EntityValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                MessageBox.Show("Hello +");
            else
                MessageBox.Show("Hello -");
        }
    }

    public class EvetListViewModal : BasePropertyChanged
    {
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
                }
            }
        }

        private int _probab;
        public int Probability
        {
            get
            {
                return _probab;
            }
            set
            {
                if (value != this._probab)
                {
                    this._probab = value;
                    RaisePropertyChanged("Probability");
                }
            }
        }

        public EvetListViewModal(string name, int probab)
        {
            Name = name;
            Probability = probab;
        }
    }

    public class MainEvetListViewModal
    { 
        public ObservableCollection<EvetListViewModal> EvetListViewModals { get; set; }
        public ProbabilitySumViewModal ProbabilitySumViewModal { get; set; }
        public MainEvetListViewModal(int prob)
        {
            EvetListViewModals = new ObservableCollection<EvetListViewModal>
                {
                    new EvetListViewModal("C1", 1), 
                    new EvetListViewModal("C2", 2),
                    new EvetListViewModal("C3", 3)
                };
            ProbabilitySumViewModal = new ProbabilitySumViewModal();
            ProbabilitySumViewModal.Sum = prob;
        }
    }
}
