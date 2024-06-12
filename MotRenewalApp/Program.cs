using Microsoft.Extensions.Configuration;
using MotRenewalApp.Components;
using MotRenewalApp.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

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

    services.AddHttpClient<IMotService, MotService>(client =>
    {
        client.BaseAddress = new Uri(config["MotApi:BaseAddress"]);
    });

    services.Configure<MotApiSettings>(config.GetSection("MotApi"));
}
