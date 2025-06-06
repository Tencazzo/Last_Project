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
                    ["Player1"] = "Игрок 1",
                    ["Player2"] = "Игрок 2",
                    ["YourTurn"] = "Ваш ход",
                    ["OpponentTurn"] = "Ход соперника",
                    ["GameFormTitle"] = "Игра",
                    ["LoginFormTitle"] = "Авторизация",
                    ["MainFormTitle"] = "Рейтинг и запуск игры",
                    ["RegisterFormTitle"] = "Регистрация",
                    ["ResultFormTitle"] = "Результат игры",
                    ["WaitingForPlayer"] = "Ожидание второго игрока...",
                    ["WaitingForGuest"] = "Ожидание гостя",
                    ["Host"] = "Хост",
                    ["Guest"] = "Гость"
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
                    ["Player1"] = "Player 1",
                    ["Player2"] = "Player 2",
                    ["YourTurn"] = "Your turn",
                    ["OpponentTurn"] = "Opponent's turn",
                    ["GameFormTitle"] = "Game",
                    ["LoginFormTitle"] = "Authorization",
                    ["MainFormTitle"] = "Rating and game start",
                    ["RegisterFormTitle"] = "Registration",
                    ["ResultFormTitle"] = "Game result",
                    ["WaitingForPlayer"] = "Waiting for player...",
                    ["WaitingForGuest"] = "Waiting for guest",
                    ["Guest"] = "Guest"
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
            return key; 
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
