using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using EntraPruefungsApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();
builder.Services.AddSingleton<ExamService>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddRazorPages(options =>
{
    // Disable default area routes to avoid conflicts
    options.Conventions.AddAreaFolderRouteModelConvention("Exams", "/", model => model.Selectors.Clear());
    options.Conventions.AddAreaFolderRouteModelConvention("Auth", "/", model => model.Selectors.Clear());
    options.Conventions.AddAreaFolderRouteModelConvention("Admin", "/", model => model.Selectors.Clear());
    
    // Add custom routes
    options.Conventions.AddAreaPageRoute("Exams", "/Index", "exams");
    options.Conventions.AddAreaPageRoute("Exams", "/MyResults", "results");
    options.Conventions.AddAreaPageRoute("Exams", "/Exam", "participate/{id:int}");
    options.Conventions.AddAreaPageRoute("Exams", "/ExamReview", "evaluate/{id:int}");
    options.Conventions.AddAreaPageRoute("Auth", "/Login", "login");
    options.Conventions.AddAreaPageRoute("Auth", "/Logout", "logout");
    options.Conventions.AddAreaPageRoute("Admin", "/Index", "admin");
})
    .AddMicrosoftIdentityUI();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
