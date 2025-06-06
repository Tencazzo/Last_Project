namespace Project3.Services
{
    public interface IGameService
    {
        bool[,] GameBoard { get; }
        int CurrentPlayer { get; }
        bool IsGameOver { get; }
        int? Winner { get; }
        bool MakeMove(int column, bool isNetworkMove);
        bool CheckWin();
        void ResetGame();
        string GetGameState();
        void LoadGameState(string gameState);
        int GetCellValue(int row, int col);
    }
}
