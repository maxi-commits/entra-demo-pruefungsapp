using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Features.Exams
{
    [Authorize(Roles = "Participant")]
    public class TakeModel : PageModel
    {
        private readonly ExamService _examService;

        public TakeModel(ExamService examService)
        {
            _examService = examService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int ExamId { get; set; }

        [BindProperty]
        public List<int> Answers { get; set; } = new();

        [BindProperty]
        public List<string> FreeTextAnswers { get; set; } = new();

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
                            Text = "Was ist die Wurzel aus 16?",
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
                return Redirect("/exams");
            }

            // Evaluate answers
            CorrectAnswers = 0;
            var mcQuestionIndex = 0;
            var freeTextIndex = 0;
            
            for (int i = 0; i < Exam.Questions.Count; i++)
            {
                var question = Exam.Questions[i];
                if (question.Type == QuestionType.MultipleChoice)
                {
                    if (mcQuestionIndex < Answers.Count && Answers[mcQuestionIndex] == question.CorrectAnswer)
                    {
                        CorrectAnswers++;
                    }
                    mcQuestionIndex++;
                }
                else if (question.Type == QuestionType.FreeText)
                {
                    freeTextIndex++;
                }
            }

            // Save result
            var userId = User.Identity?.Name ?? "anonymous";
            _examService.SaveResult(userId, ExamId, Answers, FreeTextAnswers, CorrectAnswers);

            Completed = true;
            return Page();
        }

        public int GetMCQuestionIndex(int overallIndex)
        {
            int mcIndex = 0;
            for (int i = 0; i < overallIndex && i < (Exam?.Questions.Count ?? 0); i++)
            {
                if (Exam?.Questions[i].Type == QuestionType.MultipleChoice)
                    mcIndex++;
            }
            return mcIndex;
        }

        public int GetFreeTextQuestionIndex(int overallIndex)
        {
            int ftIndex = 0;
            for (int i = 0; i < overallIndex && i < (Exam?.Questions.Count ?? 0); i++)
            {
                if (Exam?.Questions[i].Type == QuestionType.FreeText)
                    ftIndex++;
            }
            return ftIndex;
        }
    }
}