﻿namespace SlackBinReminder
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
    }
}
