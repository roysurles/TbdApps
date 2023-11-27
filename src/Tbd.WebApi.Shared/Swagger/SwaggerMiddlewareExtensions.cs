namespace Tbd.WebApi.Shared.Swagger;

public static class SwaggerMiddlewareExtensions
{
    public static IServiceCollection AddSwaggerEx(this IServiceCollection services, string title, string description, IEnumerable<string> versions, string xmlPath, bool addBearerTokenAuthorizationInput = true)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            foreach (string version in versions)
            {
                c.SwaggerDoc($"v{version}", new OpenApiInfo
                {
                    Version = $"v{version}",
                    Title = title,
                    Description = description,
                });
            }

            // This call remove version from parameter, without it we will have version as parameter for all endpoints in swagger UI
            c.OperationFilter<RemoveVersionFromParameterOperationFilter>();

            // This will make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
            c.DocumentFilter<ReplaceVersionWithExactValueInPathDocumentFilter>();

            // Order methods
            c.DocumentFilter<OrderByDocumentFilter>();

            // This is used to exclude endpoint mapped to not specified in swagger version.
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);
                return versions.Any(v => $"v{v}" == docName);
            });

            // Set the comments path for the Swagger JSON and UI.
            c.IncludeXmlComments(xmlPath);

            // Add JWT bearer token input for testing.
            if (addBearerTokenAuthorizationInput)
            {
                var openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("Bearer", openApiSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            }
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerEx(this IApplicationBuilder app, string name, IEnumerable<string> versions)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            foreach (string version in versions)
                c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"{name} v{version}");

            c.RoutePrefix = string.Empty;
            c.DocumentTitle = name;
            c.DocExpansion(DocExpansion.None);
        });

        return app;
    }
}
