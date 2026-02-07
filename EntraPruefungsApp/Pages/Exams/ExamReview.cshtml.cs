using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "Examiner")]
    public class ExamReviewModel : PageModel
    {
        private readonly ExamService _examService;

        public ExamReviewModel(ExamService examService)
        {
            _examService = examService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Exam? Exam { get; set; }
        public List<ExamResult> ExamResults { get; set; } = new();

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

        [BindProperty]
        public List<int> FreeTextScores { get; set; } = new();

        [BindProperty]
        public string? ExaminerFeedback { get; set; }

        public void OnGet()
        {
            Exam = GetExams().FirstOrDefault(e => e.Id == Id);
            
            if (Exam != null)
            {
                var allResults = _examService.GetAllResults();
                ExamResults = allResults.SelectMany(ur => ur.Value)
                                      .Where(r => r.ExamId == Id)
                                      .OrderByDescending(r => r.Date)
                                      .ToList();
            }
        }

        public IActionResult OnPost(string userId, DateTime examDate)
        {
            Exam = GetExams().FirstOrDefault(e => e.Id == Id);
            if (Exam != null)
            {
                // Get the exam result
                var allResults = _examService.GetAllResults();
                if (allResults.ContainsKey(userId))
                {
                    var result = allResults[userId].FirstOrDefault(r => r.ExamId == Id && 
                        Math.Abs((r.Date - examDate).TotalSeconds) < 5);
                    if (result != null)
                    {
                        var allScores = new List<int>();
                        var mcIndex = 0;
                        var ftIndex = 0;
                        
                        for (int i = 0; i < Exam.Questions.Count; i++)
                        {
                            var question = Exam.Questions[i];
                            if (question.Type == QuestionType.MultipleChoice)
                            {
                                var userAnswer = mcIndex < result.Answers.Count ? result.Answers[mcIndex] : -1;
                                var score = userAnswer == question.CorrectAnswer ? question.MaxPoints : 0;
                                allScores.Add(score);
                                mcIndex++;
                            }
                            else if (question.Type == QuestionType.FreeText)
                            {
                                var score = ftIndex < FreeTextScores.Count ? FreeTextScores[ftIndex] : 0;
                                allScores.Add(score);
                                ftIndex++;
                            }
                        }
                        
                        _examService.UpdateGrading(userId, Id, examDate, allScores, ExaminerFeedback);
                        TempData["GradingSuccess"] = true;
                    }
                }
            }
            return RedirectToPage(new { Id });
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