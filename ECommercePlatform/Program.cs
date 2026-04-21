using ECommercePlatform.Data; // Import the namespace for data access and DbContext
using Microsoft.AspNetCore.Identity; // Import Identity namespace for user management
using Microsoft.EntityFrameworkCore; // Import EF Core namespace for database operations

var builder = WebApplication.CreateBuilder(args); // Initialize the web application builder

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); // Retrieve the database connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => // Register the ApplicationDbContext service
    options.UseSqlServer(connectionString)); // Configure the context to use SQL Server with the provided connection string
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Add a filter that provides helpful error pages for EF migrations during development

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) // Configure ASP.NET Core Identity with required account confirmation
    .AddRoles<IdentityRole>() // Add support for Role-based authorization
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Tell Identity to use EF Core and the ApplicationDbContext for storage
builder.Services.AddAuthentication() // Initialize authentication services
    .AddGoogle(options => // Add and configure Google external authentication
    {
        options.ClientId = builder.Configuration["Google:ClientId"]; // Set the Google Client ID from the configuration secrets
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]; // Set the Google Client Secret from the configuration secrets
    });

builder.Services.AddControllersWithViews(); // Register services required for MVC controllers and views
builder.Services.AddRazorPages(); // Register services for Razor Pages

var app = builder.Build(); // Build the application instance
using (var scope = app.Services.CreateScope()) // Create a temporary dependency injection scope
{
    var services = scope.ServiceProvider; // Get the service provider from the scope
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); // Resolve the Role Manager service
    var userManger = services.GetRequiredService<UserManager<IdentityUser>>(); // Resolve the User Manager service
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Resolve the database context
    await SeedData.SeedUsersAndRoles(services, userManger, roleManager); // Asynchronously seed roles and initial users
    await SeedData.SeedSuppliers(services); // Asynchronously seed initial supplier data
    await SeedData.SeedProducts(services); // Asynchronously seed initial product data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Check if the app is running in the Development environment
{
    app.UseMigrationsEndPoint(); // Enable the endpoint that allows applying migrations via the browser
}
else // Configuration for non-development environments (Production/Staging)
{
    app.UseExceptionHandler("/Home/Error"); // Set the global error handling route
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); // Enforce HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // Redirect all HTTP requests to HTTPS
app.UseRouting(); // Enable the routing middleware to match requests to endpoints

app.UseAuthorization(); // Enable authorization middleware to check user permissions

app.MapStaticAssets(); // Map static files (CSS, JS, Images) using the optimized .NET 9 asset system

app.MapControllerRoute( // Define the default route pattern for MVC controllers
    name: "default", // Name of the route
    pattern: "{controller=Home}/{action=Index}/{id?}") // Pattern: Controller/Action/OptionalId (defaults to Home/Index)
    .WithStaticAssets(); // Ensure static assets are linked to this routing

app.MapRazorPages() // Map endpoints for Razor Pages
   .WithStaticAssets(); // Ensure static assets are linked to Razor Pages

app.Run(); // Start the web application

builder.Services.AddRazorPages(); // (Duplicate) Register Razor Pages services again

builder.Services.AddDefaultIdentity<IdentityUser>() // (Duplicate) Register Identity services again
    .AddEntityFrameworkStores<ApplicationDbContext>(); // (Duplicate) Specify the EF store again