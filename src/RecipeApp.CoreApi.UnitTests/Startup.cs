using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.UnitTests
{
    public class Startup
    {
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

            services.AddTransient((_) => services.BuildServiceProvider());
        }
    }
}
