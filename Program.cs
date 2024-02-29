using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var cstr = builder.Configuration.GetConnectionString("ContosoPizza");
// Add the "ContosoPizza" connection string to
// Secrets Manager (secrets.json or dotnet user-secrets set)
// before you run the app!
//builder.Services.AddDbContext<ContosoPizzaContext>(options =>
//    options
//        .use(cstr, ServerVersion.AutoDetect(cstr)));

builder.Services.AddDbContext<ContosoPizzaContext>(options =>
        options.UseNpgsql(cstr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ContosoPizzaContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
