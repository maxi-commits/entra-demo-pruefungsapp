using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EntraPruefungsApp.Pages
{
    [Authorize(Roles = "User")]
    public class ExamsModel : PageModel
    {
        public List<Exam> Exams { get; set; } = new();

        public void OnGet()
        {
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
                            Text = "Was ist die Quadratwurzel von 16?",
                            Answers = new List<string> { "2", "4", "8", "16" },
                            CorrectAnswer = 1
                        }
                    }
                }
            };
        }
    }

    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
    }

    public class Question
    {
        public string Text { get; set; } = string.Empty;
        public List<string> Answers { get; set; } = new();
        public int CorrectAnswer { get; set; }
    }
}