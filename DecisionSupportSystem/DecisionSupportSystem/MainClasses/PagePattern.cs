using System.Windows.Controls;
using System.Windows.Input;

namespace DecisionSupportSystem.MainClasses
{
    public class PagePattern
    { 
        #region Поля
        public int _entityErrorCount = 0;    // количество ошибок связанных с вводом данных в элементах упраления
        public int _dataGridErrorCount = 0;  // количество ошибок связанных с вводом данных в datagrid
        public BaseLayer baseLayer;  
        #endregion

        public PagePattern()
        {
            baseLayer = new BaseLayer();
        }

        public void EntityValidationError(ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityError(e);
        }
         
        public void DatagridValidationError(ValidationErrorEventArgs e)
        {
            ErrorCount.CheckEntityListError(e);
        }
        // разрешает (не разрешает) нажатие кнопки "Добавить"
        public void EntityAddCanExecute(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityErrorCount == 0;
            e.Handled = true;
        }
        // разрешает (не разрешает) нажатие кнопок "<< назад" и "далее >>"
        public void NavigatePageCanExecute(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ErrorCount.EntityListErrorCount == 0;
            e.Handled = true;
        }
    }
}
