using System;
using System.Text;
using Project3.Models;

namespace Project3.Services
{
    public class GameService : IGameService
    {
        private const int ROWS = 6;
        private const int COLS = 7;
        private readonly int[,] _board;
        private int _currentPlayer;
        private bool _isGameOver;
        private int? _winner;
        private readonly ILogger _logger;

        public bool[,] GameBoard
        {
            get
            {
                var boolBoard = new bool[ROWS, COLS];
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLS; j++)
                    {
                        boolBoard[i, j] = _board[i, j] != 0;
                    }
                }
                return boolBoard;
            }
        }

        public int CurrentPlayer => _currentPlayer;
        public bool IsGameOver => _isGameOver;
        public int? Winner => _winner;

        public GameService(ILogger logger)
        {
            _logger = logger;
            _board = new int[ROWS, COLS];
            ResetGame();
        }

        public bool MakeMove(int column, bool isNetworkMove = false)
        {
            if (_isGameOver || column < 0 || column >= COLS)
                return false;

            for (int row = ROWS - 1; row >= 0; row--)
            {
                if (_board[row, column] == 0)
                {
                    int playerToMove = _currentPlayer;

                    _board[row, column] = playerToMove;
                    _logger.LogInfo($"Player {playerToMove} made move at column {column}, row {row} (isNetworkMove: {isNetworkMove}, currentPlayer: {_currentPlayer})");

                    if (CheckWin())
                    {
                        _isGameOver = true;
                        _winner = playerToMove;
                        _logger.LogInfo($"Player {playerToMove} won the game");
                    }
                    else if (IsBoardFull())
                    {
                        _isGameOver = true;
                        _winner = null; 
                        _logger.LogInfo("Game ended in a draw");
                    }
                    else
                    {
                        _currentPlayer = _currentPlayer == 1 ? 2 : 1;
                        _logger.LogInfo($"Current player switched to: {_currentPlayer}");
                    }

                    return true;
                }
            }

            return false; 
        }

        public bool CheckWin()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col <= COLS - 4; col++)
                {
                    if (_board[row, col] != 0 &&
                        _board[row, col] == _board[row, col + 1] &&
                        _board[row, col] == _board[row, col + 2] &&
                        _board[row, col] == _board[row, col + 3])
                    {
                        return true;
                    }
                }
            }

            for (int col = 0; col < COLS; col++)
            {
                for (int row = 0; row <= ROWS - 4; row++)
                {
                    if (_board[row, col] != 0 &&
                        _board[row, col] == _board[row + 1, col] &&
                        _board[row, col] == _board[row + 2, col] &&
                        _board[row, col] == _board[row + 3, col])
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row <= ROWS - 4; row++)
            {
                for (int col = 0; col <= COLS - 4; col++)
                {
                    if (_board[row, col] != 0 &&
                        _board[row, col] == _board[row + 1, col + 1] &&
                        _board[row, col] == _board[row + 2, col + 2] &&
                        _board[row, col] == _board[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row <= ROWS - 4; row++)
            {
                for (int col = 3; col < COLS; col++)
                {
                    if (_board[row, col] != 0 &&
                        _board[row, col] == _board[row + 1, col - 1] &&
                        _board[row, col] == _board[row + 2, col - 2] &&
                        _board[row, col] == _board[row + 3, col - 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int col = 0; col < COLS; col++)
            {
                if (_board[0, col] == 0)
                    return false;
            }
            return true;
        }

        public void ResetGame()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    _board[i, j] = 0;
                }
            }

            _currentPlayer = 1; 
            _isGameOver = false;
            _winner = null;
            _logger.LogInfo("Game reset - board cleared, player 1 starts");
        }

        public string GetGameState()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    sb.Append(_board[i, j]);
                    if (j < COLS - 1) sb.Append(',');
                }
                if (i < ROWS - 1) sb.Append(';');
            }
            sb.Append($"|{_currentPlayer}|{_isGameOver}|{_winner}");
            return sb.ToString();
        }

        public void LoadGameState(string gameState)
        {
            try
            {
                var parts = gameState.Split('|');
                var boardData = parts[0];
                _currentPlayer = int.Parse(parts[1]);
                _isGameOver = bool.Parse(parts[2]);
                _winner = parts[3] == "" ? null : int.Parse(parts[3]);

                var rows = boardData.Split(';');
                for (int i = 0; i < ROWS; i++)
                {
                    var cols = rows[i].Split(',');
                    for (int j = 0; j < COLS; j++)
                    {
                        _board[i, j] = int.Parse(cols[j]);
                    }
                }

                _logger.LogInfo("Game state loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to load game state", ex);
                ResetGame();
            }
        }

        public int GetCellValue(int row, int col)
        {
            if (row >= 0 && row < ROWS && col >= 0 && col < COLS)
                return _board[row, col];
            return 0;
        }
    }
}
