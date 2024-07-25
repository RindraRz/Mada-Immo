using Mada_immo.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection; // Assurez-vous d'inclure cette directive pour utiliser AddOptions

var builder = WebApplication.CreateBuilder(args);

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); // Utilisez Distributed Memory Cache pour le stockage en mémoire (optionnel)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Définissez la durée d'inactivité avant expiration de la session
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ImmoContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication(); // Assurez-vous d'appeler UseAuthentication avant UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=AdminLog}/{id?}");

app.Run();
