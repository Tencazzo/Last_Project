using System;

namespace Project3.Services
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception? ex = null);
        void LogWarning(string message);
    }
}
