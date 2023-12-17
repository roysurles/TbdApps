namespace RecipeApp.FCoreApi
#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Reflection
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Diagnostics.HealthChecks
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.RateLimiting;
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

open Dapper

open FluentValidation
open FluentValidation.AspNetCore

open HealthChecks.UI.Client

open RecipeApp.FCoreApi.Features.Extensions.ApiVersioningMiddlewareExtensions
open RecipeApp.FCoreApi.Features.Extensions.ApiLoggingMiddlewareExtensions
open RecipeApp.FCoreApi.Features.Extensions.ExceptionHandlerMiddlewareExtensions
open RecipeApp.FCoreApi.Features.Extensions.SwaggerMiddlewareExtensions
open RecipeApp.FCoreApi.Features.Extensions.CorrelationIdMiddlewareExtensions
open RecipeApp.FCoreApi.Features.HealthChecks
open RecipeApp.FCoreApi.Features.Introduction.V1_0.Introduction

open RecipeApp.Shared.Handlers

open Tbd.Shared.ApiLog
open Tbd.Shared.ApiResult

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        let defaultConnectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.Configure<ApiLoggingOptionsModel>(builder.Configuration.GetSection("ApiLogging"));

        // TODO F#: services.AddRateLimiter

        // TODO F#: services.AddDbContext<RecipeDbContext>

        builder.Services.AddCors(fun options ->
            options.AddPolicy("AllowAll", fun builder ->
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader() |> ignore ))
            |> ignore

        // https://www.youtube.com/watch?v=p2faw9DCSsY
        // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
        builder.Services.AddHealthChecks()
            .AddCheck<SampleHealthCheck>("SampleHealthCheck")
            .AddSqlServer(defaultConnectionString, "DatabaseHealthCheck");
        //.AddCheck<DatabaseHealthCheck>("DatabaseHealthCheck");

        builder.Services.AddControllers()

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddApiVersioningEx();                      // Custom ApiVersioning

        builder.Services.AddEndpointsApiExplorer();
        // https://levelup.gitconnected.com/how-to-use-fluentvalidation-in-asp-net-core-net-6-543d52bd36b4
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Custom Swagger
        let swaggerDocumentVersions = [ "1.0"; "1.1" ]
        let xmlFullFileName = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        builder.Services.AddSwaggerGenEx("RecipeApp F# CoreApi", "RecipeApp F# CoreApi", swaggerDocumentVersions, xmlFullFileName);

        // TODO F#: builder.Services.AddTransient(typedefof<IApiResultModel<_>>, typedefof<IApiResultModel<_>>);

        SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());

        builder.Services.AddScoped<IIntroductionRepositoryV1_0>(fun _ -> new IntroductionRepositoryV1_0(defaultConnectionString))

        let app = builder.Build()

        if app.Environment.IsDevelopment()
        then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseStaticFiles()

        app.UseRouting()

        app.UseCors("AllowAll")

        app.UseHttpsRedirection()

        app.UseSwaggerEx("RecipeApp F# CoreApi", swaggerDocumentVersions)  // Custom Swagger

        app.UseCorrelationIdEx()                                           // Custom CorrelationId:  this must be before app.UseApiLoggingEx() to change HttpContext.TraceIdentifier

        app.UseApiLoggingEx()                                              // Custom ApiLogging:  this must be after app.UseSwaggerEx to avoid logging swagger

        app.UseExceptionHandlerEx(app.Environment, false);                 // Custom ExceptionHandler:  this must be after app.UseApiLoggingEx to set HttpStatusCode and write out ApiResultModel

        app.UseAuthorization()
        //app.MapControllers()

        // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
        // https://www.youtube.com/watch?v=p2faw9DCSsY
        //app.MapHealthChecks("/healthz");                                  // Note:  Map vs Use;  could not get MapHealthChecks to work, so there is some subtle difference between this app and a new web api app
        //app.UseHealthChecks("/healthz");
        // nuget search for AspNetCore.Healthchecks... AspNetCore.Healthchecks.UI.Client & other packages for specific techs like sql server
        //let healthCheckOptions = new HealthCheckOptions()
        //healthCheckOptions.ResponseWriter <- fun (context, healthReport) -> async {UIResponseWriter.WriteHealthCheckUIResponse(context, healthReport)} |> Async.AwaitTask
        //healthCheckOptions.ResponseWriter <- async { UIResponseWriter.WriteHealthCheckUIResponse() } |> Async.StartAsTask :> Task

        //healthCheckOptions.ResponseWriter = (fun<HttpContext,HealthReport,Task>)UIResponseWriter.WriteHealthCheckUIResponse()

        // TODO F#: app.UseHealthChecks("/healthz", new HealthCheckOptions(ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse)) |> ignore

        // app.UseRateLimiter();       // This has to go after app.UseRouting(); --> https://github.com/dotnet/aspnetcore/issues/45302

        //app.UseEndpoints(endpoints => endpoints.MapControllers());
        // https://github.com/dotnet/aspnetcore/issues/45302
        // https://nicolaiarocci.com/on-implementing-the-asp.net-core-7-rate-limiting-middleware/
        app.UseEndpoints(fun endpoints ->
            let b = endpoints.MapControllers()
            b.RequireRateLimiting("FixedWindowLimiter") |> ignore
        );

        app.Run()

        exitCode
