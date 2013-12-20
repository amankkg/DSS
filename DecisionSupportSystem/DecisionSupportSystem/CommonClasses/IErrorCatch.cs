using System.Windows.Controls;

namespace DecisionSupportSystem.CommonClasses
{
    public interface IErrorCatch
    {
        int EntityErrorCount { get; set; }
        int EntityGroupErrorCount { get; set; }
        EntityErrorCheck EntityErrorCheck { get; set; }
        EntityGroupErrorCheck EntityGroupErrorCheck { get; set; }
        void EntityValidationError(object sender, ValidationErrorEventArgs e);
        void EntityGroupValidationError(object sender, ValidationErrorEventArgs e);
    }
    public delegate void EntityErrorCheck(object sender, ValidationErrorEventArgs e);
    public delegate void EntityGroupErrorCheck(object sender, ValidationErrorEventArgs e);
}
