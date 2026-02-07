using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "Participant,Examiner")]
    public class IndexModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ExamService _examService;

        public IndexModel(ExamService examService, UserService userService)
        {
            _examService = examService;
            _userService = userService;
        }

        public List<Exam> Exams { get; set; } = new();
        public Dictionary<string, List<ExamResult>> AllResults { get; set; } = new();
        public bool IsExaminer => User.IsInRole("Examiner");

        public void OnGet()
        {
            var userEmail = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? 
                           User.FindFirst("preferred_username")?.Value ?? 
                           User.Identity?.Name ?? "Unknown";
            
            var userRoles = new List<string>();
            if (User.IsInRole("Admin")) userRoles.Add("Admin");
            if (User.IsInRole("Examiner")) userRoles.Add("Examiner");
            if (User.IsInRole("Participant")) userRoles.Add("Participant");
            
            _userService?.TrackUser(userEmail, userRoles);

            Exams = _examService.GetExams();

            if (IsExaminer)
                AllResults = _examService.GetAllResults();
        }
    }
}
