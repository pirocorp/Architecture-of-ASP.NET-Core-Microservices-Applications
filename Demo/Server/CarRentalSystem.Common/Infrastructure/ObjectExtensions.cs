namespace CarRentalSystem.Common.Infrastructure
{
    public static class ObjectExtensions
    {
        public static string GetAssemblyName(this object obj)
            => obj.GetType().Assembly.GetName().Name;
    }
}
