using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntraPruefungsApp.Services;
using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Areas.Exams.Pages
{
    [Authorize(Roles = "Participant")]
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

        [BindProperty]
        public List<string> FreeTextAnswers { get; set; } = new();

        public Exam? Exam { get; set; }
        public bool Completed { get; set; }
        public int CorrectAnswers { get; set; }

        public void OnGet()
        {
            Exam = _examService.GetExam(Id);
            ExamId = Id;
        }

        public IActionResult OnPost()
        {
            Exam = _examService.GetExam(ExamId);
            
            if (Exam == null)
                return RedirectToPage("/Index", new { area = "Exams" });

            CorrectAnswers = 0;
            var mcQuestionIndex = 0;
            
            foreach (var question in Exam.Questions)
            {
                if (question.Type == QuestionType.MultipleChoice)
                {
                    if (mcQuestionIndex < Answers.Count && Answers[mcQuestionIndex] == question.CorrectAnswer)
                        CorrectAnswers++;
                    mcQuestionIndex++;
                }
            }

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
