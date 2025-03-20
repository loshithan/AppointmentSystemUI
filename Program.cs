using System.Security.AccessControl;
using AppointmentUI.Components;
using AppointmentUI.Components.Services;
using BlazorServer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register custom services
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
// builder.Services.AddTransient<AuthTokenHandler>();

// Configure HttpClient for API communication
builder.Services.AddHttpClient("ApiClient", opt =>
{
    opt.BaseAddress = new Uri("http://localhost:5047/api/"); // Replace with your API base URL
});
//.AddHttpMessageHandler<AuthTokenHandler>(); // Add the custom handler;

// Add authentication and authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddScheme<CustomOptions, JMTAuthenticationHandler>("JWTAuth", opt => { });
builder.Services.AddScoped<JMTAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JMTAuthenticationStateProvider>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddMudServices();
builder.Services.AddHttpClient(); // Register HttpClient

builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // Register IConfiguration


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
