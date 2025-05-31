using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<QuickContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<QuickContext>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add these two lines in this exact order
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.StartsWithSegments("/Identity/Account"))
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

// Database initialization with migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QuickContext>();

    // Apply pending migrations
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    // Seed initial data
    await QuickInitializer.Initialize(
        serviceProvider: services,
        DeleteDatabase: false,
        UseMigrations: true,
        SeedSampleData: true
    );
}

app.Run();