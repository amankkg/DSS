//using BaseModel;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.CommonClasses
{
    public class SolvedTaskView : BasePropertyChanged
    {
        private string _recomendation;
        public string Recommendation 
        { 
            get { return _recomendation; } 
            set
            {
                if (value != this._recomendation)
                {
                    this._recomendation = value; RaisePropertyChanged("Recommendation");
                }
            } 
        }

        private decimal _maxEmv;
        public decimal MaxEmv
        {
            get { return _maxEmv; }
            set
            {
                if (value != this._maxEmv)
                {
                    this._maxEmv = value; RaisePropertyChanged("MaxEmv");
                }
            }
        }

        private decimal _minEol;
        public decimal MinEol
        {
            get { return _minEol; }
            set
            {
                if (value != this._minEol)
                {
                    this._minEol = value; RaisePropertyChanged("MinEol");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value != this._comment)
                {
                    this._comment = value; RaisePropertyChanged("Comment");
                }
            }
        }
    }
}
