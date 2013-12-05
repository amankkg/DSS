using System.Windows.Controls;

namespace DecisionSupportSystem
{
    public static class ErrorCount
    {
        public static int EntityListErrorCount = 0;
        public static void CheckEntityListError(ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                EntityListErrorCount++;
            else if (EntityListErrorCount > 0)
                EntityListErrorCount--;
        }

        public static int EntityErrorCount = 0;
        public static void CheckEntityError(ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                EntityErrorCount++;
            else if (EntityErrorCount > 0)
                EntityErrorCount--;
        }

        public static void Reset()
        {
            EntityErrorCount = EntityListErrorCount = 0;
        }
    }
}
