using System;
using System.ComponentModel;
using System.Windows.Controls;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.ViewModel
{
    public class ProbabilitySumViewModel : BasePropertyChanged, IDataErrorInfo
    {
        #region Свойства
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
        #endregion

        #region Методы
        public void ChangeSum(decimal sum)
        {
            Sum = sum;
        }
        
        #endregion

        public ProbabilitySumViewModel(IErrorCatch errorCatcher)
        {
            ErrorCatcher = errorCatcher;
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
}