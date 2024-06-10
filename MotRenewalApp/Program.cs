using Microsoft.Extensions.Configuration;
using MotRenewalApp.Components;
using MotRenewalApp.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static void ConfigureServices(IServiceCollection services, ConfigurationManager config)
{
    // Add services to the container.
    services.AddRazorComponents()
        .AddInteractiveServerComponents();

    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://beta.check-mot.service.gov.uk") });

    services.Configure<MotApiSettings>(config.GetSection("MOTApi"));
    services.AddScoped<IMotService, MotService>();
}
