using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EntraPruefungsApp.Pages.Auth;

public class LoginModel : PageModel
{
    public IActionResult OnGet()
    {
        // Redirect zu Microsoft Entra ID Login
        return Challenge(
            new AuthenticationProperties { RedirectUri = "/" },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}