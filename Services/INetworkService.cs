using System;

namespace Project3.Services
{
    public interface INetworkService
    {
        event Action<int>? MoveReceived;
        event Action<string>? MessageReceived;
        event Action? PlayerConnected;
        event Action? PlayerDisconnected;
        event Action<int>? GameEnded; 

        bool IsConnected { get; }
        bool IsServer { get; }

        bool StartServer(int port);
        bool ConnectToServer(string host, int port);
        void SendMove(int column);
        void SendMessage(string message);
        void SendGameEnd(int winner); 
        void Disconnect();
    }
}
