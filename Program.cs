var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add support for static files (wwwroot)
builder.Services.AddControllers(); // In case you want to use API endpoints later

// OPTIONAL: Add TempData support for things like alerts
builder.Services.AddSession(); // For session-based storage (if needed)

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable serving static files from wwwroot (e.g., CSS/JS)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Enable sessions if you're using session-based storage
app.UseSession();

app.MapRazorPages(); // Maps Razor Pages to routes

app.Run();







