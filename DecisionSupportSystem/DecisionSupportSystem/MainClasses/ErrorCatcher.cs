using System.Windows.Controls;

namespace DecisionSupportSystem.MainClasses
{
    public class ErrorCatcher : IErrorCatch
    {
        public int EntityErrorCount { get; set; }
        public int EntityGroupErrorCount { get; set; }

        public ErrorCatcher()
        {
            EntityErrorCheck = EntityValidationError;
            EntityGroupErrorCheck = EntityGroupValidationError;
        }

        public void Reset()
        {
            EntityErrorCount = EntityGroupErrorCount = 0;
        }

        #region Реализация интерфейса IErrorCatch
        public EntityErrorCheck EntityErrorCheck { get; set; }
        public EntityGroupErrorCheck EntityGroupErrorCheck { get; set; }
        public void EntityValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                EntityErrorCount++;
            else if (EntityErrorCount > 0)
                EntityErrorCount--;
        }
        public void EntityGroupValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                EntityGroupErrorCount++;
            else if (EntityGroupErrorCount > 0)
                EntityGroupErrorCount--;
        }
        #endregion


    }
}
