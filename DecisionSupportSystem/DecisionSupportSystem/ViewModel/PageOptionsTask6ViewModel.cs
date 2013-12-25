using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.CommonClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class PageOptionsTask6ViewModel : BasePropertyChanged
    {
        public Task Task { get; set; }

        private double _HeadBonusValue;
        public double HeadBonusValue
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

        private double _TailBonusValue;
        public double TailBonusValue
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

        private double _DoubleHeadBonusValue;
        public double DoubleHeadBonusValue
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

        private double _NumberOfThrowings;
        public double NumberOfThrowings
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


        public PageOptionsTask6ViewModel(BaseAlgorithms baseLayer, IErrorCatch errorCatcher)
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
