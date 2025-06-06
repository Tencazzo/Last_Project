using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Project3.Services;
using Project3.Infrastructure;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Project3.Forms
{
    public partial class GameForm : Form
    {
        private readonly IGameService _gameService;
        private readonly INetworkService _networkService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private volatile bool _isMyTurn = false;
        private volatile bool _gameStarted = false;
        private volatile bool _connectionAttemptMade = false;
        private readonly object _lockObject = new object();
        private int _myPlayerNumber; 

        public GameForm(IGameService gameService, INetworkService networkService,
            ILocalizationService localizationService, ILogger logger, IUserService userService)
        {
            _gameService = gameService;
            _networkService = networkService;
            _localizationService = localizationService;
            _logger = logger;
            _userService = userService;

            InitializeComponent();

            if (waitingLabel != null && gamePanel != null)
            {
                waitingLabel.Parent = gamePanel;
                waitingLabel.BringToFront();
                waitingLabel.Visible = true;
            }

            SetupEventHandlers();
            UpdateLocalization();
            MakeButtonsRound();
            UpdatePlayerLabels();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _logger.LogInfo("GameForm loaded, starting network connection");

            _gameService.ResetGame();
            ClearGameBoard();

            Task.Run(TryNetworkConnection);
        }

        private void SetupEventHandlers()
        {
            try
            {
                if (completeButton != null)
                    completeButton.Click += CompleteButton_Click;

                _networkService.MoveReceived += OnMoveReceived;
                _networkService.PlayerConnected += OnPlayerConnected;
                _networkService.PlayerDisconnected += OnPlayerDisconnected;
                _networkService.GameEnded += OnGameEnded; 

                _logger.LogInfo("Event handlers set up successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting up event handlers: {ex.Message}", ex);
            }
        }

        private void UpdateLocalization()
        {
            try
            {
                if (completeButton != null)
                    completeButton.Text = _localizationService.GetString("Complete");

                if (player1Label != null)
                    player1Label.Text = _localizationService.GetString("Player1") + ":";

                if (player2Label != null)
                    player2Label.Text = _localizationService.GetString("Player2") + ":";

                if (waitingLabel != null)
                    waitingLabel.Text = _localizationService.GetString("WaitingForPlayer");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating localization: {ex.Message}", ex);
            }
        }

        private void UpdatePlayerLabels()
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(new Action(UpdatePlayerLabelsInternal));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                UpdatePlayerLabelsInternal();
            }
        }

        private void UpdatePlayerLabelsInternal()
        {
            try
            {
                if (_userService.CurrentUser != null)
                {
                    if (_networkService.IsServer)
                    {
                        if (player1Label != null)
                            player1Label.Text = _userService.CurrentUser.Login + " (Хост):";
                        if (player2Label != null && _gameStarted)
                            player2Label.Text = "Гость:";
                        else if (player2Label != null)
                            player2Label.Text = "Ожидание гостя...";
                    }
                    else if (_networkService.IsConnected)
                    {
                        if (player2Label != null)
                            player2Label.Text = _userService.CurrentUser.Login + " (Гость):";
                        if (player1Label != null)
                            player1Label.Text = "Хост:";
                    }
                    else
                    {
                        if (player1Label != null)
                            player1Label.Text = _userService.CurrentUser.Login + ":";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating player labels: {ex.Message}", ex);
            }
        }

        private void UpdateTurnLabel()
        {
            if (turnLabel == null) return;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(new Action(UpdateTurnLabelInternal));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                UpdateTurnLabelInternal();
            }
        }

        private void UpdateTurnLabelInternal()
        {
            try
            {
                if (!_gameStarted)
                {
                    if (_networkService.IsServer)
                    {
                        turnLabel.Text = "Ожидание второго игрока...";
                    }
                    else if (_networkService.IsConnected)
                    {
                        turnLabel.Text = "Подключен к игре, ожидание начала...";
                    }
                    else
                    {
                        turnLabel.Text = "Подключение к серверу...";
                    }
                    turnLabel.ForeColor = Color.White;
                    return;
                }

                _logger.LogInfo($"Updating turn label. IsMyTurn: {_isMyTurn}, MyPlayerNumber: {_myPlayerNumber}, CurrentPlayer: {_gameService.CurrentPlayer}");

                bool isMyTurnNow = (_gameService.CurrentPlayer == _myPlayerNumber);

                if (isMyTurnNow)
                {
                    turnLabel.Text = "Ваш ход";
                    turnLabel.ForeColor = _networkService.IsServer ? Color.Orange : Color.Yellow;
                }
                else
                {
                    turnLabel.Text = "Ход соперника";
                    turnLabel.ForeColor = _networkService.IsServer ? Color.Yellow : Color.Orange;
                }

                _isMyTurn = isMyTurnNow;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating turn label: {ex.Message}", ex);
            }
        }

        private void EnableGameButtons(bool enabled)
        {
            if (gameButtons == null) return;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(new Action(() => EnableGameButtonsInternal(enabled)));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                EnableGameButtonsInternal(enabled);
            }
        }

        private void EnableGameButtonsInternal(bool enabled)
        {
            try
            {
                _logger.LogInfo($"Setting game buttons enabled: {enabled}");
                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        if (gameButtons[row, col] != null)
                        {
                            gameButtons[row, col].Enabled = enabled;
                        }
                    }
                }
                _logger.LogInfo($"Game buttons enabled set to: {enabled}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enabling game buttons: {ex.Message}", ex);
            }
        }

        private void ClearGameBoard()
        {
            if (gameButtons == null) return;

            try
            {
                _logger.LogInfo("Clearing game board");
                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        if (gameButtons[row, col] != null)
                        {
                            gameButtons[row, col].BackColor = Color.White;
                        }
                    }
                }
                _logger.LogInfo("Game board cleared");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing game board: {ex.Message}", ex);
            }
        }

        private void StartGame()
        {
            _logger.LogInfo($"StartGame called. Current _gameStarted: {_gameStarted}");

            lock (_lockObject)
            {
                if (_gameStarted)
                {
                    _logger.LogInfo("Game already started, ignoring duplicate start request");
                    return;
                }

                _logger.LogInfo("Setting _gameStarted to true");
                _gameStarted = true;
            }

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(new Action(StartGameInternal));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                StartGameInternal();
            }
        }

        private void StartGameInternal()
        {
            try
            {
                _logger.LogInfo($"StartGameInternal called. IsServer: {_networkService.IsServer}");

                _myPlayerNumber = _networkService.IsServer ? 1 : 2;
                _logger.LogInfo($"My player number: {_myPlayerNumber}");

                _gameService.ResetGame();
                ClearGameBoard();

                if (waitingLabel != null)
                {
                    waitingLabel.Visible = false;
                    _logger.LogInfo("WaitingLabel hidden");
                }

                EnableGameButtonsInternal(true);

                _isMyTurn = (_myPlayerNumber == 1);

                _logger.LogInfo($"Game started. IsServer: {_networkService.IsServer}, MyPlayerNumber: {_myPlayerNumber}, IsMyTurn: {_isMyTurn}, _gameStarted: {_gameStarted}");

                UpdateTurnLabelInternal();
                UpdatePlayerLabelsInternal();

                this.Refresh();

                _logger.LogInfo("Network game started - both players connected");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error starting game: {ex.Message}", ex);
            }
        }

        private void MakeButtonsRound()
        {
            try
            {
                if (gameButtons == null) return;

                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        var button = gameButtons[row, col];
                        if (button != null)
                        {
                            using GraphicsPath path = new();
                            path.AddEllipse(0, 0, button.Width, button.Height);
                            button.Region = new Region(path);
                        }
                    }
                }

                if (player1ColorPanel != null)
                {
                    using GraphicsPath path1 = new();
                    path1.AddEllipse(0, 0, player1ColorPanel.Width, player1ColorPanel.Height);
                    player1ColorPanel.Region = new Region(path1);
                }

                if (player2ColorPanel != null)
                {
                    using GraphicsPath path2 = new();
                    path2.AddEllipse(0, 0, player2ColorPanel.Width, player2ColorPanel.Height);
                    player2ColorPanel.Region = new Region(path2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error making buttons round: {ex.Message}", ex);
            }
        }

        private async Task TryNetworkConnection()
        {
            lock (_lockObject)
            {
                if (_connectionAttemptMade)
                {
                    _logger.LogInfo("Connection attempt already made, skipping");
                    return;
                }
                _connectionAttemptMade = true;
            }

            try
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (waitingLabel != null)
                        {
                            waitingLabel.Visible = true;
                            waitingLabel.Text = "Инициализация сетевой игры...";
                        }
                        EnableGameButtonsInternal(false);
                    }));
                }

                await Task.Delay(1000);

                DialogResult result = DialogResult.Cancel;
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() =>
                    {
                        result = MessageBox.Show(
                            "Выберите роль в сетевой игре:\n\n" +
                            "ДА - Создать игру (стать хостом)\n" +
                            "НЕТ - Подключиться к существующей игре",
                            "Сетевая игра",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                    }));

                    if (result == DialogResult.Cancel)
                    {
                        ReturnToMainMenu();
                        return;
                    }

                    if (result == DialogResult.Yes)
                    {
                        await StartAsServer();
                    }
                    else
                    {
                        await ConnectAsClient();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TryNetworkConnection: {ex.Message}", ex);
                ReturnToMainMenu();
            }
        }

        private async Task StartAsServer()
        {
            try
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (waitingLabel != null)
                        {
                            waitingLabel.Text = "Создание игры...";
                        }
                    }));
                }

                await Task.Delay(500);

                bool serverStarted = _networkService.StartServer(8888);

                if (serverStarted)
                {
                    _logger.LogInfo("Started as server, waiting for connection...");

                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (waitingLabel != null)
                            {
                                waitingLabel.Text = "Ожидание второго игрока...";
                            }
                            UpdateTurnLabelInternal();
                            UpdatePlayerLabelsInternal();
                        }));
                    }
                }
                else
                {
                    _logger.LogError("Failed to start server");
                    ShowErrorAndReturnToMenu("Не удалось создать игру. Возможно, порт уже занят.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error starting server: {ex.Message}", ex);
                ShowErrorAndReturnToMenu("Ошибка при создании игры.");
            }
        }

        private async Task ConnectAsClient()
        {
            try
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (waitingLabel != null)
                        {
                            waitingLabel.Text = "Подключение к игре...";
                        }
                    }));
                }

                await Task.Delay(500);

                bool connected = _networkService.ConnectToServer("127.0.0.1", 8888);

                if (connected)
                {
                    _logger.LogInfo("Connected as client");

                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdatePlayerLabelsInternal();

                            if (waitingLabel != null)
                            {
                                waitingLabel.Text = "Подключен! Ожидание начала игры...";
                            }

                            UpdateTurnLabelInternal();
                        }));
                    }
                }
                else
                {
                    _logger.LogError("Failed to connect to server");
                    ShowErrorAndReturnToMenu("Не удалось подключиться к игре. Убедитесь, что хост создал игру.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error connecting as client: {ex.Message}", ex);
                ShowErrorAndReturnToMenu("Ошибка при подключении к игре.");
            }
        }

        private void ShowErrorAndReturnToMenu(string message)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "Ошибка сети", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReturnToMainMenu();
                }));
            }
        }

        private void ReturnToMainMenu()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Error returning to main menu: {ex.Message}", ex);
            }
        }

        private void OnGameButtonClick(object? sender, EventArgs e)
        {
            try
            {
                _logger.LogInfo($"Game button clicked. GameOver: {_gameService.IsGameOver}, GameStarted: {_gameStarted}, IsConnected: {_networkService.IsConnected}, IsMyTurn: {_isMyTurn}, CurrentPlayer: {_gameService.CurrentPlayer}, MyPlayerNumber: {_myPlayerNumber}");

                bool gameReady;
                lock (_lockObject)
                {
                    gameReady = _gameStarted && !_gameService.IsGameOver;
                }

                if (!gameReady)
                {
                    _logger.LogInfo($"Game button clicked but game not ready. _gameStarted: {_gameStarted}, IsGameOver: {_gameService.IsGameOver}");
                    return;
                }

                if (!_networkService.IsConnected)
                {
                    _logger.LogWarning("Game button clicked but not connected");
                    MessageBox.Show("Нет сетевого подключения!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_gameService.CurrentPlayer != _myPlayerNumber)
                {
                    _logger.LogInfo($"Game button clicked but not player's turn. CurrentPlayer: {_gameService.CurrentPlayer}, MyPlayerNumber: {_myPlayerNumber}");
                    MessageBox.Show("Ждите своего хода!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (sender is Button button && button.Tag is int column)
                {
                    _logger.LogInfo($"Player {_myPlayerNumber} clicked column {column}");

                    if (_gameService.MakeMove(column, false))
                    {
                        _logger.LogInfo($"Move made successfully in column {column}");
                        UpdateGameBoard();
                        _networkService.SendMove(column);
                        UpdateTurnLabel();
                        CheckGameEnd();
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to make move in column {column}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnGameButtonClick: {ex.Message}", ex);
            }
        }

        private void OnMoveReceived(int column)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;

            try
            {
                this.Invoke(new Action(() =>
                {
                    _logger.LogInfo($"Move received: {column}");

                    if (_gameService.MakeMove(column, false))
                    {
                        _logger.LogInfo($"Opponent move processed successfully in column {column}");
                        UpdateGameBoardInternal();
                        UpdateTurnLabelInternal();
                        CheckGameEnd();
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to process opponent move in column {column}");
                    }
                }));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnMoveReceived: {ex.Message}", ex);
            }
        }

        private void OnGameEnded(int winner)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;

            try
            {
                this.Invoke(new Action(() =>
                {
                    _logger.LogInfo($"Game ended received: winner {winner}");

                    var resultForm = DIContainer.Resolve<ResultForm>();
                    bool playerWon = (winner == _myPlayerNumber);
                    resultForm.SetResult(playerWon);
                    resultForm.Show();
                    this.Close();
                }));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnGameEnded: {ex.Message}", ex);
            }
        }

        private void OnPlayerConnected()
        {
            _logger.LogInfo($"OnPlayerConnected event fired. IsServer: {_networkService.IsServer}, GameStarted: {_gameStarted}");

            if (!this.IsHandleCreated || this.IsDisposed) return;

            try
            {
                this.Invoke(new Action(() =>
                {
                    bool shouldStart;
                    lock (_lockObject)
                    {
                        shouldStart = !_gameStarted;
                        if (shouldStart)
                        {
                            _logger.LogInfo("Player connected, will start multiplayer game");
                        }
                        else
                        {
                            _logger.LogInfo("Game already started, ignoring duplicate PlayerConnected event");
                            return;
                        }
                    }

                    if (shouldStart)
                    {
                        StartGame();

                        Task.Run(() =>
                        {
                            Thread.Sleep(100); 

                            if (this.IsHandleCreated && !this.IsDisposed)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    if (_networkService.IsServer)
                                    {
                                        MessageBox.Show("Игрок подключился! Игра началась.", "Подключение",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Подключение установлено! Игра началась.", "Подключение",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }));
                            }
                        });
                    }
                }));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnPlayerConnected: {ex.Message}", ex);
            }
        }

        private void OnPlayerDisconnected()
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;

            try
            {
                this.Invoke(new Action(() =>
                {
                    lock (_lockObject)
                    {
                        _gameStarted = false;
                    }

                    _logger.LogInfo("Player disconnected");
                    MessageBox.Show("Игрок отключился. Возвращение в главное меню.", "Отключение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    EnableGameButtonsInternal(false);
                    ReturnToMainMenu();
                }));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnPlayerDisconnected: {ex.Message}", ex);
            }
        }

        private void UpdateGameBoard()
        {
            if (gameButtons == null) return;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(new Action(UpdateGameBoardInternal));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                UpdateGameBoardInternal();
            }
        }

        private void UpdateGameBoardInternal()
        {
            try
            {
                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        int cellValue = _gameService.GetCellValue(row, col);
                        var button = gameButtons[row, col];

                        if (button != null)
                        {
                            button.BackColor = cellValue switch
                            {
                                1 => Color.Orange,  
                                2 => Color.Yellow,  
                                _ => Color.White    
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating game board: {ex.Message}", ex);
            }
        }

        private void CheckGameEnd()
        {
            try
            {
                if (_gameService.IsGameOver)
                {
                    _logger.LogInfo($"Game ended. Winner: {_gameService.Winner}");

                    if (_gameService.Winner.HasValue)
                    {
                        _networkService.SendGameEnd(_gameService.Winner.Value);
                    }

                    var resultForm = DIContainer.Resolve<ResultForm>();

                    if (_gameService.Winner.HasValue)
                    {
                        bool playerWon = (_gameService.Winner == _myPlayerNumber);
                        resultForm.SetResult(playerWon);
                    }
                    else
                    {
                        resultForm.SetResult(null);
                    }

                    resultForm.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking game end: {ex.Message}", ex);
            }
        }

        private void CompleteButton_Click(object? sender, EventArgs e)
        {
            try
            {
                _networkService.Disconnect();
                ReturnToMainMenu();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CompleteButton_Click: {ex.Message}", ex);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _networkService.Disconnect();
                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnFormClosing: {ex.Message}", ex);
            }
        }
    }
}
