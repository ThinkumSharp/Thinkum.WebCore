/**
* LoggingService.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using Microsoft.Extensions.Logging;

namespace Thinkum.WebCore.Data
{
    public abstract class LoggingService : ILoggingService
    {
        protected readonly ILogger logger;
        public ILogger Logger => logger;

        public LoggingService(ILogger logger)
        {
            this.logger = logger;
        }

        public void LogDebug(string message, params object[]? args)
        {
            logger.LogDebug(message, args);
        }

        public void LogInfo(string message, params object[]? args)
        {
            logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[]? args)
        {
            logger.LogWarning(message, args);
        }

        public void LogError(string message, params object[]? args)
        {
            logger.LogError(message, args);
        }

        public void LogCritical(string message, params object[]? args)
        {
            logger.LogCritical(message, args);
        }

    }
}