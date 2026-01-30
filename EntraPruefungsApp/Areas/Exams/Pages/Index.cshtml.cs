using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "Participant,Examiner,Admin")]
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
        public bool IsExaminer => User.IsInRole("Examiner") || User.IsInRole("Admin");

        public void OnGet()
        {
            // Track current user
            var userEmail = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? 
                           User.FindFirst("preferred_username")?.Value ?? 
                           User.Identity?.Name ?? "Unknown";
            
            var userRoles = new List<string>();
            if (User.IsInRole("Admin")) userRoles.Add("Admin");
            if (User.IsInRole("Examiner")) userRoles.Add("Examiner");
            if (User.IsInRole("Participant")) userRoles.Add("Participant");
            
            if (_userService != null)
                _userService.TrackUser(userEmail, userRoles);
                        Exams = new List<Exam>
            {
                new Exam
                {
                    Id = 1,
                    Title = "Grundlagen der Informatik",
                    Description = "Teste dein Wissen über grundlegende IT-Konzepte",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Was bedeutet CPU?",
                            Answers = new List<string> { "Central Processing Unit", "Computer Processing Unit", "Central Program Unit", "Computer Program Unit" },
                            CorrectAnswer = 0,
                            Type = QuestionType.MultipleChoice
                        },
                        new Question
                        {
                            Text = "Welche Programmiersprache wird hauptsächlich für Webentwicklung verwendet?",
                            Answers = new List<string> { "C++", "JavaScript", "Assembly", "COBOL" },
                            CorrectAnswer = 1,
                            Type = QuestionType.MultipleChoice
                        },
                        new Question
                        {
                            Text = "Erklären Sie den Unterschied zwischen Frontend und Backend in der Webentwicklung.",
                            Type = QuestionType.FreeText,
                            OptimalAnswer = "Frontend ist der sichtbare Teil einer Webanwendung, den Benutzer direkt sehen und mit dem sie interagieren (HTML, CSS, JavaScript). Backend ist der serverseitige Teil, der Datenverarbeitung, Datenbankzugriffe und Geschäftslogik behandelt.",
                            MaxPoints = 3
                        }
                    }
                },
                new Exam
                {
                    Id = 2,
                    Title = "Mathematik Grundlagen",
                    Description = "Einfache mathematische Aufgaben",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Was ist 2 + 2?",
                            Answers = new List<string> { "3", "4", "5", "6" },
                            CorrectAnswer = 1,
                            Type = QuestionType.MultipleChoice
                        },
                        new Question
                        {
                            Text = "Was ist die Quadratwurzel von 16?",
                            Answers = new List<string> { "2", "4", "8", "16" },
                            CorrectAnswer = 1,
                            Type = QuestionType.MultipleChoice
                        },
                        new Question
                        {
                            Text = "Beschreiben Sie, wie Sie das Flächeninhalt eines Kreises berechnen.",
                            Type = QuestionType.FreeText,
                            OptimalAnswer = "Die Fläche eines Kreises wird mit der Formel A = π × r² berechnet, wobei r der Radius des Kreises ist. Pi (π) ist eine mathematische Konstante mit dem Wert etwa 3,14159.",
                            MaxPoints = 2
                        }
                    }
                }
            };

            if (IsExaminer)
            {
                AllResults = _examService.GetAllResults();
            }
        }
    }
}
