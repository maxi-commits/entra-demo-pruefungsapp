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
    options.Conventions.AddAreaFolderRouteModelConvention("Exams", "/", model =>
    {
        foreach (var selector in model.Selectors.ToList())
        {
            model.Selectors.Remove(selector);
        }
    });
    
    options.Conventions.AddAreaPageRoute("Exams", "/Index", "exams");
    options.Conventions.AddAreaPageRoute("Exams", "/MyResults", "results");
    options.Conventions.AddAreaPageRoute("Exams", "/Exam", "exam/{id:int}");
    options.Conventions.AddAreaPageRoute("Exams", "/ExamReview", "evaluate/{id:int}");
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