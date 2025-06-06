using System.Collections.Generic;
using Project3.Models;

namespace Project3.Data
{
    public interface IDatabaseService
    {
        bool InitializeDatabase();
        bool CreateUser(User user);
        User? GetUser(string login, string password);
        bool UpdateUserScore(int userId, int score);
        List<User> GetLeaderboard();
        bool SaveGame(Game game);
        Game? GetGame(int gameId);
    }
}
