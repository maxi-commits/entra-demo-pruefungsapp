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

        [BindProperty]
        public List<int> FreeTextScores { get; set; } = new();

        [BindProperty]
        public string? ExaminerFeedback { get; set; }

        public void OnGet()
        {
            Exam = _examService.GetExam(Id);
            
            if (Exam != null)
            {
                ExamResults = _examService.GetAllResults()
                    .SelectMany(ur => ur.Value)
                    .Where(r => r.ExamId == Id)
                    .OrderByDescending(r => r.Date)
                    .ToList();
            }
        }

        public IActionResult OnPost(string userId, DateTime examDate)
        {
            Exam = _examService.GetExam(Id);
            if (Exam != null)
            {
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
                        
                        foreach (var question in Exam.Questions)
                        {
                            if (question.Type == QuestionType.MultipleChoice)
                            {
                                var userAnswer = mcIndex < result.Answers.Count ? result.Answers[mcIndex] : -1;
                                allScores.Add(userAnswer == question.CorrectAnswer ? question.MaxPoints : 0);
                                mcIndex++;
                            }
                            else if (question.Type == QuestionType.FreeText)
                            {
                                allScores.Add(ftIndex < FreeTextScores.Count ? FreeTextScores[ftIndex] : 0);
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
                if (Exam?.Questions[i].Type == QuestionType.MultipleChoice) mcIndex++;
            return mcIndex;
        }

        public int GetFreeTextQuestionIndex(int overallIndex)
        {
            int ftIndex = 0;
            for (int i = 0; i < overallIndex && i < (Exam?.Questions.Count ?? 0); i++)
                if (Exam?.Questions[i].Type == QuestionType.FreeText) ftIndex++;
            return ftIndex;
        }
    }
}
