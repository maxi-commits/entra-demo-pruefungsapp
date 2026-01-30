namespace EntraPruefungsApp.Services
{
    public class UserService
    {
        private readonly ExamService _examService;
        private static readonly Dictionary<string, List<string>> _trackedUsers = new();

        public UserService(ExamService examService)
        {
            _examService = examService;
        }

        public void TrackUser(string userEmail, List<string> roles)
        {
            // Remove duplicates and ensure we have at least one role
            var uniqueRoles = roles.Distinct().ToList();
            _trackedUsers[userEmail] = uniqueRoles.Any() ? uniqueRoles : new List<string> { "Participant" };
        }

        public Dictionary<string, List<string>> GetAllUsersWithRoles()
        {
            var users = new Dictionary<string, List<string>>();
            
            // Get users from exam results
            var allResults = _examService.GetAllResults();
            foreach (var userId in allResults.Keys)
            {
                if (!users.ContainsKey(userId))
                {
                    users[userId] = new List<string> { "Participant" }; // Default for exam users
                }
            }
            
            // Add tracked users with their actual roles
            foreach (var user in _trackedUsers)
            {
                users[user.Key] = user.Value;
            }
            
            return users;
        }
    }
}
