using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Services
{
    public class ExamService
    {
        private static readonly Dictionary<string, List<ExamResult>> _results = new();

        public void SaveResult(string userId, int examId, List<int> answers, List<string> freeTextAnswers, int correctAnswers)
        {
            if (!_results.ContainsKey(userId))
            {
                _results[userId] = new List<ExamResult>();
            }

            var result = new ExamResult
            {
                ExamId = examId,
                Answers = answers,
                FreeTextAnswers = freeTextAnswers,
                CorrectAnswers = correctAnswers,
                Date = DateTime.Now,
                UserId = userId
            };

            _results[userId].Add(result);
        }

        public void UpdateGrading(string userId, int examId, DateTime examDate, List<int> allScores, string? feedback)
        {
            if (_results.ContainsKey(userId))
            {
                var result = _results[userId].FirstOrDefault(r => r.ExamId == examId && 
                    Math.Abs((r.Date - examDate).TotalSeconds) < 60);
                if (result != null)
                {
                    result.FreeTextScores = allScores;
                    result.ExaminerFeedback = feedback;
                    result.IsGraded = true;
                }
            }
        }

        public List<ExamResult> GetResults(string userId)
        {
            return _results.ContainsKey(userId) ? _results[userId] : new List<ExamResult>();
        }

        public Dictionary<string, List<ExamResult>> GetAllResults()
        {
            return _results;
        }
    }
}