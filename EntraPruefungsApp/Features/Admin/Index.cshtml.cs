using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using System.Security.Claims;

namespace EntraPruefungsApp.Features.Admin;

[Authorize(Roles = "Admin")]
public class AdminModel : PageModel
    {
        private readonly UserService _userService;

        public AdminModel(UserService userService)
        {
            _userService = userService;
        }

        public Dictionary<string, List<string>> UsersWithRoles { get; set; } = new();

        public void OnGet()
        {
            // Track current user with email and roles
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? 
                           User.FindFirst("preferred_username")?.Value ?? 
                           User.Identity?.Name ?? "Unknown";
            
            // Check different role claim types that Azure AD might use
            var userRoles = new List<string>();
            userRoles.AddRange(User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));
            userRoles.AddRange(User.Claims.Where(c => c.Type == "roles").Select(c => c.Value));
            userRoles.AddRange(User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(c => c.Value));
            
            // If no roles found, determine from User.IsInRole checks
            if (!userRoles.Any())
            {
                if (User.IsInRole("Admin")) userRoles.Add("Admin");
                if (User.IsInRole("Examiner")) userRoles.Add("Examiner");
                if (User.IsInRole("Participant")) userRoles.Add("Participant");
            }
            
            _userService.TrackUser(userEmail, userRoles);
            
            UsersWithRoles = _userService.GetAllUsersWithRoles();
        }
    }
