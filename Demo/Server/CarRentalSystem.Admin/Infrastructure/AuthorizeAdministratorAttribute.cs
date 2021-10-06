namespace CarRentalSystem.Admin.Infrastructure
{
    using Microsoft.AspNetCore.Authorization;

    using static CarRentalSystem.Common.Constants;

    public class AuthorizeAdministratorAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorAttribute() => this.Roles = AdministratorRoleName;
    }
}
