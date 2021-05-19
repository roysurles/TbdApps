using System;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tbd.WebApi.Shared.Swagger
{
    public class ReplaceVersionWithExactValueInPathDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var openApiPaths = new OpenApiPaths();

            foreach (var p in swaggerDoc.Paths)
                openApiPaths.Add(p.Key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.OrdinalIgnoreCase), p.Value);

            swaggerDoc.Paths = openApiPaths;
        }
    }
}
