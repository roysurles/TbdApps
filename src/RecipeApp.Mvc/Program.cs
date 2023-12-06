var builder = WebApplication.CreateBuilder(args);

var defaultJsonDeSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

// Session, Cookies & Caching only stores strings... objects need to be serialized / deserialized to/from json
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0
// https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.sqlserver.sqlservercache?view=dotnet-plat-ext-3.1
// https://code-maze.com/aspnetcore-distributed-caching/
// dotnet tool install --global dotnet-sql-cache
// dotnet sql-cache create "Server=localhost;Initial Catalog=Cache;Persist Security Info=False;Integrated Security=SSPI;Connection Timeout=30;TrustServerCertificate=true;" dbo Cache
// TODO: move this in the Recipe db
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("CacheSqlServer");
    options.SchemaName = "dbo";
    options.TableName = "Cache";
});

//var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7023") };  // TODO:  need to fetch builder.HostEnvironment.BaseAddress
builder.Services.AddScoped(_ => httpClient);

var apiUrlsOptionsModel = new ApiUrlsOptionsModel();
builder.Configuration.GetSection("ApiUrls").Bind(apiUrlsOptionsModel);
builder.Services.AddSingleton(_ => apiUrlsOptionsModel);

// TODO:  this has to be different because of NavigationManager... builder.Services.AddSingleton<ISessionViewModel, SessionViewModel>();
//builder.Services.AddScoped<CustomMessageHandler>(_ => new CustomMessageHandler(null));
builder.Services.AddScoped<CustomMvcMessageHandler>();
builder.Services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
    .AddHttpMessageHandler<CustomMvcMessageHandler>();

builder.Services.AddHttpClient<IIntroductionApiClientNativeV1_0>(client =>
    client.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
    .AddHttpMessageHandler<CustomMvcMessageHandler>()
    .AddTypedClient<IIntroductionApiClientNativeV1_0>(client => new IntroductionApiClientNativeV1_0(client, "/api/v1.0/Introduction", defaultJsonDeSerializerOptions: defaultJsonDeSerializerOptions));

builder.Services.AddTransient<IIntroductionViewModel, IntroductionViewModel>();
builder.Services.AddSingleton<IIntroductionSearchViewModel, IntroductionSearchViewModel>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//using (var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;

//    var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
//    httpContextAccessor.HttpContext!.Session.Clear();

//    httpContextAccessor.HttpContext!.Session.SetString("_forceSession", string.Empty);
//}


app.Run();
