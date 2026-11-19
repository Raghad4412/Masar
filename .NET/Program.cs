using Masar.Data;
using Masar.Support;
using Masar.Support.Console;
using Masar.Support.Exceptions;
using Masar.Support.Middleware;
using Masar.Support.Services;
using Masar.Support.Services.Gateway;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetSection("Masar").Bind(Config.Masar);
Config.WebRoot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Config.AppName = builder.Configuration["App:Name"] ?? "Masar";
Config.Values["app.name"] = Config.AppName;
Config.Values["app.url"] = builder.Configuration["App:Url"] ?? "http://localhost";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddEncryptedCookies();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.Name = "masar_session";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
    });

builder.Services
    .AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new TrimStrings());
    })
    .AddSessionStateTempDataProvider()
    .AddCsrfProtection();

builder.Services.AddExceptionHandler<Handler>();

builder.Services.AddScoped<IPaymentGateway>(_ => Config.Masar.Gateway switch
{
    _ => new FakeGateway(),
});
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<SlotService>();
builder.Services.AddScoped<RatingService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IdeaService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<VerificationService>();

builder.Services.AddScoped<ExpireHolds>();
builder.Services.AddScoped<CloseStaleRequests>();
builder.Services.AddScoped<ExpireIdeas>();
builder.Services.AddHostedService<Kernel>();

var app = builder.Build();

Routes.Configure(app.Services.GetRequiredService<LinkGenerator>(), app.Services.GetRequiredService<IHttpContextAccessor>());
Http.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

using (var scope = app.Services.CreateScope())
{
    try
    {
        DbSeeder.Run(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "The database could not be seeded.");
    }
}

app.UseTrustedHosts();
app.UseTrustedProxies();
app.UseMiddleware<PreventRequestsDuringMaintenance>();
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    AllowStatusCode404Response = true,
    ExceptionHandler = _ => Task.CompletedTask,
});
app.UseStaticFiles();
app.UseMiddleware<MethodOverride>();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Authenticate>();
app.UseMiddleware<RedirectIfAuthenticated>();
app.UseMiddleware<RoleMiddleware>();
app.UseMiddleware<ActiveMiddleware>();
app.UseMiddleware<ValidateSignature>();
app.UseMiddleware<RunDueTasks>();

app.MapControllers();

app.Run();
