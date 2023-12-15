namespace RecipeApp.FCoreApi.Features.Extensions

open System.Runtime.InteropServices
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
open Swashbuckle.AspNetCore.SwaggerUI

module SwaggerMiddlewareExtensions =
    let NewOpenApiInfo version title description =
        let openApiInfo = new OpenApiInfo(Version = version, Title = title, Description = description)
        openApiInfo

    let New_Header_OpenApiSecurityScheme =
        let openApiSecurityScheme = new OpenApiSecurityScheme()
        openApiSecurityScheme.In <- ParameterLocation.Header
        openApiSecurityScheme.Description <- "Please insert JWT with Bearer into field"
        openApiSecurityScheme.Name <- "Authorization"
        openApiSecurityScheme.Scheme <- "bearer"
        openApiSecurityScheme.Type <- SecuritySchemeType.ApiKey
        openApiSecurityScheme.BearerFormat <- "JWT"
        openApiSecurityScheme

    let NewOpenApiReference =
        let openApiReference = new OpenApiReference(Type = ReferenceType.SecurityScheme, Id = "Bearer")
        openApiReference

    let New_Bearer_OpenApiSecurityScheme =
        let openApiSecurityScheme = new OpenApiSecurityScheme(Reference = NewOpenApiReference)
        openApiSecurityScheme

    let NewOpenApiSecurityRequirement =
        let openApiSecurityRequirement = new OpenApiSecurityRequirement()
        openApiSecurityRequirement.Add(New_Bearer_OpenApiSecurityScheme, new System.Collections.Generic.List<string>())
        openApiSecurityRequirement

    type IServiceCollection with
        member this.AddSwaggerGenEx(title: string, description: string, versions : seq<string>
        , xmlPath : string, [<Optional; DefaultParameterValue(true)>] addBearerTokenAuthorizationInput : bool) =
            this.AddSwaggerGen(fun options ->
            for version in versions do
                options.SwaggerDoc($"v{version}", NewOpenApiInfo $"v{version}" title description)

            // Order methods
            options.OrderActionsBy(fun f -> f.RelativePath)  // this appears to work instead of the original options.DocumentFilter<OrderByDocumentFilter>

            // Set the comments path for the Swagger JSON and UI.
            options.IncludeXmlComments(xmlPath)

            // Add JWT bearer token input for testing.
            if addBearerTokenAuthorizationInput
            then
                options.AddSecurityDefinition("Bearer", New_Header_OpenApiSecurityScheme)
                options.AddSecurityRequirement(NewOpenApiSecurityRequirement)

            ) |> ignore
            this

    type IApplicationBuilder with
        member this.UseSwaggerEx(name: string, versions: seq<string>) =
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            this.UseSwagger() |> ignore

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            this.UseSwaggerUI(fun options ->
                // https://fsharpforfunandprofit.com/troubleshooting-fsharp/
                for version in versions do
                    options.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"{name} v{version}")

                options.RoutePrefix <- ""
                options.DocumentTitle <- name
                options.DocExpansion(DocExpansion.None)
            ) |> ignore
            this