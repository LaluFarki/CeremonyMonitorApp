using CeremonyMonitorApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new SessionAuthorizeAttribute());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}
);

//ini adalah cara kita kasih tahu EF Core: "pakai SQL Server, dan ambil connection string dari appsettings.json"
// dan ini dinamakan dependency injection — EF Core akan otomatis bikin instance AppDbContext buat kita, dan kita bisa pakai di Controller atau Service lain.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Program.cs (before builder.Build())
builder.WebHost.UseUrls("http://localhost:5137");
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

//Routing menentukan request harus menuju ke mana.
// MISAL request ke /Ceremonies/Details/5, maka routing akan memanggil method Details di CeremoniesController, dengan parameter id=5
app.UseRouting();

app.UseSession();

//Authorization Ini menangani pemeriksaan izin akses.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
