using System.Windows.Controls;

namespace DecisionSupportSystem.CommonClasses
{
    public abstract class ErrorValidateEvents
    {
        public IErrorCatch ErrorCatcher { get; set; }

        public void EntityValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCatcher.EntityErrorCheck(sender, e);
        }

        public void EntityGroupValidationError(object sender, ValidationErrorEventArgs e)
        {
            ErrorCatcher.EntityGroupValidationError(sender, e);
        }
    }
}
