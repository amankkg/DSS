using System.Windows.Controls;
using System.Windows.Input;

namespace DecisionSupportSystem.MainClasses
{
    public class PagePattern
    { 
        #region Поля
        public int _entityErrorCount = 0;    // количество ошибок связанных с вводом данных в элементах упраления
        public int _dataGridErrorCount = 0;  // количество ошибок связанных с вводом данных в datagrid
        public BaseTaskLayer baseTaskLayer;  
        #endregion

        public PagePattern()
        {
            baseTaskLayer = new BaseTaskLayer();
        }

        public void EntityValidationError(ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _entityErrorCount++;
            else
                _entityErrorCount--;
        }
         
        public void DatagridValidationError(ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _dataGridErrorCount++;
            else
                _dataGridErrorCount--;
        }
        // разрешает (не разрешает) нажатие кнопки "Добавить"
        public void EntityAddCanExecute(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _entityErrorCount == 0;
            e.Handled = true;
        }
        // разрешает (не разрешает) нажатие кнопок "<< назад" и "далее >>"
        public void NavigatePageCanExecute(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _dataGridErrorCount == 0;
            e.Handled = true;
        }
    }
}
