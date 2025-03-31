using Microsoft.EntityFrameworkCore;
using CollaborativePresentations.Data;
using CollaborativePresentations.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel FIRST (before any other configurations)
builder.WebHost.ConfigureKestrel(serverOptions => 
{
    serverOptions.ListenAnyIP(5074); // HTTP
    serverOptions.ListenAnyIP(5075, listenOptions => // HTTPS
    { 
        listenOptions.UseHttps();
    });
});

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAll", policy => 
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true)
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// CORS must come after Routing and before Authorization
app.UseCors("AllowAll");

app.UseAuthorization();

// Initialize database (moved after all configurations)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Endpoint routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PresentationHub>("/presentationHub");

app.Run();