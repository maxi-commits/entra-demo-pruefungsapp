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
    options.Conventions.AddPageRoute("/Exams/Index", "exams");
    options.Conventions.AddPageRoute("/Exams/MyResults", "results");
    options.Conventions.AddPageRoute("/Exams/Exam", "participate/exam/{id:int}");
    options.Conventions.AddPageRoute("/Exams/ExamReview", "evaluate/{id:int}");
    options.Conventions.AddPageRoute("/Auth/Login", "login");
    options.Conventions.AddPageRoute("/Auth/Logout", "logout");
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
