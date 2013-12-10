using System.ComponentModel;
using DecisionSupportSystem.CommonClasses;


namespace DecisionSupportSystem.DbModel
{
    public abstract class BasePropertyChanged : ErrorValidateEvents, INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
