using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class PageOptionsTask6ViewModel : BasePropertyChanged
    {
        public Task Task { get; set; }

        private decimal _HeadBonusValue;
        public decimal HeadBonusValue
        {
            get { return _HeadBonusValue; }
            set
            {
                if (value != _HeadBonusValue)
                {
                    _HeadBonusValue = value;
                    RaisePropertyChanged("HeadBonusValue");
                }
            }
        }

        private decimal _TailBonusValue;
        public decimal TailBonusValue
        {
            get { return _TailBonusValue; }
            set
            {
                if (value != _TailBonusValue)
                {
                    _TailBonusValue = value;
                    RaisePropertyChanged("TailBonusValue");
                }
            }
        }

        private decimal _DoubleHeadBonusValue;
        public decimal DoubleHeadBonusValue
        {
            get { return _DoubleHeadBonusValue; }
            set
            {
                if (value != _DoubleHeadBonusValue)
                {
                    _DoubleHeadBonusValue = value;
                    RaisePropertyChanged("DoubleHeadBonusValue");
                }
            }
        }

        private decimal _NumberOfThrowings;
        public decimal NumberOfThrowings
        {
            get { return _NumberOfThrowings; }
            set
            {
                if (value != _NumberOfThrowings)
                {
                    _NumberOfThrowings = value;
                    RaisePropertyChanged("NumberOfThrowings");
                }
            }
        }


        public PageOptionsTask6ViewModel(BaseLayer baseLayer, IErrorCatch errorCatcher)
        {
            Task = baseLayer.Task;
            ErrorCatcher = errorCatcher;
            NumberOfThrowings = Task.TaskParams.ToList()[0].Value;
            HeadBonusValue = Task.TaskParams.ToList()[1].Value;
            TailBonusValue = Task.TaskParams.ToList()[2].Value;
            DoubleHeadBonusValue = Task.TaskParams.ToList()[3].Value;
        }
    }
}
