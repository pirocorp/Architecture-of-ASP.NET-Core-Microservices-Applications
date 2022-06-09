namespace Common.Infrastructure.Exceptions
{
    public class UninitializedPropertyException : InvalidOperationException
    {
        private const string UninitializedPropertyExceptionMessage = "Uninitialized property: {0}";

        public UninitializedPropertyException(string propertyName)
            : base(string.Format(UninitializedPropertyExceptionMessage, propertyName))
        {
        }
    }
}
