using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DecisionSupportSystem.DbModel
{
    using System;
    using System.Collections.Generic;

    public partial class Action : BasePropertyChanged, IDataErrorInfo
    {
        public Action()
        {
            this.ActionParams = new HashSet<ActionParam>();
            this.Combinations = new HashSet<Combination>();
        }
    
        public int Id { get; set; }
        private string _name = string.Empty;
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
        public Nullable<decimal> Emv { get; set; }
        public Nullable<decimal> Eol { get; set; }
        public Guid SavingId { get; set; }
    
        public virtual ICollection<ActionParam> ActionParams { get; set; }
        public virtual ICollection<Combination> Combinations { get; set; }

        #region реализация интерфейса IDataErrorInfo
        public string Error { get { throw new NotImplementedException(); } }

        public string this[string columnName]
        {
            get
            {
                string errormsg = null;
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        errormsg = "Введите название действия";
                    }
                }
                return errormsg;
            }
        }
        #endregion
    }
}
