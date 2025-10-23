using BlazorApp1.Components;
using BlazorApp1.Controller;
using ConnectDB;
using Hashing.src.Hasher;
using Hashing.src;
using BlazorApp1.Utilities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register the Connection as a scoped service
builder.Services.AddScoped<ICust, Cust>();
builder.Services.AddScoped<Cookie_Storage_Accessor>();
builder.Services.AddScoped<Connection>();

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

app.MapStaticAssets();
app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

app.Run();
