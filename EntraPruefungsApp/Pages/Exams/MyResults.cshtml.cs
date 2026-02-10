using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "Participant")]
    public class MyResultsModel : PageModel
    {
        private readonly ExamService _examService;

        public MyResultsModel(ExamService examService)
        {
            _examService = examService;
        }

        public List<ExamResult> MyResults { get; set; } = new();
        public List<Exam> Exams { get; set; } = new();

        public void OnGet()
        {
            var userId = User.Identity?.Name ?? "anonymous";
            MyResults = _examService.GetResults(userId).OrderByDescending(r => r.Date).ToList();
            Exams = _examService.GetExams();
        }
    }
}
