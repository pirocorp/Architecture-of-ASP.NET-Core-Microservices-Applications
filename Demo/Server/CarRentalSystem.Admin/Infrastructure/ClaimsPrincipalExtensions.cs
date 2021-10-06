namespace CarRentalSystem.Admin.Infrastructure
{
    using System.Security.Claims;

    using static CarRentalSystem.Common.Constants;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdministrator(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
