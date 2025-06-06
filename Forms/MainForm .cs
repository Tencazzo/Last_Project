using System;
using System.Linq;
using System.Windows.Forms;
using Project3.Services;
using Project3.Infrastructure;
using Project3.Data;

namespace Project3.Forms
{
    public partial class MainForm : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly IUserService _userService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;

        public MainForm(ILocalizationService localizationService, IUserService userService,
            IDatabaseService databaseService, ILogger logger)
        {
            _localizationService = localizationService;
            _userService = userService;
            _databaseService = databaseService;
            _logger = logger;

            InitializeComponent();
            InitializeLanguageComboBox();
            SetupEventHandlers();
            UpdateLocalization();
            LoadLeaderboard();
            if (leaderboardListBox != null)
            {
                leaderboardListBox.DrawMode = DrawMode.OwnerDrawVariable;
                leaderboardListBox.DrawItem += LeaderboardListBox_DrawItem;
                leaderboardListBox.MeasureItem += LeaderboardListBox_MeasureItem;
            }
        }

        private void InitializeLanguageComboBox()
        {
            languageComboBox?.Items.Add("Русский");
            languageComboBox?.Items.Add("English");
            if (languageComboBox != null)
                languageComboBox.SelectedIndex = _localizationService.CurrentLanguage == "ru" ? 0 : 1;
        }

        private void SetupEventHandlers()
        {
            if (languageComboBox != null)
                languageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
            if (newGameButton != null)
                newGameButton.Click += NewGameButton_Click;
            if (logoutButton != null)
                logoutButton.Click += LogoutButton_Click;
        }

        private void LanguageComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (languageComboBox != null)
            {
                string language = languageComboBox.SelectedIndex == 0 ? "ru" : "en";
                _localizationService.SetLanguage(language);
                UpdateLocalization();
            }
        }

        private void UpdateLocalization()
        {
            if (leaderboardTitleLabel != null)
                leaderboardTitleLabel.Text = _localizationService.GetString("Leaderboard");
            if (leaderboardHeaderLabel != null)
                leaderboardHeaderLabel.Text = _localizationService.GetString("UsernamePoints");
            if (newGameButton != null)
                newGameButton.Text = _localizationService.GetString("NewGame");
            if (logoutButton != null)
                logoutButton.Text = _localizationService.GetString("LogOut");
        }

        private void LoadLeaderboard()
        {
            try
            {
                var users = _databaseService.GetLeaderboard();
                leaderboardListBox?.Items.Clear();

                foreach (var user in users.Take(10))
                {
                    leaderboardListBox?.Items.Add($"{user.Login} - {user.Score}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to load leaderboard", ex);
            }
        }

        private void NewGameButton_Click(object? sender, EventArgs e)
        {
            _logger.LogInfo($"User {_userService.CurrentUser?.Login} started new game");

            var gameForm = DIContainer.Resolve<GameForm>();
            gameForm.Show();
            this.Hide(); // Скрываем главную форму, но не закрываем
        }

        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            _userService.Logout();
            _logger.LogInfo("User logged out");

            // Find the existing login form or create a new one
            var loginForm = Application.OpenForms.OfType<LoginForm>().FirstOrDefault();
            if (loginForm == null)
            {
                loginForm = DIContainer.Resolve<LoginForm>();
            }

            loginForm.Show();
            this.Hide(); // Change from Close() to Hide()
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Завершаем приложение при закрытии главной формы
            Application.Exit();
        }
        private void LeaderboardListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (leaderboardListBox == null || e.Index < 0) return;

            e.DrawBackground();

            // Настройки для центрирования текста
            StringFormat sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            // Определение цвета текста (чтобы выделялся на белом фоне)
            Brush textBrush = new SolidBrush(Color.Black);

            // Отрисовка текста по центру
            e.Graphics.DrawString(leaderboardListBox.Items[e.Index].ToString(),
                                 e.Font,
                                 textBrush,
                                 e.Bounds,
                                 sf);

            e.DrawFocusRectangle();
            textBrush.Dispose();
        }

        private void LeaderboardListBox_MeasureItem(object? sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 35; // Высота каждой строки в списке
        }
    }
}