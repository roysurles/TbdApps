namespace Tbd.WebApi.Shared.Swagger;

public class OrderByDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// Make operations alphabetic.
    /// </summary>
    /// <param name="swaggerDoc">OpenApiDocument</param>
    /// <param name="context">DocumentFilterContext </param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();
        var openApiPaths = new OpenApiPaths();

        foreach (var p in paths)
            openApiPaths.Add(p.Key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.OrdinalIgnoreCase), p.Value);

        swaggerDoc.Paths = openApiPaths;
    }
}
