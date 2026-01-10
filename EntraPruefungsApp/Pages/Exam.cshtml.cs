using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;

namespace EntraPruefungsApp.Pages
{
    [Authorize(Roles = "User")]
    public class ExamModel : PageModel
    {
        private readonly ExamService _examService;

        public ExamModel(ExamService examService)
        {
            _examService = examService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int ExamId { get; set; }

        [BindProperty]
        public List<int> Answers { get; set; } = new();

        public Exam? Exam { get; set; }
        public bool Completed { get; set; }
        public int CorrectAnswers { get; set; }

        private static List<Exam> GetExams()
        {
            return new List<Exam>
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
                            CorrectAnswer = 0
                        },
                        new Question
                        {
                            Text = "Welche Programmiersprache wird hauptsächlich für Webentwicklung verwendet?",
                            Answers = new List<string> { "C++", "JavaScript", "Assembly", "COBOL" },
                            CorrectAnswer = 1
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
                            CorrectAnswer = 1
                        },
                        new Question
                        {
                            Text = "Was ist die Wurzel aus 16?",
                            Answers = new List<string> { "2", "4", "8", "16" },
                            CorrectAnswer = 1
                        }
                    }
                }
            };
        }

        public void OnGet()
        {
            Exam = GetExams().FirstOrDefault(p => p.Id == Id);
            ExamId = Id;
        }

        public IActionResult OnPost()
        {
            Exam = GetExams().FirstOrDefault(p => p.Id == ExamId);
            
            if (Exam == null)
            {
                return RedirectToPage("/Exams");
            }

            // Evaluate answers
            CorrectAnswers = 0;
            for (int i = 0; i < Exam.Questions.Count && i < Answers.Count; i++)
            {
                if (Answers[i] == Exam.Questions[i].CorrectAnswer)
                {
                    CorrectAnswers++;
                }
            }

            // Save result
            var userId = User.Identity?.Name ?? "anonymous";
            _examService.SaveResult(userId, ExamId, Answers, CorrectAnswers);

            Completed = true;
            return Page();
        }
    }
}