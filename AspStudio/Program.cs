using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspStudio.Data;
using AspStudio.Repositories.UserRepo;
using AspStudio.Models;
using AspStudio.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register DbContexts
var IdentityconnectionString = builder.Configuration.GetConnectionString("IdentityBEMISDBContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(IdentityconnectionString));

var BEMISconnectionString = builder.Configuration.GetConnectionString("BEMISDBContextConnection");
builder.Services.AddDbContext<BEMISDbContext>(options =>
    options.UseSqlServer(BEMISconnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BEMISUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//// Register Application Identity services (SignInManager, UserManager, etc.)
//builder.Services.AddDefaultIdentity<BEMISUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()  // Use your ApplicationDbContext for EF Core
//    .AddDefaultTokenProviders();

// Explicitly register RoleManager
//builder.Services.AddScoped<RoleManager<IdentityRole>>();
//builder.Services.AddScoped<IUser, User_Repo>();
builder.Services.AddScoped<Utility>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add Sidebar menu json file
builder.Configuration.AddJsonFile("sidebar.json", optional: true, reloadOnChange: true);


//builder.Services.AddIdentity<ApplicationDbContext, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);
app.MapControllerRoute(
    name: "Identity",
    pattern: "Identity/{controller=Auth}/{action=Login}"
    //defaults: new { area = "Identity" }
);

app.MapRazorPages();
app.MapFallback(context => {
	context.Response.Redirect("/Pages/ErrorPage");
	return Task.CompletedTask;
});
app.Run();
