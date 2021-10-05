namespace CarRentalSystem.Common.Infrastructure.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context
                .MethodInfo
                .GetCustomAttributes(true)
                .Union(context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? Array.Empty<object>())
                .ToList();

            var anonymousAttributeExists = attributes
                .Any(x => x is AllowAnonymousAttribute);

            var authorizeAttributeExists = attributes
                .Any(x => x is AuthorizeAttribute);

            if (authorizeAttributeExists && !anonymousAttributeExists)
            {
                operation.Security = new List<OpenApiSecurityRequirement> {
                    new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    }
                };
            }
        }
    }
}
