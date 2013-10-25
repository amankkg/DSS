//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace DecisionSupportSystem.DbModel
{
    using System;
    using System.Collections.Generic;

    public partial class Event : BasePropertyChanged, IDataErrorInfo
    {
        public Event()
        {
            this.Combinations = new HashSet<Combination>();
            this.EventParams = new HashSet<EventParam>();
        }

        private string _name;
        private decimal _probability;
        public int Id { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public Nullable<decimal> Probability
        {
            get { return _probability; }
            set
            {
                if (value != this._probability)
                {
                    this._probability = (decimal)value;
                    RaisePropertyChanged("Probability");
                }
            }
        }
    
        public virtual ICollection<Combination> Combinations { get; set; }
        public virtual ICollection<EventParam> EventParams { get; set; }

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
}
