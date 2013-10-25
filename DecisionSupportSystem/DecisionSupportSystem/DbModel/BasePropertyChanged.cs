using System.ComponentModel;


namespace DecisionSupportSystem.DbModel
{
    public abstract class BasePropertyChanged : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
