using System;
using System.Windows.Forms;
using Project3.Services;
using Project3.Infrastructure;
using System.Linq;

namespace Project3.Forms
{
    public partial class ResultForm : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly ILogger _logger;
        private bool? _playerWon;

        public ResultForm(ILocalizationService localizationService, IUserService userService,
            IGameService gameService, ILogger logger)
        {
            _localizationService = localizationService;
            _userService = userService;
            _gameService = gameService;
            _logger = logger;

            InitializeComponent();
            SetupEventHandlers();
            UpdateLocalization();
        }

        private void SetupEventHandlers()
        {
            if (repeatGameButton != null)
                repeatGameButton.Click += RepeatGameButton_Click;
            if (ratingButton != null)
                ratingButton.Click += RatingButton_Click;
        }

        private void UpdateLocalization()
        {
            if (repeatGameButton != null)
                repeatGameButton.Text = _localizationService.GetString("RepeatGame");
            this.Text = _localizationService.GetString("ResultFormTitle");

            if (ratingButton != null)
                ratingButton.Text = _localizationService.GetString("Rating");
        }

        public void SetResult(bool? playerWon)
        {
            _playerWon = playerWon;

            if (resultLabel != null)
            {
                if (playerWon == true)
                {
                    resultLabel.Text = _localizationService.GetString("YouWon");
                    _userService.UpdateScore(10);
                    _logger.LogInfo($"Player {_userService.CurrentUser?.Login} won and gained 10 points");
                }
                else if (playerWon == false)
                {
                    resultLabel.Text = _localizationService.GetString("YouLost");
                    _logger.LogInfo($"Player {_userService.CurrentUser?.Login} lost");
                }
                else
                {
                    resultLabel.Text = "Ничья!";
                    _userService.UpdateScore(5);
                    _logger.LogInfo($"Game ended in draw, player {_userService.CurrentUser?.Login} gained 5 points");
                }
            }
        }

        private void RepeatGameButton_Click(object? sender, EventArgs e)
        {
            _gameService.ResetGame();

            var gameForm = DIContainer.Resolve<GameForm>();
            gameForm.Show();
            this.Close();
        }

        private void RatingButton_Click(object? sender, EventArgs e)
        {
            var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.Show();
            }
            else
            {
                var newMainForm = DIContainer.Resolve<MainForm>();
                newMainForm.Show();
            }

            this.Close();
        }
    }
}
