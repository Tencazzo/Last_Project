using System;

namespace Project3.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int? WinnerId { get; set; }
        public string GameState { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
