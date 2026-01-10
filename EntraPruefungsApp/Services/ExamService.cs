namespace EntraPruefungsApp.Services
{
    public class ExamService
    {
        private static readonly Dictionary<string, List<ExamResult>> _results = new();

        public void SaveResult(string userId, int examId, List<int> answers, int correctAnswers)
        {
            if (!_results.ContainsKey(userId))
            {
                _results[userId] = new List<ExamResult>();
            }

            var result = new ExamResult
            {
                ExamId = examId,
                Answers = answers,
                CorrectAnswers = correctAnswers,
                Date = DateTime.Now
            };

            _results[userId].Add(result);
        }

        public List<ExamResult> GetResults(string userId)
        {
            return _results.ContainsKey(userId) ? _results[userId] : new List<ExamResult>();
        }
    }

    public class ExamResult
    {
        public int ExamId { get; set; }
        public List<int> Answers { get; set; } = new();
        public int CorrectAnswers { get; set; }
        public DateTime Date { get; set; }
    }
}