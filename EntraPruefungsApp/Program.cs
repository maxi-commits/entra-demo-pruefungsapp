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
    options.Conventions.AddPageRoute("/Features/Home/Index", "");
    options.Conventions.AddPageRoute("/Features/Auth/Login", "login");
    options.Conventions.AddPageRoute("/Features/Auth/Logout", "logout");
    options.Conventions.AddPageRoute("/Features/Exams/Index", "exams");
    options.Conventions.AddPageRoute("/Features/Exams/Take", "exam/{id:int}");
    options.Conventions.AddPageRoute("/Features/Exams/Review", "evaluate/{id:int}");
    options.Conventions.AddPageRoute("/Features/Exams/MyResults", "results");
    options.Conventions.AddPageRoute("/Features/Admin/Index", "admin");
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
