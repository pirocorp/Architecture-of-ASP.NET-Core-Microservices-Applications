﻿namespace CarRentalSystem.Common.Infrastructure
{
    using System.Security.Claims;

    using static Common.Constants;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdministrator(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
