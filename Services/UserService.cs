using Project3.Models;
using Project3.Data;

namespace Project3.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;
        private User? _currentUser;

        public User? CurrentUser => _currentUser;

        public UserService(IDatabaseService databaseService, ILogger logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public bool Login(string login, string password)
        {
            var user = _databaseService.GetUser(login, password);
            if (user != null)
            {
                _currentUser = user;
                _logger.LogInfo($"User {login} logged in successfully");
                return true;
            }
            _logger.LogWarning($"Failed login attempt for user {login}");
            return false;
        }

        public bool Register(User user)
        {
            var result = _databaseService.CreateUser(user);
            if (result)
            {
                _logger.LogInfo($"User {user.Login} registered successfully");
            }
            return result;
        }

        public void Logout()
        {
            if (_currentUser != null)
            {
                _logger.LogInfo($"User {_currentUser.Login} logged out");
                _currentUser = null;
            }
        }

        public bool UpdateScore(int score)
        {
            if (_currentUser != null)
            {
                var result = _databaseService.UpdateUserScore(_currentUser.Id, score);
                if (result)
                {
                    _currentUser.Score += score;
                }
                return result;
            }
            return false;
        }
    }
}
