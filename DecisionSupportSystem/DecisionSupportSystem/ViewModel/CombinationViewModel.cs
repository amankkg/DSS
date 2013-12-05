using System;
using System.ComponentModel;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.ViewModel
{
    public class CombinationViewModel : BasePropertyChanged, IDataErrorInfo
    {        
        private Combination _combination;
        public Combination Combination
        {
            get { return _combination; }
            set
            {   if (value != this._combination) {
                    this._combination = value;
                    RaisePropertyChanged("Combination");}
            }
        }
        public CombinationsViewModel CombinationWithoutParamsViewModel { get; set; }

        public CombinationViewModel(Combination combination, CombinationsViewModel combinationWithoutParamsViewModel)
        {
            CombinationWithoutParamsViewModel = combinationWithoutParamsViewModel;
            this.Combination = combination;
        }
        
        #region Реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                switch (columnName)
                {
                    case "Combination":
                        break;
                }
                return errormsg;
            }
        }
        #endregion

    }
}
