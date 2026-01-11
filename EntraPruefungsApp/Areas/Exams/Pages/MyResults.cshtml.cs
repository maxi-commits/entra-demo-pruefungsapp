using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "User")]
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
                            Type = QuestionType.MultipleChoice,
                            MaxPoints = 2
                        },
                        new Question
                        {
                            Text = "Welche Programmiersprache wird hauptsächlich für Webentwicklung verwendet?",
                            Answers = new List<string> { "C++", "JavaScript", "Assembly", "COBOL" },
                            CorrectAnswer = 1,
                            Type = QuestionType.MultipleChoice,
                            MaxPoints = 2
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
                            Type = QuestionType.MultipleChoice,
                            MaxPoints = 2
                        },
                        new Question
                        {
                            Text = "Was ist die Quadratwurzel von 16?",
                            Answers = new List<string> { "2", "4", "8", "16" },
                            CorrectAnswer = 1,
                            Type = QuestionType.MultipleChoice,
                            MaxPoints = 2
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
        }
    }
}