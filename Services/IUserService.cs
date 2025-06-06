using Project3.Models;

namespace Project3.Services
{
    public interface IUserService
    {
        User? CurrentUser { get; }
        bool Login(string login, string password);
        bool Register(User user);
        void Logout();
        bool UpdateScore(int score);
    }
}
