using BlazorApp1.Components;
using BlazorApp1.Controller;
using ConnectDB;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Cookie_Service>(); // Register Cookie_Service
builder.Services.AddScoped<Connection>(); // Register the Connection as a scoped service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

// Registering the Authentication Middleware
app.UseMiddleware<Authentication_Controller>();

app.MapStaticAssets();
app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

app.Run();