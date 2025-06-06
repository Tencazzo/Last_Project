using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Project3.Services
{
    public class NetworkService : INetworkService
    {
        private TcpListener? _server;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private volatile bool _isConnected;
        private volatile bool _isServer;
        private readonly ILogger _logger;
        private CancellationTokenSource? _cancellationTokenSource;
        private volatile bool _gameStarted = false;
        private readonly object _lockObject = new object();

        public event Action<int>? MoveReceived;
        public event Action<string>? MessageReceived;
        public event Action? PlayerConnected;
        public event Action? PlayerDisconnected;
        public event Action<int>? GameEnded; 

        public bool IsConnected => _isConnected;
        public bool IsServer => _isServer;

        public NetworkService(ILogger logger)
        {
            _logger = logger;
        }

        public bool StartServer(int port)
        {
            lock (_lockObject)
            {
                try
                {
                    if (_server != null || _isConnected)
                    {
                        _logger.LogWarning("Server already running or connection exists");
                        return false;
                    }

                    _cancellationTokenSource = new CancellationTokenSource();

                    _server = new TcpListener(IPAddress.Any, port);
                    _server.Start();
                    _isServer = true;
                    _gameStarted = false;

                    _logger.LogInfo($"Server started on port {port}");

                    Task.Run(async () => await AcceptClientsAsync(_cancellationTokenSource.Token));
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to start server: {ex.Message}", ex);
                    CleanupResources();
                    return false;
                }
            }
        }

        public bool ConnectToServer(string host, int port)
        {
            lock (_lockObject)
            {
                if (_isConnected || _client != null)
                {
                    _logger.LogWarning("Already connected or connection in progress");
                    return false;
                }

                const int maxRetries = 3;
                const int retryDelayMs = 1000;
                const int connectionTimeoutMs = 5000;

                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        _logger.LogInfo($"Connection attempt {attempt}/{maxRetries} to {host}:{port}");

                        _client = new TcpClient();
                        _client.ReceiveTimeout = 30000;
                        _client.SendTimeout = 10000;

                        var connectTask = _client.ConnectAsync(host, port);
                        if (!connectTask.Wait(connectionTimeoutMs))
                        {
                            _client.Close();
                            _client = null;
                            throw new TimeoutException("Connection timeout");
                        }

                        _stream = _client.GetStream();
                        _isConnected = true;
                        _isServer = false;
                        _gameStarted = false;

                        _logger.LogInfo($"Connected to server {host}:{port}");

                        _cancellationTokenSource = new CancellationTokenSource();

                        Task.Run(async () => await ReceiveMessagesAsync(_cancellationTokenSource.Token));

                        Thread.Sleep(500);

                        _logger.LogInfo("Client sending CLIENT_CONNECTED message");
                        SendMessage("CLIENT_CONNECTED");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Connection attempt {attempt} failed: {ex.Message}");

                        _client?.Close();
                        _client = null;
                        _stream = null;

                        if (attempt < maxRetries)
                        {
                            Thread.Sleep(retryDelayMs);
                        }
                    }
                }

                _logger.LogError($"Failed to connect to server {host}:{port} after {maxRetries} attempts");
                return false;
            }
        }

        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_server != null)
                {
                    _logger.LogInfo("Server waiting for client connection...");

                    _client = await _server.AcceptTcpClientAsync();

                    lock (_lockObject)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            _client?.Close();
                            return;
                        }

                        _stream = _client.GetStream();
                        _client.ReceiveTimeout = 30000;
                        _client.SendTimeout = 10000;
                        _isConnected = true;
                    }

                    _logger.LogInfo("Client connected to server, starting message receiving");

                    await ReceiveMessagesAsync(cancellationToken);
                }
            }
            catch (ObjectDisposedException)
            {
                _logger.LogInfo("Server was disposed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accepting client: {ex.Message}", ex);

                lock (_lockObject)
                {
                    if (_isConnected)
                    {
                        _isConnected = false;
                        Task.Run(() => PlayerDisconnected?.Invoke());
                    }
                }
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[1024];

            try
            {
                _logger.LogInfo("Starting message receiving loop");

                while (!cancellationToken.IsCancellationRequested)
                {
                    NetworkStream? currentStream;
                    lock (_lockObject)
                    {
                        if (!_isConnected || _stream == null)
                            break;
                        currentStream = _stream;
                    }

                    try
                    {
                        int bytesRead = await currentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        if (bytesRead == 0)
                        {
                            _logger.LogInfo("Connection closed by remote peer");
                            break;
                        }

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        _logger.LogInfo($"Received raw message: '{message}'");

                        ProcessMessage(message);
                    }
                    catch (Exception ex) when (!(ex is OperationCanceledException))
                    {
                        _logger.LogError($"Error reading message: {ex.Message}");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInfo("Message receiving was cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in message receiving loop: {ex.Message}", ex);
            }
            finally
            {
                _logger.LogInfo("Message receiving loop ended");

                lock (_lockObject)
                {
                    if (_isConnected)
                    {
                        _logger.LogInfo("Connection lost, invoking PlayerDisconnected");
                        _isConnected = false;
                        Task.Run(() => PlayerDisconnected?.Invoke());
                    }
                }
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                _logger.LogInfo($"Processing message: '{message}'");

                if (message.StartsWith("MOVE:"))
                {
                    if (int.TryParse(message.AsSpan(5), out int column))
                    {
                        _logger.LogInfo($"Processing move: {column}");
                        MoveReceived?.Invoke(column);
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid move format: {message}");
                    }
                }
                else if (message.StartsWith("GAME_END:"))
                {
                    if (int.TryParse(message.AsSpan(9), out int winner))
                    {
                        _logger.LogInfo($"Processing game end: winner {winner}");
                        GameEnded?.Invoke(winner);
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid game end format: {message}");
                    }
                }
                else
                {
                    _logger.LogInfo($"Processing control message: '{message}'");
                    ProcessControlMessage(message);
                    MessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message '{message}': {ex.Message}", ex);
            }
        }

        private void ProcessControlMessage(string msg)
        {
            try
            {
                if (msg == "CLIENT_CONNECTED")
                {
                    if (_isServer && !_gameStarted)
                    {
                        _logger.LogInfo("Server received CLIENT_CONNECTED, sending GAME_READY");

                        Thread.Sleep(100);

                        SendMessage("GAME_READY");

                        _gameStarted = true;

                        _logger.LogInfo("Server invoking PlayerConnected event");
                        Task.Run(() => PlayerConnected?.Invoke());
                    }
                    else
                    {
                        _logger.LogWarning($"Received CLIENT_CONNECTED but IsServer={_isServer}, GameStarted={_gameStarted}");
                    }
                }
                else if (msg == "GAME_READY")
                {
                    if (!_isServer && !_gameStarted)
                    {
                        _logger.LogInfo("Client received GAME_READY, invoking PlayerConnected");
                        _gameStarted = true;
                        Task.Run(() => PlayerConnected?.Invoke());
                    }
                    else
                    {
                        _logger.LogWarning($"Received GAME_READY but IsServer={_isServer}, GameStarted={_gameStarted}");
                    }
                }
                else
                {
                    _logger.LogInfo($"Unknown control message: '{msg}'");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing control message '{msg}': {ex.Message}", ex);
            }
        }

        public void SendMove(int column)
        {
            _logger.LogInfo($"Sending move: {column}");
            SendMessage($"MOVE:{column}");
        }

        public void SendGameEnd(int winner)
        {
            _logger.LogInfo($"Sending game end: winner {winner}");
            SendMessage($"GAME_END:{winner}");
        }

        public void SendMessage(string message)
        {
            lock (_lockObject)
            {
                if (!_isConnected || _stream == null)
                {
                    _logger.LogWarning($"Cannot send message '{message}' - not connected");
                    return;
                }

                try
                {
                    _logger.LogInfo($"Sending message: '{message}'");
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();

                    _logger.LogInfo($"Message sent successfully: '{message}'");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending message '{message}': {ex.Message}", ex);
                    _isConnected = false;
                    Task.Run(() => PlayerDisconnected?.Invoke());
                }
            }
        }

        public void Disconnect()
        {
            lock (_lockObject)
            {
                try
                {
                    _logger.LogInfo("Disconnecting network service");
                    _isConnected = false;
                    _gameStarted = false;

                    _cancellationTokenSource?.Cancel();

                    CleanupResources();

                    _logger.LogInfo("Network connection closed");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error disconnecting: {ex.Message}", ex);
                }
            }
        }

        private void CleanupResources()
        {
            try
            {
                _stream?.Close();
                _client?.Close();

                if (_server != null)
                {
                    _server.Stop();
                    _logger.LogInfo("Server stopped");
                }

                _stream = null;
                _client = null;
                _server = null;

                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cleaning up resources: {ex.Message}", ex);
            }
        }
    }
}
