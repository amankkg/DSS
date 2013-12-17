using System.Windows;
using System.Windows.Controls;

namespace DecisionSupportSystem.UserElements
{
    public class ChangeableDataGrid : DataGrid
    {        
        public static readonly DependencyProperty ParamsVisibilityProperty = DependencyProperty.Register(
       "ParamsVisibility", typeof(Visibility), typeof(ChangeableDataGrid), new PropertyMetadata(Visibility.Hidden));

        public Visibility ParamsVisibility
        {
            get { return (Visibility)GetValue(ParamsVisibilityProperty); }
            set { SetValue(ParamsVisibilityProperty, value); }
        }
    }
}
