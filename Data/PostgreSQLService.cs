using System;
using System.Collections.Generic;
using Npgsql;
using Project3.Models;
using Project3.Services;
using System.Security.Cryptography;
using System.Text;

namespace Project3.Data
{
    public class PostgreSQLService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public PostgreSQLService(ILogger logger)
        {
            _logger = logger;
            // Обновленная строка подключения с более стандартными настройками
            _connectionString = "Host=localhost;Port=5432;Database=connectfour;Username=postgres;Password=postgres;";
        }

        public bool InitializeDatabase()
        {
            try
            {
                // Сначала попробуем подключиться к базе postgres для создания нашей базы
                var masterConnectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres;";

                using (var masterConnection = new NpgsqlConnection(masterConnectionString))
                {
                    masterConnection.Open();

                    // Проверяем, существует ли база данных
                    var checkDbQuery = "SELECT 1 FROM pg_database WHERE datname = 'connectfour'";
                    using var checkCmd = new NpgsqlCommand(checkDbQuery, masterConnection);
                    var dbExists = checkCmd.ExecuteScalar();

                    if (dbExists == null)
                    {
                        // Создаем базу данных
                        var createDbQuery = "CREATE DATABASE connectfour";
                        using var createCmd = new NpgsqlCommand(createDbQuery, masterConnection);
                        createCmd.ExecuteNonQuery();
                        _logger.LogInfo("Database 'connectfour' created successfully");
                    }
                }

                // Теперь подключаемся к нашей базе и создаем таблицы
                using var appConnection = new NpgsqlConnection(_connectionString);
                appConnection.Open();

                // Создание таблицы пользователей
                var createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id SERIAL PRIMARY KEY,
                        login VARCHAR(50) UNIQUE NOT NULL,
                        email VARCHAR(100) NOT NULL,
                        password_hash VARCHAR(255) NOT NULL,
                        score INTEGER DEFAULT 0,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";

                // Создание таблицы игр
                var createGamesTable = @"
                    CREATE TABLE IF NOT EXISTS games (
                        id SERIAL PRIMARY KEY,
                        player1_id INTEGER REFERENCES users(id),
                        player2_id INTEGER REFERENCES users(id),
                        winner_id INTEGER REFERENCES users(id),
                        game_state TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        finished_at TIMESTAMP
                    )";

                using var cmd1 = new NpgsqlCommand(createUsersTable, appConnection);
                cmd1.ExecuteNonQuery();

                using var cmd2 = new NpgsqlCommand(createGamesTable, appConnection);
                cmd2.ExecuteNonQuery();

                _logger.LogInfo("Database initialized successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to initialize database", ex);
                return false;
            }
        }

        public bool CreateUser(User user)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = "INSERT INTO users (login, email, password_hash) VALUES (@login, @email, @password)";
                using var cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.AddWithValue("login", user.Login);
                cmd.Parameters.AddWithValue("email", user.Email);
                cmd.Parameters.AddWithValue("password", HashPassword(user.Password));

                cmd.ExecuteNonQuery();
                _logger.LogInfo($"User {user.Login} created successfully");
                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Unique violation
            {
                _logger.LogWarning($"User {user.Login} already exists");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create user {user.Login}", ex);
                return false;
            }
        }

        public User? GetUser(string login, string password)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = "SELECT id, login, email, score FROM users WHERE login = @login AND password_hash = @password";
                using var cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", HashPassword(password));

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Login = reader["login"]?.ToString() ?? string.Empty,
                        Email = reader["email"]?.ToString() ?? string.Empty,
                        Score = Convert.ToInt32(reader["score"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user {login}", ex);
                return null;
            }
        }

        public bool UpdateUserScore(int userId, int score)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = "UPDATE users SET score = score + @score WHERE id = @userId";
                using var cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("score", score);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update user score for user {userId}", ex);
                return false;
            }
        }

        public List<User> GetLeaderboard()
        {
            var users = new List<User>();
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = "SELECT id, login, email, score FROM users ORDER BY score DESC LIMIT 10";
                using var cmd = new NpgsqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Login = reader["login"]?.ToString() ?? string.Empty,
                        Email = reader["email"]?.ToString() ?? string.Empty,
                        Score = Convert.ToInt32(reader["score"])
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get leaderboard", ex);
            }
            return users;
        }

        public bool SaveGame(Game game)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = @"INSERT INTO games (player1_id, player2_id, winner_id, game_state, finished_at) 
                             VALUES (@player1, @player2, @winner, @state, @finished)";
                using var cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.AddWithValue("player1", game.Player1Id);
                cmd.Parameters.AddWithValue("player2", game.Player2Id);
                cmd.Parameters.AddWithValue("winner", (object?)game.WinnerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("state", game.GameState);
                cmd.Parameters.AddWithValue("finished", (object?)game.FinishedAt ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save game", ex);
                return false;
            }
        }

        public Game? GetGame(int gameId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                var query = "SELECT * FROM games WHERE id = @gameId";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("gameId", gameId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Game
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Player1Id = Convert.ToInt32(reader["player1_id"]),
                        Player2Id = Convert.ToInt32(reader["player2_id"]),
                        WinnerId = reader.IsDBNull(reader.GetOrdinal("winner_id")) ? null : Convert.ToInt32(reader["winner_id"]),
                        GameState = reader["game_state"]?.ToString() ?? string.Empty,
                        CreatedAt = Convert.ToDateTime(reader["created_at"]),
                        FinishedAt = reader.IsDBNull(reader.GetOrdinal("finished_at")) ? null : Convert.ToDateTime(reader["finished_at"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get game {gameId}", ex);
                return null;
            }
        }

        private static string HashPassword(string password)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
