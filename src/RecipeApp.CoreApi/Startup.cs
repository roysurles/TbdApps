using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using Dapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

using RecipeApp.CoreApi.Features.Ingredient;
using RecipeApp.CoreApi.Features.Instruction;
using RecipeApp.CoreApi.Features.Introduction;
using RecipeApp.Shared.Handlers;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Options;
using Tbd.WebApi.Shared.Extensions;
using Tbd.WebApi.Shared.Filters;
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
        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiLoggingOptionsModel>(Configuration.GetSection("ApiLogging"));

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddApiVersioningEx();                      // Custom ApiVersioning

            // Custom Swagger
            var xmlFullFileName = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            services.AddSwaggerEx("RecipeApp CoreApi", "RecipeApp CoreApi", _swaggerDocumentVersions, xmlFullFileName);

            services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

            var defaultConnectionString = Configuration.GetConnectionString("Default");
            SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());

            services.AddScoped<IIntroductionRepository>(_ => new IntroductionRepository(defaultConnectionString));
            services.AddScoped<IIntroductionService, IntroductionService>();

            services.AddScoped<IIngredientRepository>(_ => new IngredientRepository(defaultConnectionString));

            services.AddScoped<IInstructionRepository>(_ => new InstructionRepository(defaultConnectionString));

            // Impose global model state validation to reduce boilerplate code
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddMvcCore(config => config.Filters.Add(typeof(GlobalModelStateValidationFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

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

            app.UseHttpsRedirection();

            app.UseSwaggerEx("RecipeApp CoreApi", _swaggerDocumentVersions);   // Custom Swagger

            app.UseApiLoggingEx();                                                  // Custom ApiLogging:  this must be after app.UseSwaggerEx to avoid logging swagger

            app.UseCorrelationIdEx();                                               // Custom CorrelationId

            app.UseExceptionHandlerEx(env, false);                   // Custom ExceptionHandler  this must be after app.UseApiLoggingEx to set HttpStatusCode and write out ApiResultModel

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}