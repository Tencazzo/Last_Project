using System.Collections.Generic;

namespace Project3.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations;
        private string _currentLanguage = "ru";

        public string CurrentLanguage => _currentLanguage;

        public LocalizationService()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["ru"] = new Dictionary<string, string>
                {
                    ["Authorization"] = "Авторизация",
                    ["Registration"] = "Регистрация",
                    ["Login"] = "Логин",
                    ["Password"] = "Пароль",
                    ["Email"] = "Почта",
                    ["RepeatPassword"] = "Повторите пароль",
                    ["Enter"] = "Войти",
                    ["CreateAccount"] = "Создать аккаунт",
                    ["Leaderboard"] = "Таблица лидеров",
                    ["NewGame"] = "Новая игра",
                    ["LogOut"] = "Выйти из аккаунта",
                    ["Complete"] = "Завершить",
                    ["YouWon"] = "Вы выиграли!",
                    ["YouLost"] = "Вы проиграли!",
                    ["RepeatGame"] = "Повторная игра",
                    ["Rating"] = "Рейтинг",
                    ["UsernamePoints"] = "Имя пользователя - количество очков",
                    ["Example"] = "Пример: Сергей - 1600",
                    ["Player1"] = "Игрок 1",
                    ["Player2"] = "Игрок 2",
                    ["YourTurn"] = "Ваш ход",
                    ["OpponentTurn"] = "Ход соперника",
                    ["ShowPassword"] = "Показать пароль"
                },
                ["en"] = new Dictionary<string, string>
                {
                    ["Authorization"] = "Authorization",
                    ["Registration"] = "Registration",
                    ["Login"] = "Login",
                    ["Password"] = "Password",
                    ["Email"] = "Email",
                    ["RepeatPassword"] = "Repeat password",
                    ["Enter"] = "Login",
                    ["CreateAccount"] = "Create account",
                    ["Leaderboard"] = "Leaderboard",
                    ["NewGame"] = "New game",
                    ["LogOut"] = "Log out",
                    ["Complete"] = "Complete",
                    ["YouWon"] = "You won!",
                    ["YouLost"] = "You lost!",
                    ["RepeatGame"] = "Repeat game",
                    ["Rating"] = "Rating",
                    ["UsernamePoints"] = "Username - number of points",
                    ["Example"] = "Example: Sergey - 1600",
                    ["Player1"] = "Player 1",
                    ["Player2"] = "Player 2",
                    ["YourTurn"] = "Your turn",
                    ["OpponentTurn"] = "Opponent's turn",
                    ["ShowPassword"] = "Show password"
                }
            };
        }

        public string GetString(string key)
        {
            if (_translations.TryGetValue(_currentLanguage, out var languageDict) &&
                languageDict.TryGetValue(key, out var value))
            {
                return value;
            }
            return key; // Возвращаем ключ, если перевод не найден
        }

        public void SetLanguage(string language)
        {
            if (_translations.ContainsKey(language))
            {
                _currentLanguage = language;
            }
        }
    }
}
