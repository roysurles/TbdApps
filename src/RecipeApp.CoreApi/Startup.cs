using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Dapper;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

using RecipeApp.CoreApi.Features.HealthChecks;
using RecipeApp.CoreApi.Features.Ingredient.V1_0;
using RecipeApp.CoreApi.Features.Instruction.V1_0;
using RecipeApp.CoreApi.Features.Introduction.V1_0;
using RecipeApp.Database.Ef.RecipeDb;
using RecipeApp.Shared.Handlers;

using Tbd.Shared.ApiLog;
using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.ApiLogging;
using Tbd.WebApi.Shared.CorrelationId;
using Tbd.WebApi.Shared.Extensions;
using Tbd.WebApi.Shared.Filters;
using Tbd.WebApi.Shared.Repositories;
using Tbd.WebApi.Shared.Swagger;


namespace RecipeApp.CoreApi
{
    /// <summary>
    /// The startup class
    /// </summary>
    public class Startup
    {
        // Add Swagger Document Versions to the list here...
        private readonly IEnumerable<string> _swaggerDocumentVersions = new List<string> { "1.0", "1.1" };

        /// <summary>
        /// The ctor
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// The IConfiguration property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The IWebHostEnvironment property
        /// </summary>
        protected IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var defaultConnectionString = Configuration.GetConnectionString("Default");
            var useDapperForDataAccess = Configuration.GetValue<bool>("UseDapperForDataAccess");
            services.Configure<ApiLoggingOptionsModel>(Configuration.GetSection("ApiLogging"));

            services.AddDbContext<RecipeDbContext>(options =>
            {
                options.UseSqlServer(defaultConnectionString)
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            });

            services.AddCors(config =>
                config.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            // https://www.youtube.com/watch?v=p2faw9DCSsY
            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
            services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("SampleHealthCheck")
                .AddSqlServer(defaultConnectionString, name: "DatabaseHealthCheck");
            //.AddCheck<DatabaseHealthCheck>("DatabaseHealthCheck");

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddApiVersioningEx();                      // Custom ApiVersioning

            // Custom Swagger
            var xmlFullFileName = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            services.AddSwaggerEx("RecipeApp CoreApi", "RecipeApp CoreApi", _swaggerDocumentVersions, xmlFullFileName);

            services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

            SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());

            _ = services.AddScoped(_ => new DatabaseHealthCheckRepository(defaultConnectionString));

            _ = useDapperForDataAccess
                ? services.AddScoped<IIntroductionRepositoryV1_0>(_ => new IntroductionRepositoryV1_0(defaultConnectionString))
                : services.AddScoped<IIntroductionRepositoryV1_0, IntroductionEfRepositoryV1_0>();
            services.AddScoped<IIntroductionServiceV1_0, IntroductionServiceV1_0>();

            _ = useDapperForDataAccess
                ? services.AddScoped<IIngredientRepositoryV1_0>(_ => new IngredientRepositoryV1_0(defaultConnectionString))
                : services.AddScoped<IIngredientRepositoryV1_0, IngredientEfRepositoryV1_0>();
            services.AddScoped<IIngredientServiceV1_0, IngredientServiceV1_0>();

            _ = useDapperForDataAccess
                ? services.AddScoped<IInstructionRepositoryV1_0>(_ => new InstructionRepositoryV1_0(defaultConnectionString))
                : services.AddScoped<IInstructionRepositoryV1_0, InstructionEfRepositoryV1_0>();
            services.AddScoped<IInstructionServiceV1_0, InstructionServiceV1_0>();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            // Impose global model state validation to reduce boilerplate code
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddMvcCore(config => config.Filters.Add(typeof(GlobalModelStateValidationFilter)));
            //.SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Render logs for Auth
            if (!WebHostEnvironment.IsProduction())
                IdentityModelEventSource.ShowPII = true;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseSwaggerEx("RecipeApp CoreApi", _swaggerDocumentVersions);    // Custom Swagger

            app.UseCorrelationIdEx();                                           // Custom CorrelationId:  this must be before app.UseApiLoggingEx() to change HttpContext.TraceIdentifier

            app.UseApiLoggingEx();                                              // Custom ApiLogging:  this must be after app.UseSwaggerEx to avoid logging swagger

            app.UseExceptionHandlerEx(env, false);                              // Custom ExceptionHandler:  this must be after app.UseApiLoggingEx to set HttpStatusCode and write out ApiResultModel

            app.UseAuthorization();

            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
            // https://www.youtube.com/watch?v=p2faw9DCSsY
            //app.MapHealthChecks("/healthz");                                  // Note:  Map vs Use;  could not get MapHealthChecks to work, so there is some subtle difference between this app and a new web api app
            //app.UseHealthChecks("/healthz");
            // nuget search for AspNetCore.Healthchecks... AspNetCore.Healthchecks.UI.Client & other packages for specific techs like sql server
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}