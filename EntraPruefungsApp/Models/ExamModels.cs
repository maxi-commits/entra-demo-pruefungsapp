namespace EntraPruefungsApp.Models
{
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
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        public string? OptimalAnswer { get; set; }
        public int MaxPoints { get; set; } = 1;
    }

    public enum QuestionType
    {
        MultipleChoice,
        FreeText
    }

    public class ExamResult
    {
        public int ExamId { get; set; }
        public List<int> Answers { get; set; } = new();
        public List<string> FreeTextAnswers { get; set; } = new();
        public int CorrectAnswers { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool IsGraded { get; set; } = false;
        public List<int> FreeTextScores { get; set; } = new();
        public string? ExaminerFeedback { get; set; }
    }
}