using EntraPruefungsApp.Models;

namespace EntraPruefungsApp.Services
{
    public class ExamService
    {
        private static readonly Dictionary<string, List<ExamResult>> _results = new();

        private static readonly List<Exam> _exams = new()
        {
            new Exam
            {
                Id = 1,
                Title = "IAM & EntraID",
                Description = "Aufgaben zu IAM & Entra",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Was beschreibt Authentifizierung am besten?",
                        Answers = new List<string> { "Festlegung, welche Aktionen ein Benutzer ausführen darf", "Verifizierung der Identität eines Benutzers", "Verschlüsselung von Daten während der Übertragung" },
                        CorrectAnswer = 1,
                        Type = QuestionType.MultipleChoice,
                        MaxPoints = 2
                    },
                    new Question
                    {
                        Text = "Worauf basiert ABAC (Attribute-Based Access Control)?",
                        Answers = new List<string> { "Auf Benutzerrollen", "Auf Attributen wie Gerät, Standort oder Uhrzeit", "Auf Passwörtern" },
                        CorrectAnswer = 1,
                        Type = QuestionType.MultipleChoice,
                        MaxPoints = 2
                    },
                    new Question
                    {
                        Text = "Was ist Multi-Faktor-Authentifizierung (MFA)?",
                        Type = QuestionType.FreeText,
                        OptimalAnswer = "MFA kombiniert mehrere Faktoren (z.B. Wissen, Besitz, Biometrie) zur Anmeldung.",
                        MaxPoints = 3
                    }
                }
            },
            new Exam
            {
                Id = 2,
                Title = "Grundlagen der Informatik",
                Description = "Teste dein Wissen über IT-Konzepte",
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
            }
        };

        public List<Exam> GetExams() => _exams;

        public Exam? GetExam(int id) => _exams.FirstOrDefault(e => e.Id == id);

        public void SaveResult(string userId, int examId, List<int> answers, List<string> freeTextAnswers, int correctAnswers)
        {
            if (!_results.ContainsKey(userId))
                _results[userId] = new List<ExamResult>();

            _results[userId].Add(new ExamResult
            {
                ExamId = examId,
                Answers = answers,
                FreeTextAnswers = freeTextAnswers,
                CorrectAnswers = correctAnswers,
                Date = DateTime.Now,
                UserId = userId
            });
        }

        public void UpdateGrading(string userId, int examId, DateTime examDate, List<int> allScores, string? feedback)
        {
            if (_results.ContainsKey(userId))
            {
                var result = _results[userId].FirstOrDefault(r => r.ExamId == examId && 
                    Math.Abs((r.Date - examDate).TotalSeconds) < 5);
                if (result != null)
                {
                    result.FreeTextScores = new List<int>(allScores);
                    result.ExaminerFeedback = feedback;
                    result.IsGraded = true;
                }
            }
        }

        public List<ExamResult> GetResults(string userId) =>
            _results.ContainsKey(userId) ? _results[userId] : new List<ExamResult>();

        public Dictionary<string, List<ExamResult>> GetAllResults() => _results;
    }
}
